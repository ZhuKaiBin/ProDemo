namespace EventSourceDemo.EventStores
{
    public interface IEventStore
    {
        // 保存事件到事件存储
        void SaveEvent(IDomainEvent domainEvent);

        // 根据聚合 ID 获取事件
        List<IDomainEvent> GetEventsByAggregateId(string aggregateId);
    }
}
