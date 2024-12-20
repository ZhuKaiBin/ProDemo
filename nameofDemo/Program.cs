namespace nameofDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                int param = 5;
                int result = RetryHelper.Retry(RiskyOperation, param, maxRetryCount: 5, delayMilliseconds: 2000);
                Console.WriteLine($"Operation succeeded. Result: {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Operation failed after retries: {ex.Message}");
            }




            Console.WriteLine("Hello, World!");
        }



        // 模拟一个带有参数并可能抛出异常的操作
        public static int RiskyOperation(int param)
        {
            Random rand = new Random();
            int value = rand.Next(1, 10);

            Console.WriteLine($"Attempting operation with parameter: {param}");

            if (value <= 5)
            {
                // 模拟操作失败，抛出异常
                Console.WriteLine("Operation failed.");
                throw new InvalidOperationException("Simulated operation failure.");
            }
            else
            {
                // 模拟操作成功
                Console.WriteLine("Operation succeeded.");
                return param * 2;  // 假设操作成功时，返回参数的两倍
            }
        }
    }



    public class RetryHelper
    {
        // 带入参和返回值的重试方法
        public static TResult Retry<TResult, TParam>(Func<TParam, TResult> action, TParam param, int maxRetryCount = 3, int delayMilliseconds = 1000)
        {
            int attempts = 0;
            while (true)
            {
                try
                {
                    attempts++;
                    // 执行带有参数的操作
                    return action(param);
                }
                catch (Exception ex)
                {
                    // 如果达到最大重试次数，抛出异常
                    if (attempts >= maxRetryCount)
                    {
                        Console.WriteLine($"Attempt {attempts} failed. Max retries reached.");
                        throw new InvalidOperationException("Maximum retry attempts reached.", ex);
                    }

                    // 如果没有达到最大重试次数，等待一段时间再重试
                    Console.WriteLine($"Attempt {attempts} failed. Retrying in {delayMilliseconds}ms...");
                    Thread.Sleep(delayMilliseconds);
                }
            }
        }
    }







    class Person
    {
        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));  // 使用 nameof 确保类型安全
                }
            }
        }

        public event Action<string> PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(propertyName);
        }
    }


}
