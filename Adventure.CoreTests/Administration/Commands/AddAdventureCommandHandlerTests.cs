using Adventure.Core;
using Adventure.Core.Administration.Commands;
using Adventure.Core.Profiles;
using Adventure.Core.Repositories;
using Adventure.Domain.Exceptions;
using AutoMapper;
using BaseTests;
using FluentAssertions;
using NSubstitute;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Adventure.CoreTests.Administration.Commands;

public class AddAdventureCommandHandlerTests
{
    private readonly AddAdventureCommandHandler _handler;
    private readonly IAdventureRepository _adventureRepository = Substitute.For<IAdventureRepository>();
    private readonly IMapper _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<DomainProfile>()));

    public AddAdventureCommandHandlerTests()
    {
        _handler = new AddAdventureCommandHandler(_adventureRepository, _mapper);
    }

    [Fact]
    public async Task Handle_ThrowValidationExeption_NullAdventure()
    {
        // Arrange
        var request = new AddAdventureCommand(null);

        // Act and Assert
        await _handler.Invoking(x => x.Handle(request, CancellationToken.None))
            .Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task Handle_ThrowConflictException_TileExistsAlready()
    {
        // Arrange
        var request = new AddAdventureCommand(HelperTests.AdventureDto);
        _adventureRepository.Contains(request.Adventure.Title).Returns(Task.FromResult(true));

        // Act and Assert
        await _handler.Invoking(x => x.Handle(request, CancellationToken.None))
            .Should().ThrowAsync<ConflictException>();
    }

    [Fact]
    public async Task Handle_AddsAdventure_AdventureIsValid()
    {
        // Arrange
        var request = new AddAdventureCommand(HelperTests.AdventureDto);
        _adventureRepository.Contains(request.Adventure.Title).Returns(Task.FromResult(false));
        _adventureRepository.Add(Arg.Is<Domain.Adventure>(x => x.Title == request.Adventure.Title))
            .Returns(Task.FromResult<Domain.Adventure>(HelperTests.Adventure));

        // Act
        var _ = await _handler.Handle(request, CancellationToken.None);

        // Assert
        await _adventureRepository.Received().Contains(request.Adventure.Title);
        await _adventureRepository.Received().Add(Arg.Is<Domain.Adventure>(x => x.Title == request.Adventure.Title));
    }
}