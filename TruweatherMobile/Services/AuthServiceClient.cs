using TruweatherCore.Constants;
using TruweatherCore.Http;
using TruweatherCore.Models.DTOs;

namespace TruweatherMobile.Services;

public class AuthServiceClient
{
    private readonly HttpClientWrapper _http;
    private readonly ITokenStorage _tokenStorage;

    public AuthServiceClient(HttpClientWrapper http, ITokenStorage tokenStorage)
    {
        _http = http;
        _tokenStorage = tokenStorage;
    }

    public async Task<AuthResponse> LoginAsync(string email, string password)
    {
        var request = new LoginRequest(email, password);
        var response = await _http.PostAsync<AuthResponse>(ApiEndpoints.AuthLogin, request);

        if (response.Success && response.AccessToken != null && response.RefreshToken != null)
        {
            await _tokenStorage.SaveTokensAsync(response.AccessToken, response.RefreshToken);
            _http.SetBearerToken(response.AccessToken);
        }

        return response;
    }

    public async Task<AuthResponse> RegisterAsync(string email, string password, string confirmPassword, string? fullName)
    {
        var request = new RegisterRequest
        {
            Email = email,
            Password = password,
            ConfirmPassword = confirmPassword,
            FullName = fullName
        };
        return await _http.PostAsync<AuthResponse>(ApiEndpoints.AuthRegister, request);
    }

    public async Task LogoutAsync()
    {
        try
        {
            await _http.PostAsync<AuthResponse>(ApiEndpoints.AuthLogout);
        }
        catch
        {
            // Logout API failure shouldn't block local cleanup
        }

        _http.SetBearerToken(null);
        await _tokenStorage.ClearTokensAsync();
    }

    public async Task<bool> TryRestoreSessionAsync()
    {
        var token = await _tokenStorage.GetAccessTokenAsync();
        if (string.IsNullOrEmpty(token))
            return false;

        _http.SetBearerToken(token);
        return true;
    }
}
