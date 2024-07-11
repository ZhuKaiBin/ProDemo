using Microsoft.VisualBasic;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace ProDemo4
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            #region 有一个参数 y

            Expression<Func<string>> func_expre = () => "委托Expression";
            //////func_expre();直接这样,这个表达式是报错的，说明是不是委托
            //Console.WriteLine(func_expre.Compile());//但是编译后返回的是一个Func委托,   System.Func`1[System.String]
            //Console.WriteLine(func_expre.Compile().Invoke());

            Expression<Action<string>> expression = (name) => Console.WriteLine(name);
            ////Compile：编译,编译文件
            //expression.Compile().Invoke("bob");

            Expression<Func<string, string>> eAdd = (y) => y + "委托";
            eAdd.Compile().Invoke("bobAdd");
            Console.WriteLine(eAdd.Compile().Invoke("bobAdd"));

            #endregion 有一个参数 y

            #region 下面是动态拼接表达式树的芝士

            //Console.WriteLine("********下面是动态拼接表达式树的芝士**********************************");
            ////动态拼接表达式树
            Expression<Func<int>> funcExp = () => 5;
            //Console.WriteLine($"我是简单的{funcExp.Compile().Invoke()}");
            ////Constant：常量表达式,值不变
            ConstantExpression constantExp = Expression.Constant(5, typeof(int));
            //Expression<Func<int>> funcExp2 = Expression.Lambda<Func<int>>(constantExp, null);
            //funcExp2.Compile().Invoke();
            //Console.WriteLine($"我是动态编出来的{funcExp2.Compile().Invoke()}");

            //Console.WriteLine("*********下面是带参数的动态拼接的****************");
            Expression<Func<int, int>> fcunExppara = (y) => y + 5;
            //Console.WriteLine($"我是简单的{fcunExppara.Compile().Invoke(6)}");
            ////ParameterExpression 参数表达式
            //ParameterExpression para1 = Expression.Parameter(typeof(int), "y");//左边
            ////静态表达式
            //ConstantExpression constant1 = Expression.Constant(5, typeof(int));//右边
            //BinaryExpression binaryExp1 = Expression.Add(para1, constant1);

            //Expression<Func<int, int>> ExpressionAdd1 = Expression.Lambda<Func<int, int>>(binaryExp1, new ParameterExpression[] { para1 });
            //ExpressionAdd1.Compile().Invoke(6);

            //Console.WriteLine($"我是动态编出来的,带参数的{ExpressionAdd1.Compile().Invoke(6)}");

            //Console.WriteLine("下面是加减乘除的*********************************");
            //Expression<Func<int, int, int>> funcpara2 = (y, x) => 5 + y * y - x;
            //funcpara2.Compile().Invoke(5, 6);
            //Console.WriteLine($"我是动态编出来的{funcpara2.Compile().Invoke(5, 6)}");
            ////先说y*y  是个乘法的表达式
            ////先声明一个int的Y
            //ParameterExpression Ypara = Expression.Parameter(typeof(int), "y");//左边
            ////先声明一个Y * Y  是一个表达式相乘，就要用到BinaryExpression
            //BinaryExpression binaryExp2 = Expression.Multiply(Ypara, Ypara);//Y*Y
            ////再声明一个5 + y * y 的表达式
            ////先声明一个常量5的表达式
            //ConstantExpression constantExps5 = Expression.Constant(5, typeof(int));
            //BinaryExpression binaryExp5YY = Expression.Add(constantExps5, binaryExp2);//(5+Y*Y)
            ////最后就是声明那个(5 + y * y) - x
            ////先声明一个int的x
            //ParameterExpression Xpara = Expression.Parameter(typeof(int), "x");
            ////然后就是一个综合的表达式
            //BinaryExpression BinaryEnd = Expression.Subtract(binaryExp5YY, Xpara);
            ////最后的大汇总
            //Expression<Func<int, int, int>> expEnd = Expression.Lambda<Func<int, int, int>>(BinaryEnd, new ParameterExpression[] { Ypara, Xpara });
            //expEnd.Compile().Invoke(5, 6);
            //Console.WriteLine($"我是动态编出来的{expEnd.Compile().Invoke(5, 6)}");

            //这里面有几个点
            //1：ConstantExpression  声明静态值
            //2：ParameterExpression 声明参数值
            //3：BinaryExpression    声明表达式
            //4：Expression 可以点出来各种运算符,Add/ Multiply/ Subtract  加减乘除
            //5：ParameterExpression 含参数的表达式
            //5：Expression.Lambda<>()最后的大合集
            //7：Compile() 编译； Invoke() 调用,援引
            //https://www.bilibili.com/video/BV16v4y1f7Fe?p=2

            #endregion 下面是动态拼接表达式树的芝士

            {
                boy XiaoMing = new boy();
                XiaoMing.speak();                    //调用抽象类man,里面的《非抽象方法》
                XiaoMing.work();                      //调用派生类boy,里面的 《重写方法》

                XiaoMing.sleep();                     //调用抽象类man里，新加的方法。

                WoMan woMan = new WoMan();

                Type type = typeof(WoMan);
                Type[] inheritInterfaces = type.GetInterfaces();

                var sd = typeof(Iperson);
                var sdsdsd = sd.GetInterfaces();

                bool implementsIperson = inheritInterfaces.Contains(typeof(Iperson));

                var method1 = typeof(Iperson).GetType().IsInstanceOfType(woMan);
                if (method1)
                {
                }

                var method2 = woMan.GetType().GetInterface("Iperson");
                if (method2 != null)
                {
                }

                if (woMan is Iperson)
                {
                }

                Iperson say = woMan as Iperson;
            }
        }

        public class Bob2
        {
            public string name;
            public int id;
        }

        public static void LambdaFun(string str, Func<string, string> func)
        {
            Console.WriteLine(func(str));
        }

        private static int i = getNum();
        private int j = getNum();

        private static int num = 1;

        private static int getNum()
        {
            return num;
        }

        public class Bob
        {
            //如果一个类 既有静态构造函数 又有非静态构造函数
            //那么程序先执行静态构造函数,再执行非静态构造函数

            public static int bob_num = 666;

            static Bob()
            {
                bob_num = 888;
                Console.Write($"静态构造函数{bob_num}被执行\n");
            }

            public Bob()
            {
                bob_num = 10000;
                Console.Write($"实例构造方法{bob_num}被调用\n");
            }
        }

        public interface Iperson
        {
            void speak();

            void eat();

            void work();
        }

        public abstract class Man : Iperson
        {
            public void speak()
            {
                Console.WriteLine("111");
            }

            public void eat()
            { }

            public virtual void work()
            {
                Console.WriteLine("");
            }

            public void sleep()
            { Console.WriteLine("不管大人小孩子都需要睡觉"); }  //抽象类中额外加的方法，睡觉。
        }

        public class boy : Man
        {
            public override void work()
            {
                Console.WriteLine("小孩子不需要工作");
            }
        }

        public class WoMan : Iperson, Iper
        {
            public void speak()
            {
                Console.WriteLine("111");
            }

            public void eat()
            {
            }

            public virtual void work()
            {
                Console.WriteLine("");
            }

            public void dd()
            { }
        }

        public interface Iper
        {
            void dd();
        }

        public class Womann : Iper
        {
            public void dd()
            { }
        }
    }
}