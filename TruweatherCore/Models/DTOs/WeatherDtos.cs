using System.ComponentModel.DataAnnotations;

namespace TruweatherCore.Models.DTOs;

public record CurrentWeatherDto(
    string LocationName,
    decimal Latitude,
    decimal Longitude,
    decimal Temperature,
    decimal FeelsLike,
    string Condition,
    string Description,
    int Humidity,
    decimal WindSpeed,
    int WindDegree,
    decimal Pressure,
    int Visibility,
    string IconUrl,
    DateTime RetrievedAt
);

public record ForecastDto(
    string LocationName,
    decimal Latitude,
    decimal Longitude,
    List<ForecastDayDto> Days
);

public record ForecastDayDto(
    DateTime Date,
    decimal MaxTemperature,
    decimal MinTemperature,
    decimal AvgTemperature,
    string Condition,
    string Description,
    int Humidity,
    decimal WindSpeed,
    decimal Precipitation,
    string IconUrl
);

public record SavedLocationDto(
    int Id,
    string LocationName,
    decimal Latitude,
    decimal Longitude,
    bool IsDefault
);

public record CreateLocationRequest(
    [Required(ErrorMessage = "Location name is required")]
    [MinLength(1, ErrorMessage = "Location name must not be empty")]
    [MaxLength(100, ErrorMessage = "Location name must not exceed 100 characters")]
    string LocationName,

    [Required(ErrorMessage = "Latitude is required")]
    [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90")]
    decimal Latitude,

    [Required(ErrorMessage = "Longitude is required")]
    [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180")]
    decimal Longitude,

    bool IsDefault = false
);

public record UpdateLocationRequest(
    [Required(ErrorMessage = "Location name is required")]
    [MinLength(1, ErrorMessage = "Location name must not be empty")]
    [MaxLength(100, ErrorMessage = "Location name must not exceed 100 characters")]
    string LocationName,

    bool IsDefault
);

public record WeatherAlertDto(
    int Id,
    int SavedLocationId,
    string AlertType,
    string Condition,
    decimal Threshold,
    bool IsEnabled,
    DateTime CreatedAt
);

public record CreateWeatherAlertRequest(
    [Required(ErrorMessage = "Saved location ID is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Saved location ID must be greater than 0")]
    int SavedLocationId,

    [Required(ErrorMessage = "Alert type is required")]
    [MinLength(1, ErrorMessage = "Alert type must not be empty")]
    string AlertType,

    [Required(ErrorMessage = "Condition is required")]
    [MinLength(1, ErrorMessage = "Condition must not be empty")]
    string Condition,

    [Required(ErrorMessage = "Threshold is required")]
    decimal Threshold
);

public record UpdateWeatherAlertRequest(
    [Required(ErrorMessage = "Alert type is required")]
    [MinLength(1, ErrorMessage = "Alert type must not be empty")]
    string AlertType,

    [Required(ErrorMessage = "Condition is required")]
    [MinLength(1, ErrorMessage = "Condition must not be empty")]
    string Condition,

    [Required(ErrorMessage = "Threshold is required")]
    decimal Threshold,

    bool IsEnabled
);
