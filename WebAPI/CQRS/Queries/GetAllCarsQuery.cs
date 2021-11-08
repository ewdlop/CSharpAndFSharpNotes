using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.CQRS.Models;

namespace WebAPI.CQRS.Queries
{
    public class GetAllCarsQuery : BaseRequest, IRequest<IEnumerable<Car>> { }

    public class GetAllCarsQueryHandler : IRequestHandler<GetAllCarsQuery, IEnumerable<Car>>
    {
        // do dependency injection here if need
        public GetAllCarsQueryHandler()
        {

        }

        public async Task<IEnumerable<Car>> Handle(GetAllCarsQuery request, CancellationToken cancellationToken)
        {
            // some buisness logic

            return new[] {
                new Car { Name = $"Ford {request.UserId}" },
                new Car { Name = "Toyta" },
            };
        }
    }
}
