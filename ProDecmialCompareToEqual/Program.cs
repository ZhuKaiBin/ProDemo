namespace ProDecmialCompareToEqual
{
    public class Program
    {
        /*
         什么时候使用？
        使用 Equals 当你只关心 是否相等 时。
        使用 CompareTo 当你需要根据 大小顺序 排列或比较时。
         
         */
        private static void Main(string[] args)
        {
            double d1 = 1.0;
            double d2 = 1.00d;

            var equalResult = d1.Equals(d2);//// true，因为 1.0 和 1.00 代表的是相等的值

            var compareResult = d1.CompareTo(d2);

        }

        /*
         Equals 方法用于检查两个值是否完全相等。如果两个值相等，则返回 true；否则返回 false。它的重点是 是否相等。

        比喻：
        想象你有两张完全一样的 考试试卷，每张试卷上都是满分 100 分。你想比较这两张试卷是不是一样：        
        Equals 就是一个 "比对试卷" 的工具，查看两张试卷上是不是一模一样。只要满分数字一样，它就认为这两张试卷是一样的，即 相等。
         */

        /*
         
         CompareTo 方法用于比较两个值的大小，它返回一个整数值，表示 大小关系：
         A.CompareTo(B)   大于0代表A大 

           返回 0：表示两个值相等。
           返回正数：表示调用者的值大于参数值。
           返回负数：表示调用者的值小于参数值。
           比喻：
           继续用 考试试卷 做比喻，假设你想知道两个人的 考试成绩 谁更高：
           
           CompareTo 就是一个 "排序工具"，它比较的是两个试卷上的得分。
           如果两个试卷得分相同，它会说 “平手”。
           如果一个试卷得分更高，它会说 “这个更高”。
         */
    }
}
