namespace TruweatherAPI.Services.OpenMeteo;

/// <summary>
/// Open-Meteo API response for current weather data.
/// Maps to https://api.open-meteo.com/v1/forecast endpoint with current parameters.
/// </summary>
public class OpenMeteoCurrentResponse
{
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public string Timezone { get; set; } = string.Empty;
    public CurrentWeatherData Current { get; set; } = new();
}

public class CurrentWeatherData
{
    public DateTime Time { get; set; }
    public decimal Temperature2m { get; set; }
    public int RelativeHumidity2m { get; set; }
    public int WeatherCode { get; set; }
    public decimal WindSpeed10m { get; set; }
    public int WindDirection10m { get; set; }
    public decimal Pressure { get; set; }
}
