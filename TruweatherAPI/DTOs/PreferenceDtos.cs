namespace TruweatherAPI.DTOs;

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
    string? TemperatureUnit = null,
    string? WindSpeedUnit = null,
    bool? EnableNotifications = null,
    bool? EnableEmailAlerts = null,
    string? Theme = null,
    string? Language = null,
    int? UpdateFrequencyMinutes = null
);
