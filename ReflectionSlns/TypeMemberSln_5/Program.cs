using CommondLib;
using System.Reflection;

namespace TypeMemberInfoSln_5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            {
                var type = typeof(Class4);
                object example = Activator.CreateInstance(type);
                var members = type.GetMembers();

                foreach (var item in members)
                {
                    if (item.MemberType == MemberTypes.Method)
                    {

                        // 输出此方法的参数列表：参数类型+参数名称
                        foreach (ParameterInfo pi in ((MethodInfo)item).GetParameters())
                        {
                            Console.WriteLine("Parameter: Type={0}, Name={1}", pi.ParameterType, pi.Name);
                        }

                        if (((MethodInfo)item).GetParameters().Length == 2)
                        {
                            // 调用一个方法以及传递参数
                            MethodInfo method = (MethodInfo)item;
                            Console.WriteLine("调用一个方法，输出结果：");
                            Console.WriteLine(method.Invoke(example, new object[] { "1", "2" }));
                        }

                    }

                }

            }
        }
    }
}
