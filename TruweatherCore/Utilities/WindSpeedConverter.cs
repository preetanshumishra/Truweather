namespace TruweatherCore.Utilities;

/// <summary>
/// Utility for converting wind speeds between different units.
/// </summary>
public static class WindSpeedConverter
{
    // Conversion factors to m/s
    private const decimal KmhToMs = 1 / 3.6m;
    private const decimal MphToMs = 1 / 2.237m;
    private const decimal KnotsToMs = 1 / 1.944m;

    /// <summary>
    /// Convert meters per second to kilometers per hour.
    /// </summary>
    public static decimal MsToKmh(decimal ms)
    {
        return ms * 3.6m;
    }

    /// <summary>
    /// Convert meters per second to miles per hour.
    /// </summary>
    public static decimal MsToMph(decimal ms)
    {
        return ms * 2.237m;
    }

    /// <summary>
    /// Convert meters per second to knots.
    /// </summary>
    public static decimal MsToKnots(decimal ms)
    {
        return ms * 1.944m;
    }

    /// <summary>
    /// Convert kilometers per hour to meters per second.
    /// </summary>
    public static decimal KmhToMs(decimal kmh)
    {
        return kmh * KmhToMs;
    }

    /// <summary>
    /// Convert miles per hour to meters per second.
    /// </summary>
    public static decimal MphToMs(decimal mph)
    {
        return mph * MphToMs;
    }

    /// <summary>
    /// Convert knots to meters per second.
    /// </summary>
    public static decimal KnotsToMs(decimal knots)
    {
        return knots * KnotsToMs;
    }

    /// <summary>
    /// Convert wind speed from one unit to another.
    /// Internal representation is always m/s.
    /// </summary>
    public static decimal Convert(decimal value, string fromUnit, string toUnit)
    {
        if (fromUnit == toUnit) return value;

        // Convert to m/s first
        decimal valueInMs = fromUnit switch
        {
            "ms" => value,
            "kmh" => KmhToMs(value),
            "mph" => MphToMs(value),
            "knots" => KnotsToMs(value),
            _ => throw new ArgumentException($"Unknown wind speed unit: {fromUnit}")
        };

        // Convert from m/s to target unit
        return toUnit switch
        {
            "ms" => valueInMs,
            "kmh" => MsToKmh(valueInMs),
            "mph" => MsToMph(valueInMs),
            "knots" => MsToKnots(valueInMs),
            _ => throw new ArgumentException($"Unknown wind speed unit: {toUnit}")
        };
    }

    /// <summary>
    /// Format wind speed with unit symbol.
    /// </summary>
    public static string Format(decimal windSpeed, string unit)
    {
        return unit switch
        {
            "ms" => $"{windSpeed:F1} m/s",
            "kmh" => $"{windSpeed:F1} km/h",
            "mph" => $"{windSpeed:F1} mph",
            "knots" => $"{windSpeed:F1} kts",
            _ => $"{windSpeed:F1}"
        };
    }
}
