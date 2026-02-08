using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using TruweatherCore.Http;

namespace TruweatherWeb.Services;

public class TruweatherAuthStateProvider : AuthenticationStateProvider
{
    private readonly ITokenStorage _tokenStorage;

    public TruweatherAuthStateProvider(ITokenStorage tokenStorage)
    {
        _tokenStorage = tokenStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _tokenStorage.GetAccessTokenAsync();

        if (string.IsNullOrEmpty(token))
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        var claims = ParseClaimsFromJwt(token);
        var identity = new ClaimsIdentity(claims, "jwt");
        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    public void NotifyAuthStateChanged()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var parts = jwt.Split('.');
        if (parts.Length != 3)
            return [];

        var payload = parts[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);

        var claims = new List<Claim>();

        try
        {
            using var doc = JsonDocument.Parse(jsonBytes);
            foreach (var property in doc.RootElement.EnumerateObject())
            {
                var claimType = property.Name switch
                {
                    "sub" => ClaimTypes.NameIdentifier,
                    "email" => ClaimTypes.Email,
                    "name" => ClaimTypes.Name,
                    "role" => ClaimTypes.Role,
                    _ => property.Name
                };

                if (property.Value.ValueKind == JsonValueKind.Array)
                {
                    foreach (var item in property.Value.EnumerateArray())
                        claims.Add(new Claim(claimType, item.GetString() ?? ""));
                }
                else
                {
                    claims.Add(new Claim(claimType, property.Value.ToString()));
                }
            }
        }
        catch
        {
            // Invalid JWT payload — return empty claims
        }

        return claims;
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        base64 = base64.Replace('-', '+').Replace('_', '/');
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }
}
