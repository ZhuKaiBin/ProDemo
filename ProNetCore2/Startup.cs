using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProNetCore2
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            //app.Use(async (context, next) =>
            //{
            //    await context.Response.WriteAsync("middleware1_In\n");
            //    await next();
            //    await context.Response.WriteAsync("middleware1_Out\n");
            //});
            //// next()的作用：是否请求下个中间件
            //app.Use(async (context, next) =>
            //{
            //    await context.Response.WriteAsync("middleware2_In\n");
            //    //await next();//如果这里将Next()，注释掉下面Run()的中间件是不会请求的==》称之为短路请求
            //    //middleware1_In ===》middleware2_In ===》middleware2_Out ===》middleware1_Out
            //    //就不会再请求下面的middleware3_In了
            //    await context.Response.WriteAsync("middleware2_Out\n");
            //});

            ////从Run()(短路中间件,开始返回)这里请求就开始返回了
            //app.Run(async context =>
            //{
            //    await context.Response.WriteAsync("middleware3_In\n");

            //});
            ////middleware1_In
            ////middleware2_In
            ////middleware3_In
            ////middleware2_Out
            ////middleware1_Out
        }
    }
}
