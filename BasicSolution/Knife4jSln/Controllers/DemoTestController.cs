using Microsoft.AspNetCore.Mvc;

namespace Knife4jSln.Controllers;

[ApiController]
[Route("[controller]")]
public class DemoTestController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<DemoTestController> _logger;

    public DemoTestController(ILogger<DemoTestController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// 天气服务Demo
    /// </summary>
    /// <returns></returns>
    [HttpGet("DemoTest")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }



    /// <summary>
    /// 天气服务Demo2
    /// </summary>
    /// <returns></returns>
    [HttpGet("DemoTest2")]
    public IEnumerable<WeatherForecast> Get2()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
