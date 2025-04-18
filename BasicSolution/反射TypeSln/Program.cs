namespace 反射TypeSln
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Type type = typeof(MyClass);
            //Type baseType = type.BaseType;
            //Console.WriteLine("Hello, World!");

            Type type = typeof(System.IO.FileStream);
            Type[] list = type.GetInterfaces();
            foreach (var item in list)
                Console.WriteLine(item.Name);

            var baseType = type.BaseType;

            Console.ReadKey();
        }
    }

    public class MyClass
    { 
    
    }
}
