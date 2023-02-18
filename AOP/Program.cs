using Castle.DynamicProxy;
using System;

namespace AOP
{
    class Program
    {
        static void Main(string[] args)
        {
            var target = new PhoneService("10086", "查询余额");
            //target.Excute();
            //实现1：AOP 模式：对原有逻辑的一些增强,并不影响到原有的逻辑
            //var aopTarGet = new PhoneServiveProcess(target);
            //实现1.2：aopTarGet.Excute();
            //var log = new PhoneServiveProcess2(aopTarGet);
            //log.Excute();


            //引入组件(castle.core)，动态代理
            var castltAop = new ProxyGenerator();
            var proxy = castltAop.CreateInterfaceProxyWithTarget<IPhoneService>(target, new NewService1(), new NewService2());
            proxy.Excute();

            Console.Read();
        }
    }

    /// <summary>
    /// 定义一个接口IPhoneService
    /// </summary>
    public interface IPhoneService
    {
        string Mobile { set; get; }
        string Message { set; get; }

        void Excute();
    }

    /// <summary>
    /// 实现接口IPhoneService
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

    //代理模式
    //1.实现目标对象(接口)的标准
    //2.依赖目标对象(就是对原有逻辑的统一增强)
    //3.引入新业务
    public class PhoneServiveProcess : IPhoneService
    {
        private readonly IPhoneService _target;
        public PhoneServiveProcess(IPhoneService target)
        {
            _target = target;
        }
        public string Mobile { get => _target.Mobile; set => _target.Mobile = value; }
        public string Message { get => _target.Message; set => _target.Message = value; }

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

    public class PhoneServiveProcess2 : IPhoneService
    {
        private readonly IPhoneService _target;
        public PhoneServiveProcess2(IPhoneService target)
        {
            _target = target;
        }

        public string Mobile { get => _target.Mobile; set => _target.Mobile = value; }
        public string Message { get => _target.Message; set => _target.Message = value; }

        public void Excute()
        {
            Console.WriteLine("this is Prelog");
            _target.Excute();
            Console.WriteLine("this is Afterlog");
        }

    }


    public class NewService1 : IInterceptor
    {
        //IInvocation  链接器
        public void Intercept(IInvocation invocation)
        {
            Console.WriteLine("this is PreExcute");
            invocation.Proceed();
            Console.WriteLine("this is AfterExcute");
        }
    }

    public class NewService2 : IInterceptor
    {
        //IInvocation  链接器
        public void Intercept(IInvocation invocation)
        {
            Console.WriteLine("this is Prelog");
            invocation.Proceed();
            Console.WriteLine("this is Afterlog");
        }
    }
}
