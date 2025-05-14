using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommondLib
{
    public class Class3
    {
        //
        static Class3()
        {
            Console.WriteLine("只执行一次，且仅在首次访问该类时执行。\n 静态构造函数先执行；");
            Console.WriteLine("静态构造函数只会在类第一次“被用到”时执行一次，并且是在公共构造函数之前执行。");
        }
        public Class3() { }
        private Class3(string a) { }
        public Class3(int a) { }


        public string A { get; set; }

        // 不公开的属性，一般不会这样写
        private string B { get; set; }

        public string C;
        protected string D;
        internal string E;
        private string G;







        public void Test(string str, ref string a, out string b)
        {
            b = "666";
            Console.WriteLine(b);
        }
    }
}
