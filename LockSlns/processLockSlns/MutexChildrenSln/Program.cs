namespace MutexChildrenSln
{
    class Program
    {
        const string name = "Global\\ProcessSynchronizationExample";
        private static Mutex m;
        static void Main(string[] args)
        {
            Console.WriteLine("子进程被启动...");
            bool firstInstance;

            // 子进程创建互斥体
            m = new Mutex(true, name, out firstInstance);

            // 按照我们设计的程序，创建一定是成功的RequestDelegate
            if (firstInstance)
            {
                Console.WriteLine("子线程执行任务");
                DoWork();
                Console.WriteLine("子线程任务完成");

                // 结束程序
                Console.WriteLine("子线程10秒之后要关闭");
                Thread.Sleep(TimeSpan.FromSeconds(10));
                // 释放互斥体
                m.ReleaseMutex();
              
                return;
            }
            else
            {
                Console.WriteLine("莫名其妙的异常，直接退出");
            }
        }
        private static void DoWork()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("子线程工作中");
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
        }
    }
}
