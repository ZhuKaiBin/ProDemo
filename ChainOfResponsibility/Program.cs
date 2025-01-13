namespace ChainOfResponsibility
{
    /// <summary>
    /// 责任链模式
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {

            //// 创建处理者对象
            //var handlerA = new HandlerA();
            //var handlerB = new HandlerB();
            //var handlerC = new HandlerC();

            //// 设置责任链
            //handlerA.SetNextHandler(handlerB);
            //handlerB.SetNextHandler(handlerC);


            //// 创建请求对象
            //var request = new Request(15);

            //// 发起请求，责任链中的处理者将按顺序处理请求
            //handlerA.HandleRequest(request);


            //var dto = new ProductDto("apple", 10.0m);
            //// dto.Name = "apple2";这里会报错，因为record是不可变的，只能通过构造函数赋值

            //Console.WriteLine(dto.Name);





        }
    }


    public class Request
    {
        public int Value { get; set; }
        public Request(int value)
        {
            this.Value = value;
        }
    }


    //抽象类，就定义了一个规则，只要是继承我的，就要符合我的规则
    //


    // 处理请求的抽象类
    public abstract class Handler
    {
        protected Handler _nextHandler;

        //()设置下一个处理者
        public void SetNextHandler(Handler nextHandler)
        {
            _nextHandler = nextHandler;
        }

        // 处理请求的方法，子类需要实现具体的处理逻辑
        public abstract void HandleRequest(Request request);

        public virtual DateTime GetDateTime()
        {
            return DateTime.UtcNow;
        }
    }


    public class HandlerA : Handler
    {
        public override void HandleRequest(Request request)
        {

            Console.WriteLine("A处理开始");

            if (request.Value < 10)
            {
                Console.WriteLine("HandlerA处理了请求，值为 " + request.Value);
            }
            else if (_nextHandler != null)
            {
                // 将请求传递给下一个处理者
                _nextHandler.HandleRequest(request);
            }

            Console.WriteLine("HandlerA_" + _nextHandler.GetDateTime());
        }
    }

    public class HandlerB : Handler
    {

        public override void HandleRequest(Request request)
        {
            Console.WriteLine("HandlerB处理开始");
            if (request.Value <= 20)
            {

                Console.WriteLine("HandlerB处理了请求，值为 " + request.Value);
            }
            else if (_nextHandler != null)
            {
                _nextHandler.HandleRequest(request);
            }
            Console.WriteLine("HandlerB_" + _nextHandler.GetDateTime());
        }
    }
    public class HandlerC : Handler
    {
        public override void HandleRequest(Request request)
        {
            if (request.Value > 20)
            {
                Console.WriteLine("HandlerC处理了请求，值为 " + request.Value);
            }
        }
    }

}
