using MediatorDemo.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MediatorDemo
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
            //builder.Services.AddSingleton<IServices>(provider => new Services_A());
            //builder.Services.AddSingleton<IServices>(provider => new Services_B());

            //builder.Services.AddSingleton<IServices, Services_A>();
            //builder.Services.AddSingleton<IServices, Services_B>();



            builder.Services.AddSingleton<IServices>(new Services_A());
            builder.Services.AddSingleton<IServices>(new Services_B());



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
