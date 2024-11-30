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

            //1.����Identity������
            builder.Services.AddDbContext<MesDbContext>(op =>
            {
                //2.����mysql
                op.UseMySql("Server=localhost;Port=3306;Database=MES_Identity;uid=root;Pwd=123456;",
                              new MySqlServerVersion(new Version(8, 0, 39))); // ���� MySQL �汾��
            });

            //ע��IUserManager
            builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<MesDbContext>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UseSwagger();  // ���� Swagger
                app.UseSwaggerUI();  // ���� Swagger UI
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();
            // ȷ��ʹ��·�ɲ����ÿ�����ӳ��
            app.MapControllers();
            app.Run();
        }
    }
}