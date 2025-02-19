namespace EventSourceDemo.Entities;
public class EventStore
{
    public int EventId { get; set; }
    public string AggregateId { get; set; }  // 关联订单的ID
    public string EventType { get; set; }    // 事件类型
    public string EventData { get; set; }    // 事件数据（JSON）
    public DateTime Timestamp { get; set; }  // 事件发生时间
}

