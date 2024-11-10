using System;
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

        public string SubMaterialCode { set; get; }
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

        private string IncrementVersion(string versionStr)
        {
            // 将字符串按 '.' 分割成数组
            string[] versionParts = versionStr.Split('.');

            // 获取最后一个元素，并将其转换为整数，增加 1
            int lastPart = int.Parse(versionParts[versionParts.Length - 1]);
            lastPart++;

            // 更新最后一个元素
            versionParts[versionParts.Length - 1] = lastPart.ToString();

            // 将数组重新合并成字符串
            return string.Join(".", versionParts);
        }

        public List<PartHierarchy> _partHierarchy { set; get; } = new List<PartHierarchy>();

        // 递归查找并计算层级
        private List<PartHierarchy> GetPartHierarchy(string materialCode, string parentHierarchy = "1")
        {
            var partHierarchyList = new List<PartHierarchy>();

            int index = 1;
            //分析是不是SubMaterialCode号中的最后一个，例如H，这样的
            var subParts = _testClasses.Where(x => x.MaterialCode == materialCode).ToList();
            if (!subParts.Any())
            {
                var newLeave = $"{parentHierarchy}.{index}";
                partHierarchyList.Add(new PartHierarchy() { MaterialCode = materialCode, Hierarchy = newLeave });
                _partHierarchy.Add(new PartHierarchy() { MaterialCode = materialCode, Hierarchy = newLeave });
            }




            foreach (var subPart in subParts)
            {
                // 构造当前子零部件的层级
                string currentHierarchy = $"{parentHierarchy}.{index}";

                //如果这个集合中已经存在了这个C，那么就获取到已经存在的C，对应的层级
                var hasExistCode = _partHierarchy?.FirstOrDefault(x => x.MaterialCode == subPart.MaterialCode);
                var MaterialCode = subPart.MaterialCode;
                var newnewHierarchy = string.Empty;
                if (hasExistCode != null)
                {
                    var many = _testClasses.Where(x => x.MaterialCode == MaterialCode).ToList();
                    //这样就会找到C的数量是多个，
                    //那么就找出来另外的一个是什么
                    var filter = many.Where(x => x.MaterialCode == subPart.MaterialCode && x.SubMaterialCode != subPart.SubMaterialCode).ToList();

                    var sss = _partHierarchy.Where(x => x.MaterialCode == filter.FirstOrDefault()?.SubMaterialCode).ToList();
                    if (sss.Any())
                    {
                        var lastHierarchy = sss.FirstOrDefault().Hierarchy;
                        newnewHierarchy = IncrementVersion(lastHierarchy);
                    }
                    else
                    {
                        newnewHierarchy = currentHierarchy;
                    }

                    MaterialCode = subPart.SubMaterialCode;

                }
                else
                {
                    newnewHierarchy = currentHierarchy;
                }

                partHierarchyList.Add(new PartHierarchy
                {
                    MaterialCode = MaterialCode,
                    Hierarchy = newnewHierarchy,
                    hasParent = true
                });


                var isExist = _partHierarchy.Where(x => x.MaterialCode == subPart.MaterialCode).ToList();
                if (!isExist.Any())
                {
                    _partHierarchy.Add(new PartHierarchy() { MaterialCode = subPart.MaterialCode, Hierarchy = newnewHierarchy });

                }

                // 递归获取该子零部件的子零部件的层级
                partHierarchyList.AddRange(GetPartHierarchy(subPart.SubMaterialCode, newnewHierarchy));

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

            foreach (var item in _testClasses)
            {
                partHierarchies.AddRange(GetPartHierarchy(item.MaterialCode));
                partHierarchies.AddRange(GetPartHierarchy(item.SubMaterialCode));
            }

            //// 遍历所有零部件号
            //var lists = _testClasses.Select(x => x.MaterialCode).Distinct().ToList();
            //foreach (var part in lists)
            //{
            //    // 获取每个零部件号的层级
            //    partHierarchies.AddRange(GetPartHierarchy(part));
            //}

            // 排序并去重，确保层级按正确顺序排列
            return partHierarchies
                .GroupBy(p => p.MaterialCode)  // 根据 MaterialCode 分组
                .Select(g => g.First())         // 只保留每个零部件号的第一个出现
                .OrderBy(p => p.Hierarchy)     // 按层级排序
                .ToList();
        }
    }
}
