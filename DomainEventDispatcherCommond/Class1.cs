namespace DomainEventDispatcherCommond
{
    public interface IDomainEventDispatcher
    {
        Task DispatchAndClearEvents(IEnumerable<object> domainEvents);
    }

    // 领域事件分发器实现
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        // 事件订阅者列表
        private static List<Action<object>> _subscribers = new List<Action<object>>();

        // 订阅事件的方法
        public static void Subscribe(Action<object> subscriber)
        {
            _subscribers.Add(subscriber);
        }

        // 分发事件的方法
        public async Task DispatchAndClearEvents(IEnumerable<object> domainEvents)
        {
            foreach (var domainEvent in domainEvents)
            {
                foreach (var subscriber in _subscribers)
                {
                    // 异步调用订阅者方法
                    await Task.Run(() => subscriber(domainEvent));
                }
            }
        }

        // 分发单个事件的方法
        public static void Dispatch(object domainEvent)
        {
            foreach (var subscriber in _subscribers)
            {
                subscriber(domainEvent);
            }
        }
    }

    // 定义一个领域事件类
    public class OrderCreatedEvent
    {
        public int OrderId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
