using System;

namespace ProSingletonStatic
{
    class Program
    {
        static void Main(string[] args)
        {
            Singleton.x = 100;
            Singleton.y = 200;
            var ins = Singleton.instance;

            ins.Method("Test");

            Singleton.x = 1000;
            Singleton.y = 2000;

            ins.Method("Test");
        }
    }


    public class Singleton
    {

        public static readonly Singleton instance;

        public static int x;
        public static int y;
        //即使是多个线程下，也是可以保证只有一个instance,因为静态构造函数，只执行一次
        static Singleton()
        {
            if (instance == null)
            {

                instance = new Singleton(x, y);
            }
        }

        private Singleton(int x, int y)
        {
            Singleton.x = x;
            Singleton.y = y;
        }


        public void Method(string name)
        {
            Console.WriteLine($"x={x},y={y}");
        }
    }
}
