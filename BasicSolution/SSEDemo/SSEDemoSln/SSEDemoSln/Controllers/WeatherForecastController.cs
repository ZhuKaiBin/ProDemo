using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SSEDemoSln.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing",
            "Bracing",
            "Chilly",
            "Cool",
            "Mild",
            "Warm",
            "Balmy",
            "Hot",
            "Sweltering",
            "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
      

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            
        }

        [HttpGet(Name = "GetWeatherForecast")]
        [ResponseCache(Duration = 600, Location = ResponseCacheLocation.Client)]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {


            //Response.Headers.Add("Content-Type", "text/event-stream");
            //Response.Headers.Add("Cache-Control", "no-cache");
            //Response.Headers.Add("Connection", "keep-alive");

            HttpContext.Response.ContentType = "text/event-stream; charset=utf-8";

            var cancelToken = HttpContext.RequestAborted;

            string content = "定义一个字符串变量，模拟发送的内容;设置响应头，指定内容类型为服务器端事件流";

            // 初始化一个计数器变量，用于跟踪字符串中字符的位置
            int i = 0;

            //while (i<content.Length)
            //{
            //    //// 检查请求是否已被取消，如果已取消则退出循环
            //    //if (HttpContext.RequestAborted.IsCancellationRequested)
            //    //{
            //    //    break;

            //    //}


            //    // 计算每次发送的字符数，最多3个字符
            //    int length = Math.Min(3, content.Length - i);

            //    //1.WriteAsync 的作用
            //    //    WriteAsync 会把数据写入响应流中。它会将数据存储在内存中，但并不会立即发送给客户端，直到响应完成或者显式地刷新流。
            //    await HttpContext.Response.WriteAsync($"data: {content.Substring(i, length)}\n\n");

            //    Task.Delay(10000).Wait();
            //    // 刷新响应流，确保数据立即发送

            //    //2.FlushAsync 的作用
            //    //    FlushAsync 的作用是 强制将内存中的数据立即发送到客户端，而不是等待下一次 HTTP 响应周期结束时再发送。
            //    //    这是因为 HTTP 协议中的响应是基于流的，通常数据会被暂存在内存中，直到响应完成或者通过 Flush 操作刷新输出流。
            //    //await HttpContext.Response.Body.FlushAsync();

            //    //WriteAsync 将数据写入响应流。
            //    //FlushAsync 确保数据从内存中即时发送到客户端，而不是等到连接关闭或响应结束时再发送。

            //    // 更新计数器，准备发送下一批字符
            //    i += 3;
            //    // 延迟100毫秒，模拟每秒发送一次消息
            //    await Task.Delay(100);
            //}

           
            if (HttpContext.RequestAborted.IsCancellationRequested)
            {
                // 如果请求被取消，直接返回，不再处理响应
               return new List<WeatherForecast>();
            }



            // 如果请求没有被取消，继续处理响应
            await HttpContext.Response.WriteAsync($"data: {content}\n\n");

            Task.Delay(5000).Wait();



            //HttpContext.Response.Body.Close(); // 中止响应并关闭连接



            return Enumerable
                .Range(1, 5)
                .Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                })
                .ToArray();
        }

        [HttpPost]
        public async Task<IActionResult> Registry(string userName, string email, string pwd)
        {
            //账号:zkb   密码：zxc123456@A
            var newUser = new IdentityUser()
            {
                UserName = userName,
                Email = email,
            };

           

            return Ok();
        }


        public class WeatherForecast
        {
            public DateTime Date { get; set; }

            public int TemperatureC { get; set; }

            public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

            public string? Summary { get; set; }
        }
    }
}
