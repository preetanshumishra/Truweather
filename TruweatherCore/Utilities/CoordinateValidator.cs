using TruweatherCore.Constants;

namespace TruweatherCore.Utilities;

/// <summary>
/// Utility for validating geographic coordinates.
/// </summary>
public static class CoordinateValidator
{
    /// <summary>
    /// Validate latitude is within valid range (-90 to 90).
    /// </summary>
    public static bool IsValidLatitude(decimal latitude)
    {
        return latitude >= ValidationRules.MinLatitude && latitude <= ValidationRules.MaxLatitude;
    }

    /// <summary>
    /// Validate longitude is within valid range (-180 to 180).
    /// </summary>
    public static bool IsValidLongitude(decimal longitude)
    {
        return longitude >= ValidationRules.MinLongitude && longitude <= ValidationRules.MaxLongitude;
    }

    /// <summary>
    /// Validate both latitude and longitude are within valid ranges.
    /// </summary>
    public static bool IsValidCoordinates(decimal latitude, decimal longitude)
    {
        return IsValidLatitude(latitude) && IsValidLongitude(longitude);
    }

    /// <summary>
    /// Calculate distance between two coordinates using Haversine formula (in kilometers).
    /// </summary>
    public static double CalculateDistance(decimal lat1, decimal lon1, decimal lat2, decimal lon2)
    {
        const double earthRadiusKm = 6371.0;

        double dLat = ToRadians((double)(lat2 - lat1));
        double dLon = ToRadians((double)(lon2 - lon1));

        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Cos(ToRadians((double)lat1)) * Math.Cos(ToRadians((double)lat2)) *
                   Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        double c = 2 * Math.Asin(Math.Sqrt(a));
        return earthRadiusKm * c;
    }

    /// <summary>
    /// Format coordinates for display.
    /// </summary>
    public static string Format(decimal latitude, decimal longitude)
    {
        string latDir = latitude >= 0 ? "N" : "S";
        string lonDir = longitude >= 0 ? "E" : "W";

        return $"{Math.Abs(latitude):F4}° {latDir}, {Math.Abs(longitude):F4}° {lonDir}";
    }

    private static double ToRadians(double degrees)
    {
        return degrees * Math.PI / 180.0;
    }
}
