using Microsoft.AspNetCore.Mvc;
using Polly;
using System.Net.Http;

namespace PollyDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly HttpClient _httpClient;
        private readonly IAsyncPolicy _retryPolicy;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, HttpClient httpClient)
        {
            _logger = logger;

            _httpClient = httpClient;
            // 设置重试策略：最多重试 3 次，每次间隔 2 秒
            _retryPolicy = Policy
                .Handle<HttpRequestException>() // 处理网络相关的异常
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(2),
                    onRetry: (exception, timeSpan, retryCount, context) =>
                    {
                        Console.WriteLine($"重试 {retryCount} 次，错误信息: {exception.Message}，等待 {timeSpan.TotalSeconds} 秒");
                    });
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IActionResult> Get()
        {

            try
            {
                // 使用 Polly 重试策略来执行 HTTP 请求
                var response = await _retryPolicy.ExecuteAsync(async () =>
                {
                    // 这里模拟请求外部API
                    HttpResponseMessage result = await _httpClient.GetAsync("https://www.baidu.com111/index.htm");
                    result.EnsureSuccessStatusCode(); // 如果响应不成功，会抛出异常
                    return result;
                });

                // 处理成功响应
                string content = await response.Content.ReadAsStringAsync();
                return Ok(content);
            }
            catch (Exception ex)
            {
                // 如果所有重试都失败，捕获异常并返回错误信息
                return StatusCode(500, $"请求失败: {ex.Message}");
            }

        }






    }
}
