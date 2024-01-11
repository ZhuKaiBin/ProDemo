using Microsoft.Extensions.DependencyInjection;
using System.Data;
using Autofac.Extensions.DependencyInjection;
using Autofac;

namespace NetCoreHuaJIanDao
{
    internal class Program
    {
        static void Main(string[] args)
        {
            

            {
                #region 工厂模式
                //IServiceCollection collection = new ServiceCollection();
                ////通过collection.AddSingleton<A>()将A类型注册为服务后，容器就能够解析和提供A类型的实例对象
                //collection.AddSingleton<A>();

                //collection.AddSingleton<DbConnectionFactory>(sp =>
                //{
                //    var values = new Dictionary<string, Type>() {
                //        { "s1",typeof(SqlConnection)},
                //        { "s2",typeof(MySqlConnection)},
                //    };
                //    return new DbConnectionFactory(sp, values);
                //});

                //IServiceProvider service = collection.BuildServiceProvider();
                ////在注入的时候 collection.AddSingleton<A>();
                ////那么在调用 var factory = service.GetService<A>(); 的时候，也是只能调用A；就是说，你注入什么类型的时候，那么GetService的时候就只能调用什么类型
                //var factory = service.GetService<DbConnectionFactory>();
                //var s1 = factory.Create("s2");
                #endregion
            }

            {
                //var list = new List<ServiceDescriptor>();
                //list.Add(new ServiceDescriptor(typeof(MySqlConnection), ServiceLifttime.Transient));
                //list.Add(new ServiceDescriptor(typeof(SqlConnection), ServiceLifttime.Transient));
                //var container = new Container(list);


                //var builder = new ContainerBuilder();
                //builder.AddTransient<MySqlConnection>();
                //builder.AddTransient<MySqlConnection>();
                //var container = builder.Builder();
            }
            {
              
            }



            Console.ReadLine();

        }
    }

    #region 工厂模式
    //public class DbConnectionFactory
    //{
    //    //用来实例化和解析服务，
    //    private IServiceProvider _serviceProvider;
    //    private Dictionary<string, Type> _connection;

    //    public DbConnectionFactory(IServiceProvider serviceProvider, Dictionary<string, Type> connection)
    //    {
    //        _connection = connection;
    //        _serviceProvider = serviceProvider;
    //    }

    //    public IDbConnection? Create(string name)
    //    {
    //        if (_connection.TryGetValue(name, out Type? connectionType))
    //        if (_connection.TryGetValue(name, out Type? connectionType))
    //        {
    //            return ActivatorUtilities.CreateInstance(_serviceProvider, connectionType) as IDbConnection;

    //            //这个过程是通过依赖注入实现的。在调用ActivatorUtilities.CreateInstance时，它会检查构造函数的参数类型，并使用容器中注册的服务来解析和提供相应的实例对象。
    //            //因此，当容器解析MySqlConnection时，会自动注入A类型的实例对象。
    //            //如果没有将A注册为服务，容器无法解析和提供A类型的实例对象，就会导致实例化MySqlConnection时的参数错误，从而报错。
    //            //通过collection.AddSingleton<A>()将A注册为服务，使得容器可以正确地解析和提供A类型的实例对象，从而解决了构造函数参数的依赖关系。
    //            //通过collection.AddSingleton<A>()将类A注册为服务后，容器会在需要使用A的地方实例化它。
    //            //在调用AddSingleton方法时，容器会将A类型的实例对象创建并缓存起来。这个实例对象会在整个应用程序的生命周期中保持不变，即单例模式。
    //            //当其他地方需要使用A时，容器会从缓存中获取已经创建好的实例对象，并将其提供给需要的地方。这样就确保了每次使用A都是同一个实例对象，保持了对象的共享和一致性。                

    //            //底层实现的原理是利用了依赖注入容器（如IServiceCollection和IServiceProvider），它们负责管理和提供应用程序中的各种依赖项。
    //            //在调用BuildServiceProvider方法后，容器会根据注册的服务类型和生命周期，创建相应的实例对象，并将其缓存起来。
    //            //[IServiceCollection和IServiceProvider就像是菜谱和厨师一样，是管理菜的]
    //            //当其他地方需要获取服务实例时，容器会根据类型查找缓存中的实例对象，并提供给请求方。                
    //            //因此，通过将类A注册为单例服务，容器会在需要使用A的地方自动实例化并提供该实例对象，使得依赖关系得到满足。

    //        }
    //        return default;
    //    }
    //}

    //public interface IDbConnection
    //{ }
    //public class SqlConnection : IDbConnection
    //{ }
    //public class MySqlConnection : IDbConnection
    //{
    //    //当容器实例化MySqlConnection时，会检查构造函数的参数，发现需要一个A类型的实例。
    //    //由于A已经被注册为服务，容器会自动解析并提供一个A类型的实例对象，然后将其传递给MySqlConnection的构造函数。
    //    private A _a;
    //    public MySqlConnection(A a)
    //    {
    //        _a = a;
    //    }
    //}
    //public class A
    //{
       
    //}
    #endregion

    #region 构建者模式
    //public enum ServiceLifttime
    //{
    //    Transient, Scoped
    //}

    //public interface IDbConnection
    //{ }
    //public class SqlConnection : IDbConnection
    //{ }
    //public class MySqlConnection : IDbConnection
    //{ }

    ///// <summary>
    ///// 服务描述
    ///// </summary>
    //public class ServiceDescriptor
    //{
    //    public Type _serviceType { get; }
    //    public ServiceLifttime _lifetime { get; }
    //    public ServiceDescriptor(Type ServiceType, ServiceLifttime lifetime)
    //    {
    //        _serviceType = ServiceType;
    //        _lifetime = lifetime;
    //    }
    //}
    ////目标对象
    //public interface IContainer
    //{ }
    ////如果直接创建，成本很很高，体验很差
    //public class Container : IContainer
    //{
    //    private List<ServiceDescriptor> _services = new();
    //    public Container(List<ServiceDescriptor> services)
    //    {
    //        _services = services;
    //    }
    //}

    //public interface IContainerBuilder
    //{
    //    //提供一个万金油的公式
    //    void Add(ServiceDescriptor descriptor);
    //    //builder 用来创建Container的
    //    IContainer Builder();
    //}

    ////实现上面的万金油接口
    //public class ContainerBuilder : IContainerBuilder
    //{
    //    private List<ServiceDescriptor> _services = new();

    //    public void Add(ServiceDescriptor descriptor)
    //    {
    //        _services.Add(descriptor);
    //    }
    //    public IContainer Builder()
    //    {
    //        return new Container(_services);
    //    }
    //}

    ////拓展构造者模式,
    //public static class IContainerBuilderExtensions
    //{
    //    public static void AddTransient<T>(this IContainerBuilder builder)
    //    {
    //        builder.Add(new ServiceDescriptor(typeof(T), ServiceLifttime.Transient));
    //    }
    //    public static void AddScoped<T>(this IContainerBuilder builder)
    //    {
    //        builder.Add(new ServiceDescriptor(typeof(T), ServiceLifttime.Scoped));
    //    }
    //}

    #endregion
}