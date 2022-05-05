using Adventure.Core.Exceptions;
using Adventure.Core.Repositories;
using Adventure.Dto;
using AutoMapper;

namespace Adventure.Core.Administration.Queries;

public class GetAdventureQueryHandler : IServiceRequestHandler<GetAdventureQuery, AdventureReadModel>
{
    private readonly IAdventureRepository _adventureRepository;
    private readonly IMapper _mapper;

    public GetAdventureQueryHandler(IAdventureRepository adventureRepository, IMapper mapper)
    {
        _adventureRepository = adventureRepository;
        _mapper = mapper;
    }

    public async Task<ServiceResponse<AdventureReadModel>> Handle(GetAdventureQuery request, CancellationToken cancellationToken)
    {
        var adventure = await _adventureRepository.GetById(request.AdventureId);
        if(adventure == null)
        {
            throw new NotFoundException($"{request.AdventureId} does not exist");
        }

        return new ServiceResponse<AdventureReadModel>()
        {
            Data = _mapper.Map<AdventureReadModel>(adventure)
        };
    }
}

public class GetAdventureQuery : IServiceRequest<AdventureReadModel>
{
    public int AdventureId { get; }

    public GetAdventureQuery(int adventureId)
    {
        AdventureId = adventureId;
    }
}

