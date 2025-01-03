namespace 抛出异常全局兜底.BaseExceptionFiles
{
    // 基础异常类，所有自定义异常的基类
    public class BaseCustomException : Exception
    {
        public int ErrorCode { get; set; }
        public BaseCustomException(string message, int errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }
    }

    // 用户异常（例如，用户输入的错误）
    public class UserException : BaseCustomException
    {
        public UserException(string message) : base(message, 1001) { }
    }

    // 业务逻辑异常（例如，某个业务流程无法完成）
    public class LogicException : BaseCustomException
    {
        public LogicException(string message) : base(message, 2001) { }
    }

    // 参数异常（例如，参数不合法）
    public class ParameterException : BaseCustomException
    {
        public ParameterException(string message) : base(message, 3001) { }
    }
}
