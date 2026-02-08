using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TruweatherCore.Models.DTOs;
using TruweatherMobile.Services;

namespace TruweatherMobile.ViewModels;

public partial class AlertsViewModel : ObservableObject
{
    private readonly WeatherServiceClient _weatherService;

    public AlertsViewModel(WeatherServiceClient weatherService)
    {
        _weatherService = weatherService;
    }

    [ObservableProperty]
    private ObservableCollection<WeatherAlertDto> alerts = [];

    [ObservableProperty]
    private ObservableCollection<SavedLocationDto> locations = [];

    [ObservableProperty]
    private SavedLocationDto? selectedLocation;

    [ObservableProperty]
    private string alertType = string.Empty;

    [ObservableProperty]
    private string condition = string.Empty;

    [ObservableProperty]
    private string threshold = string.Empty;

    [ObservableProperty]
    private bool isAddFormVisible;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    [RelayCommand]
    private async Task LoadAsync()
    {
        try
        {
            IsBusy = true;
            ErrorMessage = null;

            var alertsTask = _weatherService.GetWeatherAlertsAsync();
            var locationsTask = _weatherService.GetSavedLocationsAsync();

            await Task.WhenAll(alertsTask, locationsTask);

            Alerts = new ObservableCollection<WeatherAlertDto>(alertsTask.Result);
            Locations = new ObservableCollection<SavedLocationDto>(locationsTask.Result);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to load alerts: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task CreateAsync()
    {
        if (SelectedLocation == null)
        {
            ErrorMessage = "Please select a location.";
            return;
        }

        if (string.IsNullOrWhiteSpace(AlertType) || string.IsNullOrWhiteSpace(Condition))
        {
            ErrorMessage = "Alert type and condition are required.";
            return;
        }

        if (!decimal.TryParse(Threshold, out var thresholdValue))
        {
            ErrorMessage = "Threshold must be a number.";
            return;
        }

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            var request = new CreateWeatherAlertRequest(
                SelectedLocation.Id, AlertType, Condition, thresholdValue);
            await _weatherService.CreateWeatherAlertAsync(request);

            AlertType = string.Empty;
            Condition = string.Empty;
            Threshold = string.Empty;
            SelectedLocation = null;
            IsAddFormVisible = false;

            await LoadAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to create alert: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task DeleteAsync(WeatherAlertDto alert)
    {
        try
        {
            IsBusy = true;
            ErrorMessage = null;

            await _weatherService.DeleteWeatherAlertAsync(alert.Id);
            await LoadAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to delete alert: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task ToggleEnabledAsync(WeatherAlertDto alert)
    {
        try
        {
            IsBusy = true;
            ErrorMessage = null;

            var request = new UpdateWeatherAlertRequest(
                alert.AlertType, alert.Condition, alert.Threshold, !alert.IsEnabled);
            await _weatherService.UpdateWeatherAlertAsync(alert.Id, request);
            await LoadAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to update alert: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void ToggleAddForm()
    {
        IsAddFormVisible = !IsAddFormVisible;
        ErrorMessage = null;
    }
}
