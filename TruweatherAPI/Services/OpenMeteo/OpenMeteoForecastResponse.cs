namespace TruweatherAPI.Services.OpenMeteo;

/// <summary>
/// Open-Meteo API response for daily forecast data.
/// Maps to https://api.open-meteo.com/v1/forecast endpoint with daily parameters.
/// </summary>
public class OpenMeteoForecastResponse
{
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public string Timezone { get; set; } = string.Empty;
    public DailyWeatherData Daily { get; set; } = new();
}

public class DailyWeatherData
{
    public DateTime[] Time { get; set; } = Array.Empty<DateTime>();
    public decimal[] Temperature2mMax { get; set; } = Array.Empty<decimal>();
    public decimal[] Temperature2mMin { get; set; } = Array.Empty<decimal>();
    public decimal[] Temperature2mMean { get; set; } = Array.Empty<decimal>();
    public int[] WeatherCode { get; set; } = Array.Empty<int>();
    public decimal[] PrecipitationSum { get; set; } = Array.Empty<decimal>();
    public decimal[] WindSpeed10mMax { get; set; } = Array.Empty<decimal>();
    public int[] WindDirection10mDominant { get; set; } = Array.Empty<int>();
}
