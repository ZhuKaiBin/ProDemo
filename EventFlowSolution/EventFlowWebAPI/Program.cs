using EventFlow;
using EventFlow.EventStores;
using EventFlowWebAPI.CommandHandlers;
using EventFlowWebAPI.EventStoreContexts;
using EventFlowWebAPI.EventStores;
using EventFlowWebAPI.ICommands;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace EventFlowWebAPI
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

            builder.Services.AddDbContext<EventStoreContext>(options =>
                       options.UseMySql(Configuration.GetConnectionString("DefaultConnection")));


            builder.Services.AddScoped<IEventStore, MySqlEventStore>();
            builder.Services.AddScoped<ICommandBus, CommandBus>();
            builder.Services.AddScoped<ICommandHandler<CreateOrderCommand>, CreateOrderCommandHandler>();
            builder.Services.AddScoped<ICommandHandler<AddOrderDetailCommand>, AddOrderDetailCommandHandler>();


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
