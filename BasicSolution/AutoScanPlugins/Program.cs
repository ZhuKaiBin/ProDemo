using System.Reflection;


namespace AutoScanPlugins
{
    internal class Program
    {
        static HashSet<string> loadedPlugins = new(); // 防止重复加载
        static void Main(string[] args)
        {
            //string pluginFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");

            //if (!Directory.Exists(pluginFolder))
            //    Directory.CreateDirectory(pluginFolder);

            Console.WriteLine("🔄 插件监控开始...（每5秒扫描一次）");

            while (true)
            {
                var dlls = Directory.GetFiles(@"Plugins", "*.dll");

                Console.WriteLine($"插件监控开始...（每5秒扫描一次）,时间：{DateTime.Now}");
                foreach (var dll in dlls)
                {
                    // if (loadedPlugins.Contains(dll))
                    //    continue;

                    try
                    {
                        var asm = Assembly.LoadFrom(dll);
                        var types = asm.GetTypes().Where(t => typeof(IPlugin).IsAssignableFrom(t) && !t.IsInterface);

                        foreach (var type in types)
                        {
                            var plugin = (IPlugin?)Activator.CreateInstance(type);
                            if (plugin != null)
                            {
                                Console.WriteLine($"🧩 加载插件：{plugin.Name}");
                                plugin.Run();
                            }
                        }

                        loadedPlugins.Add(dll);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"❌ 加载插件失败：{dll}\n{ex.Message}");
                    }
                }

                Thread.Sleep(5000);
            }
        }
    }

    public interface IPlugin
    {
        string Name { get; }
        void Run();
    }

}
