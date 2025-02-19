using EventSourceDemo.EventStores;

namespace EventSourceDemo.Aggregates
{
    public class OrderAggregate
    {
        public string OrderId { get; private set; }
        public string OrderStatus { get; private set; }

        public string CustomerName { get; private set; }

        public DateTime dateTime { get; private set; }

        public List<IDomainEvent> DomainEvents { get; private set; }

        // 其他属性，例如订单的总金额、订单项等

        // 构造函数
        public OrderAggregate(string orderId, string initialStatus)
        {
            OrderId = orderId;
            OrderStatus = initialStatus;
            DomainEvents = new List<IDomainEvent>();
            dateTime = DateTime.UtcNow;

        }

        // 事件应用方法
        public void ApplyEvent(IDomainEvent domainEvent)
        {
            switch (domainEvent)
            {
                case OrderPlaced orderPlaced:
                    Apply(orderPlaced);
                    break;
                case OrderShipped orderShipped:
                    Apply(orderShipped);
                    break;
                case OrderCompleted orderCompleted:
                    Apply(orderCompleted);
                    break;
                    // 其他事件...
            }
        }

        //这里是领域行为
        public void PlaceOrder()
        {
            // 创建领域事件
            var orderPlacedEvent = new OrderPlaced(OrderId, CustomerName, dateTime);

            // 发布领域事件
            DomainEvents.Add(orderPlacedEvent);
        }


        public void ShipOrder()
        {
            // 创建领域事件
            var orderPlacedEvent = new OrderShipped(OrderId, dateTime);

            // 发布领域事件
            DomainEvents.Add(orderPlacedEvent);
        }



        // 应用事件的具体实现
        private void Apply(OrderPlaced orderPlaced)
        {
            OrderStatus = "Placed";
            // 其他状态变更
        }

        private void Apply(OrderShipped orderShipped)
        {
            OrderStatus = "Shipped";
            // 其他状态变更
        }

        private void Apply(OrderCompleted orderCompleted)
        {
            OrderStatus = "Completed";
            // 其他状态变更
        }

        // 其他方法，例如触发事件、验证订单状态等
    }

    public class OrderPlaced : IDomainEvent
    {
        public string OrderId { get; }
        public string CustomerName { get; }
        public DateTime OrderDate { get; }

        public DateTime Timestamp => OrderDate;

        // 构造函数用于初始化事件数据
        public OrderPlaced(string orderId, string customerName, DateTime orderDate)
        {
            OrderId = orderId;
            CustomerName = customerName;
            OrderDate = orderDate;
        }
    }

    public class OrderShipped : IDomainEvent
    {
        public string OrderId { get; }
        public DateTime ShipmentDate { get; }
        public DateTime Timestamp => ShipmentDate;

        // 构造函数用于初始化事件数据
        public OrderShipped(string orderId, DateTime shipmentDate)
        {
            OrderId = orderId;
            ShipmentDate = shipmentDate;
        }
    }


    public class OrderCompleted : IDomainEvent
    {
        public string OrderId { get; }
        public DateTime CompletionDate { get; }

        public DateTime Timestamp => CompletionDate;

        // 构造函数用于初始化事件数据
        public OrderCompleted(string orderId, DateTime completionDate)
        {
            OrderId = orderId;
            CompletionDate = completionDate;
        }
    }



}
