using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ProNetCore
{
    public class Startup
    {
        //������������
        //services��Microsoft.Extensions.DependencyInjection.ServiceCollection
        public void ConfigureServices(IServiceCollection services) { }

        //����HTTP�������ܵ����е�һЩ���ã���Ȩ�ޣ�����־֮���?
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet(
                    "/",
                    async context =>
                    {
                        await context.Response.WriteAsync($"����ʱ�Ļ���ʱ,{env.EnvironmentName}");
                    }
                );
            });
        }
    }
}
