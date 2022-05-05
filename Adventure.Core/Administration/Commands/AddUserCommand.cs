using Adventure.Core.Repositories;
using Adventure.Domain.Exceptions;
using Adventure.Dto;
using AutoMapper;

namespace Adventure.Core.Administration.Commands;

public class AddUserCommandHandler : IServiceRequestHandler<AddUserCommand, int>
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public AddUserCommandHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<ServiceResponse<int>> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        if(request.User == null)
        {
            throw new ValidationException($"{nameof(request.User)} cannot be null.");
        }

        if(await _userRepository.Contains(request.User.Username))
        {
            throw new ConflictException($"{request.User.Username} already exists.");
        }

        var user = _mapper.Map<Domain.User>(request.User);
        user = await _userRepository.Add(user);
        
        return new ServiceResponse<int>()
        {
            Data = user.Id
        };
    }
}

public class AddUserCommand : IServiceRequest<int>
{
    public Dto.User User { get; }

    public AddUserCommand(User user)
    {
        User = user;
    }
}