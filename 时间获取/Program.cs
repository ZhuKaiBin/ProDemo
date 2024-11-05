namespace 时间获取
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            var date = DateTime.UtcNow.ToString("yyyyMMdd");
            string today = DateTime.Now.ToString("yyyyMMdd");

           Console.WriteLine(today);
        }

        // public string GenerateOrderNumber()
        // {
        //     // 获取当天的日期，格式为yyyyMMdd
        //     string today = DateTime.Now.ToString("yyyyMMdd");

        //     // 查询数据库中当天的订单号，假设返回的是一个按编号排序的列表
        //     List<string> orders = GetOrdersByDate(today); // 你需要实现这个数据库查询



        //     string newOrderNumber;
        //     if (orders.Count > 0)
        //     {
        //         // 获取当天最后一个订单号
        //         string lastOrderNumber = orders.Last();

        //         // 获取最后四位数字
        //         int lastNumber = int.Parse(lastOrderNumber.Substring(8, 4));

        //         // 生成新的订单编号
        //         newOrderNumber = $"{today}{(lastNumber + 1).ToString("D4")}";
        //     }
        //     else
        //     {
        //         // 如果当天没有订单，编号从0001开始
        //         newOrderNumber = $"{today}0001";
        //     }

        //     return newOrderNumber;
        // }

    }
}
