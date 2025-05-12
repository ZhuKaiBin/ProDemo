namespace FileStream2Sln
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filePath = "../../../../example.txt";

            // 打开文件进行写入
            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                // 锁定文件的第二部分 (从字节 10 开始，锁定 10 个字节)
                fs.Lock(10, 10);

                Console.WriteLine("进程 2 锁定文件，并开始写入...");
                Thread.Sleep(5000);
                // 写入文件
                byte[] buffer = new byte[10] { 75, 76, 77, 78, 79, 80, 81, 82, 83, 84 }; // ASCII 字符
                fs.Write(buffer, 0, buffer.Length);

                Console.WriteLine("进程 2 写入完成。");

                // 解锁文件
                fs.Unlock(10, 10);
                Console.WriteLine("进程 2 解锁文件。");
            }
        }
    }
}
