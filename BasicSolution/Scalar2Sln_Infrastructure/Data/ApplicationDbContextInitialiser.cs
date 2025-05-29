using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Scalar2Sln_Domain.Constants;
using Scalar2Sln_Domain.Entities.TodoList;
using Scalar2Sln_Infrastructure.Identity;
using System.Security.Claims;
namespace Scalar2Sln_Infrastructure.Data
{
    //应用程序初始化
    public class ApplicationDbContextInitialiser
    {
        private readonly ILogger<ApplicationDbContextInitialiser> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        public async Task InitialiseAsync()
        {
            try
            {
                //await _context.Database.MigrateAsync();
                await _context.Database.EnsureCreatedAsync(); // 一次性创建数据库（开发阶段用）

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initialising the database.");
                throw;
            }

        }


        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }


        //定义一个异步方法，通常用于 应用启动时运行一次，给数据库种子数据（如默认管理员）。
        public async Task TrySeedAsync()
        {

            #region  1.创建角色和权限 

            var rolesAndPermissions = new Dictionary<string, List<string>>
                         {
                             { "主席", Permissions.All },
                             { "省长", new List<string> { Permissions.View, Permissions.Create, Permissions.Update } },
                             { "县长", new List<string> { Permissions.View, Permissions.Create } },
                             { "镇长", new List<string> { Permissions.View } }
                         };

            foreach (var kvp in rolesAndPermissions)
            {
                var roleName = kvp.Key;
                var permissions = kvp.Value;


                //检查角色是否存在
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role==null)
                {
                    role = new IdentityRole(roleName);
                    //如果角色不存在，就创建一个新的角色 ：主席 省长....
                    await _roleManager.CreateAsync(role);
                }


                // 清理旧权限
                var existingClaims = await _roleManager.GetClaimsAsync(role);
                foreach (var claim in existingClaims)
                {
                    await _roleManager.RemoveClaimAsync(role, claim);
                }

                // 添加权限为 Claim
                foreach (var permission in permissions)
                {
                    await _roleManager.AddClaimAsync(role, new Claim(CustomClaimTypes.Permission, permission));
                }
            }

            #endregion

            #region 2.创建默认用户并分配角色和权限
            var user = new ApplicationUser
            {
                UserName = "lisi@localhost",
                Email = "lisi@localhost",
                PhoneNumber="110",
                NickName="李四"
            };

            var existingUser = await _userManager.FindByNameAsync(user.UserName);
            if (existingUser == null)
            {
                var createResult = await _userManager.CreateAsync(user, "Lisi123!");
                if (!createResult.Succeeded)
                {
                    // 输出错误信息
                    var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                    throw new Exception($"创建用户失败: {errors}");
                }

                // 分配角色
                var rolesToAdd = new[] { "县长", "镇长" };

                foreach (var role in rolesToAdd)
                {
                    var exists = await _roleManager.RoleExistsAsync(role);
                    if (!exists)
                        throw new Exception($"角色 {role} 不存在");
                }

                await _userManager.AddToRolesAsync(user, rolesToAdd);
                          
            }

            #endregion






            // Default roles
            // 创建一个角色对象，角色名为 Roles.Administrator（比如常见的是字符串 "Administrator"）
            // IdentityRole 是内置的身份管理角色模型，对应表是 AspNetRoles
            var administratorRole = new IdentityRole(Roles.Administrator);

            if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
            {
                //如果前面判断为 true，说明角色还没有，就创建 "Administrator" 角色
                //AspNetRoles 表中会多出一行 "Administrator"。
                await _roleManager.CreateAsync(administratorRole);
            }

            // Default users
            //创建一个默认的用户对象，用户名和邮箱都是 "administrator@localhost"
            //ApplicationUser 是你自定义扩展的 IdentityUser 类型。
            var administrator = new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost" };

            //检查这个用户是否已经存在（通过用户名判断）,
            if (_userManager.Users.All(u => u.UserName != administrator.UserName))
            {
                //如果用户不存在，就创建这个用户，并设置默认密码 "Administrator1!"
                await _userManager.CreateAsync(administrator, "Administrator1!");


                //给刚刚创建的用户，添加角色 "Administrator"，这样他就具有管理员权限。
                if (!string.IsNullOrWhiteSpace(administratorRole.Name))
                {
                    //使用 Usermanager 的 AddToRolesAsync 方法,用户加入某个角色（比如管理员）
                    await _userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
                }
            }

            // Default data
            // Seed, if necessary
            if (!_context.TodoLists.Any())
            {
                _context.TodoLists.Add(new TodoList
                {
                    Title = "Todo List",
                    Items =
                {
                    new TodoItem { Title = "Make a todo list 📃" },
                    new TodoItem { Title = "Check off the first item ✅" },
                    new TodoItem { Title = "Realise you've already done two things on the list! 🤯"},
                    new TodoItem { Title = "Reward yourself with a nice, long nap 🏆" },
                }
                });

                await _context.SaveChangesAsync();
            }
        }
    }

    public static class InitialiserExtensions
    {
        public static void AddAsyncSeeding(this DbContextOptionsBuilder builder, IServiceProvider serviceProvider)
        {

            builder.UseAsyncSeeding(async (context, _, ct) =>
            {
                var initialiser = serviceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

                await initialiser.SeedAsync();
            });

        }

        public static async Task InitialiseDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

            await initialiser.InitialiseAsync();
        }
    }


   




}
