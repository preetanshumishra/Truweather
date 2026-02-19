namespace TruweatherCore.Constants;

/// <summary>
/// API endpoint paths used by Web and Mobile clients.
/// Base URL is configured per environment (http://localhost:5000 for dev, production URL for prod).
/// </summary>
public static class ApiEndpoints
{
    private const string AuthBase = "/api/auth";
    private const string WeatherBase = "/api/weather";

    // Authentication endpoints
    public static string AuthRegister => $"{AuthBase}/register";
    public static string AuthLogin => $"{AuthBase}/login";
    public static string AuthRefresh => $"{AuthBase}/refresh";
    public static string AuthLogout => $"{AuthBase}/logout";

    // Weather endpoints
    public static string WeatherCurrent => $"{WeatherBase}/current";
    public static string WeatherForecast => $"{WeatherBase}/forecast";
    public static string WeatherLocations => $"{WeatherBase}/locations";
    public static string WeatherLocationDetails(int id) => $"{WeatherBase}/locations/{id}";
    public static string WeatherAlerts => $"{WeatherBase}/alerts";
    public static string WeatherAlertDetails(int id) => $"{WeatherBase}/alerts/{id}";

    // Notification endpoints
    private const string NotificationBase = "/api/notifications";
    public static string Notifications => NotificationBase;
    public static string NotificationUnreadCount => $"{NotificationBase}/unread-count";
    public static string NotificationMarkRead(int id) => $"{NotificationBase}/{id}/read";
    public static string NotificationMarkAllRead => $"{NotificationBase}/read-all";

    // Auth email endpoints
    public static string AuthVerifyEmail => $"{AuthBase}/verify-email";
    public static string AuthForgotPassword => $"{AuthBase}/forgot-password";
    public static string AuthResetPassword => $"{AuthBase}/reset-password";

    // Admin endpoints
    private const string AdminBase = "/api/admin";
    public static string AdminUsers => $"{AdminBase}/users";
    public static string AdminUserDetails(string id) => $"{AdminBase}/users/{id}";
    public static string AdminUserRole(string id) => $"{AdminBase}/users/{id}/role";
    public static string AdminStats => $"{AdminBase}/stats";
    public static string AdminAlerts => $"{AdminBase}/alerts";
}
