using System;
using System.Threading.Tasks;
using Adventure.Infrastructure.Repositories;
using BaseTests;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Adventure.InfrastructureTests.Repositories;

public class UserRepositoryTests : BaseInfrastructureTests
{
    private readonly string _databaseName = Guid.NewGuid().ToString();
    private readonly UserRepository _repository;

    public UserRepositoryTests()
    {
        _repository = new UserRepository(CreateAdventureDbContext(_databaseName));
    }

    [Fact]
    public async Task GetById_ReturnsUser_IfUserExists()
    {
        // Arrange
        var seedingContext = CreateAdventureDbContext(_databaseName);
        var addedEntity = await seedingContext.User.AddAsync(HelperTests.User);
        await seedingContext.SaveChangesAsync();
        
        // Act
        var actualEntity = await _repository.GetById(addedEntity.Entity.Id);

        // Assert
        actualEntity.Should().BeEquivalentTo(HelperTests.User, options => options.Excluding(x => x.Id));
    }

    [Fact]
    public async Task GetById_ReturnsNull_IfUserDoesNotExists()
    {        
        // Act
        var actualUser = await _repository.GetById(int.MaxValue);

        // Assert
        actualUser.Should().BeNull();
    }

    [Fact]
    public async Task Add_ReturnsAddedEntity_SuccessfulAdd()
    {
        // Act
        var addedUser = await _repository.Add(HelperTests.User);

        // Assert
        var assertContext = CreateAdventureDbContext(_databaseName);
        var actualUser = await assertContext.User.SingleAsync();
        addedUser.Should().BeEquivalentTo(actualUser);
    }

    [Fact]
    public async Task Contains_ReturnsTrue_IfUsernameExists()
    {
        // Arrange
        var seedingContext = CreateAdventureDbContext(_databaseName);
        var addedEntity = await seedingContext.User.AddAsync(HelperTests.User);
        await seedingContext.SaveChangesAsync();
        
        // Act
        var result = await _repository.Contains(addedEntity.Entity.Username.ToUpper());

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Contains_ReturnsFalse_IfUsernameDoesExists()
    {
        // Arrange
        var seedingContext = CreateAdventureDbContext(_databaseName);
        var addedEntity = await seedingContext.User.AddAsync(HelperTests.User);
        await seedingContext.SaveChangesAsync();
        
        // Act
        var result = await _repository.Contains(Guid.NewGuid().ToString());

        // Assert
        result.Should().BeFalse();
    }
}