using System.Runtime.ExceptionServices;

namespace ConsoleApp8
{
    class Program
    {
        static void Main()
        {
            // 示例数据
            var testClasses = new List<TestClass>
            {
                new TestClass{MaterialCode="A", SubMaterialCode="B"},
                new TestClass{MaterialCode="B", SubMaterialCode="C"},
                new TestClass{MaterialCode="C", SubMaterialCode="D"},
                new TestClass{MaterialCode="C", SubMaterialCode="E"},

                new TestClass{MaterialCode="E", SubMaterialCode="F"},
                new TestClass{MaterialCode="E", SubMaterialCode="G"},
                new TestClass{MaterialCode="F", SubMaterialCode="H"}
            };


            var processor = new PartHierarchyProcessor(testClasses);

            try
            {
                // 获取并输出所有零部件号的层级关系
                var hierarchies = processor.GetAllPartHierarchies();
                foreach (var hierarchy in hierarchies)
                {
                    Console.WriteLine($"零部件号: {hierarchy.MaterialCode}, 层级: {hierarchy.Hierarchy}");
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    public class TestClass
    {
        public string MaterialCode { get; set; }
        public string SubMaterialCode { get; set; }
    }

    public class PartHierarchy
    {
        public string MaterialCode { get; set; }
        public string Hierarchy { get; set; }

        public bool hasParent { get; set; }
    }

    public class PartHierarchyProcessor
    {
        private readonly List<TestClass> _testClasses;

        public PartHierarchyProcessor(List<TestClass> testClasses)
        {
            _testClasses = testClasses;
        }

        // 校验零部件号与子零部件号是否相同
        private void ValidatePartCodes()
        {
            foreach (var item in _testClasses)
            {
                if (item.MaterialCode == item.SubMaterialCode)
                {
                    throw new InvalidOperationException($"零部件号 {item.MaterialCode} 和子零部件号不能相等！");
                }
            }
        }

        // 递归查找并计算层级
        private List<PartHierarchy> GetPartHierarchy(string materialCode, string parentHierarchy = "1")
        {
            var partHierarchyList = new List<PartHierarchy>();

            int index = 1;
            // 获取所有与当前零部件号相关的子零部件号
            var subParts = _testClasses.Where(x => x.MaterialCode == materialCode).ToList();
            if (!subParts.Any())
            {
                partHierarchyList.Add(new PartHierarchy() { MaterialCode = materialCode, Hierarchy = $"{parentHierarchy}.{index}" });
            }

            var isMany = false;
            var fenChaFirst = string.Empty;
            if (subParts.Count > 1)
            {
                isMany = true;
                fenChaFirst = $"{parentHierarchy}.1";
            }

            foreach (var subPart in subParts)
            {
                // 构造当前子零部件的层级
                string currentHierarchy = $"{parentHierarchy}.{index}";

                partHierarchyList.Add(new PartHierarchy
                {
                    MaterialCode = subPart.MaterialCode,
                    Hierarchy = currentHierarchy,
                    hasParent = true
                });

                // 递归获取该子零部件的子零部件的层级
                partHierarchyList.AddRange(GetPartHierarchy(subPart.SubMaterialCode, currentHierarchy));

                index++;
            }

            return partHierarchyList;
        }

        // 获取所有零部件号的层级关系
        public List<PartHierarchy> GetAllPartHierarchies()
        {
            // 校验零部件号与子零部件号相等的情况
            ValidatePartCodes();

            var partHierarchies = new List<PartHierarchy>();

            // 遍历所有零部件号
            foreach (var part in _testClasses.Select(x => x.MaterialCode).Distinct())
            {
                // 获取每个零部件号的层级
                partHierarchies.AddRange(GetPartHierarchy(part));
            }

            // 排序并去重，确保层级按正确顺序排列
            return partHierarchies
                .GroupBy(p => p.MaterialCode)  // 根据 MaterialCode 分组
                .Select(g => g.First())         // 只保留每个零部件号的第一个出现
                .OrderBy(p => p.Hierarchy)     // 按层级排序
                .ToList();
        }
    }
}
