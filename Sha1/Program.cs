using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sha1
{
    class Program
    {
        private static Object obj = new object();
        static int location = 0;

        static void Main(string[] args)
        {
            //int location1 = 2;
            //int values =666666;
            //int comparand = 1;

            //Interlocked.CompareExchange(ref location1, values, comparand);

            //Console.WriteLine(location1);
            //Console.WriteLine(values);
            //Console.WriteLine(comparand);

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("前" + location + "\n");
                Interlocked.Increment(ref location);
                Console.WriteLine("后" + location + "\n");
                Interlocked.Decrement(ref location);
                Console.WriteLine("Decrement" + location + "\n");
            }
            Console.WriteLine(location);

            //Thread thread1 = new Thread(new ThreadStart(ThreadMethod));
            //Thread thread2 = new Thread(new ThreadStart(ThreadMethod));
            //thread1.Start();
            //thread2.Start();
            //thread1.Join();
            //thread2.Join();

            //Console.WriteLine("UnsafeInstanceCount: {0}"  +   CountClass.SafeInstanceCount.ToString());

            Console.ReadKey();
        }

        static void ThreadMethod()
        {
            CountClass cClass;

            // Create 100,000 instances of CountClass.
            for (int i = 0; i < 10; i++)
            {
                cClass = new CountClass();
            }
        }

        class CountClass
        {
            static int safeInstanceCount = 0; //使用原子操作

            public static int SafeInstanceCount
            {
                get { return safeInstanceCount; }
            }

            public CountClass()
            {
                Console.WriteLine("前" + safeInstanceCount + "\n");
                Interlocked.Increment(ref safeInstanceCount);
                Console.WriteLine("后" + safeInstanceCount + "\n");
            }
        }
    }
}
