using TruweatherAPI.DTOs;

namespace TruweatherAPI.Services;

public interface IWeatherService
{
    Task<CurrentWeatherDto?> GetCurrentWeatherAsync(decimal latitude, decimal longitude);
    Task<ForecastDto?> GetForecastAsync(decimal latitude, decimal longitude);
    Task<List<SavedLocationDto>> GetSavedLocationsAsync(string userId);
    Task<SavedLocationDto?> AddSavedLocationAsync(string userId, CreateLocationRequest request);
    Task<bool> UpdateSavedLocationAsync(string userId, int locationId, UpdateLocationRequest request);
    Task<bool> DeleteSavedLocationAsync(string userId, int locationId);
    Task<List<WeatherAlertDto>> GetWeatherAlertsAsync(string userId);
    Task<WeatherAlertDto?> CreateWeatherAlertAsync(string userId, CreateWeatherAlertRequest request);
    Task<bool> UpdateWeatherAlertAsync(string userId, int alertId, UpdateWeatherAlertRequest request);
    Task<bool> DeleteWeatherAlertAsync(string userId, int alertId);
}
