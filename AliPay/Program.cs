using System;
using System.Diagnostics;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Aop.Api;
using Aop.Api.Request;
using Aop.Api.Response;

namespace AliPay
{
    class Program
    {
        private static readonly object obj = new object();

        public class Birld
        {
            public string Eat { set; get; }

            public void Fly() { }
        }

        public class A : Birld
        {
            public void Run() { }
        }

        private static void DoSomethingLong(string name)
        {
            Console.WriteLine(
                $"****************DoSomethingLong {name} Start {Thread.CurrentThread.ManagedThreadId.ToString("00")} {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}***************"
            );
        }

        static void Main(string[] args)
        {
            #region 查看值类型的地址 unsafe
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

            //var ddd= "1".Equals(1);


            //datetime1 = datetime1.AddDays(3);
            //DateTime* p3 = &datetime1;
            //DateTime* p4 = &datetime2;

            //person person = new person();
            //person.name = "ys";
            //person person2 = new person();
            //person2.name = "asus";

            //person person3 = person;
            // bool b= person3.Equals(person);
            //};
            #endregion

            #region Func
            //Func<string, string> 是传入一个string的参数(s) 就是参数 返回值是=>后面的
            Expression<Func<string, string>> func_expression = (s) => s + "委托";
            Console.WriteLine(func_expression.Compile().Invoke("zhu"));

            Func<string, string, string> func3 = (string a, string b) => a + b;
            string ret3 = func3.Invoke("Hell0", "world");

            func3.BeginInvoke("你好", "999", callBack, null);

            #endregion

            #region DateTimeOffset
            //string expiry = "1635471114";
            //DateTimeOffset ss = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(expiry));
            //DateTimeOffset time = DateTimeOffset.Now;
            //if (ss < time)
            //{

            //}
            #endregion
            #region IDog
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
            Console.ReadKey();
        }

        public static void callBack(IAsyncResult result)
        {
            Console.WriteLine(result.AsyncState.ToString());

            Console.WriteLine(result.IsCompleted);
        }

        //抽象就好比是总公司定义几个规则,而且也设定了几个方法(不用abstract修饰)，子公司要继承abstract修饰的方法
        //对于总公司设定的方法 可用可不用，但是要继承

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

            public virtual void get() { }
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

        public class person
        {
            public string name;
        }
    }
}
