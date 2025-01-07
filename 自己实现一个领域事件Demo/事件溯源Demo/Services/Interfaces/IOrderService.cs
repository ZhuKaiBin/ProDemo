namespace EventSourceDemo.Services.Interfaces
{
    public interface IOrderService
    {
        public void CreateOrder(string orderId, string customerName);
        public void OrderShipOrder(string orderId, DateTime shipDate);


    }
}
