using System;
using System.Collections.Generic;

namespace ProMoq
{
    //https://www.cnblogs.com/tylerzhou/p/11410337.html
    // Moq是.net平台下的一个非常流行的模拟库,只要有一个接口它就可以动态生成一个对象,底层使用的是Castle的动态代理功能.

    //它的流行赖于依赖注入模式的兴起,现在越来越多的分层架构使用依赖注入的方式来解耦层与层之间的关系.最为常见的是数据层和业务逻辑层之间的依赖注入,业务逻辑层不再强依赖数据层对象,而是依赖数据层对象的接口,在IOC容器里完成依赖的配置.
    //这种解耦给单元测试带来了巨大的便利,使得对业务逻辑的测试可以脱离对数据层的依赖,单元测试的粒度更小,更容易排查出问题所在.
    //大家可能都知道,数据层的接口往往有很多方法,少则十几个,多则几十个.我们如果在单元测试的时候把接口切换为假实现,即使实现类全是空也需要大量代码,并且这些代码不可重用,一旦接口层改变不但要更改真实数据层实现还要修改这些专为测试做的假实现.这显然是不小的工作量.
    //幸好有Moq,它可以在编译时动态生成接口的代理对象.大大提高了代码的可维护性,同时也极大减少工作量.
    //除了动态创建代理外,Moq还可以进行行为测试,触发事件等.
    //Moq安装
    //Moq安装非常简单, 在Nuget里面搜索moq, 第一个结果便是moq框架,点击安装即可.
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }

    public class MyDto
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public interface IDataBaseContext<out T>
        where T : new()
    {
        T GetElementById(string id);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetElementsByName(string name);
        IEnumerable<T> GetPageElementsByName(string name, int startPage = 0, int pageSize = 20);
        IEnumerable<T> GetElementsByDate(DateTime? startDate, DateTime? endDate);
    }

    // MyDto为业务层和数据层交互的对象,
    //IDataBaseContext为数据层接口,MyBll为我们的业务逻辑层

    //我们要测试的是业务逻辑层的代码.这里业务逻辑类并没有无参构造函数, 如果手动创建起来非常麻烦, 里面的坑前面说过.下面看如何使用Moq来模拟一个IDataBaseContext对象

    public class MyBll
    {
        private readonly IDataBaseContext<MyDto> _dataBaseContext;

        public MyBll(IDataBaseContext<MyDto> dataBaseContext)
        {
            _dataBaseContext = dataBaseContext;
        }

        public MyDto GetADto(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return null;
            return _dataBaseContext.GetElementById(id);
        }
    }
}
