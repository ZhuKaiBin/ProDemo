using System.Threading;

namespace 多线程按照顺序执行ManualResetEvent
{
    internal class Program
    {
        //Thread 1 is running.
        //Thread 2 is running.
        //Thread 3 is running.
        //Threads finished execution.

        //manualEvent的一次set(),会把所有的WaitOne都唤醒起来干活
        private static ManualResetEvent manualEvent = new ManualResetEvent(false);
        static void Main(string[] args)
        {
            Thread thread1 = new Thread(Thread1);
            Thread thread2 = new Thread(Thread2);
            Thread thread3 = new Thread(Thread3);

            thread1.Start();
            thread2.Start();
            thread3.Start();

            // Start thread1
            manualEvent.Set();

            thread1.Join();
            thread2.Join();
            thread3.Join();

            Console.WriteLine("Threads finished execution.");
        }

        static void Thread1()
        {
            manualEvent.WaitOne();  // Wait for signal from Main thread
            Console.WriteLine("Thread 1 is running.");
            //manualEvent.Reset();  // Reset the event so thread2 starts after thread1
            //manualEvent.Set();    // Start thread2
        }

        static void Thread2()
        {
            manualEvent.WaitOne();  // Wait for signal from thread1
            Console.WriteLine("Thread 2 is running.");
            //manualEvent.Reset();    // Reset the event so thread3 starts after thread2
            //manualEvent.Set();      // Start thread3
        }

        static void Thread3()
        {
            manualEvent.WaitOne();  // Wait for signal from thread2
            Console.WriteLine("Thread 3 is running.");
        }

    }
}
