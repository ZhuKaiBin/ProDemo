using Microsoft.AspNetCore.Mvc;
using PollyDemo.Services;

namespace PollyDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RetryServiceController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<RetryServiceController> _logger;
        private readonly IRetryService _retryService;

        public RetryServiceController(ILogger<RetryServiceController> logger, IRetryService retryService)
        {
            _logger = logger;
            _retryService = retryService;
        }

        [HttpGet(Name = "RetryServiceDemo")]
        public async Task<IActionResult> Get()
        {
            string url = "https://api.example.com/weather"; // 假设的外部 API URL

            try
            {
                // 使用重试服务，执行请求操作
                var response = await _retryService.ExecuteWithRetryAsync(async () =>
                {
                    // 模拟外部 API 请求（可以替换为实际的 HTTP 请求）
                    var httpResponse = await new System.Net.Http.HttpClient().GetAsync(url);
                    httpResponse.EnsureSuccessStatusCode(); // 如果请求失败，抛出异常
                    return httpResponse;
                });

                // 返回响应
                string content = await response.Content.ReadAsStringAsync();
                return Ok(content);
            }
            catch (Exception ex)
            {
                // 如果重试失败，返回错误信息
                return StatusCode(500, $"请求失败: {ex.Message}");
            }
        }
    }
}
