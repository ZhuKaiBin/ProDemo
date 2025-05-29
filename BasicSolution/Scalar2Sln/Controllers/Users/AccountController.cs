using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Scalar2Sln_Infrastructure.Identity;

namespace Scalar2Sln.Controllers.Users
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // 用户注册
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                NickName=model.NickName
                // 这里如果你有自定义字段，比如昵称，可以一起赋值
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                // 注册成功，直接返回成功信息
                return Ok("注册成功");
            }

            // 返回错误信息
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return BadRequest(ModelState);
        }


        // 退出登录
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("退出登录成功");
        }

        [HttpPost("assign-roles")]
        public async Task<IActionResult> AssignRolesToUser([FromBody] AssignRolesDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.UserName);
            if (user == null)
                return NotFound("用户不存在");

            // 清理用户已有角色（如果不想覆盖可以省略这步）
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            // 给用户添加新角色
            var result = await _userManager.AddToRolesAsync(user, dto.Roles);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok($"成功给用户{dto.UserName}分配角色：{string.Join(", ", dto.Roles)}");
        }

    }

    // 注册请求模型
    public class RegisterDto
    {
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }

        public string? NickName { get; set; }
    }

    public class AssignRolesDto
    {
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
    }
}
