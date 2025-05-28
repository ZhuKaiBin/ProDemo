using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Common.Enums
{
    public enum ErrorCode
    {
        Unknown = 0,
        BusinessError = 1001,
        ValidationError = 1002,
        DatabaseError = 1003,
        AuthorizationError = 1004
        // 可扩展
    }
}
