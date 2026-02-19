using System.Net;
using System.Net.Http.Json;

namespace TruweatherAPI.Tests.Integration;

public class AdminEndpointTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AdminEndpointTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    private async Task<string> GetUserTokenAsync()
    {
        var email = $"admin-test-{Guid.NewGuid():N}@test.com";
        var register = new RegisterRequest
        {
            Email = email, Password = "Password123!",
            ConfirmPassword = "Password123!", FullName = "Test User"
        };
        await _client.PostAsJsonAsync("/api/auth/register", register);

        var login = new LoginRequest(email, "Password123!");
        var response = await _client.PostAsJsonAsync("/api/auth/login", login);
        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
        return result!.AccessToken!;
    }

    [Fact]
    public async Task GetUsers_WithoutAuth_Returns401()
    {
        _client.DefaultRequestHeaders.Authorization = null;
        var response = await _client.GetAsync("/api/admin/users");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetUsers_WithNonAdminUser_Returns403()
    {
        var token = await GetUserTokenAsync();
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/admin/users");

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task GetStats_WithNonAdminUser_Returns403()
    {
        var token = await GetUserTokenAsync();
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/admin/stats");

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task GetAlerts_WithNonAdminUser_Returns403()
    {
        var token = await GetUserTokenAsync();
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/admin/alerts");

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task DeleteUser_WithNonAdminUser_Returns403()
    {
        var token = await GetUserTokenAsync();
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync("/api/admin/users/some-user-id");

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task UpdateRole_WithNonAdminUser_Returns403()
    {
        var token = await GetUserTokenAsync();
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PutAsJsonAsync(
            "/api/admin/users/some-user-id/role",
            new UpdateRoleRequest("Admin"));

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task GetUserDetail_WithNonAdminUser_Returns403()
    {
        var token = await GetUserTokenAsync();
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/admin/users/some-user-id");

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
