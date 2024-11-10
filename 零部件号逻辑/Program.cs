namespace 零部件号逻辑
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            List<TestClass> testClasses = new List<TestClass>()
        {
            new TestClass(){MaterialCode="DX001", SubMaterialCode="DX0011" },
            new TestClass(){MaterialCode="DX0011", SubMaterialCode="DX00111" },
            new TestClass(){MaterialCode="DX00111", SubMaterialCode="DX001111" },


            new TestClass(){MaterialCode="DX002", SubMaterialCode="DX0022" },
            new TestClass(){MaterialCode="DX002", SubMaterialCode="DX00222" },
            new TestClass(){MaterialCode="DX0022", SubMaterialCode="DX00223" }
        };



            var result = GetMaterialHierarchy(testClasses);

            foreach (var item in result)
            {
                Console.WriteLine($"Material Code: {item.MaterialCode}, Level: {item.Level}");
            }



        }


        public static List<MaterialHierarchy> GetMaterialHierarchy(List<TestClass> testClasses)
        {
            // 校验：零部件号和子零部件号不应相等
            foreach (var item in testClasses)
            {
                if (item.MaterialCode == item.SubMaterialCode)
                    throw new Exception("MaterialCode and SubMaterialCode cannot be the same.");
            }

            // 记录每个零部件的层级
            var hierarchyMap = new Dictionary<string, string>();

            // 按照零部件的顺序从上到下进行处理
            foreach (var item in testClasses)
            {
                // 从当前零部件开始递归查找
                SetMaterialLevel(item.MaterialCode, item.SubMaterialCode, "1", testClasses, hierarchyMap);
            }

            // 返回计算的层级
            return hierarchyMap.Select(kvp => new MaterialHierarchy
            {
                MaterialCode = kvp.Key,
                Level = kvp.Value
            }).ToList();
        }

        private static void SetMaterialLevel(string materialCode, string subMaterialCode, string level, List<TestClass> testClasses, Dictionary<string, string> hierarchyMap)
        {
            // 记录当前零部件的层级
            if (!hierarchyMap.ContainsKey(materialCode))
            {
                hierarchyMap[materialCode] = level;
            }

            // 查找当前零部件的所有子零部件
            var children = testClasses.Where(x => x.MaterialCode == materialCode).ToList();

            // 遍历子零部件并递归设置层级
            int childIndex = 1;
            foreach (var child in children)
            {
                string childLevel = $"{level}.{childIndex}";
                SetMaterialLevel(child.MaterialCode, child.SubMaterialCode, childLevel, testClasses, hierarchyMap);
                childIndex++;
            }
        }

    }

    public class TestClass
    {
        public string MaterialCode { set; get; }

        public string SubMaterialCode { set; get; }
    }

    public class MaterialHierarchy
    {
        public string MaterialCode { get; set; }
        public string Level { get; set; }
    }
}