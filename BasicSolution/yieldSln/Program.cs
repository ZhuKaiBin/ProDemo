namespace yieldSln
{
    /*
     yield 关键字用法
     */


    internal class Program
    {
        static void Main(string[] args)
        {
            var nums = GetNumers(10);

            foreach (var numer in nums)
            {
                Console.WriteLine($"number的值是{numer}");
                if (numer == 3)
                {
                    break;
                }
            }


            await foreach (var item in GetIntsAsync(10))
            {
                Console.WriteLine($"异步number的值是{item}");
                if (item == 3)
                {
                    break;
                }
            }
        }


        static IEnumerable<int> GetNumers(int n)
        {
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine($"yield的值是{i}");
                yield return i;
            }
        }


        static async IAsyncEnumerable<int> GetIntsAsync(int n)
        {
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine($"yield的值是{i}");
                yield return i;
            }
        }
    }
}
