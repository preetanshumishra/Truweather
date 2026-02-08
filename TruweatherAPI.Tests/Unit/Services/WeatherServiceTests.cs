using Microsoft.Extensions.Caching.Memory;

namespace TruweatherAPI.Tests.Unit.Services;

public class WeatherServiceTests
{
    private readonly Mock<IMemoryCache> _mockCache;
    private readonly Mock<OpenMeteoWeatherService> _mockOpenMeteo;
    private readonly IConfiguration _configuration;
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
    private readonly DefaultHttpContext _httpContext;

    public WeatherServiceTests()
    {
        _mockCache = new Mock<IMemoryCache>();
        _configuration = TestConfiguration.Create();
        _httpContext = new DefaultHttpContext();
        _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(_httpContext);

        // Create mock OpenMeteo with virtual methods
        var mockHttpClient = new HttpClient();
        var mockLogger = new Mock<ILogger<OpenMeteoWeatherService>>();
        _mockOpenMeteo = new Mock<OpenMeteoWeatherService>(mockHttpClient, mockLogger.Object);
    }

    private WeatherService CreateService(TruweatherDbContext context)
    {
        return new WeatherService(
            context,
            _mockCache.Object,
            _mockOpenMeteo.Object,
            _configuration,
            _mockHttpContextAccessor.Object);
    }

    private void SetupCacheMiss()
    {
        object? nullValue = null;
        _mockCache.Setup(c => c.TryGetValue(It.IsAny<object>(), out nullValue)).Returns(false);
        var mockEntry = new Mock<ICacheEntry>();
        mockEntry.SetupAllProperties();
        _mockCache.Setup(c => c.CreateEntry(It.IsAny<object>())).Returns(mockEntry.Object);
    }

    private static CurrentWeatherDto CreateTestWeather()
    {
        return new CurrentWeatherDto(
            LocationName: "Test City",
            Latitude: 40.7m,
            Longitude: -74.0m,
            Temperature: 22.5m,
            FeelsLike: 21.0m,
            Condition: "Clear",
            Description: "Clear sky",
            Humidity: 55,
            WindSpeed: 3.5m,
            WindDegree: 180,
            Pressure: 1013,
            Visibility: 10000,
            IconUrl: "clear",
            RetrievedAt: DateTime.UtcNow
        );
    }

    // --- Current Weather Caching Tests ---

    [Fact]
    public async Task GetCurrentWeatherAsync_WhenCached_ReturnsCachedData()
    {
        using var context = TestDbContextFactory.Create();
        var weather = CreateTestWeather();
        object cachedValue = weather;
        _mockCache.Setup(c => c.TryGetValue(It.IsAny<object>(), out cachedValue)).Returns(true);

        var service = CreateService(context);
        var result = await service.GetCurrentWeatherAsync(40.7m, -74.0m);

        result.Should().NotBeNull();
        result!.Temperature.Should().Be(22.5m);
        _mockOpenMeteo.Verify(x => x.GetCurrentWeatherAsync(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<string?>()), Times.Never);
        _httpContext.Response.Headers["X-Data-Source"].ToString().Should().Be("MemoryCache");
    }

    [Fact]
    public async Task GetCurrentWeatherAsync_WhenNotCached_FetchesFromApi()
    {
        using var context = TestDbContextFactory.Create();
        SetupCacheMiss();
        var weather = CreateTestWeather();
        _mockOpenMeteo.Setup(x => x.GetCurrentWeatherAsync(40.7m, -74.0m, It.IsAny<string?>()))
            .ReturnsAsync(weather);

        var service = CreateService(context);
        var result = await service.GetCurrentWeatherAsync(40.7m, -74.0m);

        result.Should().NotBeNull();
        result!.Temperature.Should().Be(22.5m);
        _httpContext.Response.Headers["X-Data-Source"].ToString().Should().Be("Live");
    }

