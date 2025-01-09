using Microsoft.Web.WebView2.Core;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WebView2Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isWebView2Initialized = false; // 添加标志位来防止多次初始化

        public MainWindow()
        {
            InitializeComponent();
            InitializeWebviewAsync();
        }

        private async void InitializeWebviewAsync()
        {

            // 如果 WebView2 已经初始化，则直接返回
            if (isWebView2Initialized || webview.CoreWebView2 != null)
            {
                return;
            }

            try
            {
                // 使用单一的 CoreWebView2Environment
                var options = new CoreWebView2EnvironmentOptions(language: "zh");
                var env = await CoreWebView2Environment.CreateAsync(null, null, options);
                await webview.EnsureCoreWebView2Async(env);

                isWebView2Initialized = true; // 标记为已初始化

                // 监听 WebView2 启动失败事件
                webview.CoreWebView2.ProcessFailed += WebView_ProcessFailed;

                // 设置 WebView2 的源内容
                LoadWebViewContent();

                // 设置 WebView2 的默认配置
                SetWebView2Settings();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"WebView2 启动失败，请联系开发人员。\n{ex.Message}");
            }
        }

        private void WebView_ProcessFailed(object sender, CoreWebView2ProcessFailedEventArgs e)
        {
            MessageBox.Show($"WebView2 启动失败，失败原因: {e.Reason}\n详细信息: {e.Reason.ToString()}");
        }

        private void LoadWebViewContent()
        {
            //https://localhost:7081/swagger/index.html  是后端项目WebAPIBackend的swagger地址
            //webview.CoreWebView2.Navigate("https://localhost:7081/swagger/index.html");

            //http://localhost:3000/ 是前端项目vue的地址Dist文件中的
            webview.CoreWebView2.Navigate("http://localhost:3000/");

            //#if DEBUG
            //            // 确保 Source 只在 WebView2 初始化后设置
            //            webview.CoreWebView2.Navigate("http://localhost:3000/");
            //#else
            //        try
            //        {
            //            webview.CoreWebView2.SetVirtualHostNameToFolderMapping("sq800.sample", "./dist", CoreWebView2HostResourceAccessKind.Deny);
            //            webview.CoreWebView2.Navigate("https://sq800.sample/index.html");
            //        }
            //        catch (Exception ex)
            //        {
            //            MessageBox.Show($"WebView2 启动失败，请联系开发人员。\n{ex.Message}");
            //        }
            //#endif
        }

        private void SetWebView2Settings()
        {
            var settings = webview.CoreWebView2.Settings;
            settings.AreDefaultContextMenusEnabled = false;
            settings.AreDevToolsEnabled = false;
            settings.AreBrowserAcceleratorKeysEnabled = false;
        }
    }


}