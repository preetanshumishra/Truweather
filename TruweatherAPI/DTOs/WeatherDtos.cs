namespace TruweatherAPI.DTOs;

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
    string LocationName,
    decimal Latitude,
    decimal Longitude,
    bool IsDefault = false
);

public record UpdateLocationRequest(
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
    int SavedLocationId,
    string AlertType,
    string Condition,
    decimal Threshold
);

public record UpdateWeatherAlertRequest(
    string AlertType,
    string Condition,
    decimal Threshold,
    bool IsEnabled
);
