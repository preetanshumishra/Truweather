using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TruweatherMobile.Services;

namespace TruweatherMobile.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly AuthServiceClient _authService;

    public LoginViewModel(AuthServiceClient authService)
    {
        _authService = authService;
    }

    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private bool isBusy;

    [RelayCommand]
    private async Task LoginAsync()
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Email and password are required.";
            return;
        }

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            var response = await _authService.LoginAsync(Email, Password);

            if (response.Success)
            {
                await Shell.Current.GoToAsync("//dashboard");
            }
            else
            {
                ErrorMessage = response.Message;
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Login failed: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task GoToRegisterAsync()
    {
        await Shell.Current.GoToAsync("//register");
    }
}
