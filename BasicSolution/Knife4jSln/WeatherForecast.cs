namespace Knife4jSln;

public class WeatherForecast
{
    /// <summary>
    /// 只显示日期
    /// </summary>
    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    /// <summary>
    /// 总结
    /// </summary>
    public string? Summary { get; set; }
}
