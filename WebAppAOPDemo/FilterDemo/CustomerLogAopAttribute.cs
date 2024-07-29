using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAppAOPDemo.FilterDemo
{
    public class CustomerLogAopAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine(context.HttpContext.Request.Path);
            Console.WriteLine(context.HttpContext.Request.Method);
            Console.WriteLine("OnActionExecuting执行前");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine(context.HttpContext.Request.Path);
            Console.WriteLine(context.HttpContext.Request.Method);
            Console.WriteLine("OnActionExecuted执行后");
        }
    }
}
