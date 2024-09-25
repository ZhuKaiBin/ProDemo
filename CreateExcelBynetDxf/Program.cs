using netDxf;
using netDxf.Entities;


namespace CreateExcelBynetDxf
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 获取运行时目录
            string runtimeDirectory = AppDomain.CurrentDomain.BaseDirectory;
            // 获取项目文件所在的目录（向上导航到项目目录）
            string projectDirectory = Directory.GetParent(runtimeDirectory).Parent.Parent.Parent.FullName;

            //新文件的命名
            string combineDxfName = $"{Guid.NewGuid().ToString()}.dxf";
            string desFilePath = Path.Combine(projectDirectory, "Files", "DxfFilesResult", combineDxfName);



            DxfDocument dxf = new DxfDocument();

            // 表格内容
            string[,] tableContent = {
            { "0123456789abcdefghijk", "Header2", "Header3" },
            { "Row1Col1", "Row1Col2", "Row1Col3" },
            { "Row2Col1", "Row2Col2", "Row2Col3" },
            { "Row3Col1", "Row3Col2", "Row3Col3" }
        };

            double startX = 0;
            double startY = 0;
            double cellWidth = 15;
            double cellHeight = 10;


            int rowCount = tableContent.GetLength(0); // 获取行数
            int colCount = tableContent.GetLength(1); // 获取列数
                                                      // 创建文本实体和边框
            for (int row = 0; row < tableContent.GetLength(0); row++)
            {
                double y = startY - row * cellHeight; // 固定行的 Y 坐标

                for (int col = 0; col < tableContent.GetLength(1); col++)
                {
                    double x = startX + col * cellWidth;

                    // 处理文本换行，最大长度设为 10
                    string[] lines = WrapText(tableContent[row, col], 6);

                    // 添加文本行
                    for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
                    {

                        var line = lines.Length;//总共是几行
                        double yyy = cellHeight / line;


                        double textY = y - (cellHeight / 2) + (lineIndex * 1.5); // 确保文本在单元格中居中
                        Text text = new Text(lines[lines.Length - 1 - lineIndex], new Vector2(x + 1, textY), 1.0); // 设置字体大小为 2.0
                        dxf.Entities.Add(text);
                    }

                    // 添加单元格边框
                    AddCellBorder(dxf, x, y, cellWidth, cellHeight);
                }
            }

            // 保存 DXF 文件
            string dxfFilePath = "output_with_borders.dxf";
            dxf.Save(desFilePath);
            Console.WriteLine($"DXF file created at: {desFilePath}");

        }


        static string[] WrapText(string text, int maxLength)
        {
            var lines = new System.Collections.Generic.List<string>();

            // 处理字符串换行
            for (int i = 0; i < text.Length; i += maxLength)
            {
                string line = text.Substring(i, Math.Min(maxLength, text.Length - i));
                lines.Add(line);
            }

            return lines.ToArray();
        }



        static void AddCellBorder(DxfDocument dxf, double x, double y, double width, double height)
        {
            // 绘制矩形边框
            dxf.Entities.Add(new Line(new Vector2(x, y), new Vector2(x + width, y))); // 上边
            dxf.Entities.Add(new Line(new Vector2(x + width, y), new Vector2(x + width, y - height))); // 右边
            dxf.Entities.Add(new Line(new Vector2(x + width, y - height), new Vector2(x, y - height))); // 下边
            dxf.Entities.Add(new Line(new Vector2(x, y - height), new Vector2(x, y))); // 左边
        }
    }
}
