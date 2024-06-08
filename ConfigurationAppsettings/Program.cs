using Microsoft.Extensions.Configuration;

namespace ConfigurationAppsettings
{
    internal class Program
    {
        static void Main(string[] args)
        {


            //// 读取环境变量
            //var environment = Environment.GetEnvironmentVariable("MY_APP_ENVIRONMENT") ?? "Production";

            //// 输出环境变量的值
            //Console.WriteLine($"输出环境变量的值: {environment}");

            //// 根据环境变量值执行不同的操作
            //switch (environment)
            //{
            //    case "Development":
            //        Console.WriteLine("Running in Development mode");
            //        // 开发环境特定操作
            //        break;
            //    case "Staging":
            //        Console.WriteLine("Running in Staging mode");
            //        // 测试环境特定操作
            //        break;
            //    case "Production":
            //        Console.WriteLine("Running in Production mode");
            //        // 生产环境特定操作
            //        break;
            //    default:
            //        Console.WriteLine("Running in Default mode");
            //        // 默认操作
            //        break;
            //}

            //// 保持控制台窗口打开（可选）
            //Console.WriteLine("Press any key to exit...");
            //Console.ReadKey();



            // 获取当前环境名称，默认为Production
            var environment = Environment.GetEnvironmentVariable("MY_APP_ENVIRONMENT");


            Console.WriteLine("=======" + environment);

            // 构建配置对象
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            // 读取配置
            var applicationName = configuration["ApplicationName"];
            var logLevel = configuration["Logging:LogLevel:Default"];

            // 输出配置
            Console.WriteLine($"Environment: {environment}");
            Console.WriteLine($"Application Name: {applicationName}");
            Console.WriteLine($"Log Level: {logLevel}");


            Console.ReadKey();
        }
    }
}
