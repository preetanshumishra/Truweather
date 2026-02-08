namespace TruweatherCore.Utilities;

/// <summary>
/// Utility for converting temperatures between Celsius, Fahrenheit, and Kelvin.
/// </summary>
public static class TemperatureConverter
{
    /// <summary>
    /// Convert Celsius to Fahrenheit.
    /// </summary>
    public static decimal CelsiusToFahrenheit(decimal celsius)
    {
        return (celsius * 9 / 5) + 32;
    }

    /// <summary>
    /// Convert Celsius to Kelvin.
    /// </summary>
    public static decimal CelsiusToKelvin(decimal celsius)
    {
        return celsius + 273.15m;
    }

    /// <summary>
    /// Convert Fahrenheit to Celsius.
    /// </summary>
    public static decimal FahrenheitToCelsius(decimal fahrenheit)
    {
        return (fahrenheit - 32) * 5 / 9;
    }

    /// <summary>
    /// Convert Kelvin to Celsius.
    /// </summary>
    public static decimal KelvinToCelsius(decimal kelvin)
    {
        return kelvin - 273.15m;
    }

    /// <summary>
    /// Convert temperature from one unit to another.
    /// </summary>
    public static decimal Convert(decimal value, string fromUnit, string toUnit)
    {
        if (fromUnit == toUnit) return value;

        return (fromUnit, toUnit) switch
        {
            ("Celsius", "Fahrenheit") => CelsiusToFahrenheit(value),
            ("Celsius", "Kelvin") => CelsiusToKelvin(value),
            ("Fahrenheit", "Celsius") => FahrenheitToCelsius(value),
            ("Fahrenheit", "Kelvin") => CelsiusToKelvin(FahrenheitToCelsius(value)),
            ("Kelvin", "Celsius") => KelvinToCelsius(value),
            ("Kelvin", "Fahrenheit") => CelsiusToFahrenheit(KelvinToCelsius(value)),
            _ => throw new ArgumentException($"Unknown temperature unit: {fromUnit} or {toUnit}")
        };
    }

    /// <summary>
    /// Format temperature with unit symbol.
    /// </summary>
    public static string Format(decimal temperature, string unit)
    {
        return unit switch
        {
            "Celsius" => $"{temperature:F1}°C",
            "Fahrenheit" => $"{temperature:F1}°F",
            "Kelvin" => $"{temperature:F1}K",
            _ => $"{temperature:F1}°"
        };
    }
}
