using System;
using System.Web;
using System.IO;

namespace ProCloneable
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            {
                Console.WriteLine(Path.DirectorySeparatorChar);
            }

            {
                MOuse mouse = new MOuse();
                Usb usb = mouse;  // 使用接口类型引用

                Console.WriteLine("MOuse's Ret: " + mouse.Ret());  // 调用 MOuse 类中的 Ret 方法
                Console.WriteLine("Usb's Ret: " + usb.Ret());      // 调用接口 Usb 中的 Ret 方法
            }
            {
                string str = "hello world?";
                string encodedStr = HttpUtility.UrlEncode(str); // 编码为 hello%20world%3F
                string decodeStr = HttpUtility.UrlDecode(encodedStr); // 解码为hello world?
            }

            // 创建一个 Person 对象
            Person p1 = new Person
            {
                Name = "Tom",
                Address = new Address { Street = "123 Main St", City = "Seattle" }
            };

            // 浅拷贝
            Person p2 = (Person)p1.Clone();
            //p1.Address = new Address() { City = "333", Street = "444" };
            Console.WriteLine($"p1.Address == p2.Address: {p1.Address == p2.Address}"); // 输出 true，即浅拷贝只复制了引用

            // 深拷贝
            Person p3 = p1.DeepCopy();
            //p1.Address = new Address() {City="111",Street="222" };
            Console.WriteLine($"p1.Address == p3.Address: {p1.Address == p3.Address}"); // 输出 false，即深拷贝复制了新的对象
        }
    }

    public class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
    }

    public class Person : ICloneable
    {
        public string Name { get; set; }
        public Address Address { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public Person DeepCopy()
        {
            Person other = (Person)this.MemberwiseClone();

            other.Address = new Address { City = this.Address.Street, Street = this.Address.City };
            return other;
        }
    }

    public interface Usb
    {
        public abstract void OPen();

        public string Ret()
        {
            return "66";
        }

        public virtual string Vir()
        {
            return "";
        }
    }

    public class MOuse : Usb
    {
        public void OPen()
        {
            Console.WriteLine("");
        }

        public string Ret()
        {
            return "55";
        }

        public string Vir()
        {
            return "";
        }
    }
}