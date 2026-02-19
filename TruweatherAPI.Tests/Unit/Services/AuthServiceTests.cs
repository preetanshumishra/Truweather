namespace TruweatherAPI.Tests.Unit.Services;

public class AuthServiceTests
{
    private readonly Mock<UserManager<User>> _mockUserManager;
    private readonly Mock<SignInManager<User>> _mockSignInManager;
    private readonly Mock<IEmailService> _mockEmailService;
    private readonly IConfiguration _configuration;

    public AuthServiceTests()
    {
        _mockUserManager = MockUserManager.Create();
        _mockSignInManager = MockSignInManager.Create(_mockUserManager);
        _mockEmailService = new Mock<IEmailService>();
        _configuration = TestConfiguration.Create();

        // Setup default email service behavior
        _mockEmailService.Setup(x => x.SendEmailVerificationAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(true);

        // Setup default role assignment
        _mockUserManager.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        _mockUserManager.Setup(x => x.GenerateEmailConfirmationTokenAsync(It.IsAny<User>()))
            .ReturnsAsync("test-verification-token");
        _mockUserManager.Setup(x => x.GetRolesAsync(It.IsAny<User>()))
            .ReturnsAsync(new List<string> { "User" });
    }

    private AuthService CreateService(TruweatherDbContext context)
    {
        return new AuthService(
            _mockUserManager.Object,
            _mockSignInManager.Object,
            _configuration,
            context,
            _mockEmailService.Object);
    }

    // --- Register Tests ---

    [Fact]
    public async Task RegisterAsync_WithValidRequest_ReturnsSuccess()
    {
        using var context = TestDbContextFactory.Create();
        _mockUserManager
            .Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        var service = CreateService(context);
        var request = new RegisterRequest { Email = "test@test.com", Password = "Password123!", ConfirmPassword = "Password123!", FullName = "Test User" };

        var result = await service.RegisterAsync(request);

        result.Success.Should().BeTrue();
        result.User.Should().NotBeNull();
        result.User!.Email.Should().Be("test@test.com");
    }

    [Fact]
    public async Task RegisterAsync_WithMismatchedPasswords_ReturnsFailure()
    {
        using var context = TestDbContextFactory.Create();
        var service = CreateService(context);
        var request = new RegisterRequest { Email = "test@test.com", Password = "Password123!", ConfirmPassword = "Different456!", FullName = "Test User" };

        var result = await service.RegisterAsync(request);

        result.Success.Should().BeFalse();
        result.Message.Should().Contain("Passwords do not match");
        _mockUserManager.Verify(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task RegisterAsync_WhenUserManagerFails_ReturnsError()
    {
        using var context = TestDbContextFactory.Create();
        _mockUserManager
            .Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Email already taken" }));

        var service = CreateService(context);
        var request = new RegisterRequest { Email = "test@test.com", Password = "Password123!", ConfirmPassword = "Password123!", FullName = "Test User" };

        var result = await service.RegisterAsync(request);

        result.Success.Should().BeFalse();
        result.Message.Should().Contain("Email already taken");
    }

    // --- Login Tests ---

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ReturnsTokens()
    {
        using var context = TestDbContextFactory.Create();
        var user = new User
        {
            Id = "user-1",
            Email = "test@test.com",
            UserName = "test@test.com",
            IsEmailVerified = false,
            CreatedAt = DateTime.UtcNow
        };

        _mockUserManager
            .Setup(x => x.FindByEmailAsync("test@test.com"))
            .ReturnsAsync(user);

        _mockSignInManager
            .Setup(x => x.PasswordSignInAsync(user, "Password123!", false, false))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

        _mockUserManager
            .Setup(x => x.UpdateAsync(It.IsAny<User>()))
            .ReturnsAsync(IdentityResult.Success);

        var service = CreateService(context);
        var request = new LoginRequest("test@test.com", "Password123!");

        var result = await service.LoginAsync(request);

        result.Success.Should().BeTrue();
        result.AccessToken.Should().NotBeNullOrEmpty();
        result.RefreshToken.Should().NotBeNullOrEmpty();
        result.User.Should().NotBeNull();
    }

    [Fact]
    public async Task LoginAsync_WithNonExistentEmail_ReturnsFailure()
    {
        using var context = TestDbContextFactory.Create();
        _mockUserManager
            .Setup(x => x.FindByEmailAsync("unknown@test.com"))
            .ReturnsAsync((User?)null);

        var service = CreateService(context);
        var result = await service.LoginAsync(new LoginRequest("unknown@test.com", "Password123!"));

        result.Success.Should().BeFalse();
        result.Message.Should().Contain("Invalid email or password");
    }

    [Fact]
    public async Task LoginAsync_WithWrongPassword_ReturnsFailure()
    {
        using var context = TestDbContextFactory.Create();
        var user = new User { Id = "user-1", Email = "test@test.com", UserName = "test@test.com" };

        _mockUserManager
            .Setup(x => x.FindByEmailAsync("test@test.com"))
            .ReturnsAsync(user);

        _mockSignInManager
            .Setup(x => x.PasswordSignInAsync(user, "WrongPassword!", false, false))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

        var service = CreateService(context);
        var result = await service.LoginAsync(new LoginRequest("test@test.com", "WrongPassword!"));

        result.Success.Should().BeFalse();
    }

    [Fact]
    public async Task LoginAsync_SavesRefreshTokenToDb()
    {
        using var context = TestDbContextFactory.Create();
        var user = new User { Id = "user-1", Email = "test@test.com", UserName = "test@test.com" };

        _mockUserManager.Setup(x => x.FindByEmailAsync("test@test.com")).ReturnsAsync(user);
        _mockSignInManager.Setup(x => x.PasswordSignInAsync(user, "Pass123!", false, false))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);
        _mockUserManager.Setup(x => x.UpdateAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);

        var service = CreateService(context);
        await service.LoginAsync(new LoginRequest("test@test.com", "Pass123!"));

        context.RefreshTokens.Should().HaveCount(1);
        var token = await context.RefreshTokens.FirstAsync();
        token.UserId.Should().Be("user-1");
        token.IsRevoked.Should().BeFalse();
    }

    // --- RefreshToken Tests ---

    [Fact]
    public async Task RefreshTokenAsync_WithValidToken_ReturnsNewTokens()
    {
        using var context = TestDbContextFactory.Create();
        var user = new User { Id = "user-1", Email = "test@test.com", UserName = "test@test.com" };
        context.Users.Add(user);
        context.RefreshTokens.Add(new RefreshToken
        {
            UserId = "user-1",
            Token = "valid-refresh-token",
            ExpiresAt = DateTime.UtcNow.AddDays(30),
            IsRevoked = false,
            CreatedAt = DateTime.UtcNow
        });
        await context.SaveChangesAsync();

        var service = CreateService(context);
        var result = await service.RefreshTokenAsync("valid-refresh-token");

        result.Success.Should().BeTrue();
        result.AccessToken.Should().NotBeNullOrEmpty();
        result.RefreshToken.Should().NotBeNullOrEmpty();
        result.RefreshToken.Should().NotBe("valid-refresh-token");
    }

    [Fact]
    public async Task RefreshTokenAsync_WithRevokedToken_ReturnsFailure()
    {
        using var context = TestDbContextFactory.Create();
        var user = new User { Id = "user-1", Email = "test@test.com", UserName = "test@test.com" };
        context.Users.Add(user);
        context.RefreshTokens.Add(new RefreshToken
        {
            UserId = "user-1",
            Token = "revoked-token",
            ExpiresAt = DateTime.UtcNow.AddDays(30),
            IsRevoked = true,
            CreatedAt = DateTime.UtcNow
        });
        await context.SaveChangesAsync();

        var service = CreateService(context);
        var result = await service.RefreshTokenAsync("revoked-token");

        result.Success.Should().BeFalse();
    }

    [Fact]
    public async Task RefreshTokenAsync_WithExpiredToken_ReturnsFailure()
    {
        using var context = TestDbContextFactory.Create();
        var user = new User { Id = "user-1", Email = "test@test.com", UserName = "test@test.com" };
        context.Users.Add(user);
        context.RefreshTokens.Add(new RefreshToken
        {
            UserId = "user-1",
            Token = "expired-token",
            ExpiresAt = DateTime.UtcNow.AddDays(-1),
            IsRevoked = false,
            CreatedAt = DateTime.UtcNow.AddDays(-31)
        });
        await context.SaveChangesAsync();

        var service = CreateService(context);
        var result = await service.RefreshTokenAsync("expired-token");

        result.Success.Should().BeFalse();
    }

    [Fact]
    public async Task RefreshTokenAsync_WithNonExistentToken_ReturnsFailure()
    {
        using var context = TestDbContextFactory.Create();
        var service = CreateService(context);

        var result = await service.RefreshTokenAsync("nonexistent-token");

        result.Success.Should().BeFalse();
    }

    // --- Logout Tests ---

    [Fact]
    public async Task LogoutAsync_RevokesAllActiveTokens()
    {
        using var context = TestDbContextFactory.Create();
        context.RefreshTokens.AddRange(
            new RefreshToken { UserId = "user-1", Token = "token-1", ExpiresAt = DateTime.UtcNow.AddDays(30), IsRevoked = false },
            new RefreshToken { UserId = "user-1", Token = "token-2", ExpiresAt = DateTime.UtcNow.AddDays(30), IsRevoked = false }
        );
        await context.SaveChangesAsync();

        _mockSignInManager.Setup(x => x.SignOutAsync()).Returns(Task.CompletedTask);

        var service = CreateService(context);
        var result = await service.LogoutAsync("user-1");

        result.Should().BeTrue();
        var tokens = await context.RefreshTokens.Where(r => r.UserId == "user-1").ToListAsync();
        tokens.Should().AllSatisfy(t => t.IsRevoked.Should().BeTrue());
    }

    [Fact]
    public async Task LogoutAsync_WithNoActiveTokens_ReturnsTrue()
    {
        using var context = TestDbContextFactory.Create();
        _mockSignInManager.Setup(x => x.SignOutAsync()).Returns(Task.CompletedTask);

        var service = CreateService(context);
        var result = await service.LogoutAsync("user-1");

        result.Should().BeTrue();
    }
}
