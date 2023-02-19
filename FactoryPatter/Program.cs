using System;

namespace FactoryPatter
{
    class Program
    {
        static void Main(string[] args)
        {
            Factory factory = new HuaWeiFactory();
            Phone phone=  factory.createPhone();
            phone.print();



            Factory factory2 = new IPhoneFactory();
            Phone phone2 = factory2.createPhone();
            phone2.print();
            Console.ReadKey();
        }
    }

    //抽象手机
    public interface Phone
    {
        void print();
    
    };
    //具体手机 苹果手机
    public class iPhone : Phone
    {
        public void print()
        {
            Console.WriteLine("苹果手机");
        }
    }
    //具体手机 华为手机
    public class HuaWeiPhone : Phone
    {
        public void print()
        {
            Console.WriteLine("华为手机");
        }
    }
    //具体手机 小米手机
    public class Xiaomi : Phone
    {
        public void print()
        {
            Console.WriteLine("小米手机");
        }
    }

   //抽象工厂
    public interface Factory
    {
        Phone createPhone();
    }

    //具体工厂 苹果工厂
    public class IPhoneFactory : Factory
    {
        public Phone createPhone()
        {
            return new iPhone();
        }
    }
    //具体工厂 华为工厂
    public class HuaWeiFactory : Factory
    {
        public Phone createPhone()
        {
            return new HuaWeiPhone();
        }
    }
    //具体工厂 小米工厂
    public class XiaomiFactory : Factory
    {
        Phone Factory.createPhone()
        {
            return new Xiaomi();
        }
    }
}
