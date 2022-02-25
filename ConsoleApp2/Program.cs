using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            //string a = "abcd";
            //string b = a,c=a,d=a;
            //Console.WriteLine(a + "\n");//abcd
            //Console.WriteLine(b+"\n");//abcd
            //Console.WriteLine(c + "\n");//abcd
            //Console.WriteLine(d + "\n");//abcd

            //a = "1234";            
            //Console.WriteLine(a + "\n");//1234
            //Console.WriteLine(b + "\n");//abcd
            //Console.WriteLine(c + "\n");//abcd
            //Console.WriteLine(d + "\n");//abcd

            //bcd都是a进行复制的,abcd 都是指向同一块常量地址。
            //但是常量是不能被直接改变的，因此我们不能直接修改字符串的常量来达到修改字符串的目的
            //必须是另外开辟一个新的空间来存放新的字符串常量a="1234"
            //因此当使用a="1234"的时候,a指向的地址改变了，z指向了新的空间地址
            //但是bcd 指向的还是原来的地址,原来的地址存的还是abcd

            //引用类型：本质上是指向通一块地址,底层实现是通过指针.（这个指针可能也被称为"句柄"）


            //int x = 3;
            //int y = x, z = x;

            //Console.WriteLine(x + "\n");//3
            //Console.WriteLine(y + "\n");//3
            //Console.WriteLine(z + "\n");//3


            //x = 100;
            //Console.WriteLine(x + "\n");//100
            //Console.WriteLine(y + "\n");//3
            //Console.WriteLine(z + "\n");//3

            //WaitCallBack是个有参数没有返回值的委托,所以只需要填入一个有参数无返回值的方法就好了
            //ThreadPool.QueueUserWorkItem(get, "666");
            //ThreadPool.QueueUserWorkItem(get2);

            //ThreadPool.QueueUserWorkItem(x => Console.WriteLine(x),"54545");


            //string[] ary = new string[5];
            //ThreadPool.QueueUserWorkItem(x=> {
            //    ary[(int)x] = x.ToString();
            //    int s = ary.Length;

            //},2);


            //Console.WriteLine("\n————— {0} —————", Thread.CurrentThread.ManagedThreadId);

            //Program program = new Program();            
            //Thread thread = new Thread(program.ParallelBreak);
            //thread.Start();
            //thread.IsBackground = false;
            //int num= thread.ManagedThreadId;
            //thread.Join();
            //Console.WriteLine("\n—————num {0} —————", thread.ManagedThreadId);

            //Console.WriteLine("Main Ending........");


            //var sleepingThread = new Thread(SleepIndefinitely);
            //sleepingThread.Name = "Sleeping";
            //sleepingThread.Start();
            //var state= sleepingThread.ThreadState;
            //Thread.Sleep(10000);
            //sleepingThread.Interrupt();//他中断的是他自己这个耗时的线程

            //sleepingThread.Abort();


            //Console.WriteLine($"Ending.............{state}........");

            //Dog dog = new Dog();
            //dog.Delegatea += Wake;
            //dog.Delegatea += Run;

            //dog.Wangwang();

            ////var hamc = new HMACSHA256(Encoding.UTF8.GetBytes("123"));
            ////Console.WriteLine(hamc);
            ////var sign = Convert.ToBase64String(hamc.ComputeHash(Encoding.UTF8.GetBytes("zhuzhuzhuzhu")));
            ////Console.WriteLine(sign);

            ////Console.WriteLine(HttpUtility.UrlEncode(sign));
            //Console.WriteLine(HttpUtility.UrlEncode("https://fanyi.baidu.com/"));
            //Console.WriteLine(HttpUtility.UrlDecode(HttpUtility.UrlEncode("https://fanyi.baidu.com/")));
            //Console.WriteLine(HttpUtility.UrlEncode("zhu"));
            //Console.WriteLine(HttpUtility.UrlEncode("zhu/"));

            //int[] nums = new int[] { 1, 9, 3, 5, 6, 9, 4, 5 };
            //int temp = 0;
            //for (int i = 0; i < nums.Length - 1; i++)
            //{
            //    for (int j = 0; j < nums.Length - 1 - i; j++)
            //    {
            //        if (nums[j] > nums[j + 1])
            //        {
            //            temp = nums[j + 1];
            //            nums[j + 1] = nums[j];
            //            nums[j] = temp;
            //        }
            //    }
            //}

            //string type = "abc";
            //var typename= type.GetType();
            //var code= type.GetTypeCode();
            //var code2 = "123".GetTypeCode();

            //var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            //int workThread;
            //int IOThread;

            //{
            //    ThreadPool.GetMinThreads(out workThread, out IOThread);
            //    Console.WriteLine(workThread + "\n");
            //    Console.WriteLine(IOThread);
            //}


            //Task task = Task.Run(() => Console.WriteLine("Hello"))
            //            .ContinueWith(t => Console.WriteLine("World"));

            //// System.Threading.Tasks.ContinuationTaskFromTask
            ////Console.WriteLine(task.GetType());

            //Task.Run(() => Console.WriteLine(1))
            //    .ContinueWith(t => Console.WriteLine(2))
            //    .ContinueWith(t => Console.WriteLine(3));

            //Func<int> action;
            //action = returnRet;

            //Task task2 = Task.Run(() => action)
            //             .ContinueWith(t => Console.WriteLine($"{t.Result()} World"));


           

            Strdelegate strdelegate=new   Strdelegate(Wake);
            strdelegate += Run;
            strdelegate += Wake;
            strdelegate += Run;
            strdelegate += Run;
            strdelegate();

            
            Console.ReadKey();
        }


        public static int returnRet()
        {
            return 6;
        }
        public static int F2(int number)
        {
            int a = 1, b = 1;
            if (number == 1 || number == 2)
            {
                return 1;
            }
            else
            {
                for (int i = 3; i < number + 1; i++)
                {
                    int c = a + b;
                    b = a;
                    a = c;
                }
                return a;
            }
        }
        private static void GreetPeople(string name, Strdelegate MakeGreeting)
        {
            MakeGreeting();
        }
        public static void Wake()
        {
            Console.WriteLine("人醒了");
        }
        public static void Run()
        {
            Console.WriteLine("猫跑了");
        }

        public delegate void DelegateA();
        public class Dog
        {
            /// <summary>
            /// 定义用来作为 "事件封装类型" 的委托，用event关键字来声明事件
            /// </summary>
            public event DelegateA Delegatea;
            public void Wangwang()
            {
                Console.WriteLine("小偷来了");
                Delegatea();
                Console.WriteLine("小偷跑了");
            }
        }


        public delegate void Strdelegate();
        delegate int NumberChanger(int n);
        static int num = 10;
        public static int AddNum(int p)
        {
            num += p;
            return num;
        }

        public static int MultNum(int q)
        {
            num *= q;
            return num;
        }
        public static int getNum()
        {
            return num;
        }

        public void ParallelBreak()
        {
            object obj = new object();

            Console.WriteLine("\n————— {0} —————", Thread.CurrentThread.ManagedThreadId);

            ConcurrentBag<int> bag = new ConcurrentBag<int>();

            Parallel.For(0, 100, (i) =>
            {
                //此处加 lock (bag) 则输出一定是 300. 否则不一定是 300 ， 可能是 302, 300, 306 等
                if (bag.Count >= 50)
                {
                    //state.Stop();
                    return;
                }
                bag.Add(i);
            });

            Console.WriteLine("Bag xxxxxx is \n");

            Console.WriteLine("Bag count is " + bag.Count + ", ");

        }


        private static void SleepIndefinitely()
        {
            try
            {
                Thread.Sleep(500);
                Console.WriteLine("Thread '{0}' BOB CurrentThread.Name.", Thread.CurrentThread.Name);
            }
            catch (ThreadInterruptedException)
            {
                Console.WriteLine("Thread '{0}' InterruptedException.",
                                  Thread.CurrentThread.Name);
            }
            catch (ThreadAbortException)
            {
                Console.WriteLine("Thread '{0}' AbortException.",
                                  Thread.CurrentThread.Name);
            }

        }





        static void get(object obj)
        {
            Console.WriteLine(obj);
        }

        static void get2(object? o)
        {
            Console.WriteLine("我是嫩爹");
        }
    }
}
