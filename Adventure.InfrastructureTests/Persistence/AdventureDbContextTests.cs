using Adventure.InfrastructureTests;
using BaseTests;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Adventure.Infrastructure.Persistence.Tests;

public class AdventureDbContextTests : BaseInfrastructureTests
{
    private readonly AdventureDbContext _context;
    private readonly string _databaseName = Guid.NewGuid().ToString();
    private readonly Domain.Adventure _adventure = HelperTests.Adventure;
    private readonly Domain.UserAdventureAggregate _userAdventureAggregate = HelperTests.UserAdventureAggregate;

    public AdventureDbContextTests()
    {
        _context = CreateAdventureDbContext(_databaseName);
    }

    [Fact]
    public async Task Add_ReturnsAdventureWithRoutes_RoutesPopulated()
    {
        // Act
        await _context.Adventure.AddAsync(_adventure);
        await _context.SaveChangesAsync();

        // Assert
        var assertDbContext = CreateAdventureDbContext(_databaseName);
        var actualAdventure = await assertDbContext.Adventure.FirstOrDefaultAsync();
        actualAdventure.Should().BeEquivalentTo(_adventure, options => options.Excluding(x => x.Id));
    }

    [Fact]
    public async Task Add_ReturnsUserAdventureWithSelectedRoutes_SelectedRoutesPopulated()
    {
        // Act
        await _context.UserAdventureAggregate.AddAsync(_userAdventureAggregate);
        await _context.SaveChangesAsync();

        // Assert
        var assertDbContext = CreateAdventureDbContext(_databaseName);
        var actualUserAdventureAggregate = await assertDbContext.UserAdventureAggregate
                                .Include(x => x.User)
                                .Include(x => x.Adventure).FirstOrDefaultAsync();

        actualUserAdventureAggregate.Should().BeEquivalentTo(_userAdventureAggregate, options => 
        options.Excluding(x => x.Id).Excluding(x => x.Adventure.Id).Excluding(x => x.User.Id).Excluding(x => x.UserId).Excluding(x => x.AdventureId));
    }
    
}