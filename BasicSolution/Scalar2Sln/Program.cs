using Scalar.AspNetCore;
using Scalar2Sln.Infrastructure;
using Scalar2Sln_Application;
using Scalar2Sln_Infrastructure;
using Scalar2Sln_Infrastructure.Data;

namespace Scalar2Sln;


//直接启动：https://localhost:7120/scalar/v1#tag/weatherforecast/GET/WeatherForecast
//这个在Net 9中直接就是可以访问的;
//在Net 8中 不太顺畅

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();


        //加入基础设施层的设置
        builder.AddInfrastructureServices();

        //加入Application的依赖注入
        builder.AddApplicationServices();


        builder.AddWebServices();





        var app = builder.Build();

        app.MapDefaultEndpoints();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            //app.MapOpenApi();
            await app.InitialiseDatabaseAsync();
            app.MapScalarApiReference(); // scalar/v1
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapEndpoints();
        app.MapControllers();

        app.Run();
    }
}
