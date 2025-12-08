using Microsoft.EntityFrameworkCore;
using TheSteward.Infrastructure.Data;

namespace TheSteward.Tests.Helpers;

public static class TestDbContextFactory
{
    public static TheStewardContext CreateInMemoryContext(string databaseName = "")
    {
        var options = new DbContextOptionsBuilder<TheStewardContext>()
            .UseInMemoryDatabase(databaseName: string.IsNullOrEmpty(databaseName)
                ? Guid.NewGuid().ToString()
                : databaseName)
            .EnableSensitiveDataLogging()
            .Options;

        var context = new TheStewardContext(options);

        context.Database.EnsureCreated();

        return context;
    }
}