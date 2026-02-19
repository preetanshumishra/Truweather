using TruweatherCore.Models.DTOs;

namespace TruweatherCore.Services.Interfaces;

/// <summary>
/// Interface for notification CRUD operations.
/// </summary>
public interface INotificationService
{
    Task<List<NotificationDto>> GetNotificationsAsync(string userId);
    Task<int> GetUnreadCountAsync(string userId);
    Task<bool> MarkAsReadAsync(string userId, int notificationId);
    Task<bool> MarkAllAsReadAsync(string userId);
}
