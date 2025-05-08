using System.IO.Pipes;
using System.Security.AccessControl;
using System.Text;

namespace NamedPipeServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string pipeName = "testpipe_20250430";  // 定义管道名称

            // 创建一个命名管道服务器
            using (NamedPipeServerStream pipeServer = new NamedPipeServerStream(pipeName, PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous))
            {
                Console.WriteLine("等待客户端连接...");
                pipeServer.WaitForConnection();  // 等待客户端连接

                Console.WriteLine("客户端已连接。");

                // 读取客户端发送的数据
                using (StreamReader reader = new StreamReader(pipeServer, Encoding.UTF8))
                {
                    string message = reader.ReadLine();
                    Console.WriteLine($"收到消息: {message}");
                }

                Console.WriteLine("通信完成，按任意键退出...");
                Console.ReadKey();
            }
        }
    }
}
