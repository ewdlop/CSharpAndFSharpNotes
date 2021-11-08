using MediatR;

namespace WebAPI.CQRS
{
    public interface IRequestWrapper<T> : IRequest<Response<T>>
    { }

    public interface IHandlerWrapper<TIn, TOut> : IRequestHandler<TIn, Response<TOut>>
        where TIn : IRequestWrapper<TOut>
    { }
}
