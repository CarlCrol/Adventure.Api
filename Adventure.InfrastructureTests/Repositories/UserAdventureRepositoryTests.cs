using System;
using System.Linq;
using System.Threading.Tasks;
using Adventure.Infrastructure.Repositories;
using BaseTests;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Adventure.InfrastructureTests.Repositories;

public class UserAdventureRepositoryTests : BaseInfrastructureTests
{
    private readonly string _databaseName = Guid.NewGuid().ToString();
    private readonly UserAdventureRepository _repository;

    public UserAdventureRepositoryTests()
    {
        _repository = new UserAdventureRepository(CreateAdventureDbContext(_databaseName));
    }

    [Fact]
    public async Task GetById_ReturnsUserAdventure_IfUserAdventureExists()
    {
        // Arrange
        var seedingContext = CreateAdventureDbContext(_databaseName);
        var addedEntity = await seedingContext.UserAdventureAggregate.AddAsync(HelperTests.UserAdventureAggregate);
        await seedingContext.SaveChangesAsync();
        
        // Act
        var actualUserAdventureAggregate = await _repository.GetById(addedEntity.Entity.Id);

        // Assert
        actualUserAdventureAggregate.Should().NotBeNull();
        actualUserAdventureAggregate.User.Should().NotBeNull();
        actualUserAdventureAggregate.Adventure.Should().NotBeNull();
        actualUserAdventureAggregate.Should().BeEquivalentTo(HelperTests.UserAdventureAggregate, options => 
        options.Excluding(x => x.Id).Excluding(x => x.AdventuredOn).Excluding(x => x.UserId).Excluding(x => x.User.Id).Excluding(x => x.AdventureId).Excluding(x => x.Adventure.Id));
    }

    [Fact]
    public async Task GetUserAdventureAggregates_ReturnsList_UsernameExist()
    {
        // Arrange
        var seedingContext = CreateAdventureDbContext(_databaseName);
        var addedEntity = await seedingContext.UserAdventureAggregate.AddAsync(HelperTests.UserAdventureAggregate);
        await seedingContext.SaveChangesAsync();
        
        // Act
        var actualUserAdventureAggregates = await _repository.GetUserAdventureAggregates(HelperTests.UserAdventureAggregate.User.Username, 0, 25);

        // Assert
        var actualUserAdventureAggregate = actualUserAdventureAggregates.UserAdventureAggregates.Single();
        actualUserAdventureAggregate.Should().NotBeNull();
        actualUserAdventureAggregate.User.Should().NotBeNull();
        actualUserAdventureAggregate.Adventure.Should().NotBeNull();
        actualUserAdventureAggregate.Should().BeEquivalentTo(HelperTests.UserAdventureAggregate, options => 
        options.Excluding(x => x.Id).Excluding(x => x.AdventuredOn).Excluding(x => x.UserId).Excluding(x => x.User.Id).Excluding(x => x.AdventureId).Excluding(x => x.Adventure.Id));
    }

    [Fact]
    public async Task GetUserAdventureAggregates_ReturnsEmpty_UsernameDoesNotExist()
    {
        // Arrange
        var seedingContext = CreateAdventureDbContext(_databaseName);
        var addedEntity = await seedingContext.UserAdventureAggregate.AddAsync(HelperTests.UserAdventureAggregate);
        await seedingContext.SaveChangesAsync();
        
        // Act
        var actualUserAdventureAggregates = await _repository.GetUserAdventureAggregates("Does not exist", 0, 25);

        // Assert
        actualUserAdventureAggregates.MaxItems.Should().Be(0);
        actualUserAdventureAggregates.UserAdventureAggregates.Should().BeEmpty();
    }

    [Fact]
    public async Task GetById_ReturnsNull_IfUserAdventureDoesNotExists()
    {        
        // Act
        var actualUserAdventureAggregate = await _repository.GetById(int.MaxValue);

        // Assert
        actualUserAdventureAggregate.Should().BeNull();
    }

    [Fact]
    public async Task Add_ReturnsAddedEntity_SuccessfulAdd()
    {
        // Act
        var addedUserAdventure = await _repository.Add(HelperTests.UserAdventureAggregate);

        // Assert
        var assertContext = CreateAdventureDbContext(_databaseName);
        var actualUserAdventure = await assertContext.UserAdventureAggregate.SingleAsync();
        addedUserAdventure.Should().BeEquivalentTo(actualUserAdventure, options => options.Excluding(x => x.User).Excluding(x => x.Adventure));
    }

}