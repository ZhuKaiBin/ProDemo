using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SeqLogDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecast2Controller : ControllerBase
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

        private readonly ILogger<WeatherForecast2Controller> _logger;

        public WeatherForecast2Controller(ILogger<WeatherForecast2Controller> logger)
        {
            _logger = logger;
        }

        //https://localhost:7290/weatherforecast2
        [HttpGet(Name = "GetWeatherForecast")]
        public string Get()
        {
            _logger.LogError("LogError：我是error");
            _logger.LogInformation("LogInformation：" + Guid.NewGuid().ToString());
            _logger.LogTrace("LogTrace");

            var eventId = new EventId(1001, "CriticalError");
            _logger.LogCritical(eventId, "CriticalError：eventId");
            return "我是资源2返回的数据";
        }
    }
}