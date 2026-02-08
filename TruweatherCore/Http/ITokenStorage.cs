namespace TruweatherCore.Http;

/// <summary>
/// Interface for storing and retrieving authentication tokens.
/// Web uses localStorage, Mobile uses AsyncStorage - each implements this interface.
/// </summary>
public interface ITokenStorage
{
    /// <summary>
    /// Save access token and refresh token.
    /// </summary>
    Task SaveTokensAsync(string accessToken, string refreshToken);

    /// <summary>
    /// Get the stored access token.
    /// </summary>
    Task<string?> GetAccessTokenAsync();

    /// <summary>
    /// Get the stored refresh token.
    /// </summary>
    Task<string?> GetRefreshTokenAsync();

    /// <summary>
    /// Clear all stored tokens (on logout).
    /// </summary>
    Task ClearTokensAsync();

    /// <summary>
    /// Check if tokens are stored.
    /// </summary>
    Task<bool> HasTokensAsync();
}
