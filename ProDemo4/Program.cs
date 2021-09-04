using System;
using System.Linq.Expressions;

namespace ProDemo4
{
    class Program
    {
        static void Main(string[] args)
        {

            //int[] ary = new { 4, 5, 6, 4 };
            //MyClass.UseParams(1, 2, 3);
            //MyClass.UseParams(ary);
            string interval = "";


            IndexClass indexClass = new IndexClass();
            Random random = new Random();

            for (int i = 0; i < 5; i++)
            {
                indexClass[i] = random.Next(1, 12);
            }

            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine($"输出的元素,{indexClass[i]}");
            }
            Console.ReadKey();
            #region Expression

            //#region 无参数
            //Func<string> func = () => { return "委托"; };
            //func();
            //Console.WriteLine(func());

            //Func<string> func2 = delegate
            //  {
            //      return "委托";
            //  };

            //func2();
            ////这里func2=func  ，func2 是func编译的结果，接住SLPY反编译看下
            //#endregion
            //#region 有一个参数 y
            //Expression<Func<string>> func_expre = () => "委托Expression";
            //////func_expre();直接这样,这个表达式是报错的，说明是不是委托
            //Console.WriteLine(func_expre.Compile());//但是编译后返回的是一个Func委托,   System.Func`1[System.String]
            //Console.WriteLine(func_expre.Compile().Invoke());

            //Expression<Action<string>> expression = (name) => Console.WriteLine(name);
            ////Compile：编译,编译文件
            //expression.Compile().Invoke("bob");

            //Expression<Func<string, string>> eAdd = (y) => y + "委托";
            //eAdd.Compile().Invoke("bobAdd");
            //Console.WriteLine(eAdd.Compile().Invoke("bobAdd"));
            //#endregion
            //#region 下面是动态拼接表达式树的芝士
            //Console.WriteLine("********下面是动态拼接表达式树的芝士**********************************");
            ////动态拼接表达式树
            //Expression<Func<int>> funcExp = () => 5;
            //Console.WriteLine($"我是简单的{funcExp.Compile().Invoke()}");
            ////Constant：常量表达式,值不变
            //ConstantExpression constantExp = Expression.Constant(5, typeof(int));
            //Expression<Func<int>> funcExp2 = Expression.Lambda<Func<int>>(constantExp, null);
            //funcExp2.Compile().Invoke();
            //Console.WriteLine($"我是动态编出来的{funcExp2.Compile().Invoke()}");

            //Console.WriteLine("*********下面是带参数的动态拼接的****************");
            //Expression<Func<int, int>> fcunExppara = (y) => y + 5;
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


            ////这里面有几个点
            ////1：ConstantExpression  声明静态值
            ////2：ParameterExpression 声明参数值
            ////3：BinaryExpression    声明表达式
            ////4：Expression 可以点出来各种运算符,Add/ Multiply/ Subtract  加减乘除
            ////5：ParameterExpression 含参数的表达式
            ////5：Expression.Lambda<>()最后的大合集 
            ////7：Compile() 编译； Invoke() 调用,援引
            ////https://www.bilibili.com/video/BV16v4y1f7Fe?p=2


            //#endregion
            #endregion 

        }


        public interface ISOmeInterface
        {
            int this[int index] { set; get; }
        }


        public class IndexClass : ISOmeInterface
        {

            private int[] ary = new int[5];
            public int this[int index]
            {
                get { return ary[index]; }
                set
                {
                    if (value > 5)
                        ary[index] = 0;
                    else
                    {
                        ary[index] = value;
                    }
                }
            }
        }

        #region
        public class MyClass
        {
            public static void UseParams(params int[] list)
            {
                for (int i = 0; i < list.Length; i++)
                {
                    Console.WriteLine(list[i]);
                }
            }


            public static void UseParams2(params Object[] objects)
            {
                for (int i = 0; i < objects.Length; i++)
                {
                    Console.WriteLine(objects[i]);
                }
            }
        }
        #endregion




    }
}
