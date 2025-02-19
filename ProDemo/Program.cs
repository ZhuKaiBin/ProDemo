using System;

namespace ProDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            DerivedClassA aa = new DerivedClassA();

            //直接调用基类中的定义的方法
            aa.abstrExecute();
        }
    }

    public abstract class BaseAbstract
    {
        public void abstrExecute()
        {
            Before();
            Excute();
            After();
        }

        //每个子类必须要重写的自己的逻辑
        public abstract void Excute();

        public virtual void Before()
        {
            Console.WriteLine("BaseAbstract中的Before");
        }

        public virtual void After()
        {
            Console.WriteLine("BaseAbstract中的After");
        }
    }

    public class DerivedClassA : BaseAbstract
    {
        public override void Before()
        {
            Console.WriteLine("DerivedClassA中的Before");
        }

        public override void Excute()
        {
            Console.WriteLine("DerivedClassA中的Excute");
        }
    }
}
