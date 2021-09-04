#define Debug
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ProDemo3
{
    class Program
    {
        static private List<int> _numArray; //用来保存1-100 这100个整数

        Program() //构造函数。我们可以通过这个构造函数往待测试集合中存入1-100这100个测试数据
        {
            _numArray = new List<int>(); //给集合变量开始在堆内存上开内存，并且把内存首地址交给这个_numArray变量

            for (int i = 1; i <= 100; i++)
            {
                _numArray.Add(i);  //把1到100保存在集合当中方便操作
            }
        }


        //一个返回类型为IEnumerable<int>，其中包含三个yield return
        public static IEnumerable<int> enumerableFuc()
        {
            yield return 1;
            yield return 2;
            yield return 3;
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

        public static void Test1(out int v)
        {
            v = 100;
        }
        public static void Test2(ref int v)
        {
        }

        static void Main(string[] args)
        {


            Dictionary<int, int> dic = new Dictionary<int, int>();
            dic.Add(1,5);
            dic.Add(2,9);
            //dic.Add(1, 6);

            Hashtable hashtable = new Hashtable();
            hashtable.Add(1, 5);
            hashtable.Add(2, 9);
            hashtable.Add(3, 6);

            ArrayList arrayList = new ArrayList(hashtable.Keys);

            arrayList.Sort();

            int[] numm =  { 1, 1, 2, 2, 5, 4, 6 };

            HashSet<int> set = new HashSet<int>(numm);
            int s = set.ToArray().Length;
            
            int i, j = 100;
            //Test1(out i);
            //Test2(ref j);

            //Class1 x = new Class1();
            //x.testParams(0);
            //x.testParams(0, 1);
            //x.testParams(0, 1, 2);

            int[] num1 = { 2 };
            int[] num2 = { 0 };
            List<int> lis = new List<int>();
            lis.AddRange(num1);
            lis.AddRange(num2);
            int[] num3 = lis.ToArray();
            Array.Sort(num3);//变成数组，从小到大排序
            double med;
            int len = num3.Length;//元素个数
            if (len % 2 == 0)
            {
                int a = num3[len / 2];
                int b = num3[len / 2 - 1];
                med = (num3[len / 2] + num3[len / 2 - 1]) / 2d;
                Console.WriteLine(med);
            }
            else
            {
                var ss = len / 2;
                med = num3[len / 2];
                Console.WriteLine(med);
            }
            Console.ReadKey();


            //Thread M = new Thread(delegate ()//线程M
            //{

            //    for (int i = 0; i <= 10000000; i++)
            //    {
            //        if (i % 100 == 0)
            //        {
            //            Console.WriteLine("M");////输出结果M
            //        }
            //    }
            //});

            //Thread S = new Thread(delegate () {  //线程N

            //    for (int i = 0; i <= 50000000; i++)
            //    {
            //        if (i % 100 == 0)
            //        {
            //            Console.WriteLine("S");//输出S
            //        }
            //    }

            //    M.Join();//在这里插入M

            //    for (int i = 0; i <= 50000000; i++)
            //    {
            //        if (i % 100 == 0)
            //        {
            //            Console.WriteLine("A");//输出A
            //        }
            //    }



            //});



            //int[] array = new int[] { 1, 2, 3, 4 };
            //var num = array.GetEnumerator();


            //List<int> list = new List<int> { 1, 2, 3, 4 };
            //var snum = list.GetEnumerator();

            //ICollection<Object> vs = new List<Object> { 1, 2, "3", 4, 5, 6 };
            //var Enumable = vs.AsEnumerable();
            //var query = vs.AsQueryable();
            //var query2 = vs.AsQueryable<object>();
            //foreach (var item in query2)
            //{
            //    string c = item.ToString();
            //}



            //Hashtable ht = new Hashtable();
            //ht.Add(1, "A");
            //ht.Add(2, "B");
            //ht.Add(3, "c");


            //foreach (DictionaryEntry item in ht)
            //{
            //    var str = item.Key;
            //}



            //byte i = 255;//检查是否溢出
            //checked
            //{
            //    i++;//System.OverflowException:“Arithmetic operation resulted in an overflow.”
            //}

            //unchecked //不检查是否溢出
            //{
            //    int var = (int)2147486978 * 2;
            //}

            //string firststring = "First String";
            //string secondstring = "Second string";

            //Console.WriteLine(Comparer<string>.Default.Compare(obj1, obj2));

            //Console.WriteLine(firststring.CompareTo(secondstring));



            #region  yield return
            //new Program();
            //TestMethod();
            //Console.WriteLine("Hello World!");
            #endregion

            // 通过foreach循环迭代此函数
            //foreach (int item in enumerableFuc())
            //{
            //    Console.WriteLine(item);
            //}

            Console.ReadKey();
        }

        //测试求1到100之间的全部偶数
        static public void TestMethod()
        {
            foreach (var item in GetAllEvenNumber())
            {
                Console.WriteLine(item); //输出偶数测试
            }
        }

        //测试我们使用Yield Return情况下拿到全部偶数的方法
        static IEnumerable<int> GetAllEvenNumber()
        {

            foreach (int num in _numArray)
            {
                if (num % 2 == 0) //判断是不是偶数
                {
                    yield return num; //返回当前偶数

                }
            }
            yield break;  //当前集合已经遍历完毕，我们就跳出当前函数，其实你不加也可以
            //这个作用就是提前结束当前函数，就是说这个函数运行完毕了。
        }
    }
}
