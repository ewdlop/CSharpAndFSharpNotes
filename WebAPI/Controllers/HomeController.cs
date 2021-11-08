using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.CQRS;
using WebAPI.CQRS.Commands;
using WebAPI.CQRS.Models;
using WebAPI.CQRS.Queries;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("cars")]
    public class HomeController : Controller
    {
        private readonly IMediator mediator;

        public HomeController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public Task<IEnumerable<Car>> Index()
        {
            return mediator.Send(new GetAllCarsQuery());
        }


        [HttpPost]
        public Task<Response<Car>> Index([FromBody] CreateCarCommand command)
        {
            return mediator.Send(command);
        }
    }
}
