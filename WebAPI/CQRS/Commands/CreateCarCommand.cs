using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.CQRS.Models;

namespace WebAPI.CQRS.Commands
{
    public class CreateCarCommand : IRequestWrapper<Car> { }
    public class CreateCarCommandHandler : IHandlerWrapper<CreateCarCommand, Car>
    {
        public Task<Response<Car>> Handle(CreateCarCommand request, CancellationToken cancellationToken)
        {
            if (false)
            {
                return Task.FromResult(Response.Fail<Car>("already exists"));
            }

            return Task.FromResult(Response.Ok(new Car { Name = "Mazda" }, "Car Created"));
        }
    }
}
