namespace EventSourceDemo.Entities
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public string OrderId { get; set; }
        public string Product { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
