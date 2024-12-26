namespace 多线程按照顺序执行SemaphoreSlim
{
    internal class Program
    {
        static SemaphoreSlim sem1 = new SemaphoreSlim(0, 1);  // 控制 thread2
        static SemaphoreSlim sem2 = new SemaphoreSlim(0, 1);  // 控制 thread3

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
            Thread.Sleep(1000);  // 模拟一些工作
            Console.WriteLine("Thread 1 has finished.");
            sem1.Release();  // 唤醒线程2
        }

        static void Thread2()
        {
            sem1.Wait();  // 这里是在等待sem1.Release() 释放
            Console.WriteLine("Thread 2 is running.");
            Thread.Sleep(1000);  // 模拟一些工作
            Console.WriteLine("Thread 2 has finished.");
            sem2.Release();  // 唤醒线程3
        }

        static void Thread3()
        {
            sem2.Wait();  // 这里是在等待sem2.Release() 释放
            Console.WriteLine("Thread 3 is running.");
            Thread.Sleep(1000);  // 模拟一些工作
            Console.WriteLine("Thread 3 has finished.");
        }
    }
}
