using System.Collections.Concurrent;

namespace BlockingCollectionSlns
{
    internal class Program
    {
        static BlockingCollection<string> queue = new BlockingCollection<string>();
        static void Main(string[] args)
        {
            Thread producerThread = new Thread(Producer);
            Thread consumerThread = new Thread(Consumer);
            producerThread.Start();
            consumerThread.Start();
            producerThread.Join();
            consumerThread.Join();
        }




        /// <summary>
        /// 生产者线程
        /// </summary>
        static void Producer()
        {
            for (int i = 0; i < 10; i++)
            {
                string message = $"Message {i}";
                queue.Add(message);
                Console.WriteLine($"Produced: {message}");
                Thread.Sleep(500); // 模拟生产过程
            }
            //queue.CompleteAdding();
        }


        /// <summary>
        /// 消费者线程
        /// </summary>
        static void Consumer()
        {
            foreach (var message in queue.GetConsumingEnumerable())
            {
                Console.WriteLine($"Consumed: {message}");
                Thread.Sleep(1000); // 模拟消费过程
            }
        }
    }
}
