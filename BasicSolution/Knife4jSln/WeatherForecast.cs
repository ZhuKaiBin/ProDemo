namespace Knife4jSln;

public class WeatherForecast
{
    /// <summary>
    /// ֻ��ʾ����
    /// </summary>
    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    /// <summary>
    /// �ܽ�
    /// </summary>
    public string? Summary { get; set; }
}
