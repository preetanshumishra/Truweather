namespace TruweatherAPI.Tests.Fixtures;

public static class TestDbContextFactory
{
    public static TruweatherDbContext Create()
    {
        var options = new DbContextOptionsBuilder<TruweatherDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new TruweatherDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }
}
