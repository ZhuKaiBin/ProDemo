using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProIOC
{
    public interface ITestServices
    {
        int count { get; }

        void Add();
    }
}
