using MediatR;
using MediatRDemo.RequestMsg;
using Microsoft.Extensions.DependencyInjection;

namespace MediatRDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            var services = new ServiceCollection();
            //将MediatR组件进行注册到容器中
            services.AddMediatR(typeof(Program).Assembly);
            //获取容器
            var container = services.BuildServiceProvider();
            //从容器中获取MediatR组件
            var mediator = container.GetService<IMediator>();


            //单个消息的通知
            //var response = mediator.Send(new MyRequest { RequestType = "josn" }).Result;
            //Console.WriteLine(response);

            //多个消息的通知
            mediator.Publish(new MyNotificationMsg { MsgType="json",Message="中国龙" }).ConfigureAwait(false);







        }
    }


}
