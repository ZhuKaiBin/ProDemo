using Polly;

namespace PollyDemo.Services
{

    public interface ICircuitBreakerService
    {
        Task<HttpResponseMessage> GetWithCircuitBreakerAsync(string url);
    }

    public class CircuitBreakerService : ICircuitBreakerService
    {
        private readonly HttpClient _httpClient;
        private readonly IAsyncPolicy<HttpResponseMessage> _circuitBreakerPolicy;

        public CircuitBreakerService(HttpClient httpClient)
        {
            _httpClient = httpClient;

            // 设置断路器策略：请求失败 3 次后，断路器会打开，保持 10 秒钟的冷却期
            _circuitBreakerPolicy = Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .CircuitBreakerAsync(3, TimeSpan.FromSeconds(10), // 最大失败次数 3，冷却时间 10 秒
                    onBreak: (result, timespan) =>
                    {
                        // 断路器打开时打印日志
                        Console.WriteLine($"断路器触发! 错误状态: {result.Result.StatusCode}, 等待 {timespan.TotalSeconds} 秒");
                    },
                    onReset: () =>
                    {
                        // 断路器恢复时打印日志
                        Console.WriteLine("断路器恢复!");
                    });
        }

        public async Task<HttpResponseMessage> GetWithCircuitBreakerAsync(string url)
        {
            // 使用 Polly 断路器策略来执行 HTTP 请求
            return await _circuitBreakerPolicy.ExecuteAsync(async () =>
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode(); // 如果请求失败，抛出异常
                return response;
            });
        }
    }
}
