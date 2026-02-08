using TruweatherCore.Constants;
using TruweatherCore.Http;
using TruweatherCore.Models.DTOs;

namespace TruweatherMobile.Services;

public class WeatherServiceClient
{
    private readonly HttpClientWrapper _http;
    private readonly WeatherCacheService _cache;

    public WeatherServiceClient(HttpClientWrapper http, WeatherCacheService cache)
    {
        _http = http;
        _cache = cache;
    }

    public async Task<CurrentWeatherDto?> GetCurrentWeatherAsync(decimal latitude, decimal longitude)
    {
        // Try to get from cache first
        var cached = _cache.GetCachedCurrentWeather(latitude, longitude);
        if (cached != null)
        {
            return cached;
        }

        // Fetch from API
        var weather = await _http.GetAsync<CurrentWeatherDto>(
            $"{ApiEndpoints.WeatherCurrent}?latitude={latitude}&longitude={longitude}");

        // Cache the result
        if (weather != null)
        {
            await _cache.CacheCurrentWeatherAsync(latitude, longitude, weather);
        }

        return weather;
    }

    public async Task<ForecastDto?> GetForecastAsync(decimal latitude, decimal longitude)
    {
        // Try to get from cache first
        var cached = _cache.GetCachedForecast(latitude, longitude);
        if (cached != null)
        {
            return cached;
        }

        // Fetch from API
        var forecast = await _http.GetAsync<ForecastDto>(
            $"{ApiEndpoints.WeatherForecast}?latitude={latitude}&longitude={longitude}");

        // Cache the result
        if (forecast != null)
        {
            await _cache.CacheForecastAsync(latitude, longitude, forecast);
        }

        return forecast;
    }

    public Task<List<SavedLocationDto>> GetSavedLocationsAsync()
    {
        return _http.GetAsync<List<SavedLocationDto>>(ApiEndpoints.WeatherLocations);
    }

    public Task<SavedLocationDto> AddSavedLocationAsync(CreateLocationRequest request)
    {
        return _http.PostAsync<SavedLocationDto>(ApiEndpoints.WeatherLocations, request);
    }

    public Task<SavedLocationDto> UpdateSavedLocationAsync(int id, UpdateLocationRequest request)
    {
        return _http.PutAsync<SavedLocationDto>(ApiEndpoints.WeatherLocationDetails(id), request);
    }

    public Task<bool> DeleteSavedLocationAsync(int id)
    {
        return _http.DeleteAsync(ApiEndpoints.WeatherLocationDetails(id));
    }

    public Task<List<WeatherAlertDto>> GetWeatherAlertsAsync()
    {
        return _http.GetAsync<List<WeatherAlertDto>>(ApiEndpoints.WeatherAlerts);
    }

    public Task<WeatherAlertDto> CreateWeatherAlertAsync(CreateWeatherAlertRequest request)
    {
        return _http.PostAsync<WeatherAlertDto>(ApiEndpoints.WeatherAlerts, request);
    }

    public Task<WeatherAlertDto> UpdateWeatherAlertAsync(int id, UpdateWeatherAlertRequest request)
    {
        return _http.PutAsync<WeatherAlertDto>(ApiEndpoints.WeatherAlertDetails(id), request);
    }

    public Task<bool> DeleteWeatherAlertAsync(int id)
    {
        return _http.DeleteAsync(ApiEndpoints.WeatherAlertDetails(id));
    }
}
