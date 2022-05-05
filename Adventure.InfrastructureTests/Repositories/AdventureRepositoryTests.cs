using System;
using System.Linq;
using System.Threading.Tasks;
using Adventure.Infrastructure.Repositories;
using BaseTests;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Adventure.InfrastructureTests.Repositories;

public class AdventureRepositoryTests : BaseInfrastructureTests
{
    private readonly string _databaseName = Guid.NewGuid().ToString();
    private readonly AdventureRepository _repository;

    public AdventureRepositoryTests()
    {
        _repository = new AdventureRepository(CreateAdventureDbContext(_databaseName));
    }

    [Fact]
    public async Task GetById_ReturnsAdventure_IfAdventureExists()
    {
        // Arrange
        var seedingContext = CreateAdventureDbContext(_databaseName);
        var addedEntity = await seedingContext.Adventure.AddAsync(HelperTests.Adventure);
        await seedingContext.SaveChangesAsync();
        
        // Act
        var actualAdventure = await _repository.GetById(addedEntity.Entity.Id);

        // Assert
        actualAdventure.Should().BeEquivalentTo(HelperTests.Adventure, options => options.Excluding(x => x.Id));
    }

    [Fact]
    public async Task GetById_ReturnsNull_IfAdventureDoesNotExists()
    {        
        // Act
        var actualAdventure = await _repository.GetById(int.MaxValue);

        // Assert
        actualAdventure.Should().BeNull();
    }

    [Fact]
    public async Task Add_ReturnsAddedEntity_SuccessfulAdd()
    {
        // Act
        var addedAdventure = await _repository.Add(HelperTests.Adventure);

        // Assert
        var assertContext = CreateAdventureDbContext(_databaseName);
        var actualAdventure = await assertContext.Adventure.SingleAsync();
        addedAdventure.Should().BeEquivalentTo(actualAdventure);
    }

    [Fact]
    public async Task Contains_ReturnsTrue_IfTitleExists()
    {
        // Arrange
        var seedingContext = CreateAdventureDbContext(_databaseName);
        var addedEntity = await seedingContext.Adventure.AddAsync(HelperTests.Adventure);
        await seedingContext.SaveChangesAsync();
        
        // Act
        var result = await _repository.Contains(addedEntity.Entity.Title);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Contains_ReturnsFalse_IfTitleDoesExists()
    {
        // Arrange
        var seedingContext = CreateAdventureDbContext(_databaseName);
        var addedEntity = await seedingContext.Adventure.AddAsync(HelperTests.Adventure);
        await seedingContext.SaveChangesAsync();
        
        // Act
        var result = await _repository.Contains(Guid.NewGuid().ToString());

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task GetAdventures_ReturnList_AdventureExists()
    {
        // Arrange
        var seedingContext = CreateAdventureDbContext(_databaseName);
        var addedEntity = await seedingContext.Adventure.AddAsync(HelperTests.Adventure);
        await seedingContext.SaveChangesAsync();
        
        // Act
        var actualAdventures = await _repository.GetAdventures(0, 25);

        // Assert
        var actualAdventure = actualAdventures.Adventures.Single();
        actualAdventure.Should().BeEquivalentTo(HelperTests.Adventure, options => options.Excluding(x => x.Id));
    }

    [Fact]
    public async Task GetAdventures_ReturnsEmpty_AdventuresDoesNotExist()
    {
        // Act
        var actualAdventures = await _repository.GetAdventures(0, 25);

        // Assert
        actualAdventures.MaxItems.Should().Be(0);
        actualAdventures.Adventures.Should().BeEmpty();
    }
}