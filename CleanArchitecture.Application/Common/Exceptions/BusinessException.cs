using CleanArchitecture.Application.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Common.Exceptions
{
    public class BusinessException : BaseAppException
    {
        // 直接传默认的业务异常码
        public BusinessException(string message)
            : base(ErrorCode.BusinessError, message)
        {
        }

        public BusinessException(ErrorCode code, string message)
            : base(code, message)
        {
        }

        public BusinessException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public BusinessException(ErrorCode code, string message, Exception innerException)
            : base(code, message, innerException)
        {
        }
    }

    public class ValidationAppException : BaseAppException
    {
        // 不要重复声明 Code，否则会隐藏基类属性
        public ValidationAppException(string message)
            : base(ErrorCode.BusinessError, message)  // 直接传默认code
        {
        }

        public ValidationAppException(ErrorCode code, string message)
            : base(code, message)  // 调用基类带code的构造函数，保证Code被正确赋值
        {
        }
    }

    public class SystemAppException : BaseAppException
    {
        // 不要重复声明 Code，否则会隐藏基类属性
        public SystemAppException(string message)
            : base(ErrorCode.BusinessError, message)  // 直接传默认code
        {
        }

        public SystemAppException(ErrorCode code, string message)
            : base(code, message)  // 调用基类带code的构造函数，保证Code被正确赋值
        {
        }
    }

    public class DatabaseException : BaseAppException
    {
        // 不要重复声明 Code，否则会隐藏基类属性
        public DatabaseException(string message)
            : base(ErrorCode.BusinessError, message)  // 直接传默认code
        {
        }

        public DatabaseException(ErrorCode code, string message)
            : base(code, message)  // 调用基类带code的构造函数，保证Code被正确赋值
        {
        }
    }

}
