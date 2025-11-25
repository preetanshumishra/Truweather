namespace TruweatherAPI.DTOs;

public record RegisterRequest(
    string Email,
    string Password,
    string ConfirmPassword,
    string? FullName = null
);

public record LoginRequest(
    string Email,
    string Password
);

public record AuthResponse(
    bool Success,
    string Message,
    string? AccessToken = null,
    string? RefreshToken = null,
    UserDto? User = null
);

public record RefreshTokenRequest(
    string RefreshToken
);

public record UserDto(
    string Id,
    string Email,
    string? FullName,
    bool IsEmailVerified,
    DateTime CreatedAt
);
