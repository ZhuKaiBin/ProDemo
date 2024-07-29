using Castle.DynamicProxy;

namespace WebAppAOPDemo.AufofacIntercepter
{
    /// <summary>
    /// 拦截器演示类
    /// </summary>
    public class AufofacIntercepterDemo : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            //拦截的时候可以看下是否有忽略拦截的特性

            //IsDefined用于检查当前类型是否标记了指定的特性
            //bool attr = invocation.Method.IsDefined(typeof(ingoreInterceptorAttribute), true);
            //if (attr)
            //{
            //    invocation.Proceed();
            //    return;
            //}

            Console.WriteLine(invocation.Method.Name);
            Console.WriteLine(string.Join(',', invocation.Arguments));
            Console.WriteLine("之前");
            //【日志】【缓存】【开始前】
            //程序要执行的方法
            invocation.Proceed();
            //【开始后，计算耗时】
            Console.WriteLine("之后");
        }
    }
}
