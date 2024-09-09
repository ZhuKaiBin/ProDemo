namespace ConsoleApp7;

class Program
{
    static void Main(string[] args)
    {
        var person1 = new Person("John", "Doe");
        var person2 = new Person("John", "Doe");

        // person1.FirstName = "66";
        
        var Per = new Per { FirstName = "John", LastName = "Doe", Age = 30 };
        
        Per.Age = 31; // 可以修改 Age 属性
        Console.WriteLine("Hello, World!");
    }
    
    public record Person(string FirstName, string LastName);

    public record Per
    {
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public int Age { get; set; }
    }
}