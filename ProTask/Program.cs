using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace ProTask
{
    class Program
    {
        static void Main(string[] args)
        {

            string num = "";
            for (int i=0;i<10;i++)
            {
               
                if (i == 5)
                {
                    continue;
                }
                else
                {
                    num = num + i.ToString();
                }  

            }

            //var task = Task.Run(() => { Console.WriteLine("task：" + Thread.CurrentThread.ManagedThreadId); });

            //task.Wait();
            //var task2 = new Task(() => { Console.WriteLine("task2：" + Thread.CurrentThread.ManagedThreadId); });
            //task2.Start();


            //var factory = Task<int>.Factory.StartNew(getint); ;
            //var ss = factory.Result;
            //var s = factory.GetAwaiter().GetResult();

            //int para = 10088;
            //var tasknum = Task<int>.Run(() => { return getint(para); }).Result;


            //Func<string, string,string> func = (m, n) => m + n;

            //func("bob", "zhu");



            XmlDocument xd = new XmlDocument();
            xd.LoadXml("<Person><name> 诸葛亮 </name></Person>");
            var element = xd.DocumentElement.ToString();

            string s = xd.DocumentElement.InnerXml;


            var bob = name.bob;
            int tomnum = (int)name.tom;


        Console.ReadKey();
        }

        enum name
        {
            bob,
            tom
        };
        public static int getint(int para)
    {
        return para;
    }

}
}
