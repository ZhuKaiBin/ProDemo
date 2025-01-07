using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace BenchmarkDotNetDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            /*
            代码启动的时候，要改成Release模式，不然会报错；
            
            / Validating benchmarks:
            //    * Assembly BenchmarkDotNetDemo which defines benchmarks is non-optimized
                  Benchmark was built without optimization enabled (most probably a DEBUG configuration). Please, build it in RELEASE.
                  If you want to debug the benchmarks, please see https://benchmarkdotnet.org/articles/guides/troubleshooting.html#debugging-benchmarks.



            醒你当前的基准测试项目（BenchmarkDotNetDemo）没有启用优化。具体来说，它是说：
            未启用优化：警告中提到的“Benchmark was built without optimization enabled”表示，你当前的基准测试项目可能是以 DEBUG 配置编译的，
            而在 DEBUG 配置下，编译器不会对代码进行优化，从而影响基准测试的准确性和效率。

             */


            var summary = BenchmarkRunner.Run<MemoryBenchmarkerDemo>();

            //var summary = BenchmarkRunner.Run<AnyDemo>();
        }
    }

    public class AnyDemo
    {
        private int NumberOfItems = 50;

        [Benchmark]
        public void gg()
        {
            List<B> bs = new List<B>();
            for (int i = 0; i < NumberOfItems; i++)
            {
                bs.Add(new B() { Name = "zhu", Age = i });
            }

            if (bs.Any())
            {
                Console.WriteLine("gg");
            }
        }

        [Benchmark]
        public void GG()
        {
            List<B> bs = new List<B>();
            for (int i = 0; i < NumberOfItems; i++)
            {
                bs.Add(new B() { Name = "zhu", Age = i });
            }

            if (bs.Count > 0)
            {
                Console.WriteLine("GG");
            }
        }

        [Benchmark]
        public void GGgg()
        {
            List<B> bs = new List<B>();
            for (int i = 0; i < NumberOfItems; i++)
            {
                bs.Add(new B() { Name = "zhu", Age = i });
            }

            if (bs.Count() > 0)
            {
                Console.WriteLine("GGgg");
            }
        }
    }

    public class B
    {
        public string Name { set; get; }
        public int Age { set; get; }
    }

    public class MemoryBenchmarkerDemo
    {
        private int NumberOfItems = 1000;

        [Benchmark]
        public string ConcatStringsUsingStringBuilder()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < NumberOfItems; i++)
            {
                sb.Append("Hello World!" + i);
            }
            return sb.ToString();
        }

        [Benchmark]
        public string ConcatStringsUsingGenericList()
        {
            var list = new List<string>(NumberOfItems);
            for (int i = 0; i < NumberOfItems; i++)
            {
                list.Add("Hello World!" + i);
            }
            return list.ToString();
        }

        #region 内容解释

        //这段输出是一个性能基准测试（benchmarking）的结果，主要是对两个方法进行性能比较。以下是对每个部分的解释：

        //### 基准测试方法
        //- `ConcatStringsUsingStringBuilder` 和 `ConcatStringsUsingGenericList` 是两个被测试的方法。

        //### 结果
        //- **Mean（平均值）**：每个方法执行一次的平均时间。
        //【误差值越小，表示测量结果的精确程度越高】
        //  - `ConcatStringsUsingStringBuilder` 平均耗时 5.180 毫秒。
        //  - `ConcatStringsUsingGenericList` 平均耗时 9.510 毫秒。

        //- **Error（误差）**：99.9% 置信区间的一半，表示结果的精确程度
        //【误差值越小，表示测量结果的精确程度越高】。
        //  - `ConcatStringsUsingStringBuilder` 的误差是 0.0918 毫秒。
        //  - `ConcatStringsUsingGenericList` 的误差是 0.1858 毫秒。

        //- **StdDev（标准差）**：测量值的标准差，表示结果的离散程度。
        // 【标准差表示测量值的离散程度，标准差越小，说明测量结果越集中，波动越小。因此，标准差越小越好，表示测量结果的一致性越高】
        //  - `ConcatStringsUsingStringBuilder` 的标准差是 0.1791 毫秒。
        //  - `ConcatStringsUsingGenericList` 的标准差是 0.3255 毫秒。

        //### 警告
        //- **Environment（环境）**
        //  - `Summary -> Benchmark was executed with attached debugger`：基准测试是在附加了调试器的情况下执行的，这可能会影响测试结果的准确性。

        //### 提示
        //- **Outliers（离群值）**
        //  - `MemoryBenchmarkerDemo.ConcatStringsUsingStringBuilder: Default -> 4 outliers were removed(5.68 ms..6.20 ms)`：在 `ConcatStringsUsingStringBuilder` 方法的测试中，有4个离群值被移除了，这些值在 5.68 毫秒到 6.20 毫秒之间。

        //### 图例
        //- **Mean**：所有测量值的算术平均值。
        //- **Error**：99.9% 置信区间的一半。
        //- **StdDev**：所有测量值的标准差。
        //- **1 ms**：1 毫秒（0.001 秒）。

        //### 运行时间
        //- **BenchmarkRunner: End**
        //  - 运行时间：00:01:10（70.74 秒），执行了2个基准测试。

        //- **Global total time**
        //  - 全局总时间：00:01:30（90.24 秒），执行了2个基准测试。

        //### Artifacts cleanup
        //- **Artifacts cleanup is finished**
        //  - 基准测试产生的临时文件和数据已经清理完毕。

        //总结来说，这份报告展示了两个方法的性能比较，`ConcatStringsUsingStringBuilder` 方法明显比 `ConcatStringsUsingGenericList` 方法要快，平均耗时更少。报告还指出基准测试是在调试器附加的情况下进行的，并且移除了一些离群值以提高结果的准确性。

        #endregion 内容解释
    }
}
