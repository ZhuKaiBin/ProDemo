using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace WebLogging
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        //������ע�ᵽ�� IServiceCollection
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            //logging.AddNLog();
            services.AddLogging();
        }

        //IApplicationBuilder������һ���࣬�����ṩ��������Ӧ�ó��������ܵ��Ļ��ơ�
        //IWebHostEnvironment���ṩ�й�Ӧ�ó��������������е� web ������������Ϣ��
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            ILoggerFactory loggerFactory
        )
        {
            if (env.IsDevelopment())
            {
                //�ӹܵ�����ͬ�����첽 Exception ʵ���������� HTML ������Ӧ
                app.UseDeveloperExceptionPage();
            }
            //�������ڽ� HTTP �����ض��� HTTPS ���м����
            app.UseHttpsRedirection();
            //�� Microsoft.AspNetCore.Routing.EndpointRoutingMiddleware �м�����ӵ�ָ���� IApplicationBuilder �С�
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
