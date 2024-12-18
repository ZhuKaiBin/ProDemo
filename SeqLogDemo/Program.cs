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
                //���Seq
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
                app.UseSwagger();  // ���� Swagger
                app.UseSwaggerUI();  // ���� Swagger UI
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();
            //����м����������־��¼
            app.UseSerilogRequestLogging();
            // ȷ��ʹ��·�ɲ����ÿ�����ӳ��
            app.MapControllers();
            app.Run();
        }
    }
}