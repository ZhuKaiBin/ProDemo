namespace ChainOfResponsibilityModel_Console
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Handler handler1 = new ConcreteHandler1();
            Handler handler2 = new ConcreteHandler2();
            Handler handler3 = new ConcreteHandler3();

            handler1.SetNext(handler2);
            handler2.SetNext(handler3);

            handler1.HandleRequest(12);

        }
    }


    public abstract class Handler
    {

        #region
        //可以把责任链模式比作一个 接力赛跑，赛道上有多个运动员，每个运动员负责接力比赛的一部分。
        //每个运动员都可以选择自己接过棒子继续跑，或者将棒子交给下一个运动员。

        //每个运动员就像一个处理者，它可以处理某部分任务（比如跑步的一段路），但它也可以决定是否将任务交给下一个运动员。
        //successor 就是下一个运动员的传递点，它记录了这个接力赛的下一个参与者。
        //Method 就是交接棒的过程。每个运动员通过调用这个方法，将责任传递给下一个运动员。
        #endregion

        protected Handler successor;
        public void SetNext(Handler successor)
        {
            //这里就是转移责任的地方
            this.successor = successor;
        }

        public abstract void HandleRequest(int request);
    }

    public class ConcreteHandler1 : Handler
    {
        public override void HandleRequest(int request)
        {
            if (request >= 0 && request < 10)
            {
                Console.WriteLine($"{GetType().Name} handled request {request}");
            }
            else if (successor != null)
            {
                successor.HandleRequest(request);
            }
        }
    }

    public class ConcreteHandler2 : Handler
    {
        public override void HandleRequest(int request)
        {
            if (request >= 10 && request < 20)
            {
                Console.WriteLine($"{GetType().Name} handled request {request}");
            }
            else if (successor != null)
            {
                successor.HandleRequest(request);
            }
        }
    }

    public class ConcreteHandler3 : Handler
    {
        public override void HandleRequest(int request)
        {
            if (request >= 20 && request < 30)
            {
                Console.WriteLine($"{GetType().Name} handled request {request}");
            }
            else if (successor != null)
            {
                successor.HandleRequest(request);
            }
        }
    }
}
