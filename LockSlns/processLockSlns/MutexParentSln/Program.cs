using System.Diagnostics;

namespace MutexParentSln
{
    class Program
    {
        const string name = "Global\\ProcessSynchronizationExample";
        private static Mutex m;
        static void Main(string[] args)
        {           
            Console.WriteLine("父进程启动！");

           new Thread(() =>
             {
                 // 启动子进程
                 Process process = new Process();
                 process.StartInfo.UseShellExecute = true;
                 process.StartInfo.CreateNoWindow = false;
                 process.StartInfo.WorkingDirectory = @"D:\ProjectDemo\ProDemoSlns\LockSlns\processLockSlns\MutexChildrenSln\bin\Debug\net8.0";
                 process.StartInfo.FileName = @"D:\ProjectDemo\ProDemoSlns\LockSlns\processLockSlns\MutexChildrenSln\bin\Debug\net8.0\MutexChildrenSln.exe";
                 process.Start();
                 process.WaitForExit();
             }).Start();



            // 子进程启动需要一点时间
            Thread.Sleep(TimeSpan.FromSeconds(1));


            // 获取互斥体
            bool firstInstance;
            m = new Mutex(true, name, out firstInstance);

            // 说明子进程还在运行
            if (!firstInstance)
            {
                // 等待子进程运行结束
                Console.WriteLine("等待子进程运行结束");
                Console.WriteLine($"父线程是否获取到互斥体：{firstInstance}");
                m.WaitOne();
                Console.WriteLine("子进程运行结束，程序将在10秒后自动退出");
                m.ReleaseMutex();
                Thread.Sleep(TimeSpan.FromSeconds(10));
                return;
            }
        }
    }
}
