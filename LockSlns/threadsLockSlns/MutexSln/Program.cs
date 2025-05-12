namespace MutexSln
{
    internal class Program
    {
        static Mutex mutex = new Mutex();
        static int counter = 0;

        static void Main()
        {
            // 创建两个线程来演示 Mutex 的用法
            Thread t1 = new Thread(Worker);
            Thread t2 = new Thread(Worker);

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();

            Console.WriteLine("Final counter value: " + counter);
            Console.ReadKey();
        }

        static void Worker()
        {
            for (int i = 0; i < 1000; i++)
            {
                mutex.WaitOne();  // 获取 Mutex 锁
                counter++;        // 修改共享资源
                mutex.ReleaseMutex();  // 释放 Mutex 锁
            }
        }
    }
}
