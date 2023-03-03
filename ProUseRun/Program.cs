using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ProUseRun
{
    class Program
    {
        static void Main(string[] args)
        {
            var basePath = Directory.GetCurrentDirectory();

            //1.主要是读取配置参数
            var host = new HostBuilder();               

            var host2 = host.ConfigureAppConfiguration((hostContext, configApp) =>
                  {
                      configApp.AddJsonFile("appsettings.json", optional: true);
                      configApp.AddJsonFile(
                          $"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json",
                          optional: true);
                      configApp.AddEnvironmentVariables(prefix: "PREFIX_");
                      configApp.AddCommandLine(args);
                  });
            //注入服务
            var host3 = host2.ConfigureServices((hostContext, services) =>
                   {
                       //注册后台普通服务
                       // services.AddSingleton<IJobTimeService, JobTimeService>();
                       //注册后台THostedService类型服务
                       services.AddHostedService<LifetimeEventsHostedService>();
                       services.AddHostedService<TimedHostedService>();
                   });


            host2.ConfigureLogging((a,b) => {


                b.AddConsole();

            });

            //启动日志
            var host4 = host3.ConfigureLogging((hostContext, configLogging) =>
              {
                  configLogging.AddConsole();
                  configLogging.AddDebug();

              });

            //侦听 Ctrl+C 或 SIGTERM 并调用 StopApplication() 来启动关闭进程。 这将解除阻止 RunAsync 和 WaitForShutdownAsync 等扩展。
            var host5 = host4.UseConsoleLifetime();

            host3.Build().Run();

            //实例化注入的普通服务
            // IJobTimeService job = host.Services.GetRequiredService<IJobTimeService>();
            // job.Time();

            //运行【应用程序】并阻止调用线程，直到主机关闭
            //host6.Run();
        }
    }


    public class LifetimeEventsHostedService : IHostedService//为主机所管理的对象定义方法
    {
        private readonly ILogger _logger;
        private readonly IApplicationLifetime _appLifetime;//允许使用者在正常关闭过程中执行清理

        public LifetimeEventsHostedService(ILogger<LifetimeEventsHostedService> logger, IApplicationLifetime appLifetime)
        {
            _logger = logger;
            _appLifetime = appLifetime;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _appLifetime.ApplicationStarted.Register(OnStarted);
            _appLifetime.ApplicationStopping.Register(OnStopping);
            _appLifetime.ApplicationStopped.Register(OnStopped);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
        private void OnStarted()
        {
            _logger.LogInformation("OnStarted 开始了");

            // Perform post-startup activities here
        }

        private void OnStopping()
        {
            _logger.LogInformation("OnStopping 快结束了");

            // Perform on-stopping activities here
        }

        private void OnStopped()
        {
            _logger.LogInformation("OnStopped 结束结束了");

            // Perform post-stopped activities here
        }
    }

    /// <summary>
    /// 后台定时服务
    /// </summary>
    internal class TimedHostedService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private Timer _timer;

        public TimedHostedService(ILogger<TimedHostedService> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is starting 开始了.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }
        /// <summary>
        /// 每隔5秒执行一次
        /// </summary>
        /// <param name="state"></param>
        private void DoWork(object state)
        {
            _logger.LogInformation("Timed Background Service is working 在工作了.");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is stopping 停止了.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }


}
