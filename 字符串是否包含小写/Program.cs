using System.Collections.Concurrent;

namespace 字符串是否包含小写
{
    internal class Program
    {
        private static ConcurrentBag<string> _saveSyncGraphicsMsgBag = new ConcurrentBag<string>();
        static void Main(string[] args)
        {
            string test1 = "HELLO WORLD123";
            string test2 = "Hello World111";
            string test3 = "";

            _saveSyncGraphicsMsgBag.Add("test1");
            _saveSyncGraphicsMsgBag.Add("test1");
            _saveSyncGraphicsMsgBag.Add("test2");
            Console.WriteLine(test1.HasLowercase());
            Console.WriteLine(test2.HasLowercase());
            Console.WriteLine(test3.HasLowercase());

            _saveSyncGraphicsMsgBag.Clear();
        }
    }

    public static class StringExtensions
    {
        /// <summary>
        /// 检查字符串是否不包含小写字母。
        /// </summary>
        /// <param name="input">要检查的字符串。</param>
        /// <returns>如果字符串中没有小写字母，则返回 true；否则返回 false。</returns>
        public static bool HasLowercase(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            return input.Any(char.IsLower);
        }
    }
}
