namespace 多线程按照顺序执行CountdownEvent
{
    internal class Program
    {
        static CountdownEvent countdown = new CountdownEvent(2);  // 设置计数器初始值为2，表示有两个线程要等待

        static void Main()
        {
            Thread thread1 = new Thread(Thread1);
            Thread thread2 = new Thread(Thread2);
            Thread thread3 = new Thread(Thread3);

            // 启动线程1
            thread1.Start();
            thread1.Join();  // 等待线程1完成

            // 启动线程2
            thread2.Start();
            thread2.Join();  // 等待线程2完成

            // 启动线程3
            thread3.Start();
            thread3.Join();  // 等待线程3完成

            Console.WriteLine("All threads have finished.");
        }

        static void Thread1()
        {
            Console.WriteLine("Thread 1 is running.");
            Thread.Sleep(1000);
            Console.WriteLine("Thread 1 has finished.");
            countdown.Signal();  // 线程1完成，触发计数器
        }

        static void Thread2()
        {
            countdown.Wait();  // 等待线程1完成
            Console.WriteLine("Thread 2 is running.");
            Thread.Sleep(1000);
            Console.WriteLine("Thread 2 has finished.");
            countdown.Signal();  // 线程2完成，触发计数器
        }

        static void Thread3()
        {
            countdown.Wait();  // 等待线程2完成
            Console.WriteLine("Thread 3 is running.");
            Thread.Sleep(1000);
            Console.WriteLine("Thread 3 has finished.");
        }
    }
}
