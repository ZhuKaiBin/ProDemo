using Aop.Api;
using Aop.Api.Request;
using Aop.Api.Response;
using System;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace AliPay
{
    class Program
    {

        private readonly static object obj = new object();
        static ThreadLocal<string> local;

        public class Birld
        { 
            public string Eat { set; get; }
            public void Fly()
            { }
        }

        public class A : Birld
        {
            public void Run()
            { 
            
            }
        }





        static void Main(string[] args)
        {
            var num = Convert.ToDecimal("0.0008");
            var s1 = decimal.Round(num, 2);//这个Round(数字,小数点后几位),2就是后2位 0.0008==》0.02
            var s2 = decimal.Round(num, 3);//3就是后三位 0.0008==》0.001  因为第四位是8，四舍五入一下
            var s3 = decimal.Round(num, 1);//1就是后一位,0.0008======>0.0

            int v = (int)decimal.Round(Convert.ToDecimal("1.0"), 0);

            Console.WriteLine(DateTime.Now);
            Console.WriteLine(TimeZoneInfo.Local);
            Console.WriteLine(TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));


            DateTime Chtime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));
            Console.WriteLine(Chtime);
            var time= new DateTimeOffset(Chtime, TimeSpan.FromHours(8));
            Console.WriteLine(time);
            #region


            A a = new A();
            a.Fly();

            string qqq = "";

            //unsafe
            //{
            //DateTime是值类型,datetime1 是开辟了一个地址
            //datetime2 = datetime1  将datetime1赋值给datetime2  datetime2 又开辟了一个新的空间
            //datetime1 和datetime2  是两个不同的地址内存空间
            //所以改变了time1的值,是不会改变time2的值

            //这就是值类型的特点 ：开辟俩地址空间
            //var datetime1 = DateTime.Now;
            //var datetime2 = datetime1;

            //DateTime* p1 = &datetime1;
            //DateTime* p2 = &datetime2;

            //datetime1 = datetime1.AddDays(3);
            //DateTime* p3 = &datetime1;
            //DateTime* p4 = &datetime2;



            //int str1 = 100;
            //int str2 = str1;

            //int* s1 = &str1;
            //int* s2 = &str2;

            //str1 = str1 + 100;

            //int* s3 = &str1;
            //int* s4 = &str2;

            //};

            //Func<string> func = () => "委托";
            //Func<string,string> 是传入一个string的参数 (s) 就是参数  返回值是=>后面的
            //Expression<Func<string,string>> func_expression = (s) => s+"委托";
            //Console.WriteLine(func_expression.Compile().Invoke("zhu"));



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
            ////这里是将timeout 看看是否可以转换成int类型, 这里也考察了out的用法 不需要提前声明
            //int.TryParse(timeout, out int i);
            //if (i == 0) i = 3;
            //Console.WriteLine(i);

            ////remote master
            //string expiry = "1635471114";
            //DateTimeOffset ss = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(expiry));
            //DateTimeOffset time = DateTimeOffset.Now;
            //if (ss < time)
            //{

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
            //dog1.color = "黑色2";
            //dog1.Run();
            //dog1.WangWang();
            //Console.WriteLine(dog1.color.ToString());
#endregion
            Console.ReadLine();


        }

        public static void test1()
        {
            Thread t = new Thread(()=> {

                lock (obj)
                {
                    Console.WriteLine("1212121");
                    Thread.Sleep(500);
                }
            });
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



        //抽象就好比是总公司定义几个规则,而且也设定了几个方法(不用abstract修饰)，子公司要继承abstract修饰的方法
        //对于总公司设定的方法 可用可不用

        //抽象admin   抽象里可以有方法体
        //抽象类里可以有非抽象的的方法   但是抽象方法必须要在抽象类中
        public abstract class admin
        {
            public abstract void Run();

            public void BOB()
            {
                Console.WriteLine("BOB");
            }

            public abstract void Fly();
        }

        //接口admin  接口中不能有方法体
        public interface Iadmin
        {
            void Run();
            void Fly();

            string color { set; get; }
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

            private string _color;
            public string color
            {
                get { return _color; }
                set
                {
                    if (value == "黑色")
                    {
                        _color = "bob";
                    }
                    else
                    {
                        _color = "6666666";
                    }
                }
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
