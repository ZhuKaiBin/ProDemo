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

            // ʹ���Զ����м��
            app.UseRequestIP();
            app.UseRequestPara();
            // ��Ӧ�ó��������ܵ������һ��Funcί�У����ί����ʵ������ν���м����
            // context������HttpContext����ʾHTTP����������Ķ���
            // next������ʾ�ܵ��е���һ���м��ί��,���������next�����ʹ�ܵ���·
            // ��Use���Խ�����м��������һ��
            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync(text: "hello Use1\r\n");
                // ������һ��ί��
                await next();
            });

            // Run������Ӧ�ó��������ܵ������һ��RequestDelegateί��
            // ���ڹܵ�����棬�ն��м��
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