using DotNetify;
using DotNetify.Pulse;
using DotNetify_Pulse.BackgroundServices;


namespace DotNetify_Pulse;

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
        builder.Services.AddSignalR();
        builder.Services.AddDotNetify();
        builder.Services.AddDotNetifyPulse();
        builder.Services.AddHostedService<Worker>();


        var app = builder.Build();

        // ��� WebSocket ֧��
        app.UseWebSockets();
        // ע�� DotNetify �м��
        app.UseDotNetify();           // ������
        app.UseDotNetifyPulse();      // �����壨Pulse��

        app.UseStaticFiles();
        app.MapDefaultEndpoints();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();
        // SignalR Hub ӳ�䣨DotNetifyHub �Ǳ���ģ�
        app.MapHub<DotNetifyHub>("/dotnetify");
        //app.UseDotNetifyPulse(config => config.UIPath = Directory.GetCurrentDirectory() + "\\pulse-ui");

        app.UseDotNetifyPulse(config => config.UIPath = Path.Combine(Directory.GetCurrentDirectory(), "pulse-ui")
);

        app.Run();
    }
}
