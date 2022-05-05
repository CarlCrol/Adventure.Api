using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Adventure.Core.Exceptions;
using Adventure.Core.Profiles;
using Adventure.Core.Repositories;
using Adventure.Core.UserAdventures.Queries;
using Adventure.Domain.Exceptions;
using AutoMapper;
using BaseTests;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Adventure.CoreTests.UserAdventures.Queries;

public class GetUserAdventuresQueryHandlerTests
{
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IUserAdventureRepository _userAdventureRespository = Substitute.For<IUserAdventureRepository>();
    private readonly IMapper _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<DomainProfile>()));
    private readonly GetUserAdventuresQueryHandler _handler;

    public GetUserAdventuresQueryHandlerTests()
    {
        _handler = new GetUserAdventuresQueryHandler(_userRepository, _userAdventureRespository, _mapper);
    }

    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    [Theory]
    public async Task Handle_ThrowValidationExeption_UsernameIsEmptyOrNull(string username)
    {
        // Arrange
        var request = new GetUserAdventuresQuery(username, 1, 25);

        // Act and Assert
        await _handler.Invoking(x => x.Handle(request, CancellationToken.None))
        .Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task Handle_ThrowNotFoundException_UsernameDoesNotExist()
    {
        // Arrange
        var request = new GetUserAdventuresQuery("not found", 1 , 25);
        _userRepository.Contains(request.Username).Returns(Task.FromResult(false));

        // Act and Assert
        await _handler.Invoking(x => x.Handle(request, CancellationToken.None))
        .Should().ThrowAsync<NotFoundException>();        
    }

    [Fact]
    public async Task Handle_ThrowsValidationException_CurrentPageIsZero()
    {
        // Arrange
        var request = new GetUserAdventuresQuery("test.com", 0, 25);

        // Act and Assert
        await _handler.Invoking(x => x.Handle(request, CancellationToken.None))
        .Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task Handle_ThrowsValidationException_MaxItemsIsZero()
    {
        // Arrange
        var request = new GetUserAdventuresQuery("test.com", 10, 0);

        // Act and Assert
        await _handler.Invoking(x => x.Handle(request, CancellationToken.None))
        .Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task Handle_ReturnsUserAdventures_UserHasAdventures()
    {
        // Arrange
        var request = new GetUserAdventuresQuery("test.com", 1, 25);
        _userRepository.Contains(request.Username).Returns(Task.FromResult(true));
        _userAdventureRespository.GetUserAdventureAggregates(request.Username, 0, 25)
            .Returns(Task.FromResult<(IEnumerable<Domain.UserAdventureAggregate> UserAdventureAggregates, int MaxItems)>((new List<Domain.UserAdventureAggregate>()
            {
                HelperTests.UserAdventureAggregate
            }, 1)));

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        var selectedRoutes = response.Data.SelectedRouteResponses.Single().SelectedRoutes;
        selectedRoutes.Should().BeEquivalentTo(HelperTests.UserAdventureAggregate.SelectedRoutes);
    }
}