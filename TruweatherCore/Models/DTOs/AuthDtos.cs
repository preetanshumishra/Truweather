using System.ComponentModel.DataAnnotations;

namespace TruweatherCore.Models.DTOs;

public class RegisterRequest
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email format is invalid")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Password is required")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    public string Password { get; set; } = "";

    [Required(ErrorMessage = "Password confirmation is required")]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; } = "";

    [MaxLength(100, ErrorMessage = "Full name must not exceed 100 characters")]
    public string? FullName { get; set; }
}

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
