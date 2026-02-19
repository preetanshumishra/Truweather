using TruweatherCore.Services.Interfaces;

namespace TruweatherAPI.Services;

/// <summary>
/// No-op email service for development and testing.
/// Logs email details instead of sending.
/// </summary>
public class NoOpEmailService(ILogger<NoOpEmailService> logger) : IEmailService
{
    private readonly ILogger<NoOpEmailService> _logger = logger;

    public Task<bool> SendEmailAsync(string toEmail, string subject, string htmlContent)
    {
        _logger.LogInformation("[NoOp Email] To: {Email}, Subject: {Subject}", toEmail, subject);
        return Task.FromResult(true);
    }

    public Task<bool> SendAlertEmailAsync(string toEmail, string alertTitle, string alertMessage)
    {
        _logger.LogInformation("[NoOp Email] Alert to {Email}: {Title} - {Message}",
            toEmail, alertTitle, alertMessage);
        return Task.FromResult(true);
    }

    public Task<bool> SendEmailVerificationAsync(string toEmail, string verificationToken)
    {
        _logger.LogInformation("[NoOp Email] Verification to {Email}, Token: {Token}",
            toEmail, verificationToken);
        return Task.FromResult(true);
    }

    public Task<bool> SendPasswordResetAsync(string toEmail, string resetToken)
    {
        _logger.LogInformation("[NoOp Email] Password reset to {Email}, Token: {Token}",
            toEmail, resetToken);
        return Task.FromResult(true);
    }
}