    [Fact]
    public async Task GetCurrentWeatherAsync_WhenApiFails_FallsToDatabase()
    {
        using var context = TestDbContextFactory.Create();
        SetupCacheMiss();
        _mockOpenMeteo.Setup(x => x.GetCurrentWeatherAsync(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<string?>()))
            .ReturnsAsync((CurrentWeatherDto?)null);

        // Seed a saved location and weather data
        var location = new SavedLocation
        {
            UserId = "user-1",
            LocationName = "New York",
            Latitude = 40.7m,
            Longitude = -74.0m
        };
        context.SavedLocations.Add(location);
        await context.SaveChangesAsync();

        context.WeatherData.Add(new WeatherData
        {
            SavedLocationId = location.Id,
            Temperature = 20.0m,
            FeelsLike = 19.0m,
            Condition = "Cloudy",
            Description = "Overcast",
            WindSpeed = 5.0m,
            WindDegree = 90,
            Pressure = 1010,
            Visibility = 8000,
            CachedAt = DateTime.UtcNow
        });
        await context.SaveChangesAsync();

        var service = CreateService(context);
        var result = await service.GetCurrentWeatherAsync(40.7m, -74.0m);

        result.Should().NotBeNull();
        result!.Temperature.Should().Be(20.0m);
        _httpContext.Response.Headers["X-Data-Source"].ToString().Should().Be("Database");
    }

    [Fact]
    public async Task GetCurrentWeatherAsync_WhenAllFail_ReturnsNull()
    {
        using var context = TestDbContextFactory.Create();
        SetupCacheMiss();
        _mockOpenMeteo.Setup(x => x.GetCurrentWeatherAsync(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<string?>()))
            .ReturnsAsync((CurrentWeatherDto?)null);

        var service = CreateService(context);
        var result = await service.GetCurrentWeatherAsync(40.7m, -74.0m);

        result.Should().BeNull();
        _httpContext.Response.Headers["X-Data-Source"].ToString().Should().Be("Fallback");
    }

    [Fact]
    public async Task GetCurrentWeatherAsync_PersistsToDb_ForSavedLocations()
    {
        using var context = TestDbContextFactory.Create();
        SetupCacheMiss();
        var weather = CreateTestWeather();
        _mockOpenMeteo.Setup(x => x.GetCurrentWeatherAsync(40.7m, -74.0m, It.IsAny<string?>()))
            .ReturnsAsync(weather);

        context.SavedLocations.Add(new SavedLocation
        {
            UserId = "user-1",
            LocationName = "New York",
            Latitude = 40.7m,
            Longitude = -74.0m
        });
        await context.SaveChangesAsync();

        var service = CreateService(context);
        await service.GetCurrentWeatherAsync(40.7m, -74.0m);

        context.WeatherData.Should().HaveCount(1);
    }

    // --- Forecast Tests ---

    [Fact]
    public async Task GetForecastAsync_WhenCached_ReturnsCachedForecast()
    {
        using var context = TestDbContextFactory.Create();
        var forecast = new ForecastDto("Test", 40.7m, -74.0m, new List<ForecastDayDto>());
        object cachedValue = forecast;
        _mockCache.Setup(c => c.TryGetValue(It.IsAny<object>(), out cachedValue)).Returns(true);

        var service = CreateService(context);
        var result = await service.GetForecastAsync(40.7m, -74.0m);

        result.Should().NotBeNull();
        _mockOpenMeteo.Verify(x => x.GetForecastAsync(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<string?>()), Times.Never);
    }

    [Fact]
    public async Task GetForecastAsync_WhenApiFails_ReturnsMockFallback()
    {
        using var context = TestDbContextFactory.Create();
        SetupCacheMiss();
        _mockOpenMeteo.Setup(x => x.GetForecastAsync(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<string?>()))
            .ReturnsAsync((ForecastDto?)null);

        var service = CreateService(context);
        var result = await service.GetForecastAsync(40.7m, -74.0m);

        result.Should().NotBeNull();
        result!.Days.Should().HaveCount(7);
        _httpContext.Response.Headers["X-Data-Source"].ToString().Should().Be("Fallback");
    }

    // --- Saved Locations CRUD Tests ---

