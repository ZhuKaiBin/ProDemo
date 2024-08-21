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
    //参考：https://code-maze.com/working-with-asp-net-core-middleware/
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

            //两个参数 usingmapbranch  +  testquerystring
            app.Map("/usingmapbranch", builder =>
            {
                builder.MapWhen(context => context.Request.Query.ContainsKey("testquerystring"), branch =>
                {
                    branch.Run(async context =>
                    {
                        await context.Response.WriteAsync("1111111\n");
                    });
                });

                //你还可以在这里处理其他没有 testquerystring 的情况
                builder.Run(async context =>
                {
                    await context.Response.WriteAsync("222222\n");
                });
            });


            //https：//localhost：5000/usingmapbranch
            app.Map("/usingmapbranch2", builder =>
            {
                builder.Use(async (context, next) =>
                {
                    await next.Invoke();
                    //await next.Invoke(); 如果这里注释掉，也不会走下面的run
                });
                builder.Run(async context =>
                {

                    await context.Response.WriteAsync("33333333.\n");
                });
            });

            //https：//localhost：5000？testquerystring=test
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
