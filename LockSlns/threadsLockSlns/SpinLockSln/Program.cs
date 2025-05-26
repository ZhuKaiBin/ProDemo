using System.Net.Sockets;

namespace SpinLockSln
{
    class Program
    {
        static SpinLock spinLock = new SpinLock();
        static int counter = 0;
        static List<int> ints = new List<int>();


        static volatile bool locked = false;
        static void Main()
        {
            Thread t1 = new Thread(Worker);
            Thread t2 = new Thread(Worker);

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();

            //Console.WriteLine("Final counter value: " + counter);
            Console.WriteLine("ints counter value: " + ints.Count);
            Console.ReadKey();


            //// 线程1试图获取锁并进行自旋占用
            //Thread t1 = new Thread(Worker1);
            //Thread t2 = new Thread(Worker2);

            //t1.Start();
            //t2.Start();

            //t1.Join();
            //t2.Join();

            //Console.WriteLine("Final counter value: " + counter);
            //Console.ReadKey();
        }

        static void Worker()
        {
            for (int i = 0; i < 500; i++)
            {

                //counter++;  // 修改共享资源
                //ints.Add(i);

                bool lockTaken = false;
                try
                {
                    spinLock.Enter(ref lockTaken);  // 获取自旋锁
                    counter++;  // 修改共享资源
                    ints.Add(i);
                }
                finally
                {
                    if (lockTaken)
                    {
                        spinLock.Exit();  // 释放自旋锁
                    }
                }
            }
        }



        static void Worker1()
        {
            for (int i = 0; i < 100000000; i++)
            {
                bool lockTaken = false;
                try
                {
                    // 模拟自旋锁的占用CPU过程
                    while (!lockTaken)
                    {
                        spinLock.Enter(ref lockTaken);  // 自旋尝试获取锁
                        Console.WriteLine($"Worker1 是否获取到锁：{lockTaken}");
                        counter++;  // 执行占用锁期间的操作
                    }
                }
                finally
                {
                    if (lockTaken)
                    {
                        spinLock.Exit();  // 释放自旋锁
                    }
                }
            }
        }

        static void Worker2()
        {
            // 线程2模拟更慢的操作，导致Worker1持续占用CPU
            Thread.Sleep(1000);  // 延迟，让Worker1先占用锁
            for (int i = 0; i < 100000000; i++)
            {
                bool lockTaken = false;
                try
                {
                    spinLock.Enter(ref lockTaken);  // 获取自旋锁
                    Console.WriteLine($"Worker2 是否获取到锁：{lockTaken}");
                    counter--;  // 执行占用锁期间的操作
                }
                finally
                {
                    if (lockTaken)
                    {
                        spinLock.Exit();  // 释放自旋锁
                    }
                }
            }
        }

       
    }
}
