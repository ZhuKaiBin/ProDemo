using System;
using Castle.DynamicProxy;

namespace AOP
{
    class Program
    {
        static void Main(string[] args)
        {
            var target = new PhoneService("10086", "查询余额");
            target.Excute();

            //实现1：AOP 模式：对原有逻辑的一些增强,并不影响到原有的逻辑
            var aopTarGet = new PhoneServiveAOP(target);

            ////引入组件(castle.core)，动态代理
            //var castltAop = new ProxyGenerator();
            //var proxy = castltAop.CreateInterfaceProxyWithTarget<IPhoneService>(target, new NewService1(), new NewService2());
            //proxy.Excute();

            Console.Read();
        }
    }

    /// <summary>
    /// 定义一个接口IPhoneService 基础
    /// </summary>
    public interface IPhoneService
    {
        string Mobile { set; get; }
        string Message { set; get; }

        void Excute();
    }

    /// <summary>
    /// 一个功能实现接口IPhoneService
    /// </summary>
    public class PhoneService : IPhoneService
    {
        public PhoneService(string moible, string message)
        {
            Mobile = moible;
            Message = message;
        }

        public string Mobile { get; set; }
        public string Message { get; set; }

        public virtual void Excute()
        {
            Console.WriteLine($"已经发送短信：{Message}给到{Mobile}");
        }
    }

    //共同继承底层类
    //PhoneServiveAOP 和 实现者

    public class PhoneServiveAOP : IPhoneService
    {
        private readonly IPhoneService _target;

        /// <summary>
        /// 这个target是制要传入的某个具体的示例
        /// </summary>
        /// <param name="target"></param>
        public PhoneServiveAOP(IPhoneService target)
        {
            _target = target;
        }

        public string Mobile
        {
            get => _target.Mobile;
            set => _target.Mobile = value;
        }
        public string Message
        {
            get => _target.Message;
            set => _target.Message = value;
        }

        /// <summary>
        /// 重写父类方法,这里可以统一进行切面业务
        /// </summary>
        public void Excute()
        {
            Console.WriteLine("this is PreExcute");
            _target.Excute();
            Console.WriteLine("this is AfterExcute");
        }
    }
}
