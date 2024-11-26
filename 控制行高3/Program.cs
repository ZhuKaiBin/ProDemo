namespace 控制行高3
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // 初始化数据库中的功能单元
            List<FuncUnit> funcUnits = new List<FuncUnit>
            {
                new FuncUnit { Id = 42, Name = "8/2_01", Modulus = "8/2", SortOrder = 1 },
                new FuncUnit { Id = 43, Name = "8/2_02", Modulus = "8/2", SortOrder = 2 },
                new FuncUnit { Id = 44, Name = "8/2_03", Modulus = "8/2", SortOrder = 3 },
                new FuncUnit { Id = 45, Name = "8/2_04", Modulus = "8/2", SortOrder = 4 },
                new FuncUnit { Id = 46, Name = "8/2_05", Modulus = "8/2", SortOrder = 5 },
                new FuncUnit { Id = 47, Name = "12_01", Modulus = "12", SortOrder = 6 },
                new FuncUnit { Id = 48, Name = "12_02", Modulus = "12", SortOrder = 7 },
            };

            // 批量新增功能单元
            AddFuncUnits(funcUnits, new List<FuncUnit>
            {
                new FuncUnit { Name = "新增功能单元1", Modulus = "8/2" },
            });

            // 打印结果
            foreach (var unit in funcUnits.OrderBy(u => u.SortOrder))
            {
                Console.WriteLine($"Id: {unit.Id}, Name: {unit.Name}, Modulus: {unit.Modulus}, SortOrder: {unit.SortOrder}");
            }
        }

        private static void AddFuncUnits(List<FuncUnit> oldFuncUnits, List<FuncUnit> newUnits)
        {
            int nextId = oldFuncUnits.Any() ? oldFuncUnits.Max(f => f.Id) + 1 : 1;

            foreach (var newUnit in newUnits)
            {
                if (newUnit.Modulus.Contains("/")) // 处理带 "/" 的模数
                {
                    var parts = newUnit.Modulus.Split('/');
                    int modHeight = int.Parse(parts[0]);
                    int maxUnitsPerRow = int.Parse(parts[1]);

                    // 查找是否存在相同模数的行
                    var existingUnits = oldFuncUnits
                        .Where(f => f.Modulus == newUnit.Modulus)
                        .OrderBy(f => f.SortOrder)
                        .ToList();

                    if (existingUnits.Count > 0)
                    {
                        // 当前相同模数的行已使用的数量
                        int currentCountInRow = existingUnits.Count;

                        //这里是取模，就是看当前的这个类型的模数是不是已经是"满的"
                        //比如:数据库中已经存在的"8/2"是5个，那么再新增一个，那么5%2=1 是大于0的，说明还没有"满",那就应该再原来的"8/2"上继续排序
                        var oushu = currentCountInRow % maxUnitsPerRow;
                        if (oushu > 0 || currentCountInRow < maxUnitsPerRow)
                        {
                            // 在最后一个相同模数的行之后插入
                            var lastSameModulusUnit = existingUnits.Last();
                            int insertIndex = oldFuncUnits.OrderBy(x => x.SortOrder).ToList().IndexOf(lastSameModulusUnit) + 1;

                            oldFuncUnits.Insert(insertIndex, new FuncUnit
                            {
                                Id = nextId++,
                                Name = newUnit.Name,
                                Modulus = newUnit.Modulus,
                                SortOrder = lastSameModulusUnit.SortOrder + 1
                            });

                            // 更新后续元素的 SortOrder
                            for (int i = insertIndex + 1; i < oldFuncUnits.Count; i++)
                            {
                                var name = oldFuncUnits[i].Name;
                                oldFuncUnits[i].SortOrder++;
                            }

                            continue;
                        }
                    }

                    // 如果数据库中没有相同模数或行已满，直接新增到最后
                    oldFuncUnits.Add(new FuncUnit
                    {
                        Id = nextId++,
                        Name = newUnit.Name,
                        Modulus = newUnit.Modulus,
                        SortOrder = oldFuncUnits.Any() ? oldFuncUnits.Max(f => f.SortOrder) + 1 : 1
                    });
                }
                else // 处理整数模数
                {
                    oldFuncUnits.Add(new FuncUnit
                    {
                        Id = nextId++,
                        Name = newUnit.Name,
                        Modulus = newUnit.Modulus,
                        SortOrder = oldFuncUnits.Max(f => f.SortOrder) + 1
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
