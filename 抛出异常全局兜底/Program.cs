
using Microsoft.Extensions.Logging;
using Serilog;
using 抛出异常全局兜底.BaseExceptionFiles;

namespace 鎶涘嚭寮傚父鍏ㄥ眬鍏滃簳
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // 配置 Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug() // 设置日志的最低级别
                .WriteTo.Console()     // 输出日志到控制台
                .WriteTo.File("logs/myapp.log", rollingInterval: RollingInterval.Day) // 输出日志到文件，每天一个新文件
                .CreateLogger();

            try
            {



                var builder = WebApplication.CreateBuilder(args);

                // 配置 Serilog 为日志提供程序
                builder.Host.UseSerilog(); // 使用 Serilog 作为默认日志记录器

                // Add services to the container.

                builder.Services.AddControllers();
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                var app = builder.Build();

                // 使用全局异常处理中间件
                app.UseMiddleware<GlobalExceptionHandlingMiddleware>();


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
            catch (Exception ex)
            {

                // 捕获任何启动时的未处理异常并记录到日志
                Log.Fatal(ex, "Application startup failed");
            }
            finally
            {
                // 确保在应用程序退出时关闭 Serilog
                Log.CloseAndFlush();
            }
        }
    }
}
