using CommondLib;
using System;
using System.Reflection;
namespace AssemblyBaseSln_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            {
                //那么虽然你在项目中引用了CommondLib.Class1，但如果你 从未调用过它的任何类、方法或字段，.NET CLR 不会主动加载它，
                //AppDomain.CurrentDomain.GetAssemblies() 里自然也看不到它。
                //因为.NET 为了提高启动性能和内存效率，采用了延迟加载（Lazy Load）机制：
                //程序集不会在启动时全部加载;
                //只有首次用到的时候才会加载（或通过 Assembly.Load 强制加载）。


                //var dummy = typeof(Class1);
                //var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                //foreach (var item in assemblies)
                //{
                //    var fullName = item.FullName;
                //    var Location = item.Location;
                //    Console.WriteLine($"fullName:{fullName}\nLocation:{Location}\n");
                //}
            }

            {

                //Assembly assem = typeof(Console).Assembly;
                //Assembly ass = Assembly.GetExecutingAssembly();
                //var callingAssemble=  Assembly.GetCallingAssembly();
                //Class1 class1 = new Class1();
                //var d= class1.RetInfo();
                
            }

            {
                //Assembly assemA = typeof(Console).Assembly;
                //Assembly assemB = Assembly.GetExecutingAssembly();

                //Console.WriteLine("程序集完全限定名");
                //Console.WriteLine(assemA.FullName);
                //Console.WriteLine(assemB.FullName);

            }

            {

                Assembly assemA = Assembly.Load("System.Console");

            }

            Console.WriteLine("Hello, World!");
            Console.ReadKey();
        }
    }
}
