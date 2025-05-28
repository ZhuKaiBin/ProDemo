using CleanArchitecture.Application;
using CleanArchitecture.Application.Interfaces.Persistence;
using CleanArchitecture.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CleanArchitecture
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

            builder.AddApplicationServices();
            builder.AddInfrastructureServices();


            builder.Services.AddApplicationDependencies();
            builder.Services.AddScoped(typeof(IToDoRepository<>), typeof(CommonRepository<>));


            builder.Services.AddDbContext<ToDoDbContext>((serviceProvider, options) =>
            {
                //����������
                var interceptor = serviceProvider.GetRequiredService<ISaveChangesInterceptor>();
                options.UseMySQL("Server=localhost;Port=3306;Database=dev_cdesign666;Uid=root;Pwd=root123456;")
                       .AddInterceptors(interceptor);
            });



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

                #region ���Ի����£����������ʼ����������

                using (var scope = app.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;//�ṩ�ض��ģ����񣩺͹���

                    try
                    {
                        //ָ��Ҫ������ݿ⣬���ṩ����
                        var context = services.GetRequiredService<ToDoDbContext>();
                        // context.Database.Migrate();
                        context.Database.EnsureCreated();
                        //await SeedData.InitializeAsync(services);
                    }
                    catch (Exception ex)
                    {
                        var logger = services.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "��ʼ����������ʱ����. {exceptionMessage}", ex.Message);
                    }
                }

                #endregion ���Ի����£����������ʼ����������
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            app.UseMiddleware<RequestTimingMiddleware>();
            app.Run();
        }
    }
}
