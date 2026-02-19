using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TruweatherCore.Models.DTOs;
using TruweatherCore.Services.Interfaces;

namespace TruweatherAPI.Controllers;

/// <summary>
/// API endpoints for user authentication and account management.
///
/// Provides secure user registration, login with JWT tokens, token refresh mechanism,
/// and logout functionality. All tokens use HS256 signing and expire after 60 minutes.
/// Refresh tokens last 30 days and are used to obtain new access tokens without re-login.
///
/// Rate limit: 10 requests per minute per IP address.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService, ILogger<AuthController> logger) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    private readonly ILogger<AuthController> _logger = logger;

    /// <summary>Register a new user account and obtain JWT tokens.</summary>
    /// <remarks>
    /// Creates a new user account with the provided email and password.
    /// Password must be at least 8 characters and contain uppercase, lowercase, digit, and special character.
    /// On success, returns access token (60 min expiry) and refresh token (30 day expiry).
    /// </remarks>
    /// <param name="request">Registration details (email, password, passwordConfirm)</param>
    /// <response code="200">Registration successful, access and refresh tokens returned</response>
    /// <response code="400">Validation error (weak password, email already registered, etc)</response>
    /// <response code="429">Rate limit exceeded (10 requests/minute)</response>
    [HttpPost("register")]
    [EnableRateLimiting("auth")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.RegisterAsync(request);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>Authenticate user with email and password, obtain JWT tokens.</summary>
    /// <remarks>
    /// Validates email and password against stored user credentials.
    /// Returns access token (60 min) and refresh token (30 days).
    /// Store tokens in secure client storage and include access token in Authorization header
    /// for subsequent API requests: Authorization: Bearer {accessToken}
    /// </remarks>
    /// <param name="request">Login credentials (email, password)</param>
    /// <response code="200">Login successful, access and refresh tokens returned</response>
    /// <response code="401">Invalid email or password</response>
    /// <response code="429">Rate limit exceeded (10 requests/minute)</response>
    [HttpPost("login")]
    [EnableRateLimiting("auth")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _logger.LogInformation("Login attempt for {Email}", request.Email);
        var result = await _authService.LoginAsync(request);
        if (!result.Success)
            _logger.LogWarning("Failed login attempt for {Email}", request.Email);
        return result.Success ? Ok(result) : Unauthorized(result);
    }

    /// <summary>Obtain a new access token using a valid refresh token.</summary>
    /// <remarks>
    /// Call this endpoint when access token expires (every 60 minutes).
    /// Refresh token lasts 30 days and allows obtaining new access tokens without user re-authentication.
    /// Returns new access token and optionally a new refresh token.
    /// </remarks>
    /// <param name="request">Refresh token (obtain from login or previous refresh)</param>
    /// <response code="200">Token refresh successful, new access token returned</response>
    /// <response code="401">Invalid, expired, or revoked refresh token</response>
    /// <response code="429">Rate limit exceeded (10 requests/minute)</response>
    [HttpPost("refresh")]
    [EnableRateLimiting("auth")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<ActionResult<AuthResponse>> RefreshToken(RefreshTokenRequest request)
    {
        var result = await _authService.RefreshTokenAsync(request.RefreshToken);
        return result.Success ? Ok(result) : Unauthorized(result);
    }

    /// <summary>Verify email address with verification token.</summary>
    /// <param name="request">Email and verification token</param>
    /// <response code="200">Email verified successfully</response>
    /// <response code="400">Invalid token or email</response>
    [HttpPost("verify-email")]
    [EnableRateLimiting("auth")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthResponse>> VerifyEmail(VerifyEmailRequest request)
    {
        var result = await _authService.VerifyEmailAsync(request.Email, request.Token);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>Request a password reset email.</summary>
    /// <param name="request">Email address</param>
    /// <response code="200">Reset email sent (or user doesn't exist - same response for security)</response>
    [HttpPost("forgot-password")]
    [EnableRateLimiting("auth")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<AuthResponse>> ForgotPassword(ForgotPasswordRequest request)
    {
        var result = await _authService.ForgotPasswordAsync(request.Email);
        return Ok(result);
    }

    /// <summary>Reset password using token from email.</summary>
    /// <param name="request">Email, token, new password</param>
    /// <response code="200">Password reset successful</response>
    /// <response code="400">Invalid token or weak password</response>
    [HttpPost("reset-password")]
    [EnableRateLimiting("auth")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthResponse>> ResetPassword(ResetPasswordRequest request)
    {
        var result = await _authService.ResetPasswordAsync(request.Email, request.Token, request.NewPassword);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>Log out the current user and invalidate their refresh token.</summary>
    /// <remarks>
    /// Requires valid JWT access token in Authorization header.
    /// Invalidates the user's refresh token, preventing further token refreshes.
    /// Client should clear stored tokens and redirect to login page.
    /// </remarks>
    /// <response code="200">Logout successful, refresh token invalidated</response>
    /// <response code="401">Unauthorized - Valid JWT access token required</response>
    [HttpPost("logout")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<object>> Logout()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        await _authService.LogoutAsync(userId);
        return Ok(new { message = "Logged out successfully" });
    }
}
