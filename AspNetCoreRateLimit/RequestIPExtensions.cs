using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreRateLimit
{
    public static class RequestIPExtensions
    {
        /// <summary>
        /// 扩展方法，对IApplicationBuilder进行扩展
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseRequestIP(this IApplicationBuilder builder)
        {
            // UseMiddleware<T>
            return builder.UseMiddleware<RequestIPMiddleware>();
        }


        public static IApplicationBuilder UseRequestPara(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestMiddleware>();
        }
    }
}
