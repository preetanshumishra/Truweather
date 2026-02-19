namespace TruweatherAPI.Tests.Unit.Services;

public class AdminServiceTests
{
    private readonly Mock<UserManager<User>> _mockUserManager;

    public AdminServiceTests()
    {
        _mockUserManager = MockUserManager.Create();
    }

    private AdminService CreateService(TruweatherDbContext context)
    {
        return new AdminService(_mockUserManager.Object, context);
    }

    [Fact]
    public async Task GetUsersAsync_ReturnsAllUsers()
    {
        using var context = TestDbContextFactory.Create();
        context.Users.Add(new User { Id = "user-1", Email = "user1@test.com", UserName = "user1@test.com" });
        context.Users.Add(new User { Id = "user-2", Email = "user2@test.com", UserName = "user2@test.com" });
        await context.SaveChangesAsync();

        _mockUserManager.Setup(x => x.Users).Returns(context.Users);
        _mockUserManager.Setup(x => x.GetRolesAsync(It.IsAny<User>()))
            .ReturnsAsync(new List<string> { "User" });

        var service = CreateService(context);
        var result = await service.GetUsersAsync();

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetUsersAsync_WithSearchTerm_FiltersResults()
    {
        using var context = TestDbContextFactory.Create();
        context.Users.Add(new User { Id = "user-1", Email = "admin@test.com", UserName = "admin@test.com" });
        context.Users.Add(new User { Id = "user-2", Email = "user@test.com", UserName = "user@test.com" });
        await context.SaveChangesAsync();

        _mockUserManager.Setup(x => x.Users).Returns(context.Users);
        _mockUserManager.Setup(x => x.GetRolesAsync(It.IsAny<User>()))
            .ReturnsAsync(new List<string> { "User" });

        var service = CreateService(context);
        var result = await service.GetUsersAsync("admin");

        result.Should().HaveCount(1);
        result[0].Email.Should().Be("admin@test.com");
    }

    [Fact]
    public async Task GetUserDetailAsync_ReturnsDetailedInfo()
    {
        using var context = TestDbContextFactory.Create();
        var user = new User { Id = "user-1", Email = "test@test.com", UserName = "test@test.com" };

        _mockUserManager.Setup(x => x.FindByIdAsync("user-1")).ReturnsAsync(user);
        _mockUserManager.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(new List<string> { "User" });

        context.SavedLocations.Add(new SavedLocation
        {
            UserId = "user-1", LocationName = "NYC", Latitude = 40.7m, Longitude = -74.0m
        });
        await context.SaveChangesAsync();

        var service = CreateService(context);
        var result = await service.GetUserDetailAsync("user-1");

        result.Should().NotBeNull();
        result!.Email.Should().Be("test@test.com");
        result.SavedLocations.Should().HaveCount(1);
        result.Roles.Should().Contain("User");
    }

    [Fact]
    public async Task GetUserDetailAsync_ReturnsNullForNonExistent()
    {
        using var context = TestDbContextFactory.Create();
        _mockUserManager.Setup(x => x.FindByIdAsync("nonexistent")).ReturnsAsync((User?)null);

        var service = CreateService(context);
        var result = await service.GetUserDetailAsync("nonexistent");

        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateUserRoleAsync_UpdatesRole()
    {
        using var context = TestDbContextFactory.Create();
        var user = new User { Id = "user-1", Email = "test@test.com" };

        _mockUserManager.Setup(x => x.FindByIdAsync("user-1")).ReturnsAsync(user);
        _mockUserManager.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(new List<string> { "User" });
        _mockUserManager.Setup(x => x.RemoveFromRolesAsync(user, It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(IdentityResult.Success);
        _mockUserManager.Setup(x => x.AddToRoleAsync(user, "Admin"))
            .ReturnsAsync(IdentityResult.Success);

        var service = CreateService(context);
        var result = await service.UpdateUserRoleAsync("user-1", "Admin");

        result.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateUserRoleAsync_ReturnsFalseForNonExistent()
    {
        using var context = TestDbContextFactory.Create();
        _mockUserManager.Setup(x => x.FindByIdAsync("nonexistent")).ReturnsAsync((User?)null);

        var service = CreateService(context);
        var result = await service.UpdateUserRoleAsync("nonexistent", "Admin");

        result.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteUserAsync_DeletesUser()
    {
        using var context = TestDbContextFactory.Create();
        var user = new User { Id = "user-1", Email = "test@test.com" };

        _mockUserManager.Setup(x => x.FindByIdAsync("user-1")).ReturnsAsync(user);
        _mockUserManager.Setup(x => x.DeleteAsync(user)).ReturnsAsync(IdentityResult.Success);

        var service = CreateService(context);
        var result = await service.DeleteUserAsync("user-1");

        result.Should().BeTrue();
    }

    [Fact]
    public async Task GetSystemStatsAsync_ReturnsCorrectCounts()
    {
        using var context = TestDbContextFactory.Create();

        context.Users.Add(new User { Id = "user-1", Email = "user1@test.com", UserName = "user1@test.com" });
        context.Users.Add(new User { Id = "user-2", Email = "user2@test.com", UserName = "user2@test.com" });
        await context.SaveChangesAsync();

        _mockUserManager.Setup(x => x.Users).Returns(context.Users);

        context.SavedLocations.Add(new SavedLocation
        {
            UserId = "user-1", LocationName = "NYC", Latitude = 40.7m, Longitude = -74.0m
        });
        await context.SaveChangesAsync();

        var location = context.SavedLocations.First();
        context.WeatherAlerts.Add(new WeatherAlert
        {
            UserId = "user-1", SavedLocationId = location.Id,
            AlertType = "Temperature", Condition = "Above", Threshold = 30m, IsEnabled = true
        });
        context.Notifications.Add(new Notification
        {
            UserId = "user-1", Title = "Test", Message = "Test", IsRead = false
        });
        await context.SaveChangesAsync();

        var service = CreateService(context);
        var stats = await service.GetSystemStatsAsync();

        stats.TotalUsers.Should().Be(2);
        stats.TotalLocations.Should().Be(1);
        stats.TotalAlerts.Should().Be(1);
        stats.ActiveAlerts.Should().Be(1);
        stats.TotalNotifications.Should().Be(1);
        stats.UnreadNotifications.Should().Be(1);
    }
}
