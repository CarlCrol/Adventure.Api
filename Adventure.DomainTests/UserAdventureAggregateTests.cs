using System;
using System.Collections.Generic;
using Adventure.Domain;
using Adventure.Domain.Exceptions;
using Adventure.Domain.ValueTypes;
using BaseTests;
using FluentAssertions;
using Xunit;

namespace Adventure.DomainTests;

public class UserAdventureAggregateTests
{
    private readonly List<SelectedRoute> _selectedRoutes = new List<SelectedRoute>()
    {
        new SelectedRoute("", "Do I want a doughnut?"),
        new SelectedRoute("Yes", null),
        new SelectedRoute("", "Do I Deserve it?"),
        new SelectedRoute("Yes", "Are you sure?"),
        new SelectedRoute("Yes", "Get it.")
    };

    [Fact]
    public void Create_ThrowValidationException_UserIsNull()
    {
        // Act and Assert
        FluentActions.Invoking(() => new UserAdventureAggregate(null, HelperTests.Adventure, DateTime.UtcNow, _selectedRoutes))
                .Should().Throw<ValidationException>();
    }

    [Fact]
    public void Create_ThrowValidationException_AdventureIsNull()
    {
        // Act and Assert
        FluentActions.Invoking(() => new UserAdventureAggregate(HelperTests.User, null, DateTime.UtcNow, _selectedRoutes))
                .Should().Throw<ValidationException>();
    }
    
    [Fact]
    public void Create_ThrowValidationException_SelectedRoutesNull()
    {
        // Act and Assert
        FluentActions.Invoking(() => new UserAdventureAggregate(HelperTests.User, HelperTests.Adventure, DateTime.UtcNow, null))
                .Should().Throw<ValidationException>();
    }

    [Theory, MemberData(nameof(InvalidSelectedRoutes))]
    public void Create_ThrowValidationException_AdventureRouteIsInconsitentWithSelectedRoute(List<SelectedRoute> selectedRoutes)
    {
        // Act and Assert
        FluentActions.Invoking(() => new UserAdventureAggregate(HelperTests.User, HelperTests.Adventure, DateTime.UtcNow, selectedRoutes))
                .Should().Throw<ValidationException>();
    }

    [Fact]
    public void Create_Success_ValidParameters()
    {
        // Act and Assert
        var _ = new UserAdventureAggregate(HelperTests.User, HelperTests.Adventure, DateTime.UtcNow, _selectedRoutes);
    }

    public static IEnumerable<object?[]> InvalidSelectedRoutes =>
       new List<object?[]>
       {
            new object?[] { new List<SelectedRoute>()
            {
                new SelectedRoute("Unknown", null)
            } },
            new object?[] { new List<SelectedRoute>()
            {
                new SelectedRoute("", "Do I want a doughnut?"),
                new SelectedRoute("Yes", null),
                new SelectedRoute("", "Do I Deserve it?"),
                new SelectedRoute("Yes", "Invalid")
            }},
            new object?[] { new List<SelectedRoute>()
            {
                new SelectedRoute("", "Do I want a doughnut?"),
                new SelectedRoute("Yes", null),
                new SelectedRoute("", "Do I Deserve it?"),
                new SelectedRoute("Yes", "Are you sure?"),
                new SelectedRoute("No", "invalid")
            }},
            new object?[] { new List<SelectedRoute>()
            {
                new SelectedRoute("No", "Invalid")
            }},
       };
}