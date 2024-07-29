using System;
using System.Threading;
using System.Threading.Tasks;

public class Example
{
    // 创建一个最大计数为3的信号量
    private static Semaphore _semaphore = new Semaphore(2, 5);

    public static void Main()
    {
        {
            MyData myData = new MyData() { Value = 10086, Version = 1 };

            int newValue = 6666;

            var b = UpdateData(myData, newValue);
        }

        {
            int newVersion = 1;
            int oldVersion = 2;
            int reftarget = 3;
            //如果reftarget==oldVersion的值，说明没有被其他线程修改过，返回结果是oldVersion(reftarget)，无所谓反正都一样
            //如果reftarget!=oldVersion的值，说明被其他线程修改过，reftarget带回来的值，是被其他线程修改的版本号，返回结果是reftarget
            var compareRet = Interlocked.CompareExchange(ref reftarget, newVersion, oldVersion);
            if (compareRet == oldVersion) { }
            else
            {
                // 操作失败，target的值未被修改
            }
        }
        // 创建10个线程并启动它们
        for (int i = 1; i <= 5; i++)
        {
            Thread t = new Thread(Enter);
            t.Name = i.ToString();
            t.Start();
        }
    }

    //定义一个包含版本号的数据结构
    public class MyData
    {
        public int Version; //版本号！！！
        public int Value; //需要更新的数据项
    }

    //执行更新的方法
    public static bool UpdateData(MyData data, int newValue)
    {
        int currentVersion = data.Version; //获取版本号,其实这个data.VerSon最重要，要是其他的线程读取了，然后这个版本号就一定不一样;

        int newVersion = currentVersion + 1; //版本号加1，新版本号

        //尝试对Version进行CAS操作，如果成功，则说明未被其他线程更新过，可以更新数据
        //其实关键在于【ref data.Version】  ref的用法,是进入到系统逻辑中，然后再出来，
        //如果再出来的值等于原来的值，那证明数据没有被更改过；那么就是执行成功，我们就可以给版本号执行+1 操作
        //如果出来数据被更改了，那么就不等于原来的currentVersion，直接执行失败

        if (
            Interlocked.CompareExchange(ref data.Version, newVersion, currentVersion)
            == currentVersion
        )
        {
            data.Value = newValue;
            data.Version = newVersion;
            return true;
        }
        else
        {
            //如果CAS操作失败，则说明其他线程已经修改了Version，需要进行重试
            return false;
        }
    }

    private static void Enter()
    {
        Console.WriteLine("Thread {0} 开始执行...", Thread.CurrentThread.Name);

        // 请求信号量，如果信号量已满，则等待
        _semaphore.WaitOne();

        // 访问共享资源
        Console.WriteLine("Thread {0} 成功进入临界区...", Thread.CurrentThread.Name);
        Thread.Sleep(1000); // 模拟访问共享资源的过程
        Console.WriteLine("Thread {0} 退出临界区...", Thread.CurrentThread.Name);

        // 释放信号量
        _semaphore.Release();
    }
}
