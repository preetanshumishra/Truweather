namespace TruweatherCore.Utilities;

/// <summary>
/// Utility for formatting dates and times consistently across Web and Mobile.
/// </summary>
public static class DateTimeFormatter
{
    /// <summary>
    /// Format date as short date string (e.g., "Jan 15, 2024").
    /// </summary>
    public static string FormatShortDate(DateTime dateTime)
    {
        return dateTime.ToString("MMM d, yyyy");
    }

    /// <summary>
    /// Format date and time (e.g., "Jan 15, 2024 2:30 PM").
    /// </summary>
    public static string FormatDateTime(DateTime dateTime)
    {
        return dateTime.ToString("MMM d, yyyy h:mm tt");
    }

    /// <summary>
    /// Format time only (e.g., "2:30 PM").
    /// </summary>
    public static string FormatTime(DateTime dateTime)
    {
        return dateTime.ToString("h:mm tt");
    }

    /// <summary>
    /// Format relative time (e.g., "2 hours ago", "in 3 days").
    /// </summary>
    public static string FormatRelativeTime(DateTime dateTime)
    {
        DateTime now = DateTime.UtcNow;
        TimeSpan difference = dateTime > now ? dateTime - now : now - dateTime;

        string prefix = dateTime > now ? "in " : "";
        string suffix = dateTime > now ? "" : " ago";

        return difference.TotalSeconds < 60
            ? "just now"
            : difference.TotalMinutes < 60
                ? $"{prefix}{(int)difference.TotalMinutes} minute{(difference.TotalMinutes >= 2 ? "s" : "")}{suffix}"
            : difference.TotalHours < 24
                ? $"{prefix}{(int)difference.TotalHours} hour{(difference.TotalHours >= 2 ? "s" : "")}{suffix}"
            : difference.TotalDays < 7
                ? $"{prefix}{(int)difference.TotalDays} day{(difference.TotalDays >= 2 ? "s" : "")}{suffix}"
            : difference.TotalDays < 30
                ? $"{prefix}{(int)(difference.TotalDays / 7)} week{(difference.TotalDays / 7 >= 2 ? "s" : "")}{suffix}"
            : difference.TotalDays < 365
                ? $"{prefix}{(int)(difference.TotalDays / 30)} month{(difference.TotalDays / 30 >= 2 ? "s" : "")}{suffix}"
            : $"{prefix}{(int)(difference.TotalDays / 365)} year{(difference.TotalDays / 365 >= 2 ? "s" : "")}{suffix}";
    }

    /// <summary>
    /// Format date for forecast display (e.g., "Mon, Jan 15" or "Tomorrow" or "Today").
    /// </summary>
    public static string FormatForecastDate(DateTime dateTime)
    {
        DateTime today = DateTime.UtcNow.Date;
        DateTime targetDate = dateTime.Date;

        if (targetDate == today)
            return "Today";
        if (targetDate == today.AddDays(1))
            return "Tomorrow";
        if (targetDate == today.AddDays(-1))
            return "Yesterday";

        return dateTime.ToString("ddd, MMM d");
    }

    /// <summary>
    /// Get day of week name.
    /// </summary>
    public static string GetDayName(DateTime dateTime)
    {
        return dateTime.ToString("dddd");
    }

    /// <summary>
    /// Get month name.
    /// </summary>
    public static string GetMonthName(DateTime dateTime)
    {
        return dateTime.ToString("MMMM");
    }
}
