namespace 继承抽象
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var list = functionUnitBase();




            Console.WriteLine("Hello, World!");
        }




        public static List<FunctionUnitBase> functionUnitBase()
        {

            List<FunctionUnitBase> k = new List<FunctionUnitBase>();
            if (1 > 0)
            {
                InComingType inComingType = new InComingType();
                inComingType.Id = 1;
                inComingType.Name = "11";
                k.Add(inComingType);
            }

            if (4 > 3)
            {
                TieSldType tieSldType = new TieSldType();
                tieSldType.Id = 2;
                tieSldType.Name = "22";
                tieSldType.TieLowerTerminalPosition = "lower";
                k.Add(tieSldType);
            }
            return k;
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
