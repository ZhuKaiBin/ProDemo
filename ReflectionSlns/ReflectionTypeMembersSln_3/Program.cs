using CommondLib;
using System.Reflection;

namespace ReflectionTypeMembersSln_3
{
    internal class Program
    {
        /// <summary>
        /// 类型的基类和接口
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //Type type = typeof(Class3);
            //Type baseType = type.BaseType;

            {
                //Type type = typeof(System.IO.FileStream);
                //Type[] list = type.GetInterfaces();
                //foreach (var item in list)
                //    Console.WriteLine(item.Name);


                //Console.WriteLine("+++++++++++++\n");

                //while (type != null)
                //{
                //    Console.WriteLine(type.FullName);
                //    type = type.BaseType;
                //}
            }

            {
                Type type = typeof(Class3);
                ConstructorInfo[] list = type.GetConstructors();//只返回公共（public）实例构造函数
                foreach (var item in list)
                {
                    Console.WriteLine(item.Name + "   |   " + item.IsStatic + "   |   " + item.IsPublic);

                    ParameterInfo[] parms = item.GetParameters();
                    foreach (var itemNode in parms)
                    {
                        Console.WriteLine(itemNode.Name + "   |   " + itemNode.ParameterType + "    |   " + itemNode.DefaultValue);
                    }
                }
                Console.WriteLine("+++++++++++++\n");


                //静态构造函数只能用TypeInitializer 来获取
                ConstructorInfo staticCtor = type.TypeInitializer;

                if (staticCtor != null)
                {
                    Console.WriteLine("Found static constructor: " + staticCtor.Name);
                }
                else
                {
                    Console.WriteLine("No static constructor found.");
                }

            }
        }
    }
}
