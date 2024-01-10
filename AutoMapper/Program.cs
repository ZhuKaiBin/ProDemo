namespace AutoMapper
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Method method = new Method();

            method.Orders();


        }
    }

    public class Order { }
    public class OnlineOrder : Order { }
    public class MailOrder : Order { }

    public class OrderDto { }
    public class OnlineOrderDto : OrderDto { }
    public class MailOrderDto : OrderDto { }


    public class Method
    {
        public  Order Orders()
        {
            var line = new OnlineOrder();
            return line;
        }
    }
}
