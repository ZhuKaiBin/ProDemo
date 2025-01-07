using EventSourceDemo.EventStores;
using EventSourceDemo.Aggregates;
using EventSourceDemo.Services.Interfaces;

namespace EventSourceDemo.Services
{
    public class OrderService : IOrderService
    {
        private readonly IEventStore _eventStore;

        public OrderService(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public void CreateOrder(string orderId, string customerName)
        {
            var order = new OrderAggregate(orderId, customerName);
            order.PlaceOrder();
            foreach (var domainEvent in order.DomainEvents)
            {
                _eventStore.SaveEvent(domainEvent);  // 将事件保存到 EventStore
            }
        }

        public void OrderShipOrder(string orderId, DateTime shipDate)
        {
            var order = GetOrderAggregateById(orderId);
            order.ShipOrder();
            foreach (var domainEvent in order.DomainEvents)
            {
                _eventStore.SaveEvent(domainEvent);  // 将事件保存到 EventStore
            }
        }

        //public void CompleteOrder(string orderId, DateTime completionDate)
        //{
        //    var order = GetOrderAggregateById(orderId);
        //    order.CompleteOrder(completionDate);
        //    foreach (var domainEvent in order.DomainEvents)
        //    {
        //        _eventStore.SaveEvent(domainEvent);  // 将事件保存到 EventStore
        //    }
        //}

        private OrderAggregate GetOrderAggregateById(string orderId)
        {
            var events = _eventStore.GetEventsByAggregateId(orderId);
            var order = new OrderAggregate(orderId, string.Empty);  // 创建一个空的聚合根
            foreach (var domainEvent in events)
            {
                order.ApplyEvent(domainEvent);  // 根据事件更新订单状态
            }
            return order;
        }
    }
}
