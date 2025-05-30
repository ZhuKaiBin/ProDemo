﻿
using CleanArchitecture.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoItemController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var ret = await mediator.Send(new ToDoItemQuery());
            return Ok(ret);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateToDoItemRequestDto command)
        {
            await mediator.Send(command);
            return Created();
        }
    }
}
