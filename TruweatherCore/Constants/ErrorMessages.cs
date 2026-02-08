namespace TruweatherCore.Constants;

/// <summary>
/// Standard error messages used across API, Web, and Mobile for consistency.
/// </summary>
public static class ErrorMessages
{
    // Authentication
    public const string InvalidCredentials = "Invalid email or password.";
    public const string UserNotFound = "User not found.";
    public const string EmailAlreadyRegistered = "Email is already registered.";
    public const string PasswordMismatch = "Passwords do not match.";
    public const string InvalidToken = "Invalid or expired token.";
    public const string UnauthorizedAccess = "You are not authorized to perform this action.";

    // Validation
    public const string InvalidEmail = "Email format is invalid.";
    public const string PasswordTooShort = "Password must be at least 8 characters.";
    public const string LocationNameRequired = "Location name is required.";
    public const string InvalidCoordinates = "Latitude must be between -90 and 90, longitude between -180 and 180.";
    public const string InvalidTemperatureThreshold = "Temperature must be between -273.15°C and 60°C.";
    public const string InvalidWindSpeed = "Wind speed must be between 0 and 500 m/s.";

    // Weather/Location
    public const string LocationNotFound = "Location not found.";
    public const string WeatherDataUnavailable = "Weather data is currently unavailable.";
    public const string NoSavedLocations = "No saved locations found.";
    public const string DefaultLocationNotFound = "Default location not set.";

    // Alerts
    public const string AlertNotFound = "Weather alert not found.";
    public const string InvalidAlertType = "Invalid alert type.";
    public const string InvalidAlertCondition = "Invalid alert condition.";

    // API Communication
    public const string NetworkError = "Network error. Please check your connection.";
    public const string ServerError = "Server error. Please try again later.";
    public const string RequestTimeout = "Request timed out. Please try again.";
    public const string BadRequest = "Invalid request. Please check your input.";

    // Preferences
    public const string InvalidTemperatureUnit = "Invalid temperature unit.";
    public const string InvalidWindSpeedUnit = "Invalid wind speed unit.";
    public const string InvalidTheme = "Invalid theme selection.";
    public const string InvalidLanguage = "Invalid language selection.";
    public const string InvalidUpdateFrequency = "Update frequency must be between 5 and 1440 minutes.";
}
