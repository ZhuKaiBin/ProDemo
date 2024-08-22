using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ProNetCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //���ڲ���������K8s��������IIS���?��������
            IHostBuilder builder = Host.CreateDefaultBuilder(args);
            //�����û����õĲ���,���а�������ע��ķ���?����Լ��������ܵ��������
            builder.ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });

            //webhost ��������Http���󣬾ͻᵽStartup����,
            //��Startup������Configure������������Դ�����HTTP������
            IHost webhost = builder.Build(); //�ߵ������ʱ��ᵽConfigureServices()�������?
            webhost.Run(); //Run��ʱ��ᵽConfigure()������

            //CreateHostBuilder(args).Build().Run();


            //2���й�ģʽ��1.InProcess(������/Ĭ��)   2.OutOf(������)
            //�����ڱȽ������ṩ���õ�����
            // ��������һ����������Ҫô��IIS,Ҫô��K8s
            // ��������2����������һ�����ڲ�������(K8s),һ�����ⲿ������(IIS��nginx....)
            //AspNetCore ���õķ�������K8s

            //�ڵ��Ե�ʱ��
            //���ѡ��IIExpress���е��ԣ���ǰ���̵����־���IISExpress  �������������ͻ��ҵ�һ��IISEXpress.exe���ļ�
            //���ѡ����Ŀ����ProNetCore���е��ԣ���ǰ���̵����־���ProNetCore

            //IISExpress��IIS������
            //1��IISExpress���������汾,��Կ���ʱ�Ż���һ���汾����ʵ�ʵ�����������?����ʹ�õ���IIS
        }

        public class MyStartup
        {
            //��������ǿ��п��޵ġ�������������ע�������
            public void ConfigureServices(IServiceCollection services)
            {
                services.AddMvc();
                services.AddLogging();
            }

            //Configure����һ��Ҫ��,���������Ǹ��յġ��������йܵ���http����Ĵ���?
            public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            {
                //Configure�����������м���ķ���? ����Http��������?

                //1��������֮ǰ��֮��ִ��һЩ��Ӧ�Ĺ���
                //2���쳣�м��?��־,������Ȩ,��¼����,��̬��Դ�м��?
                //3����Щ�м���������Թ��ܵ����(��),��һְ��

                if (env.IsDevelopment()) //�ж��Ƿ��ǿ�����ģʽ
                {
                    //�쳣�м�����������ǿ�����ģʽ������£��������ͻᱻע�ᵽ�������ܵ���
                    app.UseDeveloperExceptionPage(); //�쳣�м��?
                }

                app.UseRouting();

                //������ÿ���м�����У�����Http����֮ǰ����֮��
                //��Ҳ����ѡ���м䴫�ݣ����ݵ���һ���м������Ҳ���Բ�����?
                //��������,�����Hello World!�Ͳ����������?
                // app.Run(async context => { await context.Response.WriteAsync("Hello!!!!!"); });

                //�������Next �����Hello World!�������?
                app.Use(
                    async (context, next) =>
                    {
                        await context.Response.WriteAsync("Hello!!!!!");
                        await next();
                    }
                );

                app.UseRouting(); //·���м��?
                //�ս���м��
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapGet(
                        "/",
                        async context =>
                        {
                            await context.Response.WriteAsync("Hello World!");
                        }
                    );
                });
            }
        }

        public class Test
        {
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
                            await context.Response.WriteAsync(
                                $"ProcessName��{System.Diagnostics.Process.GetCurrentProcess().ProcessName}"
                            );
                        }
                    );
                });
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
