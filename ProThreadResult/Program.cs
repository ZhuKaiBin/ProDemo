using System;
using System.Threading;

namespace ProThreadResult
{
  

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!\n");

            //ThreadWithState tws = new ThreadWithState(new DeleCallBack(ResultCallback));


            //Thread t = new Thread(new ThreadStart(tws.ThreadProc));
            //t.Start();         
            //t.Join();

            Console.WriteLine("Ends..................................");

            //这个是ParameterizedThreadStart的方式
            //第一步：是给Thread赋值方法
            Thread newThread = new Thread(DoWork);
            //第二步：Start里 赋值
            newThread.Start(42);



            //第一步：实例化类,然后赋值
            ThreadWithState ThreadWithState = new ThreadWithState();
            ThreadWithState.Data = 12;
            //第二步：把方法给到这个是ThreadStart
            ThreadStart threadDelegate = new ThreadStart(ThreadWithState.DoMoreWork);
            //第三步：把ThreadStart 复制给Thread线程
            Thread newThread2 = new Thread(threadDelegate);
            newThread2.Start();


            Console.ReadKey();

        }


        public static void DoWork(object data)
        {
            Console.WriteLine("Static thread procedure. Data='{0}'",data);
        }

        public class ThreadWithState
        {

            public static void DoWork()
            {
                Console.WriteLine("Static thread procedure.");
            }
            public int Data;
            public  void DoMoreWork()
            {
                Console.WriteLine("Instance thread procedure. Data={0}", Data);
            }
        }
    }
}
