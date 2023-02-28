using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace AOPNetCore
{
    //https://www.bilibili.com/video/BV1W14y1c7yt?p=37&spm_id_from=pageDriver&vd_source=b75e521f89833d861c15a89a5ef8cca9
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ApplicationBuilder();
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
