using System;
using System.Web;

namespace ProCloneable
{
    class Program
    {
        static void Main(string[] args)
        {


            {
                string str = "hello world?";
                string encodedStr = HttpUtility.UrlEncode(str); // 编码为 hello%20world%3F
                string decodeStr = HttpUtility.UrlDecode(encodedStr);// 解码为hello world?
            }

            // 创建一个 Person 对象
            Person p1 = new Person
            {
                Name = "Tom",
                Address = new Address
                {
                    Street = "123 Main St",
                    City = "Seattle"
                }
            };

            // 浅拷贝
            Person p2 = (Person)p1.Clone();
            //p1.Address = new Address() { City = "333", Street = "444" };
            Console.WriteLine($"p1.Address == p2.Address: {p1.Address == p2.Address}");  // 输出 true，即浅拷贝只复制了引用



            // 深拷贝
            Person p3 = p1.DeepCopy();
            //p1.Address = new Address() {City="111",Street="222" };
            Console.WriteLine($"p1.Address == p3.Address: {p1.Address == p3.Address}");  // 输出 false，即深拷贝复制了新的对象
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

            other.Address = new Address { 
             City=this.Address.Street,
             Street=this.Address.City
            };
            return other;
        }



    }
}
