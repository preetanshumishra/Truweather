using System.ComponentModel.DataAnnotations;

namespace TruweatherCore.Models.DTOs;

/// <summary>Request body for user registration.</summary>
public class RegisterRequest
{
    /// <summary>User's email address.</summary>
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email format is invalid")]
    public string Email { get; set; } = "";

    /// <summary>User's password (minimum 8 characters).</summary>
    [Required(ErrorMessage = "Password is required")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    public string Password { get; set; } = "";

    /// <summary>Password confirmation (must match Password).</summary>
    [Required(ErrorMessage = "Password confirmation is required")]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; } = "";

    /// <summary>User's full name (optional, max 100 characters).</summary>
    [MaxLength(100, ErrorMessage = "Full name must not exceed 100 characters")]
    public string? FullName { get; set; }
}

/// <summary>Request body for user login.</summary>
public record LoginRequest(
    /// <summary>User's email address.</summary>
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email format is invalid")]
    string Email,

    /// <summary>User's password.</summary>
    [Required(ErrorMessage = "Password is required")]
    string Password
);

/// <summary>Response body for authentication requests (register, login, refresh).</summary>
public record AuthResponse(
    /// <summary>Whether the operation was successful.</summary>
    bool Success,
    /// <summary>Status message describing the result.</summary>
    string Message,
    /// <summary>JWT access token (returned on successful auth).</summary>
    string? AccessToken = null,
    /// <summary>Refresh token for obtaining new access tokens (returned on successful auth).</summary>
    string? RefreshToken = null,
    /// <summary>Authenticated user details (returned on successful auth).</summary>
    UserDto? User = null
);

/// <summary>Request body for token refresh.</summary>
public record RefreshTokenRequest(
    /// <summary>Valid refresh token.</summary>
    string RefreshToken
);

/// <summary>User profile information.</summary>
public record UserDto(
    /// <summary>User's unique identifier.</summary>
    string Id,
    /// <summary>User's email address.</summary>
    string Email,
    /// <summary>User's full name (optional).</summary>
    string? FullName,
    /// <summary>Whether the user's email is verified.</summary>
    bool IsEmailVerified,
    /// <summary>Timestamp when the user account was created.</summary>
    DateTime CreatedAt
);
