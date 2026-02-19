using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TruweatherAPI.Data;
using TruweatherAPI.Models;
using TruweatherCore.Models.DTOs;
using TruweatherCore.Services.Interfaces;

namespace TruweatherAPI.Services;

public class AdminService(
    UserManager<User> userManager,
    TruweatherDbContext context) : IAdminService
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly TruweatherDbContext _context = context;

    public async Task<List<AdminUserDto>> GetUsersAsync(string? searchTerm = null)
    {
        var query = _userManager.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(u =>
                (u.Email != null && u.Email.Contains(searchTerm)) ||
                (u.UserName != null && u.UserName.Contains(searchTerm)));
        }

        var users = await query
            .OrderByDescending(u => u.CreatedAt)
            .ToListAsync();

        var result = new List<AdminUserDto>();
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var locationCount = await _context.SavedLocations.CountAsync(l => l.UserId == user.Id);
            var alertCount = await _context.WeatherAlerts.CountAsync(a => a.UserId == user.Id);

            result.Add(new AdminUserDto(
                user.Id,
                user.Email ?? string.Empty,
                user.UserName,
                user.IsEmailVerified,
                user.CreatedAt,
                user.LastLoginAt,
                roles.ToList(),
                locationCount,
                alertCount
            ));
        }

        return result;
    }

    public async Task<AdminUserDetailDto?> GetUserDetailAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return null;

        var roles = await _userManager.GetRolesAsync(user);

        var locations = await _context.SavedLocations
            .Where(l => l.UserId == userId)
            .Select(l => new SavedLocationDto(l.Id, l.LocationName, l.Latitude, l.Longitude, l.IsDefault))
            .ToListAsync();

        var alerts = await _context.WeatherAlerts
            .Where(a => a.UserId == userId)
            .Select(a => new WeatherAlertDto(a.Id, a.SavedLocationId, a.AlertType, a.Condition, a.Threshold, a.IsEnabled, a.CreatedAt))
            .ToListAsync();

        var prefs = await _context.UserPreferences
            .Where(p => p.UserId == userId)
            .Select(p => new UserPreferencesDto(
                p.TemperatureUnit, p.WindSpeedUnit, p.EnableNotifications,
                p.EnableEmailAlerts, p.Theme, p.Language, p.UpdateFrequencyMinutes))
            .FirstOrDefaultAsync();

        return new AdminUserDetailDto(
            user.Id,
            user.Email ?? string.Empty,
            user.UserName,
            user.IsEmailVerified,
            user.CreatedAt,
            user.LastLoginAt,
            roles.ToList(),
            locations,
            alerts,
            prefs
        );
    }

    public async Task<bool> UpdateUserRoleAsync(string userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return false;

        var currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        var result = await _userManager.AddToRoleAsync(user, role);

        return result.Succeeded;
    }

    public async Task<bool> DeleteUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return false;

        var result = await _userManager.DeleteAsync(user);
        return result.Succeeded;
    }

    public async Task<SystemStatsDto> GetSystemStatsAsync()
    {
        var totalUsers = await _userManager.Users.CountAsync();
        var totalLocations = await _context.SavedLocations.CountAsync();
        var totalAlerts = await _context.WeatherAlerts.CountAsync();
        var totalNotifications = await _context.Notifications.CountAsync();
        var activeAlerts = await _context.WeatherAlerts.CountAsync(a => a.IsEnabled);
        var unreadNotifications = await _context.Notifications.CountAsync(n => !n.IsRead);

        return new SystemStatsDto(
            totalUsers,
            totalLocations,
            totalAlerts,
            totalNotifications,
            activeAlerts,
            unreadNotifications
        );
    }

    public async Task<List<AdminAlertDto>> GetAllAlertsAsync()
    {
        return await _context.WeatherAlerts
            .Include(a => a.User)
            .Include(a => a.SavedLocation)
            .OrderByDescending(a => a.CreatedAt)
            .Select(a => new AdminAlertDto(
                a.Id,
                a.UserId,
                a.User.Email ?? string.Empty,
                a.SavedLocationId,
                a.SavedLocation.LocationName,
                a.AlertType,
                a.Condition,
                a.Threshold,
                a.IsEnabled,
                a.IsNotified,
                a.CreatedAt,
                a.LastTriggeredAt
            ))
            .ToListAsync();
    }
}
