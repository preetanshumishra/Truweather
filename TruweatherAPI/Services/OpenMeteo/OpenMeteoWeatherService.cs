using System.Net.Http.Json;
using TruweatherCore.Models.DTOs;

namespace TruweatherAPI.Services.OpenMeteo;

/// <summary>
/// Service for fetching real weather data from Open-Meteo API.
/// Open-Meteo is free, no API key required, unlimited requests.
/// API Reference: https://open-meteo.com/en/docs
/// </summary>
public class OpenMeteoWeatherService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://api.open-meteo.com/v1";

    // Current weather parameters
    private const string CurrentParams = "temperature_2m,relative_humidity_2m,weather_code,wind_speed_10m,wind_direction_10m,pressure";

    // Daily forecast parameters (7 days)
    private const string DailyParams = "temperature_2m_max,temperature_2m_min,temperature_2m_mean,weather_code,precipitation_sum,wind_speed_10m_max,wind_direction_10m_dominant";

    public OpenMeteoWeatherService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _httpClient.BaseAddress = new Uri(BaseUrl);
    }

    /// <summary>
    /// Get current weather for specified coordinates.
    /// </summary>
    public async Task<CurrentWeatherDto?> GetCurrentWeatherAsync(decimal latitude, decimal longitude, string? locationName = null)
    {
        try
        {
            var url = $"/forecast?latitude={latitude}&longitude={longitude}&current={CurrentParams}&timezone=auto";
            var response = await _httpClient.GetFromJsonAsync<OpenMeteoCurrentResponse>(url);

            if (response?.Current == null)
                return null;

            var (condition, description) = WeatherCodeMapper.GetWeatherDescription(response.Current.WeatherCode);
            var iconUrl = WeatherCodeMapper.GetIconUrl(response.Current.WeatherCode);

            return new CurrentWeatherDto(
                LocationName: locationName ?? $"{latitude}, {longitude}",
                Latitude: latitude,
                Longitude: longitude,
                Temperature: response.Current.Temperature2m,
                FeelsLike: response.Current.Temperature2m, // Open-Meteo doesn't provide feels-like, use temp
                Condition: condition,
                Description: description,
                Humidity: response.Current.RelativeHumidity2m,
                WindSpeed: response.Current.WindSpeed10m,
                WindDegree: response.Current.WindDirection10m,
                Pressure: response.Current.Pressure,
                Visibility: 10000, // Open-Meteo doesn't provide visibility
                IconUrl: iconUrl,
                RetrievedAt: DateTime.UtcNow
            );
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error fetching current weather: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Get 7-day weather forecast for specified coordinates.
    /// </summary>
    public async Task<ForecastDto?> GetForecastAsync(decimal latitude, decimal longitude, string? locationName = null)
    {
        try
        {
            var url = $"/forecast?latitude={latitude}&longitude={longitude}&daily={DailyParams}&timezone=auto";
            var response = await _httpClient.GetFromJsonAsync<OpenMeteoForecastResponse>(url);

            if (response?.Daily?.Time == null || response.Daily.Time.Length == 0)
                return null;

            var forecastDays = new List<ForecastDayDto>();

            // Process up to 7 days
            for (int i = 0; i < Math.Min(7, response.Daily.Time.Length); i++)
            {
                var (condition, description) = WeatherCodeMapper.GetWeatherDescription(response.Daily.WeatherCode[i]);

                forecastDays.Add(new ForecastDayDto(
                    Date: response.Daily.Time[i].Date,
                    MaxTemperature: response.Daily.Temperature2mMax[i],
                    MinTemperature: response.Daily.Temperature2mMin[i],
                    AvgTemperature: response.Daily.Temperature2mMean[i],
                    Condition: condition,
                    Description: description,
                    Humidity: 0, // Open-Meteo doesn't provide daily humidity in free tier
                    WindSpeed: response.Daily.WindSpeed10mMax[i],
                    Precipitation: response.Daily.PrecipitationSum[i],
                    IconUrl: WeatherCodeMapper.GetIconUrl(response.Daily.WeatherCode[i])
                ));
            }

            return new ForecastDto(
                LocationName: locationName ?? $"{latitude}, {longitude}",
                Latitude: latitude,
                Longitude: longitude,
                Days: forecastDays
            );
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error fetching forecast: {ex.Message}");
            return null;
        }
    }
}
