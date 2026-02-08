using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TruweatherCore.Models.DTOs;
using TruweatherMobile.Services;

namespace TruweatherMobile.ViewModels;

public partial class DashboardViewModel : ObservableObject
{
    private readonly WeatherServiceClient _weatherService;
    private readonly WeatherCacheService _cacheService;

    public DashboardViewModel(WeatherServiceClient weatherService, WeatherCacheService cacheService)
    {
        _weatherService = weatherService;
        _cacheService = cacheService;
    }

    [ObservableProperty]
    private CurrentWeatherDto? currentWeather;

    [ObservableProperty]
    private ForecastDto? forecast;

    [ObservableProperty]
    private ObservableCollection<SavedLocationDto> locations = [];

    [ObservableProperty]
    private SavedLocationDto? selectedLocation;

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private bool isRefreshing;

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private string? cacheStatus;

    [ObservableProperty]
    private bool isDataFromCache;

    [RelayCommand]
    private async Task LoadDataAsync()
    {
        if (IsLoading) return;

        try
        {
            IsLoading = true;
            ErrorMessage = null;

            var locationList = await _weatherService.GetSavedLocationsAsync();
            Locations = new ObservableCollection<SavedLocationDto>(locationList);

            var defaultLocation = locationList.FirstOrDefault(l => l.IsDefault) ?? locationList.FirstOrDefault();
            SelectedLocation = defaultLocation;

            if (defaultLocation != null)
            {
                await LoadWeatherForLocationAsync(defaultLocation);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to load data: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        try
        {
            ErrorMessage = null;

            if (SelectedLocation != null)
            {
                await LoadWeatherForLocationAsync(SelectedLocation);
            }
            else
            {
                await LoadDataAsync();
            }
        }
        finally
        {
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    private async Task SelectLocationAsync(SavedLocationDto location)
    {
        SelectedLocation = location;
        await LoadWeatherForLocationAsync(location);
    }

    private async Task LoadWeatherForLocationAsync(SavedLocationDto location)
    {
        try
        {
            // Check if data is from cache
            var isCached = _cacheService.HasCachedCurrentWeather(location.Latitude, location.Longitude);
            IsDataFromCache = isCached;

            var weatherTask = _weatherService.GetCurrentWeatherAsync(location.Latitude, location.Longitude);
            var forecastTask = _weatherService.GetForecastAsync(location.Latitude, location.Longitude);

            await Task.WhenAll(weatherTask, forecastTask);

            CurrentWeather = weatherTask.Result;
            Forecast = forecastTask.Result;

            // Update cache status
            UpdateCacheStatus(location);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to load weather: {ex.Message}";
        }
    }

    private void UpdateCacheStatus(SavedLocationDto location)
    {
        var cachedTime = _cacheService.GetCachedWeatherTimestamp(location.Latitude, location.Longitude);
        if (cachedTime.HasValue)
        {
            var timeAgo = DateTime.UtcNow - cachedTime.Value;
            var timeText = timeAgo.TotalMinutes < 1
                ? "Just now"
                : timeAgo.TotalHours < 1
                    ? $"{(int)timeAgo.TotalMinutes}m ago"
                    : $"{(int)timeAgo.TotalHours}h ago";

            CacheStatus = IsDataFromCache
                ? $"Cached • {timeText}"
                : $"Updated • {timeText}";
        }
        else
        {
            CacheStatus = "Fresh data";
        }
    }
}
