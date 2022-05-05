using Adventure.Core.Exceptions;
using Adventure.Core.Repositories;
using Adventure.Domain.Exceptions;
using Adventure.Domain.ValueTypes;
using AutoMapper;

namespace Adventure.Core.UserAdventures.Commands;

public class AddUserAdventureCommandHandler : IServiceRequestHandler<AddUserAdventureCommand, int>
{
    private readonly IUserRepository _userRepository;
    private readonly IAdventureRepository _adventureRespository;
    private readonly IUserAdventureRepository _userAdventureRepository;
    private readonly IMapper _mapper;

    public AddUserAdventureCommandHandler(IUserRepository userRepository, IAdventureRepository adventureRespository, IUserAdventureRepository userAdventureRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _adventureRespository = adventureRespository;
        _userAdventureRepository = userAdventureRepository;
        _mapper = mapper;
    }

    public async Task<ServiceResponse<int>> Handle(AddUserAdventureCommand request, CancellationToken cancellationToken)
    {
        if(!request.SelectedRoutes?.Any() ?? true)
        {
            throw new ValidationException($"{nameof(request.SelectedRoutes)} can't be null or empty.");
        }

        var user = await _userRepository.GetById(request.UserId);
        if(user == null)
        {
            throw new NotFoundException($"User with id {request.UserId} does not exist.");
        }

        var adventure = await _adventureRespository.GetById(request.AdventureId);
        if(adventure == null)
        {
            throw new NotFoundException($"Adventure with id {request.AdventureId} does not exist.");
        }
        
        var selectedRoutes = _mapper.Map<List<SelectedRoute>>(request.SelectedRoutes);
        var userAdventureAggregate = new Domain.UserAdventureAggregate(user, adventure, DateTime.UtcNow, selectedRoutes);
        userAdventureAggregate = await _userAdventureRepository.Add(userAdventureAggregate);

        return new ServiceResponse<int>()
        {
            Data = userAdventureAggregate.Id
        };
    }
}

public class AddUserAdventureCommand : IServiceRequest<int>
{
    public int UserId { get; }
    public int AdventureId { get; }
    public IEnumerable<Dto.SelectedRoute> SelectedRoutes { get; }

    public AddUserAdventureCommand(int userId, int adventureId, IEnumerable<Dto.SelectedRoute> selectedRoutes)
    {
        UserId = userId;
        AdventureId = adventureId;
        SelectedRoutes = selectedRoutes;
    }

}