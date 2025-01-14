using Microsoft.AspNetCore.Mvc;
using Polly;
using PollyDemo.Services;
using System.Net.Http;

namespace PollyDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TimeoutServiceController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<TimeoutServiceController> _logger;
        private readonly ITimeoutService _timeoutService;

        public TimeoutServiceController(ILogger<TimeoutServiceController> logger, ITimeoutService timeoutService)
        {
            _logger = logger;
            _timeoutService = timeoutService;
        }

        [HttpGet(Name = "TimeoutServiceController")]
        public async Task<IActionResult> Get()
        {
            string url = "https://chatgpt.com/c/6785b075-6190-8013-817a-1d11bde26c2d"; // ������ⲿ API URL

            try
            {

                var weatherData = await _timeoutService.GetWeatherDataWithTimeoutAsync(url);
                return Ok(weatherData);
            }
            catch (Exception ex)
            {
                // �������ʧ�ܣ����ش�����Ϣ
                return StatusCode(500, $"����ʧ��: {ex.Message}");
            }

        }
    }
}
