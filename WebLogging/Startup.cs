using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebLogging
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        //将服务注册到中 IServiceCollection
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            
            //logging.AddNLog();
            services.AddLogging();
        }
        //IApplicationBuilder：定义一个类，该类提供用于配置应用程序的请求管道的机制。
        //IWebHostEnvironment：提供有关应用程序正在其中运行的 web 宿主环境的信息。
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                //从管道捕获同步和异步 Exception 实例，并生成 HTML 错误响应
                app.UseDeveloperExceptionPage();
            }
            //添加用于将 HTTP 请求重定向到 HTTPS 的中间件。
            app.UseHttpsRedirection();
            //将 Microsoft.AspNetCore.Routing.EndpointRoutingMiddleware 中间件添加到指定的 IApplicationBuilder 中。
            app.UseRouting();

            app.UseAuthorization();
            loggerFactory.AddNLog();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
