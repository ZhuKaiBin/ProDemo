using Newtonsoft.Json;
using EventSourceDemo.EfDbContexts;
using EventSourceDemo.Entities;


namespace EventSourceDemo.EventStores
{
    public class EfEventStore : IEventStore
    {
        private readonly EfDbContext _context;

        public EfEventStore(EfDbContext context)
        {
            _context = context;
        }

        public void SaveEvent(IDomainEvent domainEvent)
        {
            var eventEntity = new EventStore
            {
                AggregateId = domainEvent.GetType().Name,  // 这里可以存储订单的ID等
                EventType = domainEvent.GetType().Name,
                EventData = JsonConvert.SerializeObject(domainEvent),  // 序列化事件数据
                Timestamp = domainEvent.Timestamp
            };

            _context.EventStore.Add(eventEntity);
            _context.SaveChanges();
        }

        public List<IDomainEvent> GetEventsByAggregateId(string aggregateId)
        {
            var events = _context.EventStore
                .Where(e => e.AggregateId == aggregateId)
                .OrderBy(e => e.Timestamp)
                .ToList();

            return events.Select(e =>
                (IDomainEvent)JsonConvert.DeserializeObject(e.EventData, Type.GetType(e.EventType))
            ).ToList();
        }
    }
}
