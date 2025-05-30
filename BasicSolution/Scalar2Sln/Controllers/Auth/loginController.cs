﻿using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Scalar2Sln_Infrastructure.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Scalar2Sln.Controllers.Auth
{
    [ApiController]
    [Route("[controller]")]
    public class loginController : ControllerBase
    {
        private readonly ILogger<TodoListsController> _logger;
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public loginController(ILogger<TodoListsController> logger, IMediator mediator
            , UserManager<ApplicationUser> userManager
            , RoleManager<IdentityRole> roleManager
             , SignInManager<ApplicationUser> signInManager)
        {
            _logger = logger;
            _mediator = mediator;
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.UserName);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                return Unauthorized();
            }

            var claims = new List<Claim>
                     {
                         new Claim(ClaimTypes.Name, user.UserName),
                         new Claim(ClaimTypes.NameIdentifier, user.Id)
                     };


            // 获取角色相关的 Claim（权限）
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var roleName in roles)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    claims.AddRange(roleClaims); // 把角色中的权限 Claim 加进去
                }


                claims.Add(new Claim(ClaimTypes.Role, roleName)); // 添加角色 Claim

            }


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisIsASecretKeyForJwtTokenWhichIsAtLeast32Bytes"));
            var token = new JwtSecurityToken(
                issuer: "yourapi",
                audience: "yourapi",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });


        }


        // 退出登录
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("退出登录成功");
        }
    }

    public class LoginDto
    {
        public required string UserName { set; get; }

        public required string Password { set; get; }


    }
}
