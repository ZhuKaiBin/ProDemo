using System;
using System.Threading;

namespace AutoResetEventDemo
{
    internal class Program
    {
        private static AutoResetEvent event1 = new AutoResetEvent(false);
        private static AutoResetEvent event2 = new AutoResetEvent(false);
        private static AutoResetEvent event3 = new AutoResetEvent(false);

        static void Main()
        {
            Thread thread1 = new Thread(Thread1);
            Thread thread2 = new Thread(Thread2);
            Thread thread3 = new Thread(Thread3);

            thread1.Start();
            thread2.Start();
            thread3.Start();

            // Start the communication
            //event1.Set();  // Start thread1

            event2.Set();

            thread1.Join();
            thread2.Join();
            thread3.Join();
            //这个调用意味着：
            // 主线程会 等待 thread1 执行完成（直到 thread1 完成后，主线程才会继续执行）。
            // 然后，主线程会 等待 thread2 执行完成（直到 thread2 完成后，主线程才会继续执行）。
            // 最后，主线程会 等待 thread3 执行完成（直到 thread3 完成后，主线程才会继续执行）。

            //Join() 只会阻塞调用它的线程，让它等待目标线程完成。并不会改变目标线程的执行顺序。
            Console.WriteLine("Threads finished execution.");
        }


        //前提：Set 就是一个开工的信息，就是调用的Set，就可以通知所有的event1下的WaitOne后面的程序开始工作了
        //WaitOne 就是一个等待开工的人，它在等待event1调用Set的信号发布
        static void Thread1()
        {
            event1.WaitOne();  // 这个是在等待 event1的Set信号
            Console.WriteLine("Thread 1 is running.");
            event2.Set();  // 告诉2开工了
        }

        static void Thread2()
        {
            event2.WaitOne();  // 等待event2发出set信号
            Console.WriteLine("Thread 2 is running.");
            event3.Set();  // Signal thread3 to run


            //Thread.Sleep(5000);
        }

        static void Thread3()
        {
            event3.WaitOne();  // 等待等待event3发出set信号
            Console.WriteLine("Thread 3 is running.");

            //event1.Set();
        }
    }
}
