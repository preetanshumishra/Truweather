namespace TruweatherAPI.Models;

public class UserPreferences
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string TemperatureUnit { get; set; } = "Celsius";
    public string WindSpeedUnit { get; set; } = "ms";
    public bool EnableNotifications { get; set; } = true;
    public bool EnableEmailAlerts { get; set; } = true;
    public string Theme { get; set; } = "Light";
    public string Language { get; set; } = "en";
    public int UpdateFrequencyMinutes { get; set; } = 30;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigation property
    public User User { get; set; } = null!;
}
