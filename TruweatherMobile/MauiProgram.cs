using TruweatherCore.Http;
using TruweatherMobile.Pages;
using TruweatherMobile.Services;
using TruweatherMobile.ViewModels;

namespace TruweatherMobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Services
        builder.Services.AddSingleton<ITokenStorage, SecureTokenStorage>();
        builder.Services.AddSingleton(sp =>
        {
            var client = new HttpClient();
            return client;
        });
        builder.Services.AddSingleton(sp =>
        {
            var httpClient = sp.GetRequiredService<HttpClient>();
#if DEBUG
            const string apiBaseUrl = "http://localhost:5000";
#else
            const string apiBaseUrl = "https://api.truweather.com";
#endif
            return new HttpClientWrapper(httpClient, apiBaseUrl);
        });
        builder.Services.AddSingleton<AuthServiceClient>();
        builder.Services.AddSingleton<WeatherServiceClient>();
        builder.Services.AddSingleton<PreferencesServiceClient>();
        builder.Services.AddSingleton<WeatherCacheService>();

        // ViewModels
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<RegisterViewModel>();
        builder.Services.AddTransient<DashboardViewModel>();
        builder.Services.AddTransient<LocationsViewModel>();
        builder.Services.AddTransient<AlertsViewModel>();
        builder.Services.AddTransient<SettingsViewModel>();

        // Pages
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<RegisterPage>();
        builder.Services.AddTransient<DashboardPage>();
        builder.Services.AddTransient<LocationsPage>();
        builder.Services.AddTransient<AlertsPage>();
        builder.Services.AddTransient<SettingsPage>();

        return builder.Build();
    }
}
