using MediatR;

namespace Scalar2Sln_Domain.Common
{
   // 定义了一个 抽象类 BaseEvent，用于表示所有领域事件的基类。
   // 它实现了 INotification 接口，这个接口来自于 MediatR 库，表示这类事件可以被 MediatR 发布与订阅。
   //用于领域事件机制，配合 IMediator.Publish() 使用。


    public abstract class BaseEvent: INotification
    {
    }
}
