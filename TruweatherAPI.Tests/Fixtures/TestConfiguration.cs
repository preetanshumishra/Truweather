namespace TruweatherAPI.Tests.Fixtures;

public static class TestConfiguration
{
    public static IConfiguration Create()
    {
        var config = new Dictionary<string, string?>
        {
            ["Jwt:Secret"] = "test-secret-key-that-is-at-least-32-chars-long-for-hmac256!!",
            ["Jwt:Issuer"] = "https://test.truweather.com",
            ["Jwt:Audience"] = "truweather-test",
            ["Jwt:ExpirationMinutes"] = "60",
            ["Cache:WeatherTtlMinutes"] = "60",
            ["Cache:ForecastTtlMinutes"] = "60"
        };

        return new ConfigurationBuilder()
            .AddInMemoryCollection(config)
            .Build();
    }
}
