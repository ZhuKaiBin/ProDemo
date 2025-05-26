namespace BarrierSln
{
    internal class Program
    {
        static Barrier barrier = new Barrier(participantCount: 3,
                                             b => Console.WriteLine($"== 所有线程已完成阶段 {b.CurrentPhaseNumber} =="));
        private static volatile bool _test;
        static void Main()
        {

            for (int i = 0; i < 3; i++)
            {
                int threadId = i;
                new Thread(() =>
                {
                    for (int phase = 0; phase < 3; phase++)
                    {
                        Console.WriteLine($"线程 {threadId} 正在执行阶段 {phase}");
                        Thread.Sleep(1000 + threadId * 200); // 模拟任务耗时不同
                        barrier.SignalAndWait(); // 同步点
                    }
                }).Start();
            }
        }
    }
}
