using CleanArchitecture.Application.Common.Enums;
using CleanArchitecture.Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Middlewares
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = ex switch
                {
                    ValidationAppException => StatusCodes.Status400BadRequest,
                    BusinessException => StatusCodes.Status422UnprocessableEntity,
                    DatabaseException => StatusCodes.Status500InternalServerError,
                    _ => StatusCodes.Status500InternalServerError
                };

                _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
                var errorCode = (ex as BaseAppException)?.Code ?? ErrorCode.Unknown;

                var errorResponse = new
                {
                    type = ex.GetType().Name,
                    code = (int)errorCode,
                    message = ex.Message,
                    detail = ex.InnerException?.Message,
                    traceId = context.TraceIdentifier
                    //traceId 是 请求追踪标识符，它是 ASP.NET Core（特别是通过 HttpContext.TraceIdentifier）自动生成的一个唯一字符串。每一次 HTTP 请求都会生成一个新的 traceId，保证唯一性。
                };

                var json = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = false
                });

                await context.Response.WriteAsync(json);
            }
        }
    }

}
