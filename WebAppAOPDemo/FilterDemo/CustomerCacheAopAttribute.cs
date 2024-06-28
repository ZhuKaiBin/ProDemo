using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAppAOPDemo.FilterDemo
{
    public class CustomerCacheAopAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine(context.HttpContext.Request.Path);
            Console.WriteLine(context.HttpContext.Request.Method);
            Console.WriteLine("缓存执行前");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine(context.HttpContext.Request.Path);
            Console.WriteLine(context.HttpContext.Request.Method);
            Console.WriteLine("缓存执行后");
        }
    }
}