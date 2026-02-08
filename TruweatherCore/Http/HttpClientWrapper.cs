using System.Net.Http.Json;
using TruweatherCore.Exceptions;

namespace TruweatherCore.Http;

/// <summary>
/// Wrapper around HttpClient for standardized API communication across Web and Mobile.
/// Handles common concerns: error handling, response deserialization, header management.
/// </summary>
public class HttpClientWrapper
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public HttpClientWrapper(HttpClient httpClient, string baseUrl)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _baseUrl = baseUrl?.TrimEnd('/') ?? throw new ArgumentNullException(nameof(baseUrl));
        _httpClient.BaseAddress = new Uri(_baseUrl);
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }

    /// <summary>
    /// Set the JWT bearer token for authenticated requests.
    /// </summary>
    public void SetBearerToken(string? token)
    {
        if (string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
        }
        else
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
    }

    /// <summary>
    /// Perform a GET request and deserialize response.
    /// </summary>
    public async Task<T> GetAsync<T>(string endpoint)
    {
        try
        {
            var response = await _httpClient.GetAsync(endpoint);
            return await HandleResponse<T>(response);
        }
        catch (HttpRequestException ex)
        {
            throw new ApiException("Network error during GET request", ex);
        }
        catch (TaskCanceledException ex)
        {
            throw new ApiException("Request timeout", ex);
        }
    }

    /// <summary>
    /// Perform a POST request with JSON body and deserialize response.
    /// </summary>
    public async Task<T> PostAsync<T>(string endpoint, object? body = null)
    {
        try
        {
            HttpContent? content = body != null
                ? JsonContent.Create(body)
                : null;

            var response = await _httpClient.PostAsync(endpoint, content);
            return await HandleResponse<T>(response);
        }
        catch (HttpRequestException ex)
        {
            throw new ApiException("Network error during POST request", ex);
        }
        catch (TaskCanceledException ex)
        {
            throw new ApiException("Request timeout", ex);
        }
    }

    /// <summary>
    /// Perform a PUT request with JSON body and deserialize response.
    /// </summary>
    public async Task<T> PutAsync<T>(string endpoint, object? body = null)
    {
        try
        {
            HttpContent? content = body != null
                ? JsonContent.Create(body)
                : null;

            var response = await _httpClient.PutAsync(endpoint, content);
            return await HandleResponse<T>(response);
        }
        catch (HttpRequestException ex)
        {
            throw new ApiException("Network error during PUT request", ex);
        }
        catch (TaskCanceledException ex)
        {
            throw new ApiException("Request timeout", ex);
        }
    }

    /// <summary>
    /// Perform a DELETE request.
    /// </summary>
    public async Task<bool> DeleteAsync(string endpoint)
    {
        try
        {
            var response = await _httpClient.DeleteAsync(endpoint);
            if (response.IsSuccessStatusCode)
                return true;

            var content = await response.Content.ReadAsStringAsync();
            throw new ApiException(
                $"DELETE request failed: {response.StatusCode}",
                (int)response.StatusCode,
                content);
        }
        catch (HttpRequestException ex)
        {
            throw new ApiException("Network error during DELETE request", ex);
        }
        catch (TaskCanceledException ex)
        {
            throw new ApiException("Request timeout", ex);
        }
    }

    /// <summary>
    /// Get raw string response (useful for non-JSON responses).
    /// </summary>
    public async Task<string> GetStringAsync(string endpoint)
    {
        try
        {
            return await _httpClient.GetStringAsync(endpoint);
        }
        catch (HttpRequestException ex)
        {
            throw new ApiException("Network error during GET request", ex);
        }
        catch (TaskCanceledException ex)
        {
            throw new ApiException("Request timeout", ex);
        }
    }

    /// <summary>
    /// Handle HTTP response and deserialize or throw appropriate exception.
    /// </summary>
    private async Task<T> HandleResponse<T>(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            try
            {
                var content = await response.Content.ReadAsAsync<T>();
                return content ?? throw new ApiException("Response body was null");
            }
            catch (HttpRequestException ex)
            {
                throw new ApiException("Failed to deserialize response", ex);
            }
        }

        var errorContent = await response.Content.ReadAsStringAsync();

        throw response.StatusCode switch
        {
            System.Net.HttpStatusCode.BadRequest =>
                new ApiException("Invalid request", (int)response.StatusCode, errorContent),
            System.Net.HttpStatusCode.Unauthorized =>
                new ApiException("Unauthorized - please login", (int)response.StatusCode, errorContent),
            System.Net.HttpStatusCode.Forbidden =>
                new ApiException("Forbidden - access denied", (int)response.StatusCode, errorContent),
            System.Net.HttpStatusCode.NotFound =>
                new ApiException("Resource not found", (int)response.StatusCode, errorContent),
            System.Net.HttpStatusCode.Conflict =>
                new ApiException("Conflict - resource already exists", (int)response.StatusCode, errorContent),
            System.Net.HttpStatusCode.InternalServerError =>
                new ApiException("Server error", (int)response.StatusCode, errorContent),
            System.Net.HttpStatusCode.ServiceUnavailable =>
                new ApiException("Service unavailable", (int)response.StatusCode, errorContent),
            _ => new ApiException(
                $"HTTP {(int)response.StatusCode}: {response.ReasonPhrase}",
                (int)response.StatusCode,
                errorContent)
        };
    }
}
