using MediatR;

namespace Adventure.Core;
public interface IServiceRequest<T> : IRequest<ServiceResponse<T>>
{
}