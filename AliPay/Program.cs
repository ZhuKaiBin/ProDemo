using Aop.Api;
using Aop.Api.Request;
using Aop.Api.Response;
using System;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AliPay
{
    class Program
    {
        static ThreadLocal<string> local;
        static void Main(string[] args)
        {


            Func<string> func = () => "委托";

            Expression<Func<string,string>> func_expression = (s) => s+"委托";


            Console.WriteLine(func_expression.Compile().Invoke("zhu"));






            // Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            // Console.WriteLine(Thread.CurrentThread.Name);
            //Console.WriteLine("Hello World!");

            //string s1 = "test";
            //s1 += "BOB";

            //Console.WriteLine(s1);

            //StringBuilder sb = new StringBuilder();
            //sb.Append("1");
            //sb.Append("2");

            //Console.WriteLine(sb.ToString());

            //StringBuilder builder = new StringBuilder(5);
            //builder.AppendLine("12121");


            //Console.WriteLine(builder.ToString());




            //创建ThreadLocal并提供默认值
            //local = new ThreadLocal<string>(() => "hehe");

            ////修改TLS的线程
            //Thread th = new Thread(() =>
            //{
            //    local.Value = "Mgen";
            //    Display();
            //});

            //th.Start();
            //th.Join();


            //ThreadLocal<string> threadLocal = new ThreadLocal<string>();


            //threadLocal.Value = "zhu";
            //threadLocal.Value = "666";



            //Console.WriteLine("{0} {1}", Thread.CurrentThread.ManagedThreadId, threadLocal.Value);

            //string timeout = "zhukaib";
            //int.TryParse(timeout, out int i);
            //if (i == 0) i = 3;
            //Console.WriteLine(i);

            ////remote master
            //string expiry = "1631774580";
            //DateTimeOffset ss = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(expiry));
            //DateTimeOffset time = DateTimeOffset.UtcNow.AddMinutes(120);
            //if (ss < time)
            //{
            //    string ssss = "";
            //}


            //ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadMethod1), new object());    //参数可选
            //Console.WriteLine("Hello~~~ !");
            //ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadMethod1), new object());    //参数可选
            //Console.WriteLine("Hello1 !");
            //Task task = Task.Run(() => { ThreadMethod1(""); });

            //Console.WriteLine("Hello2 !");
            //var t = new Task(() => { ThreadMethod1(""); });
            //t.Start();
            //Console.ReadKey();

            //Dog dog = new Dog();
            //dog.BOB();
            //dog.Run();
            //dog.Eat();



            //IDog dog1 = new IDog();
            //dog1.Run();
            //dog1.WangWang();
            Console.ReadLine();

           
        }
        public static void ThreadMethod1(object val)
        {
            for (int i = 0; i <= 50; i++)
            {
                if (i % 10 == 0)
                {
                    Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
                }
            }
        }



        public abstract class admin
        {
            public abstract void Run();

            public void BOB()
            {
                Console.WriteLine("BOB");
            }

            public abstract void Fly();
        }

        public interface Iadmin
        {
            void Run();
            void Fly();
        }

        public class IDog : Iadmin
        {
            public void Run()
            {
                Console.WriteLine("Dog Can Run");
            }

            public void Fly()
            {
                throw new NotImplementedException();
            }

            public void WangWang()
            {
                Console.WriteLine("Dog Can WangWang");
            }
        }


        public class Dog : admin
        {
            public override void Run()
            {
                Console.WriteLine("Dog Can Run");
            }

            public void Eat()
            {
                Console.WriteLine("Dog 吃 骨头");
            }

            public override void Fly()
            {
                throw new NotImplementedException();
            }
        }

        public class Bird : admin
        {
            public override void Fly()
            {
                Console.WriteLine("Bird Can Fly");
            }



            public override void Run()
            {
                throw new NotImplementedException();
            }
        }

    }

   
}
