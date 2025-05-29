using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Scalar2Sln_Application.Handler.CreateTodoList;
using Scalar2Sln_Infrastructure.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Scalar2Sln.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class TodoListsController : ControllerBase
    {
        private readonly ILogger<TodoListsController> _logger;
        private readonly IMediator _mediator;     
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public TodoListsController(ILogger<TodoListsController> logger, IMediator mediator          
            , UserManager<ApplicationUser> userManager
            , RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _mediator = mediator;
            _roleManager = roleManager;
            _userManager = userManager;
        }


        [HttpPost(Name = "createTodo")]
        public async Task<Created<int>> createTodo(CreateTodoListCommand command)
        {
            var id = await _mediator.Send(command);


            return TypedResults.Created($"/{nameof(CreateTodoListCommand)}/{id}", id);

        }

        [Authorize(Roles = "省长,主席,县长")]
        //[Authorize(Policy = "Permission.View")]
        [HttpGet("test-view-permission")]
        public IActionResult TestViewPermission()
        {
            return Ok("你有查看权限，可以访问这个接口。");
        }
    }
}
