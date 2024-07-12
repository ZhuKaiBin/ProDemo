using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Text;

namespace BenchmarkDotNetDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<MemoryBenchmarkerDemo>();
        }
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
        //  - `ConcatStringsUsingStringBuilder` 平均耗时 5.180 毫秒。
        //  - `ConcatStringsUsingGenericList` 平均耗时 9.510 毫秒。

        //- **Error（误差）**：99.9% 置信区间的一半，表示结果的精确程度。
        //  - `ConcatStringsUsingStringBuilder` 的误差是 0.0918 毫秒。
        //  - `ConcatStringsUsingGenericList` 的误差是 0.1858 毫秒。

        //- **StdDev（标准差）**：测量值的标准差，表示结果的离散程度。
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