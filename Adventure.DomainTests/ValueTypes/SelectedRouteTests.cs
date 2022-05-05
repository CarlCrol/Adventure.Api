using Adventure.Domain.Exceptions;
using Adventure.Domain.ValueTypes;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace Adventure.DomainTests.ValueTypes;

public class SelectedRouteTests
{
    [InlineData("", "")]
    [InlineData(" ", " ")]
    [InlineData(null, "")]
    [InlineData("", null)]
    [Theory]
    public void Create_ThrowValidationException_DecisionAndCommentIsNullOrEmpty(string decision, string comment)
    {
        // Act and Assert
        FluentActions.Invoking(() => new SelectedRoute(decision, comment)).Should().Throw<ValidationException>();
    }

}