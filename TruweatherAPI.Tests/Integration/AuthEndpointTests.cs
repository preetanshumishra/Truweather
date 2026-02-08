using System.Net;
using System.Net.Http.Json;

namespace TruweatherAPI.Tests.Integration;

public class AuthEndpointTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthEndpointTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    private async Task<AuthResponse?> RegisterUserAsync(string email = "test@test.com", string password = "Password123!")
    {
        var request = new RegisterRequest { Email = email, Password = password, ConfirmPassword = password, FullName = "Test User" };
        var response = await _client.PostAsJsonAsync("/api/auth/register", request);
        return await response.Content.ReadFromJsonAsync<AuthResponse>();
    }

    private async Task<AuthResponse?> LoginUserAsync(string email = "test@test.com", string password = "Password123!")
    {
        var request = new LoginRequest(email, password);
        var response = await _client.PostAsJsonAsync("/api/auth/login", request);
        return await response.Content.ReadFromJsonAsync<AuthResponse>();
    }

    [Fact]
    public async Task Register_WithValidData_Returns200()
    {
        var request = new RegisterRequest { Email = "newuser@test.com", Password = "Password123!", ConfirmPassword = "Password123!", FullName = "New User" };
        var response = await _client.PostAsJsonAsync("/api/auth/register", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
        result!.Success.Should().BeTrue();
    }

    [Fact]
    public async Task Register_WithMismatchedPasswords_ReturnsBadRequestOrFailure()
    {
        var request = new RegisterRequest { Email = "mismatch@test.com", Password = "Password123!", ConfirmPassword = "Different456!", FullName = "User" };
        var response = await _client.PostAsJsonAsync("/api/auth/register", request);

        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
        result!.Success.Should().BeFalse();
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsTokens()
    {
        var email = $"logintest-{Guid.NewGuid():N}@test.com";
        await RegisterUserAsync(email);

        var loginRequest = new LoginRequest(email, "Password123!");
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
        result!.Success.Should().BeTrue();
        result.AccessToken.Should().NotBeNullOrEmpty();
        result.RefreshToken.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Login_WithNonExistentEmail_ReturnsUnauthorized()
    {
        var request = new LoginRequest("unknown@test.com", "Password123!");
        var response = await _client.PostAsJsonAsync("/api/auth/login", request);

        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
        result!.Success.Should().BeFalse();
    }

    [Fact]
    public async Task Login_WithWrongPassword_ReturnsFailure()
    {
        var email = $"wrongpw-{Guid.NewGuid():N}@test.com";
        await RegisterUserAsync(email);

        var request = new LoginRequest(email, "WrongPassword!");
        var response = await _client.PostAsJsonAsync("/api/auth/login", request);

        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
        result!.Success.Should().BeFalse();
    }

    [Fact]
    public async Task RefreshToken_WithValidToken_ReturnsNewTokens()
    {
        var email = $"refresh-{Guid.NewGuid():N}@test.com";
        await RegisterUserAsync(email);
        var loginResult = await LoginUserAsync(email);

        var refreshRequest = new RefreshTokenRequest(loginResult!.RefreshToken!);
        var response = await _client.PostAsJsonAsync("/api/auth/refresh", refreshRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
        result!.Success.Should().BeTrue();
        result.AccessToken.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Logout_WithValidToken_Returns200()
    {
        var email = $"logout-{Guid.NewGuid():N}@test.com";
        await RegisterUserAsync(email);
        var loginResult = await LoginUserAsync(email);

        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginResult!.AccessToken);

        var response = await _client.PostAsync("/api/auth/logout", null);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Logout_WithoutAuth_Returns401()
    {
        _client.DefaultRequestHeaders.Authorization = null;
        var response = await _client.PostAsync("/api/auth/logout", null);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
