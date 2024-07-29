using System.Reflection;

namespace AssemblyDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Assembly assembly = Assembly.GetExecutingAssembly();
            //AssemblyName assemblyName = assembly.GetName();
            //Console.WriteLine(assemblyName.Name); // 输出程序集的名称

            //Console.WriteLine("Hello, World!");


            //Assembly assembly = Assembly.GetExecutingAssembly();
            //Type[] types = assembly.GetTypes();
            //foreach (Type type in types)
            //{
            //    Console.WriteLine(type.FullName); // 输出程序集中每个类型的全名
            //}

            //Assembly assembly = Assembly.GetExecutingAssembly();
            //string location = assembly.Location;
            //Console.WriteLine(location); // 输出程序集文件的路径


            //Assembly assembly = Assembly.GetExecutingAssembly();
            //var definedTypes = assembly.DefinedTypes;
            //foreach (var type in definedTypes)
            //{
            //    Console.WriteLine(type.FullName); // 输出程序集中每个定义的类型的全名
            //}


            //Assembly assembly = Assembly.GetExecutingAssembly();
            //var customAttributes = assembly.GetCustomAttributes();
            //foreach (var attribute in customAttributes)
            //{
            //    Console.WriteLine(attribute.GetType().Name); // 输出每个自定义特性的类型名
            //}

            //Assembly assembly = Assembly.GetExecutingAssembly();
            //Type[] types = assembly.GetTypes();
            //foreach (Type type in types)
            //{
            //    Console.WriteLine(type.FullName); // 输出程序集中每个类型的全名
            //}


            //Assembly assembly = Assembly.GetExecutingAssembly();
            //string runtimeVersion = assembly.ImageRuntimeVersion;
            //Console.WriteLine(runtimeVersion); // 输出程序集的运行时版本


            //Assembly assembly = Assembly.GetExecutingAssembly();
            //var files = assembly.GetFiles();
            //foreach (var file in files)
            //{
            //    Console.WriteLine(file.Name); // 输出程序集中每个文件的名称
            //}

            //Assembly assembly = Assembly.GetExecutingAssembly();
            //string[] resourceNames = assembly.GetManifestResourceNames();
            //foreach (string resourceName in resourceNames)
            //{
            //    Console.WriteLine(resourceName); // 输出程序集中每个嵌入资源的名称
            //}


            //Assembly assembly = Assembly.GetExecutingAssembly();
            //Type[] exportedTypes = assembly.GetExportedTypes();
            //foreach (Type type in exportedTypes)
            //{
            //    Console.WriteLine(type.FullName); // 输出程序集中每个公共类型的全名
            //}


            //Assembly assembly = Assembly.GetExecutingAssembly();
            //Type type = assembly.GetType("AssemblyDemo._11");
            //object instance = Activator.CreateInstance(type);
            //Console.WriteLine(instance); // 输出创建的实例

            //Assembly assembly = Assembly.GetExecutingAssembly();
            //string codeBase = assembly.CodeBase;
            //Console.WriteLine(codeBase); // 输出程序集文件的 URL



            //Assembly assembly = Assembly.GetExecutingAssembly();
            //string location = assembly.Location;
            //Console.WriteLine(location); // 输出程序集文件的路径

            //Assembly assembly = Assembly.GetExecutingAssembly();
            //string assemblyLocation = assembly.Location;
            //Console.WriteLine("Assembly location: " + assemblyLocation);

            //var file = assembly.GetFile(assemblyLocation);
            //Console.WriteLine("File path: " + file);



            //Assembly assembly = Assembly.GetExecutingAssembly();
            //using (Stream stream = assembly.GetManifestResourceStream("AssemblyDemo._11"))
            //{
            //    if (stream != null)
            //    {
            //        StreamReader reader = new StreamReader(stream);
            //        Console.WriteLine(reader.ReadToEnd());
            //    }
            //    else
            //    {
            //        Console.WriteLine("Resource not found.");
            //    }
            //}




            Assembly assembly = Assembly.GetExecutingAssembly();
            var modules = assembly.GetModules();
            foreach (var module in modules)
            {
                Console.WriteLine(module.Name); // 输出程序集中每个模块的名称
            }
        }
    }
}
