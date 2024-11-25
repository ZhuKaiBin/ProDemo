namespace 控制行高2
{
    /// <summary>
    /// 背景:就是在添加单元的时候，要默认的把上面缺少的补齐,
    /// 比如，第一行是:6/2,第二行是24，第三行是6/2,那么第三行的6/2，要先把第一行的6/2补齐，再进行第三行；
    /// 
    /// 就是说，如果前面有与"自己"相同的，先查看前面相同的是是不是"够数",如果够数，那么就新行新增，如果不够数，那么先补充前面的
    /// 
    /// 
    /// 
    /// 排序问题:
    /// 就是顺序打乱后，再重新排序,按照【模数】从小到大排序
    /// </summary>
    internal class Program
    {
        private static void Main()
        {
            var numbers = new List<string>
             {
                 "6/2", "8/3","24", "24", "5", "6/2", "6/2", "6/2","6/2","6/2", "6/4", "6/4", "6/4", "6/4",
                 "8", "8/3", "8/3","8/3","8/3","5/3","8/3", "9", "5/3", "5/3", "5/3","5/3","5/3", "5/3", "6/3", "6/2", "6/2","23","23"
             };


            //这个是给所有的进行分组,
            var result = ProcessNumbers(numbers);

            foreach (var line in result)
            {
                Console.WriteLine(string.Join(", ", line));
            }



            //这个是检查每一行还剩下多少“空余的模数”
            var missingItems = CheckFullRows2(result);
            //(1, "6/2", 1)
            //(7, "6/2", 1)

            foreach (var item in missingItems)
            {
                Console.WriteLine($"({item.Item1}, \"{item.Item2}\", {item.Item3})");
            }
        }

        private static List<List<string>> ProcessNumbers(List<string> numbers)
        {
            var result = new List<List<string>>();
            int i = 0;

            while (i < numbers.Count)
            {
                string current = numbers[i];

                if (!current.Contains("/"))
                {
                    // 如果是整数或者当前数和前一个不一样，独占一行
                    result.Add(new List<string> { current });
                    i++;
                }
                else
                {
                    // 处理带 "/" 的字符串
                    var parts = current.Split('/');
                    int limit = int.Parse(parts[1]);
                    int count = 0;

                    // 计算连续相同的字符串数量
                    while (i < numbers.Count && numbers[i] == current)
                    {
                        count++;
                        i++;
                    }

                    // 按规则进行行分割
                    while (count > 0)
                    {
                        int take = Math.Min(count, limit);
                        result.Add(new List<string>(new string[take].Fill(current)));
                        count -= take;
                    }
                }
            }

            return result;
        }

        public static List<Tuple<int, string, int>> CheckFullRows2(List<List<string>> result)
        {
            var missingItems = new List<Tuple<int, string, int>>();

            for (int i = 0; i < result.Count; i++)
            {
                var row = result[i];
                var groupedCounts = row
                    .Where(s => s.Contains("/")) // 只处理带"/"的字符串
                    .GroupBy(s => s) // 按相同的字符串分组
                    .ToDictionary(g => g.Key, g => g.Count());

                foreach (var item in groupedCounts)
                {
                    var parts = item.Key.Split('/');
                    if (parts.Length == 2 && int.TryParse(parts[1], out int requiredCount))
                    {
                        int actualCount = item.Value;

                        if (actualCount < requiredCount)
                        {
                            missingItems.Add(Tuple.Create(i + 1, item.Key, requiredCount - actualCount));
                        }
                    }
                }
            }

            return missingItems;
        }
    }

    public static class StringExtensions
    {
        // 用于填充字符串数组
        public static string[] Fill(this string[] array, string value)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = value;
            }
            return array;
        }
    }
}