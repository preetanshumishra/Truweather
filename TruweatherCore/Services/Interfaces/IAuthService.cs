using TruweatherCore.Models.DTOs;

namespace TruweatherCore.Services.Interfaces;

/// <summary>
/// Interface for authentication operations shared across API, Web, and Mobile.
/// API implements business logic; Web/Mobile use HttpClient to call API endpoints.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Register a new user account.
    /// </summary>
    Task<AuthResponse> RegisterAsync(RegisterRequest request);

    /// <summary>
    /// Authenticate user with email and password, returns JWT tokens.
    /// </summary>
    Task<AuthResponse> LoginAsync(LoginRequest request);

    /// <summary>
    /// Refresh expired access token using refresh token.
    /// </summary>
    Task<AuthResponse> RefreshTokenAsync(string refreshToken);

    /// <summary>
    /// Logout user and invalidate tokens.
    /// </summary>
    Task<bool> LogoutAsync(string userId);
}
