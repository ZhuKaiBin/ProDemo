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
        static void Main(string[] args)
        {
            #region Thread
            //Thread M = new Thread(delegate ()//线程M
            //{

            //    for (int i = 0; i <= 100; i++)
            //    {
            //        if (i % 100 == 0)
            //        {
            //            Console.WriteLine("M");////输出结果M
            //        }
            //    }
            //});

            //Thread S = new Thread(delegate ()
            //{  //线程N

            //    for (int i = 0; i <= 500; i++)
            //    {
            //        if (i % 100 == 0)
            //        {
            //            Console.WriteLine("S");//输出S
            //        }
            //    }

            //    M.Join();//在这里插入M

            //    for (int i = 0; i <= 500; i++)
            //    {
            //        if (i % 100 == 0)
            //        {
            //            Console.WriteLine("A");//输出A
            //        }
            //    }

            //});
            #endregion


            #region  yield return


            Console.WriteLine("Hello World!");
            #endregion

            Console.ReadKey();
        }
    }
}
