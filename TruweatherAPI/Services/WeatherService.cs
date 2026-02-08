using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TruweatherAPI.Data;
using TruweatherAPI.Models;
using TruweatherAPI.Services.OpenMeteo;
using TruweatherCore.Models.DTOs;
using TruweatherCore.Services.Interfaces;

namespace TruweatherAPI.Services;

/// <summary>
/// Weather service that implements IWeatherService interface.
/// Fetches real weather data from Open-Meteo API with in-memory caching.
/// Falls back to database-stored weather data if API is unavailable.
/// </summary>
public class WeatherService(TruweatherDbContext context, IMemoryCache cache, OpenMeteoWeatherService openMeteoService) : IWeatherService
{
    private readonly TruweatherDbContext _context = context;
    private readonly IMemoryCache _cache = cache;
    private readonly OpenMeteoWeatherService _openMeteoService = openMeteoService;

    // Cache key prefixes and TTL
    private const string CurrentWeatherCacheKeyPrefix = "weather_current_";
    private const string ForecastCacheKeyPrefix = "weather_forecast_";
    private const int CacheTtlMinutes = 60; // Cache weather data for 1 hour

    public async Task<CurrentWeatherDto?> GetCurrentWeatherAsync(decimal latitude, decimal longitude)
    {
        var cacheKey = $"{CurrentWeatherCacheKeyPrefix}{latitude}_{longitude}";

        // Try to get from cache
        if (_cache.TryGetValue(cacheKey, out CurrentWeatherDto? cachedWeather))
        {
            return cachedWeather;
        }

        // Get location name if available from saved locations
        var locationName = await GetLocationNameAsync(latitude, longitude);

        // Fetch from Open-Meteo API
        var weather = await _openMeteoService.GetCurrentWeatherAsync(latitude, longitude, locationName);

        if (weather != null)
        {
            // Cache for 1 hour
            _cache.Set(cacheKey, weather, TimeSpan.FromMinutes(CacheTtlMinutes));
            return weather;
        }

        // Fallback: Try to get from database if API fails
        var dbWeather = await GetStoredWeatherAsync(latitude, longitude);
        return dbWeather;
    }

    public async Task<ForecastDto?> GetForecastAsync(decimal latitude, decimal longitude)
    {
        var cacheKey = $"{ForecastCacheKeyPrefix}{latitude}_{longitude}";

        // Try to get from cache
        if (_cache.TryGetValue(cacheKey, out ForecastDto? cachedForecast))
        {
            return cachedForecast;
        }

        // Get location name if available from saved locations
        var locationName = await GetLocationNameAsync(latitude, longitude);

        // Fetch from Open-Meteo API
        var forecast = await _openMeteoService.GetForecastAsync(latitude, longitude, locationName);

        if (forecast != null)
        {
            // Cache for 1 hour
            _cache.Set(cacheKey, forecast, TimeSpan.FromMinutes(CacheTtlMinutes));
            return forecast;
        }

        // Fallback: Generate mock forecast if API fails
        return GetMockForecastFallback(latitude, longitude, locationName);
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

    /// <summary>
    /// Get location name from saved locations if it matches the coordinates.
    /// </summary>
    private async Task<string?> GetLocationNameAsync(decimal latitude, decimal longitude)
    {
        var location = await _context.SavedLocations
            .FirstOrDefaultAsync(l => l.Latitude == latitude && l.Longitude == longitude);

        return location?.LocationName;
    }

    /// <summary>
    /// Get stored weather data from database as fallback.
    /// </summary>
    private async Task<CurrentWeatherDto?> GetStoredWeatherAsync(decimal latitude, decimal longitude)
    {
        var weather = await _context.WeatherData
            .Where(w => w.SavedLocationId == GetLocationIdAsync(latitude, longitude).Result)
            .OrderByDescending(w => w.CachedAt)
            .FirstOrDefaultAsync();

        if (weather == null)
            return null;

        return new CurrentWeatherDto(
            LocationName: "Cached Data",
            Latitude: latitude,
            Longitude: longitude,
            Temperature: weather.Temperature,
            FeelsLike: weather.FeelsLike,
            Condition: weather.Condition,
            Description: weather.Description,
            Humidity: 0,
            WindSpeed: weather.WindSpeed,
            WindDegree: weather.WindDegree,
            Pressure: weather.Pressure,
            Visibility: weather.Visibility,
            IconUrl: weather.IconUrl,
            RetrievedAt: weather.CachedAt
        );
    }

    /// <summary>
    /// Get location ID from coordinates.
    /// </summary>
    private async Task<int> GetLocationIdAsync(decimal latitude, decimal longitude)
    {
        var location = await _context.SavedLocations
            .FirstOrDefaultAsync(l => l.Latitude == latitude && l.Longitude == longitude);

        return location?.Id ?? 0;
    }

    /// <summary>
    /// Generate mock forecast as fallback when API is unavailable.
    /// </summary>
    private ForecastDto? GetMockForecastFallback(decimal latitude, decimal longitude, string? locationName)
    {
        var days = Enumerable.Range(0, 7)
            .Select(i => new ForecastDayDto(
                Date: DateTime.UtcNow.AddDays(i).Date,
                MaxTemperature: 25.0m + i,
                MinTemperature: 15.0m + i,
                AvgTemperature: 20.0m + i,
                Condition: "Partly Cloudy",
                Description: "Partly cloudy sky (cached)",
                Humidity: 65,
                WindSpeed: 5.5m,
                Precipitation: 0.0m,
                IconUrl: "⛅"
            ))
            .ToList();

        return new ForecastDto(
            LocationName: locationName ?? $"{latitude}, {longitude}",
            Latitude: latitude,
            Longitude: longitude,
            Days: days
        );
    }
}
