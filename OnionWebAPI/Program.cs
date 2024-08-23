using Infrastructure;

namespace OnionWebAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var webHost = CreateHostBuilder(args).Build();

            await ApplyMigrations(webHost.Services);

            await webHost.RunAsync();
        }

        private static async Task ApplyMigrations(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;//�ṩ�ض��ģ����񣩺͹���

                try
                {
                    //ָ��Ҫ������ݿ⣬���ṩ����
                    var context = services.GetRequiredService<EFDbContext>();
                    // context.Database.Migrate();
                    context.Database.EnsureCreated();
                    //await SeedData.InitializeAsync(services);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "��ʼ����������ʱ����. {exceptionMessage}", ex.Message);
                }
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
    }
}