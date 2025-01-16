namespace DomainEventsDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 先登记，登记需要的参数是：Action<IDomainEvent> 的委托
            //DomainEvents.RegisterHandler(HandleDomainEvent);

            DomainEvents.RegisterHandler(handler);//把所有后备军团都接入指挥部，等待候命

            A a = new A();
            a.Start();

            B b = new B();
            b.Start();
        }


        private static void handler(IDomainEvent domainEvent)
        {
            switch (domainEvent)
            {
                case AEvent ae:
                    Console.WriteLine("A后方部队出发");
                    break;
                case BEvent be:
                    Console.WriteLine("后方部队出发");
                    break;
                    // 处理其他领域事件
            }
        }


    }

    // 领域事件接口
    //领域事件 就像一个【通知单】，它告诉系统“发生了某个重要事件”，比如：“电影票已购买”。
    //这个通知单可以被其他系统或模块接收到，然后执行相应的动作，比如发送邮件、更新统计数据等。
    public interface IDomainEvent { }


    public class AEvent : IDomainEvent
    {
        public AEvent()
        {
        }
    }

    public class BEvent : IDomainEvent
    {
        public BEvent()
        {
        }
    }




    public class B
    {
        public void Start()
        {
            var bEvent = new BEvent();
            Console.WriteLine("B前方告急，请求支援......");
            DomainEvents.Notify(bEvent);
        }
    }


    public class A
    {
        public void Start()
        {
            var aEvent = new AEvent();
            Console.WriteLine("A前方告急，请求支援......");
            //向调度中心发信息
            DomainEvents.Notify(aEvent);
        }
    }


    //调度中心
    public static class DomainEvents
    {
        private static readonly List<IDomainEvent> _events = new List<IDomainEvent>();

        //声明一个调度员，它就是分配器，不同的事件然后分配给不同的处理程序调度员的作用就是【通知】
        private static Action<IDomainEvent> _dispatcher;

        public static void RegisterHandler(Action<IDomainEvent> dispatcher)
        {
            _dispatcher = dispatcher;
        }

        //供发起方调用，将派生的事件注入进来，只接收派生自IDomainEvent
        public static void Notify(IDomainEvent domainEvent)
        {
            _events.Add(domainEvent);

            //这段代码的作用是触发已注册的领域事件处理程序。
            //_dispatcher 是一个委托，它指向一个方法，该方法用于处理领域事件。
            Console.WriteLine($"收到前方{domainEvent.GetType()}部队的调令,开始通知后勤部队");

            //这里的代码domainEvent是个基类，
            //就比如说，A继承了domainEvent，如果如果前方是A发送过来的，直接就将信息转到A对应的方法去了
            //                           如果是B发过来的，就将信息转到B对应的方法去

            //这个_dispatcher有所有的实现IDomainEvent的方法，domainEvent和哪个匹配，他就处理哪个
            _dispatcher?.Invoke(domainEvent);
        }
    }


}
