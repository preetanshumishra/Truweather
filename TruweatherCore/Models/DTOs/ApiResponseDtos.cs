namespace TruweatherCore.Models.DTOs;

/// <summary>
/// Standard API response wrapper for all endpoints.
/// Provides consistent response format across the entire API.
/// </summary>
public record ApiResponse<T>(
    bool Success,
    string Message,
    T? Data = default,
    ApiError? Error = null
);

/// <summary>
/// Standard API error response format.
/// </summary>
public record ApiError(
    string Code,
    string Message,
    Dictionary<string, string[]>? ValidationErrors = null,
    string? TraceId = null
);

/// <summary>
/// Pagination wrapper for list responses.
/// </summary>
public record PaginatedResponse<T>(
    List<T> Items,
    int TotalCount,
    int PageNumber,
    int PageSize,
    int TotalPages
)
{
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}

/// <summary>
/// Common API error codes used throughout the application.
/// </summary>
public static class ApiErrorCodes
{
    // Authentication errors
    public const string InvalidCredentials = "INVALID_CREDENTIALS";
    public const string UnauthorizedAccess = "UNAUTHORIZED";
    public const string TokenExpired = "TOKEN_EXPIRED";
    public const string InvalidToken = "INVALID_TOKEN";
    public const string UserNotFound = "USER_NOT_FOUND";
    public const string EmailAlreadyRegistered = "EMAIL_ALREADY_REGISTERED";
    public const string PasswordMismatch = "PASSWORD_MISMATCH";

    // Validation errors
    public const string ValidationFailed = "VALIDATION_FAILED";
    public const string InvalidInput = "INVALID_INPUT";
    public const string RequiredFieldMissing = "REQUIRED_FIELD_MISSING";

    // Resource errors
    public const string NotFound = "NOT_FOUND";
    public const string Conflict = "CONFLICT";
    public const string Forbidden = "FORBIDDEN";

    // Server errors
    public const string InternalServerError = "INTERNAL_SERVER_ERROR";
    public const string ServiceUnavailable = "SERVICE_UNAVAILABLE";

    // Weather-specific errors
    public const string WeatherDataUnavailable = "WEATHER_DATA_UNAVAILABLE";
    public const string InvalidCoordinates = "INVALID_COORDINATES";
    public const string LocationNotFound = "LOCATION_NOT_FOUND";

    // Alert-specific errors
    public const string AlertNotFound = "ALERT_NOT_FOUND";
    public const string InvalidAlertType = "INVALID_ALERT_TYPE";

    // Preference errors
    public const string PreferencesNotFound = "PREFERENCES_NOT_FOUND";
    public const string InvalidPreferences = "INVALID_PREFERENCES";
}
