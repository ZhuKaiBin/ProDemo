using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 领域事件Demo
{
    // 定义一个自己的领域事件的接口
    public interface IDomainEvent
    {
        DateTime OccurredOn { get; }
    }


    // 具体的领域事件，表示订单已创建
    public class OrderPlaced : IDomainEvent
    {
        public Guid OrderId { get; }
        public string CustomerName { get; }
        public DateTime OrderDate { get; }

        public DateTime OccurredOn { get; private set; }

        //限制了OccurredOn只能通过构造函数赋值

        public OrderPlaced(Guid orderId, string customerName, DateTime orderDate)
        {
            OrderId = orderId;
            CustomerName = customerName;
            OrderDate = orderDate;
            OccurredOn = DateTime.UtcNow; // 事件发生的时间
        }
    }


    public class OrderAggregate
    {
        public Guid Id { get; private set; }
        public string CustomerName { get; private set; }
        public DateTime OrderDate { get; private set; }

        // 这里可以通过某种方式发布领域事件，例如事件总线
        public List<IDomainEvent> DomainEvents { get; private set; }

        public OrderAggregate(Guid id, string customerName)
        {
            Id = id;
            CustomerName = customerName;
            OrderDate = DateTime.UtcNow;
            DomainEvents = new List<IDomainEvent>();
        }

        //这里是领域行为
        public void PlaceOrder()
        {
            // 创建领域事件
            var orderPlacedEvent = new OrderPlaced(Id, CustomerName, OrderDate);

            // 发布领域事件
            DomainEvents.Add(orderPlacedEvent);
        }
    }



    public class OrderPlacedHandler
    {
        // 处理 OrderPlaced 事件
        public void Handle(OrderPlaced orderPlacedEvent)
        {
            Console.WriteLine($"Order placed: {orderPlacedEvent.OrderId} for customer {orderPlacedEvent.CustomerName}.");
            // 在这里执行相关的操作，如更新库存、通知其他系统等
        }
    }


    public class SimpleEventBus
    {
        private readonly List<Action<IDomainEvent>> _subscribers = new List<Action<IDomainEvent>>();

        // 订阅事件
        public void Subscribe<TEvent>(Action<TEvent> eventHandler) where TEvent : IDomainEvent
        {
            _subscribers.Add(evt => eventHandler((TEvent)evt));
        }

        // 发布事件
        public void Publish<TEvent>(TEvent eventToPublish) where TEvent : IDomainEvent
        {
            foreach (var subscriber in _subscribers.OfType<Action<TEvent>>())
            {
                subscriber(eventToPublish);  // 执行事件处理程序
            }
        }
    }

}
