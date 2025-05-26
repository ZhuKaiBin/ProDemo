namespace AutoResetEventSln
{
    /// <summary>
    /// 自动重置信号
    /// 只会唤醒一个线程，并且在唤醒后会自动重置。
    /// </summary>
    internal class Program
    {
        static AutoResetEvent autoEvent = new AutoResetEvent(false);
        static void Main()
        {
            //// 启动多个线程
            //for (int i = 0; i < 3; i++)
            //{
            //    int threadId = i;
            //    new Thread(() =>
            //    {
            //        Console.WriteLine($"Thread {threadId} waiting for signal...");
            //        autoEvent.WaitOne(); // 等待信号
            //        Console.WriteLine($"Thread {threadId} received the signal.");
            //    }).Start();
            //}

            //// 给第一个线程发信号
            //Console.WriteLine("Main thread sending signal...");


            //Thread.Sleep(2000);
            //autoEvent.Set(); // 唤醒一个线程



            Thread threadB = new Thread(MethodB);
            Thread threadA = new Thread(MethodA);

            Thread threadC = new Thread(MethodC);

            threadB.Start();
            Thread.Sleep(500); // 确保线程 B 先启动并等待信号
            threadA.Start();

            threadC.Start();

        }



        static void MethodA()
        {
            Console.WriteLine("Thread A: 正在执行方法 A...");


            Thread.Sleep(2000); // 模拟耗时操作
            Console.WriteLine("Thread A: 方法 A 执行完毕，发出信号");
            autoEvent.Set(); // 发出信号，通知线程 B
        }

        static void MethodB()
        {
            Console.WriteLine("Thread B: 等待线程 A 的信号...");
            autoEvent.WaitOne(); // 等待信号它不是忙等待（busy wait），而是操作系统级别的 阻塞等待（blocking wait）。
            Console.WriteLine("Thread B: 收到信号，开始执行方法 B...");

            Thread.Sleep(1000); // 模拟耗时操作
            Console.WriteLine("Thread B: 方法 B 执行完毕");

            //autoEvent.Set(); // 发出信号，通知线程 C
        }


        static void MethodC()
        {
            Console.WriteLine("Thread C: 等待线程 B 的信号...");
            autoEvent.WaitOne(); // 等待信号
            Console.WriteLine("Thread C: 收到信号，开始执行方法 C...");

            Thread.Sleep(1000); // 模拟耗时操作
            Console.WriteLine("Thread C: 方法 C 执行完毕");
        }

    }
}
