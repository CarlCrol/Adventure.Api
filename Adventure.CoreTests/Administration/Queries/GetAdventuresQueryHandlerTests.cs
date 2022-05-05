using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Adventure.Core.Administration.Queries;
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

namespace Adventure.CoreTests.Administration.Queries;

public class GetAdventuresQueryHandlerTests
{
    private readonly IAdventureRepository _adventureRespository = Substitute.For<IAdventureRepository>();
    private readonly IMapper _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<DomainProfile>()));
    private readonly GetAdventuresQueryHandler _handler;

    public GetAdventuresQueryHandlerTests()
    {
        _handler = new GetAdventuresQueryHandler(_adventureRespository, _mapper);
    }

    [Fact]
    public async Task Handle_ThrowsValidationException_CurrentPageIsZero()
    {
        // Arrange
        var request = new GetAdventuresQuery(0, 25);

        // Act and Assert
        await _handler.Invoking(x => x.Handle(request, CancellationToken.None))
        .Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task Handle_ThrowsValidationException_MaxItemsIsZero()
    {
        // Arrange
        var request = new GetAdventuresQuery(10, 0);

        // Act and Assert
        await _handler.Invoking(x => x.Handle(request, CancellationToken.None))
        .Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task Handle_ReturnsAdventures_HasAdventure()
    {
        // Arrange
        var request = new GetAdventuresQuery(1, 25);
        _adventureRespository.GetAdventures(0, 25)
            .Returns(Task.FromResult<(IEnumerable<Domain.Adventure> Adventures, int MaxItems)>((new List<Domain.Adventure>()
            {
                HelperTests.Adventure
            }, 1)));

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        response.Data.Adventures.Should().HaveCount(1);
    }
}