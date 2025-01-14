using System.Diagnostics;
using System;
using EnsureThat;

namespace MsProviderHelper
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region Debug.Assert
            {

#if DEBUG
                // 设置一个 TraceListener，将 Debug 输出写到控制台
                Trace.Listeners.Add(new ConsoleTraceListener());
                Trace.AutoFlush = true;
#endif

                int x = -2;

                // 使用 Debug.Assert 来验证 x 是否大于 0
                Debug.Assert(x > 0, "x must be greater than zero");

                Console.WriteLine("Program continues running...");
            }

            #endregion

            {
                string? input = null; // 可以修改为任意值来测试不同的输入

                try
                {
                    // 对引用类型进行非 null 校验
                    Ensure.That(input).IsNotNull();

                    Console.WriteLine("Input is valid.");
                }
                catch (ArgumentException ex)
                {
                    // 捕获验证失败的异常
                    Console.WriteLine($"Validation failed: {ex.Message}");
                }

            }


            {

                var s = new Bucket(10, TimeSpan.FromSeconds(1));
            }
        }
    }
}
