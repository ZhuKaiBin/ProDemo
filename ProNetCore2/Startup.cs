using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ProNetCore2
{
    //�ο���https://code-maze.com/working-with-asp-net-core-middleware/
    public class Startup
    {

        public void ConfigureServices(IServiceCollection services) { }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("middleware1_In\n");
                await next();
                await context.Response.WriteAsync("middleware1_Out\n");
            });

            //�������� usingmapbranch  +  testquerystring
            app.Map("/usingmapbranch", builder =>
            {
                builder.MapWhen(context => context.Request.Query.ContainsKey("testquerystring"), branch =>
                {
                    branch.Run(async context =>
                    {
                        await context.Response.WriteAsync("1111111\n");
                    });
                });

                //�㻹���������ﴦ������û�� testquerystring �����
                builder.Run(async context =>
                {
                    await context.Response.WriteAsync("222222\n");
                });
            });


            //https��//localhost��5000/usingmapbranch
            app.Map("/usingmapbranch2", builder =>
            {
                builder.Use(async (context, next) =>
                {
                    await next.Invoke();
                    //await next.Invoke(); �������ע�͵���Ҳ�����������run
                });
                builder.Run(async context =>
                {

                    await context.Response.WriteAsync("33333333.\n");
                });
            });

            //https��//localhost��5000��testquerystring=test
            app.MapWhen(context => context.Request.Query.ContainsKey("testquerystring"), builder =>
            {
                builder.Run(async context =>
                {
                    await context.Response.WriteAsync(".\n");
                });
            });


            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("middleware666_In\n");
                await next();
                await context.Response.WriteAsync("middleware888_Out\n");
            });

            app.Run(async context =>
            {

                await context.Response.WriteAsync("middleware3_In\n");

            });

        }
    }
}
