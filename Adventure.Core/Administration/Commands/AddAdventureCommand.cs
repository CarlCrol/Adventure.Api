using Adventure.Core.Repositories;
using Adventure.Domain.Exceptions;
using AutoMapper;

namespace Adventure.Core.Administration.Commands;

public class AddAdventureCommandHandler : IServiceRequestHandler<AddAdventureCommand, int>
{
    private readonly IMapper _mapper;
    private readonly IAdventureRepository _adventureRepository;

    public AddAdventureCommandHandler(IAdventureRepository adventureRepository, IMapper mapper)
    {
        _adventureRepository = adventureRepository;
        _mapper = mapper;
    }

    public async Task<ServiceResponse<int>> Handle(AddAdventureCommand request, CancellationToken cancellationToken)
    {
        if(request.Adventure == null)
        {
            throw new ValidationException($"{nameof(request.Adventure)} cannot be null.");
        }

        var adventure = _mapper.Map<Domain.Adventure>(request.Adventure);
        if(await _adventureRepository.Contains(adventure.Title))
        {
            throw new ConflictException($"{adventure.Title} already exists.");
        }
        adventure = await _adventureRepository.Add(adventure);
        return new ServiceResponse<int>()
        {
            Data = adventure.Id
        };
    }
}

public class AddAdventureCommand : IServiceRequest<int>
{
    public Dto.Adventure Adventure { get; }

    public AddAdventureCommand(Dto.Adventure adventure)
    {
        Adventure = adventure;
    }
}