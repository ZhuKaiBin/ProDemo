using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

            // 使锟斤拷锟皆讹拷锟斤拷锟叫硷拷锟?
            app.UseRequestIP();
            app.UseRequestPara();
            // 锟斤拷应锟矫筹拷锟斤拷锟斤拷锟斤拷锟杰碉拷锟斤拷锟斤拷锟揭伙拷锟紽unc委锟叫ｏ拷锟斤拷锟轿拷锟斤拷锟绞碉拷锟斤拷锟斤拷锟轿斤拷锟斤拷屑锟斤拷锟斤拷
            // context锟斤拷锟斤拷锟斤拷HttpContext锟斤拷锟斤拷示HTTP锟斤拷锟斤拷锟斤拷锟斤拷锟斤拷亩锟斤拷锟?
            // next锟斤拷锟斤拷锟斤拷示锟杰碉拷锟叫碉拷锟斤拷一锟斤拷锟叫硷拷锟轿拷锟?锟斤拷锟斤拷锟斤拷锟斤拷锟絥ext锟斤拷锟斤拷锟绞癸拷艿锟斤拷锟铰?
            // 锟斤拷Use锟斤拷锟皆斤拷锟斤拷锟斤拷屑锟斤拷锟斤拷锟斤拷锟斤拷一锟斤拷
            app.Use(
                async (context, next) =>
                {
                    await context.Response.WriteAsync(text: "hello Use1\r\n");
                    // 锟斤拷锟斤拷锟斤拷一锟斤拷委锟斤拷
                    await next();
                }
            );

            // Run锟斤拷锟斤拷锟斤拷应锟矫筹拷锟斤拷锟斤拷锟斤拷锟杰碉拷锟斤拷锟斤拷锟揭伙拷锟絉equestDelegate委锟斤拷
            // 锟斤拷锟节管碉拷锟斤拷锟斤拷妫拷斩锟斤拷屑锟斤拷
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
