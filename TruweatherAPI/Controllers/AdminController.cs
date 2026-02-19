using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TruweatherCore.Models.DTOs;
using TruweatherCore.Services.Interfaces;

namespace TruweatherAPI.Controllers;

/// <summary>
/// Admin-only endpoints for user management and system overview.
/// Requires the "Admin" role.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
[EnableRateLimiting("api")]
public class AdminController(IAdminService adminService) : ControllerBase
{
    private readonly IAdminService _adminService = adminService;

    /// <summary>Get all users with optional search.</summary>
    /// <param name="search">Optional search term to filter by email or name</param>
    /// <response code="200">List of users</response>
    /// <response code="403">User is not an admin</response>
    [HttpGet("users")]
    [ProducesResponseType(typeof(List<AdminUserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<List<AdminUserDto>>> GetUsers([FromQuery] string? search = null)
    {
        var users = await _adminService.GetUsersAsync(search);
        return Ok(users);
    }

    /// <summary>Get detailed information about a specific user.</summary>
    /// <param name="id">User ID</param>
    /// <response code="200">Detailed user info</response>
    /// <response code="404">User not found</response>
    [HttpGet("users/{id}")]
    [ProducesResponseType(typeof(AdminUserDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AdminUserDetailDto>> GetUserDetail(string id)
    {
        var user = await _adminService.GetUserDetailAsync(id);
        return user == null ? NotFound() : Ok(user);
    }

    /// <summary>Update a user's role.</summary>
    /// <param name="id">User ID</param>
    /// <param name="request">New role (Admin or User)</param>
    /// <response code="204">Role updated</response>
    /// <response code="404">User not found</response>
    [HttpPut("users/{id}/role")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUserRole(string id, UpdateRoleRequest request)
    {
        var success = await _adminService.UpdateUserRoleAsync(id, request.Role);
        return success ? NoContent() : NotFound();
    }

    /// <summary>Delete a user account.</summary>
    /// <param name="id">User ID</param>
    /// <response code="204">User deleted</response>
    /// <response code="404">User not found</response>
    [HttpDelete("users/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var success = await _adminService.DeleteUserAsync(id);
        return success ? NoContent() : NotFound();
    }

    /// <summary>Get system-wide statistics.</summary>
    /// <response code="200">System stats</response>
    [HttpGet("stats")]
    [ProducesResponseType(typeof(SystemStatsDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<SystemStatsDto>> GetSystemStats()
    {
        var stats = await _adminService.GetSystemStatsAsync();
        return Ok(stats);
    }

    /// <summary>Get all weather alerts across all users.</summary>
    /// <response code="200">List of all alerts with user info</response>
    [HttpGet("alerts")]
    [ProducesResponseType(typeof(List<AdminAlertDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<AdminAlertDto>>> GetAllAlerts()
    {
        var alerts = await _adminService.GetAllAlertsAsync();
        return Ok(alerts);
    }
}
