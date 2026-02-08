using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TruweatherCore.Models.DTOs;
using TruweatherCore.Services.Interfaces;

namespace TruweatherAPI.Controllers;

/// <summary>
/// API endpoints for user preferences management.
/// Provides endpoints to get and update user preferences (temperature unit, language, theme, etc).
/// All endpoints require authentication via JWT.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PreferencesController(IPreferencesService preferencesService) : ControllerBase
{
    private readonly IPreferencesService _preferencesService = preferencesService;

    /// <summary>
    /// Extract user ID from JWT claims.
    /// </summary>
    private string GetUserId()
    {
        return User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new UnauthorizedAccessException();
    }

    /// <summary>
    /// GET /api/preferences
    /// Get the current user's preferences.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<UserPreferencesDto>> GetPreferences()
    {
        var userId = GetUserId();
        var preferences = await _preferencesService.GetPreferencesAsync(userId);
        return preferences == null ? NotFound() : Ok(preferences);
    }

    /// <summary>
    /// PUT /api/preferences
    /// Update the current user's preferences.
    /// Any null/unspecified fields will not be updated.
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> UpdatePreferences(UpdatePreferencesRequest request)
    {
        var userId = GetUserId();
        var success = await _preferencesService.UpdatePreferencesAsync(userId, request);
        return success ? NoContent() : NotFound();
    }
}
