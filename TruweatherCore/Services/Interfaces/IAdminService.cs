using TruweatherCore.Models.DTOs;

namespace TruweatherCore.Services.Interfaces;

/// <summary>
/// Interface for admin operations: user management, system stats, alert overview.
/// </summary>
public interface IAdminService
{
    Task<List<AdminUserDto>> GetUsersAsync(string? searchTerm = null);
    Task<AdminUserDetailDto?> GetUserDetailAsync(string userId);
    Task<bool> UpdateUserRoleAsync(string userId, string role);
    Task<bool> DeleteUserAsync(string userId);
    Task<SystemStatsDto> GetSystemStatsAsync();
    Task<List<AdminAlertDto>> GetAllAlertsAsync();
}
