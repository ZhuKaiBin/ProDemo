using System;
using System.Linq.Expressions;

namespace ProDemo4
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Static的知识
            //Console.WriteLine("i={0}", i);
            //Console.WriteLine("j={0}", new Program().j);
            //Console.Read();

            //Console.Write(Bob.bob_num);
            //Bob b2 = new Bob();
            //Console.Write(Bob.bob_num);//结果为2，首先，类被加载，所有的静态成员被创建在静态存储区，i=0,接着调用了类的成员，这时候静态构造函数就会被调用，i=2
            //Bob b = new Bob();
            //Console.Write(Bob.bob_num);

            #endregion
            #region 索引
            //IndexClass indexClass = new IndexClass();
            //Random random = new Random();

            //for (int i = 0; i < 5; i++)
            //{
            //    indexClass[i] = random.Next(1, 12);
            //}

            //for (int i = 0; i < 5; i++)
            //{
            //    Console.WriteLine($"输出的元素,{indexClass[i]}");
            //}
            //Console.ReadKey();
            #endregion
            #region Expression

            LambdaFun("BeiJing 2013", s =>
            {
                if (s.Contains("2013"))
                {
                    s = s.Replace("2013", "2014");
                }
                return s;
            });

            static void LambdaFun(string str, Func<string, string> func)
            {
                Console.WriteLine(func(str));
            }
            Console.ReadKey();


            //#region 无参数
            Func<string> func = () => { return "委托"; };
            func();
            Console.WriteLine(func());

            Func<string> func2 = delegate
              {
                  return "委托";
              };

            func2();
            //这里func2=func  ，func2 是func编译的结果，接住SLPY反编译看下
            #endregion
            #region 有一个参数 y
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
            #endregion
            #region 下面是动态拼接表达式树的芝士
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


            //这里面有几个点
            //1：ConstantExpression  声明静态值
            //2：ParameterExpression 声明参数值
            //3：BinaryExpression    声明表达式
            //4：Expression 可以点出来各种运算符,Add/ Multiply/ Subtract  加减乘除
            //5：ParameterExpression 含参数的表达式
            //5：Expression.Lambda<>()最后的大合集 
            //7：Compile() 编译； Invoke() 调用,援引
            //https://www.bilibili.com/video/BV16v4y1f7Fe?p=2


            #endregion
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



        static int i = getNum();
        int j = getNum();

        static int num = 1;

        static int getNum()
        {
            return num;
        }

        public  class Bob
        {
            //如果一个类 既有静态构造函数 又有非静态构造函数
            //那么程序先执行静态构造函数,再执行非静态构造函数

            public static int bob_num = 666;
            static Bob()
            {
                bob_num = 888;
                Console.Write("静态构造函数被执行");
            }


            public Bob()
            {
                bob_num = 10000;
                Console.Write("实例构造方法被调用");
            }
                
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
    }
}
