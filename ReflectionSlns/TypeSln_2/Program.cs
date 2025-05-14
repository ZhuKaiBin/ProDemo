using System.Reflection;
using CommondLib;

namespace TypeSln_2
{
    internal class Program
    {
        //Type 类型是反射技术的基础，反射所有操作都离不开 Type。
        static void Main(string[] args)
        {

            {
                //Type type = typeof(Program);
                //Console.WriteLine(type.Namespace);
                //Console.WriteLine(type.Name);    // 类型名称
                //Console.WriteLine(type.FullName); // 类型的完全限定名(命名空间+类名)  TypeSln_2.Program

                //Assembly ass = type.Assembly;
                //Console.WriteLine(ass.FullName);//TypeSln_2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

                //Console.WriteLine("=======================\n");

            }

            {

                //Type type1 = typeof(Class1);

                //var sd= typeof(Class1).Assembly;//file:///C:/Program Files/dotnet/shared/Microsoft.NETCore.App/8.0.15/System.Console.dll

                //Assembly ass= Assembly.LoadFrom(@"D:/ProjectDemo/ProDemoSlns/ReflectionSlns/TypeSln_2/bin/Debug/net8.0/CommondLib.dll");
                //Type type = ass.GetType("CommondLib");
                //Type[] types = ass.GetTypes();

                ////Type:对应的是类：Class, 所有类以内的属性，就要从Type中提供的方法来获取

                //// 获取 CommondLib.dll 中的所有类型
                //foreach (var item in types)
                //{
                //    Console.WriteLine("item: " + item.Name);
                //}

                //Console.WriteLine("=======================\n");
            }

            {
                //泛型 Type

                //Type typeA = typeof(Dictionary<,>);
                //Type typeB = typeof(Dictionary<string, int>);
                //Type typeC = typeof(List<>);
                //Type typeD = typeof(List<string>);

                //Type tupleE = typeof(Tuple<,,,,>);

                //Console.WriteLine(typeA.Name);
                //Console.WriteLine(typeB.Name);
                //Console.WriteLine(typeC.Name);
                //Console.WriteLine(typeD.Name);
                //Console.WriteLine(tupleE.Name);

                //Console.WriteLine("=======================\n");

                //Console.WriteLine(typeA.FullName);
                //Console.WriteLine(typeB.FullName);
                //Console.WriteLine(typeC.FullName);
                //Console.WriteLine(typeD.FullName);
                //Console.WriteLine(tupleE.FullName);


                //Console.WriteLine("=======================\n");
                //Console.WriteLine($"是否是泛型：{typeA.IsGenericType}");

            }

            {
                //Type type = typeof(Dictionary<string, int>);
                //Type[] list = type.GetGenericArguments();
                //foreach (var item in list)
                //{
                //    Console.WriteLine(item.Name);
                //}

                //Console.WriteLine("=======================\n");
            }


            {
                //方法的参数和 ref / out
                //ParameterInfo[] paramList = typeof(Class3).GetMethod(nameof(Class3.Test)).GetParameters();
                //foreach (var item in paramList)
                //    Console.WriteLine(item);

                ////如果参数类型后面有 & ，则代表是 ref 或 out 修饰的参数。
                ////System.String str
                ////System.String & a
                ////System.String & b
                //Console.WriteLine("=======================\n");

            }



            {
                Console.WriteLine("=======================\n");

            }



            {
                Console.WriteLine("=======================\n");

            }







        }
    }
}
