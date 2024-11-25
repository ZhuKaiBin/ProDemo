namespace 控制行高3
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // 初始化数据库中的功能单元
            List<FuncUnit> funcUnits = new List<FuncUnit>
            {
                new FuncUnit { Id = 1, Name = "功能单元1", Modulus = "12", SortOrder = 1 },
                new FuncUnit { Id = 2, Name = "功能单元2", Modulus = "8/2", SortOrder = 2 },
                new FuncUnit { Id = 3, Name = "功能单元3", Modulus = "8/2", SortOrder = 3 },
                 new FuncUnit { Id = 3, Name = "功能单元4", Modulus = "8/4", SortOrder = 4 },
                //new FuncUnit { Id = 3, Name = "功能单元3", Modulus = "12", SortOrder = 3 }
            };

            // 批量新增功能单元
            AddFuncUnits(funcUnits, new List<FuncUnit>
            {
                new FuncUnit { Name = "新增功能单元1", Modulus = "8/2" },
                new FuncUnit { Name = "新增功能单元2", Modulus = "8/2" },
                new FuncUnit { Name = "新增功能单元3", Modulus = "8/3" },
                new FuncUnit { Name = "新增功能单元5", Modulus = "12" },
                new FuncUnit { Name = "新增功能单元4", Modulus = "8/3" },
                new FuncUnit { Name = "新增功能单元6", Modulus = "8/3" },
                new FuncUnit { Name = "新增功能单元7", Modulus = "8/2" },
                new FuncUnit { Name = "新增功能单元8", Modulus = "8/2" },

                new FuncUnit { Name = "新增功能单元8", Modulus = "8/4" },
            });

            // 新增功能单元逻辑

            // 打印结果
            foreach (var unit in funcUnits.OrderBy(u => u.SortOrder))
            {
                Console.WriteLine($"Id: {unit.Id}, Name: {unit.Name}, Modulus: {unit.Modulus}, SortOrder: {unit.SortOrder}");
            }
        }

        private static void AddFuncUnits(List<FuncUnit> funcUnits, List<FuncUnit> newUnits)
        {
            int nextId = funcUnits.Any() ? funcUnits.Max(f => f.Id) + 1 : 1;

            foreach (var newUnit in newUnits)
            {
                if (newUnit.Modulus.Contains("/")) // 处理带 "/" 的模数
                {
                    var parts = newUnit.Modulus.Split('/');
                    int modHeight = int.Parse(parts[0]);
                    int maxUnitsPerRow = int.Parse(parts[1]);

                    // 查找是否存在相同模数的行
                    var existingUnits = funcUnits
                        .Where(f => f.Modulus == newUnit.Modulus)
                        .OrderBy(f => f.SortOrder)
                        .ToList();

                    if (existingUnits.Count > 0)
                    {
                        // 当前相同模数的行已使用的数量
                        int currentCountInRow = existingUnits.Count;

                        if (currentCountInRow < maxUnitsPerRow)
                        {
                            // 在最后一个相同模数的行之后插入
                            var lastSameModulusUnit = existingUnits.Last();
                            int insertIndex = funcUnits.IndexOf(lastSameModulusUnit) + 1;

                            funcUnits.Insert(insertIndex, new FuncUnit
                            {
                                Id = nextId++,
                                Name = newUnit.Name,
                                Modulus = newUnit.Modulus,
                                SortOrder = lastSameModulusUnit.SortOrder + 1
                            });

                            // 更新后续元素的 SortOrder
                            for (int i = insertIndex + 1; i < funcUnits.Count; i++)
                            {
                                funcUnits[i].SortOrder++;
                            }

                            continue;
                        }
                    }

                    // 如果数据库中没有相同模数或行已满，直接新增到最后
                    funcUnits.Add(new FuncUnit
                    {
                        Id = nextId++,
                        Name = newUnit.Name,
                        Modulus = newUnit.Modulus,
                        SortOrder = funcUnits.Any() ? funcUnits.Max(f => f.SortOrder) + 1 : 1
                    });
                }
                else // 处理整数模数
                {
                    funcUnits.Add(new FuncUnit
                    {
                        Id = nextId++,
                        Name = newUnit.Name,
                        Modulus = newUnit.Modulus,
                        SortOrder = funcUnits.Max(f => f.SortOrder) + 1
                    });
                }
            }
        }
    }

    public class FuncUnit
    {
        public int Id { set; get; }

        public string Name { set; get; }

        public string Modulus { set; get; }

        public int SortOrder { set; get; }
    }
}