using System.ComponentModel.DataAnnotations;

namespace TruweatherCore.Models.DTOs;

/// <summary>Request body for email verification.</summary>
public record VerifyEmailRequest(
    [Required(ErrorMessage = "Token is required")]
    string Token,

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    string Email
);

/// <summary>Request body for initiating password reset.</summary>
public record ForgotPasswordRequest(
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    string Email
);

/// <summary>Request body for resetting password with token.</summary>
public class ResetPasswordRequest
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Token is required")]
    public string Token { get; set; } = "";

    [Required(ErrorMessage = "New password is required")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    public string NewPassword { get; set; } = "";

    [Required(ErrorMessage = "Password confirmation is required")]
    [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; } = "";
}
