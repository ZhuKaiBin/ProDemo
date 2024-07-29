using System;
using System.Threading;

namespace ProSpinLock
{
    class Program
    {
        static void Main(string[] args)
        {
            SpinLockDemo demo = new SpinLockDemo();

            //线程1和线程2 对共享的对象 demo进行操作；
            //线程1是+1  线程2是-1
            Thread t1 = new Thread(() =>
            {
                for (int i = 0; i < 50; i++)
                {
                    demo.Increment();
                }
            });

            Thread t2 = new Thread(() =>
            {
                for (int i = 0; i < 50; i++)
                {
                    demo.Decrement();
                }
            });

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();

            Console.WriteLine("Count: " + demo.GetCount());
            Console.ReadKey();
        }
    }

    class SpinLockDemo
    {
        private int count = 0;
        private SpinLock spinLock = new SpinLock();

        public void Increment()
        {
            bool lockTaken = false;
            try
            {
                spinLock.Enter(ref lockTaken); //lockTaken 默认是false 没有获取到锁,要是获取到锁,那么就是true；
                Console.WriteLine(
                    $"Increment方法，当前线程Id是{Thread.CurrentThread.ManagedThreadId}，count={count}"
                );
                count++;
            }
            finally
            {
                if (lockTaken)
                    spinLock.Exit(); //上文如果获取到了锁，这里就可以释放锁
            }
        }

        public void Decrement()
        {
            bool lockTaken = false;
            try
            {
                spinLock.Enter(ref lockTaken);
                Console.WriteLine(
                    $"Decrement方法，当前线程Id是{Thread.CurrentThread.ManagedThreadId}，count={count}"
                );
                count--;
            }
            finally
            {
                if (lockTaken)
                    spinLock.Exit();
            }
        }

        public int GetCount()
        {
            return count;
        }
    }
}
