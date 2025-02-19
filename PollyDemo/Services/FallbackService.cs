using Polly;

namespace PollyDemo.Services
{

    public interface IFallbackService
    {
        Task<string> GetWeatherDataWithFallbackAsync(string url);
    }

    public class FallbackService : IFallbackService
    {
        private readonly HttpClient _httpClient;

        public FallbackService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetWeatherDataWithFallbackAsync(string url)
        {

            //// 设置泛型回退策略：如果请求失败，返回默认的天气数据
            //var fallbackPolicy = Policy<string> // 使用泛型策略
            //    .Handle<HttpRequestException>() // 处理网络异常
            //    .FallbackAsync(
            //        // 回退的返回值，包装成 Task<string>
            //        async (cancellationToken) => Task.FromResult("当前天气信息不可用，返回默认数据：温度 20°C"),
            //        onFallbackAsync: (exception, context) =>
            //        {
            //            // 打印日志，记录回退操作
            //            Console.WriteLine($"外部 API 请求失败: {exception.Exception.Message}, 启用回退策略");
            //            return Task.CompletedTask;
            //        });

            //// 使用回退策略执行 HTTP 请求
            //var response = await fallbackPolicy.ExecuteAsync(async () =>
            //{
            //    var result = await _httpClient.GetStringAsync(url);
            //    return result;
            //});

            //return response;

            return await Task.FromResult("当前天气信息不可用，返回默认数据：温度 20°C");

        }
    }
}
