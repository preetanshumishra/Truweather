using Microsoft.AspNetCore.Identity;

namespace TruweatherAPI.Models;

public class User : IdentityUser
{
    public bool IsEmailVerified { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }

    // Navigation properties
    public ICollection<SavedLocation> SavedLocations { get; set; } = [];
    public ICollection<WeatherAlert> WeatherAlerts { get; set; } = [];
    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
    public UserPreferences? UserPreferences { get; set; }
}
