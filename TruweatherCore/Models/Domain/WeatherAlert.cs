namespace TruweatherCore.Models.Domain;

public class WeatherAlert
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int SavedLocationId { get; set; }
    public string AlertType { get; set; } = string.Empty;
    public string Condition { get; set; } = string.Empty;
    public decimal Threshold { get; set; }
    public bool IsEnabled { get; set; } = true;
    public bool IsNotified { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastTriggeredAt { get; set; }
}
