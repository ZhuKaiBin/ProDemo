using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProNetCore
{
    public class Startup
    {


        //配置容器服务
        //services：Microsoft.Extensions.DependencyInjection.ServiceCollection
        public void ConfigureServices(IServiceCollection services)
        {
        }

        
        //配置HTTP请求处理管道当中的一些配置，加权限，加日志之类的
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync($"运行时的环境时,{env.EnvironmentName}");
                });
            });
        }
    }
}
