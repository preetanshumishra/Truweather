namespace TruweatherCore.Models.Domain;

public class WeatherData
{
    public int Id { get; set; }
    public int SavedLocationId { get; set; }
    public DateTime Date { get; set; }
    public string Condition { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Temperature { get; set; }
    public decimal FeelsLike { get; set; }
    public decimal MinTemperature { get; set; }
    public decimal MaxTemperature { get; set; }
    public int Humidity { get; set; }
    public decimal Pressure { get; set; }
    public int Visibility { get; set; }
    public decimal WindSpeed { get; set; }
    public int WindDegree { get; set; }
    public decimal Cloudiness { get; set; }
    public decimal Precipitation { get; set; }
    public decimal Uvi { get; set; }
    public string IconUrl { get; set; } = string.Empty;
    public bool IsForecast { get; set; } = false;
    public DateTime CachedAt { get; set; } = DateTime.UtcNow;
}
