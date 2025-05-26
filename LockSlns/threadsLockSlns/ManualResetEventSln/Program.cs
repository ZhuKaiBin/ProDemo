namespace ManualResetEventSln
{
    /// <summary>
    /// 手动重置
    /// 所有等待的线程都会被唤醒
    /// </summary>
    internal class Program
    {
        static ManualResetEvent manualEvent = new ManualResetEvent(false);

        static void Main()
        {
            //// 启动多个线程
            //for (int i = 0; i < 3; i++)
            //{
            //    int threadId = i;
            //    new Thread(() =>
            //    {
            //        Console.WriteLine($"Thread {threadId} waiting for signal...");
            //        manualEvent.WaitOne(); // 等待信号
            //        Console.WriteLine($"Thread {threadId} received the signal.");
            //    }).Start();
            //}

            //// 给所有线程发信号
            //Console.WriteLine("Main thread sending signal...");
            //manualEvent.Set(); // 唤醒所有线程
                      
        }
    }
}
