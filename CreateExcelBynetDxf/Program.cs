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

            // 测试表格内容
            string[,] tableContent = {
            { "abcdefghijklmn", "67", "890" },
            { "asdfgh11op1abchsdfdifh1iod111", "sdfghjkl", "xdftgbh" },
            { "zxcvb", "可以根据需要调整 lineSpacing 的值来获得理想的效果,", "abcdefghijklmn" },
            { "0123456789abcdefghijk", "12", "34" },
            { "122", "12", "您尝试访问的网站类型属于[访问网站/IP站点]已经被上网策略[Deny-All]拒绝访问" }
        };

            double startX = 0;
            double startY = 0;
            double defaultCellWidth = 15;//测试默认单元格宽度
            double defaultCellHeight = 10;//测试默认单元格高度

            int rowCount = tableContent.GetLength(0); // 获取行数
            int colCount = tableContent.GetLength(1); // 获取列数

            // 创建文本样式
            TextStyle textStyle = new TextStyle("微软雅黑 Light", FontStyle.Regular);
            var frontHeight = 1.5;//字体的宽度
            var frontWidthFactor = 0.8;//字体默认宽度是1;设置文本的宽度因子（宽度比例）,控制字体的宽度缩放
            double lineSpacing = 2.0; // 设置行间距//不设置的话,会导致换行的数据上下行太过紧凑，不美观
            int substringLength = 10;//设置文本截取长度为10


            // 创建文本实体和边框
            var realHeight = 0d;//如果存在换行，那么当前行高就变了，不再是defaultCellHeight           
            Dictionary<int, double> dicSaveY = new Dictionary<int, double>();//用来记录执行完毕后，当前y坐标(长度)在哪个位置
            for (int row = 0; row < rowCount; row++)
            {
                double y = 0d;// 固定行的 Y 坐标
                if (realHeight > 0)
                {
                    var currentYHeight = dicSaveY.Values.Sum();
                    y = startY - currentYHeight;
                }
                else
                {
                    y = startY - row * defaultCellHeight;
                }

                //先排查下当前行中是不是需要多行,多行的话,就是要改变默认的高度的，就要执行实际的高度
                int maxLines = 1; // 记录当前行的最大行数
                for (int col = 0; col < colCount; col++)
                {
                    int lineCount = WrapText(tableContent[row, col], substringLength).Length;
                    if (lineCount > maxLines)
                    {
                        maxLines = lineCount; // 更新最大行数
                    }
                }

                realHeight = maxLines * defaultCellHeight;


                for (int col = 0; col < colCount; col++)
                {
                    double x = startX + col * defaultCellWidth;
                    // 处理文本换行
                    string[] lines = WrapText(tableContent[row, col], substringLength);

                    // 添加文本行
                    for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
                    {
                        double textY = y - (realHeight / 2) + (lineIndex * (frontHeight + lineSpacing));
                        var zuoBiaoX = x + 1;//x的左边距离
                        var zuoBiaoY = lines.Length > 1 ? textY - frontHeight : textY;

                        var textContent = lines[lines.Length - 1 - lineIndex];
                        Text text = new Text(textContent,
                                             new Vector2(zuoBiaoX, zuoBiaoY),
                                             frontHeight)
                        {
                            Style = textStyle
                        };
                        text.WidthFactor = frontWidthFactor;

                        dxf.Entities.Add(text);
                    }

                    // 添加单元格边框
                    AddCellBorder(dxf, x, y, defaultCellWidth, realHeight);
                }

                dicSaveY.Add(row, realHeight);//存储当期下边框 Y的高度;如果是(0,0),高度是10，那么Y的坐标其实是(0,-10)
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
