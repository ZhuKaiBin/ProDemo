using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            services
                .AddAuthentication(defaultScheme: "Cookies")
                .AddCookie(option =>
                {
                    option.LoginPath = new Microsoft.AspNetCore.Http.PathString(
                        value: "/Authorization/Index"
                    );
                });

            #region ����
            services.AddAuthorization(configure =>
            {
                //SVIP�ǲ��Ե�����
                configure.AddPolicy(
                    name: "SVIP",
                    option =>
                    {
                        //SVIP�ǽ�ɫ
                        option.RequireRole(roles: "SVIP"); //�û��Ľ�ɫ������SVIP,���ܷ���SVIP Page
                        //option.RequireRole(roles: "admin");�����˼�Ǵ������Ĳ��Ե�ֵ������SVIP��Admin
                    }
                );
                configure.AddPolicy(
                    name: "VIP",
                    option =>
                    {
                        //���ǽ�ɫ"SVIP", "VIP" �����Է��ʲ���ΪVIP Page
                        option.RequireRole("VIP", "SVIP");
                    }
                );
                configure.AddPolicy(
                    name: "NoVIP",
                    option =>
                    {
                        //���ǽ�ɫ"NoVIP", "VIP" �����Է��ʲ���ΪNoVIP Page
                        option.RequireRole("NoVIP", "VIP", "SVIP");
                    }
                );
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
            //ʹ�ü�Ȩ(������֤)��ָ��֤�û��Ƿ�ӵ�з���ϵͳ��Ȩ��
            app.UseAuthentication();
            //ʹ����Ȩ
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );
            });
        }
    }
}
