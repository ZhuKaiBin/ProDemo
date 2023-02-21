using System;

namespace ProAbstract_Virtual_Interfance
{
    class Program
    {
        static void Main(string[] args)
        {

        }

        public interface IPerson
        {
            void eat();
            void Walk();            
        }

        public  interface IMusic
        {
            public string Chainese();
            public string America();
        }


        public class Asia : IPerson,IMusic
        {
            public void eat()
            {
                Console.WriteLine("人要吃饭");
            }

            public void Walk()
            {
                Console.WriteLine("人要走路");
            }

            public string Chainese()
            {
                return "中国人";
            }

            public string America()
            {
                return "美国人";
            }


        }


        public abstract class Animal_Abstract
        {
           public abstract void start();

            public void Start2()
            {
                Console.WriteLine("我是抽象类中的非抽象方法");
            }
        }
        public class Dog : Animal_Abstract
        {
            public override void start()
            {
                Console.WriteLine("继承抽象类,那么抽象类里的抽象方法必须重新\n");
            }
        }

        public class Car
        {
            public virtual string Start()
            {
                return "车辆启动";
            }
            public string Wheel()
            {
                return "车轮子";
            }
        }
        public class Bench : Car
        {
            public string BenchCar()
            {
                return "我是奔驰车";
            }
            public override string Start()
            {
                return "奔驰车启动....wuwuwuwu~";
            }
        }
    }
}
