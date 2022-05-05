using Microsoft.EntityFrameworkCore;

namespace Adventure.Infrastructure.Persistence;
public class AdventureDbContext : DbContext
{
    public AdventureDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Domain.Adventure> Adventure { get; set; }

    public DbSet<Domain.User> User { get; set; }

    public DbSet<Domain.UserAdventureAggregate> UserAdventureAggregate { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new AdventureEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new UserAdventureAggregateEntityTypeConfiguration());
    }
}