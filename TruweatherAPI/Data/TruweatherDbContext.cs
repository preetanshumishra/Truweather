using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TruweatherAPI.Models;

namespace TruweatherAPI.Data;

public class TruweatherDbContext(DbContextOptions<TruweatherDbContext> options)
    : IdentityDbContext<User>(options)
{
    public DbSet<SavedLocation> SavedLocations { get; set; }
    public DbSet<WeatherData> WeatherData { get; set; }
    public DbSet<WeatherAlert> WeatherAlerts { get; set; }
    public DbSet<UserPreferences> UserPreferences { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // User configuration
        builder.Entity<User>()
            .HasMany(u => u.SavedLocations)
            .WithOne(l => l.User)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<User>()
            .HasMany(u => u.WeatherAlerts)
            .WithOne(a => a.User)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<User>()
            .HasOne(u => u.UserPreferences)
            .WithOne(p => p.User)
            .HasForeignKey<UserPreferences>(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<User>()
            .HasMany(u => u.RefreshTokens)
            .WithOne(r => r.User)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // RefreshToken configuration
        builder.Entity<RefreshToken>()
            .HasIndex(r => r.Token)
            .IsUnique(true);

        builder.Entity<RefreshToken>()
            .HasIndex(r => r.UserId)
            .IsUnique(false);

        // SavedLocation configuration
        builder.Entity<SavedLocation>()
            .HasMany(l => l.WeatherAlerts)
            .WithOne(a => a.SavedLocation)
            .HasForeignKey(a => a.SavedLocationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<SavedLocation>()
            .HasIndex(l => l.UserId)
            .IsUnique(false);

        // WeatherData configuration
        builder.Entity<WeatherData>()
            .HasOne(w => w.SavedLocation)
            .WithMany()
            .HasForeignKey(w => w.SavedLocationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<WeatherData>()
            .HasIndex(w => new { w.SavedLocationId, w.Date })
            .IsUnique(false);

        // WeatherAlert configuration
        builder.Entity<WeatherAlert>()
            .HasIndex(a => a.UserId)
            .IsUnique(false);

        builder.Entity<WeatherAlert>()
            .HasIndex(a => a.SavedLocationId)
            .IsUnique(false);

        // Precision configuration for decimal columns
        builder.Entity<SavedLocation>()
            .Property(l => l.Latitude)
            .HasPrecision(9, 6);

        builder.Entity<SavedLocation>()
            .Property(l => l.Longitude)
            .HasPrecision(9, 6);

        builder.Entity<WeatherData>()
            .Property(w => w.Temperature)
            .HasPrecision(5, 2);

        builder.Entity<WeatherData>()
            .Property(w => w.FeelsLike)
            .HasPrecision(5, 2);

        builder.Entity<WeatherData>()
            .Property(w => w.MinTemperature)
            .HasPrecision(5, 2);

        builder.Entity<WeatherData>()
            .Property(w => w.MaxTemperature)
            .HasPrecision(5, 2);

        builder.Entity<WeatherData>()
            .Property(w => w.WindSpeed)
            .HasPrecision(5, 2);

        builder.Entity<WeatherAlert>()
            .Property(a => a.Threshold)
            .HasPrecision(5, 2);
    }
}
