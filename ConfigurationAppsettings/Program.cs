using Microsoft.Extensions.Configuration;

namespace ConfigurationAppsettings
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 获取当前环境名称，默认为Production
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";


            Console.WriteLine("======="+environment);

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
        }
    }
}
