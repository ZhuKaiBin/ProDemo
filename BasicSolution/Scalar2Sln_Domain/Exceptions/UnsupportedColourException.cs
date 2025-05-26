using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scalar2Sln_Domain.Exceptions
{
    public class UnsupportedColourException : Exception
    {
        public UnsupportedColourException(string code):base($"此颜色 \"{code}\" 暂不支持.")
        {
            
        }
    }
}
