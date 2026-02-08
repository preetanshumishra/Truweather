using TruweatherMobile.Services;

namespace TruweatherMobile;

public partial class App : Application
{
    private readonly AuthServiceClient _authService;

    public App(AuthServiceClient authService)
    {
        InitializeComponent();
        _authService = authService;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = new Window(new AppShell());

        window.Created += async (s, e) =>
        {
            var hasSession = await _authService.TryRestoreSessionAsync();
            if (hasSession)
            {
                await Shell.Current.GoToAsync("//dashboard");
            }
            else
            {
                await Shell.Current.GoToAsync("//login");
            }
        };

        return window;
    }
}
