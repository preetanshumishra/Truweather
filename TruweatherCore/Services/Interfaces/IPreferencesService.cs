using TruweatherCore.Models.DTOs;

namespace TruweatherCore.Services.Interfaces;

/// <summary>
/// Interface for user preferences management shared across API, Web, and Mobile.
/// API implements business logic; Web/Mobile use HttpClient to call API endpoints.
/// </summary>
public interface IPreferencesService
{
    /// <summary>
    /// Get user preferences for the current user.
    /// </summary>
    Task<UserPreferencesDto?> GetPreferencesAsync(string userId);

    /// <summary>
    /// Update user preferences.
    /// </summary>
    Task<bool> UpdatePreferencesAsync(string userId, UpdatePreferencesRequest request);
}
