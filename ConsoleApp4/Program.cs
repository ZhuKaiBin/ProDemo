using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;


namespace ConsoleApp4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string dwgFilePath = @"C:\Users\dyzhukb\Desktop\CADDemo\5320db1794en.dxf";
            string svgFilePath = @"C:\Users\dyzhukb\Desktop\CADDemo";

            try
            {
                // 加载 DWG 文件
                using (var cadImage = (CadImage)Image.Load(dwgFilePath))
                {
                    // 设置输出文件格式为 SVG
                    var options = new CadRasterizationOptions
                    {
                        PageWidth = 1600, // 设置输出 SVG 的页面宽度
                        PageHeight = 1200 // 设置输出 SVG 的页面高度
                    };

                    // 将 DWG 文件转换为 SVG 文件
                    cadImage.Save(svgFilePath);
                }

                System.Console.WriteLine("DWG 文件已成功转换为 SVG 文件。");
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine("转换过程中发生错误：" + ex.Message);
            }
        }
    }
}
