using CleanArchitecture.Application.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Common.Exceptions
{
    public class BaseAppException : Exception
    {
        public ErrorCode Code { get; }
        public string ErrorMessage { get; set; }

        protected BaseAppException(ErrorCode code, string message)
            : base(message)
        {
            Code = code;
            ErrorMessage = message;
        }

        protected BaseAppException(string message)
            : base(message)
        {
            Code = ErrorCode.Unknown; // 默认错误码
            ErrorMessage = message;
        }

        protected BaseAppException(string message, Exception innerException)
            : base(message, innerException)
        {
            Code = ErrorCode.Unknown;
            ErrorMessage = message;
        }

        protected BaseAppException(ErrorCode code, string message, Exception innerException)
            : base(message, innerException)
        {
            Code = code;
            ErrorMessage = message;
        }
    }

}
