using Microsoft.EntityFrameworkCore;
using EventSourceDemo.EfDbContexts;
using Microsoft.Extensions.Options;
using EventSourceDemo.EventStores;
using EventSourceDemo.Services.Interfaces;
using EventSourceDemo.Services;
namespace EventSourceDemo
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
            builder.Services.AddDbContext<EfDbContext>(options =>
                     options.UseMySql(
            "Server=localhost;Port=3306;Database=devEventSourceDemo;Uid=root;Pwd=root123456;",
            new MySqlServerVersion(new Version(5, 7, 44))  // 指定 MySQL 版本号
            ));

            builder.Services.AddScoped<IEventStore, EfEventStore>();  // 事件存储接口实现
            builder.Services.AddScoped<IOrderService, OrderService>();
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


            if (app.Environment.IsDevelopment())
            {
                using (var scope = app.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;

                    try
                    {
                        var context = services.GetRequiredService<EfDbContext>();
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



            }

            app.Run();
        }
    }
}
