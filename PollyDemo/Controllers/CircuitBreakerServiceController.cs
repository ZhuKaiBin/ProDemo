using Microsoft.AspNetCore.Mvc;
using Polly;
using PollyDemo.Services;
using System.Net.Http;

namespace PollyDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CircuitBreakerServiceController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<CircuitBreakerServiceController> _logger;
        private readonly ICircuitBreakerService _circuitBreakerService;

        public CircuitBreakerServiceController(ILogger<CircuitBreakerServiceController> logger, ICircuitBreakerService circuitBreakerService)
        {
            _logger = logger;
            _circuitBreakerService = circuitBreakerService;
        }

        [HttpGet(Name = "CircuitBreakerServiceController")]
        public async Task<IActionResult> Get()
        {
            string url = "https://api.example.com/weather"; // 假设的外部 API URL


            try
            {
                // 使用断路器服务，执行请求操作
                var response = await _circuitBreakerService.GetWithCircuitBreakerAsync(url);

                // 返回响应
                string content = await response.Content.ReadAsStringAsync();
                return Ok(content);
            }
            catch (Exception ex)
            {
                // 如果断路器打开或请求失败，返回错误信息
                return StatusCode(500, $"请求失败: {ex.Message}");
            }

        }
    }
}
