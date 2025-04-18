using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoScanPluginsLibs
{
    public class HelloPluginC : IPlugin
    {
        public string Name => "C 插件";

        public void Run()
        {
            Console.WriteLine("C 插件执行啦！");
        }
    }
}
