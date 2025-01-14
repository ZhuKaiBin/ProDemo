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
            string url = "https://api.example.com/weather"; // ������ⲿ API URL

            try
            {
                // ʹ�����Է���ִ���������
                var response = await _retryService.ExecuteWithRetryAsync(async () =>
                {
                    // ģ���ⲿ API ���󣨿����滻Ϊʵ�ʵ� HTTP ����
                    var httpResponse = await new System.Net.Http.HttpClient().GetAsync(url);
                    httpResponse.EnsureSuccessStatusCode(); // �������ʧ�ܣ��׳��쳣
                    return httpResponse;
                });

                // ������Ӧ
                string content = await response.Content.ReadAsStringAsync();
                return Ok(content);
            }
            catch (Exception ex)
            {
                // �������ʧ�ܣ����ش�����Ϣ
                return StatusCode(500, $"����ʧ��: {ex.Message}");
            }
        }
    }
}
