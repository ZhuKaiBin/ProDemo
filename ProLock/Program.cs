using System;
using System.Collections.Generic;
using System.Threading;

namespace ProLock
{

  
    class Program
    {
       
        //实现单例是要考虑并发问题的，一般情况下，我们都会使用synchronized来保证线程安全。
        static void Main(string[] args)
        {
            //调用
            //SingleModel.GetInstance().StartThread();
            Console.WriteLine("Hello World!");

            Action<string> action;

            action = ret;
            action += ret2;
            action += ret;
            action("bob1");

            //action = ret2;
            //action("bob2");
            Console.ReadKey();

        }




        public static void ret(string x)
        {
            Console.WriteLine(x);
        }

        public static void ret2(string x)
        {
            Console.WriteLine("ret2"+x);
        }

    }

    #region SingleModel_Class
    public class SingleModel
    {
        private static SingleModel _singlemodel;
        private SingleModel()//私有化构造函数,外界不可以通过new来实例化该类
        {
        
        }

        //声明一个方法,返回的类型是SingleModel
        public static SingleModel GetInstance()
        {
            if (_singlemodel == null)
            {
                _singlemodel = new SingleModel();
            }
            return _singlemodel;
        }

        public string name = "Bob";
        private int level = 6;
       
        //obj一定是私有的,静态的只读的
        private static readonly object obj = new object();      

        public Thread threadone;
        public Thread threadtwo;
        public Queue<int> _queue = new Queue<int>();

        public void StartThread()
        { 
         
        }

        public void SS()
        {
            //Lock关键字可以确保代码块完成运行,而不被其他线程中断 
            //应该避免锁定Public类型,否则被其他代码实例化,而超出代码的控制范围

            //lock锁定的必须是引用类型
            lock (_queue)//lock来锁定queue,一个先执行,再执行另外一个
            {
                _queue.Enqueue(5);
                Thread.Sleep(1000);
            }
        }
      
    }
    #endregion


    #region 在多线程中，需要确保一个实例。（我们可以使用线程锁lock来控制 ）
    //单例模式：确保一个类只有一个实例,并提供一个全局访问点。（定义）
    //概念拆解：
    //      (1)：确保一个类只有一个实例
    //      (2)：提供一个访问它的全局访问点
    //一个类不被New,在类的方法不被重复New,  在多线程调用实例时，确保只有一个实例在运行
    // 类似 一个国家只有一个国王
    public class Singleton
    {
        //定义一个静态变量来保存类的实例!!!!==>静态类保存类的实例
        private static Singleton uniqueInstance;
        //定义一个标识确保线程同步
        private static readonly object locker = new object();


        //定义私有的构造函数,那么Singleton类就不可以被外界NEW
        private Singleton()
        { 
        }

        //定义一个公有方法,提供一个"全局访问点",
        public static Singleton GetInstance()
        {
            //当第一个线程运行到这里的时候,此时会对locker对象"枷锁"
            //当第二个线程运行到该方法时,首先检测到locker对象为"枷锁"状态
            //该线程就会挂起等待第一个线程解锁

            if (uniqueInstance == null)//加上这个判断，就是双重锁定
            {

                //locker语句执行完后,会对该对象"解锁"
                lock (locker)
                {
                    if (uniqueInstance == null)
                    {
                        uniqueInstance = new Singleton();
                    }
                    return uniqueInstance;
                }
            }
            return uniqueInstance;
        }

        //然后我们可以声明一些其他的sql连接类的方法，通过GetGetInstance()调用

    }
    #endregion


}
