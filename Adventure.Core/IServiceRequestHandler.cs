using MediatR;

namespace Adventure.Core;

public interface IServiceRequestHandler<TIn, TOut> : IRequestHandler<TIn, ServiceResponse<TOut>>
                        where TIn : IServiceRequest<TOut>
{
}
