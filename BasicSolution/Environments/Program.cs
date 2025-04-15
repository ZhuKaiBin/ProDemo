using System.Diagnostics;
using System.Globalization;

namespace Environments
{
    internal class Program
    {
        //这里的args在属性里面的调试：打开调试启用命令行参数UI
        static void Main(string[] args)
        {


            // 增加缩进
            Debug.Indent();
            Debug.WriteLine("Indented message");

            //// 减少缩进
            //Debug.Unindent();
            //Debug.WriteLine("Unindented message");


            // 仅在条件为 true 时写入信息
            Debug.WriteIf(-9 > 0, "x is greater than 0");
            Trace.WriteIf(-9 > 0, "x is greater than 0");




            // 1.获取命令行参数
            //Console.WriteLine("Command Line: " + Environment.CommandLine);


            // 2.获取命令行参数
            //string[] args = Environment.GetCommandLineArgs();

            //// 遍历打印参数
            //for (int i = 0; i < 100000000; i++)
            //{
            //    //Console.WriteLine($"Arg[{i}] = {args[i]}");
            //    Console.WriteLine(i);
            //}


            //// 获取当前工作目录(运行应用程序时的默认路径)
            //Console.WriteLine("Current Directory: " + Environment.CurrentDirectory);


            ////程序集所在的目录。
            //Console.WriteLine("App Directory: " + AppContext.BaseDirectory);


            //// 获取操作系统版本
            //Console.WriteLine("OS Version: " + Environment.OSVersion);

            //// 获取当前用户名
            //Console.WriteLine("User Name: " + Environment.UserName);

            //// 判断操作系统和进程是否为 64 位
            //Console.WriteLine("Is 64-bit OS: " + Environment.Is64BitOperatingSystem);
            //Console.WriteLine("Is 64-bit Process: " + Environment.Is64BitProcess);


            // 获取操作系统版本信息
            //var osVersion = Environment.OSVersion;
            //Console.WriteLine("操作系统版本信息：");
            //Console.WriteLine($"平台：{osVersion.Platform}");
            //Console.WriteLine($"版本号：{osVersion.Version}");
            //Console.WriteLine($"服务包：{osVersion.ServicePack}");

            // 获取当前文化区域设置
            Console.WriteLine("操作系统语言和区域设置：");
            Console.WriteLine($"当前文化区域设置：{CultureInfo.CurrentCulture.Name}");
            Console.WriteLine($"当前 UI 文化区域设置：{CultureInfo.CurrentUICulture.Name}");

            // 获取系统启动时间
            Console.WriteLine($"系统启动时间（毫秒）：{Environment.TickCount}");

            //// 获取 PATH 环境变量
            //string path = Environment.GetEnvironmentVariable("PATH");
            //Console.WriteLine($"PATH 环境变量：{path}");

            //// 获取临时文件夹路径
            ////string tempPath = Environment.GetFolderPath(Environment.SpecialFolder.Temp);
            ////Console.WriteLine($"临时文件夹路径：{tempPath}");

            //// 获取当前进程的工作集大小
            //Console.WriteLine($"当前进程的工作集大小：{Environment.WorkingSet} 字节");

            //// 获取当前环境中的换行符
            //Console.WriteLine($"当前环境中的换行符：{Environment.NewLine}");

            //// 获取系统目录路径
            //Console.WriteLine($"系统目录路径：{Environment.SystemDirectory}");
        }
    }
}
