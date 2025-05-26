using System.Runtime.CompilerServices;

namespace MethodImplOptionsSynchronizedSln
{
    internal class Program
    {
        static int counter = 0;
        static void Main(string[] args)
        {
            // 创建两个线程来演示 Mutex 的用法
            Thread t1 = new Thread(Worker);
            Thread t2 = new Thread(Worker);

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();

            Console.WriteLine("Final counter value: " + counter);
            Console.ReadKey();
        }


        //对于静态方法，加的锁是 typeof(Program) 类型对象的锁，不是某个实例。
        //多个线程调用 Worker()，它们会争用 Program 这个类型的锁。
        [MethodImpl(MethodImplOptions.Synchronized)]
        static void Worker()
        {
            for (int i = 0; i < 3000000; i++)
            {
                counter++;   
            }
        }
    }


    
}
