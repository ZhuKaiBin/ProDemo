using System.Text.RegularExpressions;

namespace EnergiDemo
{
    public enum SourceType
    {
        A,
        B
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            string input = "朱";

            SourceType result;
            if (Enum.TryParse(input, true, out result))
            {
                if (Enum.IsDefined(typeof(SourceType), result))
                {
                    Console.WriteLine($"{input} 是 SourceType 枚举的成员之一。");
                }
                else
                {
                    Console.WriteLine($"{input} 不是 SourceType 枚举的有效成员。");
                }
            }
            else
            {
                Console.WriteLine($"{input} 不是有效的枚举成员值。");
            }

            var value = "啊是#";

            bool b = new Regex("[@#$*|/()\\[\\]{}'\"\\\\_&#]").IsMatch(value);

            var sd = "";
            //前加强粱 qianJiaQiangLiang = new 前加强粱()
            //{
            //    Type = "前加强粱",
            //    PartCode = "1CN00087",
            //    Name = "前加强粱A",
            //    Depth = 800
            //};


            //后下门板 后下门板 = new 后下门板()
            //{
            //    PartCode = "1CN00158+3*1CN10011+3*1CN10001+2*1CN10021",
            //    Name = "后下门板B",
            //    Height = 2200,
            //    Width = 1200,
            //    防护等级 = "IP54",
            //    通风类型 = "自然通风",
            //    通风格栅数量 = "2",
            //    后门类型 = "左",
            //    开门方向 = "中间开"
            //};

            B bbbb = new B();
        }
    }

    public abstract class A
    {
        public string name { set; get; }
    }

    public class B : A { }

    public abstract class Component
    {
        public string Type { get; set; }
        public string PartCode { get; set; }
        public string Name { get; set; }
    }

    // 定义一个抽象类，表示具有尺寸属性的配电柜部件
    public abstract class SizedComponent : Component
    {
        public virtual double Height { get; set; }
        public virtual double Width { get; set; }
        public virtual double Depth { get; set; }
    }

    //骨架
    public class 骨架 : SizedComponent { }

    //眉头
    public class 眉头 : SizedComponent { }

    //前加强粱
    public class 前加强粱 : SizedComponent { }

    //后加强粱
    public class 后加强粱 : SizedComponent { }

    public class 后双开门中立柱 : SizedComponent
    {
        public bool 是否单开门 { set; get; }
    }

    public class 绑电缆横梁 : SizedComponent
    {
        public string 进出线方式 { set; get; }
    }

    public class 后下门板 : SizedComponent
    {
        public string 防护等级 { set; get; }
        public string 通风类型 { set; get; }
        public string 通风格栅数量 { set; get; }
        public string 后门类型 { set; get; }
        public string 开门方向 { set; get; }
    }
}
