using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TruweatherCore.Models.DTOs;
using TruweatherMobile.Services;

namespace TruweatherMobile.ViewModels;

public partial class DashboardViewModel : ObservableObject
{
    private readonly WeatherServiceClient _weatherService;

    public DashboardViewModel(WeatherServiceClient weatherService)
    {
        _weatherService = weatherService;
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
            var weatherTask = _weatherService.GetCurrentWeatherAsync(location.Latitude, location.Longitude);
            var forecastTask = _weatherService.GetForecastAsync(location.Latitude, location.Longitude);

            await Task.WhenAll(weatherTask, forecastTask);

            CurrentWeather = weatherTask.Result;
            Forecast = forecastTask.Result;
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to load weather: {ex.Message}";
        }
    }
}
