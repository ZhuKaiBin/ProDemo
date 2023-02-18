using System;
using System.Text;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            x x = new x();
            x.Ret();
            Console.ReadLine();

        }

        class Base
        {
            public Base()
            {
                Func();
            }

            public virtual void Func()
            {
                Console.Write("Base.Func");
            }
        }

        class Sub : Base
        {
            //Sub是继承了Base,在


            public Sub()
            {
                Func();
            }

            public void SubMethod()
            {
                Console.WriteLine("我是SubMethod");
            }

            /// <summary>
            /// Func 是重写的Base里的
            /// </summary>
            //public override void Func()
            //{
            //    Console.Write("Sub.Func");
            //}
        }

        class x :  IFa, Sub
        {
            public x()
            {
                this.Func();
            }

            public void Ret()
            {
                Console.WriteLine("我是x里的方法");
            }

            public 
        }
        //就是说子类继承父类,父类又继承爷爷类,
        //子类会以此访问父类,爷爷类的构造函数
        //然后再依次的退出来
        //这就是继承
    }
}
