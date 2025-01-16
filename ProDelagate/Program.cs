using System;

namespace ProDelagate
{
    class Program
    {
        static void Main(string[] args)
        {

            Test test = new Test();
            test.delegatePrint += p1;
            test.delegatePrint += p2;

            test.Run();

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



        //委托的定义，相同签名方法收集器
        public delegate void delegatePrint();

        class Test
        {
            //这个事件 delegatePrint 一定是在Test内部的
            //这样才能保证外部调用Test来注册事件

            //委托就是一个个的方法，所以委托以 "委托()"的形式，是没错的
            public event delegatePrint delegatePrint;

            public void Run()
            {
                delegatePrint();//这样是调用委托的方法，后面加括号,加括号就是代表这是一个方法
                                //调用Run，就都会把绑定在delegatePrint上的方法都调用一遍

                //委托就是一个集合，然后调用这个集合，就会调用集合里面的所有方法
            }
        }

        /*
         
         */
    }
}
