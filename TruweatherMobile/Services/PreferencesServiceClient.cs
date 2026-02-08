using TruweatherCore.Http;
using TruweatherCore.Models.DTOs;

namespace TruweatherMobile.Services;

public class PreferencesServiceClient
{
    private readonly HttpClientWrapper _http;

    public PreferencesServiceClient(HttpClientWrapper http)
    {
        _http = http;
    }

    public Task<UserPreferencesDto> GetPreferencesAsync()
    {
        return _http.GetAsync<UserPreferencesDto>("/api/weather/preferences");
    }

    public Task<UserPreferencesDto> UpdatePreferencesAsync(UpdatePreferencesRequest request)
    {
        return _http.PutAsync<UserPreferencesDto>("/api/weather/preferences", request);
    }
}
