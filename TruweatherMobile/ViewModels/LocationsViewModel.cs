using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TruweatherCore.Models.DTOs;
using TruweatherMobile.Services;

namespace TruweatherMobile.ViewModels;

public partial class LocationsViewModel : ObservableObject
{
    private readonly WeatherServiceClient _weatherService;

    public LocationsViewModel(WeatherServiceClient weatherService)
    {
        _weatherService = weatherService;
    }

    [ObservableProperty]
    private ObservableCollection<SavedLocationDto> locations = [];

    [ObservableProperty]
    private string newLocationName = string.Empty;

    [ObservableProperty]
    private string newLatitude = string.Empty;

    [ObservableProperty]
    private string newLongitude = string.Empty;

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

            var list = await _weatherService.GetSavedLocationsAsync();
            Locations = new ObservableCollection<SavedLocationDto>(list);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to load locations: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task AddAsync()
    {
        if (string.IsNullOrWhiteSpace(NewLocationName))
        {
            ErrorMessage = "Location name is required.";
            return;
        }

        if (!decimal.TryParse(NewLatitude, out var lat) || lat < -90 || lat > 90)
        {
            ErrorMessage = "Latitude must be between -90 and 90.";
            return;
        }

        if (!decimal.TryParse(NewLongitude, out var lon) || lon < -180 || lon > 180)
        {
            ErrorMessage = "Longitude must be between -180 and 180.";
            return;
        }

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            var request = new CreateLocationRequest(NewLocationName, lat, lon);
            await _weatherService.AddSavedLocationAsync(request);

            NewLocationName = string.Empty;
            NewLatitude = string.Empty;
            NewLongitude = string.Empty;
            IsAddFormVisible = false;

            await LoadAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to add location: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task DeleteAsync(SavedLocationDto location)
    {
        try
        {
            IsBusy = true;
            ErrorMessage = null;

            await _weatherService.DeleteSavedLocationAsync(location.Id);
            await LoadAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to delete location: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task SetDefaultAsync(SavedLocationDto location)
    {
        try
        {
            IsBusy = true;
            ErrorMessage = null;

            var request = new UpdateLocationRequest(location.LocationName, true);
            await _weatherService.UpdateSavedLocationAsync(location.Id, request);
            await LoadAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to set default: {ex.Message}";
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
