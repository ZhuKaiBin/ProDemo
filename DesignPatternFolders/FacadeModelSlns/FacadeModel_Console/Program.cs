using System.Threading.Channels;

namespace FacadeModel_Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Facade facade = new Facade();
            facade.MethodB();
        }
    }


    public class Facade
    {
        private SubSystemOne _one;
        private SubSystemTwo _two;
        private SubSystemThree _three;
        public Facade()
        {
            _one = new SubSystemOne();
            _two = new SubSystemTwo();
            _three = new SubSystemThree();
        }
        public void MethodA()
        {
            Console.WriteLine("MethodA() ---- ");
            _one.MethodOne();
        }
        public void MethodB()
        {
            Console.WriteLine("MethodB() ---- ");
            _two.MethodTwo();
            _one.MethodOne();
        }
    }



    public class SubSystemOne
    {
        public void MethodOne()
        {
            Console.WriteLine(" SubSystemOne Method");
        }
    }

    public class SubSystemTwo
    {
        public void MethodTwo()
        {
            Console.WriteLine(" SubSystemTwo Method");
        }
    }

    public class SubSystemThree
    {
        public void MethodThree()
        {
            Console.WriteLine(" SubSystemThree Method");
        }
    }
}
