using Microsoft.Extensions.DependencyInjection;

namespace TruweatherAPI.Tests.Unit.Services;

public class AlertEvaluationServiceTests
{
    // --- GetWeatherValue Tests ---

    [Theory]
    [InlineData("Temperature", 22.5)]
    [InlineData("temperature", 22.5)]
    public void GetWeatherValue_Temperature_ReturnsCorrectValue(string alertType, decimal expected)
    {
        var weather = CreateTestWeather(temperature: expected);
        var result = AlertEvaluationService.GetWeatherValue(alertType, weather);

        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("Wind", 5.0)]
    [InlineData("WindSpeed", 5.0)]
    [InlineData("wind", 5.0)]
    public void GetWeatherValue_Wind_ReturnsCorrectValue(string alertType, decimal expected)
    {
        var weather = CreateTestWeather(windSpeed: expected);
        var result = AlertEvaluationService.GetWeatherValue(alertType, weather);

        result.Should().Be(expected);
    }

    [Fact]
    public void GetWeatherValue_Humidity_ReturnsCorrectValue()
    {
        var weather = CreateTestWeather(humidity: 75);
        var result = AlertEvaluationService.GetWeatherValue("Humidity", weather);

        result.Should().Be(75);
    }

    [Fact]
    public void GetWeatherValue_Pressure_ReturnsCorrectValue()
    {
        var weather = CreateTestWeather(pressure: 1013);
        var result = AlertEvaluationService.GetWeatherValue("Pressure", weather);

        result.Should().Be(1013);
    }

    [Fact]
    public void GetWeatherValue_UnknownType_ReturnsNull()
    {
        var weather = CreateTestWeather();
        var result = AlertEvaluationService.GetWeatherValue("Unknown", weather);

        result.Should().BeNull();
    }

    // --- EvaluateCondition Tests ---

    [Theory]
    [InlineData("GreaterThan", 35, 30, true)]
    [InlineData("GreaterThan", 25, 30, false)]
    [InlineData("Above", 35, 30, true)]
    [InlineData(">", 35, 30, true)]
    public void EvaluateCondition_GreaterThan_EvaluatesCorrectly(string condition, decimal current, decimal threshold, bool expected)
    {
        var result = AlertEvaluationService.EvaluateCondition(current, condition, threshold);
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("LessThan", 25, 30, true)]
    [InlineData("LessThan", 35, 30, false)]
    [InlineData("Below", 25, 30, true)]
    [InlineData("<", 25, 30, true)]
    public void EvaluateCondition_LessThan_EvaluatesCorrectly(string condition, decimal current, decimal threshold, bool expected)
    {
        var result = AlertEvaluationService.EvaluateCondition(current, condition, threshold);
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("Equal", 30, 30, true)]
    [InlineData("Equals", 30.005, 30, true)]
    [InlineData("=", 30.02, 30, false)]
    public void EvaluateCondition_Equal_EvaluatesCorrectly(string condition, decimal current, decimal threshold, bool expected)
    {
        var result = AlertEvaluationService.EvaluateCondition(current, condition, threshold);
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(">=", 30, 30, true)]
    [InlineData(">=", 35, 30, true)]
    [InlineData(">=", 25, 30, false)]
    public void EvaluateCondition_GreaterThanOrEqual_EvaluatesCorrectly(string condition, decimal current, decimal threshold, bool expected)
    {
        var result = AlertEvaluationService.EvaluateCondition(current, condition, threshold);
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("<=", 30, 30, true)]
    [InlineData("<=", 25, 30, true)]
    [InlineData("<=", 35, 30, false)]
    public void EvaluateCondition_LessThanOrEqual_EvaluatesCorrectly(string condition, decimal current, decimal threshold, bool expected)
    {
        var result = AlertEvaluationService.EvaluateCondition(current, condition, threshold);
        result.Should().Be(expected);
    }

    [Fact]
    public void EvaluateCondition_UnknownCondition_ReturnsFalse()
    {
        var result = AlertEvaluationService.EvaluateCondition(30, "InvalidCondition", 25);
        result.Should().BeFalse();
    }

    // --- Integration-style: EvaluateAlertsAsync ---

