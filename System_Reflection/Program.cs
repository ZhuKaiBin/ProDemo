using System.Reflection;

namespace System_Reflection
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // 可以从程序集中加载所有类型
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] types = assembly.GetTypes();

            foreach (Type type in types)
            {
                // 检查类型是否实现了接口 IA
                if (typeof(IA).IsAssignableFrom(type) && type.IsClass)
                {
                    // 创建实例并调用方法
                    IA instance = (IA)Activator.CreateInstance(type);
                    instance.MethodA();
                }
            }

            System_Reflection_Library.Class1 class1 = new System_Reflection_Library.Class1();

            // 获取当前主程序的程序集
            Assembly mainAssembly = Assembly.GetExecutingAssembly();

            // 获取主程序引用的所有程序集
            AssemblyName[] referencedAssemblies = mainAssembly.GetReferencedAssemblies();

            // 遍历所有引用的程序集
            foreach (var assemblyName in referencedAssemblies)
            {
                // 找到类库 C 的程序集
                if (assemblyName.Name == "LibraryC")
                {
                    // 加载类库 C 的程序集
                    Assembly assemblyC = Assembly.Load(assemblyName);

                    // 示例：实例化类库 C 中的某个类并调用方法
                    Type typeC = assemblyC.GetType("LibraryC.SomeClass"); // 替换成类库 C 中的实际类型
                    if (typeC != null)
                    {
                        dynamic instance = Activator.CreateInstance(typeC);
                        instance.SomeMethod();
                    }
                    else
                    {
                        Console.WriteLine("Type not found in Library C.");
                    }
                }
            }
        }
    }

    public interface IA
    {
        void MethodA();
    }

    public class ClassA : IA
    {
        public void MethodA()
        {
            Console.WriteLine("Method A from ClassA");
        }
    }

    public class ClassB : IA
    {
        public void MethodA()
        {
            Console.WriteLine("Method A from ClassB");
        }
    }
}