using Microsoft.EntityFrameworkCore;
using TruweatherAPI.Data;
using TruweatherAPI.Models;
using TruweatherAPI.Services.OpenMeteo;
using TruweatherCore.Services.Interfaces;

namespace TruweatherAPI.Services;

/// <summary>
/// Background service that evaluates weather alerts every N minutes.
/// Fetches live weather data and compares against user-defined alert thresholds.
/// Creates notifications when conditions are met and resets when conditions clear.
/// </summary>
public class AlertEvaluationService(
    IServiceScopeFactory scopeFactory,
    IConfiguration configuration,
    ILogger<AlertEvaluationService> logger) : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
    private readonly ILogger<AlertEvaluationService> _logger = logger;
    private readonly int _intervalMinutes = configuration.GetValue<int>("AlertEvaluation:IntervalMinutes", 15);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("AlertEvaluationService started, interval: {Interval}min", _intervalMinutes);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await EvaluateAlertsAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during alert evaluation cycle");
            }

            await Task.Delay(TimeSpan.FromMinutes(_intervalMinutes), stoppingToken);
        }
    }

    public async Task EvaluateAlertsAsync(CancellationToken cancellationToken = default)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TruweatherDbContext>();
        var openMeteo = scope.ServiceProvider.GetRequiredService<OpenMeteoWeatherService>();
        var emailService = scope.ServiceProvider.GetService<IEmailService>();

        // Get all enabled alerts with their locations and user preferences
        var alerts = await context.WeatherAlerts
            .Include(a => a.SavedLocation)
            .Include(a => a.User)
                .ThenInclude(u => u.UserPreferences)
            .Where(a => a.IsEnabled)
            .ToListAsync(cancellationToken);

        if (alerts.Count == 0)
            return;

        // Group by location to minimize API calls
        var alertsByLocation = alerts.GroupBy(a => a.SavedLocationId);

        foreach (var group in alertsByLocation)
        {
            var location = group.First().SavedLocation;
            var weather = await openMeteo.GetCurrentWeatherAsync(
                location.Latitude, location.Longitude, location.LocationName);

            if (weather == null)
            {
                _logger.LogWarning("Could not fetch weather for location {Id} ({Name})",
                    location.Id, location.LocationName);
                continue;
            }

            foreach (var alert in group)
            {
                await EvaluateSingleAlertAsync(context, alert, weather, emailService);
            }
        }

        await context.SaveChangesAsync(cancellationToken);
    }

    private async Task EvaluateSingleAlertAsync(
        TruweatherDbContext context,
        WeatherAlert alert,
        TruweatherCore.Models.DTOs.CurrentWeatherDto weather,
        IEmailService? emailService)
    {
        // Skip if user has notifications disabled
        if (alert.User.UserPreferences?.EnableNotifications == false)
            return;

        var currentValue = GetWeatherValue(alert.AlertType, weather);
        if (currentValue == null)
            return;

        var isTriggered = EvaluateCondition(currentValue.Value, alert.Condition, alert.Threshold);

        if (isTriggered && !alert.IsNotified)
        {
            // Alert triggered - create notification
            alert.IsNotified = true;
            alert.LastTriggeredAt = DateTime.UtcNow;

            var notification = new Notification
            {
                UserId = alert.UserId,
                AlertId = alert.Id,
                Title = $"{alert.AlertType} Alert: {alert.SavedLocation.LocationName}",
                Message = $"{alert.AlertType} is {currentValue.Value:F1} " +
                          $"({alert.Condition} {alert.Threshold:F1}) at {alert.SavedLocation.LocationName}"
            };

            context.Notifications.Add(notification);
            _logger.LogInformation("Alert {AlertId} triggered for user {UserId}", alert.Id, alert.UserId);

            // Send email if enabled and rate-limited (max 1/hour per alert)
            if (emailService != null &&
                alert.User.UserPreferences?.EnableEmailAlerts == true &&
                (alert.LastTriggeredAt == null ||
                 DateTime.UtcNow - alert.LastTriggeredAt > TimeSpan.FromHours(1)))
            {
                try
                {
                    await emailService.SendAlertEmailAsync(
                        alert.User.Email ?? string.Empty,
                        notification.Title,
                        notification.Message);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to send alert email for alert {AlertId}", alert.Id);
                }
            }
        }
        else if (!isTriggered && alert.IsNotified)
        {
            // Condition cleared - reset for re-triggering
            alert.IsNotified = false;
            _logger.LogInformation("Alert {AlertId} cleared for user {UserId}", alert.Id, alert.UserId);
        }
    }

    public static decimal? GetWeatherValue(string alertType, TruweatherCore.Models.DTOs.CurrentWeatherDto weather)
    {
        return alertType.ToLowerInvariant() switch
        {
            "temperature" => weather.Temperature,
            "wind" or "windspeed" => weather.WindSpeed,
            "humidity" => weather.Humidity,
            "pressure" => weather.Pressure,
            "precipitation" => 0m, // Current weather doesn't include precipitation
            _ => null
        };
    }

    public static bool EvaluateCondition(decimal currentValue, string condition, decimal threshold)
    {
        return condition.ToLowerInvariant() switch
        {
            "greaterthan" or "above" or ">" => currentValue > threshold,
            "lessthan" or "below" or "<" => currentValue < threshold,
            "equal" or "equals" or "=" or "==" => Math.Abs(currentValue - threshold) < 0.01m,
            "greaterthanorequal" or ">=" => currentValue >= threshold,
            "lessthanorequal" or "<=" => currentValue <= threshold,
            _ => false
        };
    }
}
