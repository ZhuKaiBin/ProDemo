using System;
using System.Threading;

namespace ProThreadResult
{
    class Program
    {
        static void Main(string[] args) { }

        public static void DoWork(object data)
        {
            Console.WriteLine("Static thread procedure. Data='{0}'", data);
        }

        public class ThreadWithState { }
    }
}
