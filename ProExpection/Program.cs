using System;

namespace ProExpection
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            try
            {
                throw new InternalExpection("001", "P1", "P2", "我是朱凯宾");

            }
            catch (Exception ex)
            {
                throw new InternalExpection("001", "P1", "P2", ex.Message);


               string type= ex.GetType().ToString();
            }

        }

        public class InternalExpection : Exception
        {
            public string errorcode { set; get; }
            public string p1 { set; get; }
            public string p2 { set; get; }

            public InternalExpection(string erroecode, string p1, string p2, string message) : base(message)
            {
                this.p1 = p1;
                this.p2 = p2;
                this.errorcode = errorcode;
            }
        }
    }
}
