namespace SemaphoreSln
{
    internal class Program
    {
        // 初始时最多 2 辆车可以进入，但最多能停 3 辆车。
        static Semaphore semaphore = new Semaphore(2, 3);

        static void Main(string[] args)
        {
            // 启动多个线程模拟并发请求
            for (int i = 0; i < 10; i++)
            {
                int taskId = i;  // 为每个任务分配一个唯一ID
                ThreadPool.QueueUserWorkItem(Worker, taskId);
            }

            // 等待用户按键退出
            Console.ReadKey();
        }

        static void Worker(object taskId)
        {
            int id = (int)taskId;

            Console.WriteLine($"任务 {id} 尝试获取信号量...");

            // 获取信号量，若信号量计数器为0，则线程会被阻塞直到信号量可用
            semaphore.WaitOne();

            Console.WriteLine($"任务 {id} 获取到信号量，开始执行...");

            // 模拟任务执行
            Thread.Sleep(2000);

            Console.WriteLine($"任务 {id} 执行完成，释放信号量...");

            // 释放信号量，允许其他线程获取
            semaphore.Release();
        }
    }
}
