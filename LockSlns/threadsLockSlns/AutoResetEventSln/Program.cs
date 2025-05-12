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
            // 启动多个线程
            for (int i = 0; i < 3; i++)
            {
                int threadId = i;
                new Thread(() =>
                {
                    Console.WriteLine($"Thread {threadId} waiting for signal...");
                    autoEvent.WaitOne(); // 等待信号
                    Console.WriteLine($"Thread {threadId} received the signal.");
                }).Start();
            }

            // 给第一个线程发信号
            Console.WriteLine("Main thread sending signal...");


            Thread.Sleep(2000);

            autoEvent.Set(); // 唤醒一个线程
        }
    }
}
