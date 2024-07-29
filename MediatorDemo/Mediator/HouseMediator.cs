using System.Xml.Serialization;

namespace MediatorDemo.Mediator
{
    /// <summary>
    /// 由于有多个房屋中介公司，这里先将其进行抽象出来，相当于结构图中Mediator
    /// </summary>
    public abstract class HouseMediator
    {
        //声明这个类就是说，抽象出来一个公共的卖房的一个方法

        public abstract void SendHouseMsg(string msg, People people);
    }
}
