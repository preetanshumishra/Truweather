using TruweatherCore.Http;

namespace TruweatherMobile.Services;

public class SecureTokenStorage : ITokenStorage
{
    private const string AccessTokenKey = "access_token";
    private const string RefreshTokenKey = "refresh_token";

    public async Task SaveTokensAsync(string accessToken, string refreshToken)
    {
        await SecureStorage.Default.SetAsync(AccessTokenKey, accessToken);
        await SecureStorage.Default.SetAsync(RefreshTokenKey, refreshToken);
    }

    public Task<string?> GetAccessTokenAsync()
    {
        return SecureStorage.Default.GetAsync(AccessTokenKey);
    }

    public Task<string?> GetRefreshTokenAsync()
    {
        return SecureStorage.Default.GetAsync(RefreshTokenKey);
    }

    public Task ClearTokensAsync()
    {
        SecureStorage.Default.Remove(AccessTokenKey);
        SecureStorage.Default.Remove(RefreshTokenKey);
        return Task.CompletedTask;
    }

    public async Task<bool> HasTokensAsync()
    {
        var token = await SecureStorage.Default.GetAsync(AccessTokenKey);
        return !string.IsNullOrEmpty(token);
    }
}
