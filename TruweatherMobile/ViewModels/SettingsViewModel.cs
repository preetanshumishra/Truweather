using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TruweatherCore.Models.DTOs;
using TruweatherMobile.Services;

namespace TruweatherMobile.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    private readonly PreferencesServiceClient _preferencesService;
    private readonly AuthServiceClient _authService;

    public SettingsViewModel(PreferencesServiceClient preferencesService, AuthServiceClient authService)
    {
        _preferencesService = preferencesService;
        _authService = authService;
    }

    public List<string> TemperatureUnits { get; } = ["Celsius", "Fahrenheit", "Kelvin"];
    public List<string> WindSpeedUnits { get; } = ["ms", "kmh", "mph", "knots"];
    public List<string> Themes { get; } = ["Light", "Dark"];
    public List<string> Languages { get; } = ["en", "es", "fr", "de", "it", "pt", "ru", "zh", "ja", "ko"];

    [ObservableProperty]
    private int selectedTemperatureIndex;

    [ObservableProperty]
    private int selectedWindSpeedIndex;

    [ObservableProperty]
    private int selectedThemeIndex;

    [ObservableProperty]
    private int selectedLanguageIndex;

    [ObservableProperty]
    private bool enableNotifications;

    [ObservableProperty]
    private bool enableEmailAlerts;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private string? successMessage;

    [RelayCommand]
    private async Task LoadAsync()
    {
        try
        {
            IsBusy = true;
            ErrorMessage = null;

            var prefs = await _preferencesService.GetPreferencesAsync();

            SelectedTemperatureIndex = TemperatureUnits.IndexOf(prefs.TemperatureUnit);
            SelectedWindSpeedIndex = WindSpeedUnits.IndexOf(prefs.WindSpeedUnit);
            SelectedThemeIndex = Themes.IndexOf(prefs.Theme);
            SelectedLanguageIndex = Languages.IndexOf(prefs.Language);
            EnableNotifications = prefs.EnableNotifications;
            EnableEmailAlerts = prefs.EnableEmailAlerts;
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to load preferences: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        try
        {
            IsBusy = true;
            ErrorMessage = null;
            SuccessMessage = null;

            var request = new UpdatePreferencesRequest(
                TemperatureUnit: SelectedTemperatureIndex >= 0 ? TemperatureUnits[SelectedTemperatureIndex] : null,
                WindSpeedUnit: SelectedWindSpeedIndex >= 0 ? WindSpeedUnits[SelectedWindSpeedIndex] : null,
                Theme: SelectedThemeIndex >= 0 ? Themes[SelectedThemeIndex] : null,
                Language: SelectedLanguageIndex >= 0 ? Languages[SelectedLanguageIndex] : null,
                EnableNotifications: EnableNotifications,
                EnableEmailAlerts: EnableEmailAlerts
            );

            await _preferencesService.UpdatePreferencesAsync(request);
            SuccessMessage = "Preferences saved.";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to save preferences: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task LogoutAsync()
    {
        await _authService.LogoutAsync();
        await Shell.Current.GoToAsync("//login");
    }
}
