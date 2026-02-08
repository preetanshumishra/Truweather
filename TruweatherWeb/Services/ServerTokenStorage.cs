using TruweatherCore.Http;

namespace TruweatherWeb.Services;

/// <summary>
/// In-memory token storage for Blazor Server circuits.
/// Registered as scoped — each circuit (user session) gets its own instance.
/// </summary>
public class ServerTokenStorage : ITokenStorage
{
    private string? _accessToken;
    private string? _refreshToken;

    public Task SaveTokensAsync(string accessToken, string refreshToken)
    {
        _accessToken = accessToken;
        _refreshToken = refreshToken;
        return Task.CompletedTask;
    }

    public Task<string?> GetAccessTokenAsync() => Task.FromResult(_accessToken);

    public Task<string?> GetRefreshTokenAsync() => Task.FromResult(_refreshToken);

    public Task ClearTokensAsync()
    {
        _accessToken = null;
        _refreshToken = null;
        return Task.CompletedTask;
    }

    public Task<bool> HasTokensAsync() =>
        Task.FromResult(!string.IsNullOrEmpty(_accessToken));
}
