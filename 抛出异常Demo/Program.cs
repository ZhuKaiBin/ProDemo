namespace 抛出异常Demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            {
                //try
                //{
                //    // 模拟抛出异常
                //    throw new InvalidOperationException("An error occurred!");
                //}
                //catch (Exception ex)
                //{
                //    // 捕获异常并创建 ExceptionDispatchInfo
                //    var edi = System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(ex);

                //    // 可以在这里进行其他操作，之后将异常重新抛出
                //    // 重新抛出并保留原始的堆栈跟踪信息
                //    edi.Throw();
                //}

            }

            {
                try
                {
                    Test();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }


        private static void Test()
        {
            try
            {
                Test2();
            }
            catch (Exception ex)
            {
                //throw ex; //会丢失调用链,找不到真正的异常所在
                throw; //调用链完整
                //System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(ex).Throw();//调用链更完整，显示了重新抛出异常所在的位置。
            }
            //finally
            //{
            //    Console.WriteLine("finally");
            //}
        }

        private static void Test2()
        {
            try
            {
                throw new Exception("test2");
            }
            catch (Exception ex)
            {
                //throw ex; //会丢失调用链,找不到真正的异常所在
                //throw; //调用链完整
                System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(ex).Throw();//调用链更完整，显示了重新抛出异常所在的位置。
            }
        }
    }


}
