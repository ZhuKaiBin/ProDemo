using System;
using System.Threading;

namespace ProSpinLock
{
    class Program
    {
        static void Main(string[] args)
        {
            SpinLockDemo demo = new SpinLockDemo();

            Thread t1 = new Thread(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    demo.Increment();
                }
            });

            Thread t2 = new Thread(() =>
            {
                for (int i = 0; i < 99; i++)
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
                spinLock.Enter(ref lockTaken);
                count++;
            }
            finally
            {
                if (lockTaken) spinLock.Exit();
            }
        }

        public void Decrement()
        {
            bool lockTaken = false;
            try
            {
                spinLock.Enter(ref lockTaken);
                count--;
            }
            finally
            {
                if (lockTaken) spinLock.Exit();
            }
        }

        public int GetCount()
        {
            return count;
        }
    }
}
