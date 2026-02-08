using System.Net;
using System.Net.Http.Json;
using System.Net.Http.Headers;

namespace TruweatherAPI.Tests.Integration;

public class PreferencesEndpointTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public PreferencesEndpointTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    private async Task<string> GetAuthTokenAsync()
    {
        var email = $"prefs-{Guid.NewGuid():N}@test.com";
        var registerRequest = new RegisterRequest { Email = email, Password = "Password123!", ConfirmPassword = "Password123!", FullName = "Test User" };
        await _client.PostAsJsonAsync("/api/auth/register", registerRequest);

        var loginRequest = new LoginRequest(email, "Password123!");
        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);
        var result = await loginResponse.Content.ReadFromJsonAsync<AuthResponse>();
        return result!.AccessToken!;
    }

    [Fact]
    public async Task GetPreferences_WithoutAuth_Returns401()
    {
        _client.DefaultRequestHeaders.Authorization = null;
        var response = await _client.GetAsync("/api/preferences");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetPreferences_NewUser_Returns200WithDefaults()
    {
        var token = await GetAuthTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/preferences");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var prefs = await response.Content.ReadFromJsonAsync<UserPreferencesDto>();
        prefs!.TemperatureUnit.Should().Be("Celsius");
        prefs.Language.Should().Be("en");
    }

    [Fact]
    public async Task UpdatePreferences_Returns204()
    {
        var token = await GetAuthTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var request = new UpdatePreferencesRequest(
            TemperatureUnit: "Fahrenheit",
            WindSpeedUnit: null,
            EnableNotifications: null,
            EnableEmailAlerts: null,
            Theme: "Dark",
            Language: null,
            UpdateFrequencyMinutes: null
        );
        var response = await _client.PutAsJsonAsync("/api/preferences", request);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task UpdateThenGet_ReflectsChanges()
    {
        var token = await GetAuthTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var updateRequest = new UpdatePreferencesRequest(
            TemperatureUnit: "Kelvin",
            WindSpeedUnit: "knots",
            EnableNotifications: false,
            EnableEmailAlerts: null,
            Theme: "Dark",
            Language: "ja",
            UpdateFrequencyMinutes: 120
        );
        await _client.PutAsJsonAsync("/api/preferences", updateRequest);

        var getResponse = await _client.GetAsync("/api/preferences");
        var prefs = await getResponse.Content.ReadFromJsonAsync<UserPreferencesDto>();

        prefs!.TemperatureUnit.Should().Be("Kelvin");
        prefs.WindSpeedUnit.Should().Be("knots");
        prefs.EnableNotifications.Should().BeFalse();
        prefs.Theme.Should().Be("Dark");
        prefs.Language.Should().Be("ja");
        prefs.UpdateFrequencyMinutes.Should().Be(120);
    }
}
