namespace DecoratorModel_Console
{

    //装饰器模式（Decorator Pattern）是一种结构型设计模式，目的是通过将对象嵌套在其他对象中来动态地为其添加额外的功能，
    //而无需修改原有对象的代码。它通过继承或组合的方式来扩展对象的功能。

    //装饰器模式通常包括以下角色：
    //组件接口（Component）：定义一个对象接口，可以是一个抽象类或接口，表示所有可以装饰的对象。
    //具体组件（ConcreteComponent）：实现了组件接口的具体对象，即需要装饰的原始对象。

    //装饰器（Decorator）：实现了组件接口，持有一个组件对象，功能是增强或修改原有组件的功能。
    //具体装饰器（ConcreteDecorator）：扩展了装饰器，实际添加功能的具体装饰器。

    internal class Program
    {
        static void Main(string[] args)
        {
            Decorator decoratorA = new DecoratorA(new Student("小明"));
            decoratorA.Operation();


            Decorator decoratorB = new DecoratorA(new Teacher("老师"));
            decoratorB.Operation();
        }
    }


    public abstract class Person
    {
        protected string name;
        public abstract void Operation();//职责
    }

    public class Student : Person
    {
        public Student(string name)
        {
            this.name = name;
        }
        public override void Operation()
        {
            Console.WriteLine("学生{0}的主要职责是学习！", name);
        }
    }

    public class Teacher : Person
    {
        public Teacher(string name)
        {
            this.name = name;
        }
        public override void Operation()
        {
            Console.WriteLine("老师{0}的主要职责是教书！", name);
        }
    }




    //这个装饰器接口之所以要集成Person，是为了给这一系列的接口都添加一些额外的功能
    public abstract class Decorator : Person
    {
        protected Person person;
        protected Decorator(Person person)
        {
            this.person = person;
        }

        public override void Operation()
        {
            if (person != null)
            {
                //确保Person中的接口可以正常调用，别用了装饰器，原本的工不能用了
                person.Operation();
            }
        }
    }


    public class DecoratorA : Decorator
    {

        public DecoratorA(Person person) : base(person)
        {
            this.person = person;
        }

        public override void Operation()
        {
            PreBehavior();
            person.Operation();//对象原本的方法
            AfterBehavior();

        }
        private void PreBehavior()
        {
            Console.WriteLine("PreBehavior的独有职责！");
        }

        private void AfterBehavior()
        {
            Console.WriteLine("AfterBehavior的独有职责！");
        }
    }


    public class DecoratorB : Decorator
    {
        public DecoratorB(Person person) : base(person)
        {
            this.person = person;
        }

        public override void Operation()
        {
            PreBehavior();
            person.Operation();//对象原本的方法
            AfterBehavior();

        }
        private void PreBehavior()
        {
            Console.WriteLine("PreBehavior的独有职责！");
        }

        private void AfterBehavior()
        {
            Console.WriteLine("AfterBehavior的独有职责！");
        }
    }




}
