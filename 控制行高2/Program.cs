namespace 控制行高2
{
    internal class Program
    {
        private static void Main()
        {
            var numbers = new List<string>
             {
                 "6/2", "24", "24", "5", "6/2", "6/2", "6/2","6/2","6/2", "6/4", "6/4", "6/4", "6/4",
                 "8", "8/3", "8/3","8/3","8/3","5/3","8/3", "9", "5/3", "5/3", "5/3","5/3","5/3", "5/3", "6/3", "6/2", "6/2","23","23"
             };

            var result = ProcessNumbers(numbers);

            foreach (var line in result)
            {
                Console.WriteLine(string.Join(", ", line));
            }

            //CheckFullRows(result);

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

        public static void CheckFullRows(List<List<string>> result)
        {
            for (int i = 0; i < result.Count; i++)
            {
                var row = result[i];
                var groupedCounts = row
                    .Where(s => s.Contains("/")) // 只处理带"/"的字符串
                    .GroupBy(s => s) // 按相同的字符串分组
                    .ToDictionary(g => g.Key, g => g.Count());

                bool rowIsFull = true;

                foreach (var item in groupedCounts)
                {
                    var parts = item.Key.Split('/');
                    if (parts.Length == 2 && int.TryParse(parts[1], out int requiredCount))
                    {
                        int actualCount = item.Value;

                        if (actualCount < requiredCount)
                        {
                            Console.WriteLine($"第 {i + 1} 行缺少 {requiredCount - actualCount} 个 \"{item.Key}\"");
                            rowIsFull = false;
                        }
                    }
                }

                if (rowIsFull)
                {
                    Console.WriteLine($"第 {i + 1} 行已满");
                }
            }
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