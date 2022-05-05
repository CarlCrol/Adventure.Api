using Adventure.Domain.Exceptions;
using Adventure.Domain.ValueTypes;
using BaseTests;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Adventure.DomainTests;

public class AdventureTests
{
    private readonly List<Route> _routes = HelperTests.Adventure.Routes.ToList();

    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    [Theory]
    public void Create_ThrowValidationException_InvalidTitle(string title)
    {
        // Act and Assert
        FluentActions.Invoking(() => new Domain.Adventure(title, _routes)).Should().Throw<ValidationException>();
    }

    [Theory, MemberData(nameof(InvalidRoutes))]
    public void Create_ThrowValidationException_InvalidRoutes(List<Route> routes)
    {
        // Act and Assert
        FluentActions.Invoking(() => new Domain.Adventure("test", routes)).Should().Throw<ValidationException>();
    }

    [Fact]
    public void Create_Success_ValidParameters()
    {
        // Act and Assert
        FluentActions.Invoking(() => new Domain.Adventure("test", _routes)).Should().NotThrow();
    }


    public static IEnumerable<object?[]> InvalidRoutes =>
       new List<object?[]>
       {
            new object?[] { new List<Route>() },
            new object?[] { null },
            new object?[] { new List<Route>()
            {
                new Route("", "Do I want doughnut?", null),
                new Route("", "Do I want coffee?", new List<Route>()
                {
                    new Route("Yes","",null)
                })
            } },
            new object?[] { new List<Route>()
            {
                new Route("", "Do I want doughnut?", null),
                new Route("", "Do I want coffee?", null)
            } },
            new object?[] { new List<Route>()
            {
                new Route("", "Do I want doughnut?", null)
            } }
       };

}