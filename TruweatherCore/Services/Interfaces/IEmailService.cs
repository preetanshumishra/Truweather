namespace TruweatherCore.Services.Interfaces;

/// <summary>
/// Interface for sending emails (alerts, verification, password reset).
/// </summary>
public interface IEmailService
{
    Task<bool> SendEmailAsync(string toEmail, string subject, string htmlContent);
    Task<bool> SendAlertEmailAsync(string toEmail, string alertTitle, string alertMessage);
    Task<bool> SendEmailVerificationAsync(string toEmail, string verificationToken);
    Task<bool> SendPasswordResetAsync(string toEmail, string resetToken);
}
