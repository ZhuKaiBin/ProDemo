namespace AdapterModel_Console
{

    //适配器模式（Adapter Pattern）是一种结构型设计模式，目的是将一个类的接口转化为客户端所期望的另一种接口，使得原本由于接口不兼容而无法一起工作的类能够协同工作。

    //适配器模式通常包括以下角色：
    //目标接口（Target）：客户端所期待的接口。
    //适配者（Adaptee）：需要适配的类，拥有一个与目标接口不兼容的接口。
    //适配器（Adapter）：实现目标接口，将适配者的接口转换为目标接口。
    internal class Program
    {
        static void Main(string[] args)
        {
            // 创建一个适配者实例
            Adaptee adaptee = new Adaptee();

            ITarget target = new Adapter(adaptee);
            target.Request();
        }
    }

    /// <summary>
    /// 目标接口
    /// </summary>
    public interface ITarget
    {
        void Request();
    }


    // 适配者类(已经存在的类，但是这个类不是客户想要的)
    //客户有一个usb的接口，但是现在只有一个typeC的接口，这时候就需要一个适配器
    public class Adaptee
    {
        public void SpecificRequest()
        {
            Console.WriteLine("Specific request from Adaptee.");
        }
    }

    /// <summary>
    /// 这个适配器就是将typeC接口转换成usb接口
    /// </summary>

    public class Adapter : ITarget
    {
        private Adaptee adaptee;
        public Adapter(Adaptee adaptee)
        {
            this.adaptee = adaptee;
        }

        public void Request()
        {
            // 适配器将目标接口的方法请求转发给适配者的相应方法
            adaptee.SpecificRequest();
        }
    }

}
