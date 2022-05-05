using Adventure.Core.Exceptions;
using Adventure.Core.Repositories;
using Adventure.Domain.Exceptions;
using AutoMapper;

namespace Adventure.Core.UserAdventures.Queries;

public class GetUserAdventuresQueryHandler : IServiceRequestHandler<GetUserAdventuresQuery, GetUserAdventuresResponse>
{
    private readonly IUserRepository _userRespository;
    private readonly IUserAdventureRepository _userAdventureRespository;
    private readonly IMapper _mapper;

    public GetUserAdventuresQueryHandler(IUserRepository userRespository, IUserAdventureRepository userAdventureRespository, IMapper mapper)
    {
        _userRespository = userRespository;
        _userAdventureRespository = userAdventureRespository;
        _mapper = mapper;
    }

    public async Task<ServiceResponse<GetUserAdventuresResponse>> Handle(GetUserAdventuresQuery request, CancellationToken cancellationToken)
    {
        if(request.CurrentPage <= 0)
        {
            throw new ValidationException($"{nameof(request.CurrentPage)} should be greater than 0");
        }

        if(request.MaxPageCount <= 0)
        {
            throw new ValidationException($"{nameof(request.MaxPageCount)} should be greater than 0");
        }

        if(string.IsNullOrWhiteSpace(request.Username))
        {
            throw new ValidationException($"{nameof(request.Username)} can't be null or empty.");
        }

        if(!await _userRespository.Contains(request.Username))
        {
            throw new NotFoundException($"{request.Username} does not exist.");
        }

        var userAdventureAggregates = await _userAdventureRespository.GetUserAdventureAggregates(request.Username, request.CurrentPage - 1, request.MaxPageCount);
        
        var userAdventureAggregatesDto = _mapper.Map<List<Dto.UserAdventure>>(userAdventureAggregates.UserAdventureAggregates);

        return new ServiceResponse<GetUserAdventuresResponse>()
        {
            Data = new GetUserAdventuresResponse(new Dto.User(request.Username), 
                            request.CurrentPage, 
                            userAdventureAggregates.MaxItems, 
                            userAdventureAggregatesDto.Select(x => new SelectedRouteResponse(x.Adventure, x.SelectedRoutes)))
        };
    }
}

public class GetUserAdventuresQuery : IServiceRequest<GetUserAdventuresResponse>
{
    public string Username { get; }
    public int CurrentPage { get; }
    public int MaxPageCount { get; }

    public GetUserAdventuresQuery(string username, int currentPage, int maxPageCount)
    {
        Username = username;
        CurrentPage = currentPage;
        MaxPageCount = maxPageCount;
    }

}

public record SelectedRouteResponse(Dto.Adventure Adventure, IEnumerable<Dto.SelectedRoute> SelectedRoutes);
public record GetUserAdventuresResponse(Dto.User User, int CurrentPage, int MaxItems, IEnumerable<SelectedRouteResponse> SelectedRouteResponses);