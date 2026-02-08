using TruweatherCore.Models.DTOs;

namespace TruweatherCore.Services.Interfaces;

/// <summary>
/// Interface for weather data and location management shared across API, Web, and Mobile.
/// API implements business logic; Web/Mobile use HttpClient to call API endpoints.
/// </summary>
public interface IWeatherService
{
    // Current Weather
    /// <summary>
    /// Get current weather for specified coordinates.
    /// </summary>
    Task<CurrentWeatherDto?> GetCurrentWeatherAsync(decimal latitude, decimal longitude);

    /// <summary>
    /// Get 7-day weather forecast for specified coordinates.
    /// </summary>
    Task<ForecastDto?> GetForecastAsync(decimal latitude, decimal longitude);

    // Saved Locations
    /// <summary>
    /// Get all saved locations for the current user.
    /// </summary>
    Task<List<SavedLocationDto>> GetSavedLocationsAsync(string userId);

    /// <summary>
    /// Add a new saved location for the current user.
    /// </summary>
    Task<SavedLocationDto?> AddSavedLocationAsync(string userId, CreateLocationRequest request);

    /// <summary>
    /// Update an existing saved location.
    /// </summary>
    Task<bool> UpdateSavedLocationAsync(string userId, int locationId, UpdateLocationRequest request);

    /// <summary>
    /// Delete a saved location.
    /// </summary>
    Task<bool> DeleteSavedLocationAsync(string userId, int locationId);

    // Weather Alerts
    /// <summary>
    /// Get all weather alerts for the current user.
    /// </summary>
    Task<List<WeatherAlertDto>> GetWeatherAlertsAsync(string userId);

    /// <summary>
    /// Create a new weather alert.
    /// </summary>
    Task<WeatherAlertDto?> CreateWeatherAlertAsync(string userId, CreateWeatherAlertRequest request);

    /// <summary>
    /// Update an existing weather alert.
    /// </summary>
    Task<bool> UpdateWeatherAlertAsync(string userId, int alertId, UpdateWeatherAlertRequest request);

    /// <summary>
    /// Delete a weather alert.
    /// </summary>
    Task<bool> DeleteWeatherAlertAsync(string userId, int alertId);
}
