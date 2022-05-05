using Adventure.Domain.Exceptions;
using Adventure.Domain.ValueTypes;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace Adventure.DomainTests.ValueTypes;

public class RouteTests
{
    [InlineData("", "")]
    [InlineData(" ", " ")]
    [InlineData(null, "")]
    [InlineData("", null)]
    [Theory]
    public void Create_ThrowValidationException_DecisionAndCommentIsNullOrEmpty(string decision, string comment)
    {
        // Act and Assert
        FluentActions.Invoking(() => new Route(decision, comment, null)).Should().Throw<ValidationException>();
    }

    [Theory, MemberData(nameof(ValidParameters))]
    public void Create_Success_ValidParameters(string? decision, string? comment, List<Route>? subRoutes)
    {
        // Act and Assert
        FluentActions.Invoking(() => new Route(decision, comment, subRoutes)).Should().NotThrow();
    }

    public static IEnumerable<object?[]> ValidParameters =>
       new List<object?[]>
       {
            new object?[] { "", "Do I want a donut?", new List<Route>() },
            new object?[] { null, "Do I want a donut?", null },
            new object?[] { "yes", "", new List<Route>() },
            new object?[] { "yes", null, null },
            new object?[] { "yes", "", new List<Route>() { new Route("Yes", null, null), new Route("No", null, null) }},
            new object?[] { "", "Do I want a dounut?", new List<Route>() { new Route("Yes", null, null), new Route("No", null, null) }}
       };

}