using System.Diagnostics;
using System.IO.Compression;

namespace base64UploadDown
{
    internal class Program
    {
        static void Main(string[] args)
        {
            {
                //string path = @"D:\正泰文档\成套软件\4月1号图纸\零部件图纸\20250101\8580DB2527EN.dxf";  // 改成你的实际路径
                //string base64 = ConvertDxfToBase64(path);
                //Console.WriteLine(base64);  // 打印或用在上传中

                //var size = GetBase64SizeInKB(base64);
                //string outputPath = "Pictures\\8580DB2527EN.dxf";
                //SaveBase64AsDxf(base64, outputPath);
                //Console.WriteLine("还原完成！");
            }

            #region 压缩算法
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                // 目标文件路径
                string filePath = @"D:\正泰文档\成套软件\4月1号图纸\零部件图纸\20250101\8580DB2527EN.dxf";  // 改成你的实际路径
                string compressedFilePath = "Pictures\\8580DB2527EN.dxf.gz";

                // 压缩文件
                CompressFile(filePath, compressedFilePath);

                // 获取压缩后的文件大小
                FileInfo compressedFileInfo = new FileInfo(compressedFilePath);
                Console.WriteLine($"Compressed file size: {compressedFileInfo.Length / 1024.0} KB");

                Console.WriteLine($"压缩耗时：{stopwatch.ElapsedMilliseconds} 毫秒");
              
            }
            #endregion

            //Compressed file size: 27.47265625 KB
            //压缩耗时：39 毫秒
            //Decompressed file size: 223.24609375 KB
            //解压缩耗时：2 毫秒


            #region 解压缩算法
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                // 目标文件路径
                string compressedFilePath = @"D:\ProjectDemo\ProDemoSlns\BasicSolution\base64UploadDown\bin\Debug\net8.0\Pictures\8580DB2527EN.dxf.gz";
                string decompressedFilePath = @"D:\ProjectDemo\ProDemoSlns\BasicSolution\base64UploadDown\bin\Debug\net8.0\Pictures\8580DB2527EN_decompressed.dxf";

                // 解压缩文件
                DecompressFile(compressedFilePath, decompressedFilePath);

                stopwatch.Stop();
                // 获取解压后的文件大小
                FileInfo decompressedFileInfo = new FileInfo(decompressedFilePath);
                Console.WriteLine($"Decompressed file size: {decompressedFileInfo.Length / 1024.0} KB");

                Console.WriteLine($"解压缩耗时：{stopwatch.ElapsedMilliseconds} 毫秒");
                Console.ReadKey();
            }

            #endregion
        }

        public static string ConvertDxfToBase64(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("文件不存在：", filePath);

            byte[] fileBytes = File.ReadAllBytes(filePath);
            string base64String = Convert.ToBase64String(fileBytes);
            return base64String;
        }

        public static void SaveBase64AsDxf(string base64String, string outputFileName)
        {
            if (string.IsNullOrWhiteSpace(base64String))
                throw new ArgumentException("Base64 字符串不能为空");

            // 获取当前项目的根目录
            string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // 构建目标文件的完整路径
            string outputFilePath = Path.Combine(projectDirectory, outputFileName);

            // 确保目录存在
            string folderPath = Path.GetDirectoryName(outputFilePath);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath); // 如果目录不存在，则创建
            }

            // 转换 Base64 为字节数组并保存为文件
            byte[] fileBytes = Convert.FromBase64String(base64String);
            File.WriteAllBytes(outputFilePath, fileBytes);
        }


        public static double GetBase64SizeInKB(string base64String)
        {
            if (string.IsNullOrEmpty(base64String))
                throw new ArgumentException("Base64 字符串不能为空");

            // 将 Base64 字符串转换为字节数组
            byte[] base64Bytes = Convert.FromBase64String(base64String);

            // 获取字节数组大小并转换为KB
            double sizeInKB = base64Bytes.Length / 1024.0;
            return sizeInKB;
        }



        // 压缩文件方法
        static void CompressFile(string sourceFilePath, string compressedFilePath)
        {
            using (FileStream sourceStream = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read))
            using (FileStream compressedStream = new FileStream(compressedFilePath, FileMode.Create, FileAccess.Write))
            using (GZipStream gzipStream = new GZipStream(compressedStream, CompressionMode.Compress))
            {
                sourceStream.CopyTo(gzipStream);
            }
        }

        // 解压缩文件方法
        static void DecompressFile(string compressedFilePath, string decompressedFilePath)
        {
            using (FileStream compressedStream = new FileStream(compressedFilePath, FileMode.Open, FileAccess.Read))
            using (FileStream decompressedStream = new FileStream(decompressedFilePath, FileMode.Create, FileAccess.Write))
            using (GZipStream gzipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
            {
                gzipStream.CopyTo(decompressedStream);
            }
        }



    }



}
