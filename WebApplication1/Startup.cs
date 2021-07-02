using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1
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
            services.AddControllersWithViews();
            services.AddRazorPages();
            //Cookies
            services.AddAuthentication(defaultScheme: "Cookies")
                .AddCookie(option =>
                {
                    option.LoginPath = new Microsoft.AspNetCore.Http.PathString(value: "/Authorization/Index");
                });

            #region 策略
            services.AddAuthorization(configure =>
            {
                //SVIP是策略的名字
                configure.AddPolicy(name: "SVIP",
                    option =>
                     {
                         //SVIP是角色                    
                         option.RequireRole(roles: "SVIP");//用户的角色必须是SVIP,才能访问SVIP Page
                         //option.RequireRole(roles: "admin");这个意思是传过来的策略的值必须是SVIP和Admin
                     }
                     );
                configure.AddPolicy(name: "VIP",
                     option =>
                     {
                         //就是角色"SVIP", "VIP" 都可以访问策略为VIP Page
                         option.RequireRole("VIP","SVIP");
                      });
                configure.AddPolicy(name: "NoVIP",
                    option =>
                    {
                        //就是角色"NoVIP", "VIP" 都可以访问策略为NoVIP Page
                        option.RequireRole("NoVIP", "VIP", "SVIP");
                    });
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            //使用鉴权(身份验证)是指验证用户是否拥有访问系统的权利
            app.UseAuthentication();
            //使用授权
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
