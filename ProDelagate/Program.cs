using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ProDelagate
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");

            //HostBuilder builder = new HostBuilder();
            //builder.ConfigureServices((h, services) =>
            //{

            //    var str = h.Configuration.GetSection("");

            //    services.AddSingleton<>();//Microsoft.Extensions.DependencyInjection
            //});

            //builder.ConfigureLogging((h, logger) =>
            //{
            //    logger.AddConsole();
            //});

            //var host = builder.Build();
            //using (host)
            //{
            //    host.Run();//运行程序
            //}

            Test test = new Test();
            test.print += p1;

            test.start();

            Console.ReadKey();
        }

        static void p1()
        {
            Console.WriteLine("输出第一段字符串");
        }

        static void p2()
        {
            Console.WriteLine("输出第2段字符串");
        }

        public delegate void Print();

        class Test
        {
            public event Print print;

            public void start()
            {
                print();
            }
        }
    }
}
