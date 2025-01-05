
using Microsoft.Extensions.Logging;
using Serilog;
using �׳��쳣ȫ�ֶ���.BaseExceptionFiles;

namespace 抛出异常全局兜底
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // ���� Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug() // ������־����ͼ���
                .WriteTo.Console()     // �����־������̨
                .WriteTo.File("logs/myapp.log", rollingInterval: RollingInterval.Day) // �����־���ļ���ÿ��һ�����ļ�
                .CreateLogger();

            try
            {



                var builder = WebApplication.CreateBuilder(args);

                // ���� Serilog Ϊ��־�ṩ����
                builder.Host.UseSerilog(); // ʹ�� Serilog ��ΪĬ����־��¼��

                // Add services to the container.

                builder.Services.AddControllers();
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                var app = builder.Build();

                // ʹ��ȫ���쳣�����м��
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

                // �����κ�����ʱ��δ�����쳣����¼����־
                Log.Fatal(ex, "Application startup failed");
            }
            finally
            {
                // ȷ����Ӧ�ó����˳�ʱ�ر� Serilog
                Log.CloseAndFlush();
            }
        }
    }
}
