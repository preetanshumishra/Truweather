using System.ComponentModel.DataAnnotations;

namespace TruweatherCore.Models.DTOs;

/// <summary>User preference settings for weather display and notifications.</summary>
public record UserPreferencesDto(
    /// <summary>Temperature unit (Celsius, Fahrenheit, or Kelvin).</summary>
    string TemperatureUnit,
    /// <summary>Wind speed unit (ms, kmh, mph, or knots).</summary>
    string WindSpeedUnit,
    /// <summary>Whether push notifications are enabled.</summary>
    bool EnableNotifications,
    /// <summary>Whether email alerts are enabled.</summary>
    bool EnableEmailAlerts,
    /// <summary>UI theme (Light or Dark).</summary>
    string Theme,
    /// <summary>Preferred language code (en, es, fr, de, it, pt, ru, zh, ja, ko).</summary>
    string Language,
    /// <summary>Frequency to update weather data in minutes (5-1440).</summary>
    int UpdateFrequencyMinutes
);

/// <summary>Request body for updating user preferences. All fields are optional.</summary>
public record UpdatePreferencesRequest(
    /// <summary>Temperature unit (Celsius, Fahrenheit, or Kelvin). Optional.</summary>
    [RegularExpression("^(Celsius|Fahrenheit|Kelvin)$", ErrorMessage = "Invalid temperature unit")]
    string? TemperatureUnit = null,

    /// <summary>Wind speed unit (ms, kmh, mph, or knots). Optional.</summary>
    [RegularExpression("^(ms|kmh|mph|knots)$", ErrorMessage = "Invalid wind speed unit")]
    string? WindSpeedUnit = null,

    /// <summary>Whether to enable push notifications. Optional.</summary>
    bool? EnableNotifications = null,

    /// <summary>Whether to enable email alerts. Optional.</summary>
    bool? EnableEmailAlerts = null,

    /// <summary>UI theme (Light or Dark). Optional.</summary>
    [RegularExpression("^(Light|Dark)$", ErrorMessage = "Invalid theme")]
    string? Theme = null,

    /// <summary>Preferred language code (en, es, fr, de, it, pt, ru, zh, ja, ko). Optional.</summary>
    [RegularExpression("^(en|es|fr|de|it|pt|ru|zh|ja|ko)$", ErrorMessage = "Invalid language")]
    string? Language = null,

    /// <summary>Update frequency in minutes (5-1440). Optional.</summary>
    [Range(5, 1440, ErrorMessage = "Update frequency must be between 5 and 1440 minutes")]
    int? UpdateFrequencyMinutes = null
);
