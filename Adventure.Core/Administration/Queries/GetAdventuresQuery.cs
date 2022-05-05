using Adventure.Core.Repositories;
using Adventure.Domain.Exceptions;
using AutoMapper;

namespace Adventure.Core.Administration.Queries;

public class GetAdventuresQueryHandler : IServiceRequestHandler<GetAdventuresQuery, GetAdventuresQueryResponse>
{
    private readonly IAdventureRepository _adventuresRespository;
    private readonly IMapper _mapper;

    public GetAdventuresQueryHandler(IAdventureRepository adventuresRespository, IMapper mapper)
    {
        _adventuresRespository = adventuresRespository;
        _mapper = mapper;
    }

    public async Task<ServiceResponse<GetAdventuresQueryResponse>> Handle(GetAdventuresQuery request, CancellationToken cancellationToken)
    {
        if(request.CurrentPage <= 0)
        {
            throw new ValidationException($"{nameof(request.CurrentPage)} should be greater than 0");
        }

        if(request.MaxPageCount <= 0)
        {
            throw new ValidationException($"{nameof(request.MaxPageCount)} should be greater than 0");
        }

        var adventures = await _adventuresRespository.GetAdventures(request.CurrentPage - 1, request.MaxPageCount);
        var adventuresDto = _mapper.Map<List<Dto.AdventureReadModel>>(adventures.Adventures);
        return new ServiceResponse<GetAdventuresQueryResponse>()
        {
            Data = new GetAdventuresQueryResponse(request.CurrentPage, adventures.MaxItems, adventuresDto)
        };
    }
}

public class GetAdventuresQuery : IServiceRequest<GetAdventuresQueryResponse>
{
    public int CurrentPage { get; }
    public int MaxPageCount { get; }

    public GetAdventuresQuery(int currentPage, int maxPageCount)
    {
        CurrentPage = currentPage;
        MaxPageCount = maxPageCount;
    }
}

public record GetAdventuresQueryResponse(int CurrentPage, int MaxItems, IEnumerable<Dto.AdventureReadModel> Adventures);