using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProIOC
{
    public class TestServiceTemp : ITestServices
    {

        private int _count;      

        public int count { get { return _count; } }

        public TestServiceTemp()
        {
            _count = 0;
            Console.WriteLine("这里是TestServiceTemp的构造函数");
        }

      public  void Add()
        {
            _count++;
        }

       
    }
}
