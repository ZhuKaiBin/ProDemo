using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace ProTask
{
    class Program
    {
        static async Task Main(string[] args)
        {
            DateTime dt1 = DateTime.Now;
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId + "sssssssssssssssssssssssss" + "：" + Thread.CurrentThread.IsThreadPoolThread);
            await AsyncMethod();

            DateTime dt2 = DateTime.Now;
            TimeSpan ts = dt1 - dt2;
            double t = ts.TotalMilliseconds;

            Console.WriteLine(+Thread.CurrentThread.ManagedThreadId + "耗时" + t);
            Console.ReadKey();

        }


        public static async Task AsyncMethod()
        {

            Console.WriteLine("aaaaaaaaaaaaaaaaaaaaaaaaaaaa" + Thread.CurrentThread.ManagedThreadId + "：" + Thread.CurrentThread.IsThreadPoolThread);
            await Task.Run(() =>
            {

                for (int i = 0; i < 5; i++)
                {
                    HttpClient client = new HttpClient();
                    Console.WriteLine(client.GetAsync("https://www.baidu.com/?tn=80035161_1_dg").Result);

                }
                Console.WriteLine("ccccccccccccccccccccccccccc" + Thread.CurrentThread.ManagedThreadId + "：" + Thread.CurrentThread.IsThreadPoolThread);
            });

            Console.WriteLine("xxxxxxxxxxxxxxxxxxx" + Thread.CurrentThread.ManagedThreadId + "：" + Thread.CurrentThread.IsThreadPoolThread);
        }

    }
}
