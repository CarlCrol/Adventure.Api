using Adventure.Core.Exceptions;
using Adventure.Core.Profiles;
using Adventure.Core.Repositories;
using AutoMapper;
using BaseTests;
using FluentAssertions;
using NSubstitute;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Adventure.Core.Administration.Queries.Tests;

public class GetAdventureQueryHandlerTests
{
    private readonly IAdventureRepository _repository = Substitute.For<IAdventureRepository>();
    private readonly IMapper _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<DomainProfile>()));
    private readonly GetAdventureQueryHandler _handler;
    

    public GetAdventureQueryHandlerTests()
    {
        _handler = new GetAdventureQueryHandler(_repository, _mapper);
    }

    [Fact]
    public async Task Handle_ThrowNotFoundExecption_AdventureDoesNotExist()
    {
        // Act
        var adventureId = 1;
        var request = new GetAdventureQuery(adventureId);
        _repository.GetById(adventureId).Returns(Task.FromResult<Domain.Adventure?>(null));

        // Act and Assert
        await _handler.Invoking(x => x.Handle(request, CancellationToken.None))
            .Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_ReturnsAdventureReadModel_AdventureExist()
    {
        // Act
        var adventureId = 1;
        var request = new GetAdventureQuery(adventureId);
        _repository.GetById(adventureId).Returns(Task.FromResult<Domain.Adventure?>(HelperTests.Adventure));

        // Act
        var actualAdventureReadModel = await _handler.Handle(request, CancellationToken.None);

        // Assert
        actualAdventureReadModel.Should().NotBeEquivalentTo(HelperTests.Adventure);
    }
}