using Polly;

namespace PollyDemo.Services
{

    public interface ITimeoutService
    {
        Task<string> GetWeatherDataWithTimeoutAsync(string url);
    }

    public class TimeoutService : ITimeoutService
    {
        private readonly HttpClient _httpClient;

        public TimeoutService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetWeatherDataWithTimeoutAsync(string url)
        {
            //       // 设置超时策略：如果请求超时超过 3 秒，则自动取消请求
            //       var timeoutPolicy = Policy
            //.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(3),
            //    onTimeoutAsync: async (context, timespan, task) =>
            //    {
            //        // 打印日志，表示请求超时
            //        Console.WriteLine($"请求超时！操作已取消，超时 {timespan.TotalSeconds} 秒");

            //        // 返回已完成的任务
            //        return Task.CompletedTask;
            //    });

            //       // 使用超时策略执行 HTTP 请求
            //       var result = await timeoutPolicy.ExecuteAsync(async () =>
            //       {
            //           // 模拟 HTTP 请求（例如通过 _httpClient 获取数据）
            //           var response = await _httpClient.GetStringAsync(url);
            //           return response;
            //       });

            //       return result;


            return await Task.FromResult("请求超时！操作已取消，超时 3 秒");
        }
    }
}
