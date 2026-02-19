namespace TruweatherCore.Models.DTOs;

/// <summary>Notification data for the user.</summary>
public record NotificationDto(
    int Id,
    int? AlertId,
    string Title,
    string Message,
    bool IsRead,
    DateTime CreatedAt
);

/// <summary>Count of unread notifications.</summary>
public record UnreadCountDto(int Count);
