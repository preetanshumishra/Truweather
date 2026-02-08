using System.ComponentModel.DataAnnotations;

namespace TruweatherCore.Models.DTOs;

public record UserPreferencesDto(
    string TemperatureUnit,
    string WindSpeedUnit,
    bool EnableNotifications,
    bool EnableEmailAlerts,
    string Theme,
    string Language,
    int UpdateFrequencyMinutes
);

public record UpdatePreferencesRequest(
    [RegularExpression("^(Celsius|Fahrenheit|Kelvin)$", ErrorMessage = "Invalid temperature unit")]
    string? TemperatureUnit = null,

    [RegularExpression("^(ms|kmh|mph|knots)$", ErrorMessage = "Invalid wind speed unit")]
    string? WindSpeedUnit = null,

    bool? EnableNotifications = null,

    bool? EnableEmailAlerts = null,

    [RegularExpression("^(Light|Dark)$", ErrorMessage = "Invalid theme")]
    string? Theme = null,

    [RegularExpression("^(en|es|fr|de|it|pt|ru|zh|ja|ko)$", ErrorMessage = "Invalid language")]
    string? Language = null,

    [Range(5, 1440, ErrorMessage = "Update frequency must be between 5 and 1440 minutes")]
    int? UpdateFrequencyMinutes = null
);
