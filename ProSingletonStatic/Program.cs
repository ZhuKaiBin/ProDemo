using System;

namespace ProSingletonStatic
{
    class Program
    {
        static void Main(string[] args)
        {
            var ins = Singleton.instance;
            ins.Method("Test");
        }
    }


    public class Singleton
    {

        public static readonly Singleton instance;

        static Singleton()
        {
            instance = new Singleton();
        }

        private Singleton() { }


        public void Method(string name)
        {
            Console.WriteLine(name);
        }
    }
}
