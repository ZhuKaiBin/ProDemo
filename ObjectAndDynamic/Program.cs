namespace ObjectAndDynamic
{
    internal class Program
    {
        static void Main(string[] args)
        {
            object myObject = "Hello, World!";
            myObject = 42;

            Console.WriteLine(myObject);

            dynamic myDynamic = "Hello, Dynamic!";
            myDynamic = 42;
            Console.WriteLine(myDynamic);
        }
    }
}
