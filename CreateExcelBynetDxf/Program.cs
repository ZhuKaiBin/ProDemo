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
            string combineDxfName = "Combine333.dxf";
            string desFilePath = Path.Combine(projectDirectory, "Files", "DxfFilesResult", combineDxfName);



            DxfDocument dxf = new DxfDocument();

            // 表格内容
            string[,] tableContent = {
            { "Header1", "Header2", "Header3" },
            { "Row1Col1", "Row1Col2", "Row1Col3" },
            { "Row2Col1", "Row2Col2", "Row2Col3" },
            { "Row3Col1", "Row3Col2", "Row3Col3" }
        };

            double startX = 0;
            double startY = 0;
            double cellWidth = 10;
            double cellHeight = 5;

            // 创建文本实体
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    double x = startX + col * cellWidth;
                    double y = startY - row * cellHeight;

                    // 添加文本实体，调整 Y 坐标以放置在单元格内
                    double textY = y - (cellHeight / 2); // 在单元格中居中显示
                    Text text = new Text(tableContent[row, col], new Vector2(x + 1, textY), 1.0);
                    dxf.Entities.Add(text);

                    // 添加单元格边框
                    AddCellBorder(dxf, x, y, cellWidth, cellHeight);
                }
            }

            // 保存 DXF 文件
            string dxfFilePath = "output.dxf";
            dxf.Save(desFilePath);
            Console.WriteLine($"DXF file created at: {dxfFilePath}");

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
