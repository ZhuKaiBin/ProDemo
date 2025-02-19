namespace DomainEvent2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 创建事件总线
            var eventBus = new EventBus();

            // 创建事件处理器
            var deliveryService = new DeliveryService();
            var warehouseService = new WarehouseService();

            // 订阅领域事件
            eventBus.Subscribe<OrderCreatedEvent>(deliveryService.OnOrderCreated);
            eventBus.Subscribe<OrderCreatedEvent>(warehouseService.OnOrderCreated);

            // 创建订单服务
            var orderService = new OrderService(eventBus);

            // 创建一个订单并触发事件
            orderService.CreateOrder(1, 199.99m);
        }
    }

    // 领域事件接口
    public interface IDomainEvent { }

    // 订单创建事件
    public class OrderCreatedEvent : IDomainEvent
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }

        public OrderCreatedEvent(int orderId, DateTime orderDate, decimal totalAmount)
        {
            OrderId = orderId;
            OrderDate = orderDate;
            TotalAmount = totalAmount;
        }
    }

    //快递处理器
    public class DeliveryService
    {
        //要接收订单创建的事件通知
        public void OnOrderCreated(OrderCreatedEvent orderEvent)
        {
            Console.WriteLine($"[快递通知] 订单 {orderEvent.OrderId} 创建成功，快递准备发货，金额：{orderEvent.TotalAmount}元");
        }
    }

    //库房处理器
    public class WarehouseService
    {
        //要接收订单创建的事件通知
        public void OnOrderCreated(OrderCreatedEvent orderEvent)
        {
            Console.WriteLine($"[库房通知] 订单 {orderEvent.OrderId} 创建成功，库房准备发货，金额：{orderEvent.TotalAmount}元");
        }
    }

    // 模拟订单创建并触发事件
    public class OrderService
    {
        private readonly EventBus _eventBus;

        public OrderService(EventBus eventBus)
        {
            _eventBus = eventBus;
        }

        // 创建订单并发布事件
        public void CreateOrder(int orderId, decimal totalAmount)
        {
            var orderEvent = new OrderCreatedEvent(orderId, DateTime.Now, totalAmount);

            // 发布事件
            _eventBus.Publish(orderEvent);
        }
    }

    //创建事件总线(把事件都绑在这一个总线上)
    public class EventBus
    {
        //收集所有订阅事件的处理器(收集所有 继承IDomainEvent 的委托方法)
        private readonly List<Action<IDomainEvent>> _subscribers = new List<Action<IDomainEvent>>();




        // 订阅事件(被动的事件，绑定上来) //Action<TEvent> eventHandler   这个的含义是参数是有一个入参的方法，Get(TEvent te)
        public void Subscribe<TEvent>(Action<TEvent> eventHandler) where TEvent : IDomainEvent
        {
            _subscribers.Add(e =>
            {
                if (e is TEvent tEvent)
                {
                    eventHandler(tEvent);//Get(TEvent te),
                }
            });
        }



        // 发布事件
        public void Publish<TEvent>(TEvent domainEvent) where TEvent : IDomainEvent
        {
            foreach (var subscriber in _subscribers)
            {
                subscriber(domainEvent);//执行所有订阅该事件的处理器
            }
        }
    }




}
