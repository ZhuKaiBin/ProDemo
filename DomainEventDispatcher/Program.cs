using DomainEventDispatcherCommond;

namespace DomainEventDispatcher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 订阅订单创建事件的处理方法
            DomainEventDispatcherCommond.DomainEventDispatcher.Subscribe(
                (domainEvent) =>
                {
                    if (domainEvent is OrderCreatedEvent orderCreatedEvent)
                    {
                        Console.WriteLine($"订单 {orderCreatedEvent.OrderId} 已创建。");
                        // 可以在此处执行其他处理逻辑
                    }
                }
            );
        }
    }
}
