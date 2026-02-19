namespace TruweatherAPI.Tests.Unit.Services;

public class EmailServiceTests
{
    private readonly Mock<ILogger<NoOpEmailService>> _mockLogger;
    private readonly NoOpEmailService _service;

    public EmailServiceTests()
    {
        _mockLogger = new Mock<ILogger<NoOpEmailService>>();
        _service = new NoOpEmailService(_mockLogger.Object);
    }

    [Fact]
    public async Task SendEmailAsync_ReturnsTrue()
    {
        var result = await _service.SendEmailAsync("test@test.com", "Subject", "<p>Body</p>");
        result.Should().BeTrue();
    }

    [Fact]
    public async Task SendAlertEmailAsync_ReturnsTrue()
    {
        var result = await _service.SendAlertEmailAsync("test@test.com", "High Temp", "Temperature is 35");
        result.Should().BeTrue();
    }

    [Fact]
    public async Task SendEmailVerificationAsync_ReturnsTrue()
    {
        var result = await _service.SendEmailVerificationAsync("test@test.com", "verify-token-123");
        result.Should().BeTrue();
    }

    [Fact]
    public async Task SendPasswordResetAsync_ReturnsTrue()
    {
        var result = await _service.SendPasswordResetAsync("test@test.com", "reset-token-456");
        result.Should().BeTrue();
    }

    [Fact]
    public async Task SendEmailAsync_LogsEmailDetails()
    {
        await _service.SendEmailAsync("user@example.com", "Test Subject", "<p>Content</p>");

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("user@example.com")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task SendAlertEmailAsync_LogsAlertDetails()
    {
        await _service.SendAlertEmailAsync("user@example.com", "Wind Alert", "Wind speed exceeded 50");

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Wind Alert")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}
