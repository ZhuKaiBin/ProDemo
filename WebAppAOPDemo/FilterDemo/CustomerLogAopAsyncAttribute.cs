using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAppAOPDemo.FilterDemo
{
    public class CustomerLogAopAsyncAttribute : Attribute, IAsyncActionFilter
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next">异步回调委托</param>
        /// <returns></returns>
        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next
        )
        {
            Console.WriteLine(context.HttpContext.Request.Path);
            Console.WriteLine(context.HttpContext.Request.Method);
            Console.WriteLine("异步执行前");
            await next();
            Console.WriteLine("异步执行后");
        }
    }
}
