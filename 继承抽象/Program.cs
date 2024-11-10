namespace 继承抽象
{

    public class b
    {
        public string a;
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            //var list = functionUnitBase();
            //string testString = " HelloWorld";
            //bool d = ContainsSpaceWithLinq(testString);


            b b = new b();
            b = null;
            bool sd = "A".Equals(b?.a);

            if (b?.a == "A")
            {

            }


        }






        public static List<FunctionUnitBase> functionUnitBase()
        {

            List<FunctionUnitBase> k = new List<FunctionUnitBase>();

            InComingType inComingType = new InComingType();
            inComingType.Id = 1;
            inComingType.Name = "11";
            k.Add(inComingType);


            TieSldType tieSldType = new TieSldType();
            tieSldType.Id = 2;
            tieSldType.Name = "22";
            tieSldType.TieLowerTerminalPosition = "lower";
            k.Add(tieSldType);

            return k;
        }


        public static bool ContainsSpaceWithLinq(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            // 使用 LINQ 的 Any 方法检查是否有空格
            return input.Any(char.IsWhiteSpace);
        }



    }

    public abstract class FunctionUnitBase
    {
        public string? Type { get; set; }  // 只允许在类内部设置 Type
        public int Id { get; set; }
        public string? SortOrder { get; set; }
        public string? Name { get; set; }
        public string? SldSkuCode { get; set; }
        public string? SldCode { get; set; }

    }

    public class InComingType : FunctionUnitBase
    {

    }

    public class TieSldType : FunctionUnitBase
    {


        public string? TieLowerTerminalPosition { set; get; }
    }
}
