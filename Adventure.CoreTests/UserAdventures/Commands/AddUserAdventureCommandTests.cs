using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Adventure.Core.Exceptions;
using Adventure.Core.Profiles;
using Adventure.Core.Repositories;
using Adventure.Core.UserAdventures.Commands;
using Adventure.Domain.Exceptions;
using Adventure.Dto;
using AutoMapper;
using BaseTests;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Adventure.CoreTests.UserAdventures.Commands;

public class AddUserAdventureCommandTests
{
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IAdventureRepository _adventureRepository = Substitute.For<IAdventureRepository>();
    private readonly IUserAdventureRepository _userAdventureRepository = Substitute.For<IUserAdventureRepository>();
    private readonly IMapper _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<DomainProfile>()));
    private readonly AddUserAdventureCommandHandler _handler;
    private readonly Domain.UserAdventureAggregate _userAdventureAggregate = HelperTests.UserAdventureAggregate;
    private readonly List<Dto.SelectedRoute> _selectedRoutes = HelperTests.SelectedRoutesDto;

    public AddUserAdventureCommandTests()
    {
        _handler = new AddUserAdventureCommandHandler(_userRepository, _adventureRepository, _userAdventureRepository, _mapper);
    }

    [Fact]
    public async Task Handle_ThrowValidationException_SelectedRoutesAreEmpty()
    {
        // Arrange
        var request = new AddUserAdventureCommand(1, 1, new List<SelectedRoute>());
        _userRepository.GetById(request.UserId).Returns(Task.FromResult<Domain.User?>(HelperTests.User));
        _adventureRepository.GetById(request.AdventureId).Returns(Task.FromResult<Domain.Adventure?>(HelperTests.Adventure));

        // Act and Assert
        await _handler.Invoking(x => x.Handle(request, CancellationToken.None)).Should().ThrowAsync<ValidationException>();
        await _userRepository.DidNotReceive().GetById(request.UserId);
        await _adventureRepository.DidNotReceive().GetById(request.AdventureId);
    }

    [Fact]
    public async Task Handle_ThrowNotFoundException_UserDoesNotExist()
    {
        // Arrange
        var request = new AddUserAdventureCommand(1, 1, _selectedRoutes);
        _userRepository.GetById(request.UserId).Returns(Task.FromResult<Domain.User?>(null));
        _adventureRepository.GetById(request.AdventureId).Returns(Task.FromResult<Domain.Adventure?>(HelperTests.Adventure));

        // Act and Assert
        await _handler.Invoking(x => x.Handle(request, CancellationToken.None)).Should().ThrowAsync<NotFoundException>();
        await _userRepository.Received().GetById(request.UserId);
        await _adventureRepository.DidNotReceive().GetById(request.AdventureId);
    }

    [Fact]
    public async Task Handle_ThrowNotFoundException_AdventureDoesNotExist()
    {
        // Arrange
        var request = new AddUserAdventureCommand(1, 1, _selectedRoutes);
        _userRepository.GetById(request.UserId).Returns(Task.FromResult<Domain.User?>(HelperTests.User));
        _adventureRepository.GetById(request.AdventureId).Returns(Task.FromResult<Domain.Adventure?>(null));

        // Act and Assert
        await _handler.Invoking(x => x.Handle(request, CancellationToken.None)).Should().ThrowAsync<NotFoundException>();
        await _userRepository.Received().GetById(request.UserId);
        await _adventureRepository.Received().GetById(request.AdventureId);
    }

    [Fact]
    public async Task Handle_UserAdventureCreated_ValidParameters()
    {
        // Arrange
        var request = new AddUserAdventureCommand(1, 1, _selectedRoutes);
        _userRepository.GetById(request.UserId).Returns(Task.FromResult<Domain.User?>(HelperTests.User));
        _adventureRepository.GetById(request.AdventureId).Returns(Task.FromResult<Domain.Adventure?>(HelperTests.Adventure));
        _userAdventureRepository.Add(Arg.Any<Domain.UserAdventureAggregate>()).Returns(Task.FromResult(HelperTests.UserAdventureAggregate));

        // Act
        var _ = _handler.Handle(request, CancellationToken.None);

        // Assert
        await _userRepository.Received().GetById(request.UserId);
        await _adventureRepository.Received().GetById(request.AdventureId);
        await _userAdventureRepository.Received().Add(Arg.Is<Domain.UserAdventureAggregate>(x => x.User.Username == HelperTests.User.Username && x.Adventure.Title == HelperTests.Adventure.Title));

    }
}