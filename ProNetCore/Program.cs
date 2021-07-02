using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProNetCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //在内部会先配置K8s服务器和IIS相关,其他配置
            IHostBuilder builder = Host.CreateDefaultBuilder(args);
            //启用用户配置的参数,其中包含我们注册的服务/组件以及请求处理管道相关内容
            builder.ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });

            //webhost 用来监听Http请求，就会到Startup类中,
            //在Startup类中有Configure这个方法来可以处理：HTTP请求处理
            IHost webhost = builder.Build();//走到这里的时候会到ConfigureServices()这个方法
            webhost.Run();//Run的时候会到Configure()方法里

            //CreateHostBuilder(args).Build().Run();


            //2种托管模式：1.InProcess(进程内/默认)   2.OutOf(进程外)
            //进程内比进程外提供更好的性能
            // 进程内有一个服务器：要么是IIS,要么是K8s
            // 进程外有2个服务器：一个是内部服务器(K8s),一个是外部服务器(IIS，nginx....)
            //AspNetCore 内置的服务器是K8s 

            //在调试的时候
            //如果选择IIExpress进行调试：当前进程的名字就是IISExpress  在任务管理器里就会找到一个IISEXpress.exe的文件
            //如果选择项目名称ProNetCore进行调试：当前进程的名字就是ProNetCore

            //IISExpress和IIS的区别：
            //1：IISExpress是轻量级版本,针对开发时优化的一个版本，在实际的生产过程中,往往使用的是IIS
        }
        public class MyStartup
        {

           //这个方法是可有可无的、用来进行依赖注入的添加
            public void ConfigureServices(IServiceCollection services)
            {
                services.AddMvc();
                services.AddLogging();
            }
            //Configure类是一定要的,哪怕里面是个空的、用来进行管道，http请求的处理
            public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            {
                //Configure是用来处理中间件的方法  处理Http请求和相应

                //1：在请求之前和之后，执行一些相应的工作
                //2：异常中间件,日志,身份授权,记录请求,静态资源中间件
                //3：这些中间件都是特性功能的组件(类),单一职责

                if (env.IsDevelopment())//判断是否是开发者模式
                {
                    //异常中间件，当我们是开发者模式的情况下，这个组件就会被注册到请求处理管道中
                    app.UseDeveloperExceptionPage();//异常中间件
                }

                app.UseRouting();

                
                //我们在每个中间件当中，处理Http请求，之前或者之后
                //你也可以选择中间传递，传递到下一个中间件，你也可以不传递
                //这个输出了,下面的Hello World!就不会再输出了
                // app.Run(async context => { await context.Response.WriteAsync("Hello!!!!!"); });

                //这里加上Next 下面的Hello World!会输出的
                app.Use(async (context, next) => {
                    await context.Response.WriteAsync("Hello!!!!!");
                    await next();
                });

                app.UseRouting();//路由中间件
                //终结点中间件
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapGet("/", async context =>
                    {
                        await context.Response.WriteAsync("Hello World!");
                    });
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
                    endpoints.MapGet("/", async context =>
                    {
                        await context.Response.WriteAsync($"ProcessName：{System.Diagnostics.Process.GetCurrentProcess().ProcessName}");
                    });
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
