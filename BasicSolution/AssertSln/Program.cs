using System.Diagnostics;

namespace AssertSln
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int x = 5;
            int y = 000;

            // 使用 Assert 断言 x + y 是否等于 15

            //在这个例子中，Debug.Assert(x + y == 15) 断言 x + y 等于 15。
            //如果条件不成立，程序会抛出异常，提示开发者错误信息 "x + y should be 15"。

            //不成立就会显示后面的信息
            //所以，C# 中的断言的 “肯定”，是指在运行时 验证条件是否为真，并且如果为假则引发异常。
            Debug.Assert(x + y == 15, "x + y should be 15");


            //断言里的东西是判断是一定是为真的，否则就报错：
            //Debug.Assert(screw.Length == 5);
            //这个意思是你**“断定”这个条件必须成立：长度必须是5。**
            //如果某颗螺丝不等于5cm，你就报警！停线检查！



            //Debug.Assert(user != null);
            //意思是：我断定这时候 user 一定已经初始化好了，如果不是，就说明我的逻辑写错了，赶紧崩掉提醒我！




            Console.WriteLine("Program continues executing...");
        }
    }
}
