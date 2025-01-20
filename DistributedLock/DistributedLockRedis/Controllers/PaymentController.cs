using DistributedLockRedis.Services;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System;
using System.Net.Http;
using System.Threading.Tasks;
namespace DistributedLockRedis.Controllers
{
    [ApiController]
    [Route("[controller]/[Action]")]
    public class PaymentController : ControllerBase
    {

        private readonly IDatabase _redisDatabase;

        public PaymentController(IConnectionMultiplexer redis)
        {
            _redisDatabase = redis.GetDatabase();
        }


        [HttpPost("pay")]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequest request)
        {
            // 创建分布式锁
            var lockKey = $"PaymentLock:{request.OrderId}"; // 锁的键，基于订单 ID
            var lockValue = Guid.NewGuid().ToString(); // 锁的值，确保唯一性
            var expiry = TimeSpan.FromSeconds(10); // 锁的过期时间;设置合理的过期时间（如 10 秒），防止锁被长时间占用

            var distributedLock = new RedisDistributedLock(_redisDatabase, lockKey, lockValue, expiry);

            var orderIds = new[] { "Order1", "Order2", "Order3", "Order4", "Order5" }; // 模拟多个订单 ID
            var tasks = new List<Task>();


            var orderIds2 = new[] { "Order1", "Order2", "Order3", "Order4", "Order5" }; // 模拟订单ID           


            Parallel.ForEach(orderIds, orderId =>
            {
                Parallel.For(0, 10, async i =>
                {
                    var lockKey = $"PaymentLock:{orderId}";
                    var lockValue = Guid.NewGuid().ToString();
                    var expiry = TimeSpan.FromSeconds(10);

                    var distributedLock = new RedisDistributedLock(_redisDatabase, lockKey, lockValue, expiry);

                    try
                    {
                        if (await distributedLock.AcquireLockAsync())
                        {
                            Console.WriteLine($"[{DateTime.Now}] Order {orderId}: 获取到锁，执行订单逻辑");
                            await Task.Delay(1000); // 模拟处理时间
                        }
                        else
                        {
                            Console.WriteLine($"[{DateTime.Now}] Order {orderId}: 获取锁失败.....");
                        }
                    }
                    finally
                    {
                        await distributedLock.ReleaseLockAsync();
                        Console.WriteLine($"[{DateTime.Now}] Order {orderId}: Released lock.");
                    }
                });
            });


            return Ok(new { Message = "运行结束" });
        }

    }


    public class PaymentRequest
    {
        public int UserId { set; get; }

        public int OrderId { set; get; }

        public decimal Amount { set; get; }

    }
}
