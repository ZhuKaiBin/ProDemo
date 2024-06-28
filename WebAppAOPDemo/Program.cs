using WebAppAOPDemo.Core;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using WebAppAOPDemo.AufofacIntercepter;
using Microsoft.OpenApi.Models;

namespace WebAppAOPDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddControllers();

            //builder.Services.AddTransient<IUserService, UserService>();

            //dotnet add package Autofac
            //dotnet add package Autofac.Extensions.DependencyInjection
            //置入AutoFac

            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

            // 同时使用IOC自带的和AutoFac 同时生效
            builder.Host.ConfigureContainer<ContainerBuilder>(autofac =>
            {
                //autofac.RegisterType<UserService>().As<IUserService>().InstancePerDependency();
                //EnableInterfaceInterceptors 启用接口拦截器[Autofac.Extras.DynamicProxy]
                autofac.RegisterType<UserService>().As<IUserService>().InstancePerDependency().EnableInterfaceInterceptors();
                autofac.RegisterType<AufofacIntercepterDemo>();
            });

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "My API",
                    Version = "v1",
                    Description = "ASP.NET Core Web API"
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Enable middleware to serve Swagger-generated JSON endpoint
                app.UseSwagger();

                // Enable middleware to serve Swagger UI (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                });
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            // Map Razor Pages
            app.MapRazorPages();

            // Map Controllers
            app.MapControllers();

            app.Run();
        }
    }
}