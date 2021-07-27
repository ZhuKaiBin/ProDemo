using System;
using System.Collections.Generic;
using System.Xml;

namespace ProDemo2
{
    class Program
    {
        public delegate void CatcallEventHandler();
        public delegate void mouserunEventHandler();

        //猫这个类里声明一个事件 (事件是需要委托的)
        //事件的触发是需要Invoke
        public class Cat
        {
            //事件要依靠 委托  事件在类中声明且生成
            public event CatcallEventHandler catcall;
            public void OncatCall()
            {
                Console.WriteLine("猫叫了一声");
                catcall.Invoke();//这个Invoke是事件的调用
            }
        }

        public class Mouse
        {
            public event mouserunEventHandler mousecall;
            public void MouseRun()
            {
                Console.WriteLine("老鼠跑了");
                mousecall.Invoke();
            }
        }

        public class People
        {
            //定义主人醒来方法
            public void WakeUp()
            {
                Console.WriteLine("主人醒了");
            }
        }

        static void Main(string[] args)
        {

            //Cat cat = new Cat();
            //Mouse m = new Mouse();
            //People p = new People();

            ////catcall 是事件,是猫的这个事件引发下一个事件
            //cat.catcall += new CatcallEventHandler(m.MouseRun);
            ////因为在这里 触发了m.MouseRun  而且m.MouseRun又触发了 mousecall.Invoke(); 然后又触发了下面的p.wake
            ////这就是为什么老鼠跑了,主人醒了


            //m.mousecall += new mouserunEventHandler(p.WakeUp);
            //cat.OncatCall();
            //m.MouseRun();

            //TestA a = new TestA();
            //TestB b = new TestB(a);
            //5.事件的触发：在代码中就是a.Do("a", "b");这一句。通过代码发现，a.Do("a", "b")实际上是调用了对象b中的OnGetStr方法，
            //所谓的事件“订阅”相当于是把符合委托“模板”的方法OnGetStr传递过去，最终执行的是OnGetStr方法。这样的语言描述可能不准确，得多体会才行。
            //a.Do("a", "b");//事件的触发

            //XmlDocument xd = new XmlDocument();
            //xd.LoadXml("<Person><name>诸葛亮</name></Person>");
            //Console.WriteLine(xd.DocumentElement.InnerXml);


            Class1 x = new Class1();
            x.testParams(0);
            x.testParams(0, 1);
            x.testParams(0, 1, 2);

            Console.ReadKey();
        }

        class Class1
        {
            public void testParams(params int[] arr)
            {
                Console.Write("使用Params参数！");
            }
            public void testParams(int x, int y)
            {
                Console.Write("使用两个整型参数！");
            }

        }



        //1.首先定义一个委托类型：void GetStrHandler(string x, string y)，为事件的声明作准备。
        public delegate void GetStrHander(string x, string y);
        //2.定义事件拥有者的类，即class TestA，事件拥有者是这个类的实例对象即TestA a = new TestA();中定义的a。
        //因为事件GetStr,是在TestA这个类中定义的，也就是事件的拥有者。
        public class TestA
        {
            //事件类型与委托一起  GetStr最终还是一个事件,闪电符号
            //声明事件
            public event GetStrHander GetStr;
            public void Do(string x, string y)
            {
                GetStr(x, y);
            }
        }

        //3..定义事件订阅者的类，即class TestB ，事件拥订阅者是这个类的实例对象即TestB b = new TestB(a);中定义的b，
        //仔细看看TestB的构造方法，发现它依赖于对象a，这是因为订阅者需要监听事件拥有的事件(这句的"监听"也就是当TestA有啥动静,这边及时作出反应)，
        //因此需要把事件拥有者“引”进来，使订阅者能实现事件订阅
        public class TestB
        {
            private TestA _A;
            public TestB(TestA a)
            {
                _A = a;
                _A.GetStr += OnGetStr;//事件的订阅
                _A.GetStr += onGetStr2;//后面再追加一个订阅,也就是多播委托
            }
            //4.定义事件处理方法：在三要素中已强调过，事件处理方法是在订阅者中定义的，表示事件触发时需要执行的方法。
            //这里就是执行了“Console.WriteLine(string.Format("{0}+{1}", x, y));”这个动作。
            private void OnGetStr(string x, string y)
            {
                Console.WriteLine(string.Format("{0}+{1}", x, y));
            }

            public void onGetStr2(string x, string y)
            {
                Console.WriteLine(string.Format("{0}-{1}", x, y));
            }
        }
    }
}
