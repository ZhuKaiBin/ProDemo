using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace AOPNetCore
{   
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ApplicationBuilder();

            //Use这个方法的参数就是传入一个委托Func<RequestDelegate, RequestDelegate>  
            builder.Use(next =>
            {
                return async context =>
                {
                    Console.WriteLine("清理杂质1");
                    await next(context);
                    Console.WriteLine("清理杂质2");
                };
            });
            builder.Use(next =>
            {
                return async context =>
                {
                    Console.WriteLine("水已经净化了");
                };
            });

            var app = builder.Build();
             app(new HttpContext());
        }
    }
    public class HttpContext
    {
        public string Request { get; }
        public string Response { get; }
    }


    //这个委托是传入一个参数，返回一个Task
    public delegate Task RequestDelegate(HttpContext context);

    //底层的中间件是怎么运行的，
    public class ApplicationBuilder
    {
        //这个是委托的集合
        private readonly List<Func<RequestDelegate, RequestDelegate>> _components = new List<Func<RequestDelegate, RequestDelegate>>();

        public void Use(Func<RequestDelegate, RequestDelegate> middleware)
        {
            _components.Add(middleware);
        }

        public RequestDelegate Build()
        {
            RequestDelegate app = (context) =>
            {
                throw new InvalidOperationException("不合理的管道");
            };

            for (int i = _components.Count - 1; i > -1; i--)
            {
                app = _components[i](app);
            }
            return app;
        }
    }

}
