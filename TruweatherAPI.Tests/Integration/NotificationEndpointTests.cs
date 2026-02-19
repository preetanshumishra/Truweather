using System.Net;
using System.Net.Http.Json;

namespace TruweatherAPI.Tests.Integration;

public class NotificationEndpointTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public NotificationEndpointTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    private async Task<string> GetAuthTokenAsync()
    {
        var email = $"notif-{Guid.NewGuid():N}@test.com";
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
    public async Task GetNotifications_WithAuth_Returns200()
    {
        var token = await GetAuthTokenAsync();
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/notification");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var notifications = await response.Content.ReadFromJsonAsync<List<NotificationDto>>();
        notifications.Should().NotBeNull();
    }

    [Fact]
    public async Task GetNotifications_WithoutAuth_Returns401()
    {
        _client.DefaultRequestHeaders.Authorization = null;
        var response = await _client.GetAsync("/api/notification");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetUnreadCount_WithAuth_Returns200()
    {
        var token = await GetAuthTokenAsync();
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/notification/unread-count");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<UnreadCountDto>();
        result.Should().NotBeNull();
        result!.Count.Should().Be(0);
    }

    [Fact]
    public async Task MarkAllAsRead_WithAuth_Returns204()
    {
        var token = await GetAuthTokenAsync();
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PutAsync("/api/notification/read-all", null);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task MarkAsRead_WithNonExistentId_Returns404()
    {
        var token = await GetAuthTokenAsync();
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PutAsync("/api/notification/99999/read", null);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
