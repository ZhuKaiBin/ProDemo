using System;
using System.Collections.Generic;

namespace 计算行高
{
    public class Row
    {
        public int RowNumber { get; set; }
        public int RowHeight { get; set; } // 行高
        public List<string> Values { get; set; } = new List<string>(); // 存储该行的所有字符串值
    }

    class Program
    {
        static void Main()
        {
            var numbers = new List<string>
             {
                 "24", "24", "24", "5", "6/2", "6/2", "6/2", "6/4", "6/4", "6/4", "6/4",
                 "8",  "5/3", "5/3", "6/2"
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
                    if (result.Count == 0 || result[^1][0] != current)
                    {
                        result.Add(new List<string> { current });
                    }
                    i++;
                }
                else
                {
                    // 处理带 "/" 的字符串
                    var parts = current.Split('/');
                    string baseNumber = parts[0];
                    int limit = int.Parse(parts[1]);

                    // 计算连续相同的字符串数量
                    int count = 0;
                    while (i + count < numbers.Count && numbers[i + count] == current)
                    {
                        count++;
                    }

                    // 按规则进行行分割
                    while (count > 0)
                    {
                        int take = Math.Min(count, limit);
                        var line = new List<string>();
                        for (int j = 0; j < take; j++)
                        {
                            line.Add(current);
                        }
                        result.Add(line);
                        count -= take;
                    }
                    i += count;
                }
            }

            return result;
        }
    }
}
