using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using ConvertApiDotNet;

namespace DWGTOSVG
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            {
                //string From = @"D:\cad\5320db1794en.dxf";
                //string To = @"D:\cad";
                //ConvertApi convertApi = new ConvertApi("7a2BOAw4bHQVjzU3");
                //var convert = await convertApi.ConvertAsync("dwg", "svg", new ConvertApiFileParam("File", From));
                //await convert.SaveFilesAsync(To);
            }

            {
                string DwgFiles = @"D:\cad\hj.dwg";
                string SvgFiles = @"C:\q.svg";
                // 加载输入 DWG 文件
                Image image = Image.Load(DwgFiles);

                //// 初始化 SvgOptions 类对象
                //ImageOptions.SvgOptions options = new Aspose.CAD.ImageOptions.SvgOptions();

                //// 设置 SVG 颜色模式
                //options.ColorType = SvgColorMode.Grayscale;
                //options.TextAsShapes = true;

                // 保存输出 SVG 文件
                //image.Save("sample.svg", options);
                image.Save(SvgFiles);
            }
        }
    }
}
