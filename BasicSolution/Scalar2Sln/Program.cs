
using Scalar.AspNetCore;

namespace Scalar2Sln;


//直接启动：https://localhost:7120/scalar/v1#tag/weatherforecast/GET/WeatherForecast
//这个在Net 9中直接就是可以访问的;
//在Net 8中 不太顺畅

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        app.MapDefaultEndpoints();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            //app.MapOpenApi();
            app.MapScalarApiReference(); // scalar/v1
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
