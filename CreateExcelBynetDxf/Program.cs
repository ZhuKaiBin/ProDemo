using netDxf;
using netDxf.Entities;
using netDxf.Tables;


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
            { "相似的字体", "保你的项保你的项", "Header3" },
            { "Row1Col1", "Row1Col2", "目的运行目录中" },
            { "Row2Col1", "0123456789abcdefghijk", "Row2Col3" },
            { "0123456789abcdefghijk", "Row3Col2", "Row3Col3" }
        };

            double startX = 0;
            double startY = 0;
            double defaultCellWidth = 15;//默认单元格宽度
            double defaultCellHeight = 10;//默认单元格高度

            int rowCount = tableContent.GetLength(0); // 获取行数
            int colCount = tableContent.GetLength(1); // 获取列数

            // 创建文本样式
            TextStyle textStyle = new TextStyle("微软雅黑 Light", FontStyle.Regular);
            var frontHeight = 1.5;//字体的宽度
            var frontWidthFactor = 0.8;//字体默认宽度是1;设置文本的宽度因子（宽度比例）,控制字体的宽度缩放

            // 创建文本实体和边框
            for (int row = 0; row < rowCount; row++)
            {
                double y = startY - row * defaultCellHeight; // 固定行的 Y 坐标

                for (int col = 0; col < colCount; col++)
                {
                    double x = startX + col * defaultCellWidth;
                    // 处理文本换行，最大长度设为 10,10个长度就换行
                    string[] lines = WrapText(tableContent[row, col], 10);

                    // 添加文本行
                    for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
                    {
                        double textY = y - (defaultCellHeight / 2) + (lineIndex * frontHeight); // 确保文本在单元格中居中

                        var zuoBiaoX = x + 1;//x的左边距离
                        var zuoBiaoY = lines.Length > 1 ? textY - frontHeight : textY;

                        Text text = new Text(lines[lines.Length - 1 - lineIndex],
                                             new Vector2(zuoBiaoX, zuoBiaoY),
                                             frontHeight)
                        {
                            Style = textStyle
                        };
                        text.WidthFactor = frontWidthFactor;


                        dxf.Entities.Add(text);
                    }
                    // 添加单元格边框
                    AddCellBorder(dxf, x, y, defaultCellWidth, defaultCellHeight);
                }
            }

            // 保存 DXF 文件
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
