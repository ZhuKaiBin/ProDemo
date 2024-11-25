using MiniExcelLibs;

namespace MIniExcel
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // SheetA 数据
            var sheetAData = new List<Dictionary<string, object>>()
        {
            new Dictionary<string, object> { { "aa", "value1" }, { "bb", "value2" }, { "cc", "value3" } }
        };

            // SheetB 数据
            var sheetBData = new List<Dictionary<string, object>>()
        {
            new Dictionary<string, object> { { "mm", "value4" }, { "nn", "value5" } }
        };

            // 创建 Excel 文件并写入两个工作表
            var filePath = "output.xlsx";
            MiniExcel.SaveAs(filePath, new
            {
                SheetA = sheetAData,
                SheetB = sheetBData
            });
        }
    }
}
