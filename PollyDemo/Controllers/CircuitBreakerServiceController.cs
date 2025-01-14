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
            string url = "https://api.example.com/weather"; // ������ⲿ API URL


            try
            {
                // ʹ�ö�·������ִ���������
                var response = await _circuitBreakerService.GetWithCircuitBreakerAsync(url);

                // ������Ӧ
                string content = await response.Content.ReadAsStringAsync();
                return Ok(content);
            }
            catch (Exception ex)
            {
                // �����·���򿪻�����ʧ�ܣ����ش�����Ϣ
                return StatusCode(500, $"����ʧ��: {ex.Message}");
            }

        }
    }
}
