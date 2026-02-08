using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TruweatherAPI.Data;

/// <summary>
/// Factory for creating TruweatherDbContext instances at design time.
/// This is used by EF Core tools (migrations, etc.) to create a DbContext without running the full application.
/// </summary>
public class TruweatherDbContextFactory : IDesignTimeDbContextFactory<TruweatherDbContext>
{
    public TruweatherDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TruweatherDbContext>();
        var connectionString = "Server=.;Database=truweather_dev;Integrated Security=true;Encrypt=false;";

        optionsBuilder.UseSqlServer(connectionString);

        return new TruweatherDbContext(optionsBuilder.Options);
    }
}
