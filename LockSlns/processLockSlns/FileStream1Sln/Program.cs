using System;
using System.IO;
using System.Threading;

class Program
{
    // 文件路径
    static string filePath = "example.txt";

    static void Main(string[] args)
    {
        // 创建两个线程来并发写入文件
        Thread thread1 = new Thread(WriteFile1);
        Thread thread2 = new Thread(WriteFile2);

        // 启动线程
        thread1.Start();
        thread2.Start();

        // 等待线程完成
        thread1.Join();
        thread2.Join();

        Console.WriteLine("所有操作完成。");
    }

    // 线程 1：写入文件的第 0 到 9 字节
    static void WriteFile1()
    {
        using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
        {
            // 锁定文件的第一部分 (从字节 0 开始，锁定 10 个字节)
            fs.Lock(0, 10);

            Console.WriteLine("进程 1 锁定文件，并开始写入...");
            // 模拟一些操作
            Thread.Sleep(2000);  // 等待 2 秒钟，模拟长时间操作
            byte[] buffer1 = new byte[10] { 65, 66, 67, 68, 69, 70, 71, 72, 73, 74 }; // 字符 A到J
            fs.Write(buffer1, 0, buffer1.Length);

            Console.WriteLine("进程 1 写入完成。");

            // 解锁文件
            fs.Unlock(0, 10);
            Console.WriteLine("进程 1 解锁文件。");
        }
    }

    // 线程 2：写入文件的第 10 到 19 字节
    static void WriteFile2()
    {
        using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
        {
            // 锁定文件的第二部分 (从字节 10 开始，锁定 10 个字节)
            fs.Lock(10, 10);

            Console.WriteLine("进程 2 锁定文件，并开始写入...");
            // 模拟一些操作
            Thread.Sleep(1000);  // 等待 1 秒钟，模拟稍短的操作
            byte[] buffer2 = new byte[10] { 75, 76, 77, 78, 79, 80, 81, 82, 83, 84 }; // 字符 K到T
            fs.Write(buffer2, 0, buffer2.Length);

            Console.WriteLine("进程 2 写入完成。");

            // 解锁文件
            fs.Unlock(10, 10);
            Console.WriteLine("进程 2 解锁文件。");
        }
    }
}
