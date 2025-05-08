using System.IO.Pipes;
using System.Security.AccessControl;
using System.Text;

namespace NamedPipeClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string pipeName = "testpipe_20250430";  // 定义管道名称

            // 创建一个命名管道客户端
            using (NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", pipeName, PipeDirection.InOut))
            {
                // 连接到管道服务器
                Console.WriteLine("正在连接到服务器...");
                pipeClient.Connect();
                Console.WriteLine("已连接到服务器。");

                // 向服务器发送数据
                using (StreamWriter writer = new StreamWriter(pipeClient, Encoding.UTF8))
                {
                    writer.WriteLine("Hello from Client!");
                    writer.Flush();  // 确保数据被写入管道
                }

                Console.WriteLine("消息已发送。按任意键退出...");
                Console.ReadKey();
            }
        }
    }
}
