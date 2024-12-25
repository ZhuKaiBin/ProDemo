using Microsoft.Extensions.DependencyInjection;


namespace ServiceCollectionDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {


            //// 创建一个服务集合（就像餐厅的厨房）
            //var serviceCollection = new ServiceCollection();
            //// 在厨房中注册服务（例如：Greeter）
            //serviceCollection.AddSingleton<IGreeter, Greeter>();

            //// 构建 IServiceProvider（就像雇佣了一个服务员）
            //var serviceProvider = serviceCollection.BuildServiceProvider();

            //// 服务员通过 IServiceProvider 获取服务（就像服务员从厨房拿菜）
            //var greeter = serviceProvider.GetService<IGreeter>();

            //// 使用服务（就像客人享用餐品）
            //greeter.Greet("World");



            //那么就是这样，然后要用到Greet这个方法的地方，就直接用构造函数引入就行了，
            //也不需要在意是传入了什么参数，也是解耦了，不需要每一次调用的时候，都去new Greeter()对象了

        }
    }


    public interface IGreeter
    {
        void Greet(string name);
    }

    public class Greeter : IGreeter
    {
        public void Greet(string name)
        {
            Console.WriteLine($"Hello, {name}!");
        }
    }
}
