using System.ComponentModel.DataAnnotations;

namespace TruweatherCore.Models.DTOs;

/// <summary>Admin view of a user account.</summary>
public record AdminUserDto(
    string Id,
    string Email,
    string? FullName,
    bool IsEmailVerified,
    DateTime CreatedAt,
    DateTime? LastLoginAt,
    List<string> Roles,
    int SavedLocationCount,
    int AlertCount
);

/// <summary>Detailed admin view of a user.</summary>
public record AdminUserDetailDto(
    string Id,
    string Email,
    string? FullName,
    bool IsEmailVerified,
    DateTime CreatedAt,
    DateTime? LastLoginAt,
    List<string> Roles,
    List<SavedLocationDto> SavedLocations,
    List<WeatherAlertDto> WeatherAlerts,
    UserPreferencesDto? Preferences
);

/// <summary>System-wide statistics for admin dashboard.</summary>
public record SystemStatsDto(
    int TotalUsers,
    int TotalLocations,
    int TotalAlerts,
    int TotalNotifications,
    int ActiveAlerts,
    int UnreadNotifications
);

/// <summary>Admin view of a weather alert with user info.</summary>
public record AdminAlertDto(
    int Id,
    string UserId,
    string UserEmail,
    int SavedLocationId,
    string LocationName,
    string AlertType,
    string Condition,
    decimal Threshold,
    bool IsEnabled,
    bool IsNotified,
    DateTime CreatedAt,
    DateTime? LastTriggeredAt
);

/// <summary>Request to update a user's role.</summary>
public record UpdateRoleRequest(
    [Required(ErrorMessage = "Role is required")]
    [RegularExpression("^(Admin|User)$", ErrorMessage = "Role must be Admin or User")]
    string Role
);
