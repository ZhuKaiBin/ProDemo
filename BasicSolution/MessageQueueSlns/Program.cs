using System;
using System.Messaging;


namespace MessageQueueSlns
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 发送消息
            SendMessage("Hello from MSMQ");

            // 接收消息
            ReceiveMessage();
        }



        static void SendMessage(string message)
        {
            string queuePath = @".\private$\myQueue";

            // 创建队列
            if (!MessageQueue.Exists(queuePath))
            {
                MessageQueue.Create(queuePath);
            }

            using (MessageQueue queue = new MessageQueue(queuePath))
            {
                queue.Send(message);
                Console.WriteLine($"Sent: {message}");
            }
        }


        static void ReceiveMessage()
        {
            string queuePath = @".\private$\myQueue";

            using (MessageQueue queue = new MessageQueue(queuePath))
            {
                queue.Formatter = new XmlMessageFormatter(new String[] { "System.String,mscorlib" });

                // 获取一条消息
                Message message = queue.Receive();
                string content = (string)message.Body;
                Console.WriteLine($"Received: {content}");
            }
        }
    }
}
