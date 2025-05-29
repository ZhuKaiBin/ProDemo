using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Scalar2Sln_Infrastructure.Data;
using Scalar2Sln_Infrastructure.Identity;

namespace Scalar2Sln.Controllers.Users
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
       
        public UsersController(UserManager<ApplicationUser> userManager
            , SignInManager<ApplicationUser> signInManager
            , RoleManager<IdentityRole> roleManager
          )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
           
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
                    NickName = model.NickName
                    // 这里如果你有自定义字段，比如昵称，可以一起赋值
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    // 返回错误信息
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return BadRequest(ModelState);
                }
                // 默认分配 "用户" 角色
                var defaultRole = "用户"; // 你需要提前创建好这个角色并给它权限
                if (!await _roleManager.RoleExistsAsync(defaultRole))
                {
                    await _roleManager.CreateAsync(new IdentityRole(defaultRole));
                    // 这里可以给角色分配权限Claim，示例中不写了
                }
                await _userManager.AddToRoleAsync(user, defaultRole);
                // 注册成功，直接返回成功信息
                return Ok("注册成功");
          
        }


        // 退出登录
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("退出登录成功");
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

   
}
