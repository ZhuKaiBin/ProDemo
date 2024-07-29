using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ProNetCore2
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services) { }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.Use(async (context, next) =>
            //{
            //    await context.Response.WriteAsync("middleware1_In\n");
            //    await next();
            //    await context.Response.WriteAsync("middleware1_Out\n");
            //});
            //// next()�����ã��Ƿ������¸��м��
            //app.Use(async (context, next) =>
            //{
            //    await context.Response.WriteAsync("middleware2_In\n");
            //    //await next();//������ｫNext()��ע�͵�����Run()���м���ǲ��������==����֮Ϊ��·����
            //    //middleware1_In ===��middleware2_In ===��middleware2_Out ===��middleware1_Out
            //    //�Ͳ��������������middleware3_In��
            //    await context.Response.WriteAsync("middleware2_Out\n");
            //});

            ////��Run()(��·�м��,��ʼ����)��������Ϳ�ʼ������
            //app.Run(async context =>
            //{
            //    await context.Response.WriteAsync("middleware3_In\n");

            //});
            ////middleware1_In
            ////middleware2_In
            ////middleware3_In
            ////middleware2_Out
            ////middleware1_Out
        }
    }
}
