using System.Diagnostics;
using MonkeyCache;
using MonkeyCache.FileStore;
using TruweatherCore.Models.DTOs;

namespace TruweatherMobile.Services;

/// <summary>
/// Service for offline caching of weather data using MonkeyCache.FileStore.
/// Enables app to function without network connection using previously cached data.
/// </summary>
public class WeatherCacheService
{
    private const int CurrentWeatherCacheTtlMinutes = 60;
    private const int ForecastCacheTtlMinutes = 60;
    private const string CurrentWeatherCacheKeyPrefix = "weather_current_";
    private const string ForecastCacheKeyPrefix = "weather_forecast_";

    /// <summary>
    /// Initialize MonkeyCache with FileStore backend on first use.
    /// </summary>
    public WeatherCacheService()
    {
        // Initialize Barrel (MonkeyCache) with FileStore
        try
        {
            Barrel.ApplicationId = "truweather_cache";
            // Force FileStore initialization
            var _ = Barrel.Current;
            Debug.WriteLine("MonkeyCache FileStore initialized successfully");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error initializing MonkeyCache: {ex.Message}");
        }
    }

    /// <summary>
    /// Cache current weather data with 60-minute TTL.
    /// </summary>
    public async Task CacheCurrentWeatherAsync(decimal latitude, decimal longitude, CurrentWeatherDto weather)
    {
        try
        {
            var cacheKey = GetCurrentWeatherCacheKey(latitude, longitude);
            await Task.Run(() =>
            {
                Barrel.Current.Add(cacheKey, weather, TimeSpan.FromMinutes(CurrentWeatherCacheTtlMinutes));
            });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error caching current weather: {ex.Message}");
            // Silently fail - caching error shouldn't break the app
        }
    }

    /// <summary>
    /// Retrieve cached current weather if available and not expired.
    /// </summary>
    public CurrentWeatherDto? GetCachedCurrentWeather(decimal latitude, decimal longitude)
    {
        try
        {
            var cacheKey = GetCurrentWeatherCacheKey(latitude, longitude);
            if (Barrel.Current.Exists(cacheKey))
            {
                return Barrel.Current.Get<CurrentWeatherDto>(cacheKey);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error retrieving cached current weather: {ex.Message}");
        }
        return null;
    }

    /// <summary>
    /// Cache 7-day forecast with 60-minute TTL.
    /// </summary>
    public async Task CacheForecastAsync(decimal latitude, decimal longitude, ForecastDto forecast)
    {
        try
        {
            var cacheKey = GetForecastCacheKey(latitude, longitude);
            await Task.Run(() =>
            {
                Barrel.Current.Add(cacheKey, forecast, TimeSpan.FromMinutes(ForecastCacheTtlMinutes));
            });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error caching forecast: {ex.Message}");
        }
    }

    /// <summary>
    /// Retrieve cached forecast if available and not expired.
    /// </summary>
    public ForecastDto? GetCachedForecast(decimal latitude, decimal longitude)
    {
        try
        {
            var cacheKey = GetForecastCacheKey(latitude, longitude);
            if (Barrel.Current.Exists(cacheKey))
            {
                return Barrel.Current.Get<ForecastDto>(cacheKey);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error retrieving cached forecast: {ex.Message}");
        }
        return null;
    }

    /// <summary>
    /// Check if current weather is cached and not expired.
    /// </summary>
    public bool HasCachedCurrentWeather(decimal latitude, decimal longitude)
    {
        try
        {
            var cacheKey = GetCurrentWeatherCacheKey(latitude, longitude);
            return Barrel.Current.Exists(cacheKey);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error checking cached current weather: {ex.Message}");
        }
        return false;
    }

    /// <summary>
    /// Check if forecast is cached and not expired.
    /// </summary>
    public bool HasCachedForecast(decimal latitude, decimal longitude)
    {
        try
        {
            var cacheKey = GetForecastCacheKey(latitude, longitude);
            return Barrel.Current.Exists(cacheKey);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error checking cached forecast: {ex.Message}");
        }
        return false;
    }

    /// <summary>
    /// Get the last cached timestamp for weather data (or null if not cached).
    /// </summary>
    public DateTime? GetCachedWeatherTimestamp(decimal latitude, decimal longitude)
    {
        try
        {
            var weather = GetCachedCurrentWeather(latitude, longitude);
            return weather?.RetrievedAt;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error getting cached weather timestamp: {ex.Message}");
        }
        return null;
    }

    /// <summary>
    /// Clear all cached weather data from disk.
    /// </summary>
    public async Task ClearAllCacheAsync()
    {
        try
        {
            await Task.Run(() =>
            {
                Barrel.Current.EmptyAll();
            });
            Debug.WriteLine("Weather cache cleared successfully");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error clearing cache: {ex.Message}");
        }
    }

    /// <summary>
    /// Clear cache for specific location.
    /// </summary>
    public async Task ClearLocationCacheAsync(decimal latitude, decimal longitude)
    {
        try
        {
            var currentWeatherKey = GetCurrentWeatherCacheKey(latitude, longitude);
            var forecastKey = GetForecastCacheKey(latitude, longitude);

            await Task.Run(() =>
            {
                if (Barrel.Current.Exists(currentWeatherKey))
                    Barrel.Current.Empty(currentWeatherKey);
                if (Barrel.Current.Exists(forecastKey))
                    Barrel.Current.Empty(forecastKey);
            });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error clearing location cache: {ex.Message}");
        }
    }

    /// <summary>
    /// Get total size of cached data (approximate).
    /// </summary>
    public string GetCacheSize()
    {
        try
        {
            var cacheDir = FileSystem.CacheDirectory;
            var barrelDir = Path.Combine(cacheDir, "barrel");

            if (!Directory.Exists(barrelDir))
                return "0 KB";

            long totalSize = 0;
            var files = Directory.GetFiles(barrelDir);
            foreach (var file in files)
            {
                totalSize += new FileInfo(file).Length;
            }

            return FormatBytes(totalSize);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error getting cache size: {ex.Message}");
            return "Unknown";
        }
    }

    /// <summary>
    /// Generate cache key for current weather.
    /// </summary>
    private string GetCurrentWeatherCacheKey(decimal latitude, decimal longitude)
    {
        return $"{CurrentWeatherCacheKeyPrefix}{latitude}_{longitude}";
    }

    /// <summary>
    /// Generate cache key for forecast.
    /// </summary>
    private string GetForecastCacheKey(decimal latitude, decimal longitude)
    {
        return $"{ForecastCacheKeyPrefix}{latitude}_{longitude}";
    }

    /// <summary>
    /// Format bytes to human-readable size.
    /// </summary>
    private string FormatBytes(long bytes)
    {
        if (bytes < 1024)
            return $"{bytes} B";
        if (bytes < 1024 * 1024)
            return $"{bytes / 1024.0:F1} KB";
        return $"{bytes / (1024.0 * 1024):F1} MB";
    }
}
