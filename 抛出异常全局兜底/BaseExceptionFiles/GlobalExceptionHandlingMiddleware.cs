using System.Text.Json.Serialization;

namespace 抛出异常全局兜底.BaseExceptionFiles
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // 继续执行请求管道中的下一个中间件
                await _next(context);
            }
            catch (BaseCustomException ex)
            {
                // 处理自定义的不同类型的异常
                _logger.LogError(ex, "Handled custom exception");

                // 返回不同的错误码和信息
                context.Response.StatusCode = ex.ErrorCode switch
                {
                    1001 => StatusCodes.Status400BadRequest,  // 用户异常
                    2001 => StatusCodes.Status500InternalServerError,  // 逻辑异常
                    3001 => StatusCodes.Status400BadRequest,  // 参数异常
                    _ => StatusCodes.Status500InternalServerError  // 其他异常
                };

                // 返回 JSON 格式的错误信息
                var response = new
                {
                    errorCode = ex.ErrorCode,
                    errorMessage = ex.Message
                };


                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                // 处理其他未预料到的异常
                _logger.LogError(ex, "Unhandled exception");

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                var response = new
                {
                    errorCode = 5000,  // 系统错误的错误码
                    errorMessage = "An unexpected error occurred."
                };

                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
    }
}
