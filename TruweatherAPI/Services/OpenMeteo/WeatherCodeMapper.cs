namespace TruweatherAPI.Services.OpenMeteo;

/// <summary>
/// Maps Open-Meteo WMO weather codes to human-readable conditions.
/// Reference: https://www.weatherapi.com/docs/weather_codes.asp
/// </summary>
public static class WeatherCodeMapper
{
    private static readonly Dictionary<int, (string Condition, string Description)> CodeMap = new()
    {
        // Clear sky
        { 0, ("Clear Sky", "Clear sky") },
        { 1, ("Partly Cloudy", "Mainly clear") },
        { 2, ("Partly Cloudy", "Partly cloudy") },
        { 3, ("Overcast", "Overcast") },

        // Fog
        { 45, ("Fog", "Foggy") },
        { 48, ("Fog", "Depositing rime fog") },

        // Drizzle
        { 51, ("Light Drizzle", "Light drizzle") },
        { 53, ("Moderate Drizzle", "Moderate drizzle") },
        { 55, ("Heavy Drizzle", "Dense drizzle") },

        // Rain
        { 61, ("Light Rain", "Slight rain") },
        { 63, ("Moderate Rain", "Moderate rain") },
        { 65, ("Heavy Rain", "Heavy rain") },
        { 71, ("Light Snow", "Slight snow") },
        { 73, ("Moderate Snow", "Moderate snow") },
        { 75, ("Heavy Snow", "Heavy snow") },
        { 77, ("Snow Grains", "Snow grains") },

        // Rain and snow mixed
        { 80, ("Light Rain Showers", "Slight rain showers") },
        { 81, ("Moderate Rain Showers", "Moderate rain showers") },
        { 82, ("Heavy Rain Showers", "Violent rain showers") },
        { 85, ("Light Snow Showers", "Slight snow showers") },
        { 86, ("Heavy Snow Showers", "Heavy snow showers") },

        // Thunderstorm
        { 80, ("Thunderstorm", "Thunderstorm with slight hail") },
        { 81, ("Thunderstorm", "Thunderstorm with moderate hail") },
        { 82, ("Thunderstorm", "Thunderstorm with heavy hail") },
        { 95, ("Thunderstorm", "Thunderstorm with slight hail") },
        { 96, ("Thunderstorm", "Thunderstorm with moderate hail") },
        { 99, ("Thunderstorm", "Thunderstorm with heavy hail") },
    };

    /// <summary>
    /// Get condition and description from WMO weather code.
    /// </summary>
    public static (string Condition, string Description) GetWeatherDescription(int weatherCode)
    {
        return CodeMap.TryGetValue(weatherCode, out var result)
            ? result
            : ("Unknown", "Unknown weather condition");
    }

    /// <summary>
    /// Get weather icon URL based on WMO code (uses placeholder for now).
    /// </summary>
    public static string GetIconUrl(int weatherCode)
    {
        // Map weather codes to emoji or icon names
        return weatherCode switch
        {
            0 => "☀️",          // Clear
            1 or 2 => "⛅",     // Partly cloudy
            3 => "☁️",          // Overcast
            45 or 48 => "🌫️",  // Fog
            51 or 53 or 55 => "🌧️", // Drizzle
            61 or 63 or 65 => "🌧️", // Rain
            71 or 73 or 75 or 77 => "❄️", // Snow
            80 or 81 or 82 => "⛈️", // Rain showers
            85 or 86 => "🌨️",  // Snow showers
            95 or 96 or 99 => "⛈️", // Thunderstorm
            _ => "❓"
        };
    }
}
