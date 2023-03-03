using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ProList
{
    class Program
    {
        static void Main(string[] args)
        {
            ListBenchmark ListBenchmark = new ListBenchmark();
            ListBenchmark.Setup();

            ListBenchmark.List_Foreach();

        }
    }


    public class ListBenchmark
    {
        private List<int> _list = default!;

        // 分别测试10、1千、1万、10万及100万数据时的表现
        //[Params(10, 1000, 1_0000, 10_0000, 100_0000)]
        public int Size { get; set; }

        //[GlobalSetup]
        public void Setup()
        {
            // 提前创建好数组
            _list = new List<int>(5000);
            for (var i = 0; i < Size; i++)
            {
                _list.Add(i);
            }
        }

        //public void Foreach_Span()
        //{
        //    foreach (var item in CollectionsMarshal.AsSpan(_list))
        //    {
        //    }
        //}

        public void List_Foreach()
        {
            _list.ForEach(x => { Console.WriteLine(x); });
        }


    }
}
