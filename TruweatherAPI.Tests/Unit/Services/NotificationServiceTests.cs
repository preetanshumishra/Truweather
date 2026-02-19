namespace TruweatherAPI.Tests.Unit.Services;

public class NotificationServiceTests
{
    private static NotificationService CreateService(TruweatherDbContext context)
    {
        return new NotificationService(context);
    }

    private static Notification CreateNotification(string userId = "user-1", bool isRead = false, int? alertId = null)
    {
        return new Notification
        {
            UserId = userId,
            AlertId = alertId,
            Title = "Test Alert",
            Message = "Temperature exceeded threshold",
            IsRead = isRead,
            CreatedAt = DateTime.UtcNow
        };
    }

    [Fact]
    public async Task GetNotificationsAsync_ReturnsUserNotifications()
    {
        using var context = TestDbContextFactory.Create();
        context.Notifications.Add(CreateNotification("user-1"));
        context.Notifications.Add(CreateNotification("user-1"));
        context.Notifications.Add(CreateNotification("user-2"));
        await context.SaveChangesAsync();

        var service = CreateService(context);
        var result = await service.GetNotificationsAsync("user-1");

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetNotificationsAsync_ReturnsEmptyForNewUser()
    {
        using var context = TestDbContextFactory.Create();
        var service = CreateService(context);

        var result = await service.GetNotificationsAsync("new-user");

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetNotificationsAsync_OrdersByMostRecentFirst()
    {
        using var context = TestDbContextFactory.Create();
        context.Notifications.Add(new Notification
        {
            UserId = "user-1", Title = "Old", Message = "Old",
            CreatedAt = DateTime.UtcNow.AddHours(-2)
        });
        context.Notifications.Add(new Notification
        {
            UserId = "user-1", Title = "New", Message = "New",
            CreatedAt = DateTime.UtcNow
        });
        await context.SaveChangesAsync();

        var service = CreateService(context);
        var result = await service.GetNotificationsAsync("user-1");

        result[0].Title.Should().Be("New");
        result[1].Title.Should().Be("Old");
    }

    [Fact]
    public async Task GetUnreadCountAsync_ReturnsCorrectCount()
    {
        using var context = TestDbContextFactory.Create();
        context.Notifications.Add(CreateNotification("user-1", isRead: false));
        context.Notifications.Add(CreateNotification("user-1", isRead: false));
        context.Notifications.Add(CreateNotification("user-1", isRead: true));
        await context.SaveChangesAsync();

        var service = CreateService(context);
        var count = await service.GetUnreadCountAsync("user-1");

        count.Should().Be(2);
    }

    [Fact]
    public async Task GetUnreadCountAsync_ReturnsZeroWhenAllRead()
    {
        using var context = TestDbContextFactory.Create();
        context.Notifications.Add(CreateNotification("user-1", isRead: true));
        await context.SaveChangesAsync();

        var service = CreateService(context);
        var count = await service.GetUnreadCountAsync("user-1");

        count.Should().Be(0);
    }

    [Fact]
    public async Task MarkAsReadAsync_MarksNotification()
    {
        using var context = TestDbContextFactory.Create();
        context.Notifications.Add(CreateNotification("user-1"));
        await context.SaveChangesAsync();
        var id = context.Notifications.First().Id;

        var service = CreateService(context);
        var result = await service.MarkAsReadAsync("user-1", id);

        result.Should().BeTrue();
        var notification = await context.Notifications.FindAsync(id);
        notification!.IsRead.Should().BeTrue();
    }

    [Fact]
    public async Task MarkAsReadAsync_ReturnsFalseForWrongUser()
    {
        using var context = TestDbContextFactory.Create();
        context.Notifications.Add(CreateNotification("user-1"));
        await context.SaveChangesAsync();
        var id = context.Notifications.First().Id;

        var service = CreateService(context);
        var result = await service.MarkAsReadAsync("user-2", id);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task MarkAllAsReadAsync_MarksAllUnread()
    {
        using var context = TestDbContextFactory.Create();
        context.Notifications.Add(CreateNotification("user-1", isRead: false));
        context.Notifications.Add(CreateNotification("user-1", isRead: false));
        context.Notifications.Add(CreateNotification("user-2", isRead: false));
        await context.SaveChangesAsync();

        var service = CreateService(context);
        var result = await service.MarkAllAsReadAsync("user-1");

        result.Should().BeTrue();
        var user1Notifications = context.Notifications.Where(n => n.UserId == "user-1").ToList();
        user1Notifications.Should().AllSatisfy(n => n.IsRead.Should().BeTrue());

        var user2Notifications = context.Notifications.Where(n => n.UserId == "user-2").ToList();
        user2Notifications.Should().AllSatisfy(n => n.IsRead.Should().BeFalse());
    }
}
