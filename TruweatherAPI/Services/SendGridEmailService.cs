using SendGrid;
using SendGrid.Helpers.Mail;
using TruweatherCore.Services.Interfaces;

namespace TruweatherAPI.Services;

public class SendGridEmailService(
    IConfiguration configuration,
    ILogger<SendGridEmailService> logger) : IEmailService
{
    private readonly ILogger<SendGridEmailService> _logger = logger;
    private readonly string _apiKey = configuration["Email:SendGridApiKey"] ?? string.Empty;
    private readonly string _fromEmail = configuration["Email:FromEmail"] ?? "noreply@truweather.com";
    private readonly string _fromName = configuration["Email:FromName"] ?? "Truweather";

    public async Task<bool> SendEmailAsync(string toEmail, string subject, string htmlContent)
    {
        try
        {
            var client = new SendGridClient(_apiKey);
            var from = new EmailAddress(_fromEmail, _fromName);
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, null, htmlContent);
            var response = await client.SendEmailAsync(msg);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Email sent to {Email}: {Subject}", toEmail, subject);
                return true;
            }

            _logger.LogWarning("Failed to send email to {Email}. Status: {Status}",
                toEmail, response.StatusCode);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email to {Email}", toEmail);
            return false;
        }
    }

    public async Task<bool> SendAlertEmailAsync(string toEmail, string alertTitle, string alertMessage)
    {
        var html = $"""
            <div style="font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;">
                <h2 style="color: #e74c3c;">Weather Alert</h2>
                <h3>{alertTitle}</h3>
                <p>{alertMessage}</p>
                <hr />
                <p style="color: #666; font-size: 12px;">
                    You received this email because you have email alerts enabled in Truweather.
                    Update your preferences in the app to manage notifications.
                </p>
            </div>
            """;

        return await SendEmailAsync(toEmail, $"Truweather Alert: {alertTitle}", html);
    }

    public async Task<bool> SendEmailVerificationAsync(string toEmail, string verificationToken)
    {
        var html = $"""
            <div style="font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;">
                <h2>Verify Your Email</h2>
                <p>Thank you for registering with Truweather. Please use the code below to verify your email:</p>
                <div style="background: #f0f0f0; padding: 20px; text-align: center; font-size: 24px; letter-spacing: 4px; font-weight: bold;">
                    {verificationToken}
                </div>
                <p style="color: #666; font-size: 12px; margin-top: 20px;">
                    If you did not create an account, please ignore this email.
                </p>
            </div>
            """;

        return await SendEmailAsync(toEmail, "Verify Your Truweather Account", html);
    }

    public async Task<bool> SendPasswordResetAsync(string toEmail, string resetToken)
    {
        var html = $"""
            <div style="font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;">
                <h2>Reset Your Password</h2>
                <p>You requested a password reset. Use the code below to reset your password:</p>
                <div style="background: #f0f0f0; padding: 20px; text-align: center; font-size: 24px; letter-spacing: 4px; font-weight: bold;">
                    {resetToken}
                </div>
                <p style="color: #666; font-size: 12px; margin-top: 20px;">
                    If you did not request a password reset, please ignore this email.
                    This code expires in 1 hour.
                </p>
            </div>
            """;

        return await SendEmailAsync(toEmail, "Reset Your Truweather Password", html);
    }
}
