using MediatR;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.CQRS;
using WebAPI.CQRS.Models;

namespace WebAPI.Infrastructure
{
    public class UserIdPipe<TIn, TOut> : IPipelineBehavior<TIn, TOut>
    {
        private HttpContext httpContext;

        public UserIdPipe(IHttpContextAccessor accessor)
        {
            httpContext = accessor.HttpContext;
        }

        public async Task<TOut> Handle(
            TIn request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TOut> next)
        {
            //var userId = httpContext.User.Claims
            //    .FirstOrDefault(x => x.Type.Equals(ClaimTypes.NameIdentifier))
            //    .Value;

            if (request is BaseRequest br)
            {
                br.UserId = "wooop";
            }

            var result = await next();

            if (result is Response<Car> carResponse)
            {
                carResponse.Data.Name += " CHECKEED";
            }

            return result;
        }
    }
}
