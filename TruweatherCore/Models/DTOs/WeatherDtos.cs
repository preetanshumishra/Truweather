using System.ComponentModel.DataAnnotations;

namespace TruweatherCore.Models.DTOs;

/// <summary>Current weather conditions for a location.</summary>
public record CurrentWeatherDto(
    /// <summary>Name of the location.</summary>
    string LocationName,
    /// <summary>Latitude of the location.</summary>
    decimal Latitude,
    /// <summary>Longitude of the location.</summary>
    decimal Longitude,
    /// <summary>Current temperature in the configured unit.</summary>
    decimal Temperature,
    /// <summary>Feels-like temperature in the configured unit.</summary>
    decimal FeelsLike,
    /// <summary>Weather condition (e.g., "Clear", "Rainy").</summary>
    string Condition,
    /// <summary>Detailed weather description.</summary>
    string Description,
    /// <summary>Humidity percentage (0-100).</summary>
    int Humidity,
    /// <summary>Wind speed in the configured unit.</summary>
    decimal WindSpeed,
    /// <summary>Wind direction in degrees (0-360).</summary>
    int WindDegree,
    /// <summary>Atmospheric pressure in hPa.</summary>
    decimal Pressure,
    /// <summary>Visibility in meters.</summary>
    int Visibility,
    /// <summary>URL to weather condition icon.</summary>
    string IconUrl,
    /// <summary>Timestamp when weather data was retrieved.</summary>
    DateTime RetrievedAt
);

/// <summary>7-day weather forecast for a location.</summary>
public record ForecastDto(
    /// <summary>Name of the location.</summary>
    string LocationName,
    /// <summary>Latitude of the location.</summary>
    decimal Latitude,
    /// <summary>Longitude of the location.</summary>
    decimal Longitude,
    /// <summary>List of daily forecast data (7 days).</summary>
    List<ForecastDayDto> Days
);

/// <summary>Weather forecast for a single day.</summary>
public record ForecastDayDto(
    /// <summary>Date of the forecast.</summary>
    DateTime Date,
    /// <summary>Maximum temperature in the configured unit.</summary>
    decimal MaxTemperature,
    /// <summary>Minimum temperature in the configured unit.</summary>
    decimal MinTemperature,
    /// <summary>Average temperature in the configured unit.</summary>
    decimal AvgTemperature,
    /// <summary>Weather condition.</summary>
    string Condition,
    /// <summary>Detailed weather description.</summary>
    string Description,
    /// <summary>Humidity percentage (0-100).</summary>
    int Humidity,
    /// <summary>Wind speed in the configured unit.</summary>
    decimal WindSpeed,
    /// <summary>Precipitation amount in millimeters.</summary>
    decimal Precipitation,
    /// <summary>URL to weather condition icon.</summary>
    string IconUrl
);

/// <summary>A location saved by the user.</summary>
public record SavedLocationDto(
    /// <summary>Location ID.</summary>
    int Id,
    /// <summary>Name of the location.</summary>
    string LocationName,
    /// <summary>Latitude coordinate.</summary>
    decimal Latitude,
    /// <summary>Longitude coordinate.</summary>
    decimal Longitude,
    /// <summary>Whether this is the user's default location.</summary>
    bool IsDefault
);

/// <summary>Request body for creating a new saved location.</summary>
public record CreateLocationRequest(
    /// <summary>Name of the location.</summary>
    [Required(ErrorMessage = "Location name is required")]
    [MinLength(1, ErrorMessage = "Location name must not be empty")]
    [MaxLength(100, ErrorMessage = "Location name must not exceed 100 characters")]
    string LocationName,

    /// <summary>Latitude coordinate (-90 to 90).</summary>
    [Required(ErrorMessage = "Latitude is required")]
    [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90")]
    decimal Latitude,

    /// <summary>Longitude coordinate (-180 to 180).</summary>
    [Required(ErrorMessage = "Longitude is required")]
    [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180")]
    decimal Longitude,

    /// <summary>Whether to set as default location (optional).</summary>
    bool IsDefault = false
);

/// <summary>Request body for updating a saved location.</summary>
public record UpdateLocationRequest(
    /// <summary>Updated location name.</summary>
    [Required(ErrorMessage = "Location name is required")]
    [MinLength(1, ErrorMessage = "Location name must not be empty")]
    [MaxLength(100, ErrorMessage = "Location name must not exceed 100 characters")]
    string LocationName,

    /// <summary>Whether to set as default location.</summary>
    bool IsDefault
);

/// <summary>A weather alert configuration.</summary>
public record WeatherAlertDto(
    /// <summary>Alert ID.</summary>
    int Id,
    /// <summary>ID of the saved location this alert applies to.</summary>
    int SavedLocationId,
    /// <summary>Type of alert (e.g., "Temperature", "Humidity").</summary>
    string AlertType,
    /// <summary>Trigger condition (e.g., "GreaterThan", "LessThan").</summary>
    string Condition,
    /// <summary>Threshold value for the condition.</summary>
    decimal Threshold,
    /// <summary>Whether the alert is currently enabled.</summary>
    bool IsEnabled,
    /// <summary>Timestamp when the alert was created.</summary>
    DateTime CreatedAt
);

/// <summary>Request body for creating a new weather alert.</summary>
public record CreateWeatherAlertRequest(
    /// <summary>ID of the saved location to create alert for.</summary>
    [Required(ErrorMessage = "Saved location ID is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Saved location ID must be greater than 0")]
    int SavedLocationId,

    /// <summary>Type of alert to create.</summary>
    [Required(ErrorMessage = "Alert type is required")]
    [MinLength(1, ErrorMessage = "Alert type must not be empty")]
    string AlertType,

    /// <summary>Trigger condition for the alert.</summary>
    [Required(ErrorMessage = "Condition is required")]
    [MinLength(1, ErrorMessage = "Condition must not be empty")]
    string Condition,

    /// <summary>Threshold value to trigger the alert.</summary>
    [Required(ErrorMessage = "Threshold is required")]
    decimal Threshold
);

/// <summary>Request body for updating a weather alert.</summary>
public record UpdateWeatherAlertRequest(
    /// <summary>Updated alert type.</summary>
    [Required(ErrorMessage = "Alert type is required")]
    [MinLength(1, ErrorMessage = "Alert type must not be empty")]
    string AlertType,

    /// <summary>Updated trigger condition.</summary>
    [Required(ErrorMessage = "Condition is required")]
    [MinLength(1, ErrorMessage = "Condition must not be empty")]
    string Condition,

    /// <summary>Updated threshold value.</summary>
    [Required(ErrorMessage = "Threshold is required")]
    decimal Threshold,

    /// <summary>Whether the alert should be enabled.</summary>
    bool IsEnabled
);
