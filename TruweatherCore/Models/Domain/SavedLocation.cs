namespace TruweatherCore.Models.Domain;

public class SavedLocation
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string LocationName { get; set; } = string.Empty;
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public bool IsDefault { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
