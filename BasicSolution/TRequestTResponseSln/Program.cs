namespace TRequestTResponseSln
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var handler = new RequestHandler<string, int>("Hello");

            int result = handler.Handle(req =>
            {
                return req.Length;
            });


            M1("9090");

        }




        public static void  M1(in string userName)
        {
             //userName = "new";
            Console.WriteLine(userName);
        
        }



    }

    public class RequestHandler<TRequest, TResponse>
    {
        public TRequest Request { get; set; }

        public RequestHandler(TRequest request)
        {
            Request = request;
        }


        public TResponse Handle(Func<TRequest, TResponse> handlerLogic)
        {
            // 1. 空函数处理
            if (handlerLogic == null)
            {
                throw new ArgumentNullException(nameof(handlerLogic), "处理逻辑不能为 null");
            }

            // 2. 可加上日志或调试输出（Console）
            Console.WriteLine($"开始处理请求：{Request}");

            // 3. 执行处理逻辑
            TResponse response = handlerLogic(Request);

            Console.WriteLine($"处理完成，响应结果：{response}");

            // 4. 返回结果
            return response;
        }

    }
}
