using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Context;
using WebApplication1.Controllers;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddControllers().AddApplicationPart(typeof(WeatherForecastController).Assembly);

            //1.集成Identity上下文
            builder.Services.AddDbContext<MesDbContext>(op =>
            {
                //2.集成mysql
                op.UseMySql("Server=localhost;Port=3306;Database=MES_Identity;uid=root;Pwd=123456;",
                              new MySqlServerVersion(new Version(8, 0, 39))); // 设置 MySQL 版本号
            });

            //注册IUserManager
            builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<MesDbContext>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UseSwagger();  // 启用 Swagger
                app.UseSwaggerUI();  // 启用 Swagger UI
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();
            // 确保使用路由并启用控制器映射
            app.MapControllers();
            app.Run();
        }
    }
}