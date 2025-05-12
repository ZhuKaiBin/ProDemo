using System.Threading;

namespace MutexSln
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string mutexName = "Global\\MyUniqueAppMutex";

            bool createdNew;
            var  mutex = new Mutex(true, mutexName, out createdNew);

            if (!createdNew)
            {
                Console.WriteLine("❌ 程序已在运行中，禁止重复启动！");
                //return;   //注释掉可以在控制台打印出
            }


            Thread.Sleep(10000);

            Console.WriteLine("✅ 程序启动成功，按任意键退出。");

            // 模拟主程序逻辑运行
            Console.ReadKey();

            // 程序结束时释放 Mutex  
            mutex.ReleaseMutex();
            mutex.Dispose();
        }
    }
}
