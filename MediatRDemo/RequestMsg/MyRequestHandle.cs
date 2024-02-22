using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediatRDemo.RequestMsg
{

    
    public class MyRequestHandle : IRequestHandler<MyRequest, int>
    {
        public Task<int> Handle(MyRequest request, CancellationToken cts)
        {
            Console.WriteLine($"消息类型是{request.RequestType}");
            return Task.FromResult(1);
        }
    }
    public class MyRequestHandle2 : IRequestHandler<MyRequest, int>
    {
        public Task<int> Handle(MyRequest request, CancellationToken cts)
        {
            Console.WriteLine($"2消息类型是{request.RequestType}");
            return Task.FromResult(1);
        }
    }




    public class MyNotificationMsg : INotification
    {
        public string MsgType { set; get; }
        public string Message { set; get; }
    }


    public class MyNotificationHandle : INotificationHandler<MyNotificationMsg>
    {
        public Task Handle(MyNotificationMsg msg, CancellationToken cts)
        {
            Console.WriteLine($"消息处理1：消息类型是{msg.MsgType},消息内容是{msg.Message}");
            return Task.CompletedTask;
        }
    }


    public class MyNotificationHandle2 : INotificationHandler<MyNotificationMsg>
    {
        public Task Handle(MyNotificationMsg msg, CancellationToken cts)
        {
            Console.WriteLine($"消息处理2：消息类型2是{msg.MsgType},消息内容是{msg.Message}");
            return Task.CompletedTask;
        }
    }

    public class MyNotificationHandle3 : INotificationHandler<MyNotificationMsg>
    {
        public Task Handle(MyNotificationMsg msg, CancellationToken cts)
        {
            Console.WriteLine($"消息处理3：消息类型3是{msg.MsgType},消息内容是{msg.Message}");
            return Task.CompletedTask;
        }
    }





}
