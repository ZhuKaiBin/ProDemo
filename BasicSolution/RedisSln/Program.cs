using StackExchange.Redis;

namespace RedisSln
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Redis 连接字符串，指定了密码和主机
            string redisConnectionString = "47.116.12.81:6379,abortConnect=false,password=123456";

            // 创建连接
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(redisConnectionString);

            // 获取 Redis 数据库
            IDatabase db = redis.GetDatabase();

            // 设置一个键值对
            db.StringSet("mykey2", "Hello Redis2");

            //// 获取键值对
            //string value = db.StringGet("mykey");

            //// 输出结果
            //Console.WriteLine("mykey: " + value);
        }
    }
}
