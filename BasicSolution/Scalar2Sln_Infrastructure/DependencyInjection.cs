using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Scalar2Sln_Application.Common.Interfaces;
using Scalar2Sln_Domain.Constants;
using Scalar2Sln_Infrastructure.Data;
using Scalar2Sln_Infrastructure.Data.Interceptors;
using Scalar2Sln_Infrastructure.Identity;
using Scalar2Sln_Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


namespace Scalar2Sln_Infrastructure
{
    public static class DependencyInjection
    {
        /// <summary>
        /// 这个用于基础架构的构建，数据库，Redis,日志.....
        /// </summary>
        /// <param name="builder"></param>
        public static void AddInfrastructureServices(this IHostApplicationBuilder builder)
        {

            var connectionString = builder.Configuration.GetConnectionString("CleanArchitectureSolutionTemplateDb");
            Guard.Against.Null(connectionString, message: "Connection string 'CleanArchitectureSolutionTemplateDb' not found.");


            //实体审计
            builder.Services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();

            //领域事件的分发
            builder.Services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();



            builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                options.UseSqlServer(connectionString).AddAsyncSeeding(sp);
            });


            builder.Services.AddScoped<IApplicationDbContext>(_serviceProvider => _serviceProvider.GetRequiredService<ApplicationDbContext>());

            //
            builder.Services.AddScoped<ApplicationDbContextInitialiser>();


            builder.Services
                         .AddDefaultIdentity<ApplicationUser>()
                         .AddRoles<IdentityRole>()
                         .AddEntityFrameworkStores<ApplicationDbContext>();


            //TimeProvider.System 是 TimeProvider 类的一个静态只读属性，代表系统默认的时间提供者。
            //它底层调用的是系统的时钟，也就是调用 DateTime.Now 和 DateTime.UtcNow 获取时间。
            //它能提供两个时间：
            //GetUtcNow() 返回的是 UTC时间（协调世界时），和 DateTime.UtcNow 等价。
            //GetNow() 返回的是 本地时间，等同于 DateTime.Now，即系统当前设置的时区时间。
            builder.Services.AddSingleton(TimeProvider.System);



            builder.Services.AddTransient<IIdentityService, IdentityService>();


            builder.Services.AddAuthorization(configure =>
            configure.AddPolicy(Policies.CanPurge, policy => policy.RequireRole(Roles.Administrator)));


            builder.Services.AddTransient<IEmailSender, EmailSender>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "yourapi",
                    ValidAudience = "yourapi",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisIsASecretKeyForJwtTokenWhichIsAtLeast32Bytes"))
                };
            });



            //自动为你定义的每个权限（比如 View、Edit、Delete）创建一个门卫规则（策略 Policy），以后你就可以用这些策略来控制每个接口了。
            // [Authorize(Policy = "Edit")]
            // public IActionResult EditUser() => Ok("编辑用户成功");
            //“必须带有通行证 Permission=Edit 的人，才能进入这个方法。”
            builder.Services.AddAuthorization(ops =>
            {
                foreach (var permission in Permissions.All)
                {
                    ops.AddPolicy(permission, policy => policy.RequireClaim(CustomClaimTypes.Permission, permission));
                }
            });

        }

    }
}
