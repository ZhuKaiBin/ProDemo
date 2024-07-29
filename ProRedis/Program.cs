using System;
using System.Text;
using ServiceStack.Redis;

namespace ProRedis
{
    class Program
    {
        static void Main(string[] args)
        {
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

            Console.ReadKey();
        }
    }
}
