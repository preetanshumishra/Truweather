using Microsoft.EntityFrameworkCore;
using TruweatherAPI.Data;
using TruweatherAPI.DTOs;
using TruweatherAPI.Models;

namespace TruweatherAPI.Services;

public class WeatherService(TruweatherDbContext context) : IWeatherService
{
    private readonly TruweatherDbContext _context = context;

    public async Task<CurrentWeatherDto?> GetCurrentWeatherAsync(decimal latitude, decimal longitude)
    {
        // TODO: Integrate with OpenWeatherMap API
        // For now, return mock data
        return await Task.FromResult(new CurrentWeatherDto(
            LocationName: "Mock Location",
            Latitude: latitude,
            Longitude: longitude,
            Temperature: 22.5m,
            FeelsLike: 21.0m,
            Condition: "Partly Cloudy",
            Description: "Partly cloudy sky",
            Humidity: 65,
            WindSpeed: 5.5m,
            WindDegree: 180,
            Pressure: 1013m,
            Visibility: 10000,
            IconUrl: "https://via.placeholder.com/150",
            RetrievedAt: DateTime.UtcNow
        ));
    }

    public async Task<ForecastDto?> GetForecastAsync(decimal latitude, decimal longitude)
    {
        // TODO: Integrate with OpenWeatherMap API
        // For now, return mock data
        var days = Enumerable.Range(0, 7)
            .Select(i => new ForecastDayDto(
                Date: DateTime.UtcNow.AddDays(i).Date,
                MaxTemperature: 25.0m + i,
                MinTemperature: 15.0m + i,
                AvgTemperature: 20.0m + i,
                Condition: "Partly Cloudy",
                Description: "Partly cloudy sky",
                Humidity: 65,
                WindSpeed: 5.5m,
                Precipitation: 0.0m,
                IconUrl: "https://via.placeholder.com/150"
            ))
            .ToList();

        return await Task.FromResult(new ForecastDto(
            LocationName: "Mock Location",
            Latitude: latitude,
            Longitude: longitude,
            Days: days
        ));
    }

    public async Task<List<SavedLocationDto>> GetSavedLocationsAsync(string userId)
    {
        var locations = await _context.SavedLocations
            .Where(l => l.UserId == userId)
            .Select(l => new SavedLocationDto(
                l.Id,
                l.LocationName,
                l.Latitude,
                l.Longitude,
                l.IsDefault
            ))
            .ToListAsync();

        return locations;
    }

    public async Task<SavedLocationDto?> AddSavedLocationAsync(string userId, CreateLocationRequest request)
    {
        var location = new SavedLocation
        {
            UserId = userId,
            LocationName = request.LocationName,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            IsDefault = request.IsDefault
        };

        _context.SavedLocations.Add(location);
        await _context.SaveChangesAsync();

        return new SavedLocationDto(
            Id: location.Id,
            LocationName: location.LocationName,
            Latitude: location.Latitude,
            Longitude: location.Longitude,
            IsDefault: location.IsDefault
        );
    }

    public async Task<bool> UpdateSavedLocationAsync(string userId, int locationId, UpdateLocationRequest request)
    {
        var location = await _context.SavedLocations
            .FirstOrDefaultAsync(l => l.Id == locationId && l.UserId == userId);

        if (location == null)
            return false;

        location.LocationName = request.LocationName;
        location.IsDefault = request.IsDefault;
        location.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteSavedLocationAsync(string userId, int locationId)
    {
        var location = await _context.SavedLocations
            .FirstOrDefaultAsync(l => l.Id == locationId && l.UserId == userId);

        if (location == null)
            return false;

        _context.SavedLocations.Remove(location);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<WeatherAlertDto>> GetWeatherAlertsAsync(string userId)
    {
        var alerts = await _context.WeatherAlerts
            .Where(a => a.UserId == userId)
            .Select(a => new WeatherAlertDto(
                a.Id,
                a.SavedLocationId,
                a.AlertType,
                a.Condition,
                a.Threshold,
                a.IsEnabled,
                a.CreatedAt
            ))
            .ToListAsync();

        return alerts;
    }

    public async Task<WeatherAlertDto?> CreateWeatherAlertAsync(string userId, CreateWeatherAlertRequest request)
    {
        var alert = new WeatherAlert
        {
            UserId = userId,
            SavedLocationId = request.SavedLocationId,
            AlertType = request.AlertType,
            Condition = request.Condition,
            Threshold = request.Threshold
        };

        _context.WeatherAlerts.Add(alert);
        await _context.SaveChangesAsync();

        return new WeatherAlertDto(
            Id: alert.Id,
            SavedLocationId: alert.SavedLocationId,
            AlertType: alert.AlertType,
            Condition: alert.Condition,
            Threshold: alert.Threshold,
            IsEnabled: alert.IsEnabled,
            CreatedAt: alert.CreatedAt
        );
    }

    public async Task<bool> UpdateWeatherAlertAsync(string userId, int alertId, UpdateWeatherAlertRequest request)
    {
        var alert = await _context.WeatherAlerts
            .FirstOrDefaultAsync(a => a.Id == alertId && a.UserId == userId);

        if (alert == null)
            return false;

        alert.AlertType = request.AlertType;
        alert.Condition = request.Condition;
        alert.Threshold = request.Threshold;
        alert.IsEnabled = request.IsEnabled;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteWeatherAlertAsync(string userId, int alertId)
    {
        var alert = await _context.WeatherAlerts
            .FirstOrDefaultAsync(a => a.Id == alertId && a.UserId == userId);

        if (alert == null)
            return false;

        _context.WeatherAlerts.Remove(alert);
        await _context.SaveChangesAsync();
        return true;
    }
}
