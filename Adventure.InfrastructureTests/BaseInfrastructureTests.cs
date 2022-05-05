using Adventure.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Adventure.InfrastructureTests;

public abstract class BaseInfrastructureTests
{
    protected AdventureDbContext CreateAdventureDbContext(string dbName)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AdventureDbContext>();
        optionsBuilder.UseSqlite($"DataSource = {dbName}");
        var dbContext = new AdventureDbContext(optionsBuilder.Options);
        dbContext.Database.EnsureCreated();
        return dbContext;
    }
}