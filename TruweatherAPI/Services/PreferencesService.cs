using Microsoft.EntityFrameworkCore;
using TruweatherAPI.Data;
using TruweatherAPI.Models;
using TruweatherCore.Models.DTOs;
using TruweatherCore.Services.Interfaces;

namespace TruweatherAPI.Services;

/// <summary>
/// Service for managing user preferences.
/// Implements IPreferencesService interface for API/Web/Mobile consumption.
/// </summary>
public class PreferencesService(TruweatherDbContext context) : IPreferencesService
{
    private readonly TruweatherDbContext _context = context;

    public async Task<UserPreferencesDto?> GetPreferencesAsync(string userId)
    {
        var preferences = await _context.UserPreferences
            .FirstOrDefaultAsync(p => p.UserId == userId);

        if (preferences == null)
        {
            // Create default preferences if they don't exist
            return await CreateDefaultPreferencesAsync(userId);
        }

        return MapToDto(preferences);
    }

    public async Task<bool> UpdatePreferencesAsync(string userId, UpdatePreferencesRequest request)
    {
        var preferences = await _context.UserPreferences
            .FirstOrDefaultAsync(p => p.UserId == userId);

        if (preferences == null)
        {
            // Create default preferences and apply updates
            preferences = new UserPreferences
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };
            _context.UserPreferences.Add(preferences);
        }

        // Update fields if provided in request
        if (!string.IsNullOrEmpty(request.TemperatureUnit))
            preferences.TemperatureUnit = request.TemperatureUnit;

        if (!string.IsNullOrEmpty(request.WindSpeedUnit))
            preferences.WindSpeedUnit = request.WindSpeedUnit;

        if (request.EnableNotifications.HasValue)
            preferences.EnableNotifications = request.EnableNotifications.Value;

        if (request.EnableEmailAlerts.HasValue)
            preferences.EnableEmailAlerts = request.EnableEmailAlerts.Value;

        if (!string.IsNullOrEmpty(request.Theme))
            preferences.Theme = request.Theme;

        if (!string.IsNullOrEmpty(request.Language))
            preferences.Language = request.Language;

        if (request.UpdateFrequencyMinutes.HasValue)
            preferences.UpdateFrequencyMinutes = request.UpdateFrequencyMinutes.Value;

        preferences.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Create default preferences for a new user.
    /// </summary>
    private async Task<UserPreferencesDto?> CreateDefaultPreferencesAsync(string userId)
    {
        var defaultPreferences = new UserPreferences
        {
            UserId = userId,
            TemperatureUnit = "Celsius",
            WindSpeedUnit = "ms",
            EnableNotifications = true,
            EnableEmailAlerts = true,
            Theme = "Light",
            Language = "en",
            UpdateFrequencyMinutes = 30,
            CreatedAt = DateTime.UtcNow
        };

        _context.UserPreferences.Add(defaultPreferences);
        await _context.SaveChangesAsync();

        return MapToDto(defaultPreferences);
    }

    /// <summary>
    /// Map UserPreferences entity to DTO.
    /// </summary>
    private static UserPreferencesDto MapToDto(UserPreferences preferences)
    {
        return new UserPreferencesDto(
            TemperatureUnit: preferences.TemperatureUnit,
            WindSpeedUnit: preferences.WindSpeedUnit,
            EnableNotifications: preferences.EnableNotifications,
            EnableEmailAlerts: preferences.EnableEmailAlerts,
            Theme: preferences.Theme,
            Language: preferences.Language,
            UpdateFrequencyMinutes: preferences.UpdateFrequencyMinutes
        );
    }
}
