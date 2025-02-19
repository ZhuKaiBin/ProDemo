namespace EventSourceDemo.EventStores
{
    public interface IDomainEvent
    {
        // 通常，领域事件会有时间戳
        DateTime Timestamp { get; }
    }
}
