using Microsoft.AspNetCore.Mvc;

namespace WebAppAOPDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        //https://localhost:7232/WeatherForecast/GetWeatherForecast
        [HttpGet("GetWeatherForecast")]
        public string Index()
        {
            return "OK";
        }
    }
}