using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CoreRateLimit
{
    /// <summary>
    /// 记录IP地址的中间件
    /// </summary>
    public class RequestIPMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestIPMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await context.Response.WriteAsync(
                $"User IP:{context.Connection.RemoteIpAddress.ToString()}\r\n"
            );

            // 调用管道中的下一个委托
            await _next.Invoke(context);
        }
    }

    public class RequestMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await context.Response.WriteAsync($"User Para:{context.Request.Method.ToString()}\r\n");

            // 调用管道中的下一个委托
            //await _next.Invoke(context);
        }
    }
}
