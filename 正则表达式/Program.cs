using System.Text.RegularExpressions;

namespace 正则表达式
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string input1 = "1000(C100+F700+B200)";
            // 调用方法判断 C 和 F 的相对位置
            CheckCPositionRelativeToF(input1);
        }



        static void CheckCPositionRelativeToF(string input)
        {
            // 正则表达式匹配 C 和 F
            string patternC = @"C\d+";
            string patternF = @"F\d+";

            Match matchC = Regex.Match(input, patternC);
            Match matchF = Regex.Match(input, patternF);

            if (matchC.Success && matchF.Success)
            {
                // 比较 C 和 F 在字符串中的索引位置
                int positionC = matchC.Index;
                int positionF = matchF.Index;

                if (positionC < positionF)
                {
                    Console.WriteLine("C 在 F 的左侧");
                }
                else if (positionC > positionF)
                {
                    Console.WriteLine("C 在 F 的右侧");
                }
            }
            else
            {
                Console.WriteLine("未找到 C 或 F 的值");
            }
        }
    }
}
