namespace SpinLockSln
{
    class Program
    {
        static SpinLock spinLock = new SpinLock();
        static int counter = 0;
        static List<int> ints = new List<int>();  
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
        }

        static void Worker()
        {
            for (int i = 0; i < 100; i++)
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
    }
}
