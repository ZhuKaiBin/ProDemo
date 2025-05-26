using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scalar2Sln_Domain.Constants
{
    public abstract class Policies
    {
        public const string CanPuge = nameof(CanPuge);
        public const string CanPurge2 = "CanPurge2";


        //这里 nameof(CanPurge) 的作用是避免硬编码字符串，写错了编译器会报错。


    }
}
