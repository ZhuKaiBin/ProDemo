namespace 控制行高2
{
    internal class Program
    {
        static void Main()
        {
            var numbers = new List<string>
        {
            "24", "24", "24", "5", "6/2", "6/2", "6/2", "6/4", "6/4", "6/4", "6/4",
            "8", "8/3", "8/3", "9", "5/3", "5/3", "5/3", "5/3", "6/2"
        };

            var result = ProcessNumbers(numbers);
            foreach (var line in result)
            {
                Console.WriteLine(string.Join(", ", line));
            }
        }

        static List<List<string>> ProcessNumbers(List<string> numbers)
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
