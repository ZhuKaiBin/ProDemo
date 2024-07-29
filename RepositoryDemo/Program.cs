using DomainEventDispatcherCommond;

namespace RepositoryDemo
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // 创建订单
            var order = new Order(1, "John Doe");
            order.Create();

            var order2 = new Order(2, "John Doe");
            order2.Create();

            // 模拟其他操作...

            //// 模拟领域事件分发器处理事件
            //await SimulateDispatchEvents();

            Console.ReadLine();
        }

        // 模拟领域事件分发器处理事件的异步方法
        static async Task SimulateDispatchEvents()
        {
            var eventsToDispatch = new List<object>
            {
                new OrderCreatedEvent { OrderId = 2, Timestamp = DateTime.UtcNow }
            };

            var dispatcher = new DomainEventDispatcher();
            await dispatcher.DispatchAndClearEvents(eventsToDispatch);
        }
    }

    // 领域模型中的订单实体
    public class Order
    {
        public int OrderId { get; private set; }
        public string CustomerName { get; private set; }

        // 构造函数
        public Order(int orderId, string customerName)
        {
            OrderId = orderId;
            CustomerName = customerName;
        }

        // 创建订单方法，会引发订单创建事件
        public void Create()
        {
            // 其他创建订单的逻辑

            // 创建订单事件
            var orderCreatedEvent = new OrderCreatedEvent
            {
                OrderId = this.OrderId,
                Timestamp = DateTime.UtcNow
            };

            // 将事件传递给领域事件分发器
            DomainEventDispatcher.Dispatch(orderCreatedEvent);
        }
    }

    // 领域事件分发器接口
}
