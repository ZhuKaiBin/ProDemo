namespace ActionDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var processor = new OrderProcessor();

            // 创建一个标准订单
            Order standardOrder = new Order
            {
                OrderType = OrderType.Standard,
                Quantity = 5
            };

            // 创建一个特殊订单
            Order specialOrder = new Order
            {
                OrderType = OrderType.Special,
                Quantity = 10
            };

            // 处理标准订单
            long standardOrderProcessingTime = processor.ProcessStandardOrder(standardOrder);
            Console.WriteLine($"标准订单处理时间: {standardOrderProcessingTime} ms");

            // 处理特殊订单
            long specialOrderProcessingTime = processor.ProcessSpecialOrder(specialOrder);
            Console.WriteLine($"特殊订单处理时间: {specialOrderProcessingTime} ms");
        }
    }

    public enum OrderType
    {
        Standard,
        Special
    }

    public class Order
    {
        public OrderType OrderType { get; set; }
        public int Quantity { get; set; }
    }

    public class OrderProcessor
    {
        // 通用的订单处理方法，接收两个参数并返回long类型的值
        public long ProcessOrder(Order order, Func<OrderType, int, long> processOrderFunc)
        {
            // 验证订单
            if (!ValidateOrder(order))
            {
                throw new InvalidOperationException("订单无效");
            }

            // 执行传入的处理逻辑并获取返回值
            long result = processOrderFunc(order.OrderType, order.Quantity);

            // 保存订单
            SaveOrder(order);

            // 返回处理结果
            return result;
        }

        // 处理标准订单
        public long ProcessStandardOrder(Order order)
        {
            return ProcessOrder(order, (orderType, quantity) =>
            {
                // 处理标准订单的逻辑
                Console.WriteLine("处理标准订单...");
                Console.WriteLine($"订单类型: {orderType}, 数量: {quantity}");

                // 返回处理的时间或其他结果
                return 100L;  // 假设处理标准订单的时间是100毫秒
            });
        }

        // 处理特殊订单
        public long ProcessSpecialOrder(Order order)
        {
            return ProcessOrder(order, (orderType, quantity) =>
            {
                // 处理特殊订单的逻辑
                Console.WriteLine("处理特殊订单...");
                Console.WriteLine($"订单类型: {orderType}, 数量: {quantity}");

                // 返回处理的时间或其他结果
                return 150L;  // 假设处理特殊订单的时间是150毫秒
            });
        }

        // 验证订单的逻辑
        private bool ValidateOrder(Order order)
        {
            // 假设所有订单都是有效的，实际应用中可以添加更多的验证逻辑
            return order != null && order.Quantity > 0;
        }

        // 保存订单的逻辑
        private void SaveOrder(Order order)
        {
            // 保存订单到数据库或其他存储，当前为模拟输出
            Console.WriteLine("订单已保存");
        }
    }
}