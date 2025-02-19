using Polly;

namespace PollyDemo.Services
{

    public interface IRetryService
    {
        Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> action);
    }

    public class RetryService : IRetryService
    {
        private readonly IAsyncPolicy _retryPolicy;
        public RetryService()
        {
            // // 设置重试策略：最多重试 3 次，每次间隔 2 秒
            _retryPolicy = Policy
              .Handle<HttpRequestException>() // 处理网络相关的异常
              .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(2),
                  onRetry: (exception, timeSpan, retryCount, context) =>
                  {
                      Console.WriteLine($"重试 {retryCount} 次，错误信息: {exception.Message}，等待 {timeSpan.TotalSeconds} 秒");
                  });
        }


        public async Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> action)
        {
            // 执行操作并自动进行重试
            return await _retryPolicy.ExecuteAsync(action);
        }
    }
}
