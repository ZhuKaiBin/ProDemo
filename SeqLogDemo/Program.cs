using Serilog;

namespace SeqLogDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // Add services to the container.
            builder.Services.AddAuthorization();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddRazorPages();
            builder.Services.AddEndpointsApiExplorer();

            builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
            {
                loggerConfiguration
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Debug()
                //添加Seq
                .WriteTo.Seq("http://localhost:5341", apiKey: "k6DagIXlErCaxjZ8A7B3");
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UseSwagger();  // 启用 Swagger
                app.UseSwaggerUI();  // 启用 Swagger UI
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();
            //添加中间件简化请求日志记录
            app.UseSerilogRequestLogging();
            // 确保使用路由并启用控制器映射
            app.MapControllers();
            app.Run();
        }
    }
}