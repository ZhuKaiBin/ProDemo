using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProIOC
{
  public  interface IConstructor
    {
        public ITestServices service { get; }
    }
}
