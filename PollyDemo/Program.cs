
using PollyDemo.Services;

namespace PollyDemo
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

            builder.Services.AddHttpClient();

            // 注册重试服务
            builder.Services.AddSingleton<IRetryService, RetryService>();

            // 注册 HttpClient 和断路器服务
            builder.Services.AddHttpClient<ICircuitBreakerService, CircuitBreakerService>();

            // 注册 HttpClient 和回退服务
            builder.Services.AddHttpClient<IFallbackService, FallbackService>();


            //注册超时时间 ITimeoutService
            builder.Services.AddHttpClient<ITimeoutService, TimeoutService>();


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
