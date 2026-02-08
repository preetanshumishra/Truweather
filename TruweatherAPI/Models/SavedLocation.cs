namespace TruweatherAPI.Models;

public class SavedLocation
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string LocationName { get; set; } = string.Empty;
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public bool IsDefault { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public User User { get; set; } = null!;
    public ICollection<WeatherAlert> WeatherAlerts { get; set; } = [];
}
