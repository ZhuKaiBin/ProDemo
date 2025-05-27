using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Scalar2Sln_Application.Handler.CreateTodoList;
using Scalar2Sln_Application.Handler.WeatherForecasts.Queries.GetWeatherForecasts;
using Scalar2Sln_Domain.Entities.TodoList;

namespace Scalar2Sln.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class TodoListsController : ControllerBase
    {
        private readonly ILogger<TodoListsController> _logger;
        private readonly IMediator _mediator;
        public TodoListsController(ILogger<TodoListsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }


        [HttpPost(Name = "createTodo")]
        public async Task<Created<int>> createTodo(CreateTodoListCommand command)
        {
            var id = await _mediator.Send(command);


            return TypedResults.Created($"/{nameof(CreateTodoListCommand)}/{id}", id);


        }
    }
}
