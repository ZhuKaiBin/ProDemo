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
            // �������Բ��ԣ�������� 3 �Σ�ÿ�μ�� 2 ��
            _retryPolicy = Policy
                .Handle<HttpRequestException>() // ����������ص��쳣
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(2),
                    onRetry: (exception, timeSpan, retryCount, context) =>
                    {
                        Console.WriteLine($"���� {retryCount} �Σ�������Ϣ: {exception.Message}���ȴ� {timeSpan.TotalSeconds} ��");
                    });
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IActionResult> Get()
        {

            try
            {
                // ʹ�� Polly ���Բ�����ִ�� HTTP ����
                var response = await _retryPolicy.ExecuteAsync(async () =>
                {
                    // ����ģ�������ⲿAPI
                    HttpResponseMessage result = await _httpClient.GetAsync("https://www.baidu.com111/index.htm");
                    result.EnsureSuccessStatusCode(); // �����Ӧ���ɹ������׳��쳣
                    return result;
                });

                // ����ɹ���Ӧ
                string content = await response.Content.ReadAsStringAsync();
                return Ok(content);
            }
            catch (Exception ex)
            {
                // ����������Զ�ʧ�ܣ������쳣�����ش�����Ϣ
                return StatusCode(500, $"����ʧ��: {ex.Message}");
            }

        }






    }
}
