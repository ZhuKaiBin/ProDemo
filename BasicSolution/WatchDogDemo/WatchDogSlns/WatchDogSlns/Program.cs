using Microsoft.OpenApi.Writers;
using WatchDog;
using WatchDog.src.Enums;
namespace WatchDogSlns;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddWatchDogServices(op => {
            op.IsAutoClear = true;
            op.ClearTimeSchedule = WatchDogAutoClearScheduleEnum.Monthly;
        });

        var app = builder.Build();

        app.MapDefaultEndpoints();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseWatchDogExceptionLogger();
        app.UseWatchDog(opt => { 
            opt.WatchPageUsername = "admin";
            opt.WatchPagePassword= "admin";
        });

        app.MapControllers();

        app.Run();
    }
}
