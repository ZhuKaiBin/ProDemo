using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        {
            string[] A = { "apple", "banana", "cherry" };
            int[] B = { 1, 2, 3 };

            List<string> C = A.SelectMany(a => B, (a, b) => a + b.ToString()).ToList();

            foreach (string c in C)
            {
                Console.WriteLine(c);
            }
        }
        {
            int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            // 将数字按照奇偶性进行分组
            var groups = numbers.ToLookup(n => n % 2 == 0 ? "Even" : "Odd");

            // 输出分组结果
            foreach (var group in groups)
            {
                Console.WriteLine("{0} numbers: {1}", group.Key, string.Join(",", group));
            }
        }

        Button button = new Button();
        button.Clicked += OnButtonClicked; // 注册事件处理程序
        button.Click(); // 执行单击操作
    }

    static void OnButtonClicked(object sender, EventArgs e)
    {
        Console.WriteLine("按钮被单击了！");
    }
}

class Button
{
    public event EventHandler Clicked; // 定义事件  EventHandler是有两个参数，但是没有返回值的一个委托

    public void Click()
    {
        // 模拟按钮单击操作
        Console.WriteLine("执行单击操作！");
        // 触发事件
        Clicked?.Invoke(this, EventArgs.Empty);
    }
}