    [Fact]
    public async Task GetSavedLocationsAsync_ReturnsUserLocations()
    {
        using var context = TestDbContextFactory.Create();
        context.SavedLocations.Add(new SavedLocation
        {
            UserId = "user-1", LocationName = "New York", Latitude = 40.7m, Longitude = -74.0m
        });
        context.SavedLocations.Add(new SavedLocation
        {
            UserId = "user-1", LocationName = "London", Latitude = 51.5m, Longitude = -0.1m
        });
        await context.SaveChangesAsync();

        var service = CreateService(context);
        var result = await service.GetSavedLocationsAsync("user-1");

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetSavedLocationsAsync_ReturnsEmptyForNewUser()
    {
        using var context = TestDbContextFactory.Create();
        var service = CreateService(context);

        var result = await service.GetSavedLocationsAsync("new-user");

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetSavedLocationsAsync_DoesNotReturnOtherUsersLocations()
    {
        using var context = TestDbContextFactory.Create();
        context.SavedLocations.Add(new SavedLocation { UserId = "user-1", LocationName = "NYC", Latitude = 40.7m, Longitude = -74.0m });
        context.SavedLocations.Add(new SavedLocation { UserId = "user-2", LocationName = "London", Latitude = 51.5m, Longitude = -0.1m });
        await context.SaveChangesAsync();

        var service = CreateService(context);
        var result = await service.GetSavedLocationsAsync("user-1");

        result.Should().HaveCount(1);
        result[0].LocationName.Should().Be("NYC");
    }

    [Fact]
    public async Task AddSavedLocationAsync_CreatesAndReturnsDto()
    {
        using var context = TestDbContextFactory.Create();
        var service = CreateService(context);
        var request = new CreateLocationRequest("Tokyo", 35.6m, 139.6m, false);

        var result = await service.AddSavedLocationAsync("user-1", request);

        result.Should().NotBeNull();
        result!.LocationName.Should().Be("Tokyo");
        result.Latitude.Should().Be(35.6m);
        result.Id.Should().BeGreaterThan(0);
        context.SavedLocations.Should().HaveCount(1);
    }

    [Fact]
    public async Task UpdateSavedLocationAsync_UpdatesExisting()
    {
        using var context = TestDbContextFactory.Create();
        context.SavedLocations.Add(new SavedLocation
        {
            UserId = "user-1", LocationName = "NYC", Latitude = 40.7m, Longitude = -74.0m
        });
        await context.SaveChangesAsync();
        var locationId = context.SavedLocations.First().Id;

        var service = CreateService(context);
        var result = await service.UpdateSavedLocationAsync("user-1", locationId,
            new UpdateLocationRequest("New York City", true));

        result.Should().BeTrue();
        var updated = await context.SavedLocations.FindAsync(locationId);
        updated!.LocationName.Should().Be("New York City");
        updated.IsDefault.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateSavedLocationAsync_ReturnsFalseForWrongUser()
    {
        using var context = TestDbContextFactory.Create();
        context.SavedLocations.Add(new SavedLocation
        {
            UserId = "user-1", LocationName = "NYC", Latitude = 40.7m, Longitude = -74.0m
        });
        await context.SaveChangesAsync();
        var locationId = context.SavedLocations.First().Id;

        var service = CreateService(context);
        var result = await service.UpdateSavedLocationAsync("user-2", locationId,
            new UpdateLocationRequest("Hacked", true));

        result.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateSavedLocationAsync_ReturnsFalseForNonExistent()
    {
        using var context = TestDbContextFactory.Create();
        var service = CreateService(context);

        var result = await service.UpdateSavedLocationAsync("user-1", 999,
            new UpdateLocationRequest("Nothing", false));

        result.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteSavedLocationAsync_RemovesLocation()
    {
        using var context = TestDbContextFactory.Create();
        context.SavedLocations.Add(new SavedLocation
        {
            UserId = "user-1", LocationName = "NYC", Latitude = 40.7m, Longitude = -74.0m
        });
        await context.SaveChangesAsync();
        var locationId = context.SavedLocations.First().Id;

        var service = CreateService(context);
        var result = await service.DeleteSavedLocationAsync("user-1", locationId);

        result.Should().BeTrue();
        context.SavedLocations.Should().BeEmpty();
    }

    [Fact]
    public async Task DeleteSavedLocationAsync_ReturnsFalseForNonExistent()
    {
        using var context = TestDbContextFactory.Create();
        var service = CreateService(context);

        var result = await service.DeleteSavedLocationAsync("user-1", 999);

        result.Should().BeFalse();
    }

    // --- Weather Alerts CRUD Tests ---

    [Fact]
    public async Task GetWeatherAlertsAsync_ReturnsUserAlerts()
    {
        using var context = TestDbContextFactory.Create();
        var location = new SavedLocation { UserId = "user-1", LocationName = "NYC", Latitude = 40.7m, Longitude = -74.0m };
        context.SavedLocations.Add(location);
        await context.SaveChangesAsync();

        context.WeatherAlerts.Add(new WeatherAlert
        {
            UserId = "user-1", SavedLocationId = location.Id, AlertType = "Temperature", Condition = "Above", Threshold = 35.0m
        });
        await context.SaveChangesAsync();

        var service = CreateService(context);
        var result = await service.GetWeatherAlertsAsync("user-1");

        result.Should().HaveCount(1);
        result[0].AlertType.Should().Be("Temperature");
    }

    [Fact]
    public async Task CreateWeatherAlertAsync_CreatesAndReturnsDto()
    {
        using var context = TestDbContextFactory.Create();
        var location = new SavedLocation { UserId = "user-1", LocationName = "NYC", Latitude = 40.7m, Longitude = -74.0m };
        context.SavedLocations.Add(location);
        await context.SaveChangesAsync();

        var service = CreateService(context);
        var request = new CreateWeatherAlertRequest(location.Id, "WindSpeed", "Above", 50.0m);

        var result = await service.CreateWeatherAlertAsync("user-1", request);

        result.Should().NotBeNull();
        result!.AlertType.Should().Be("WindSpeed");
        result.Threshold.Should().Be(50.0m);
        result.Id.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task UpdateWeatherAlertAsync_UpdatesExisting()
    {
        using var context = TestDbContextFactory.Create();
        var location = new SavedLocation { UserId = "user-1", LocationName = "NYC", Latitude = 40.7m, Longitude = -74.0m };
        context.SavedLocations.Add(location);
        await context.SaveChangesAsync();

        context.WeatherAlerts.Add(new WeatherAlert
        {
            UserId = "user-1", SavedLocationId = location.Id, AlertType = "Temperature", Condition = "Above", Threshold = 30.0m
        });
        await context.SaveChangesAsync();
        var alertId = context.WeatherAlerts.First().Id;

        var service = CreateService(context);
        var result = await service.UpdateWeatherAlertAsync("user-1", alertId,
            new UpdateWeatherAlertRequest("Humidity", "Below", 20.0m, false));

        result.Should().BeTrue();
        var updated = await context.WeatherAlerts.FindAsync(alertId);
        updated!.AlertType.Should().Be("Humidity");
        updated.IsEnabled.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateWeatherAlertAsync_ReturnsFalseForWrongUser()
    {
        using var context = TestDbContextFactory.Create();
        var location = new SavedLocation { UserId = "user-1", LocationName = "NYC", Latitude = 40.7m, Longitude = -74.0m };
        context.SavedLocations.Add(location);
        await context.SaveChangesAsync();

        context.WeatherAlerts.Add(new WeatherAlert
        {
            UserId = "user-1", SavedLocationId = location.Id, AlertType = "Temperature", Condition = "Above", Threshold = 30.0m
        });
        await context.SaveChangesAsync();
        var alertId = context.WeatherAlerts.First().Id;

        var service = CreateService(context);
        var result = await service.UpdateWeatherAlertAsync("user-2", alertId,
            new UpdateWeatherAlertRequest("Hacked", "Above", 0m, true));

        result.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteWeatherAlertAsync_RemovesAlert()
    {
        using var context = TestDbContextFactory.Create();
        var location = new SavedLocation { UserId = "user-1", LocationName = "NYC", Latitude = 40.7m, Longitude = -74.0m };
        context.SavedLocations.Add(location);
        await context.SaveChangesAsync();

        context.WeatherAlerts.Add(new WeatherAlert
        {
            UserId = "user-1", SavedLocationId = location.Id, AlertType = "Temperature", Condition = "Above", Threshold = 30.0m
        });
        await context.SaveChangesAsync();
        var alertId = context.WeatherAlerts.First().Id;

        var service = CreateService(context);
        var result = await service.DeleteWeatherAlertAsync("user-1", alertId);

        result.Should().BeTrue();
        context.WeatherAlerts.Should().BeEmpty();
    }

    [Fact]
    public async Task DeleteWeatherAlertAsync_ReturnsFalseForNonExistent()
    {
        using var context = TestDbContextFactory.Create();
        var service = CreateService(context);

        var result = await service.DeleteWeatherAlertAsync("user-1", 999);

        result.Should().BeFalse();
    }
}
