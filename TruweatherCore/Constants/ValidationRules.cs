namespace TruweatherCore.Constants;

/// <summary>
/// Validation rules and constraints shared across API, Web, and Mobile.
/// </summary>
public static class ValidationRules
{
    // Email validation
    public const int EmailMinLength = 5;
    public const int EmailMaxLength = 254;

    // Password validation
    public const int PasswordMinLength = 8;
    public const int PasswordMaxLength = 128;

    // Location name validation
    public const int LocationNameMinLength = 1;
    public const int LocationNameMaxLength = 100;

    // Coordinates validation
    public const decimal MinLatitude = -90m;
    public const decimal MaxLatitude = 90m;
    public const decimal MinLongitude = -180m;
    public const decimal MaxLongitude = 180m;

    // Alert threshold validation
    public const decimal MinTemperature = -273.15m; // Absolute zero
    public const decimal MaxTemperature = 60m; // Practical maximum
    public const decimal MinWindSpeed = 0m;
    public const decimal MaxWindSpeed = 500m; // Highest recorded wind speeds

    // Preferences
    public static readonly string[] TemperatureUnits = { "Celsius", "Fahrenheit", "Kelvin" };
    public static readonly string[] WindSpeedUnits = { "ms", "kmh", "mph", "knots" };
    public static readonly string[] Themes = { "Light", "Dark" };
    public static readonly string[] Languages = { "en", "es", "fr", "de", "it", "pt", "ru", "zh", "ja", "ko" };
    public const int MinUpdateFrequency = 5;  // minutes
    public const int MaxUpdateFrequency = 1440; // 24 hours
}
