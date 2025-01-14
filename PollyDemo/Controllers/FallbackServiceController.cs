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
            string url = "https://api.weather.com/v3/weather/forecast"; // ������ⲿ API URL

            try
            {
                // ʹ�û��˷���ִ���������
                var weatherData = await _fallbackService.GetWeatherDataWithFallbackAsync(url);
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
