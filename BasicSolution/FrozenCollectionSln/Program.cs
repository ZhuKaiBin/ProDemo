using System.Collections.Immutable;

namespace FrozenCollectionSln
{
    internal class Program
    {

        //.NET 8 中引入了 FrozenCollection 使得只读 Collection 的操作性能更加好了,
        //Stephen 在.NET 8 的性能改进博客中也有提到，在【只读的场景】可以考虑使用 FrozenSet/FrozenDictionary 来提升性能

        static void Main(string[] args)
        {
            //// 创建并初始化普通的字典
            //var mutableDict = new Dictionary<int, string>
            //                 {
            //                     { 1, "one" },
            //                     { 2, "two" },
            //                     { 3, "three" }
            //                 };

            //// 将普通字典转换为冻结字典
            //var frozenDict = mutableDict.ToFrozenDictionary();

            //// 读取操作（高效且线程安全）
            //if (frozenDict.TryGetValue(2, out var value))
            //{
            //    Console.WriteLine(value);  // 输出: two
            //}

            // 下面的代码将无法编译，因为 FrozenDictionary 不允许修改
            // frozenDict[4] = "four"; // 编译错误

            // 在多线程环境下安全读取
            //Parallel.For(0, 10, i =>
            //{
            //    Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId},{frozenDict[1]}");  // 输出: one
            //});



            var list = new List<int> { 1, 2, 3 };
            var froZenSet = list.ToImmutableHashSet();

            list.Clear();

            Parallel.For(0, 10, i =>
            {
                Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId},{froZenSet.FirstOrDefault().ToString()}");  // 输出: 
            });



        }
    }
}
