using FastEndpoints;
using FastEndpoints.Swagger.Swashbuckle;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using Swashbuckle.AspNetCore.SwaggerGen;
using FastEndpoints.ApiExplorer;

namespace FastEndpointsWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            builder.Services.AddFastEndpoints();
            builder.Services.AddFastEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
              
                c.OperationFilter<FastEndpointsOperationFilter>();
            });

            var app = builder.Build();



            app.UseFastEndpoints();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {

                app.UseSwagger();
                app.UseSwaggerUI(c => {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                    c.DocExpansion(DocExpansion.List);
                });
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
