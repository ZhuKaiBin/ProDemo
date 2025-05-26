using System.Threading;

namespace MutexSln
{
    //当两个或两个以上的线程同时访问共享资源时，操作系统需要一个同步机制来确保每次只有一个线程使用资源。
    internal class Program
    {
        static void Main(string[] args)
        {
            //如果其名称以前缀 "Global\" 开头，则 mutex 在所有终端服务器会话中可见

            //如果其名称以前缀 "Local\" 开头，则 mutex 仅在创建它的终端服务器会话中可见。
            //在这种情况下，可以在服务器上的其他每个终端服务器会话中存在具有相同名称的单独 mutex。
            const string mutexName = "Global\\MyUniqueAppMutex";


            //通过 new 来实例化 Mutex 类，会检查系统中此互斥量 name 是否已经被使用，
            //如果没有被使用，则会创建 name 互斥量并且此线程拥有此互斥量的使用权；
            //此时 createdNew == true。
            bool createdNew;
            var mutex = new Mutex(true, mutexName, out createdNew);

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
