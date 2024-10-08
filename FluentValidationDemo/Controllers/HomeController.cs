﻿using FluentValidationDemo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FluentValidationDemo.Controllers
{
    [Route("api/Home/[action]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly A _a;
        public HomeController(A a)
        {
            _a = a;
        }

        [HttpPost]
        public IActionResult Register(User newUser)
        {
            var validator = new UserValidator();
            var validationResult = validator.Validate(newUser);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.First().ErrorMessage);
            }

            _a.print();
            return Ok();
        }

        [HttpPost]
        public IActionResult RegisterSec(User newUser)
        {
            // 这里一定要加： builder.Services.AddControllers().AddFluentValidation();
            //这里用来测试 builder.Services.AddTransient<IValidator<User>, UserValidator>();

            return Ok();
        }
    }
}
