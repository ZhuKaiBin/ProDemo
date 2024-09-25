using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace 组件扫描
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
    }


    public class Extension
    {
        public static IServiceCollection serviceDescriptors<T>(this IServiceCollection serviceDescriptors)
        {
            var types = typeof(T).Assembly.GetTypes();

            foreach (var item in types)
            {
                //GetCustomAttributes,只用来收集InjectionAttribute的标签
                var injection = item.GetCustomAttribute<InjectionAttribute>();
                if (injection!.ServiceType == null)
                {
                    serviceDescriptors.Add(new ServiceDescriptor(item, item, injection.Lifetime));
                }
            }
        }
    }


    [AttributeUsage(AttributeTargets.Class)]
    public class InjectionAttribute : Attribute
    {
        public Type? Type { get; set; }

        public ServiceLifetime Lifetime { get; set; } = ServiceLifetime.Transient;
    }
}
