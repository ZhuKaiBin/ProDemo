using Microsoft.AspNetCore.Mvc;
using Polly;
using PollyDemo.Services;
using System.Net.Http;

namespace PollyDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FallbackServiceController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<FallbackServiceController> _logger;
        private readonly IFallbackService _fallbackService;

        public FallbackServiceController(ILogger<FallbackServiceController> logger, IFallbackService fallbackService)
        {
            _logger = logger;
            _fallbackService = fallbackService;
        }

        [HttpGet(Name = "FallbackServiceController")]
        public async Task<IActionResult> Get()
        {
            string url = "https://api.weather.com/v3/weather/forecast"; // 假设的外部 API URL

            try
            {
                // 使用回退服务，执行请求操作
                var weatherData = await _fallbackService.GetWeatherDataWithFallbackAsync(url);
                return Ok(weatherData);
            }
            catch (Exception ex)
            {
                // 如果回退失败，返回错误信息
                return StatusCode(500, $"请求失败: {ex.Message}");
            }

        }
    }
}
