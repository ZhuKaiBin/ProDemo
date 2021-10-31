using System;
using System.Text;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("顺序表测试开始...");

            Sub sub = new Sub();
            sub.Func();
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
            //因为是继承了Base
            // 进到sub的时候,不是先Sub的构造,而是从Sub的构造，去Base的构造
            //Base里虚方法 不可以直接被调用,而是要找到重载(Override)它的"实体方法"


            public Sub()
            {
                Func();
            }

            /// <summary>
            /// Func 是重写的Base里的
            /// </summary>
            public override void Func()
            {
                Console.Write("Sub.Func");
            }
        }
    }
}
