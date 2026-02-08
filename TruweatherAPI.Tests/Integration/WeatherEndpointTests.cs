using System.Net;
using System.Net.Http.Json;
using System.Net.Http.Headers;

namespace TruweatherAPI.Tests.Integration;

public class WeatherEndpointTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public WeatherEndpointTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    private async Task<string> GetAuthTokenAsync()
    {
        var email = $"weather-{Guid.NewGuid():N}@test.com";
        var registerRequest = new RegisterRequest { Email = email, Password = "Password123!", ConfirmPassword = "Password123!", FullName = "Test User" };
        await _client.PostAsJsonAsync("/api/auth/register", registerRequest);

        var loginRequest = new LoginRequest(email, "Password123!");
        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);
        var result = await loginResponse.Content.ReadFromJsonAsync<AuthResponse>();
        return result!.AccessToken!;
    }

    private void SetAuth(string token)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    [Fact]
    public async Task GetCurrentWeather_WithoutAuth_Returns401()
    {
        _client.DefaultRequestHeaders.Authorization = null;
        var response = await _client.GetAsync("/api/weather/current?latitude=40.7&longitude=-74.0");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetCurrentWeather_WithAuth_ReturnsSuccess()
    {
        var token = await GetAuthTokenAsync();
        SetAuth(token);

        var response = await _client.GetAsync("/api/weather/current?latitude=40.7&longitude=-74.0");

        // May return 200 with data or 404 if Open-Meteo is unreachable
        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetForecast_WithAuth_ReturnsSuccess()
    {
        var token = await GetAuthTokenAsync();
        SetAuth(token);

        var response = await _client.GetAsync("/api/weather/forecast?latitude=40.7&longitude=-74.0");

        // Forecast always returns data (has mock fallback)
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetLocations_Empty_Returns200WithEmptyArray()
    {
        var token = await GetAuthTokenAsync();
        SetAuth(token);

        var response = await _client.GetAsync("/api/weather/locations");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var locations = await response.Content.ReadFromJsonAsync<List<SavedLocationDto>>();
        locations.Should().BeEmpty();
    }

    [Fact]
    public async Task AddLocation_Returns201()
    {
        var token = await GetAuthTokenAsync();
        SetAuth(token);

        var request = new CreateLocationRequest("New York", 40.7128m, -74.0060m, true);
        var response = await _client.PostAsJsonAsync("/api/weather/locations", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var location = await response.Content.ReadFromJsonAsync<SavedLocationDto>();
        location!.LocationName.Should().Be("New York");
    }

    [Fact]
    public async Task AddAndGetLocations_ReturnsCreatedLocation()
    {
        var token = await GetAuthTokenAsync();
        SetAuth(token);

        var request = new CreateLocationRequest("Tokyo", 35.6762m, 139.6503m, false);
        await _client.PostAsJsonAsync("/api/weather/locations", request);

        var getResponse = await _client.GetAsync("/api/weather/locations");
        var locations = await getResponse.Content.ReadFromJsonAsync<List<SavedLocationDto>>();

        locations.Should().HaveCount(1);
        locations![0].LocationName.Should().Be("Tokyo");
    }

    [Fact]
    public async Task UpdateLocation_Returns204()
    {
        var token = await GetAuthTokenAsync();
        SetAuth(token);

        var createRequest = new CreateLocationRequest("NYC", 40.7m, -74.0m, false);
        var createResponse = await _client.PostAsJsonAsync("/api/weather/locations", createRequest);
        var created = await createResponse.Content.ReadFromJsonAsync<SavedLocationDto>();

        var updateRequest = new UpdateLocationRequest("New York City", true);
        var updateResponse = await _client.PutAsJsonAsync($"/api/weather/locations/{created!.Id}", updateRequest);

        updateResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task UpdateLocation_NonExistent_Returns404()
    {
        var token = await GetAuthTokenAsync();
        SetAuth(token);

        var updateRequest = new UpdateLocationRequest("Nothing", false);
        var response = await _client.PutAsJsonAsync("/api/weather/locations/99999", updateRequest);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteLocation_Returns204()
    {
        var token = await GetAuthTokenAsync();
        SetAuth(token);

        var createRequest = new CreateLocationRequest("Delete Me", 10.0m, 20.0m, false);
        var createResponse = await _client.PostAsJsonAsync("/api/weather/locations", createRequest);
        var created = await createResponse.Content.ReadFromJsonAsync<SavedLocationDto>();

        var deleteResponse = await _client.DeleteAsync($"/api/weather/locations/{created!.Id}");

        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteLocation_NonExistent_Returns404()
    {
        var token = await GetAuthTokenAsync();
        SetAuth(token);

        var response = await _client.DeleteAsync("/api/weather/locations/99999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateAlert_Returns201()
    {
        var token = await GetAuthTokenAsync();
        SetAuth(token);

        // First create a location
        var locationRequest = new CreateLocationRequest("Alert City", 40.7m, -74.0m, false);
        var locationResponse = await _client.PostAsJsonAsync("/api/weather/locations", locationRequest);
        var location = await locationResponse.Content.ReadFromJsonAsync<SavedLocationDto>();

        var alertRequest = new CreateWeatherAlertRequest(location!.Id, "Temperature", "Above", 35.0m);
        var response = await _client.PostAsJsonAsync("/api/weather/alerts", alertRequest);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task DeleteAlert_Returns204()
    {
        var token = await GetAuthTokenAsync();
        SetAuth(token);

        var locationRequest = new CreateLocationRequest("Alert City 2", 41.0m, -75.0m, false);
        var locationResponse = await _client.PostAsJsonAsync("/api/weather/locations", locationRequest);
        var location = await locationResponse.Content.ReadFromJsonAsync<SavedLocationDto>();

        var alertRequest = new CreateWeatherAlertRequest(location!.Id, "WindSpeed", "Above", 50.0m);
        var alertResponse = await _client.PostAsJsonAsync("/api/weather/alerts", alertRequest);
        var alert = await alertResponse.Content.ReadFromJsonAsync<WeatherAlertDto>();

        var deleteResponse = await _client.DeleteAsync($"/api/weather/alerts/{alert!.Id}");

        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
