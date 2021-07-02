using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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

namespace ProIOC
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
            /*services.AddTransient<ITestServices, TestServiceTemp>();*///这里每次调用TestServiceTemp,都是新创建一个TestServiceTemp实例
            //services.AddSingleton<ITestServices, TestServiceTemp>();//Singleton "单个的,单一序列;单例模式"  每次调用TestServiceTemp,都是同一个TestServiceTemp实例
            //services.AddScoped<ITestServices, TestServiceTemp>();//这里每次调用TestServiceTemp,都是新创建一个TestServiceTemp实例
        }
        //添加一个对容器的配置函数,再来对actofac进行添加
        public void ConfigureContainer(ContainerBuilder builder)//Autofac.ContainerBuilder
        {
            //这是只注入了组件(类)
            //builder.RegisterType<TestServiceTemp>();

            //这是注入了服务(接口)//SingleInstance() 是单例模式//InstancePerLifetimeScope() 作用域
            //builder.RegisterType<TestServiceTemp>().As<ITestServices>().InstancePerLifetimeScope();
            builder.RegisterType<TestServiceTemp>().As<ITestServices>();

            //builder.RegisterType<ConstructorTemp>().As<IConstructor>();
            builder.RegisterType<ConstructorTemp>().As<IConstructor>().UsingConstructor();
            //UsingConstructor关键字是可以指定构造函数进行注册
            //就是说，你注册的一个构造函数有带参数的，有不带参数的，
            //可以用这个字指定无参数；
            //如果多个参数不带这个字就是有参数


            //还可以指定特定的实例来进行注入
            //var Instance = new ConstructorTemp();
            //builder.RegisterInstance(Instance).As<IConstructor>();

            //后面还是lamda表达式进行注入

            //RegisterType：注册一个类
            //AS：将类注册为此接口
            //SingleInstance() 是单例模式  全局只有一个
            //InstancePerLifetimeScope() 作用域 在同一个作用域,服务请求只创建一次
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
