using System.Globalization;
using System.Reflection;

namespace EmitSln
{
    internal class Program
    {
        static void Main(string[] args)
        {
            {
                AssemblyName assemblyName = new AssemblyName("MyTest");
                assemblyName.Name = "MyTest";   // 构造函数中已经设置，此处可以忽略

                // Version 表示程序集、操作系统或公共语言运行时的版本号.
                // 构造函数比较多，可以选用 主版本号、次版本号、内部版本号和修订号
                // 请参考 https://docs.microsoft.com/zh-cn/dotnet/api/system.version?view=netcore-3.1
                assemblyName.Version = new Version("1.0.0");
                assemblyName.CultureName = CultureInfo.CurrentCulture.Name; // = "zh-CN" 
                assemblyName.SetPublicKeyToken(new Guid().ToByteArray());
            }

            { 
            
            
            
            }
        }
    }
}
