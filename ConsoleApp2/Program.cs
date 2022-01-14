using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

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

            Console.WriteLine("\n————— {0} —————", Thread.CurrentThread.ManagedThreadId);

            Program program = new Program();            
            Thread thread = new Thread(program.ParallelBreak);
            thread.Start();
            thread.IsBackground = false;
            int num= thread.ManagedThreadId;          
            Console.WriteLine("\n—————num {0} —————", num);
            Console.ReadKey();
        }



        public void ParallelBreak()
        {

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

            string num = "";
            for (int i = 0; i < bag.ToArray().Length; i++)
            {
                num = num + bag.ToArray()[i].ToString()+",";
            }

            Console.WriteLine("Bag xxxxxx is " + num + "\n");

            Console.WriteLine("Bag count is " + bag.Count + ", ");

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