    [Fact]
    public async Task EvaluateAlertsAsync_WhenConditionMet_CreatesNotification()
    {
        using var context = TestDbContextFactory.Create();

        var user = new User
        {
            Id = "user-1", Email = "test@test.com", UserName = "test@test.com",
            UserPreferences = new UserPreferences { UserId = "user-1", EnableNotifications = true }
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var location = new SavedLocation
        {
            UserId = "user-1", LocationName = "NYC", Latitude = 40.7m, Longitude = -74.0m
        };
        context.SavedLocations.Add(location);
        await context.SaveChangesAsync();

        context.WeatherAlerts.Add(new WeatherAlert
        {
            UserId = "user-1", SavedLocationId = location.Id,
            AlertType = "Temperature", Condition = "GreaterThan",
            Threshold = 20.0m, IsEnabled = true, IsNotified = false
        });
        await context.SaveChangesAsync();

        var mockOpenMeteo = CreateMockOpenMeteo(temperature: 25.0m);
        var mockEmailService = new Mock<IEmailService>();
        var service = CreateEvaluationService(context, mockOpenMeteo.Object, mockEmailService.Object);

        await service.EvaluateAlertsAsync();

        context.Notifications.Should().HaveCount(1);
        var notification = context.Notifications.First();
        notification.Title.Should().Contain("Temperature");
        notification.UserId.Should().Be("user-1");

        var alert = context.WeatherAlerts.First();
        alert.IsNotified.Should().BeTrue();
        alert.LastTriggeredAt.Should().NotBeNull();
    }

    [Fact]
    public async Task EvaluateAlertsAsync_WhenConditionClears_ResetsNotified()
    {
        using var context = TestDbContextFactory.Create();

        var user = new User
        {
            Id = "user-1", Email = "test@test.com", UserName = "test@test.com",
            UserPreferences = new UserPreferences { UserId = "user-1", EnableNotifications = true }
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var location = new SavedLocation
        {
            UserId = "user-1", LocationName = "NYC", Latitude = 40.7m, Longitude = -74.0m
        };
        context.SavedLocations.Add(location);
        await context.SaveChangesAsync();

        context.WeatherAlerts.Add(new WeatherAlert
        {
            UserId = "user-1", SavedLocationId = location.Id,
            AlertType = "Temperature", Condition = "GreaterThan",
            Threshold = 30.0m, IsEnabled = true, IsNotified = true,
            LastTriggeredAt = DateTime.UtcNow.AddHours(-1)
        });
        await context.SaveChangesAsync();

        // Temperature is 25 which is NOT > 30 -> should clear
        var mockOpenMeteo = CreateMockOpenMeteo(temperature: 25.0m);
        var service = CreateEvaluationService(context, mockOpenMeteo.Object);

        await service.EvaluateAlertsAsync();

        var alert = context.WeatherAlerts.First();
        alert.IsNotified.Should().BeFalse();
        context.Notifications.Should().BeEmpty();
    }

    [Fact]
    public async Task EvaluateAlertsAsync_SkipsDisabledAlerts()
    {
        using var context = TestDbContextFactory.Create();

        var user = new User
        {
            Id = "user-1", Email = "test@test.com", UserName = "test@test.com",
            UserPreferences = new UserPreferences { UserId = "user-1", EnableNotifications = true }
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var location = new SavedLocation
        {
            UserId = "user-1", LocationName = "NYC", Latitude = 40.7m, Longitude = -74.0m
        };
        context.SavedLocations.Add(location);
        await context.SaveChangesAsync();

        context.WeatherAlerts.Add(new WeatherAlert
        {
            UserId = "user-1", SavedLocationId = location.Id,
            AlertType = "Temperature", Condition = "GreaterThan",
            Threshold = 20.0m, IsEnabled = false, IsNotified = false
        });
        await context.SaveChangesAsync();

        var mockOpenMeteo = CreateMockOpenMeteo(temperature: 25.0m);
        var service = CreateEvaluationService(context, mockOpenMeteo.Object);

        await service.EvaluateAlertsAsync();

        context.Notifications.Should().BeEmpty();
    }

    [Fact]
    public async Task EvaluateAlertsAsync_SkipsUsersWithNotificationsDisabled()
    {
        using var context = TestDbContextFactory.Create();

        var user = new User
        {
            Id = "user-1", Email = "test@test.com", UserName = "test@test.com",
            UserPreferences = new UserPreferences { UserId = "user-1", EnableNotifications = false }
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var location = new SavedLocation
        {
            UserId = "user-1", LocationName = "NYC", Latitude = 40.7m, Longitude = -74.0m
        };
        context.SavedLocations.Add(location);
        await context.SaveChangesAsync();

        context.WeatherAlerts.Add(new WeatherAlert
        {
            UserId = "user-1", SavedLocationId = location.Id,
            AlertType = "Temperature", Condition = "GreaterThan",
            Threshold = 20.0m, IsEnabled = true, IsNotified = false
        });
        await context.SaveChangesAsync();

        var mockOpenMeteo = CreateMockOpenMeteo(temperature: 25.0m);
        var service = CreateEvaluationService(context, mockOpenMeteo.Object);

        await service.EvaluateAlertsAsync();

        context.Notifications.Should().BeEmpty();
    }

    // --- Helpers ---

    private static CurrentWeatherDto CreateTestWeather(
        decimal temperature = 22.5m, decimal windSpeed = 5.0m,
        int humidity = 55, decimal pressure = 1013)
    {
        return new CurrentWeatherDto(
            LocationName: "Test City",
            Latitude: 40.7m,
            Longitude: -74.0m,
            Temperature: temperature,
            FeelsLike: temperature,
            Condition: "Clear",
            Description: "Clear sky",
            Humidity: humidity,
            WindSpeed: windSpeed,
            WindDegree: 180,
            Pressure: pressure,
            Visibility: 10000,
            IconUrl: "clear",
            RetrievedAt: DateTime.UtcNow
        );
    }

    private static Mock<OpenMeteoWeatherService> CreateMockOpenMeteo(decimal temperature = 22.5m)
    {
        var mockHttpClient = new HttpClient();
        var mockLogger = new Mock<ILogger<OpenMeteoWeatherService>>();
        var mock = new Mock<OpenMeteoWeatherService>(mockHttpClient, mockLogger.Object);

        var weather = CreateTestWeather(temperature: temperature);
        mock.Setup(x => x.GetCurrentWeatherAsync(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<string?>()))
            .ReturnsAsync(weather);

        return mock;
    }

    private static AlertEvaluationService CreateEvaluationService(
        TruweatherDbContext context,
        OpenMeteoWeatherService openMeteo,
        IEmailService? emailService = null)
    {
        var serviceCollection = new Microsoft.Extensions.DependencyInjection.ServiceCollection();
        serviceCollection.AddSingleton(context);
        serviceCollection.AddSingleton(openMeteo);
        if (emailService != null)
            serviceCollection.AddSingleton(emailService);

        // Build a scope factory from a real provider
        var provider = serviceCollection.BuildServiceProvider();
        var scopeFactory = provider.GetRequiredService<IServiceScopeFactory>();

        var config = TestConfiguration.Create();
        var logger = new Mock<ILogger<AlertEvaluationService>>();

        return new AlertEvaluationService(scopeFactory, config, logger.Object);
    }
}
