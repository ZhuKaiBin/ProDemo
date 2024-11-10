using OfficeOpenXml;

namespace EPPlusExcel
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ExcelReader excelReader = new ExcelReader();

            excelReader.ReadExcelToEntities(@"D:\文档\a\映射逻辑 1.9版本.xlsx");
        }
    }

    public class ExcelReader
    {
        public ExcelReader()
        {
            // 设置许可证上下文为非商业用途
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public List<ExcelEntity> ReadExcelToEntities(string filePath)
        {
            List<ExcelEntity> people = new List<ExcelEntity>();

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                var rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++) // 假设第一行是标题行
                {
                    var pp = new ExcelEntity
                    {
                        optionNameZh1 = worksheet.Cells[row, 1].Text,
                        optionValue1 = worksheet.Cells[row, 2].Text,
                        PageType1 = worksheet.Cells[row, 3].Text,

                        optionNameZh2 = worksheet.Cells[row, 4].Text,
                        optionValue2 = worksheet.Cells[row, 5].Text,
                        PageType2 = worksheet.Cells[row, 6].Text,

                        optionNameZh3 = worksheet.Cells[row, 7].Text,
                        optionValue3 = worksheet.Cells[row, 8].Text,
                        PageType3 = worksheet.Cells[row, 9].Text,

                        optionNameZh4 = worksheet.Cells[row, 10].Text,
                        optionValue4 = worksheet.Cells[row, 11].Text,
                        PageType4 = worksheet.Cells[row, 12].Text,

                        optionNameZh = worksheet.Cells[row, 13].Text,
                        optionValueRange = worksheet.Cells[row, 14].Text,
                    };

                    people.Add(pp);
                }
            }

            return people;
        }
    }

    public class ExcelEntity
    {
        public string optionNameZh1 { get; set; }
        public string optionValue1 { get; set; }
        public string PageType1 { get; set; }

        public string optionNameZh2 { get; set; }
        public string optionValue2 { get; set; }
        public string PageType2 { get; set; }

        public string optionNameZh3 { get; set; }
        public string optionValue3 { get; set; }
        public string PageType3 { get; set; }

        public string optionNameZh4 { get; set; }
        public string optionValue4 { get; set; }
        public string PageType4 { get; set; }

        public string optionNameZh { get; set; }
        public string optionValueRange { get; set; }
    }
}