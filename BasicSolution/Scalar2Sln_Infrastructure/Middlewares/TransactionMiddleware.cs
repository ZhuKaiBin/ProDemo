using Scalar2Sln_Infrastructure.Data;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Scalar2Sln_Infrastructure.Middlewares
{
    public class TransactionMiddleware
    {
        private readonly RequestDelegate _next;

        public TransactionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // 从作用域中获取 ApplicationDbContext
            var dbContext = context.RequestServices.GetRequiredService<ApplicationDbContext>();

            // 开始事务
            var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                await _next(context); // 调用下一个中间件或控制器

                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

}
