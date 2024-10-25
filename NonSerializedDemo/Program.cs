using System.Reflection;
using System.Text.Json.Serialization;

namespace NonSerializedDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Person person = new Person() { Name = "Alice", Age = 30, Password = "secret123" };
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(person));

            Assembly assembly = Assembly.GetExecutingAssembly();

            //这是获取所有的类 
            var types = assembly.GetTypes();



            AssemblyName assemblyName = assembly.GetName();

        }
    }

    class Person
    {
        public string Name;
        public int Age;

        [NonSerialized]
        public string Password;  // 这个字段不会被序列化

        public override string ToString()
        {
            return $"Name: {Name}, Age: {Age}, Password: {Password}";
        }
    }


}
