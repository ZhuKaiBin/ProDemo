
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

            // ע�����Է���
            builder.Services.AddSingleton<IRetryService, RetryService>();

            // ע�� HttpClient �Ͷ�·������
            builder.Services.AddHttpClient<ICircuitBreakerService, CircuitBreakerService>();

            // ע�� HttpClient �ͻ��˷���
            builder.Services.AddHttpClient<IFallbackService, FallbackService>();


            //ע�ᳬʱʱ�� ITimeoutService
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
