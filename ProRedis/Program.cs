using ServiceStack.Redis;
using System;
using System.Text;

namespace ProRedis
{
    class Program
    {
        static void Main(string[] args)
        {

            int[] num = new int[] { 6, 5, 3, 9, 7, 0, 2 };

            int temp = 0;

            for (int i = 0; i < num.Length; i++)
            {

                int minval = num[i];
                int minindex = i;

                for (int j = 0; j < num.Length; j++)
                {
                    if(minval>num[j])
                    {
                        minval = num[j];
                        minindex = j;
                    }
                }

                temp = num[i];
                num[i] = num[minindex];
                num[minindex] = temp;
            }

            foreach (var item in num)
            {
                Console.WriteLine("C#遍历：{0}", item);
            }
            Console.ReadKey();
            //using (RedisClient client = new RedisClient("192.168.3.201", 6379))
            //{
            //    //先清空Redis里的东西
            //    //client.FlushAll();

            //    client.Set<string>("namse", "bob");
            //    client.Set<string>("pwd", "123456");

            //    client.Expire("namse", 20);

            //    string name = client.Get<string>("name");
            //    string pwd = client.Get<string>("password");


            //    Console.WriteLine(name);
            //    Console.WriteLine(pwd);
            //}








            //Console.WriteLine(Encoding.UTF8.GetBytes("Ａ").Length);
            //Console.WriteLine(Encoding.UTF8.GetBytes("A").Length);
            //int? num = 3;
            //Console.WriteLine(num.HasValue);//true
            //Console.WriteLine(num.Value);//3

            //int? num2 = null;
            //Console.WriteLine(num2.HasValue);//false
            ////Console.WriteLine(num2.Value);//异常报错
            //Console.WriteLine(num2.GetValueOrDefault());//int的默认值是0
            //Console.WriteLine(num2.GetValueOrDefault(1));//这里num2是null值,给了一个指定的默认值1



            //string Ret = Get((a) => { return a == "bob" ? "A" : "B"; });
            //Console.WriteLine(Ret);

            //string zhu = Get2("我是手动Key的", (Para) => { return Para; });
            //bool b = Get3("我是手动Key的", (Para) => { return Para == "A" ? true : false; });

            //Func<string, int, string> GetSubLocator;
            //GetSubLocator = (string s, int i) =>
            //{
            //    if (string.IsNullOrEmpty(s))
            //        return "";
            //    else
            //    {
            //        var tempary = s.Split('|');
            //        return tempary[i];
            //    }
            //};
            //string end = GetSubLocator("4|6|9", 1);

            Console.ReadKey();
        }

        public static string Get(Func<string, string> func)
        {
            return func.Invoke("bob");
        }


        public static string Get2(string Para, Func<string, string> func)
        {
            //Invoke里的这个参数(xxx),是要传递给Func<string,string>中的第一个string的
            //然后第一个string,是要用到委托"=>"后面的运算中的            
            //还有就是假如这个Get2是有返回值的,那么是要带return的
            return func.Invoke(Para);
        }

        public static bool Get3(string Para, Func<string, bool> func)
        {
            return func.Invoke(Para);
        }
    }
}
