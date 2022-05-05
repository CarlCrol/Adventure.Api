using System.Threading;
using System.Threading.Tasks;
using Adventure.Core;
using Adventure.Core.Administration;
using Adventure.Core.Administration.Commands;
using Adventure.Core.Profiles;
using Adventure.Core.Repositories;
using Adventure.Domain.Exceptions;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Adventure.CoreTests.Administration.Commands;

public class AddUserCommandHandlerTests
{
    private readonly IMapper _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<DomainProfile>()));
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly AddUserCommandHandler _handler;

    public AddUserCommandHandlerTests()
    {
        _handler = new AddUserCommandHandler(_userRepository, _mapper);
    }

    [Fact]
    public async Task Handle_ThrowValidationException_UserIsNull()
    {
        // Act and Assert
        await _handler.Invoking(x => x.Handle(new AddUserCommand(null), CancellationToken.None)).Should()
                .ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task Handle_ThrowConflictException_UsernameExistsAlready()
    {
        // Arrange
        var username = "test.com";
        var request = new AddUserCommand(new Dto.User(username));
        _userRepository.Contains(username).Returns(Task.FromResult(true));

        // Act and Assert
        await _handler.Invoking(x => x.Handle(request, CancellationToken.None))
            .Should().ThrowAsync<ConflictException>();
    }

    [Fact]
    public async Task Handle_AddsUser_UserIsValid()
    {
        // Arrange
        var username = "test.com";
        var request = new AddUserCommand(new Dto.User(username));
        _userRepository.Contains(username).Returns(Task.FromResult(false));
        _userRepository.Add(Arg.Is<Domain.User>(x => x.Username == username)).Returns(Task.FromResult(new Domain.User(username)));

        // Assert
        var _ = await _handler.Handle(request, CancellationToken.None);

        // Assert
        await _userRepository.Received().Contains(username);
        await _userRepository.Received().Add(Arg.Is<Domain.User>(x => x.Username == username));

    }
}