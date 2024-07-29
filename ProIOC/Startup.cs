using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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
            /*services.AddTransient<ITestServices, TestServiceTemp>();*/
            //����ÿ�ε���TestServiceTemp,�����´���һ��TestServiceTempʵ��
            //services.AddSingleton<ITestServices, TestServiceTemp>();//Singleton "������,��һ����;����ģʽ"  ÿ�ε���TestServiceTemp,����ͬһ��TestServiceTempʵ��
            //services.AddScoped<ITestServices, TestServiceTemp>();//����ÿ�ε���TestServiceTemp,�����´���һ��TestServiceTempʵ��
        }

        //����һ�������������ú���,������actofac��������
        public void ConfigureContainer(ContainerBuilder builder) //Autofac.ContainerBuilder
        {
            //����ֻע�������(��)
            //builder.RegisterType<TestServiceTemp>();

            //����ע���˷���(�ӿ�)//SingleInstance() �ǵ���ģʽ//InstancePerLifetimeScope() ������
            //builder.RegisterType<TestServiceTemp>().As<ITestServices>().InstancePerLifetimeScope();
            builder.RegisterType<TestServiceTemp>().As<ITestServices>();

            //builder.RegisterType<ConstructorTemp>().As<IConstructor>();
            builder.RegisterType<ConstructorTemp>().As<IConstructor>().UsingConstructor();
            //UsingConstructor�ؼ����ǿ���ָ�����캯������ע��
            //����˵����ע���һ�����캯���д������ģ��в��������ģ�
            //�����������ָ���޲�����
            //������������������־����в���


            //������ָ���ض���ʵ��������ע��
            //var Instance = new ConstructorTemp();
            //builder.RegisterInstance(Instance).As<IConstructor>();

            //���滹��lamda����ʽ����ע��

            //RegisterType��ע��һ����
            //AS������ע��Ϊ�˽ӿ�
            //SingleInstance() �ǵ���ģʽ  ȫ��ֻ��һ��
            //InstancePerLifetimeScope() ������ ��ͬһ��������,��������ֻ����һ��
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
