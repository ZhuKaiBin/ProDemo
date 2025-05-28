using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Infrastructure.Middlewares
{
    public class RequestTimingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestTimingMiddleware> _logger;

        public RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            var path = context.Request.Path;
            var query = context.Request.QueryString.ToString();

            string requestBody = await ReadRequestBody(context.Request);

            var originalBodyStream = context.Response.Body;
            var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            try
            {
                await _next(context);

                stopwatch.Stop();

                responseBody.Position = 0;
                string responseBodyText = await new StreamReader(responseBody).ReadToEndAsync();

                responseBody.Position = 0;
                await responseBody.CopyToAsync(originalBodyStream);

                _logger.LogInformation("Request {Path} took {ElapsedMilliseconds}ms\nQuery: {Query}\nRequestBody: {RequestBody}\nResponseBody: {ResponseBody}",
                    path, stopwatch.ElapsedMilliseconds, query, requestBody, responseBodyText);
            }
            finally
            {
                context.Response.Body = originalBodyStream;
                responseBody.Dispose();
            }
        }


        private async Task<string> ReadRequestBody(HttpRequest request)
        {
            request.EnableBuffering();
            using var reader = new StreamReader(request.Body, leaveOpen: true);
            string body = await reader.ReadToEndAsync();
            request.Body.Position = 0; // 重置流位置，方便后续中间件使用
            return body;
        }

        private async Task<string> ReadResponseBody(MemoryStream responseBody)
        {
            responseBody.Position = 0;
            using var reader = new StreamReader(responseBody);
            string text = await reader.ReadToEndAsync();
            responseBody.Position = 0;
            return text;
        }
    }

}
