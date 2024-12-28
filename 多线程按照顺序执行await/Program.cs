namespace 多线程按照顺序执行await
{
    internal class Program
    {
        async static Task Main(string[] args)
        {
            await Task1();
            await Task2();
            await Task3();

            Console.WriteLine("All tasks have finished.");
        }

        static async Task Task1()
        {
            Console.WriteLine("Task 1 is running.");
            await Task.Delay(1000);  // 模拟一些异步操作
            Console.WriteLine("Task 1 has finished.");
        }

        static async Task Task2()
        {
            Console.WriteLine("Task 2 is running.");
            await Task.Delay(1000);  // 模拟一些异步操作
            Console.WriteLine("Task 2 has finished.");
        }

        static async Task Task3()
        {
            Console.WriteLine("Task 3 is running.");
            await Task.Delay(1000);  // 模拟一些异步操作
            Console.WriteLine("Task 3 has finished.");
        }
    }
}
