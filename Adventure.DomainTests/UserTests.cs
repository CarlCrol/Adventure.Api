using Adventure.Domain.Exceptions;
using FluentAssertions;
using Xunit;

namespace Adventure.DomainTests;

public class UserTests
{
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    [Theory]
    public void Create_ThrowValidationException_InvalidUsername(string username)
    {
        // Act and Assert
        FluentActions.Invoking(() => new Domain.User(username)).Should().Throw<ValidationException>();
    }
}