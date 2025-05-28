using CleanArchitecture.Application;
using CleanArchitecture.Application.Interfaces.Persistence;
using CleanArchitecture.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CleanArchitecture
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.AddApplicationServices();
            builder.AddInfrastructureServices();


            builder.Services.AddApplicationDependencies();
            builder.Services.AddScoped(typeof(IToDoRepository<>), typeof(CommonRepository<>));


            builder.Services.AddDbContext<ToDoDbContext>((serviceProvider, options) =>
            {
                //启用拦截器
                var interceptor = serviceProvider.GetRequiredService<ISaveChangesInterceptor>();
                options.UseMySQL("Server=localhost;Port=3306;Database=dev_cdesign666;Uid=root;Pwd=root123456;")
                       .AddInterceptors(interceptor);
            });



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

                #region 测试环境下，创建服务初始化种子数据

                using (var scope = app.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;//提供特定的（服务）和工具

                    try
                    {
                        //指定要这个数据库，来提供服务
                        var context = services.GetRequiredService<ToDoDbContext>();
                        // context.Database.Migrate();
                        context.Database.EnsureCreated();
                        //await SeedData.InitializeAsync(services);
                    }
                    catch (Exception ex)
                    {
                        var logger = services.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "初始化测试数据时出错. {exceptionMessage}", ex.Message);
                    }
                }

                #endregion 测试环境下，创建服务初始化种子数据
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            app.UseMiddleware<RequestTimingMiddleware>();
            app.Run();
        }
    }
}
