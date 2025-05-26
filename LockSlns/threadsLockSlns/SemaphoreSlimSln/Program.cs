namespace SemaphoreSlimSln
{
    internal class Program
    {
        static async Task Main(string[] args) 
        {
            await SemaphoreTest();  // 等待任务全部完成          
        }

        public static async Task SemaphoreTest()
        {
            var semaphore = new SemaphoreSlim(5);
            var tasks = new List<Task>();

            for (int i = 1; i <= 10; i++)
            {
                await Task.Delay(100); // 排队上桥
                var index = i;
                var task = Task.Run(async () =>
                {
                    await semaphore.WaitAsync();
                    try
                    {
                        Console.WriteLine($"第{index}个人正在过桥。");
                        await Task.Delay(5000); // 模拟过桥时间
                    }
                    finally
                    {
                        Console.WriteLine($"第{index}个人已经过桥。");
                        semaphore.Release();
                    }
                });

                tasks.Add(task);
            }

            await Task.WhenAll(tasks); // 等待所有人过桥完毕
        }
    }


}
