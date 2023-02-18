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
            {
                //string a = "abcd";
                //string b = a, c = a, d = a;
                //Console.WriteLine(a + "\n");//abcd
                //Console.WriteLine(b + "\n");//abcd
                //Console.WriteLine(c + "\n");//abcd
                //Console.WriteLine(d + "\n");//abcd

                //a = "1234";
                //Console.WriteLine(a + "\n");//1234
                //Console.WriteLine(b + "\n");//abcd
                //Console.WriteLine(c + "\n");//abcd
                //Console.WriteLine(d + "\n");//abcd

                //bcd都是a进行复制的,abcd 都是指向同一块常量地址。
                //但是常量是不能被直接改变的，因此我们不能直接修改字符串的常量来达到修改字符串的目的
                //必须是另外开辟一个新的空间来存放新的字符串常量a = "1234"
                //因此当使用a = "1234"的时候,a指向的地址改变了，z指向了新的空间地址
                //  但是bcd 指向的还是原来的地址,原来的地址存的还是abcd

                //  引用类型：本质上是指向通一块地址,底层实现是通过指针.（这个指针可能也被称为"句柄"）


                //int x = 3;
                //int y = x, z = x;

                //Console.WriteLine(x + "\n");//3
                //Console.WriteLine(y + "\n");//3
                //Console.WriteLine(z + "\n");//3
                //x = 100;
                //Console.WriteLine(x + "\n");//100
                //Console.WriteLine(y + "\n");//3
                //Console.WriteLine(z + "\n");//3
            }
            {
                //WaitCallBack是个有参数没有返回值的委托,所以只需要填入一个有参数无返回值的方法就好了
                //ThreadPool.QueueUserWorkItem(get, "1");
                //ThreadPool.QueueUserWorkItem(x => Console.WriteLine(x), "54545");


                //这个是master
                //这个是prodemo3

                //var sleepingThread = new Thread(SleepIndefinitely);
                //sleepingThread.Name = "Sleeping";
                //sleepingThread.Start();
                //var state = sleepingThread.ThreadState;
                //sleepingThread.Join(20000);
                //sleepingThread.Interrupt();//他中断的是他自己这个耗时的线程
            }

            MD5 md5 = MD5.Create();   // 默认实现类：Create("System.Security.Cryptography.MD5");         
            byte[] hashByte = md5.ComputeHash(Encoding.UTF8.GetBytes("boB"));

            Encoding.UTF8.GetString(hashByte);

           
            Console.WriteLine(ByteArrayToString(hashByte));
            Console.WriteLine(Encoding.UTF8.GetString(hashByte));

            Console.WriteLine("1.0：MD5默认实现类对明文字符串进行哈希计算后的结果：");
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

            {
                //ThreadPool.GetMinThreads(out int workThread, out int IOThread);
                //Console.WriteLine(workThread + "\n");
                //Console.WriteLine(IOThread);
            }

            {
                //Task task = Task.Run(() => Console.WriteLine("Hello"))
                //            .ContinueWith(t => Console.WriteLine("World"))
                //            .ContinueWith(x => Console.WriteLine(000000))

                //            ;

                ////// System.Threading.Tasks.ContinuationTaskFromTask
                //Console.WriteLine(task.GetType());

                //Task.Run(() => Console.WriteLine(1))
                //    .ContinueWith(t => Console.WriteLine(2))
                //    .ContinueWith(t => Console.WriteLine(3));

                //Func<int> action;
                //action = returnRet;

                //Task task2 = Task.Run(() => action)
                //             .ContinueWith(t => Console.WriteLine($"{t.Result()} World"));

            }

            {
                //ConcurrentBag();
                //Console.WriteLine("11111111111111111111111111111");
                //ListcurrentBag();
            }

            Console.ReadKey();
        }

        /// <summary>
        /// 字节数组转化成16进制字符串
        /// </summary>
        /// <param name="arrInput"></param>
        /// <returns></returns>
        static string ByteArrayToString(byte[] arrInput)
        {
            int i;
            StringBuilder sOutput = new StringBuilder(arrInput.Length);
            for (i = 0; i < arrInput.Length - 1; i++)
            {
                sOutput.Append(arrInput[i].ToString("X2"));
            }
            return sOutput.ToString();
        }

        public static void ConcurrentBag()
        {
            ConcurrentBag<int> conList = new ConcurrentBag<int>();
            Parallel.For(0, 10000, x =>
              {
                  conList.Add(x);
                  Console.WriteLine(x);
              });

            Console.WriteLine($"当前ConcurrentBag的count是{conList.Count}");
        }

        public static void ListcurrentBag()
        {
            List<int> conList = new List<int>();
            Parallel.For(0, 10000, x =>
            {
                conList.Add(x);
                Console.WriteLine(x);
            });

            Console.WriteLine($"当前conList的count是{conList.Count}");
        }
       
    }
}
