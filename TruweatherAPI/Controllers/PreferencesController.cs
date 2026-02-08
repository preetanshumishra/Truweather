using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;
using TruweatherCore.Models.DTOs;
using TruweatherCore.Services.Interfaces;

namespace TruweatherAPI.Controllers;

/// <summary>
/// API endpoints for user preference and settings management.
///
/// Provides endpoints to retrieve and update user settings including temperature unit (Celsius/Fahrenheit),
/// wind speed unit (m/s, km/h, mph), language/locale (10 languages), theme preference (light/dark),
/// and notification preferences. Preferences auto-created on first user registration.
///
/// All endpoints require JWT authentication via Authorization header.
/// Rate limit: 100 requests per minute per IP address.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
[EnableRateLimiting("api")]
public class PreferencesController(IPreferencesService preferencesService, ILogger<PreferencesController> logger) : ControllerBase
{
    private readonly IPreferencesService _preferencesService = preferencesService;
    private readonly ILogger<PreferencesController> _logger = logger;

    /// <summary>
    /// Extract user ID from JWT claims.
    /// </summary>
    private string GetUserId()
    {
        return User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new UnauthorizedAccessException();
    }

    /// <summary>Get the authenticated user's preferences and settings.</summary>
    /// <remarks>
    /// Returns all user settings including:
    /// - TemperatureUnit: "Celsius" or "Fahrenheit"
    /// - WindSpeedUnit: "ms" (m/s), "kmh" (km/h), or "mph"
    /// - Language: "en", "es", "fr", "de", "it", "pt", "ru", "zh", "ja", "ko"
    /// - Theme: "light", "dark", or "auto"
    /// - NotificationsEnabled: boolean flag for push notifications
    ///
    /// Preferences are auto-created with defaults on first user registration.
    /// </remarks>
    [HttpGet]
    [ProducesResponseType(typeof(UserPreferencesDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<ActionResult<UserPreferencesDto>> GetPreferences()
    {
        var userId = GetUserId();
        var preferences = await _preferencesService.GetPreferencesAsync(userId);
        return preferences == null ? NotFound() : Ok(preferences);
    }

    /// <summary>Update the authenticated user's preferences and settings.</summary>
    /// <remarks>
    /// Supports partial updates - only include fields to be changed.
    /// Null/missing fields are not updated, preserving existing values.
    ///
    /// Valid values:
    /// - TemperatureUnit: "Celsius", "Fahrenheit"
    /// - WindSpeedUnit: "ms", "kmh", "mph"
    /// - Language: "en", "es", "fr", "de", "it", "pt", "ru", "zh", "ja", "ko"
    /// - Theme: "light", "dark", "auto"
    /// - NotificationsEnabled: true/false
    ///
    /// Example: Update language only while keeping other settings:
    /// ```
    /// PUT /api/preferences
    /// { "language": "es" }
    /// ```
    /// </remarks>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<IActionResult> UpdatePreferences(UpdatePreferencesRequest request)
    {
        var userId = GetUserId();
        var success = await _preferencesService.UpdatePreferencesAsync(userId, request);
        return success ? NoContent() : NotFound();
    }
}
