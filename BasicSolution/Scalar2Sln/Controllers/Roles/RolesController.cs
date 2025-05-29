using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Scalar2Sln_Infrastructure.Data;
using Scalar2Sln_Infrastructure.Identity;

namespace Scalar2Sln.Controllers.Roles
{
    [ApiController]
    [Route("[controller]")]
    public class RolesController : ControllerBase
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _dbContext;
        public RolesController(UserManager<ApplicationUser> userManager
            , SignInManager<ApplicationUser> signInManager
            , RoleManager<IdentityRole> roleManager
            , ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
        }


        [Authorize(Roles = "省长,主席")]
        [Authorize(Policy = "Permission.Create")]
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



        [HttpGet("user-roles/{username}")]
        public async Task<IActionResult> GetUserRoles(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound($"用户 '{username}' 不存在。");
            }

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new
            {
                Username = username,
                Roles = roles
            });
        }



        [HttpPost("createRole")]
        //[Authorize(Roles = "省长,主席")] // 可选：限制只有“省长”或“主席”可以创建角色
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.RoleName))
            {
                return BadRequest("角色名称不能为空。");
            }

            // 检查是否已存在
            if (await _roleManager.RoleExistsAsync(dto.RoleName))
            {
                return BadRequest($"角色“{dto.RoleName}”已经存在。");
            }

            var result = await _roleManager.CreateAsync(new IdentityRole(dto.RoleName));
            //throw new Exception("9090");

            if (result.Succeeded)
            {
                return Ok($"角色“{dto.RoleName}”创建成功。");
            }

            return StatusCode(500, $"角色创建失败：{string.Join(", ", result.Errors.Select(e => e.Description))}");
        }


    }

    public class AssignRolesDto
    {
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
    }

    public class CreateRoleDto
    {
        public string RoleName { get; set; }
    }

}
