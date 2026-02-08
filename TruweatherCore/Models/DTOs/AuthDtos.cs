using System.ComponentModel.DataAnnotations;

namespace TruweatherCore.Models.DTOs;

public record RegisterRequest(
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email format is invalid")]
    string Email,

    [Required(ErrorMessage = "Password is required")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    string Password,

    [Required(ErrorMessage = "Password confirmation is required")]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    string ConfirmPassword,

    [MaxLength(100, ErrorMessage = "Full name must not exceed 100 characters")]
    string? FullName = null
);

public record LoginRequest(
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email format is invalid")]
    string Email,

    [Required(ErrorMessage = "Password is required")]
    string Password
);

public record AuthResponse(
    bool Success,
    string Message,
    string? AccessToken = null,
    string? RefreshToken = null,
    UserDto? User = null
);

public record RefreshTokenRequest(
    string RefreshToken
);

public record UserDto(
    string Id,
    string Email,
    string? FullName,
    bool IsEmailVerified,
    DateTime CreatedAt
);
