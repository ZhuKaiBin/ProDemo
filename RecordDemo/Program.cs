namespace RecordDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 示例数据：创建一个 List<Order> 
            var orders = new List<Order>
                      {
                          new Order
                          {
                              name = "Order1",
                              Price = 100.0m,
                              OrderDetails = new List<OrderDetail>
                              {
                                  new OrderDetail { Name = "Item1", Price = 50.0m, Address = "Address1" },
                                  new OrderDetail { Name = "Item2", Price = 50.0m, Address = "Address2" }
                              }
                          },
                          new Order
                          {
                              name = "Order2",
                              Price = 200.0m,
                              OrderDetails = new List<OrderDetail>
                              {
                                  new OrderDetail { Name = "Item3", Price = 100.0m, Address = "Address3" },
                                  new OrderDetail { Name = "Item4", Price = 100.0m, Address = "Address4" }
                              }
                          }
                      };

            // 使用 LINQ 将 Order 列表转换为 OrderDto 列表
            List<OrderDto> orderDtos = orders.Select(order => new OrderDto(
                order.name,
                order.Price,
                order.OrderDetails.Select(detail => new OrderDetailDto(
                    detail.Name,
                    detail.Price,
                    detail.Address
                )).ToList()
            )).ToList();


        }
    }

    // 原始类定义
    public class Order
    {
        public string name { get; set; }
        public decimal Price { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }

    public class OrderDetail
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Address { get; set; }
    }

    // DTO类定义
    public record OrderDto(string Name, decimal Price, List<OrderDetailDto> OrderDetailDtos);

    public record OrderDetailDto(string Name, decimal Price, string Description);





}
