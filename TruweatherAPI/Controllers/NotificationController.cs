using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;
using TruweatherCore.Models.DTOs;
using TruweatherCore.Services.Interfaces;

namespace TruweatherAPI.Controllers;

/// <summary>
/// API endpoints for user notification management.
/// Provides access to alert-triggered notifications with read/unread tracking.
/// All endpoints require JWT authentication.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
[EnableRateLimiting("api")]
public class NotificationController(INotificationService notificationService) : ControllerBase
{
    private readonly INotificationService _notificationService = notificationService;

    private string GetUserId()
    {
        return User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new UnauthorizedAccessException();
    }

    /// <summary>Get all notifications for the authenticated user.</summary>
    /// <response code="200">List of notifications ordered by most recent first</response>
    /// <response code="401">Unauthorized</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<NotificationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<NotificationDto>>> GetNotifications()
    {
        var userId = GetUserId();
        var notifications = await _notificationService.GetNotificationsAsync(userId);
        return Ok(notifications);
    }

    /// <summary>Get the count of unread notifications.</summary>
    /// <response code="200">Unread notification count</response>
    /// <response code="401">Unauthorized</response>
    [HttpGet("unread-count")]
    [ProducesResponseType(typeof(UnreadCountDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UnreadCountDto>> GetUnreadCount()
    {
        var userId = GetUserId();
        var count = await _notificationService.GetUnreadCountAsync(userId);
        return Ok(new UnreadCountDto(count));
    }

    /// <summary>Mark a specific notification as read.</summary>
    /// <param name="id">Notification ID</param>
    /// <response code="204">Notification marked as read</response>
    /// <response code="404">Notification not found</response>
    [HttpPut("{id}/read")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var userId = GetUserId();
        var success = await _notificationService.MarkAsReadAsync(userId, id);
        return success ? NoContent() : NotFound();
    }

    /// <summary>Mark all notifications as read.</summary>
    /// <response code="204">All notifications marked as read</response>
    [HttpPut("read-all")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> MarkAllAsRead()
    {
        var userId = GetUserId();
        await _notificationService.MarkAllAsReadAsync(userId);
        return NoContent();
    }
}
