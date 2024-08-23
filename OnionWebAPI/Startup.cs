using Domain.Repositories.Interfaces;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OnionWebAPI.Middleware;
using Services;
using Services.Interfances;

namespace OnionWebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            //.AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);

            services.AddSwaggerGen(c =>
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Web", Version = "v1" }));

            services.AddScoped<IServiceManager, ServiceManager>();

            services.AddScoped<IRepositoryManager, RepositoryManager>();

            services.AddDbContextPool<RepositoryDbContext>(builder =>
            {
                var connectionString = Configuration.GetConnectionString("Database");

                builder.UseMySQL("Server=localhost;Port=3306;Database=dev_cdesign2;Uid=root;Pwd=root123456;");
            });

            services.AddTransient<ExceptionHandlingMiddleware>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();

                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web v1"));
            }

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}