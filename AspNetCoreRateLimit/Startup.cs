using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreRateLimit
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddOptions();
            services.AddMemoryCache();
            services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimit"));
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // 使用自定义中间件
            app.UseRequestIP();
            app.UseRequestPara();
            // 向应用程序的请求管道中添加一个Func委托，这个委托其实就是所谓的中间件。
            // context参数是HttpContext，表示HTTP请求的上下文对象
            // next参数表示管道中的下一个中间件委托,如果不调用next，则会使管道短路
            // 用Use可以将多个中间件链接在一起
            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync(text: "hello Use1\r\n");
                // 调用下一个委托
                await next();
            });          


            // Run方法向应用程序的请求管道中添加一个RequestDelegate委托
            // 放在管道最后面，终端中间件
            app.Run(async context =>
            {
                await context.Response.WriteAsync(text: "Hello World1\r\n");              
            });
          


            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseIpRateLimiting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }







    }
}
