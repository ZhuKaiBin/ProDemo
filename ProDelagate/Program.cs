using System;

namespace ProDelagate
{
    class Program
    {
        static void Main(string[] args)
        {
            {
                var publisher = new Publisher();
                var subscriber = new Subscriber();
                publisher.PrintEvent += subscriber.OrderHandle;

                publisher.PrintEvent = null;  // ①这里直接修改了委托链，后续的事件将无法被触发
                // ②或者替换委托链，导致事件处理方法被覆盖
                publisher.PrintEvent = new delegateNoEventPrint((me) => Console.WriteLine("New handler"));

                //①② 这两种情况，就好比是没有走正常的发布订阅，半路杀出来个程咬金，然后破坏我们的委托链
                //可能导致后边触发事件失败，执行失败，如果执行①，那么后面的触发事件是不起作用的

                publisher.Trigger("开始发布事件");

            }

            {
                Test test = new Test();
                test.delegatePrint += p1;
                test.delegatePrint += p2;

                //test.delegatePrint = null;//这里编译是报错的：事件“Program.Test.delegatePrint”只能出现在 += 或 -= 的左边(从类型“Program.Test”中使用时除外)



                test.Run();

                Console.ReadKey();
            }
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

        public delegate void delegateNoEventPrint(string message);
        class Publisher
        {
            public delegateNoEventPrint PrintEvent;

            public void Trigger(string message)
            {
                PrintEvent?.Invoke(message);
            }
        }

        public class Subscriber
        {
            public void OrderHandle(string message)
            {
                Console.WriteLine($"Received message: {message}");
            }


            public void DeliverHandle(string message)
            {
                Console.WriteLine($"Received message: {message}");
            }
        }

        /*
         
         */
    }
}
