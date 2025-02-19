using Newtonsoft.Json;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;

namespace ProCloneable
{
    internal class Program
    {
        /*
         什么时候使用浅克隆和深克隆
            浅克隆：适用于你只关心对象本身的值类型字段（如 Name），且不介意共享引用类型字段的场景。
            深克隆：适用于你需要完全独立的对象副本，确保引用类型字段不共享内存地址的场景。
         */


        private static void Main(string[] args)
        {
            {
                Console.WriteLine(Path.DirectorySeparatorChar);
            }

            {
                Mouse mouse = new Mouse();
                IUSB usb = mouse;  // 使用接口类型引用

                Console.WriteLine("MOuse's Ret: " + mouse.Ret());  // 调用 MOuse 类中的 Ret 方法
                Console.WriteLine("Usb's Ret: " + usb.Ret());      // 调用接口 Usb 中的 Ret 方法
            }
            {
                string str = "hello world?";
                string encodedStr = HttpUtility.UrlEncode(str); // 编码为 hello%20world%3F
                string decodeStr = HttpUtility.UrlDecode(encodedStr); // 解码为hello world?
            }

            {
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
            {
                Person p1 = new Person
                {
                    Name = "Alice",
                    Address = new Address { City = "New York", Street = "123 St" }
                };

                Person p2 = (Person)p1.DeepCloneBatchByJson();
                p1.Address.City = "Los Angeles";  // 修改原对象的 Address

                Console.WriteLine(p2.Address.City);  // 输出: "New York"（p2 是深拷贝，完全独立）

            }
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
            //成员逐个的：指对数据结构中的每个成员分别进行操作。

            /*
             MemberwiseClone 是一个 浅克隆 方法，意思是它创建了一个新的对象，并且将原对象中的所有字段的【值】复制到新对象中。
             但是 如果某个字段是引用类型（比如 Address），它不会创建这个字段的副本，而是复制引用。
             这意味着，浅克隆后的对象和原对象的引用类型字段依然指向同一块内存地址。 

            假设你有两份 菜单（Person 对象），菜单上写着顾客的名字（Name），以及顾客的地址（Address）。
            如果你 浅克隆 了一个菜单，意味着你将顾客的名字和地址抄写到了一个新的菜单上，
            但地址字段只是抄写了原菜单上地址的地址，而不是具体的地址内容。
            结果，新旧菜单上的地址都指向了同一个地方——所以它们之间有共享的部分。

             */

            //直接复制当前对象的所有字段值，包括引用类型字段的引用。
            /*
             如果字段是 值类型（如 int、DateTime），它会直接复制值。
             如果字段是 引用类型（如 Address），它只复制引用，导致新对象和原对象的 Address 字段指向同一个内存地址。             
             */
            return this.MemberwiseClone();
        }

        public Person DeepCopy()
        {
            // 先进行浅克隆
            Person other = (Person)this.MemberwiseClone();

            // 深克隆：重新创建引用类型字段的副本(把被克隆对象中是引用类型的，自己重新创建)
            //深克隆 的关键是对引用类型字段进行递归复制，确保它们不共享内存地址。
            other.Address = new Address { City = this.Address.Street, Street = this.Address.City };
            return other;


            /*
            深克隆会创建一个完全独立的副本，不仅克隆了当前对象，还会克隆它的所有引用类型字段。
            也就是说，深克隆不仅复制了对象本身的值，还会递归地复制对象引用的所有字段，
            使得原对象和克隆对象完全独立，它们之间没有任何共享的部分。

 
            继续用菜单的比喻。如果你进行 深克隆，你不仅抄写了顾客的名字和地址，还单独复制了地址的内容。
            也就是说，新菜单上的地址指向的是一块新地方，而不再与原菜单共享地址内容。
            这样，新旧菜单之间的地址就完全独立了。
            */
        }

        public object DeepCloneBatchByJson()
        {
            // 使用JSON序列化和反序列化进行深拷贝
            string json = JsonConvert.SerializeObject(this);  // 将对象序列化为JSON字符串
            return JsonConvert.DeserializeObject<Person>(json);  // 反序列化为一个新的对象
        }


        public object DeepCloneBatchByBinary()
        {
            // 二进制序列化和反序列化实现深拷贝
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, this);  // 序列化对象
                stream.Seek(0, SeekOrigin.Begin);  // 重置流位置
                return (Person)formatter.Deserialize(stream);  // 反序列化创建新对象
            }
        }


    }

    public interface IUSB
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

    public class Mouse : IUSB
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