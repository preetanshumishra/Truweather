namespace TruweatherAPI.Tests.Unit.Services;

public class PreferencesServiceTests
{
    private PreferencesService CreateService(TruweatherDbContext context)
    {
        return new PreferencesService(context);
    }

    [Fact]
    public async Task GetPreferencesAsync_WhenExists_ReturnsDto()
    {
        using var context = TestDbContextFactory.Create();
        var userId = "user-1";
        context.UserPreferences.Add(new UserPreferences
        {
            UserId = userId,
            TemperatureUnit = "Fahrenheit",
            WindSpeedUnit = "mph",
            EnableNotifications = false,
            EnableEmailAlerts = false,
            Theme = "Dark",
            Language = "es",
            UpdateFrequencyMinutes = 15,
            CreatedAt = DateTime.UtcNow
        });
        await context.SaveChangesAsync();

        var service = CreateService(context);
        var result = await service.GetPreferencesAsync(userId);

        result.Should().NotBeNull();
        result!.TemperatureUnit.Should().Be("Fahrenheit");
        result.WindSpeedUnit.Should().Be("mph");
        result.EnableNotifications.Should().BeFalse();
        result.Theme.Should().Be("Dark");
        result.Language.Should().Be("es");
        result.UpdateFrequencyMinutes.Should().Be(15);
    }

    [Fact]
    public async Task GetPreferencesAsync_WhenNotExists_CreatesDefaults()
    {
        using var context = TestDbContextFactory.Create();
        var service = CreateService(context);

        var result = await service.GetPreferencesAsync("new-user");

        result.Should().NotBeNull();
        context.UserPreferences.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetPreferencesAsync_DefaultValues_AreCorrect()
    {
        using var context = TestDbContextFactory.Create();
        var service = CreateService(context);

        var result = await service.GetPreferencesAsync("new-user");

        result.Should().NotBeNull();
        result!.TemperatureUnit.Should().Be("Celsius");
        result.WindSpeedUnit.Should().Be("ms");
        result.EnableNotifications.Should().BeTrue();
        result.EnableEmailAlerts.Should().BeTrue();
        result.Theme.Should().Be("Light");
        result.Language.Should().Be("en");
        result.UpdateFrequencyMinutes.Should().Be(30);
    }

    [Fact]
    public async Task UpdatePreferencesAsync_UpdatesAllFields()
    {
        using var context = TestDbContextFactory.Create();
        var userId = "user-1";
        context.UserPreferences.Add(new UserPreferences
        {
            UserId = userId,
            TemperatureUnit = "Celsius",
            WindSpeedUnit = "ms",
            EnableNotifications = true,
            EnableEmailAlerts = true,
            Theme = "Light",
            Language = "en",
            UpdateFrequencyMinutes = 30,
            CreatedAt = DateTime.UtcNow
        });
        await context.SaveChangesAsync();

        var service = CreateService(context);
        var request = new UpdatePreferencesRequest(
            TemperatureUnit: "Fahrenheit",
            WindSpeedUnit: "mph",
            EnableNotifications: false,
            EnableEmailAlerts: false,
            Theme: "Dark",
            Language: "de",
            UpdateFrequencyMinutes: 60
        );

        var result = await service.UpdatePreferencesAsync(userId, request);

        result.Should().BeTrue();
        var updated = await context.UserPreferences.FirstAsync(p => p.UserId == userId);
        updated.TemperatureUnit.Should().Be("Fahrenheit");
        updated.WindSpeedUnit.Should().Be("mph");
        updated.EnableNotifications.Should().BeFalse();
        updated.Theme.Should().Be("Dark");
        updated.Language.Should().Be("de");
        updated.UpdateFrequencyMinutes.Should().Be(60);
    }

    [Fact]
    public async Task UpdatePreferencesAsync_PartialUpdate_OnlyChangesProvided()
    {
        using var context = TestDbContextFactory.Create();
        var userId = "user-1";
        context.UserPreferences.Add(new UserPreferences
        {
            UserId = userId,
            TemperatureUnit = "Celsius",
            WindSpeedUnit = "ms",
            EnableNotifications = true,
            EnableEmailAlerts = true,
            Theme = "Light",
            Language = "en",
            UpdateFrequencyMinutes = 30,
            CreatedAt = DateTime.UtcNow
        });
        await context.SaveChangesAsync();

        var service = CreateService(context);
        var request = new UpdatePreferencesRequest(
            TemperatureUnit: "Kelvin",
            WindSpeedUnit: null,
            EnableNotifications: null,
            EnableEmailAlerts: null,
            Theme: null,
            Language: null,
            UpdateFrequencyMinutes: null
        );

        await service.UpdatePreferencesAsync(userId, request);

        var updated = await context.UserPreferences.FirstAsync(p => p.UserId == userId);
        updated.TemperatureUnit.Should().Be("Kelvin");
        updated.WindSpeedUnit.Should().Be("ms");
        updated.EnableNotifications.Should().BeTrue();
        updated.Theme.Should().Be("Light");
        updated.Language.Should().Be("en");
    }

    [Fact]
    public async Task UpdatePreferencesAsync_WhenNotExists_CreatesAndUpdates()
    {
        using var context = TestDbContextFactory.Create();
        var service = CreateService(context);
        var request = new UpdatePreferencesRequest(
            TemperatureUnit: "Fahrenheit",
            WindSpeedUnit: null,
            EnableNotifications: null,
            EnableEmailAlerts: null,
            Theme: "Dark",
            Language: null,
            UpdateFrequencyMinutes: null
        );

        var result = await service.UpdatePreferencesAsync("new-user", request);

        result.Should().BeTrue();
        var prefs = await context.UserPreferences.FirstAsync(p => p.UserId == "new-user");
        prefs.TemperatureUnit.Should().Be("Fahrenheit");
        prefs.Theme.Should().Be("Dark");
    }

    [Fact]
    public async Task UpdatePreferencesAsync_SetsUpdatedAtTimestamp()
    {
        using var context = TestDbContextFactory.Create();
        var userId = "user-1";
        context.UserPreferences.Add(new UserPreferences
        {
            UserId = userId,
            TemperatureUnit = "Celsius",
            WindSpeedUnit = "ms",
            CreatedAt = DateTime.UtcNow.AddDays(-1)
        });
        await context.SaveChangesAsync();

        var before = DateTime.UtcNow;
        var service = CreateService(context);
        await service.UpdatePreferencesAsync(userId, new UpdatePreferencesRequest(
            TemperatureUnit: "Kelvin",
            WindSpeedUnit: null, EnableNotifications: null, EnableEmailAlerts: null,
            Theme: null, Language: null, UpdateFrequencyMinutes: null
        ));

        var updated = await context.UserPreferences.FirstAsync(p => p.UserId == userId);
        updated.UpdatedAt.Should().BeOnOrAfter(before);
    }
}
