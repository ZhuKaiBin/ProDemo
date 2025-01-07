namespace 领域事件Demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 创建事件总线
            var eventBus = new SimpleEventBus();

            // 创建并订阅事件处理程序
            var orderPlacedHandler = new OrderPlacedHandler();
            eventBus.Subscribe<OrderPlaced>(orderPlacedHandler.Handle);

            // 创建订单聚合
            var order = new OrderAggregate(Guid.NewGuid(), "此订单来此京东");
            order.PlaceOrder();

            var order2 = new OrderAggregate(Guid.NewGuid(), "此订单来此天猫");
            order2.PlaceOrder();

            // 发布事件
            foreach (var domainEvent in order.DomainEvents)
            {
                eventBus.Publish(domainEvent);
            }
        }
    }
}
