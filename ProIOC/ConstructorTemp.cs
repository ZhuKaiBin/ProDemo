using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProIOC
{
    public class ConstructorTemp : IConstructor
    {
        public ConstructorTemp()
        {
            Console.WriteLine("0参数的构造函数");
        }

        public ITestServices _services;

        public ConstructorTemp(ITestServices services)
        {
            _services = services;
            Console.WriteLine("有参数的构造函数");
        }

        public ITestServices service
        {
            get { return _services; }
        }
    }
}
