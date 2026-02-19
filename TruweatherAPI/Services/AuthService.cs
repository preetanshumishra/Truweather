using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TruweatherCore.Models.DTOs;
using TruweatherCore.Services.Interfaces;
using TruweatherAPI.Data;
using TruweatherAPI.Models;

namespace TruweatherAPI.Services;

public class AuthService(
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    IConfiguration configuration,
    TruweatherDbContext context,
    IEmailService emailService) : IAuthService
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly SignInManager<User> _signInManager = signInManager;
    private readonly IConfiguration _configuration = configuration;
    private readonly TruweatherDbContext _context = context;
    private readonly IEmailService _emailService = emailService;

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        if (request.Password != request.ConfirmPassword)
        {
            return new AuthResponse(false, "Passwords do not match");
        }

        var user = new User
        {
            UserName = request.Email,
            Email = request.Email,
            EmailConfirmed = false
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new AuthResponse(false, $"Registration failed: {errors}");
        }

        // Assign default "User" role
        await _userManager.AddToRoleAsync(user, "User");

        // Send verification email
        var verificationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        await _emailService.SendEmailVerificationAsync(user.Email!, verificationToken);

        return new AuthResponse(true, "Registration successful", User: new UserDto(
            user.Id,
            user.Email,
            user.UserName,
            user.IsEmailVerified,
            user.CreatedAt
        ));
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return new AuthResponse(false, "Invalid email or password");
        }

        var result = await _signInManager.PasswordSignInAsync(
            user, request.Password, false, false);

        if (!result.Succeeded)
        {
            return new AuthResponse(false, "Invalid email or password");
        }

        user.LastLoginAt = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        var accessToken = await GenerateJwtTokenAsync(user);
        var refreshToken = GenerateRefreshToken();

        // Save refresh token to database
        var refreshTokenEntity = new RefreshToken
        {
            UserId = user.Id,
            Token = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(30),
            CreatedAt = DateTime.UtcNow
        };
        _context.RefreshTokens.Add(refreshTokenEntity);
        await _context.SaveChangesAsync();

        return new AuthResponse(
            true,
            "Login successful",
            AccessToken: accessToken,
            RefreshToken: refreshToken,
            User: new UserDto(
                user.Id,
                user.Email ?? string.Empty,
                user.UserName,
                user.IsEmailVerified,
                user.CreatedAt
            )
        );
    }

    public async Task<AuthResponse> RefreshTokenAsync(string refreshToken)
    {
        var tokenEntity = await _context.RefreshTokens
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Token == refreshToken);

        if (tokenEntity == null || tokenEntity.IsRevoked || tokenEntity.ExpiresAt <= DateTime.UtcNow)
        {
            return new AuthResponse(false, "Invalid or expired refresh token");
        }

        // Revoke the old token (rotation)
        tokenEntity.IsRevoked = true;

        var newAccessToken = await GenerateJwtTokenAsync(tokenEntity.User);
        var newRefreshToken = GenerateRefreshToken();

        var newTokenEntity = new RefreshToken
        {
            UserId = tokenEntity.UserId,
            Token = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(30),
            CreatedAt = DateTime.UtcNow
        };
        _context.RefreshTokens.Add(newTokenEntity);
        await _context.SaveChangesAsync();

        return new AuthResponse(
            true,
            "Token refreshed",
            AccessToken: newAccessToken,
            RefreshToken: newRefreshToken
        );
    }

    public async Task<bool> LogoutAsync(string userId)
    {
        var activeTokens = await _context.RefreshTokens
            .Where(r => r.UserId == userId && !r.IsRevoked)
            .ToListAsync();

        foreach (var token in activeTokens)
        {
            token.IsRevoked = true;
        }

        await _context.SaveChangesAsync();
        await _signInManager.SignOutAsync();
        return true;
    }

    private async Task<string> GenerateJwtTokenAsync(User user)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            jwtSettings["Secret"] ?? throw new InvalidOperationException("JWT Secret not configured")));

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
            new(ClaimTypes.Name, user.UserName ?? string.Empty)
        };

        // Add role claims
        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(int.Parse(
            jwtSettings["ExpirationMinutes"] ?? "60"));

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public async Task<AuthResponse> VerifyEmailAsync(string email, string token)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return new AuthResponse(false, "User not found");

        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new AuthResponse(false, $"Email verification failed: {errors}");
        }

        user.IsEmailVerified = true;
        await _userManager.UpdateAsync(user);

        return new AuthResponse(true, "Email verified successfully");
    }

    public async Task<AuthResponse> ForgotPasswordAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            // Don't reveal whether user exists
            return new AuthResponse(true, "If an account exists, a password reset email has been sent");
        }

        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        await _emailService.SendPasswordResetAsync(email, resetToken);

        return new AuthResponse(true, "If an account exists, a password reset email has been sent");
    }

    public async Task<AuthResponse> ResetPasswordAsync(string email, string token, string newPassword)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return new AuthResponse(false, "Invalid request");

        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new AuthResponse(false, $"Password reset failed: {errors}");
        }

        return new AuthResponse(true, "Password reset successful");
    }
}
