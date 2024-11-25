using MiniExcelLibs;
using System.Collections.Generic;

namespace mimiimiexcel
{
    internal class Program
    {
        static void Main(string[] args)
        {
            {
                // SheetA 的数据
                var sheetAData = new List<dynamic>
        {
            new { aa = "Value1", bb = "Value2", cc = "Value3" },
            new { aa = "Value4", bb = "Value5", cc = "Value6" }
        };

                // SheetB 的数据
                var sheetBData = new List<dynamic>
        {
            new { mm = "Value7", nn = "Value8" },
            new { mm = "Value9", nn = "Value10" }
        };

                // 创建多 Sheet 数据
                var sheets = new Dictionary<string, object>
        {
            { "SheetA", sheetAData },
            { "SheetB", sheetBData }
        };

                // 导出到 Excel 文件
                MiniExcel.SaveAs("MultiSheetExcel.xlsx", sheets);

                Console.WriteLine("Excel 导出成功！");

            }
        }
    }
}
