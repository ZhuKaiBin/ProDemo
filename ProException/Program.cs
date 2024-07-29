using System;

namespace ProException
{
    class Program
    {
        static void Main(string[] args)
        {
            Cat cat = new Cat();
            People p = new People();
            cat.CatCallEvent += new CatCallEventHandler(p.WakeUp);

            ////现在调用猫叫这个方法
            ////这个方法里又调用了这个事件
            ////事件又触发了委托
            cat.OnCatCall("猫叫");

            Console.ReadKey();
        }
    }

    // 声明事件，首先必须声明该事件的委托类型
    public delegate string CatCallEventHandler(string anme);

    public class Cat
    {
        //事件要在类里面声明
        // 基于上面的委托定义事件
        //定义猫叫事件=========>用到委托的地方，就是要声明,不声明就会报错
        public event CatCallEventHandler CatCallEvent;

        public void OnCatCall(string msg)
        {
            CatCallEvent.Invoke(msg);
        }
        //该事件在生成的时候会调用委托
        //事件是委托的实现，就是说，事件是依附于委托
    }

    public class People
    {
        //定义主人醒来方法
        public string WakeUp(string name)
        {
            return name + "WakeUp";
        }
    }
}
