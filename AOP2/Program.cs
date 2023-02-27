using System;
using System.Collections;
using System.Collections.Generic;

namespace AOP2
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> list = new List<int>();
            MyList myList = new MyList(list);
            myList.Add(10);

            foreach (var item in myList)
            {
               
            }

            Console.WriteLine("Hello World!");
        }
    }

    public class MyList : IList<int>
    {

        private IList<int> target;
        public MyList(IList<int> _target)
        {
            target = _target;
        }

       
    }
}
