using System.Collections.Generic;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace 获取Action中Item的数据源
{

    public class na
    {
        public string? name { set; get; }
    }


    internal class Program
    {
        static void Main(string[] args)
        {


            List<na> nas = new List<na>()
            {
                 new na(){ },
                 new na(){ name="1"}
            };


            var n = new na();
            if (n == null)
            {

            }


            var sd = nas.Where(x => x?.name.Contains("s") == true);





            List<string> sidePlate = new List<string>() { "panelHeight", "overallDepth", "ingressProtectionDegree", "whetherWithHole", "mainBusbarPosition", "mainBusbarSize" };

            ss ss = new ss();
            List<Dictionary<string, List<BaseOptionRangeItem>>> s = new List<Dictionary<string, List<BaseOptionRangeItem>>>();
            foreach (string side in sidePlate)
            {
                //var dic = ss.ReturnCompents("mainBusbarSize");
                var dic = ss.ReturnCompents(side);
                s.Add(dic);
            }

        }
    }


    #region 

    public class ss
    {
        //传入要查询的哪一个，比如说【端封板】的是否:

        public Dictionary<string, List<BaseOptionRangeItem>> ReturnCompents(string productCode = "whetherWithHole")
        {
            List<OptionsConstraintCabinetEntitySpec> source = OptionsConstraintCabinetEntitySpecSource2.GetSource();

            //1.【端封板】  需要因素：柜体高度(mm)	整体深度(mm)	防护等级	是否开孔	水平主母线位置	水平主母线规格(mm)	左/右端封板

            //2.针对是否开孔在【选项约束柜体规格(OptionsConstraintCabinetEntitySpec)】中有多少约束                                   
            var filteredList = source
                .Where(spec => spec.Actions.Any(action => action.OptionCode == productCode))
                .ToList();


            if (!filteredList.Any())
            {
                throw new Exception("这里是在M列中没有找到对应的配置，那么就是找产品去映射");
            }


            //3.这种情况是：进线方式==》 是否开孔
            //            ACB下端子方案==》是否开孔
            //groupedData 是所有符合【是否开孔】的约束数据
            var groupedData = filteredList
                .GroupBy(spec => string.Join(",", spec.Condition.Options.Select(option => option.OptionCode)))
                .ToDictionary(g => g.Key, g => g.ToList());


            Dictionary<string, List<BaseOptionRangeItem>> save = new Dictionary<string, List<BaseOptionRangeItem>>();

            //组柜参数
            //var groupEntityDefineParas = groupEntity.DefineParas;
            foreach (var spec in groupedData)
            {
                var key = spec.Key;//进线方式 或者 ACB下端子方案
                var sd = spec.Value;

                if (key.Contains(','))//如果包含，那证明是多个因素组成的复合条件
                {
                    var ssss = key.Split(',');

                    var doubleSd = sd;

                    foreach (var i in ssss)
                    {
                        var UIvalue = "根据i获取用户界面上的值";
                        string eeeed = "";
                        doubleSd = doubleSd
                                 .Where(spec => spec.Condition.Options
                                 .Any(option => option.OptionValue.Code.Equals(UIvalue, StringComparison.CurrentCultureIgnoreCase)))
                                 .ToList();
                    }

                    if (doubleSd.Count == 0)
                    {
                        save.Add(productCode, new List<BaseOptionRangeItem>());
                    }

                    var ddd = doubleSd.FirstOrDefault().Actions.FirstOrDefault().OptionValueRanges;
                    if (ddd.Count == 1 && string.IsNullOrEmpty(ddd[0].Code))
                    {
                        continue;
                    }
                    else
                    {
                        // 不满足条件时，过滤掉 Code 为空的对象
                        ddd = ddd.Where(item => !string.IsNullOrEmpty(item.Code)).ToList();

                        ddd = ddd
                                    .GroupBy(item => new { item.Code, item.DescZh, item.DescEn })
                                    .Select(group => group.First())
                                    .ToList();

                    }

                    save.Add(productCode, ddd);
                    continue;
                }

                //strCode是界面上，客户选择的值

                var input = Console.ReadLine();
                var uiChoiceValue = input.Trim();

                var filteredSpecs = sd
                                   .Where(spec => spec.Condition.Options
                                   .Any(option => option.OptionValue.Code.Equals(uiChoiceValue, StringComparison.CurrentCultureIgnoreCase)))
                                   .ToList();

                if (filteredSpecs.Count > 1)
                {
                    throw new Exception("这里是映射错误，比如【进线方式】的【母排上进】对应的M列有多个值");
                }

                if (filteredSpecs.Count == 0)
                {
                    continue;
                }

                var dd = filteredSpecs.FirstOrDefault().Actions.FirstOrDefault().OptionValueRanges;
                if (dd.Count == 1 && string.IsNullOrEmpty(dd[0].Code))
                {
                    continue;
                }
                else
                {
                    // 不满足条件时，过滤掉 Code 为空的对象
                    dd = dd.Where(item => !string.IsNullOrEmpty(item.Code)).ToList();
                }

                save.Add(productCode, dd);
            }

            if (save.Count > 0)
            {
                var firstCodeList = save.First().Value.Select(item => item.Code).ToList();

                bool allCodeListsEqual = save.Values.All(list =>
                    list.Select(item => item.Code).SequenceEqual(firstCodeList)
                );

                if (!allCodeListsEqual)
                {
                    throw new Exception("save 中的 List<BaseOptionRangeItem> 的 Code 值不一致，无法合并。");
                }
                else
                {
                    // 如果所有 Code 值一致，可以合并成一个
                    save = new Dictionary<string, List<BaseOptionRangeItem>>
                        {
                            { productCode, save.First().Value }
                        };
                }
            }

            return save;

        }

        #endregion

    }

    #region 逻辑

    public class OptionsConstraintCabinetEntitySpecSource2
    {
        #region GetSource 配置资源

        public static List<OptionsConstraintCabinetEntitySpec> GetSource()
        {
            List<OptionsConstraintCabinetEntitySpec> list = new List<OptionsConstraintCabinetEntitySpec>();
            var _ODfI = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "进线方式",PageType = "CabinetSingle", OptionCode = "incomingType", OptionValue = new BaseOptionRangeItem() { DescEn = "Busbar top incoming", DescZh = "母排上进", Code = "busbarTopIncoming" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "是否开孔",
    OptionCode = "whetherWithHole",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "No", DescZh = "否", Code = "no" },
 } }
            }
            };
            list.Add(_ODfI);



            var _bCVi = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "进线方式",PageType = "CabinetSingle", OptionCode = "incomingType", OptionValue = new BaseOptionRangeItem() { DescEn = "Busbar side incoming", DescZh = "母排侧进", Code = "busbarSideIncoming" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "是否开孔",
    OptionCode = "whetherWithHole",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "Yes", DescZh = "是", Code = "yes" },
 } }
            }
            };
            list.Add(_bCVi);



            var _AicP = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "进线方式",PageType = "CabinetSingle", OptionCode = "incomingType", OptionValue = new BaseOptionRangeItem() { DescEn = "Cable top incoming", DescZh = "电缆上进", Code = "cableTopIncoming" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "是否开孔",
    OptionCode = "whetherWithHole",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "No", DescZh = "否", Code = "no" },
 } }
            }
            };
            list.Add(_AicP);



            var _owKw = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "进线方式",PageType = "CabinetSingle", OptionCode = "incomingType", OptionValue = new BaseOptionRangeItem() { DescEn = "Cable bottom incoming", DescZh = "电缆下进", Code = "cableBottomIncoming" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "是否开孔",
    OptionCode = "whetherWithHole",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "No", DescZh = "否", Code = "no" },
 } }
            }
            };
            list.Add(_owKw);



            var _yCHa = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "ACB下端子方案",PageType = "FunctionUnit", OptionCode = "lowerTerminalACB", OptionValue = new BaseOptionRangeItem() { DescEn = "Left tie", DescZh = "左母联", Code = "leftTie" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "是否开孔",
    OptionCode = "whetherWithHole",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "No", DescZh = "否", Code = "no" },
 } }
            }
            };
            list.Add(_yCHa);



            var _nMHv = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "ACB下端子方案",PageType = "FunctionUnit", OptionCode = "lowerTerminalACB", OptionValue = new BaseOptionRangeItem() { DescEn = "Right tie", DescZh = "右母联", Code = "rightTie" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "是否开孔",
    OptionCode = "whetherWithHole",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "No", DescZh = "否", Code = "no" },
 } }
            }
            };
            list.Add(_nMHv);



            var _zUub = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "ACB下端子方案",PageType = "FunctionUnit", OptionCode = "lowerTerminalACB", OptionValue = new BaseOptionRangeItem() { DescEn = "Extend top of panel", DescZh = "伸出柜顶", Code = "extendTopOfPanel" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "是否开孔",
    OptionCode = "whetherWithHole",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "No", DescZh = "否", Code = "no" },
 } }
            }
            };
            list.Add(_zUub);



            var _FcQb = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "ACB下端子方案",PageType = "FunctionUnit", OptionCode = "lowerTerminalACB", OptionValue = new BaseOptionRangeItem() { DescEn = "Connected with cable", DescZh = "电缆连接", Code = "connectedWithCable" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "是否开孔",
    OptionCode = "whetherWithHole",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "No", DescZh = "否", Code = "no" },
 } }
            }
            };
            list.Add(_FcQb);



            var _KanX = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "ACB下端子方案",PageType = "FunctionUnit", OptionCode = "lowerTerminalACB", OptionValue = new BaseOptionRangeItem() { DescEn = "Extend from side", DescZh = "伸出侧面", Code = "extendFromSide" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "是否开孔",
    OptionCode = "whetherWithHole",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "Yes", DescZh = "是", Code = "yes" },
 } }
            }
            };
            list.Add(_KanX);



            var _nVvG = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "ACB上端子方案",PageType = "FunctionUnit", OptionCode = "upperTerminalACB", OptionValue = new BaseOptionRangeItem() { DescEn = "Connect horizontal bar", DescZh = "连接上水平排", Code = "connectHorizontalBar" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "上桩头/下桩头",
    OptionCode = "busbarTerminal",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "Upper", DescZh = "上桩头", Code = "upper" },
         new BaseOptionRangeItem() { DescEn = "Upper", DescZh = "上桩头", Code = "upper" },
         new BaseOptionRangeItem() { DescEn = "Upper", DescZh = "上桩头", Code = "upper" },
 } }
            }
            };
            list.Add(_nVvG);



            var _Gzep = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "ACB上端子方案",PageType = "FunctionUnit", OptionCode = "upperTerminalACB", OptionValue = new BaseOptionRangeItem() { DescEn = "Connect vertical bar", DescZh = "连接垂直排", Code = "connectVerticalBar" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "上桩头/下桩头",
    OptionCode = "busbarTerminal",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "Upper", DescZh = "上桩头", Code = "upper" },
         new BaseOptionRangeItem() { DescEn = "Upper", DescZh = "上桩头", Code = "upper" },
         new BaseOptionRangeItem() { DescEn = "Upper", DescZh = "上桩头", Code = "upper" },
 } }
            }
            };
            list.Add(_Gzep);



            var _fOcw = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "ACB上端子方案",PageType = "FunctionUnit", OptionCode = "upperTerminalACB", OptionValue = new BaseOptionRangeItem() { DescEn = "Connect horizontal bar", DescZh = "连接上水平排", Code = "connectHorizontalBar" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "ACB桩头连接方式",
    OptionCode = "acbConnectionsType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "ToHorizontalBusbar(ACBUpperBusbarTerminal)", DescZh = "连接水平排(上桩头)", Code = "toHorizontalBusbarACBUpperBusbarTerminal" },
         new BaseOptionRangeItem() { DescEn = "ToHorizontalBusbar(ACBUpperBusbarTerminal)", DescZh = "连接水平排(上桩头)", Code = "toHorizontalBusbarACBUpperBusbarTerminal" },
         new BaseOptionRangeItem() { DescEn = "ToHorizontalBusbar(ACBUpperBusbarTerminal)", DescZh = "连接水平排(上桩头)", Code = "toHorizontalBusbarACBUpperBusbarTerminal" },
 } }
            }
            };
            list.Add(_fOcw);



            var _AMrx = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "ACB上端子方案",PageType = "FunctionUnit", OptionCode = "upperTerminalACB", OptionValue = new BaseOptionRangeItem() { DescEn = "Connect vertical bar", DescZh = "连接垂直排", Code = "connectVerticalBar" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "ACB桩头连接方式",
    OptionCode = "acbConnectionsType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "ToVerticalBusbar(ACBUpperBusbarTerminal)", DescZh = "连接垂直排(上桩头)", Code = "toVerticalBusbarACBUpperBusbarTerminal" },
         new BaseOptionRangeItem() { DescEn = "ToVerticalBusbar(ACBUpperBusbarTerminal)", DescZh = "连接垂直排(上桩头)", Code = "toVerticalBusbarACBUpperBusbarTerminal" },
         new BaseOptionRangeItem() { DescEn = "ToVerticalBusbar(ACBUpperBusbarTerminal)", DescZh = "连接垂直排(上桩头)", Code = "toVerticalBusbarACBUpperBusbarTerminal" },
 } }
            }
            };
            list.Add(_AMrx);



            var _vZOY = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "ACB下端子方案",PageType = "FunctionUnit", OptionCode = "lowerTerminalACB", OptionValue = new BaseOptionRangeItem() { DescEn = "Left tie", DescZh = "左母联", Code = "leftTie" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "上桩头/下桩头",
    OptionCode = "busbarTerminal",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "Lower", DescZh = "下桩头", Code = "lower" },
         new BaseOptionRangeItem() { DescEn = "Lower", DescZh = "下桩头", Code = "lower" },
         new BaseOptionRangeItem() { DescEn = "Lower", DescZh = "下桩头", Code = "lower" },
 } }
            }
            };
            list.Add(_vZOY);



            var _LVHP = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "ACB下端子方案",PageType = "FunctionUnit", OptionCode = "lowerTerminalACB", OptionValue = new BaseOptionRangeItem() { DescEn = "Right tie", DescZh = "右母联", Code = "rightTie" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "上桩头/下桩头",
    OptionCode = "busbarTerminal",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "Lower", DescZh = "下桩头", Code = "lower" },
         new BaseOptionRangeItem() { DescEn = "Lower", DescZh = "下桩头", Code = "lower" },
         new BaseOptionRangeItem() { DescEn = "Lower", DescZh = "下桩头", Code = "lower" },
 } }
            }
            };
            list.Add(_LVHP);



            var _zBgH = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "ACB下端子方案",PageType = "FunctionUnit", OptionCode = "lowerTerminalACB", OptionValue = new BaseOptionRangeItem() { DescEn = "Extend top of panel", DescZh = "伸出柜顶", Code = "extendTopOfPanel" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "上桩头/下桩头",
    OptionCode = "busbarTerminal",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "Lower", DescZh = "下桩头", Code = "lower" },
         new BaseOptionRangeItem() { DescEn = "Lower", DescZh = "下桩头", Code = "lower" },
         new BaseOptionRangeItem() { DescEn = "Lower", DescZh = "下桩头", Code = "lower" },
 } }
            }
            };
            list.Add(_zBgH);



            var _YSvZ = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "ACB下端子方案",PageType = "FunctionUnit", OptionCode = "lowerTerminalACB", OptionValue = new BaseOptionRangeItem() { DescEn = "Connected with cable", DescZh = "电缆连接", Code = "connectedWithCable" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "上桩头/下桩头",
    OptionCode = "busbarTerminal",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "Lower", DescZh = "下桩头", Code = "lower" },
         new BaseOptionRangeItem() { DescEn = "Lower", DescZh = "下桩头", Code = "lower" },
         new BaseOptionRangeItem() { DescEn = "Lower", DescZh = "下桩头", Code = "lower" },
 } }
            }
            };
            list.Add(_YSvZ);



            var _GTju = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "ACB下端子方案",PageType = "FunctionUnit", OptionCode = "lowerTerminalACB", OptionValue = new BaseOptionRangeItem() { DescEn = "Extend from side", DescZh = "伸出侧面", Code = "extendFromSide" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "上桩头/下桩头",
    OptionCode = "busbarTerminal",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "Lower", DescZh = "下桩头", Code = "lower" },
         new BaseOptionRangeItem() { DescEn = "Lower", DescZh = "下桩头", Code = "lower" },
         new BaseOptionRangeItem() { DescEn = "Lower", DescZh = "下桩头", Code = "lower" },
 } }
            }
            };
            list.Add(_GTju);



            var _NcPf = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "ACB下端子方案",PageType = "FunctionUnit", OptionCode = "lowerTerminalACB", OptionValue = new BaseOptionRangeItem() { DescEn = "Left tie", DescZh = "左母联", Code = "leftTie" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "ACB桩头连接方式",
    OptionCode = "acbConnectionsType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "LeftTie(ACBLowerBusbarTerminal)", DescZh = "左母联(下桩头)", Code = "leftTieACBLowerBusbarTerminal" },
         new BaseOptionRangeItem() { DescEn = "LeftTie(ACBLowerBusbarTerminal)", DescZh = "左母联(下桩头)", Code = "leftTieACBLowerBusbarTerminal" },
         new BaseOptionRangeItem() { DescEn = "LeftTie(ACBLowerBusbarTerminal)", DescZh = "左母联(下桩头)", Code = "leftTieACBLowerBusbarTerminal" },
 } }
            }
            };
            list.Add(_NcPf);



            var _uBLa = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "ACB下端子方案",PageType = "FunctionUnit", OptionCode = "lowerTerminalACB", OptionValue = new BaseOptionRangeItem() { DescEn = "Right tie", DescZh = "右母联", Code = "rightTie" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "ACB桩头连接方式",
    OptionCode = "acbConnectionsType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "RightSideContactToBusbar(LowerBusbarTerminal)", DescZh = "右母联(下桩头)", Code = "rightSideContactToBusbarLowerBusbarTerminal" },
         new BaseOptionRangeItem() { DescEn = "RightSideContactToBusbar(LowerBusbarTerminal)", DescZh = "右母联(下桩头)", Code = "rightSideContactToBusbarLowerBusbarTerminal" },
         new BaseOptionRangeItem() { DescEn = "RightSideContactToBusbar(LowerBusbarTerminal)", DescZh = "右母联(下桩头)", Code = "rightSideContactToBusbarLowerBusbarTerminal" },
 } }
            }
            };
            list.Add(_uBLa);



            var _EzDh = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "ACB下端子方案",PageType = "FunctionUnit", OptionCode = "lowerTerminalACB", OptionValue = new BaseOptionRangeItem() { DescEn = "Extend top of panel", DescZh = "伸出柜顶", Code = "extendTopOfPanel" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "ACB桩头连接方式",
    OptionCode = "acbConnectionsType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "ExtendToTopOfPanel(ACBLowerBusbarTerminal)", DescZh = "伸出柜顶(下桩头)", Code = "extendToTopOfPanelACBLowerBusbarTerminal" },
         new BaseOptionRangeItem() { DescEn = "ExtendToTopOfPanel(ACBLowerBusbarTerminal)", DescZh = "伸出柜顶(下桩头)", Code = "extendToTopOfPanelACBLowerBusbarTerminal" },
         new BaseOptionRangeItem() { DescEn = "ExtendToTopOfPanel(ACBLowerBusbarTerminal)", DescZh = "伸出柜顶(下桩头)", Code = "extendToTopOfPanelACBLowerBusbarTerminal" },
 } }
            }
            };
            list.Add(_EzDh);



            var _RVrG = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "ACB下端子方案",PageType = "FunctionUnit", OptionCode = "lowerTerminalACB", OptionValue = new BaseOptionRangeItem() { DescEn = "Connected with cable", DescZh = "电缆连接", Code = "connectedWithCable" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "ACB桩头连接方式",
    OptionCode = "acbConnectionsType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "CableContect(ACBLowerBusbarTerminal)", DescZh = "电缆连接(下桩头)", Code = "cableContectACBLowerBusbarTerminal" },
         new BaseOptionRangeItem() { DescEn = "CableContect(ACBLowerBusbarTerminal)", DescZh = "电缆连接(下桩头)", Code = "cableContectACBLowerBusbarTerminal" },
         new BaseOptionRangeItem() { DescEn = "CableContect(ACBLowerBusbarTerminal)", DescZh = "电缆连接(下桩头)", Code = "cableContectACBLowerBusbarTerminal" },
 } }
            }
            };
            list.Add(_RVrG);



            var _gzqn = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "ACB下端子方案",PageType = "FunctionUnit", OptionCode = "lowerTerminalACB", OptionValue = new BaseOptionRangeItem() { DescEn = "Extend from side", DescZh = "伸出侧面", Code = "extendFromSide" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "ACB桩头连接方式",
    OptionCode = "acbConnectionsType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "ExtendToSideOfPanel(LowerBusbarTerminal)", DescZh = "伸出侧面(下桩头)", Code = "extendToSideOfPanelLowerBusbarTerminal" },
         new BaseOptionRangeItem() { DescEn = "ExtendToSideOfPanel(LowerBusbarTerminal)", DescZh = "伸出侧面(下桩头)", Code = "extendToSideOfPanelLowerBusbarTerminal" },
         new BaseOptionRangeItem() { DescEn = "ExtendToSideOfPanel(LowerBusbarTerminal)", DescZh = "伸出侧面(下桩头)", Code = "extendToSideOfPanelLowerBusbarTerminal" },
 } }
            }
            };
            list.Add(_gzqn);



            var _fcZH = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "垂直母线载流(A)",PageType = "CabinetSingle", OptionCode = "verticalBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "630", DescZh = "630", Code = "630" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "PEN排载流(A)",
    OptionCode = "pENBarCurrent",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "315", DescZh = "315", Code = "315" },
 } }
            }
            };
            list.Add(_fcZH);



            var _FKzh = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "垂直母线载流(A)",PageType = "CabinetSingle", OptionCode = "verticalBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "PEN排载流(A)",
    OptionCode = "pENBarCurrent",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "400", DescZh = "400", Code = "400" },
 } }
            }
            };
            list.Add(_FKzh);



            var _wyHY = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "垂直母线载流(A)",PageType = "CabinetSingle", OptionCode = "verticalBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "PEN排载流(A)",
    OptionCode = "pENBarCurrent",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
 } }
            }
            };
            list.Add(_wyHY);



            var _uNpb = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "垂直母线载流(A)",PageType = "CabinetSingle", OptionCode = "verticalBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "PEN排载流(A)",
    OptionCode = "pENBarCurrent",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
 } }
            }
            };
            list.Add(_uNpb);



            var _sFJM = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "垂直母线载流(A)",PageType = "CabinetSingle", OptionCode = "verticalBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "PEN排载流(A)",
    OptionCode = "pENBarCurrent",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "750", DescZh = "750", Code = "750" },
 } }
            }
            };
            list.Add(_sFJM);



            var _rgot = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "垂直母线载流(A)",PageType = "CabinetSingle", OptionCode = "verticalBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "1800", DescZh = "1800", Code = "1800" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "PEN排载流(A)",
    OptionCode = "pENBarCurrent",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
 } }
            }
            };
            list.Add(_rgot);



            var _mGXM = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "垂直母线载流(A)",PageType = "CabinetSingle", OptionCode = "verticalBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "2000", DescZh = "2000", Code = "2000" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "PEN排载流(A)",
    OptionCode = "pENBarCurrent",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
 } }
            }
            };
            list.Add(_mGXM);



            var _HmEj = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "垂直母线载流(A)",PageType = "CabinetSingle", OptionCode = "verticalBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "2200", DescZh = "2200", Code = "2200" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "PEN排载流(A)",
    OptionCode = "pENBarCurrent",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
 } }
            }
            };
            list.Add(_HmEj);



            var _Suum = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "垂直母线载流(A)",PageType = "CabinetSingle", OptionCode = "verticalBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "2500", DescZh = "2500", Code = "2500" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "PEN排载流(A)",
    OptionCode = "pENBarCurrent",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1250", DescZh = "1250", Code = "1250" },
 } }
            }
            };
            list.Add(_Suum);



            var _lTTx = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "垂直母线载流(A)",PageType = "CabinetSingle", OptionCode = "verticalBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "3200", DescZh = "3200", Code = "3200" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "PEN排载流(A)",
    OptionCode = "pENBarCurrent",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" },
 } }
            }
            };
            list.Add(_lTTx);



            var _dYEb = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "功能单元极数",PageType = "FunctionUnit", OptionCode = "polesOfFunctionUnit", OptionValue = new BaseOptionRangeItem() { DescEn = "3P", DescZh = "3P", Code = "3P" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "PEN排类型",
    OptionCode = "pENBarModel",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "PE+N", DescZh = "PE+N", Code = "pe+n" },
 } }
            }
            };
            list.Add(_dYEb);



            var _Egtb = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "功能单元极数",PageType = "FunctionUnit", OptionCode = "polesOfFunctionUnit", OptionValue = new BaseOptionRangeItem() { DescEn = "4P", DescZh = "4P", Code = "4P" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "PEN排类型",
    OptionCode = "pENBarModel",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "PE", DescZh = "PE", Code = "pe" },
 } }
            }
            };
            list.Add(_Egtb);



            var _eNDI = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "功能单元极数",PageType = "FunctionUnit", OptionCode = "polesOfFunctionUnit", OptionValue = new BaseOptionRangeItem() { DescEn = "3P+N", DescZh = "3P+N", Code = "3P+N" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "PEN排类型",
    OptionCode = "pENBarModel",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "PE", DescZh = "PE", Code = "pe" },
 } }
            }
            };
            list.Add(_eNDI);



            var _bycO = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
 } }
            }
            };
            list.Add(_bycO);



            var _OwJX = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
 } }
            }
            };
            list.Add(_OwJX);



            var _JWMg = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
 } }
            }
            };
            list.Add(_JWMg);



            var _Qucp = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
 } }
            }
            };
            list.Add(_Qucp);



            var _KeAI = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
 } }
            }
            };
            list.Add(_KeAI);



            var _LJcU = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
 } }
            }
            };
            list.Add(_LJcU);



            var _JxNT = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
 } }
            }
            };
            list.Add(_JxNT);



            var _YWyC = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
 } }
            }
            };
            list.Add(_YWyC);



            var _CQOw = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
 } }
            }
            };
            list.Add(_CQOw);



            var _Lwle = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
 } }
            }
            };
            list.Add(_Lwle);



            var _GIEc = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "700(B200+F500)", DescZh = "700(B200+F500)", Code = "700(B200+F500)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
 } }
            }
            };
            list.Add(_GIEc);



            var _NoUt = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "800(B200+F600)", DescZh = "800(B200+F600)", Code = "800(B200+F600)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
 } }
            }
            };
            list.Add(_NoUt);



            var _GOnR = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "900(B200+F700)", DescZh = "900(B200+F700)", Code = "900(B200+F700)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
 } }
            }
            };
            list.Add(_GOnR);



            var _qbhU = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1000(B200+F800)", DescZh = "1000(B200+F800)", Code = "1000(B200+F800)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
 } }
            }
            };
            list.Add(_qbhU);



            var _AdXm = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1100(B200+F900)", DescZh = "1100(B200+F900)", Code = "1100(B200+F900)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
 } }
            }
            };
            list.Add(_AdXm);



            var _RmkT = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1200(B200+F1000)", DescZh = "1200(B200+F1000)", Code = "1200(B200+F1000)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
 } }
            }
            };
            list.Add(_RmkT);



            var _WdQp = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1300(B200+F1100)", DescZh = "1300(B200+F1100)", Code = "1300(B200+F1100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
 } }
            }
            };
            list.Add(_WdQp);



            var _nePV = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1400(B200+F1200)", DescZh = "1400(B200+F1200)", Code = "1400(B200+F1200)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
 } }
            }
            };
            list.Add(_nePV);



            var _SeFG = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1500(B200+F1300)", DescZh = "1500(B200+F1300)", Code = "1500(B200+F1300)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
 } }
            }
            };
            list.Add(_SeFG);



            var _XDRR = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "700(F500+B200)", DescZh = "700(F500+B200)", Code = "700(F500+B200)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
 } }
            }
            };
            list.Add(_XDRR);



            var _Rrcf = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "800(F600+B200)", DescZh = "800(F600+B200)", Code = "800(F600+B200)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
 } }
            }
            };
            list.Add(_Rrcf);



            var _WIdY = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "900(F700+B200)", DescZh = "900(F700+B200)", Code = "900(F700+B200)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
 } }
            }
            };
            list.Add(_WIdY);



            var _Icbl = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1000(F800+B200)", DescZh = "1000(F800+B200)", Code = "1000(F800+B200)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
 } }
            }
            };
            list.Add(_Icbl);



            var _JZGT = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1100(F900+B200)", DescZh = "1100(F900+B200)", Code = "1100(F900+B200)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
 } }
            }
            };
            list.Add(_JZGT);



            var _VvqF = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1200(F1000+B200)", DescZh = "1200(F1000+B200)", Code = "1200(F1000+B200)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
 } }
            }
            };
            list.Add(_VvqF);



            var _mFyb = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1300(F1100+B200)", DescZh = "1300(F1100+B200)", Code = "1300(F1100+B200)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
 } }
            }
            };
            list.Add(_mFyb);



            var _yyLF = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1400(F1200+B200)", DescZh = "1400(F1200+B200)", Code = "1400(F1200+B200)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
 } }
            }
            };
            list.Add(_yyLF);



            var _KYVi = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1500(F1300+B200)", DescZh = "1500(F1300+B200)", Code = "1500(F1300+B200)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
 } }
            }
            };
            list.Add(_KYVi);



            var _iOYG = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "800(F500+C300)", DescZh = "800(F500+C300)", Code = "800(F500+C300)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
 } }
            }
            };
            list.Add(_iOYG);



            var _aOXw = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1000(F600+C400)", DescZh = "1000(F600+C400)", Code = "1000(F600+C400)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
 } }
            }
            };
            list.Add(_aOXw);



            var _bIZr = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1000(F700+C300)", DescZh = "1000(F700+C300)", Code = "1000(F700+C300)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
 } }
            }
            };
            list.Add(_bIZr);



            var _XiKY = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1100(F800+C300)", DescZh = "1100(F800+C300)", Code = "1100(F800+C300)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
 } }
            }
            };
            list.Add(_XiKY);



            var _VEjg = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1200(F800+C400)", DescZh = "1200(F800+C400)", Code = "1200(F800+C400)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
 } }
            }
            };
            list.Add(_VEjg);



            var _sqjE = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1200(F900+C300)", DescZh = "1200(F900+C300)", Code = "1200(F900+C300)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
 } }
            }
            };
            list.Add(_sqjE);



            var _ubeR = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1400(F1000+C400)", DescZh = "1400(F1000+C400)", Code = "1400(F1000+C400)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
 } }
            }
            };
            list.Add(_ubeR);



            var _VymY = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1000(B200+F500+C300)", DescZh = "1000(B200+F500+C300)", Code = "1000(B200+F500+C300)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
 } }
            }
            };
            list.Add(_VymY);



            var _sooP = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1200(B200+F600+C400)", DescZh = "1200(B200+F600+C400)", Code = "1200(B200+F600+C400)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
 } }
            }
            };
            list.Add(_sooP);



            var _fOhM = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1200(B200+F700+C300)", DescZh = "1200(B200+F700+C300)", Code = "1200(B200+F700+C300)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
 } }
            }
            };
            list.Add(_fOhM);



            var _kkRw = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1300(B200+F800+C300)", DescZh = "1300(B200+F800+C300)", Code = "1300(B200+F800+C300)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
 } }
            }
            };
            list.Add(_kkRw);



            var _IzBn = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1400(B200+F800+C400)", DescZh = "1400(B200+F800+C400)", Code = "1400(B200+F800+C400)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
 } }
            }
            };
            list.Add(_IzBn);



            var _GWuD = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1400(B200+F900+C300)", DescZh = "1400(B200+F900+C300)", Code = "1400(B200+F900+C300)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
 } }
            }
            };
            list.Add(_GWuD);



            var _ULld = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1600(B200+F1000+C400)", DescZh = "1600(B200+F1000+C400)", Code = "1600(B200+F1000+C400)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
 } }
            }
            };
            list.Add(_ULld);



            var _vssD = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "600(F500+100)", DescZh = "600(F500+100)", Code = "600(F500+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
 } }
            }
            };
            list.Add(_vssD);



            var _uuPJ = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "700(F600+100)", DescZh = "700(F600+100)", Code = "700(F600+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
 } }
            }
            };
            list.Add(_uuPJ);



            var _HsAT = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "800(F700+100)", DescZh = "800(F700+100)", Code = "800(F700+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
 } }
            }
            };
            list.Add(_HsAT);



            var _bCkU = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "900(F800+100)", DescZh = "900(F800+100)", Code = "900(F800+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
 } }
            }
            };
            list.Add(_bCkU);



            var _BblQ = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1000(F900+100)", DescZh = "1000(F900+100)", Code = "1000(F900+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
 } }
            }
            };
            list.Add(_BblQ);



            var _AwIv = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1100(F1000+100)", DescZh = "1100(F1000+100)", Code = "1100(F1000+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
 } }
            }
            };
            list.Add(_AwIv);



            var _ZpWS = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1200(F1100+100)", DescZh = "1200(F1100+100)", Code = "1200(F1100+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
 } }
            }
            };
            list.Add(_ZpWS);



            var _IGFj = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1300(F1200+100)", DescZh = "1300(F1200+100)", Code = "1300(F1200+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
 } }
            }
            };
            list.Add(_IGFj);



            var _OBuJ = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1400(F1300+100)", DescZh = "1400(F1300+100)", Code = "1400(F1300+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
 } }
            }
            };
            list.Add(_OBuJ);



            var _pVob = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1500(F1400+100)", DescZh = "1500(F1400+100)", Code = "1500(F1400+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
 } }
            }
            };
            list.Add(_pVob);



            var _nNGm = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "800(B200+F500+100)", DescZh = "800(B200+F500+100)", Code = "800(B200+F500+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
 } }
            }
            };
            list.Add(_nNGm);



            var _bHOn = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "900(B200+F600+100)", DescZh = "900(B200+F600+100)", Code = "900(B200+F600+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
 } }
            }
            };
            list.Add(_bHOn);



            var _AKzv = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1000(B200+F700+100)", DescZh = "1000(B200+F700+100)", Code = "1000(B200+F700+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
 } }
            }
            };
            list.Add(_AKzv);



            var _nepM = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1100(B200+F800+100)", DescZh = "1100(B200+F800+100)", Code = "1100(B200+F800+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
 } }
            }
            };
            list.Add(_nepM);



            var _gtkT = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1200(B200+F900+100)", DescZh = "1200(B200+F900+100)", Code = "1200(B200+F900+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
 } }
            }
            };
            list.Add(_gtkT);



            var _pATr = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1300(B200+F1000+100)", DescZh = "1300(B200+F1000+100)", Code = "1300(B200+F1000+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
 } }
            }
            };
            list.Add(_pATr);



            var _GsHe = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1400(B200+F1100+100)", DescZh = "1400(B200+F1100+100)", Code = "1400(B200+F1100+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
 } }
            }
            };
            list.Add(_GsHe);



            var _MPNN = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1500(B200+F1200+100)", DescZh = "1500(B200+F1200+100)", Code = "1500(B200+F1200+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
 } }
            }
            };
            list.Add(_MPNN);



            var _oGRv = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1600(B200+F1300+100)", DescZh = "1600(B200+F1300+100)", Code = "1600(B200+F1300+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
 } }
            }
            };
            list.Add(_oGRv);



            var _GSZf = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "800(F500+B200+100)", DescZh = "800(F500+B200+100)", Code = "800(F500+B200+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
 } }
            }
            };
            list.Add(_GSZf);



            var _LoXN = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "900(F600+B200+100)", DescZh = "900(F600+B200+100)", Code = "900(F600+B200+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
 } }
            }
            };
            list.Add(_LoXN);



            var _uFPV = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1000(F700+B200+100)", DescZh = "1000(F700+B200+100)", Code = "1000(F700+B200+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
 } }
            }
            };
            list.Add(_uFPV);



            var _wltf = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1100(F800+B200+100)", DescZh = "1100(F800+B200+100)", Code = "1100(F800+B200+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
 } }
            }
            };
            list.Add(_wltf);



            var _iBAb = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1200(F900+B200+100)", DescZh = "1200(F900+B200+100)", Code = "1200(F900+B200+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
 } }
            }
            };
            list.Add(_iBAb);



            var _HBje = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1300(F1000+B200+100)", DescZh = "1300(F1000+B200+100)", Code = "1300(F1000+B200+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
 } }
            }
            };
            list.Add(_HBje);



            var _BjHd = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1400(F1100+B200+100)", DescZh = "1400(F1100+B200+100)", Code = "1400(F1100+B200+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
 } }
            }
            };
            list.Add(_BjHd);



            var _bqDv = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1500(F1200+B200+100)", DescZh = "1500(F1200+B200+100)", Code = "1500(F1200+B200+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
 } }
            }
            };
            list.Add(_bqDv);



            var _RpOV = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1600(F1300+B200+100)", DescZh = "1600(F1300+B200+100)", Code = "1600(F1300+B200+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
 } }
            }
            };
            list.Add(_RpOV);



            var _NvMO = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "900(F500+C300+100)", DescZh = "900(F500+C300+100)", Code = "900(F500+C300+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
 } }
            }
            };
            list.Add(_NvMO);



            var _qgdn = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1100(F600+C400+100)", DescZh = "1100(F600+C400+100)", Code = "1100(F600+C400+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
 } }
            }
            };
            list.Add(_qgdn);



            var _hObV = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1100(F700+C300+100)", DescZh = "1100(F700+C300+100)", Code = "1100(F700+C300+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
 } }
            }
            };
            list.Add(_hObV);



            var _zXaJ = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1200(F800+C300+100)", DescZh = "1200(F800+C300+100)", Code = "1200(F800+C300+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
 } }
            }
            };
            list.Add(_zXaJ);



            var _uUPD = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1300(F800+C400+100)", DescZh = "1300(F800+C400+100)", Code = "1300(F800+C400+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
 } }
            }
            };
            list.Add(_uUPD);



            var _YOFB = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1300(F900+C300+100)", DescZh = "1300(F900+C300+100)", Code = "1300(F900+C300+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
 } }
            }
            };
            list.Add(_YOFB);



            var _AyUL = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1500(F1000+C400+100)", DescZh = "1500(F1000+C400+100)", Code = "1500(F1000+C400+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
 } }
            }
            };
            list.Add(_AyUL);



            var _uEsS = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1100(B200+F500+C300+100)", DescZh = "1100(B200+F500+C300+100)", Code = "1100(B200+F500+C300+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
 } }
            }
            };
            list.Add(_uEsS);



            var _ndbs = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1300(B200+F600+C400+100)", DescZh = "1300(B200+F600+C400+100)", Code = "1300(B200+F600+C400+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
 } }
            }
            };
            list.Add(_ndbs);



            var _dlAQ = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1300(B200+F700+C300+100)", DescZh = "1300(B200+F700+C300+100)", Code = "1300(B200+F700+C300+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
 } }
            }
            };
            list.Add(_dlAQ);



            var _aEZI = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1400(B200+F800+C300+100)", DescZh = "1400(B200+F800+C300+100)", Code = "1400(B200+F800+C300+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
 } }
            }
            };
            list.Add(_aEZI);



            var _KCrf = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1500(B200+F800+C400+100)", DescZh = "1500(B200+F800+C400+100)", Code = "1500(B200+F800+C400+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
 } }
            }
            };
            list.Add(_KCrf);



            var _lfwx = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1500(B200+F900+C300+100)", DescZh = "1500(B200+F900+C300+100)", Code = "1500(B200+F900+C300+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
 } }
            }
            };
            list.Add(_lfwx);



            var _aqyb = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1700(B200+F1000+C400+100)", DescZh = "1700(B200+F1000+C400+100)", Code = "1700(B200+F1000+C400+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单元宽度(mm)",
    OptionCode = "horizontalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
 } }
            }
            };
            list.Add(_aqyb);



            var _wjQl = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "800(F500+C300)", DescZh = "800(F500+C300)", Code = "800(F500+C300)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "电缆室宽度(mm)",
    OptionCode = "cableRoomWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "300", DescZh = "300", Code = "300" },
 } }
            }
            };
            list.Add(_wjQl);



            var _mLnH = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1000(F600+C400)", DescZh = "1000(F600+C400)", Code = "1000(F600+C400)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "电缆室宽度(mm)",
    OptionCode = "cableRoomWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "400", DescZh = "400", Code = "400" },
 } }
            }
            };
            list.Add(_mLnH);



            var _NWcy = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1000(F700+C300)", DescZh = "1000(F700+C300)", Code = "1000(F700+C300)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "电缆室宽度(mm)",
    OptionCode = "cableRoomWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "300", DescZh = "300", Code = "300" },
 } }
            }
            };
            list.Add(_NWcy);



            var _jqxr = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1100(F800+C300)", DescZh = "1100(F800+C300)", Code = "1100(F800+C300)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "电缆室宽度(mm)",
    OptionCode = "cableRoomWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "300", DescZh = "300", Code = "300" },
 } }
            }
            };
            list.Add(_jqxr);



            var _FoYg = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1200(F800+C400)", DescZh = "1200(F800+C400)", Code = "1200(F800+C400)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "电缆室宽度(mm)",
    OptionCode = "cableRoomWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "400", DescZh = "400", Code = "400" },
 } }
            }
            };
            list.Add(_FoYg);



            var _KSmE = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1200(F900+C300)", DescZh = "1200(F900+C300)", Code = "1200(F900+C300)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "电缆室宽度(mm)",
    OptionCode = "cableRoomWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "300", DescZh = "300", Code = "300" },
 } }
            }
            };
            list.Add(_KSmE);



            var _ZFRw = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1400(F1000+C400)", DescZh = "1400(F1000+C400)", Code = "1400(F1000+C400)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "电缆室宽度(mm)",
    OptionCode = "cableRoomWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "400", DescZh = "400", Code = "400" },
 } }
            }
            };
            list.Add(_ZFRw);



            var _oYoi = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1000(B200+F500+C300)", DescZh = "1000(B200+F500+C300)", Code = "1000(B200+F500+C300)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "电缆室宽度(mm)",
    OptionCode = "cableRoomWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "300", DescZh = "300", Code = "300" },
 } }
            }
            };
            list.Add(_oYoi);



            var _uXlc = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1200(B200+F600+C400)", DescZh = "1200(B200+F600+C400)", Code = "1200(B200+F600+C400)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "电缆室宽度(mm)",
    OptionCode = "cableRoomWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "400", DescZh = "400", Code = "400" },
 } }
            }
            };
            list.Add(_uXlc);



            var _ykHj = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1200(B200+F700+C300)", DescZh = "1200(B200+F700+C300)", Code = "1200(B200+F700+C300)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "电缆室宽度(mm)",
    OptionCode = "cableRoomWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "300", DescZh = "300", Code = "300" },
 } }
            }
            };
            list.Add(_ykHj);



            var _qqwA = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1300(B200+F800+C300)", DescZh = "1300(B200+F800+C300)", Code = "1300(B200+F800+C300)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "电缆室宽度(mm)",
    OptionCode = "cableRoomWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "300", DescZh = "300", Code = "300" },
 } }
            }
            };
            list.Add(_qqwA);



            var _yIvT = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1400(B200+F800+C400)", DescZh = "1400(B200+F800+C400)", Code = "1400(B200+F800+C400)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "电缆室宽度(mm)",
    OptionCode = "cableRoomWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "400", DescZh = "400", Code = "400" },
 } }
            }
            };
            list.Add(_yIvT);



            var _eCuo = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1400(B200+F900+C300)", DescZh = "1400(B200+F900+C300)", Code = "1400(B200+F900+C300)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "电缆室宽度(mm)",
    OptionCode = "cableRoomWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "300", DescZh = "300", Code = "300" },
 } }
            }
            };
            list.Add(_eCuo);



            var _LGbs = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1600(B200+F1000+C400)", DescZh = "1600(B200+F1000+C400)", Code = "1600(B200+F1000+C400)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "电缆室宽度(mm)",
    OptionCode = "cableRoomWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "400", DescZh = "400", Code = "400" },
 } }
            }
            };
            list.Add(_LGbs);



            var _FMNp = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "900(F500+C300+100)", DescZh = "900(F500+C300+100)", Code = "900(F500+C300+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "电缆室宽度(mm)",
    OptionCode = "cableRoomWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "300", DescZh = "300", Code = "300" },
 } }
            }
            };
            list.Add(_FMNp);



            var _zxOo = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1100(F600+C400+100)", DescZh = "1100(F600+C400+100)", Code = "1100(F600+C400+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "电缆室宽度(mm)",
    OptionCode = "cableRoomWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "400", DescZh = "400", Code = "400" },
 } }
            }
            };
            list.Add(_zxOo);



            var _dmgf = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1100(F700+C300+100)", DescZh = "1100(F700+C300+100)", Code = "1100(F700+C300+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "电缆室宽度(mm)",
    OptionCode = "cableRoomWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "300", DescZh = "300", Code = "300" },
 } }
            }
            };
            list.Add(_dmgf);



            var _PPPl = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1200(F800+C300+100)", DescZh = "1200(F800+C300+100)", Code = "1200(F800+C300+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "电缆室宽度(mm)",
    OptionCode = "cableRoomWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "300", DescZh = "300", Code = "300" },
 } }
            }
            };
            list.Add(_PPPl);



            var _ipRa = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1300(F800+C400+100)", DescZh = "1300(F800+C400+100)", Code = "1300(F800+C400+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "电缆室宽度(mm)",
    OptionCode = "cableRoomWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "400", DescZh = "400", Code = "400" },
 } }
            }
            };
            list.Add(_ipRa);



            var _Wtzc = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1300(F900+C300+100)", DescZh = "1300(F900+C300+100)", Code = "1300(F900+C300+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "电缆室宽度(mm)",
    OptionCode = "cableRoomWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "300", DescZh = "300", Code = "300" },
 } }
            }
            };
            list.Add(_Wtzc);



            var _LRtx = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1500(F1000+C400+100)", DescZh = "1500(F1000+C400+100)", Code = "1500(F1000+C400+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "电缆室宽度(mm)",
    OptionCode = "cableRoomWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "400", DescZh = "400", Code = "400" },
 } }
            }
            };
            list.Add(_LRtx);



            var _tKRd = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1100(B200+F500+C300+100)", DescZh = "1100(B200+F500+C300+100)", Code = "1100(B200+F500+C300+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "电缆室宽度(mm)",
    OptionCode = "cableRoomWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "300", DescZh = "300", Code = "300" },
 } }
            }
            };
            list.Add(_tKRd);



            var _VVBZ = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1300(B200+F600+C400+100)", DescZh = "1300(B200+F600+C400+100)", Code = "1300(B200+F600+C400+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "电缆室宽度(mm)",
    OptionCode = "cableRoomWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "400", DescZh = "400", Code = "400" },
 } }
            }
            };
            list.Add(_VVBZ);



            var _vOsI = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1300(B200+F700+C300+100)", DescZh = "1300(B200+F700+C300+100)", Code = "1300(B200+F700+C300+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "电缆室宽度(mm)",
    OptionCode = "cableRoomWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "300", DescZh = "300", Code = "300" },
 } }
            }
            };
            list.Add(_vOsI);



            var _zUJi = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1400(B200+F800+C300+100)", DescZh = "1400(B200+F800+C300+100)", Code = "1400(B200+F800+C300+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "电缆室宽度(mm)",
    OptionCode = "cableRoomWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "300", DescZh = "300", Code = "300" },
 } }
            }
            };
            list.Add(_zUJi);



            var _ZiVp = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1500(B200+F800+C400+100)", DescZh = "1500(B200+F800+C400+100)", Code = "1500(B200+F800+C400+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "电缆室宽度(mm)",
    OptionCode = "cableRoomWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "400", DescZh = "400", Code = "400" },
 } }
            }
            };
            list.Add(_ZiVp);



            var _PVvG = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1500(B200+F900+C300+100)", DescZh = "1500(B200+F900+C300+100)", Code = "1500(B200+F900+C300+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "电缆室宽度(mm)",
    OptionCode = "cableRoomWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "300", DescZh = "300", Code = "300" },
 } }
            }
            };
            list.Add(_PVvG);



            var _cmur = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1700(B200+F1000+C400+100)", DescZh = "1700(B200+F1000+C400+100)", Code = "1700(B200+F1000+C400+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "电缆室宽度(mm)",
    OptionCode = "cableRoomWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "400", DescZh = "400", Code = "400" },
 } }
            }
            };
            list.Add(_cmur);



            var _ICUZ = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "500(B200+F300)", DescZh = "500(B200+F300)", Code = "500(B200+F300)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_ICUZ);



            var _Lhnq = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "600(B200+F400)", DescZh = "600(B200+F400)", Code = "600(B200+F400)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_Lhnq);



            var _wpkq = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "700(B200+F500)", DescZh = "700(B200+F500)", Code = "700(B200+F500)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_wpkq);



            var _gmiy = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "800(B200+F600)", DescZh = "800(B200+F600)", Code = "800(B200+F600)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_gmiy);



            var _xlve = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "900(B200+F700)", DescZh = "900(B200+F700)", Code = "900(B200+F700)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_xlve);



            var _ueKX = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1000(B200+F800)", DescZh = "1000(B200+F800)", Code = "1000(B200+F800)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_ueKX);



            var _otLR = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1100(B200+F900)", DescZh = "1100(B200+F900)", Code = "1100(B200+F900)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_otLR);



            var _Uqrl = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1200(B200+F1000)", DescZh = "1200(B200+F1000)", Code = "1200(B200+F1000)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_Uqrl);



            var _bILX = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1300(B200+F1100)", DescZh = "1300(B200+F1100)", Code = "1300(B200+F1100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_bILX);



            var _QIRH = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1400(B200+F1200)", DescZh = "1400(B200+F1200)", Code = "1400(B200+F1200)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_QIRH);



            var _HyLv = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1500(B200+F1300)", DescZh = "1500(B200+F1300)", Code = "1500(B200+F1300)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_HyLv);



            var _TjDF = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "500(F300+B200)", DescZh = "500(F300+B200)", Code = "500(F300+B200)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_TjDF);



            var _ZBEk = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "600(F400+B200)", DescZh = "600(F400+B200)", Code = "600(F400+B200)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_ZBEk);



            var _mPSf = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "700(F500+B200)", DescZh = "700(F500+B200)", Code = "700(F500+B200)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_mPSf);



            var _igpt = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "800(F600+B200)", DescZh = "800(F600+B200)", Code = "800(F600+B200)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_igpt);



            var _Zfeo = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "900(F700+B200)", DescZh = "900(F700+B200)", Code = "900(F700+B200)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_Zfeo);



            var _SCvn = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1000(F800+B200)", DescZh = "1000(F800+B200)", Code = "1000(F800+B200)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_SCvn);



            var _BIBk = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1100(F900+B200)", DescZh = "1100(F900+B200)", Code = "1100(F900+B200)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_BIBk);



            var _AMvQ = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1200(F1000+B200)", DescZh = "1200(F1000+B200)", Code = "1200(F1000+B200)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_AMvQ);



            var _HVLT = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1300(F1100+B200)", DescZh = "1300(F1100+B200)", Code = "1300(F1100+B200)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_HVLT);



            var _rhVR = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1400(F1200+B200)", DescZh = "1400(F1200+B200)", Code = "1400(F1200+B200)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_rhVR);



            var _vXqd = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1500(F1300+B200)", DescZh = "1500(F1300+B200)", Code = "1500(F1300+B200)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_vXqd);



            var _BZuD = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1000(B200+F500+C300)", DescZh = "1000(B200+F500+C300)", Code = "1000(B200+F500+C300)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_BZuD);



            var _XTgn = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1200(B200+F600+C400)", DescZh = "1200(B200+F600+C400)", Code = "1200(B200+F600+C400)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_XTgn);



            var _KhLz = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1200(B200+F700+C300)", DescZh = "1200(B200+F700+C300)", Code = "1200(B200+F700+C300)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_KhLz);



            var _bIyc = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1300(B200+F800+C300)", DescZh = "1300(B200+F800+C300)", Code = "1300(B200+F800+C300)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_bIyc);



            var _YIrh = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1400(B200+F800+C400)", DescZh = "1400(B200+F800+C400)", Code = "1400(B200+F800+C400)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_YIrh);



            var _GHUz = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1400(B200+F900+C300)", DescZh = "1400(B200+F900+C300)", Code = "1400(B200+F900+C300)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_GHUz);



            var _gfnd = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1600(B200+F1000+C400)", DescZh = "1600(B200+F1000+C400)", Code = "1600(B200+F1000+C400)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_gfnd);



            var _Iusw = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "600(B200+F300+100)", DescZh = "600(B200+F300+100)", Code = "600(B200+F300+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_Iusw);



            var _IPfc = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "700(B200+F400+100)", DescZh = "700(B200+F400+100)", Code = "700(B200+F400+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_IPfc);



            var _mTTH = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "800(B200+F500+100)", DescZh = "800(B200+F500+100)", Code = "800(B200+F500+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_mTTH);



            var _slzg = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "900(B200+F600+100)", DescZh = "900(B200+F600+100)", Code = "900(B200+F600+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_slzg);



            var _zuDz = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1000(B200+F700+100)", DescZh = "1000(B200+F700+100)", Code = "1000(B200+F700+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_zuDz);



            var _Kigq = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1100(B200+F800+100)", DescZh = "1100(B200+F800+100)", Code = "1100(B200+F800+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_Kigq);



            var _nIbQ = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1200(B200+F900+100)", DescZh = "1200(B200+F900+100)", Code = "1200(B200+F900+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_nIbQ);



            var _HvuT = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1300(B200+F1000+100)", DescZh = "1300(B200+F1000+100)", Code = "1300(B200+F1000+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_HvuT);



            var _xAcT = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1400(B200+F1100+100)", DescZh = "1400(B200+F1100+100)", Code = "1400(B200+F1100+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_xAcT);



            var _VOWz = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1500(B200+F1200+100)", DescZh = "1500(B200+F1200+100)", Code = "1500(B200+F1200+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_VOWz);



            var _PrEX = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1600(B200+F1300+100)", DescZh = "1600(B200+F1300+100)", Code = "1600(B200+F1300+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_PrEX);



            var _KHUn = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "600(F300+B200+100)", DescZh = "600(F300+B200+100)", Code = "600(F300+B200+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_KHUn);



            var _SIxM = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "700(F400+B200+100)", DescZh = "700(F400+B200+100)", Code = "700(F400+B200+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_SIxM);



            var _RUMB = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "800(F500+B200+100)", DescZh = "800(F500+B200+100)", Code = "800(F500+B200+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_RUMB);



            var _GjQn = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "900(F600+B200+100)", DescZh = "900(F600+B200+100)", Code = "900(F600+B200+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_GjQn);



            var _Rlum = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1000(F700+B200+100)", DescZh = "1000(F700+B200+100)", Code = "1000(F700+B200+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_Rlum);



            var _FwBu = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1100(F800+B200+100)", DescZh = "1100(F800+B200+100)", Code = "1100(F800+B200+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_FwBu);



            var _sAuB = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1200(F900+B200+100)", DescZh = "1200(F900+B200+100)", Code = "1200(F900+B200+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_sAuB);



            var _fHfk = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1300(F1000+B200+100)", DescZh = "1300(F1000+B200+100)", Code = "1300(F1000+B200+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_fHfk);



            var _pirF = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1400(F1100+B200+100)", DescZh = "1400(F1100+B200+100)", Code = "1400(F1100+B200+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_pirF);



            var _RfiT = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1500(F1200+B200+100)", DescZh = "1500(F1200+B200+100)", Code = "1500(F1200+B200+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_RfiT);



            var _aJdG = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1600(F1300+B200+100)", DescZh = "1600(F1300+B200+100)", Code = "1600(F1300+B200+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_aJdG);



            var _ghwY = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1100(B200+F500+C300+100)", DescZh = "1100(B200+F500+C300+100)", Code = "1100(B200+F500+C300+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_ghwY);



            var _ZSkj = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1300(B200+F600+C400+100)", DescZh = "1300(B200+F600+C400+100)", Code = "1300(B200+F600+C400+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_ZSkj);



            var _cJyz = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1300(B200+F700+C300+100)", DescZh = "1300(B200+F700+C300+100)", Code = "1300(B200+F700+C300+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_cJyz);



            var _CScO = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1400(B200+F800+C300+100)", DescZh = "1400(B200+F800+C300+100)", Code = "1400(B200+F800+C300+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_CScO);



            var _jRoF = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1500(B200+F800+C400+100)", DescZh = "1500(B200+F800+C400+100)", Code = "1500(B200+F800+C400+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_jRoF);



            var _Ufco = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1500(B200+F900+C300+100)", DescZh = "1500(B200+F900+C300+100)", Code = "1500(B200+F900+C300+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_Ufco);



            var _OAQu = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1700(B200+F1000+C400+100)", DescZh = "1700(B200+F1000+C400+100)", Code = "1700(B200+F1000+C400+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直通道宽度(mm)",
    OptionCode = "verticalBusbarUnitWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_OAQu);



            var _eEnk = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "300", DescZh = "300", Code = "300" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "300", DescZh = "300", Code = "300" },
         new BaseOptionRangeItem() { DescEn = "300", DescZh = "300", Code = "300" },
         new BaseOptionRangeItem() { DescEn = "300", DescZh = "300", Code = "300" },
         new BaseOptionRangeItem() { DescEn = "300", DescZh = "300", Code = "300" },
         new BaseOptionRangeItem() { DescEn = "300", DescZh = "300", Code = "300" },
         new BaseOptionRangeItem() { DescEn = "300", DescZh = "300", Code = "300" },
         new BaseOptionRangeItem() { DescEn = "300", DescZh = "300", Code = "300" },
         new BaseOptionRangeItem() { DescEn = "300", DescZh = "300", Code = "300" },
         new BaseOptionRangeItem() { DescEn = "300", DescZh = "300", Code = "300" },
         new BaseOptionRangeItem() { DescEn = "300", DescZh = "300", Code = "300" },
 } }
            }
            };
            list.Add(_eEnk);



            var _fkMZ = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "400", DescZh = "400", Code = "400" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "400", DescZh = "400", Code = "400" },
         new BaseOptionRangeItem() { DescEn = "400", DescZh = "400", Code = "400" },
         new BaseOptionRangeItem() { DescEn = "400", DescZh = "400", Code = "400" },
         new BaseOptionRangeItem() { DescEn = "400", DescZh = "400", Code = "400" },
         new BaseOptionRangeItem() { DescEn = "400", DescZh = "400", Code = "400" },
         new BaseOptionRangeItem() { DescEn = "400", DescZh = "400", Code = "400" },
         new BaseOptionRangeItem() { DescEn = "400", DescZh = "400", Code = "400" },
         new BaseOptionRangeItem() { DescEn = "400", DescZh = "400", Code = "400" },
         new BaseOptionRangeItem() { DescEn = "400", DescZh = "400", Code = "400" },
         new BaseOptionRangeItem() { DescEn = "400", DescZh = "400", Code = "400" },
 } }
            }
            };
            list.Add(_fkMZ);



            var _otOf = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
 } }
            }
            };
            list.Add(_otOf);



            var _FuDc = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
 } }
            }
            };
            list.Add(_FuDc);



            var _SPad = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
 } }
            }
            };
            list.Add(_SPad);



            var _yZCq = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
 } }
            }
            };
            list.Add(_yZCq);



            var _Ezub = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
 } }
            }
            };
            list.Add(_Ezub);



            var _zJOE = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
 } }
            }
            };
            list.Add(_zJOE);



            var _eKCS = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
 } }
            }
            };
            list.Add(_eKCS);



            var _sDVz = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
 } }
            }
            };
            list.Add(_sDVz);



            var _aFQL = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
 } }
            }
            };
            list.Add(_aFQL);



            var _QEFp = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
 } }
            }
            };
            list.Add(_QEFp);



            var _ZQnL = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
 } }
            }
            };
            list.Add(_ZQnL);



            var _PBNn = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "500(B200+F300)", DescZh = "500(B200+F300)", Code = "500(B200+F300)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
 } }
            }
            };
            list.Add(_PBNn);



            var _qbGk = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "600(B200+F400)", DescZh = "600(B200+F400)", Code = "600(B200+F400)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
 } }
            }
            };
            list.Add(_qbGk);



            var _PAFU = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "700(B200+F500)", DescZh = "700(B200+F500)", Code = "700(B200+F500)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
 } }
            }
            };
            list.Add(_PAFU);



            var _uLVV = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "800(B200+F600)", DescZh = "800(B200+F600)", Code = "800(B200+F600)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
 } }
            }
            };
            list.Add(_uLVV);



            var _EYlz = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "900(B200+F700)", DescZh = "900(B200+F700)", Code = "900(B200+F700)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
 } }
            }
            };
            list.Add(_EYlz);



            var _JsXq = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1000(B200+F800)", DescZh = "1000(B200+F800)", Code = "1000(B200+F800)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
 } }
            }
            };
            list.Add(_JsXq);



            var _rJBC = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1100(B200+F900)", DescZh = "1100(B200+F900)", Code = "1100(B200+F900)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
 } }
            }
            };
            list.Add(_rJBC);



            var _DqRH = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1200(B200+F1000)", DescZh = "1200(B200+F1000)", Code = "1200(B200+F1000)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
 } }
            }
            };
            list.Add(_DqRH);



            var _qlkx = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1300(B200+F1100)", DescZh = "1300(B200+F1100)", Code = "1300(B200+F1100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
 } }
            }
            };
            list.Add(_qlkx);



            var _ipBq = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1400(B200+F1200)", DescZh = "1400(B200+F1200)", Code = "1400(B200+F1200)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
 } }
            }
            };
            list.Add(_ipBq);



            var _VZVf = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1500(B200+F1300)", DescZh = "1500(B200+F1300)", Code = "1500(B200+F1300)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
 } }
            }
            };
            list.Add(_VZVf);



            var _FUip = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "500(F300+B200)", DescZh = "500(F300+B200)", Code = "500(F300+B200)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
 } }
            }
            };
            list.Add(_FUip);



            var _vakC = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "600(F400+B200)", DescZh = "600(F400+B200)", Code = "600(F400+B200)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
 } }
            }
            };
            list.Add(_vakC);



            var _ESzf = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "700(F500+B200)", DescZh = "700(F500+B200)", Code = "700(F500+B200)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
 } }
            }
            };
            list.Add(_ESzf);



            var _mzJG = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "800(F600+B200)", DescZh = "800(F600+B200)", Code = "800(F600+B200)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
 } }
            }
            };
            list.Add(_mzJG);



            var _KPCg = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "900(F700+B200)", DescZh = "900(F700+B200)", Code = "900(F700+B200)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
 } }
            }
            };
            list.Add(_KPCg);



            var _ukNT = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1000(F800+B200)", DescZh = "1000(F800+B200)", Code = "1000(F800+B200)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
 } }
            }
            };
            list.Add(_ukNT);



            var _KfWr = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1100(F900+B200)", DescZh = "1100(F900+B200)", Code = "1100(F900+B200)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
 } }
            }
            };
            list.Add(_KfWr);



            var _svOb = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1200(F1000+B200)", DescZh = "1200(F1000+B200)", Code = "1200(F1000+B200)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
 } }
            }
            };
            list.Add(_svOb);



            var _izes = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1300(F1100+B200)", DescZh = "1300(F1100+B200)", Code = "1300(F1100+B200)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
 } }
            }
            };
            list.Add(_izes);



            var _IPpp = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1400(F1200+B200)", DescZh = "1400(F1200+B200)", Code = "1400(F1200+B200)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
 } }
            }
            };
            list.Add(_IPpp);



            var _rrGe = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1500(F1300+B200)", DescZh = "1500(F1300+B200)", Code = "1500(F1300+B200)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
 } }
            }
            };
            list.Add(_rrGe);



            var _ZrBK = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "800(F500+C300)", DescZh = "800(F500+C300)", Code = "800(F500+C300)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
 } }
            }
            };
            list.Add(_ZrBK);



            var _riRo = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1000(F600+C400)", DescZh = "1000(F600+C400)", Code = "1000(F600+C400)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
 } }
            }
            };
            list.Add(_riRo);



            var _QUBG = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1000(F700+C300)", DescZh = "1000(F700+C300)", Code = "1000(F700+C300)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
 } }
            }
            };
            list.Add(_QUBG);



            var _AlKA = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1100(F800+C300)", DescZh = "1100(F800+C300)", Code = "1100(F800+C300)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
 } }
            }
            };
            list.Add(_AlKA);



            var _npDF = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1200(F800+C400)", DescZh = "1200(F800+C400)", Code = "1200(F800+C400)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
 } }
            }
            };
            list.Add(_npDF);



            var _CaPI = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1200(F900+C300)", DescZh = "1200(F900+C300)", Code = "1200(F900+C300)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
 } }
            }
            };
            list.Add(_CaPI);



            var _Iqki = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1400(F1000+C400)", DescZh = "1400(F1000+C400)", Code = "1400(F1000+C400)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
 } }
            }
            };
            list.Add(_Iqki);



            var _NmeR = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1000(B200+F500+C300)", DescZh = "1000(B200+F500+C300)", Code = "1000(B200+F500+C300)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
 } }
            }
            };
            list.Add(_NmeR);



            var _fHFw = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1200(B200+F600+C400)", DescZh = "1200(B200+F600+C400)", Code = "1200(B200+F600+C400)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
 } }
            }
            };
            list.Add(_fHFw);



            var _PxQQ = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1200(B200+F700+C300)", DescZh = "1200(B200+F700+C300)", Code = "1200(B200+F700+C300)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
 } }
            }
            };
            list.Add(_PxQQ);



            var _vrfS = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1300(B200+F800+C300)", DescZh = "1300(B200+F800+C300)", Code = "1300(B200+F800+C300)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
 } }
            }
            };
            list.Add(_vrfS);



            var _iyRl = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1400(B200+F800+C400)", DescZh = "1400(B200+F800+C400)", Code = "1400(B200+F800+C400)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
 } }
            }
            };
            list.Add(_iyRl);



            var _VxFM = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1400(B200+F900+C300)", DescZh = "1400(B200+F900+C300)", Code = "1400(B200+F900+C300)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
 } }
            }
            };
            list.Add(_VxFM);



            var _xQSM = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1600(B200+F1000+C400)", DescZh = "1600(B200+F1000+C400)", Code = "1600(B200+F1000+C400)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" },
         new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" },
         new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" },
         new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" },
         new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" },
         new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" },
         new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" },
         new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" },
         new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" },
         new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" },
 } }
            }
            };
            list.Add(_xQSM);



            var _REoa = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "600(F500+100)", DescZh = "600(F500+100)", Code = "600(F500+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
 } }
            }
            };
            list.Add(_REoa);



            var _MaXy = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "700(F600+100)", DescZh = "700(F600+100)", Code = "700(F600+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
 } }
            }
            };
            list.Add(_MaXy);



            var _xcKF = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "800(F700+100)", DescZh = "800(F700+100)", Code = "800(F700+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
 } }
            }
            };
            list.Add(_xcKF);



            var _HpVM = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "900(F800+100)", DescZh = "900(F800+100)", Code = "900(F800+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
 } }
            }
            };
            list.Add(_HpVM);



            var _iMSd = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1000(F900+100)", DescZh = "1000(F900+100)", Code = "1000(F900+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
 } }
            }
            };
            list.Add(_iMSd);



            var _LxmU = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1100(F1000+100)", DescZh = "1100(F1000+100)", Code = "1100(F1000+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
 } }
            }
            };
            list.Add(_LxmU);



            var _eHjC = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1200(F1100+100)", DescZh = "1200(F1100+100)", Code = "1200(F1100+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
 } }
            }
            };
            list.Add(_eHjC);



            var _tAMV = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1300(F1200+100)", DescZh = "1300(F1200+100)", Code = "1300(F1200+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
 } }
            }
            };
            list.Add(_tAMV);



            var _BVAL = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1400(F1300+100)", DescZh = "1400(F1300+100)", Code = "1400(F1300+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
 } }
            }
            };
            list.Add(_BVAL);



            var _fdgM = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1500(F1400+100)", DescZh = "1500(F1400+100)", Code = "1500(F1400+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
 } }
            }
            };
            list.Add(_fdgM);



            var _FBcr = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1600(F1500+100)", DescZh = "1600(F1500+100)", Code = "1600(F1500+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" },
         new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" },
         new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" },
         new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" },
         new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" },
         new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" },
         new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" },
         new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" },
         new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" },
         new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" },
 } }
            }
            };
            list.Add(_FBcr);



            var _Zzoj = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "600(B200+F300+100)", DescZh = "600(B200+F300+100)", Code = "600(B200+F300+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
 } }
            }
            };
            list.Add(_Zzoj);



            var _Qupd = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "700(B200+F400+100)", DescZh = "700(B200+F400+100)", Code = "700(B200+F400+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
 } }
            }
            };
            list.Add(_Qupd);



            var _IJqO = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "800(B200+F500+100)", DescZh = "800(B200+F500+100)", Code = "800(B200+F500+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
 } }
            }
            };
            list.Add(_IJqO);



            var _OeKw = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "900(B200+F600+100)", DescZh = "900(B200+F600+100)", Code = "900(B200+F600+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
 } }
            }
            };
            list.Add(_OeKw);



            var _vUZc = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1000(B200+F700+100)", DescZh = "1000(B200+F700+100)", Code = "1000(B200+F700+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
 } }
            }
            };
            list.Add(_vUZc);



            var _qmUj = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1100(B200+F800+100)", DescZh = "1100(B200+F800+100)", Code = "1100(B200+F800+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
 } }
            }
            };
            list.Add(_qmUj);



            var _bxhz = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1200(B200+F900+100)", DescZh = "1200(B200+F900+100)", Code = "1200(B200+F900+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
 } }
            }
            };
            list.Add(_bxhz);



            var _GERp = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1300(B200+F1000+100)", DescZh = "1300(B200+F1000+100)", Code = "1300(B200+F1000+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
 } }
            }
            };
            list.Add(_GERp);



            var _fKNI = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1400(B200+F1100+100)", DescZh = "1400(B200+F1100+100)", Code = "1400(B200+F1100+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
 } }
            }
            };
            list.Add(_fKNI);



            var _RQSq = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1500(B200+F1200+100)", DescZh = "1500(B200+F1200+100)", Code = "1500(B200+F1200+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
 } }
            }
            };
            list.Add(_RQSq);



            var _iQnz = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1600(B200+F1300+100)", DescZh = "1600(B200+F1300+100)", Code = "1600(B200+F1300+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" },
         new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" },
         new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" },
         new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" },
         new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" },
         new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" },
         new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" },
         new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" },
         new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" },
         new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" },
 } }
            }
            };
            list.Add(_iQnz);



            var _WfaM = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "600(F300+B200+100)", DescZh = "600(F300+B200+100)", Code = "600(F300+B200+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
 } }
            }
            };
            list.Add(_WfaM);



            var _tCgq = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "700(F400+B200+100)", DescZh = "700(F400+B200+100)", Code = "700(F400+B200+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
 } }
            }
            };
            list.Add(_tCgq);



            var _hxdu = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "800(F500+B200+100)", DescZh = "800(F500+B200+100)", Code = "800(F500+B200+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
 } }
            }
            };
            list.Add(_hxdu);



            var _AOvw = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "900(F600+B200+100)", DescZh = "900(F600+B200+100)", Code = "900(F600+B200+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
 } }
            }
            };
            list.Add(_AOvw);



            var _IdQB = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1000(F700+B200+100)", DescZh = "1000(F700+B200+100)", Code = "1000(F700+B200+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
 } }
            }
            };
            list.Add(_IdQB);



            var _kVDZ = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1100(F800+B200+100)", DescZh = "1100(F800+B200+100)", Code = "1100(F800+B200+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
 } }
            }
            };
            list.Add(_kVDZ);



            var _epPk = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1200(F900+B200+100)", DescZh = "1200(F900+B200+100)", Code = "1200(F900+B200+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
 } }
            }
            };
            list.Add(_epPk);



            var _Afcd = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1300(F1000+B200+100)", DescZh = "1300(F1000+B200+100)", Code = "1300(F1000+B200+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
 } }
            }
            };
            list.Add(_Afcd);



            var _mLuP = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1400(F1100+B200+100)", DescZh = "1400(F1100+B200+100)", Code = "1400(F1100+B200+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
 } }
            }
            };
            list.Add(_mLuP);



            var _tFVF = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1500(F1200+B200+100)", DescZh = "1500(F1200+B200+100)", Code = "1500(F1200+B200+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
 } }
            }
            };
            list.Add(_tFVF);



            var _FdBV = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1600(F1300+B200+100)", DescZh = "1600(F1300+B200+100)", Code = "1600(F1300+B200+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" },
         new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" },
         new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" },
         new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" },
         new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" },
         new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" },
         new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" },
         new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" },
         new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" },
         new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" },
 } }
            }
            };
            list.Add(_FdBV);



            var _sMAp = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "900(F500+C300+100)", DescZh = "900(F500+C300+100)", Code = "900(F500+C300+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
 } }
            }
            };
            list.Add(_sMAp);



            var _Yrfy = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1100(F600+C400+100)", DescZh = "1100(F600+C400+100)", Code = "1100(F600+C400+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
 } }
            }
            };
            list.Add(_Yrfy);



            var _QlOI = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1100(F700+C300+100)", DescZh = "1100(F700+C300+100)", Code = "1100(F700+C300+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
 } }
            }
            };
            list.Add(_QlOI);



            var _YSFb = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1200(F800+C300+100)", DescZh = "1200(F800+C300+100)", Code = "1200(F800+C300+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
 } }
            }
            };
            list.Add(_YSFb);



            var _lRpP = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1300(F800+C400+100)", DescZh = "1300(F800+C400+100)", Code = "1300(F800+C400+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
 } }
            }
            };
            list.Add(_lRpP);



            var _msix = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1300(F900+C300+100)", DescZh = "1300(F900+C300+100)", Code = "1300(F900+C300+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
 } }
            }
            };
            list.Add(_msix);



            var _gTGr = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1500(F1000+C400+100)", DescZh = "1500(F1000+C400+100)", Code = "1500(F1000+C400+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
 } }
            }
            };
            list.Add(_gTGr);



            var _BRti = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1100(B200+F500+C300+100)", DescZh = "1100(B200+F500+C300+100)", Code = "1100(B200+F500+C300+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
 } }
            }
            };
            list.Add(_BRti);



            var _HFTS = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1300(B200+F600+C400+100)", DescZh = "1300(B200+F600+C400+100)", Code = "1300(B200+F600+C400+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
 } }
            }
            };
            list.Add(_HFTS);



            var _ZrEx = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1300(B200+F700+C300+100)", DescZh = "1300(B200+F700+C300+100)", Code = "1300(B200+F700+C300+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
 } }
            }
            };
            list.Add(_ZrEx);



            var _tPQt = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1400(B200+F800+C300+100)", DescZh = "1400(B200+F800+C300+100)", Code = "1400(B200+F800+C300+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
         new BaseOptionRangeItem() { DescEn = "1400", DescZh = "1400", Code = "1400" },
 } }
            }
            };
            list.Add(_tPQt);



            var _myjy = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1500(B200+F800+C400+100)", DescZh = "1500(B200+F800+C400+100)", Code = "1500(B200+F800+C400+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
 } }
            }
            };
            list.Add(_myjy);



            var _sUbw = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体宽度(mm)",PageType = "CabinetSingle", OptionCode = "panelWidth", OptionValue = new BaseOptionRangeItem() { DescEn = "1500(B200+F900+C300+100)", DescZh = "1500(B200+F900+C300+100)", Code = "1500(B200+F900+C300+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体宽度(mm)",
    OptionCode = "overallWidth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
 } }
            }
            };
            list.Add(_sUbw);



            var _oSNd = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体深度(mm)",PageType = "CabinetGroup", OptionCode = "panelDepth", OptionValue = new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体深度(mm)",
    OptionCode = "overallDepth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
 } }
            }
            };
            list.Add(_oSNd);



            var _OQrE = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体深度(mm)",PageType = "CabinetGroup", OptionCode = "panelDepth", OptionValue = new BaseOptionRangeItem() { DescEn = "700(600+100)", DescZh = "700(600+100)", Code = "700(600+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体深度(mm)",
    OptionCode = "overallDepth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
         new BaseOptionRangeItem() { DescEn = "700", DescZh = "700", Code = "700" },
 } }
            }
            };
            list.Add(_OQrE);



            var _wxPi = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体深度(mm)",PageType = "CabinetGroup", OptionCode = "panelDepth", OptionValue = new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体深度(mm)",
    OptionCode = "overallDepth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
 } }
            }
            };
            list.Add(_wxPi);



            var _HFSY = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体深度(mm)",PageType = "CabinetGroup", OptionCode = "panelDepth", OptionValue = new BaseOptionRangeItem() { DescEn = "900(800+100)", DescZh = "900(800+100)", Code = "900(800+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体深度(mm)",
    OptionCode = "overallDepth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
 } }
            }
            };
            list.Add(_HFSY);



            var _yOMK = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体深度(mm)",PageType = "CabinetGroup", OptionCode = "panelDepth", OptionValue = new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体深度(mm)",
    OptionCode = "overallDepth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
 } }
            }
            };
            list.Add(_yOMK);



            var _sQYe = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体深度(mm)",PageType = "CabinetGroup", OptionCode = "panelDepth", OptionValue = new BaseOptionRangeItem() { DescEn = "1100(1000+100)", DescZh = "1100(1000+100)", Code = "1100(1000+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体深度(mm)",
    OptionCode = "overallDepth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
         new BaseOptionRangeItem() { DescEn = "1100", DescZh = "1100", Code = "1100" },
 } }
            }
            };
            list.Add(_sQYe);



            var _Ldia = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体深度(mm)",PageType = "CabinetGroup", OptionCode = "panelDepth", OptionValue = new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体深度(mm)",
    OptionCode = "overallDepth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
 } }
            }
            };
            list.Add(_Ldia);



            var _iEWC = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体深度(mm)",PageType = "CabinetGroup", OptionCode = "panelDepth", OptionValue = new BaseOptionRangeItem() { DescEn = "1300(1200+100)", DescZh = "1300(1200+100)", Code = "1300(1200+100)" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "整体深度(mm)",
    OptionCode = "overallDepth",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
         new BaseOptionRangeItem() { DescEn = "1300", DescZh = "1300", Code = "1300" },
 } }
            }
            };
            list.Add(_iEWC);



            var _Ghte = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "水平主母线载流(A)",PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "1250", DescZh = "1250", Code = "1250" }  },
   new OptionsItem()
   { OptionNameZh = "通风类型",PageType = "CabinetGroup", OptionCode = "ventilationType", OptionValue = new BaseOptionRangeItem() { DescEn = "Natural ventilation", DescZh = "自然通风", Code = "naturalVentilation" }  },
   new OptionsItem()
   { OptionNameZh = "防护等级",PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem() { DescEn = "IP40", DescZh = "IP40", Code = "ip40" }  },
   new OptionsItem()
   { OptionNameZh = "环境温度(℃)",PageType = "CabinetGroup", OptionCode = "ambientTemperature", OptionValue = new BaseOptionRangeItem() { DescEn = "35", DescZh = "35", Code = "35" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "水平主母线规格(mm)",
    OptionCode = "mainBusbarSize",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "3-80×10+40×10", DescZh = "3-80×10+40×10", Code = "3-80×10+40×10" },
         new BaseOptionRangeItem() { DescEn = "3-80×10+40×10", DescZh = "3-80×10+40×10", Code = "3-80×10+40×10" },
 } }
            }
            };
            list.Add(_Ghte);



            var _ToKK = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "水平主母线载流(A)",PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "1250", DescZh = "1250", Code = "1250" }  },
   new OptionsItem()
   { OptionNameZh = "通风类型",PageType = "CabinetGroup", OptionCode = "ventilationType", OptionValue = new BaseOptionRangeItem() { DescEn = "Forced ventilation", DescZh = "强制通风", Code = "forcedVentilation" }  },
   new OptionsItem()
   { OptionNameZh = "防护等级",PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem() { DescEn = "IP40", DescZh = "IP40", Code = "ip40" }  },
   new OptionsItem()
   { OptionNameZh = "环境温度(℃)",PageType = "CabinetGroup", OptionCode = "ambientTemperature", OptionValue = new BaseOptionRangeItem() { DescEn = "50", DescZh = "50", Code = "50" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "水平主母线规格(mm)",
    OptionCode = "mainBusbarSize",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "3-80×10+40×10", DescZh = "3-80×10+40×10", Code = "3-80×10+40×10" },
         new BaseOptionRangeItem() { DescEn = "3-80×10+40×10", DescZh = "3-80×10+40×10", Code = "3-80×10+40×10" },
 } }
            }
            };
            list.Add(_ToKK);



            var _EHkJ = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "水平主母线载流(A)",PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "1250", DescZh = "1250", Code = "1250" }  },
   new OptionsItem()
   { OptionNameZh = "通风类型",PageType = "CabinetGroup", OptionCode = "ventilationType", OptionValue = new BaseOptionRangeItem() { DescEn = "Natural ventilation", DescZh = "自然通风", Code = "naturalVentilation" }  },
   new OptionsItem()
   { OptionNameZh = "防护等级",PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem() { DescEn = "IP54", DescZh = "IP54", Code = "ip54" }  },
   new OptionsItem()
   { OptionNameZh = "环境温度(℃)",PageType = "CabinetGroup", OptionCode = "ambientTemperature", OptionValue = new BaseOptionRangeItem() { DescEn = "35", DescZh = "35", Code = "35" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "水平主母线规格(mm)",
    OptionCode = "mainBusbarSize",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "3-100×10+50×10", DescZh = "3-100×10+50×10", Code = "3-100×10+50×10" },
         new BaseOptionRangeItem() { DescEn = "3-100×10+50×10", DescZh = "3-100×10+50×10", Code = "3-100×10+50×10" },
 } }
            }
            };
            list.Add(_EHkJ);



            var _jnrf = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "水平主母线载流(A)",PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "1250", DescZh = "1250", Code = "1250" }  },
   new OptionsItem()
   { OptionNameZh = "通风类型",PageType = "CabinetGroup", OptionCode = "ventilationType", OptionValue = new BaseOptionRangeItem() { DescEn = "Natural ventilation", DescZh = "自然通风", Code = "naturalVentilation" }  },
   new OptionsItem()
   { OptionNameZh = "防护等级",PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem() { DescEn = "IP54", DescZh = "IP54", Code = "ip54" }  },
   new OptionsItem()
   { OptionNameZh = "环境温度(℃)",PageType = "CabinetGroup", OptionCode = "ambientTemperature", OptionValue = new BaseOptionRangeItem() { DescEn = "50", DescZh = "50", Code = "50" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "水平主母线规格(mm)",
    OptionCode = "mainBusbarSize",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "3-100×10+50×10", DescZh = "3-100×10+50×10", Code = "3-100×10+50×10" },
         new BaseOptionRangeItem() { DescEn = "3-100×10+50×10", DescZh = "3-100×10+50×10", Code = "3-100×10+50×10" },
 } }
            }
            };
            list.Add(_jnrf);



            var _yewj = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "水平主母线载流(A)",PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "1250", DescZh = "1250", Code = "1250" }  },
   new OptionsItem()
   { OptionNameZh = "通风类型",PageType = "CabinetGroup", OptionCode = "ventilationType", OptionValue = new BaseOptionRangeItem() { DescEn = "Forced ventilation", DescZh = "强制通风", Code = "forcedVentilation" }  },
   new OptionsItem()
   { OptionNameZh = "防护等级",PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem() { DescEn = "IP54", DescZh = "IP54", Code = "ip54" }  },
   new OptionsItem()
   { OptionNameZh = "环境温度(℃)",PageType = "CabinetGroup", OptionCode = "ambientTemperature", OptionValue = new BaseOptionRangeItem() { DescEn = "35", DescZh = "35", Code = "35" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "水平主母线规格(mm)",
    OptionCode = "mainBusbarSize",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "3-80×10+40×10", DescZh = "3-80×10+40×10", Code = "3-80×10+40×10" },
         new BaseOptionRangeItem() { DescEn = "3-80×10+40×10", DescZh = "3-80×10+40×10", Code = "3-80×10+40×10" },
 } }
            }
            };
            list.Add(_yewj);



            var _HCBI = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "水平主母线载流(A)",PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "1250", DescZh = "1250", Code = "1250" }  },
   new OptionsItem()
   { OptionNameZh = "通风类型",PageType = "CabinetGroup", OptionCode = "ventilationType", OptionValue = new BaseOptionRangeItem() { DescEn = "Forced ventilation", DescZh = "强制通风", Code = "forcedVentilation" }  },
   new OptionsItem()
   { OptionNameZh = "防护等级",PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem() { DescEn = "IP54", DescZh = "IP54", Code = "ip54" }  },
   new OptionsItem()
   { OptionNameZh = "环境温度(℃)",PageType = "CabinetGroup", OptionCode = "ambientTemperature", OptionValue = new BaseOptionRangeItem() { DescEn = "50", DescZh = "50", Code = "50" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "水平主母线规格(mm)",
    OptionCode = "mainBusbarSize",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "3-80×10+40×10", DescZh = "3-80×10+40×10", Code = "3-80×10+40×10" },
         new BaseOptionRangeItem() { DescEn = "3-80×10+40×10", DescZh = "3-80×10+40×10", Code = "3-80×10+40×10" },
 } }
            }
            };
            list.Add(_HCBI);



            var _boQQ = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "水平主母线载流(A)",PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" }  },
   new OptionsItem()
   { OptionNameZh = "通风类型",PageType = "CabinetGroup", OptionCode = "ventilationType", OptionValue = new BaseOptionRangeItem() { DescEn = "Natural ventilation", DescZh = "自然通风", Code = "naturalVentilation" }  },
   new OptionsItem()
   { OptionNameZh = "防护等级",PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem() { DescEn = "IP40", DescZh = "IP40", Code = "ip40" }  },
   new OptionsItem()
   { OptionNameZh = "环境温度(℃)",PageType = "CabinetGroup", OptionCode = "ambientTemperature", OptionValue = new BaseOptionRangeItem() { DescEn = "35", DescZh = "35", Code = "35" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "水平主母线规格(mm)",
    OptionCode = "mainBusbarSize",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "3-100×10+50×10", DescZh = "3-100×10+50×10", Code = "3-100×10+50×10" },
         new BaseOptionRangeItem() { DescEn = "3-100×10+50×10", DescZh = "3-100×10+50×10", Code = "3-100×10+50×10" },
 } }
            }
            };
            list.Add(_boQQ);



            var _GhbV = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "水平主母线载流(A)",PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" }  },
   new OptionsItem()
   { OptionNameZh = "通风类型",PageType = "CabinetGroup", OptionCode = "ventilationType", OptionValue = new BaseOptionRangeItem() { DescEn = "Forced ventilation", DescZh = "强制通风", Code = "forcedVentilation" }  },
   new OptionsItem()
   { OptionNameZh = "防护等级",PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem() { DescEn = "IP40", DescZh = "IP40", Code = "ip40" }  },
   new OptionsItem()
   { OptionNameZh = "环境温度(℃)",PageType = "CabinetGroup", OptionCode = "ambientTemperature", OptionValue = new BaseOptionRangeItem() { DescEn = "50", DescZh = "50", Code = "50" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "水平主母线规格(mm)",
    OptionCode = "mainBusbarSize",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "3-100×10+50×10", DescZh = "3-100×10+50×10", Code = "3-100×10+50×10" },
         new BaseOptionRangeItem() { DescEn = "3-100×10+50×10", DescZh = "3-100×10+50×10", Code = "3-100×10+50×10" },
 } }
            }
            };
            list.Add(_GhbV);



            var _jjTB = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "水平主母线载流(A)",PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" }  },
   new OptionsItem()
   { OptionNameZh = "通风类型",PageType = "CabinetGroup", OptionCode = "ventilationType", OptionValue = new BaseOptionRangeItem() { DescEn = "Forced ventilation", DescZh = "强制通风", Code = "forcedVentilation" }  },
   new OptionsItem()
   { OptionNameZh = "防护等级",PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem() { DescEn = "IP54", DescZh = "IP54", Code = "ip54" }  },
   new OptionsItem()
   { OptionNameZh = "环境温度(℃)",PageType = "CabinetGroup", OptionCode = "ambientTemperature", OptionValue = new BaseOptionRangeItem() { DescEn = "35", DescZh = "35", Code = "35" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "水平主母线规格(mm)",
    OptionCode = "mainBusbarSize",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "3-100×10+50×10", DescZh = "3-100×10+50×10", Code = "3-100×10+50×10" },
         new BaseOptionRangeItem() { DescEn = "3-100×10+50×10", DescZh = "3-100×10+50×10", Code = "3-100×10+50×10" },
 } }
            }
            };
            list.Add(_jjTB);



            var _yLaN = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "水平主母线载流(A)",PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" }  },
   new OptionsItem()
   { OptionNameZh = "通风类型",PageType = "CabinetGroup", OptionCode = "ventilationType", OptionValue = new BaseOptionRangeItem() { DescEn = "Forced ventilation", DescZh = "强制通风", Code = "forcedVentilation" }  },
   new OptionsItem()
   { OptionNameZh = "防护等级",PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem() { DescEn = "IP54", DescZh = "IP54", Code = "ip54" }  },
   new OptionsItem()
   { OptionNameZh = "环境温度(℃)",PageType = "CabinetGroup", OptionCode = "ambientTemperature", OptionValue = new BaseOptionRangeItem() { DescEn = "50", DescZh = "50", Code = "50" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "水平主母线规格(mm)",
    OptionCode = "mainBusbarSize",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "3-100×10+50×10", DescZh = "3-100×10+50×10", Code = "3-100×10+50×10" },
         new BaseOptionRangeItem() { DescEn = "3-100×10+50×10", DescZh = "3-100×10+50×10", Code = "3-100×10+50×10" },
 } }
            }
            };
            list.Add(_yLaN);



            var _rFWe = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "水平主母线载流(A)",PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "2000", DescZh = "2000", Code = "2000" }  },
   new OptionsItem()
   { OptionNameZh = "通风类型",PageType = "CabinetGroup", OptionCode = "ventilationType", OptionValue = new BaseOptionRangeItem() { DescEn = "Natural ventilation", DescZh = "自然通风", Code = "naturalVentilation" }  },
   new OptionsItem()
   { OptionNameZh = "防护等级",PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem() { DescEn = "IP40", DescZh = "IP40", Code = "ip40" }  },
   new OptionsItem()
   { OptionNameZh = "环境温度(℃)",PageType = "CabinetGroup", OptionCode = "ambientTemperature", OptionValue = new BaseOptionRangeItem() { DescEn = "35", DescZh = "35", Code = "35" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "水平主母线规格(mm)",
    OptionCode = "mainBusbarSize",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "3-2×60×10+60×10", DescZh = "3-2×60×10+60×10", Code = "3-2×60×10+60×10" },
         new BaseOptionRangeItem() { DescEn = "3-2×60×10+60×10", DescZh = "3-2×60×10+60×10", Code = "3-2×60×10+60×10" },
         new BaseOptionRangeItem() { DescEn = "3-120×10+60×10", DescZh = "3-120×10+60×10", Code = "3-120×10+60×10" },
         new BaseOptionRangeItem() { DescEn = "3-120×10+60×10", DescZh = "3-120×10+60×10", Code = "3-120×10+60×10" },
 } }
            }
            };
            list.Add(_rFWe);



            var _lpHs = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "水平主母线载流(A)",PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "2000", DescZh = "2000", Code = "2000" }  },
   new OptionsItem()
   { OptionNameZh = "通风类型",PageType = "CabinetGroup", OptionCode = "ventilationType", OptionValue = new BaseOptionRangeItem() { DescEn = "Forced ventilation", DescZh = "强制通风", Code = "forcedVentilation" }  },
   new OptionsItem()
   { OptionNameZh = "防护等级",PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem() { DescEn = "IP40", DescZh = "IP40", Code = "ip40" }  },
   new OptionsItem()
   { OptionNameZh = "环境温度(℃)",PageType = "CabinetGroup", OptionCode = "ambientTemperature", OptionValue = new BaseOptionRangeItem() { DescEn = "50", DescZh = "50", Code = "50" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "水平主母线规格(mm)",
    OptionCode = "mainBusbarSize",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "3-2×60×10+60×10", DescZh = "3-2×60×10+60×10", Code = "3-2×60×10+60×10" },
         new BaseOptionRangeItem() { DescEn = "3-2×60×10+60×10", DescZh = "3-2×60×10+60×10", Code = "3-2×60×10+60×10" },
         new BaseOptionRangeItem() { DescEn = "3-120×10+60×10", DescZh = "3-120×10+60×10", Code = "3-120×10+60×10" },
         new BaseOptionRangeItem() { DescEn = "3-120×10+60×10", DescZh = "3-120×10+60×10", Code = "3-120×10+60×10" },
 } }
            }
            };
            list.Add(_lpHs);



            var _IvKy = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "水平主母线载流(A)",PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "2000", DescZh = "2000", Code = "2000" }  },
   new OptionsItem()
   { OptionNameZh = "通风类型",PageType = "CabinetGroup", OptionCode = "ventilationType", OptionValue = new BaseOptionRangeItem() { DescEn = "Forced ventilation", DescZh = "强制通风", Code = "forcedVentilation" }  },
   new OptionsItem()
   { OptionNameZh = "防护等级",PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem() { DescEn = "IP54", DescZh = "IP54", Code = "ip54" }  },
   new OptionsItem()
   { OptionNameZh = "环境温度(℃)",PageType = "CabinetGroup", OptionCode = "ambientTemperature", OptionValue = new BaseOptionRangeItem() { DescEn = "35", DescZh = "35", Code = "35" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "水平主母线规格(mm)",
    OptionCode = "mainBusbarSize",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "3-2×60×10+60×10", DescZh = "3-2×60×10+60×10", Code = "3-2×60×10+60×10" },
         new BaseOptionRangeItem() { DescEn = "3-2×60×10+60×10", DescZh = "3-2×60×10+60×10", Code = "3-2×60×10+60×10" },
         new BaseOptionRangeItem() { DescEn = "3-120×10+60×10", DescZh = "3-120×10+60×10", Code = "3-120×10+60×10" },
         new BaseOptionRangeItem() { DescEn = "3-120×10+60×10", DescZh = "3-120×10+60×10", Code = "3-120×10+60×10" },
 } }
            }
            };
            list.Add(_IvKy);



            var _bAdF = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "水平主母线载流(A)",PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "2000", DescZh = "2000", Code = "2000" }  },
   new OptionsItem()
   { OptionNameZh = "通风类型",PageType = "CabinetGroup", OptionCode = "ventilationType", OptionValue = new BaseOptionRangeItem() { DescEn = "Forced ventilation", DescZh = "强制通风", Code = "forcedVentilation" }  },
   new OptionsItem()
   { OptionNameZh = "防护等级",PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem() { DescEn = "IP54", DescZh = "IP54", Code = "ip54" }  },
   new OptionsItem()
   { OptionNameZh = "环境温度(℃)",PageType = "CabinetGroup", OptionCode = "ambientTemperature", OptionValue = new BaseOptionRangeItem() { DescEn = "50", DescZh = "50", Code = "50" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "水平主母线规格(mm)",
    OptionCode = "mainBusbarSize",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "3-2×60×10+60×10", DescZh = "3-2×60×10+60×10", Code = "3-2×60×10+60×10" },
         new BaseOptionRangeItem() { DescEn = "3-2×60×10+60×10", DescZh = "3-2×60×10+60×10", Code = "3-2×60×10+60×10" },
         new BaseOptionRangeItem() { DescEn = "3-120×10+60×10", DescZh = "3-120×10+60×10", Code = "3-120×10+60×10" },
         new BaseOptionRangeItem() { DescEn = "3-120×10+60×10", DescZh = "3-120×10+60×10", Code = "3-120×10+60×10" },
 } }
            }
            };
            list.Add(_bAdF);



            var _KPbc = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "水平主母线载流(A)",PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "3200", DescZh = "3200", Code = "3200" }  },
   new OptionsItem()
   { OptionNameZh = "通风类型",PageType = "CabinetGroup", OptionCode = "ventilationType", OptionValue = new BaseOptionRangeItem() { DescEn = "Natural ventilation", DescZh = "自然通风", Code = "naturalVentilation" }  },
   new OptionsItem()
   { OptionNameZh = "防护等级",PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem() { DescEn = "IP40", DescZh = "IP40", Code = "ip40" }  },
   new OptionsItem()
   { OptionNameZh = "环境温度(℃)",PageType = "CabinetGroup", OptionCode = "ambientTemperature", OptionValue = new BaseOptionRangeItem() { DescEn = "35", DescZh = "35", Code = "35" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "水平主母线规格(mm)",
    OptionCode = "mainBusbarSize",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "3-3×80×10+120×10", DescZh = "3-3×80×10+120×10", Code = "3-3×80×10+120×10" },
         new BaseOptionRangeItem() { DescEn = "3-3×80×10+120×10", DescZh = "3-3×80×10+120×10", Code = "3-3×80×10+120×10" },
         new BaseOptionRangeItem() { DescEn = "3-2×120×10+120×10", DescZh = "3-2×120×10+120×10", Code = "3-2×120×10+120×10" },
         new BaseOptionRangeItem() { DescEn = "3-2×120×10+120×10", DescZh = "3-2×120×10+120×10", Code = "3-2×120×10+120×10" },
 } }
            }
            };
            list.Add(_KPbc);



            var _Uicv = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "水平主母线载流(A)",PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "3200", DescZh = "3200", Code = "3200" }  },
   new OptionsItem()
   { OptionNameZh = "通风类型",PageType = "CabinetGroup", OptionCode = "ventilationType", OptionValue = new BaseOptionRangeItem() { DescEn = "Forced ventilation", DescZh = "强制通风", Code = "forcedVentilation" }  },
   new OptionsItem()
   { OptionNameZh = "防护等级",PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem() { DescEn = "IP40", DescZh = "IP40", Code = "ip40" }  },
   new OptionsItem()
   { OptionNameZh = "环境温度(℃)",PageType = "CabinetGroup", OptionCode = "ambientTemperature", OptionValue = new BaseOptionRangeItem() { DescEn = "50", DescZh = "50", Code = "50" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "水平主母线规格(mm)",
    OptionCode = "mainBusbarSize",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "3-3×80×10+120×10", DescZh = "3-3×80×10+120×10", Code = "3-3×80×10+120×10" },
         new BaseOptionRangeItem() { DescEn = "3-3×80×10+120×10", DescZh = "3-3×80×10+120×10", Code = "3-3×80×10+120×10" },
         new BaseOptionRangeItem() { DescEn = "3-2×120×10+120×10", DescZh = "3-2×120×10+120×10", Code = "3-2×120×10+120×10" },
         new BaseOptionRangeItem() { DescEn = "3-2×120×10+120×10", DescZh = "3-2×120×10+120×10", Code = "3-2×120×10+120×10" },
 } }
            }
            };
            list.Add(_Uicv);



            var _ipnU = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "水平主母线载流(A)",PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "3200", DescZh = "3200", Code = "3200" }  },
   new OptionsItem()
   { OptionNameZh = "通风类型",PageType = "CabinetGroup", OptionCode = "ventilationType", OptionValue = new BaseOptionRangeItem() { DescEn = "Forced ventilation", DescZh = "强制通风", Code = "forcedVentilation" }  },
   new OptionsItem()
   { OptionNameZh = "防护等级",PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem() { DescEn = "IP54", DescZh = "IP54", Code = "ip54" }  },
   new OptionsItem()
   { OptionNameZh = "环境温度(℃)",PageType = "CabinetGroup", OptionCode = "ambientTemperature", OptionValue = new BaseOptionRangeItem() { DescEn = "35", DescZh = "35", Code = "35" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "水平主母线规格(mm)",
    OptionCode = "mainBusbarSize",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "3-3×80×10+120×10", DescZh = "3-3×80×10+120×10", Code = "3-3×80×10+120×10" },
         new BaseOptionRangeItem() { DescEn = "3-3×80×10+120×10", DescZh = "3-3×80×10+120×10", Code = "3-3×80×10+120×10" },
         new BaseOptionRangeItem() { DescEn = "3-2×120×10+120×10", DescZh = "3-2×120×10+120×10", Code = "3-2×120×10+120×10" },
         new BaseOptionRangeItem() { DescEn = "3-2×120×10+120×10", DescZh = "3-2×120×10+120×10", Code = "3-2×120×10+120×10" },
 } }
            }
            };
            list.Add(_ipnU);



            var _Dzqa = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "水平主母线载流(A)",PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "3200", DescZh = "3200", Code = "3200" }  },
   new OptionsItem()
   { OptionNameZh = "通风类型",PageType = "CabinetGroup", OptionCode = "ventilationType", OptionValue = new BaseOptionRangeItem() { DescEn = "Forced ventilation", DescZh = "强制通风", Code = "forcedVentilation" }  },
   new OptionsItem()
   { OptionNameZh = "防护等级",PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem() { DescEn = "IP54", DescZh = "IP54", Code = "ip54" }  },
   new OptionsItem()
   { OptionNameZh = "环境温度(℃)",PageType = "CabinetGroup", OptionCode = "ambientTemperature", OptionValue = new BaseOptionRangeItem() { DescEn = "50", DescZh = "50", Code = "50" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "水平主母线规格(mm)",
    OptionCode = "mainBusbarSize",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "3-3×80×10+120×10", DescZh = "3-3×80×10+120×10", Code = "3-3×80×10+120×10" },
         new BaseOptionRangeItem() { DescEn = "3-3×80×10+120×10", DescZh = "3-3×80×10+120×10", Code = "3-3×80×10+120×10" },
         new BaseOptionRangeItem() { DescEn = "3-2×120×10+120×10", DescZh = "3-2×120×10+120×10", Code = "3-2×120×10+120×10" },
         new BaseOptionRangeItem() { DescEn = "3-2×120×10+120×10", DescZh = "3-2×120×10+120×10", Code = "3-2×120×10+120×10" },
 } }
            }
            };
            list.Add(_Dzqa);



            var _wxvF = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "水平主母线载流(A)",PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "4000", DescZh = "4000", Code = "4000" }  },
   new OptionsItem()
   { OptionNameZh = "通风类型",PageType = "CabinetGroup", OptionCode = "ventilationType", OptionValue = new BaseOptionRangeItem() { DescEn = "Natural ventilation", DescZh = "自然通风", Code = "naturalVentilation" }  },
   new OptionsItem()
   { OptionNameZh = "防护等级",PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem() { DescEn = "IP40", DescZh = "IP40", Code = "ip40" }  },
   new OptionsItem()
   { OptionNameZh = "环境温度(℃)",PageType = "CabinetGroup", OptionCode = "ambientTemperature", OptionValue = new BaseOptionRangeItem() { DescEn = "35", DescZh = "35", Code = "35" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "水平主母线规格(mm)",
    OptionCode = "mainBusbarSize",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "3-3×100×10+2×80×10", DescZh = "3-3×100×10+2×80×10", Code = "3-3×100×10+2×80×10" },
         new BaseOptionRangeItem() { DescEn = "3-3×100×10+2×80×10", DescZh = "3-3×100×10+2×80×10", Code = "3-3×100×10+2×80×10" },
 } }
            }
            };
            list.Add(_wxvF);



            var _xhwM = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "水平主母线载流(A)",PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "4000", DescZh = "4000", Code = "4000" }  },
   new OptionsItem()
   { OptionNameZh = "通风类型",PageType = "CabinetGroup", OptionCode = "ventilationType", OptionValue = new BaseOptionRangeItem() { DescEn = "Forced ventilation", DescZh = "强制通风", Code = "forcedVentilation" }  },
   new OptionsItem()
   { OptionNameZh = "防护等级",PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem() { DescEn = "IP40", DescZh = "IP40", Code = "ip40" }  },
   new OptionsItem()
   { OptionNameZh = "环境温度(℃)",PageType = "CabinetGroup", OptionCode = "ambientTemperature", OptionValue = new BaseOptionRangeItem() { DescEn = "50", DescZh = "50", Code = "50" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "水平主母线规格(mm)",
    OptionCode = "mainBusbarSize",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "3-3×100×10+2×80×10", DescZh = "3-3×100×10+2×80×10", Code = "3-3×100×10+2×80×10" },
         new BaseOptionRangeItem() { DescEn = "3-3×100×10+2×80×10", DescZh = "3-3×100×10+2×80×10", Code = "3-3×100×10+2×80×10" },
 } }
            }
            };
            list.Add(_xhwM);



            var _ZuVU = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "水平主母线载流(A)",PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "4000", DescZh = "4000", Code = "4000" }  },
   new OptionsItem()
   { OptionNameZh = "通风类型",PageType = "CabinetGroup", OptionCode = "ventilationType", OptionValue = new BaseOptionRangeItem() { DescEn = "Forced ventilation", DescZh = "强制通风", Code = "forcedVentilation" }  },
   new OptionsItem()
   { OptionNameZh = "防护等级",PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem() { DescEn = "IP54", DescZh = "IP54", Code = "ip54" }  },
   new OptionsItem()
   { OptionNameZh = "环境温度(℃)",PageType = "CabinetGroup", OptionCode = "ambientTemperature", OptionValue = new BaseOptionRangeItem() { DescEn = "35", DescZh = "35", Code = "35" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "水平主母线规格(mm)",
    OptionCode = "mainBusbarSize",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "3-3×100×10+2×80×10", DescZh = "3-3×100×10+2×80×10", Code = "3-3×100×10+2×80×10" },
         new BaseOptionRangeItem() { DescEn = "3-3×100×10+2×80×10", DescZh = "3-3×100×10+2×80×10", Code = "3-3×100×10+2×80×10" },
 } }
            }
            };
            list.Add(_ZuVU);



            var _smdq = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "水平主母线载流(A)",PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "4000", DescZh = "4000", Code = "4000" }  },
   new OptionsItem()
   { OptionNameZh = "通风类型",PageType = "CabinetGroup", OptionCode = "ventilationType", OptionValue = new BaseOptionRangeItem() { DescEn = "Forced ventilation", DescZh = "强制通风", Code = "forcedVentilation" }  },
   new OptionsItem()
   { OptionNameZh = "防护等级",PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem() { DescEn = "IP54", DescZh = "IP54", Code = "ip54" }  },
   new OptionsItem()
   { OptionNameZh = "环境温度(℃)",PageType = "CabinetGroup", OptionCode = "ambientTemperature", OptionValue = new BaseOptionRangeItem() { DescEn = "50", DescZh = "50", Code = "50" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "水平主母线规格(mm)",
    OptionCode = "mainBusbarSize",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "3-3×100×10+2×80×10", DescZh = "3-3×100×10+2×80×10", Code = "3-3×100×10+2×80×10" },
         new BaseOptionRangeItem() { DescEn = "3-3×100×10+2×80×10", DescZh = "3-3×100×10+2×80×10", Code = "3-3×100×10+2×80×10" },
 } }
            }
            };
            list.Add(_smdq);



            var _RPPW = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "水平主母线短时耐受",PageType = "CabinetGroup", OptionCode = "mainbusbarIcw", OptionValue = new BaseOptionRangeItem() { DescEn = "25kA/1S", DescZh = "25kA/1S", Code = "25kA/1S" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "水平主母线短时耐受",
    OptionCode = "mainBusbarIcw",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "25kA/1S", DescZh = "25kA/1S", Code = "25kA/1S" },
 } }
            }
            };
            list.Add(_RPPW);



            var _fxEA = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "水平主母线短时耐受",PageType = "CabinetGroup", OptionCode = "mainbusbarIcw", OptionValue = new BaseOptionRangeItem() { DescEn = "36kA/1S", DescZh = "36kA/1S", Code = "36kA/1S" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "水平主母线短时耐受",
    OptionCode = "mainBusbarIcw",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "36kA/1S", DescZh = "36kA/1S", Code = "36kA/1S" },
 } }
            }
            };
            list.Add(_fxEA);



            var _tEkA = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "水平主母线短时耐受",PageType = "CabinetGroup", OptionCode = "mainbusbarIcw", OptionValue = new BaseOptionRangeItem() { DescEn = "50kA/1S", DescZh = "50kA/1S", Code = "50kA/1S" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "水平主母线短时耐受",
    OptionCode = "mainBusbarIcw",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "50kA/1S", DescZh = "50kA/1S", Code = "50kA/1S" },
 } }
            }
            };
            list.Add(_tEkA);



            var _KkFx = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "水平主母线短时耐受",PageType = "CabinetGroup", OptionCode = "mainbusbarIcw", OptionValue = new BaseOptionRangeItem() { DescEn = "65kA/1S", DescZh = "65kA/1S", Code = "65kA/1S" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "水平主母线短时耐受",
    OptionCode = "mainBusbarIcw",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "65kA/1S", DescZh = "65kA/1S", Code = "65kA/1S" },
 } }
            }
            };
            list.Add(_KkFx);



            var _vUSd = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "水平主母线短时耐受",PageType = "CabinetGroup", OptionCode = "mainbusbarIcw", OptionValue = new BaseOptionRangeItem() { DescEn = "85kA/1S", DescZh = "85kA/1S", Code = "85kA/1S" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "水平主母线短时耐受",
    OptionCode = "mainBusbarIcw",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "85kA/1S", DescZh = "85kA/1S", Code = "85kA/1S" },
 } }
            }
            };
            list.Add(_vUSd);



            var _IJkr = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "水平主母线短时耐受",PageType = "CabinetGroup", OptionCode = "mainbusbarIcw", OptionValue = new BaseOptionRangeItem() { DescEn = "100kA/1S", DescZh = "100kA/1S", Code = "100kA/1S" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "水平主母线短时耐受",
    OptionCode = "mainBusbarIcw",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "100kA/1S", DescZh = "100kA/1S", Code = "100kA/1S" },
 } }
            }
            };
            list.Add(_IJkr);



            var _QYsv = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "水平主母线短时耐受",PageType = "CabinetGroup", OptionCode = "mainbusbarIcw", OptionValue = new BaseOptionRangeItem() { DescEn = "25kA/3S", DescZh = "25kA/3S", Code = "25kA/3S" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "水平主母线短时耐受",
    OptionCode = "mainBusbarIcw",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "25kA/3S", DescZh = "25kA/3S", Code = "25kA/3S" },
 } }
            }
            };
            list.Add(_QYsv);



            var _WFaZ = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "水平主母线短时耐受",PageType = "CabinetGroup", OptionCode = "mainbusbarIcw", OptionValue = new BaseOptionRangeItem() { DescEn = "36kA/3S", DescZh = "36kA/3S", Code = "36kA/3S" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "水平主母线短时耐受",
    OptionCode = "mainBusbarIcw",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "36kA/3S", DescZh = "36kA/3S", Code = "36kA/3S" },
 } }
            }
            };
            list.Add(_WFaZ);



            var _undy = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "水平主母线短时耐受",PageType = "CabinetGroup", OptionCode = "mainbusbarIcw", OptionValue = new BaseOptionRangeItem() { DescEn = "50kA/3S", DescZh = "50kA/3S", Code = "50kA/3S" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "水平主母线短时耐受",
    OptionCode = "mainBusbarIcw",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "50kA/3S", DescZh = "50kA/3S", Code = "50kA/3S" },
 } }
            }
            };
            list.Add(_undy);



            var _lGjd = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "水平主母线短时耐受",PageType = "CabinetGroup", OptionCode = "mainbusbarIcw", OptionValue = new BaseOptionRangeItem() { DescEn = "65kA/3S", DescZh = "65kA/3S", Code = "65kA/3S" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "水平主母线短时耐受",
    OptionCode = "mainBusbarIcw",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "65kA/3S", DescZh = "65kA/3S", Code = "65kA/3S" },
 } }
            }
            };
            list.Add(_lGjd);



            var _HHge = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "水平主母线短时耐受",PageType = "CabinetGroup", OptionCode = "mainbusbarIcw", OptionValue = new BaseOptionRangeItem() { DescEn = "100kA/3S", DescZh = "100kA/3S", Code = "100kA/3S" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "水平主母线短时耐受",
    OptionCode = "mainBusbarIcw",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "100kA/3S", DescZh = "100kA/3S", Code = "100kA/3S" },
 } }
            }
            };
            list.Add(_HHge);



            var _JUZt = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "水平主母线短时耐受",PageType = "CabinetGroup", OptionCode = "mainbusbarIcw", OptionValue = new BaseOptionRangeItem() { DescEn = "105kA/3S", DescZh = "105kA/3S", Code = "105kA/3S" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "水平主母线短时耐受",
    OptionCode = "mainBusbarIcw",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "105kA/3S", DescZh = "105kA/3S", Code = "105kA/3S" },
 } }
            }
            };
            list.Add(_JUZt);



            var _RlKL = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "通风类型",PageType = "CabinetGroup", OptionCode = "ventilationType", OptionValue = new BaseOptionRangeItem() { DescEn = "Natural ventilation", DescZh = "自然通风", Code = "naturalVentilation" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "通风类型",
    OptionCode = "ventilationType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "NaturalVentilation", DescZh = "自然通风", Code = "naturalVentilation" },
         new BaseOptionRangeItem() { DescEn = "NaturalVentilation", DescZh = "自然通风", Code = "naturalVentilation" },
         new BaseOptionRangeItem() { DescEn = "NaturalVentilation", DescZh = "自然通风", Code = "naturalVentilation" },
         new BaseOptionRangeItem() { DescEn = "NaturalVentilation", DescZh = "自然通风", Code = "naturalVentilation" },
 } }
            }
            };
            list.Add(_RlKL);



            var _Xdcx = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "通风类型",PageType = "CabinetGroup", OptionCode = "ventilationType", OptionValue = new BaseOptionRangeItem() { DescEn = "Forced ventilation", DescZh = "强制通风", Code = "forcedVentilation" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "通风类型",
    OptionCode = "ventilationType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "ForcedVentilation", DescZh = "强制通风", Code = "forcedVentilation" },
         new BaseOptionRangeItem() { DescEn = "ForcedVentilation", DescZh = "强制通风", Code = "forcedVentilation" },
         new BaseOptionRangeItem() { DescEn = "ForcedVentilation", DescZh = "强制通风", Code = "forcedVentilation" },
         new BaseOptionRangeItem() { DescEn = "ForcedVentilation", DescZh = "强制通风", Code = "forcedVentilation" },
 } }
            }
            };
            list.Add(_Xdcx);



            var _TxTZ = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "水平主母线载流(A)",PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "7500", DescZh = "7500", Code = "7500" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "水平主母线载流(A)",
    OptionCode = "mainBusbarCurrent",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "7500", DescZh = "7500", Code = "7500" },
         new BaseOptionRangeItem() { DescEn = "7500", DescZh = "7500", Code = "7500" },
 } }
            }
            };
            list.Add(_TxTZ);



            var _rQQE = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "水平主母线载流(A)",PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "6300", DescZh = "6300", Code = "6300" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "水平主母线载流(A)",
    OptionCode = "mainBusbarCurrent",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "6300", DescZh = "6300", Code = "6300" },
         new BaseOptionRangeItem() { DescEn = "6300", DescZh = "6300", Code = "6300" },
 } }
            }
            };
            list.Add(_rQQE);



            var _jppE = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "水平主母线载流(A)",PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "5000", DescZh = "5000", Code = "5000" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "水平主母线载流(A)",
    OptionCode = "mainBusbarCurrent",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "5000", DescZh = "5000", Code = "5000" },
         new BaseOptionRangeItem() { DescEn = "5000", DescZh = "5000", Code = "5000" },
 } }
            }
            };
            list.Add(_jppE);



            var _dhjA = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "水平主母线载流(A)",PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "4000", DescZh = "4000", Code = "4000" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "水平主母线载流(A)",
    OptionCode = "mainBusbarCurrent",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "4000", DescZh = "4000", Code = "4000" },
         new BaseOptionRangeItem() { DescEn = "4000", DescZh = "4000", Code = "4000" },
 } }
            }
            };
            list.Add(_dhjA);



            var _xFJR = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "水平主母线载流(A)",PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "3200", DescZh = "3200", Code = "3200" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "水平主母线载流(A)",
    OptionCode = "mainBusbarCurrent",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "3200", DescZh = "3200", Code = "3200" },
         new BaseOptionRangeItem() { DescEn = "3200", DescZh = "3200", Code = "3200" },
 } }
            }
            };
            list.Add(_xFJR);



            var _fhdR = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "水平主母线载流(A)",PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "2500", DescZh = "2500", Code = "2500" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "水平主母线载流(A)",
    OptionCode = "mainBusbarCurrent",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "2500", DescZh = "2500", Code = "2500" },
         new BaseOptionRangeItem() { DescEn = "2500", DescZh = "2500", Code = "2500" },
 } }
            }
            };
            list.Add(_fhdR);



            var _DeSq = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "水平主母线载流(A)",PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "2000", DescZh = "2000", Code = "2000" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "水平主母线载流(A)",
    OptionCode = "mainBusbarCurrent",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "2000", DescZh = "2000", Code = "2000" },
         new BaseOptionRangeItem() { DescEn = "2000", DescZh = "2000", Code = "2000" },
 } }
            }
            };
            list.Add(_DeSq);



            var _dXyA = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "水平主母线载流(A)",PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "水平主母线载流(A)",
    OptionCode = "mainBusbarCurrent",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" },
         new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" },
 } }
            }
            };
            list.Add(_dXyA);



            var _wqCc = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "水平主母线载流(A)",PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "1250", DescZh = "1250", Code = "1250" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "水平主母线载流(A)",
    OptionCode = "mainBusbarCurrent",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1250", DescZh = "1250", Code = "1250" },
         new BaseOptionRangeItem() { DescEn = "1250", DescZh = "1250", Code = "1250" },
 } }
            }
            };
            list.Add(_wqCc);



            var _JgrY = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "水平主母线载流(A)",PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "水平主母线载流(A)",
    OptionCode = "mainBusbarCurrent",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
 } }
            }
            };
            list.Add(_JgrY);



            var _qvVG = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "水平主母线载流(A)",PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "水平主母线载流(A)",
    OptionCode = "mainBusbarCurrent",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
 } }
            }
            };
            list.Add(_qvVG);



            var _ARur = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "水平主母线载流(A)",PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "630", DescZh = "630", Code = "630" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "水平主母线载流(A)",
    OptionCode = "mainBusbarCurrent",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "630", DescZh = "630", Code = "630" },
         new BaseOptionRangeItem() { DescEn = "630", DescZh = "630", Code = "630" },
 } }
            }
            };
            list.Add(_ARur);



            var _zOpI = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体高度(mm)",PageType = "CabinetGroup", OptionCode = "panelHeight", OptionValue = new BaseOptionRangeItem() { DescEn = "2000", DescZh = "2000", Code = "2000" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "柜体高度(mm)",
    OptionCode = "panelHeight",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "2000", DescZh = "2000", Code = "2000" },
         new BaseOptionRangeItem() { DescEn = "2000", DescZh = "2000", Code = "2000" },
 } }
            }
            };
            list.Add(_zOpI);



            var _QoNi = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体高度(mm)",PageType = "CabinetGroup", OptionCode = "panelHeight", OptionValue = new BaseOptionRangeItem() { DescEn = "2200", DescZh = "2200", Code = "2200" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "柜体高度(mm)",
    OptionCode = "panelHeight",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "2200", DescZh = "2200", Code = "2200" },
         new BaseOptionRangeItem() { DescEn = "2200", DescZh = "2200", Code = "2200" },
 } }
            }
            };
            list.Add(_QoNi);



            var _Cffn = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体高度(mm)",PageType = "CabinetGroup", OptionCode = "panelHeight", OptionValue = new BaseOptionRangeItem() { DescEn = "2300", DescZh = "2300", Code = "2300" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "柜体高度(mm)",
    OptionCode = "panelHeight",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "2300", DescZh = "2300", Code = "2300" },
         new BaseOptionRangeItem() { DescEn = "2300", DescZh = "2300", Code = "2300" },
 } }
            }
            };
            list.Add(_Cffn);



            var _NuRI = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜体高度(mm)",PageType = "CabinetGroup", OptionCode = "panelHeight", OptionValue = new BaseOptionRangeItem() { DescEn = "2400", DescZh = "2400", Code = "2400" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "柜体高度(mm)",
    OptionCode = "panelHeight",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "2400", DescZh = "2400", Code = "2400" },
         new BaseOptionRangeItem() { DescEn = "2400", DescZh = "2400", Code = "2400" },
 } }
            }
            };
            list.Add(_NuRI);



            var _bTLN = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "底座高度(mm)",PageType = "CabinetGroup", OptionCode = "baseHeight", OptionValue = new BaseOptionRangeItem() { DescEn = "100", DescZh = "100", Code = "100" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "底座高度(mm)",
    OptionCode = "plinthHeight",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "100", DescZh = "100", Code = "100" },
 } }
            }
            };
            list.Add(_bTLN);



            var _GmZD = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "底座高度(mm)",PageType = "CabinetGroup", OptionCode = "baseHeight", OptionValue = new BaseOptionRangeItem() { DescEn = "150", DescZh = "150", Code = "150" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "底座高度(mm)",
    OptionCode = "plinthHeight",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "150", DescZh = "150", Code = "150" },
 } }
            }
            };
            list.Add(_GmZD);



            var _lvNB = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "底座高度(mm)",PageType = "CabinetGroup", OptionCode = "baseHeight", OptionValue = new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "底座高度(mm)",
    OptionCode = "plinthHeight",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_lvNB);



            var _ReqC = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "后门类型",PageType = "CabinetGroup", OptionCode = "rearDoorType", OptionValue = new BaseOptionRangeItem() { DescEn = "Door", DescZh = "门板", Code = "door" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "后门类型",
    OptionCode = "rearDoorType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "Door", DescZh = "门板", Code = "door" },
 } }
            }
            };
            list.Add(_ReqC);



            var _YDnt = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "后门类型",PageType = "CabinetGroup", OptionCode = "rearDoorType", OptionValue = new BaseOptionRangeItem() { DescEn = "Cover", DescZh = "盖板", Code = "cover" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "后门类型",
    OptionCode = "rearDoorType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "Cover", DescZh = "盖板", Code = "cover" },
 } }
            }
            };
            list.Add(_YDnt);



            var _uwkx = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "单/双层门",PageType = "CabinetGroup", OptionCode = "doorType", OptionValue = new BaseOptionRangeItem() { DescEn = "Single", DescZh = "单层门", Code = "single" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单/双层门",
    OptionCode = "doorType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "Single", DescZh = "单层门", Code = "single" },
 } }
            }
            };
            list.Add(_uwkx);



            var _NtMK = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "单/双层门",PageType = "CabinetGroup", OptionCode = "doorType", OptionValue = new BaseOptionRangeItem() { DescEn = "Double", DescZh = "双层门", Code = "double" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "单/双层门",
    OptionCode = "doorType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "Double", DescZh = "双层门", Code = "double" },
 } }
            }
            };
            list.Add(_NtMK);



            var _PpJX = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "分隔类型",PageType = "CabinetGroup", OptionCode = "separationType", OptionValue = new BaseOptionRangeItem() { DescEn = "Form 4a", DescZh = "Form 4a", Code = "form4a" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "分隔类型",
    OptionCode = "separationType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "Form 4a", DescZh = "Form 4a", Code = "Form4a" },
         new BaseOptionRangeItem() { DescEn = "Form 4a", DescZh = "Form 4a", Code = "Form4a" },
         new BaseOptionRangeItem() { DescEn = "Form 4a", DescZh = "Form 4a", Code = "Form4a" },
         new BaseOptionRangeItem() { DescEn = "Form 4a", DescZh = "Form 4a", Code = "Form4a" },
 } }
            }
            };
            list.Add(_PpJX);



            var _KWsp = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "分隔类型",PageType = "CabinetGroup", OptionCode = "separationType", OptionValue = new BaseOptionRangeItem() { DescEn = "Form 4b", DescZh = "Form 4b", Code = "form4b" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "分隔类型",
    OptionCode = "separationType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "Form 4b", DescZh = "Form 4b", Code = "Form4b" },
         new BaseOptionRangeItem() { DescEn = "Form 4b", DescZh = "Form 4b", Code = "Form4b" },
         new BaseOptionRangeItem() { DescEn = "Form 4b", DescZh = "Form 4b", Code = "Form4b" },
         new BaseOptionRangeItem() { DescEn = "Form 4b", DescZh = "Form 4b", Code = "Form4b" },
 } }
            }
            };
            list.Add(_KWsp);



            var _PMfs = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "分隔类型",PageType = "CabinetGroup", OptionCode = "separationType", OptionValue = new BaseOptionRangeItem() { DescEn = "Form 3a", DescZh = "Form 3a", Code = "form3a" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "分隔类型",
    OptionCode = "separationType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "Form 3a", DescZh = "Form 3a", Code = "Form3a" },
         new BaseOptionRangeItem() { DescEn = "Form 3a", DescZh = "Form 3a", Code = "Form3a" },
         new BaseOptionRangeItem() { DescEn = "Form 3a", DescZh = "Form 3a", Code = "Form3a" },
         new BaseOptionRangeItem() { DescEn = "Form 3a", DescZh = "Form 3a", Code = "Form3a" },
 } }
            }
            };
            list.Add(_PMfs);



            var _kmUW = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "分隔类型",PageType = "CabinetGroup", OptionCode = "separationType", OptionValue = new BaseOptionRangeItem() { DescEn = "Form 3b", DescZh = "Form 3b", Code = "form3b" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "分隔类型",
    OptionCode = "separationType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "Form 3b", DescZh = "Form 3b", Code = "Form3b" },
         new BaseOptionRangeItem() { DescEn = "Form 3b", DescZh = "Form 3b", Code = "Form3b" },
         new BaseOptionRangeItem() { DescEn = "Form 3b", DescZh = "Form 3b", Code = "Form3b" },
         new BaseOptionRangeItem() { DescEn = "Form 3b", DescZh = "Form 3b", Code = "Form3b" },
 } }
            }
            };
            list.Add(_kmUW);



            var _KhOC = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "分隔类型",PageType = "CabinetGroup", OptionCode = "separationType", OptionValue = new BaseOptionRangeItem() { DescEn = "Form 2a", DescZh = "Form 2a", Code = "form2a" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "分隔类型",
    OptionCode = "separationType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "Form 2a", DescZh = "Form 2a", Code = "Form2a" },
         new BaseOptionRangeItem() { DescEn = "Form 2a", DescZh = "Form 2a", Code = "Form2a" },
         new BaseOptionRangeItem() { DescEn = "Form 2a", DescZh = "Form 2a", Code = "Form2a" },
         new BaseOptionRangeItem() { DescEn = "Form 2a", DescZh = "Form 2a", Code = "Form2a" },
 } }
            }
            };
            list.Add(_KhOC);



            var _HCxd = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "分隔类型",PageType = "CabinetGroup", OptionCode = "separationType", OptionValue = new BaseOptionRangeItem() { DescEn = "Form 2b", DescZh = "Form 2b", Code = "form2b" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "分隔类型",
    OptionCode = "separationType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "Form 2b", DescZh = "Form 2b", Code = "Form2b" },
         new BaseOptionRangeItem() { DescEn = "Form 2b", DescZh = "Form 2b", Code = "Form2b" },
         new BaseOptionRangeItem() { DescEn = "Form 2b", DescZh = "Form 2b", Code = "Form2b" },
         new BaseOptionRangeItem() { DescEn = "Form 2b", DescZh = "Form 2b", Code = "Form2b" },
 } }
            }
            };
            list.Add(_HCxd);



            var _exjG = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "环境温度(℃)",PageType = "CabinetGroup", OptionCode = "ambientTemperature", OptionValue = new BaseOptionRangeItem() { DescEn = "35", DescZh = "35", Code = "35" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "环境温度(℃)",
    OptionCode = "ambientTemperature",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "35", DescZh = "35", Code = "35" },
         new BaseOptionRangeItem() { DescEn = "35", DescZh = "35", Code = "35" },
         new BaseOptionRangeItem() { DescEn = "35", DescZh = "35", Code = "35" },
         new BaseOptionRangeItem() { DescEn = "35", DescZh = "35", Code = "35" },
         new BaseOptionRangeItem() { DescEn = "35", DescZh = "35", Code = "35" },
         new BaseOptionRangeItem() { DescEn = "35", DescZh = "35", Code = "35" },
 } }
            }
            };
            list.Add(_exjG);



            var _unyD = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "环境温度(℃)",PageType = "CabinetGroup", OptionCode = "ambientTemperature", OptionValue = new BaseOptionRangeItem() { DescEn = "50", DescZh = "50", Code = "50" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "环境温度(℃)",
    OptionCode = "ambientTemperature",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "50", DescZh = "50", Code = "50" },
         new BaseOptionRangeItem() { DescEn = "50", DescZh = "50", Code = "50" },
         new BaseOptionRangeItem() { DescEn = "50", DescZh = "50", Code = "50" },
         new BaseOptionRangeItem() { DescEn = "50", DescZh = "50", Code = "50" },
         new BaseOptionRangeItem() { DescEn = "50", DescZh = "50", Code = "50" },
         new BaseOptionRangeItem() { DescEn = "50", DescZh = "50", Code = "50" },
 } }
            }
            };
            list.Add(_unyD);



            var _nlqE = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "防护等级",PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem() { DescEn = "IP65", DescZh = "IP65", Code = "ip65" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "防护等级",
    OptionCode = "ingressProtectionDegree",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "IP65", DescZh = "IP65", Code = "IP65" },
         new BaseOptionRangeItem() { DescEn = "IP65", DescZh = "IP65", Code = "IP65" },
         new BaseOptionRangeItem() { DescEn = "IP65", DescZh = "IP65", Code = "IP65" },
         new BaseOptionRangeItem() { DescEn = "IP65", DescZh = "IP65", Code = "IP65" },
         new BaseOptionRangeItem() { DescEn = "IP65", DescZh = "IP65", Code = "IP65" },
         new BaseOptionRangeItem() { DescEn = "IP65", DescZh = "IP65", Code = "IP65" },
         new BaseOptionRangeItem() { DescEn = "IP65", DescZh = "IP65", Code = "IP65" },
         new BaseOptionRangeItem() { DescEn = "IP65", DescZh = "IP65", Code = "IP65" },
         new BaseOptionRangeItem() { DescEn = "IP65", DescZh = "IP65", Code = "IP65" },
         new BaseOptionRangeItem() { DescEn = "IP65", DescZh = "IP65", Code = "IP65" },
         new BaseOptionRangeItem() { DescEn = "IP65", DescZh = "IP65", Code = "IP65" },
         new BaseOptionRangeItem() { DescEn = "IP65", DescZh = "IP65", Code = "IP65" },
 } }
            }
            };
            list.Add(_nlqE);



            var _ssuz = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "防护等级",PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem() { DescEn = "IP54", DescZh = "IP54", Code = "ip54" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "防护等级",
    OptionCode = "ingressProtectionDegree",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "IP54", DescZh = "IP54", Code = "IP54" },
         new BaseOptionRangeItem() { DescEn = "IP54", DescZh = "IP54", Code = "IP54" },
         new BaseOptionRangeItem() { DescEn = "IP54", DescZh = "IP54", Code = "IP54" },
         new BaseOptionRangeItem() { DescEn = "IP54", DescZh = "IP54", Code = "IP54" },
         new BaseOptionRangeItem() { DescEn = "IP54", DescZh = "IP54", Code = "IP54" },
         new BaseOptionRangeItem() { DescEn = "IP54", DescZh = "IP54", Code = "IP54" },
         new BaseOptionRangeItem() { DescEn = "IP54", DescZh = "IP54", Code = "IP54" },
         new BaseOptionRangeItem() { DescEn = "IP54", DescZh = "IP54", Code = "IP54" },
         new BaseOptionRangeItem() { DescEn = "IP54", DescZh = "IP54", Code = "IP54" },
         new BaseOptionRangeItem() { DescEn = "IP54", DescZh = "IP54", Code = "IP54" },
         new BaseOptionRangeItem() { DescEn = "IP54", DescZh = "IP54", Code = "IP54" },
         new BaseOptionRangeItem() { DescEn = "IP54", DescZh = "IP54", Code = "IP54" },
 } }
            }
            };
            list.Add(_ssuz);



            var _lHst = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "防护等级",PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem() { DescEn = "IP43", DescZh = "IP43", Code = "ip43" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "防护等级",
    OptionCode = "ingressProtectionDegree",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "IP43", DescZh = "IP43", Code = "IP43" },
         new BaseOptionRangeItem() { DescEn = "IP43", DescZh = "IP43", Code = "IP43" },
         new BaseOptionRangeItem() { DescEn = "IP43", DescZh = "IP43", Code = "IP43" },
         new BaseOptionRangeItem() { DescEn = "IP43", DescZh = "IP43", Code = "IP43" },
         new BaseOptionRangeItem() { DescEn = "IP43", DescZh = "IP43", Code = "IP43" },
         new BaseOptionRangeItem() { DescEn = "IP43", DescZh = "IP43", Code = "IP43" },
         new BaseOptionRangeItem() { DescEn = "IP43", DescZh = "IP43", Code = "IP43" },
         new BaseOptionRangeItem() { DescEn = "IP43", DescZh = "IP43", Code = "IP43" },
         new BaseOptionRangeItem() { DescEn = "IP43", DescZh = "IP43", Code = "IP43" },
         new BaseOptionRangeItem() { DescEn = "IP43", DescZh = "IP43", Code = "IP43" },
         new BaseOptionRangeItem() { DescEn = "IP43", DescZh = "IP43", Code = "IP43" },
         new BaseOptionRangeItem() { DescEn = "IP43", DescZh = "IP43", Code = "IP43" },
 } }
            }
            };
            list.Add(_lHst);



            var _gWtx = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "防护等级",PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem() { DescEn = "IP42", DescZh = "IP42", Code = "ip42" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "防护等级",
    OptionCode = "ingressProtectionDegree",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "IP42", DescZh = "IP42", Code = "IP42" },
         new BaseOptionRangeItem() { DescEn = "IP42", DescZh = "IP42", Code = "IP42" },
         new BaseOptionRangeItem() { DescEn = "IP42", DescZh = "IP42", Code = "IP42" },
         new BaseOptionRangeItem() { DescEn = "IP42", DescZh = "IP42", Code = "IP42" },
         new BaseOptionRangeItem() { DescEn = "IP42", DescZh = "IP42", Code = "IP42" },
         new BaseOptionRangeItem() { DescEn = "IP42", DescZh = "IP42", Code = "IP42" },
         new BaseOptionRangeItem() { DescEn = "IP42", DescZh = "IP42", Code = "IP42" },
         new BaseOptionRangeItem() { DescEn = "IP42", DescZh = "IP42", Code = "IP42" },
         new BaseOptionRangeItem() { DescEn = "IP42", DescZh = "IP42", Code = "IP42" },
         new BaseOptionRangeItem() { DescEn = "IP42", DescZh = "IP42", Code = "IP42" },
         new BaseOptionRangeItem() { DescEn = "IP42", DescZh = "IP42", Code = "IP42" },
         new BaseOptionRangeItem() { DescEn = "IP42", DescZh = "IP42", Code = "IP42" },
 } }
            }
            };
            list.Add(_gWtx);



            var _wFuO = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "防护等级",PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem() { DescEn = "IP41", DescZh = "IP41", Code = "ip41" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "防护等级",
    OptionCode = "ingressProtectionDegree",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "IP41", DescZh = "IP41", Code = "IP41" },
         new BaseOptionRangeItem() { DescEn = "IP41", DescZh = "IP41", Code = "IP41" },
         new BaseOptionRangeItem() { DescEn = "IP41", DescZh = "IP41", Code = "IP41" },
         new BaseOptionRangeItem() { DescEn = "IP41", DescZh = "IP41", Code = "IP41" },
         new BaseOptionRangeItem() { DescEn = "IP41", DescZh = "IP41", Code = "IP41" },
         new BaseOptionRangeItem() { DescEn = "IP41", DescZh = "IP41", Code = "IP41" },
         new BaseOptionRangeItem() { DescEn = "IP41", DescZh = "IP41", Code = "IP41" },
         new BaseOptionRangeItem() { DescEn = "IP41", DescZh = "IP41", Code = "IP41" },
         new BaseOptionRangeItem() { DescEn = "IP41", DescZh = "IP41", Code = "IP41" },
         new BaseOptionRangeItem() { DescEn = "IP41", DescZh = "IP41", Code = "IP41" },
         new BaseOptionRangeItem() { DescEn = "IP41", DescZh = "IP41", Code = "IP41" },
         new BaseOptionRangeItem() { DescEn = "IP41", DescZh = "IP41", Code = "IP41" },
 } }
            }
            };
            list.Add(_wFuO);



            var _DiHS = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "防护等级",PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem() { DescEn = "IP40", DescZh = "IP40", Code = "ip40" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "防护等级",
    OptionCode = "ingressProtectionDegree",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "IP40", DescZh = "IP40", Code = "IP40" },
         new BaseOptionRangeItem() { DescEn = "IP40", DescZh = "IP40", Code = "IP40" },
         new BaseOptionRangeItem() { DescEn = "IP40", DescZh = "IP40", Code = "IP40" },
         new BaseOptionRangeItem() { DescEn = "IP40", DescZh = "IP40", Code = "IP40" },
         new BaseOptionRangeItem() { DescEn = "IP40", DescZh = "IP40", Code = "IP40" },
         new BaseOptionRangeItem() { DescEn = "IP40", DescZh = "IP40", Code = "IP40" },
         new BaseOptionRangeItem() { DescEn = "IP40", DescZh = "IP40", Code = "IP40" },
         new BaseOptionRangeItem() { DescEn = "IP40", DescZh = "IP40", Code = "IP40" },
         new BaseOptionRangeItem() { DescEn = "IP40", DescZh = "IP40", Code = "IP40" },
         new BaseOptionRangeItem() { DescEn = "IP40", DescZh = "IP40", Code = "IP40" },
         new BaseOptionRangeItem() { DescEn = "IP40", DescZh = "IP40", Code = "IP40" },
         new BaseOptionRangeItem() { DescEn = "IP40", DescZh = "IP40", Code = "IP40" },
 } }
            }
            };
            list.Add(_DiHS);



            var _NPFW = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "防护等级",PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem() { DescEn = "IP30", DescZh = "IP30", Code = "ip30" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "防护等级",
    OptionCode = "ingressProtectionDegree",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "IP30", DescZh = "IP30", Code = "IP30" },
         new BaseOptionRangeItem() { DescEn = "IP30", DescZh = "IP30", Code = "IP30" },
         new BaseOptionRangeItem() { DescEn = "IP30", DescZh = "IP30", Code = "IP30" },
         new BaseOptionRangeItem() { DescEn = "IP30", DescZh = "IP30", Code = "IP30" },
         new BaseOptionRangeItem() { DescEn = "IP30", DescZh = "IP30", Code = "IP30" },
         new BaseOptionRangeItem() { DescEn = "IP30", DescZh = "IP30", Code = "IP30" },
         new BaseOptionRangeItem() { DescEn = "IP30", DescZh = "IP30", Code = "IP30" },
         new BaseOptionRangeItem() { DescEn = "IP30", DescZh = "IP30", Code = "IP30" },
         new BaseOptionRangeItem() { DescEn = "IP30", DescZh = "IP30", Code = "IP30" },
         new BaseOptionRangeItem() { DescEn = "IP30", DescZh = "IP30", Code = "IP30" },
         new BaseOptionRangeItem() { DescEn = "IP30", DescZh = "IP30", Code = "IP30" },
         new BaseOptionRangeItem() { DescEn = "IP30", DescZh = "IP30", Code = "IP30" },
 } }
            }
            };
            list.Add(_NPFW);



            var _bAeE = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "防护等级",PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem() { DescEn = "IP31", DescZh = "IP31", Code = "ip31" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "防护等级",
    OptionCode = "ingressProtectionDegree",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "IP31", DescZh = "IP31", Code = "IP31" },
         new BaseOptionRangeItem() { DescEn = "IP31", DescZh = "IP31", Code = "IP31" },
         new BaseOptionRangeItem() { DescEn = "IP31", DescZh = "IP31", Code = "IP31" },
         new BaseOptionRangeItem() { DescEn = "IP31", DescZh = "IP31", Code = "IP31" },
         new BaseOptionRangeItem() { DescEn = "IP31", DescZh = "IP31", Code = "IP31" },
         new BaseOptionRangeItem() { DescEn = "IP31", DescZh = "IP31", Code = "IP31" },
         new BaseOptionRangeItem() { DescEn = "IP31", DescZh = "IP31", Code = "IP31" },
         new BaseOptionRangeItem() { DescEn = "IP31", DescZh = "IP31", Code = "IP31" },
         new BaseOptionRangeItem() { DescEn = "IP31", DescZh = "IP31", Code = "IP31" },
         new BaseOptionRangeItem() { DescEn = "IP31", DescZh = "IP31", Code = "IP31" },
         new BaseOptionRangeItem() { DescEn = "IP31", DescZh = "IP31", Code = "IP31" },
         new BaseOptionRangeItem() { DescEn = "IP31", DescZh = "IP31", Code = "IP31" },
 } }
            }
            };
            list.Add(_bAeE);



            var _sGxY = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "防护等级",PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem() { DescEn = "IP20", DescZh = "IP20", Code = "ip20" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "防护等级",
    OptionCode = "ingressProtectionDegree",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "IP20", DescZh = "IP20", Code = "IP20" },
         new BaseOptionRangeItem() { DescEn = "IP20", DescZh = "IP20", Code = "IP20" },
         new BaseOptionRangeItem() { DescEn = "IP20", DescZh = "IP20", Code = "IP20" },
         new BaseOptionRangeItem() { DescEn = "IP20", DescZh = "IP20", Code = "IP20" },
         new BaseOptionRangeItem() { DescEn = "IP20", DescZh = "IP20", Code = "IP20" },
         new BaseOptionRangeItem() { DescEn = "IP20", DescZh = "IP20", Code = "IP20" },
         new BaseOptionRangeItem() { DescEn = "IP20", DescZh = "IP20", Code = "IP20" },
         new BaseOptionRangeItem() { DescEn = "IP20", DescZh = "IP20", Code = "IP20" },
         new BaseOptionRangeItem() { DescEn = "IP20", DescZh = "IP20", Code = "IP20" },
         new BaseOptionRangeItem() { DescEn = "IP20", DescZh = "IP20", Code = "IP20" },
         new BaseOptionRangeItem() { DescEn = "IP20", DescZh = "IP20", Code = "IP20" },
         new BaseOptionRangeItem() { DescEn = "IP20", DescZh = "IP20", Code = "IP20" },
 } }
            }
            };
            list.Add(_sGxY);



            var _brgn = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜型",PageType = "CabinetSingle", OptionCode = "panelType", OptionValue = new BaseOptionRangeItem() { DescEn = "Single ACB", DescZh = "单ACB", Code = "singleACB" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "柜型",
    OptionCode = "panelType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "SingleACB", DescZh = "单ACB", Code = "singleACB" },
         new BaseOptionRangeItem() { DescEn = "SingleACB", DescZh = "单ACB", Code = "singleACB" },
         new BaseOptionRangeItem() { DescEn = "SingleACB", DescZh = "单ACB", Code = "singleACB" },
         new BaseOptionRangeItem() { DescEn = "SingleACB", DescZh = "单ACB", Code = "singleACB" },
         new BaseOptionRangeItem() { DescEn = "SingleACB", DescZh = "单ACB", Code = "singleACB" },
 } }
            }
            };
            list.Add(_brgn);



            var _JRfY = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜型",PageType = "CabinetSingle", OptionCode = "panelType", OptionValue = new BaseOptionRangeItem() { DescEn = "Dual ACB", DescZh = "双ACB", Code = "dualACB" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "柜型",
    OptionCode = "panelType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "DualACB", DescZh = "双ACB", Code = "dualACB" },
         new BaseOptionRangeItem() { DescEn = "DualACB", DescZh = "双ACB", Code = "dualACB" },
         new BaseOptionRangeItem() { DescEn = "DualACB", DescZh = "双ACB", Code = "dualACB" },
         new BaseOptionRangeItem() { DescEn = "DualACB", DescZh = "双ACB", Code = "dualACB" },
         new BaseOptionRangeItem() { DescEn = "DualACB", DescZh = "双ACB", Code = "dualACB" },
 } }
            }
            };
            list.Add(_JRfY);



            var _NtIi = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜型",PageType = "CabinetSingle", OptionCode = "panelType", OptionValue = new BaseOptionRangeItem() { DescEn = "Three ACB", DescZh = "三ACB", Code = "threeACB" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "柜型",
    OptionCode = "panelType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "ThreeACB", DescZh = "三ACB", Code = "threeACB" },
         new BaseOptionRangeItem() { DescEn = "ThreeACB", DescZh = "三ACB", Code = "threeACB" },
         new BaseOptionRangeItem() { DescEn = "ThreeACB", DescZh = "三ACB", Code = "threeACB" },
         new BaseOptionRangeItem() { DescEn = "ThreeACB", DescZh = "三ACB", Code = "threeACB" },
 } }
            }
            };
            list.Add(_NtIi);



            var _qAaE = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜型",PageType = "CabinetSingle", OptionCode = "panelType", OptionValue = new BaseOptionRangeItem() { DescEn = "Drawer panel", DescZh = "抽屉柜", Code = "drawerPanel" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "柜型",
    OptionCode = "panelType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "DrawerPanel", DescZh = "抽屉柜", Code = "drawerPanel" },
         new BaseOptionRangeItem() { DescEn = "DrawerPanel", DescZh = "抽屉柜", Code = "drawerPanel" },
         new BaseOptionRangeItem() { DescEn = "DrawerPanel", DescZh = "抽屉柜", Code = "drawerPanel" },
         new BaseOptionRangeItem() { DescEn = "DrawerPanel", DescZh = "抽屉柜", Code = "drawerPanel" },
         new BaseOptionRangeItem() { DescEn = "DrawerPanel", DescZh = "抽屉柜", Code = "drawerPanel" },
 } }
            }
            };
            list.Add(_qAaE);



            var _ILxr = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜型",PageType = "CabinetSingle", OptionCode = "panelType", OptionValue = new BaseOptionRangeItem() { DescEn = "Fixed partition cabinet", DescZh = "固定分隔柜", Code = "fixedSeparatelyPanel" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "柜型",
    OptionCode = "panelType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "FixedSeparatelyPanel", DescZh = "固定分隔柜", Code = "fixedSeparatelyPanel" },
         new BaseOptionRangeItem() { DescEn = "FixedSeparatelyPanel", DescZh = "固定分隔柜", Code = "fixedSeparatelyPanel" },
         new BaseOptionRangeItem() { DescEn = "FixedSeparatelyPanel", DescZh = "固定分隔柜", Code = "fixedSeparatelyPanel" },
         new BaseOptionRangeItem() { DescEn = "FixedSeparatelyPanel", DescZh = "固定分隔柜", Code = "fixedSeparatelyPanel" },
         new BaseOptionRangeItem() { DescEn = "FixedSeparatelyPanel", DescZh = "固定分隔柜", Code = "fixedSeparatelyPanel" },
 } }
            }
            };
            list.Add(_ILxr);



            var _Qhzy = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜型",PageType = "CabinetSingle", OptionCode = "panelType", OptionValue = new BaseOptionRangeItem() { DescEn = "Fixed separately panel", DescZh = "固定柜", Code = "fixedPanel" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "柜型",
    OptionCode = "panelType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "FixedPanel", DescZh = "固定柜", Code = "fixedPanel" },
         new BaseOptionRangeItem() { DescEn = "FixedPanel", DescZh = "固定柜", Code = "fixedPanel" },
         new BaseOptionRangeItem() { DescEn = "FixedPanel", DescZh = "固定柜", Code = "fixedPanel" },
         new BaseOptionRangeItem() { DescEn = "FixedPanel", DescZh = "固定柜", Code = "fixedPanel" },
         new BaseOptionRangeItem() { DescEn = "FixedPanel", DescZh = "固定柜", Code = "fixedPanel" },
 } }
            }
            };
            list.Add(_Qhzy);



            var _ORUf = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜型",PageType = "CabinetSingle", OptionCode = "panelType", OptionValue = new BaseOptionRangeItem() { DescEn = "Mixed panel", DescZh = "混装柜", Code = "mixedPanel" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "柜型",
    OptionCode = "panelType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "MixedPanel", DescZh = "混装柜", Code = "mixedPanel" },
         new BaseOptionRangeItem() { DescEn = "MixedPanel", DescZh = "混装柜", Code = "mixedPanel" },
         new BaseOptionRangeItem() { DescEn = "MixedPanel", DescZh = "混装柜", Code = "mixedPanel" },
         new BaseOptionRangeItem() { DescEn = "MixedPanel", DescZh = "混装柜", Code = "mixedPanel" },
         new BaseOptionRangeItem() { DescEn = "MixedPanel", DescZh = "混装柜", Code = "mixedPanel" },
 } }
            }
            };
            list.Add(_ORUf);



            var _xovv = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜型",PageType = "CabinetSingle", OptionCode = "panelType", OptionValue = new BaseOptionRangeItem() { DescEn = "Tie panel", DescZh = "母联柜", Code = "tiePanel" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "柜型",
    OptionCode = "panelType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "TiePanel", DescZh = "母联柜", Code = "tiePanel" },
         new BaseOptionRangeItem() { DescEn = "TiePanel", DescZh = "母联柜", Code = "tiePanel" },
         new BaseOptionRangeItem() { DescEn = "TiePanel", DescZh = "母联柜", Code = "tiePanel" },
         new BaseOptionRangeItem() { DescEn = "TiePanel", DescZh = "母联柜", Code = "tiePanel" },
         new BaseOptionRangeItem() { DescEn = "TiePanel", DescZh = "母联柜", Code = "tiePanel" },
 } }
            }
            };
            list.Add(_xovv);



            var _IFjz = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜型",PageType = "CabinetSingle", OptionCode = "panelType", OptionValue = new BaseOptionRangeItem() { DescEn = "Capacitor panel", DescZh = "电容柜", Code = "capacitorPanel" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "柜型",
    OptionCode = "panelType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "CapacitorPanel", DescZh = "电容柜", Code = "capacitorPanel" },
         new BaseOptionRangeItem() { DescEn = "CapacitorPanel", DescZh = "电容柜", Code = "capacitorPanel" },
         new BaseOptionRangeItem() { DescEn = "CapacitorPanel", DescZh = "电容柜", Code = "capacitorPanel" },
         new BaseOptionRangeItem() { DescEn = "CapacitorPanel", DescZh = "电容柜", Code = "capacitorPanel" },
 } }
            }
            };
            list.Add(_IFjz);



            var _VPNX = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜型",PageType = "CabinetSingle", OptionCode = "panelType", OptionValue = new BaseOptionRangeItem() { DescEn = "SVG panel", DescZh = "SVG柜", Code = "svgPanel" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "柜型",
    OptionCode = "panelType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "SVGPanel", DescZh = "SVG柜", Code = "svgPanel" },
         new BaseOptionRangeItem() { DescEn = "SVGPanel", DescZh = "SVG柜", Code = "svgPanel" },
         new BaseOptionRangeItem() { DescEn = "SVGPanel", DescZh = "SVG柜", Code = "svgPanel" },
         new BaseOptionRangeItem() { DescEn = "SVGPanel", DescZh = "SVG柜", Code = "svgPanel" },
         new BaseOptionRangeItem() { DescEn = "SVGPanel", DescZh = "SVG柜", Code = "svgPanel" },
 } }
            }
            };
            list.Add(_VPNX);



            var _Opvv = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜型",PageType = "CabinetSingle", OptionCode = "panelType", OptionValue = new BaseOptionRangeItem() { DescEn = "APF panel", DescZh = "APF柜", Code = "apfCabinet" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "柜型",
    OptionCode = "panelType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "APFPanel", DescZh = "APF柜", Code = "apfCabinet" },
         new BaseOptionRangeItem() { DescEn = "APFPanel", DescZh = "APF柜", Code = "apfCabinet" },
         new BaseOptionRangeItem() { DescEn = "APFPanel", DescZh = "APF柜", Code = "apfCabinet" },
         new BaseOptionRangeItem() { DescEn = "APFPanel", DescZh = "APF柜", Code = "apfCabinet" },
         new BaseOptionRangeItem() { DescEn = "APFPanel", DescZh = "APF柜", Code = "apfCabinet" },
 } }
            }
            };
            list.Add(_Opvv);



            var _VpcD = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜型",PageType = "CabinetSingle", OptionCode = "panelType", OptionValue = new BaseOptionRangeItem() { DescEn = "VFD panel", DescZh = "变频柜", Code = "vfdPanel" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "柜型",
    OptionCode = "panelType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "VFDPanel", DescZh = "变频柜", Code = "vfdPanel" },
         new BaseOptionRangeItem() { DescEn = "VFDPanel", DescZh = "变频柜", Code = "vfdPanel" },
         new BaseOptionRangeItem() { DescEn = "VFDPanel", DescZh = "变频柜", Code = "vfdPanel" },
         new BaseOptionRangeItem() { DescEn = "VFDPanel", DescZh = "变频柜", Code = "vfdPanel" },
         new BaseOptionRangeItem() { DescEn = "VFDPanel", DescZh = "变频柜", Code = "vfdPanel" },
 } }
            }
            };
            list.Add(_VpcD);



            var _tGEX = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜型",PageType = "CabinetSingle", OptionCode = "panelType", OptionValue = new BaseOptionRangeItem() { DescEn = "Soft start panel", DescZh = "软启柜", Code = "softStartCabinet" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "柜型",
    OptionCode = "panelType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "SoftStartPanel", DescZh = "软启柜", Code = "softStartCabinet" },
         new BaseOptionRangeItem() { DescEn = "SoftStartPanel", DescZh = "软启柜", Code = "softStartCabinet" },
         new BaseOptionRangeItem() { DescEn = "SoftStartPanel", DescZh = "软启柜", Code = "softStartCabinet" },
         new BaseOptionRangeItem() { DescEn = "SoftStartPanel", DescZh = "软启柜", Code = "softStartCabinet" },
         new BaseOptionRangeItem() { DescEn = "SoftStartPanel", DescZh = "软启柜", Code = "softStartCabinet" },
 } }
            }
            };
            list.Add(_tGEX);



            var _lCsZ = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜型",PageType = "CabinetSingle", OptionCode = "panelType", OptionValue = new BaseOptionRangeItem() { DescEn = "Fixed panel (4MCCB)", DescZh = "固定柜(4MCCB)", Code = "fixedPanel4MCCB" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "柜型",
    OptionCode = "panelType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "FixedPanel(4MCCB)", DescZh = "固定柜(4MCCB)", Code = "fixedPanel4MCCB" },
         new BaseOptionRangeItem() { DescEn = "FixedPanel(4MCCB)", DescZh = "固定柜(4MCCB)", Code = "fixedPanel4MCCB" },
         new BaseOptionRangeItem() { DescEn = "FixedPanel(4MCCB)", DescZh = "固定柜(4MCCB)", Code = "fixedPanel4MCCB" },
         new BaseOptionRangeItem() { DescEn = "FixedPanel(4MCCB)", DescZh = "固定柜(4MCCB)", Code = "fixedPanel4MCCB" },
 } }
            }
            };
            list.Add(_lCsZ);



            var _UMRJ = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜型",PageType = "CabinetSingle", OptionCode = "panelType", OptionValue = new BaseOptionRangeItem() { DescEn = "Fixed panel (2MCCB)", DescZh = "固定柜(双MCCB)", Code = "fixedPanel2MCCB" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "柜型",
    OptionCode = "panelType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "FixedPanel(2MCCB)", DescZh = "固定柜(双MCCB)", Code = "fixedPanel2MCCB" },
         new BaseOptionRangeItem() { DescEn = "FixedPanel(2MCCB)", DescZh = "固定柜(双MCCB)", Code = "fixedPanel2MCCB" },
         new BaseOptionRangeItem() { DescEn = "FixedPanel(2MCCB)", DescZh = "固定柜(双MCCB)", Code = "fixedPanel2MCCB" },
         new BaseOptionRangeItem() { DescEn = "FixedPanel(2MCCB)", DescZh = "固定柜(双MCCB)", Code = "fixedPanel2MCCB" },
 } }
            }
            };
            list.Add(_UMRJ);



            var _abMF = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜型",PageType = "CabinetSingle", OptionCode = "panelType", OptionValue = new BaseOptionRangeItem() { DescEn = "Double power 2ACB", DescZh = "双电源双ACB", Code = "doublePower2ACB" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "柜型",
    OptionCode = "panelType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "DoublePower2ACB", DescZh = "双电源双ACB", Code = "doublePower2ACB" },
         new BaseOptionRangeItem() { DescEn = "DoublePower2ACB", DescZh = "双电源双ACB", Code = "doublePower2ACB" },
         new BaseOptionRangeItem() { DescEn = "DoublePower2ACB", DescZh = "双电源双ACB", Code = "doublePower2ACB" },
         new BaseOptionRangeItem() { DescEn = "DoublePower2ACB", DescZh = "双电源双ACB", Code = "doublePower2ACB" },
         new BaseOptionRangeItem() { DescEn = "DoublePower2ACB", DescZh = "双电源双ACB", Code = "doublePower2ACB" },
 } }
            }
            };
            list.Add(_abMF);



            var _wVpq = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜型",PageType = "CabinetSingle", OptionCode = "panelType", OptionValue = new BaseOptionRangeItem() { DescEn = "Double power 2MCCB", DescZh = "双电源双MCCB", Code = "doublePower2MCCB" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "柜型",
    OptionCode = "panelType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "DoublePower2MCCB", DescZh = "双电源双MCCB", Code = "doublePower2MCCB" },
         new BaseOptionRangeItem() { DescEn = "DoublePower2MCCB", DescZh = "双电源双MCCB", Code = "doublePower2MCCB" },
         new BaseOptionRangeItem() { DescEn = "DoublePower2MCCB", DescZh = "双电源双MCCB", Code = "doublePower2MCCB" },
         new BaseOptionRangeItem() { DescEn = "DoublePower2MCCB", DescZh = "双电源双MCCB", Code = "doublePower2MCCB" },
         new BaseOptionRangeItem() { DescEn = "DoublePower2MCCB", DescZh = "双电源双MCCB", Code = "doublePower2MCCB" },
 } }
            }
            };
            list.Add(_wVpq);



            var _pUEG = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜型",PageType = "CabinetSingle", OptionCode = "panelType", OptionValue = new BaseOptionRangeItem() { DescEn = "Double power one ATS", DescZh = "双电源单ATS", Code = "doublePowerOneATS" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "柜型",
    OptionCode = "panelType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "DoublePowerOneATS", DescZh = "双电源单ATS", Code = "doublePowerOneATS" },
         new BaseOptionRangeItem() { DescEn = "DoublePowerOneATS", DescZh = "双电源单ATS", Code = "doublePowerOneATS" },
         new BaseOptionRangeItem() { DescEn = "DoublePowerOneATS", DescZh = "双电源单ATS", Code = "doublePowerOneATS" },
         new BaseOptionRangeItem() { DescEn = "DoublePowerOneATS", DescZh = "双电源单ATS", Code = "doublePowerOneATS" },
         new BaseOptionRangeItem() { DescEn = "DoublePowerOneATS", DescZh = "双电源单ATS", Code = "doublePowerOneATS" },
 } }
            }
            };
            list.Add(_pUEG);



            var _qRVf = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜型",PageType = "CabinetSingle", OptionCode = "panelType", OptionValue = new BaseOptionRangeItem() { DescEn = "Double power 2ACB+ATS", DescZh = "双电源双ACB+ATS", Code = "doublePower2ACBATS" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "柜型",
    OptionCode = "panelType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "DoublePower2ACB+ATS", DescZh = "双电源双ACB+ATS", Code = "doublePower2ACBATS" },
         new BaseOptionRangeItem() { DescEn = "DoublePower2ACB+ATS", DescZh = "双电源双ACB+ATS", Code = "doublePower2ACBATS" },
         new BaseOptionRangeItem() { DescEn = "DoublePower2ACB+ATS", DescZh = "双电源双ACB+ATS", Code = "doublePower2ACBATS" },
         new BaseOptionRangeItem() { DescEn = "DoublePower2ACB+ATS", DescZh = "双电源双ACB+ATS", Code = "doublePower2ACBATS" },
         new BaseOptionRangeItem() { DescEn = "DoublePower2ACB+ATS", DescZh = "双电源双ACB+ATS", Code = "doublePower2ACBATS" },
 } }
            }
            };
            list.Add(_qRVf);



            var _mDFn = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜型",PageType = "CabinetSingle", OptionCode = "panelType", OptionValue = new BaseOptionRangeItem() { DescEn = "Double power 2MCCB+ATS", DescZh = "双电源双MCCB+ATS", Code = "doublePower2MCCBATS" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "柜型",
    OptionCode = "panelType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "DoublePower2MCCB+ATS", DescZh = "双电源双MCCB+ATS", Code = "doublePower2MCCBATS" },
         new BaseOptionRangeItem() { DescEn = "DoublePower2MCCB+ATS", DescZh = "双电源双MCCB+ATS", Code = "doublePower2MCCBATS" },
         new BaseOptionRangeItem() { DescEn = "DoublePower2MCCB+ATS", DescZh = "双电源双MCCB+ATS", Code = "doublePower2MCCBATS" },
         new BaseOptionRangeItem() { DescEn = "DoublePower2MCCB+ATS", DescZh = "双电源双MCCB+ATS", Code = "doublePower2MCCBATS" },
         new BaseOptionRangeItem() { DescEn = "DoublePower2MCCB+ATS", DescZh = "双电源双MCCB+ATS", Code = "doublePower2MCCBATS" },
 } }
            }
            };
            list.Add(_mDFn);



            var _fwcD = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜型",PageType = "CabinetSingle", OptionCode = "panelType", OptionValue = new BaseOptionRangeItem() { DescEn = "Vertical busbar panel", DescZh = "垂直母线柜", Code = "verticalBusbarPanel" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "柜型",
    OptionCode = "panelType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "VerticalBusbarPanel", DescZh = "垂直母线柜", Code = "verticalBusbarPanel" },
         new BaseOptionRangeItem() { DescEn = "VerticalBusbarPanel", DescZh = "垂直母线柜", Code = "verticalBusbarPanel" },
         new BaseOptionRangeItem() { DescEn = "VerticalBusbarPanel", DescZh = "垂直母线柜", Code = "verticalBusbarPanel" },
         new BaseOptionRangeItem() { DescEn = "VerticalBusbarPanel", DescZh = "垂直母线柜", Code = "verticalBusbarPanel" },
 } }
            }
            };
            list.Add(_fwcD);



            var _WoFE = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "柜型",PageType = "CabinetSingle", OptionCode = "panelType", OptionValue = new BaseOptionRangeItem() { DescEn = "Cable panel", DescZh = "电缆连接柜", Code = "cablePanel" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "柜型",
    OptionCode = "panelType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "CablePanel", DescZh = "电缆连接柜", Code = "cablePanel" },
         new BaseOptionRangeItem() { DescEn = "CablePanel", DescZh = "电缆连接柜", Code = "cablePanel" },
         new BaseOptionRangeItem() { DescEn = "CablePanel", DescZh = "电缆连接柜", Code = "cablePanel" },
         new BaseOptionRangeItem() { DescEn = "CablePanel", DescZh = "电缆连接柜", Code = "cablePanel" },
 } }
            }
            };
            list.Add(_WoFE);



            var _uCBO = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "后门开门类型",PageType = "CabinetSingle", OptionCode = "rearDoorOpenType", OptionValue = new BaseOptionRangeItem() { DescEn = "Double open door", DescZh = "双开门", Code = "doubleOpenDoor" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "后门开门类型",
    OptionCode = "rearDoorOpenType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "DoubleOpenDoor", DescZh = "双开门", Code = "doubleOpenDoor" },
         new BaseOptionRangeItem() { DescEn = "DoubleOpenDoor", DescZh = "双开门", Code = "doubleOpenDoor" },
 } }
            }
            };
            list.Add(_uCBO);



            var _PCND = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "后门开门类型",PageType = "CabinetSingle", OptionCode = "rearDoorOpenType", OptionValue = new BaseOptionRangeItem() { DescEn = "Single door left open", DescZh = "单门左开", Code = "singleDoorLeftOpen" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "后门开门类型",
    OptionCode = "rearDoorOpenType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "SingleDoorLeftOpen", DescZh = "单门左开", Code = "singleDoorLeftOpen" },
         new BaseOptionRangeItem() { DescEn = "SingleDoorLeftOpen", DescZh = "单门左开", Code = "singleDoorLeftOpen" },
 } }
            }
            };
            list.Add(_PCND);



            var _WXdM = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "后门开门类型",PageType = "CabinetSingle", OptionCode = "rearDoorOpenType", OptionValue = new BaseOptionRangeItem() { DescEn = "Single door right open", DescZh = "单门右开", Code = "singleDoorRightOpen" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "后门开门类型",
    OptionCode = "rearDoorOpenType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "SingleDoorRightOpen", DescZh = "单门右开", Code = "singleDoorRightOpen" },
         new BaseOptionRangeItem() { DescEn = "SingleDoorRightOpen", DescZh = "单门右开", Code = "singleDoorRightOpen" },
 } }
            }
            };
            list.Add(_WXdM);



            var _JXKo = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "进线方式",PageType = "CabinetSingle", OptionCode = "incomingType", OptionValue = new BaseOptionRangeItem() { DescEn = "Busbar top incoming", DescZh = "母排上进", Code = "busbarTopIncoming" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "进线方式",
    OptionCode = "incomingType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "BusbarTopIncoming", DescZh = "母排上进", Code = "busbarTopIncoming" },
         new BaseOptionRangeItem() { DescEn = "BusbarTopIncoming", DescZh = "母排上进", Code = "busbarTopIncoming" },
         new BaseOptionRangeItem() { DescEn = "BusbarTopIncoming", DescZh = "母排上进", Code = "busbarTopIncoming" },
         new BaseOptionRangeItem() { DescEn = "BusbarTopIncoming", DescZh = "母排上进", Code = "busbarTopIncoming" },
 } }
            }
            };
            list.Add(_JXKo);



            var _jKJn = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "进线方式",PageType = "CabinetSingle", OptionCode = "incomingType", OptionValue = new BaseOptionRangeItem() { DescEn = "Busbar side incoming", DescZh = "母排侧进", Code = "busbarSideIncoming" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "进线方式",
    OptionCode = "incomingType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "BusbarSideIncoming", DescZh = "母排侧进", Code = "busbarSideIncoming" },
         new BaseOptionRangeItem() { DescEn = "BusbarSideIncoming", DescZh = "母排侧进", Code = "busbarSideIncoming" },
         new BaseOptionRangeItem() { DescEn = "BusbarSideIncoming", DescZh = "母排侧进", Code = "busbarSideIncoming" },
         new BaseOptionRangeItem() { DescEn = "BusbarSideIncoming", DescZh = "母排侧进", Code = "busbarSideIncoming" },
 } }
            }
            };
            list.Add(_jKJn);



            var _JyRG = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "进线方式",PageType = "CabinetSingle", OptionCode = "incomingType", OptionValue = new BaseOptionRangeItem() { DescEn = "Cable bottom incoming", DescZh = "电缆下进", Code = "cableBottomIncoming" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "进线方式",
    OptionCode = "incomingType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "CableBottomIncoming", DescZh = "电缆下进", Code = "cableBottomIncoming" },
         new BaseOptionRangeItem() { DescEn = "CableBottomIncoming", DescZh = "电缆下进", Code = "cableBottomIncoming" },
         new BaseOptionRangeItem() { DescEn = "CableBottomIncoming", DescZh = "电缆下进", Code = "cableBottomIncoming" },
         new BaseOptionRangeItem() { DescEn = "CableBottomIncoming", DescZh = "电缆下进", Code = "cableBottomIncoming" },
 } }
            }
            };
            list.Add(_JyRG);



            var _yFPf = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "进线方式",PageType = "CabinetSingle", OptionCode = "incomingType", OptionValue = new BaseOptionRangeItem() { DescEn = "Cable top incoming", DescZh = "电缆上进", Code = "cableTopIncoming" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "进线方式",
    OptionCode = "incomingType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "CableTopIncoming", DescZh = "电缆上进", Code = "cableTopIncoming" },
         new BaseOptionRangeItem() { DescEn = "CableTopIncoming", DescZh = "电缆上进", Code = "cableTopIncoming" },
         new BaseOptionRangeItem() { DescEn = "CableTopIncoming", DescZh = "电缆上进", Code = "cableTopIncoming" },
         new BaseOptionRangeItem() { DescEn = "CableTopIncoming", DescZh = "电缆上进", Code = "cableTopIncoming" },
 } }
            }
            };
            list.Add(_yFPf);



            var _QdyB = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "出线方式",PageType = "CabinetSingle", OptionCode = "outgoingMethod", OptionValue = new BaseOptionRangeItem() { DescEn = "Cable top outgoing", DescZh = "电缆下出", Code = "cableBottomOutgoing" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "出线方式",
    OptionCode = "outgoingMethod",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "CableBottomOutgoing", DescZh = "电缆下出", Code = "cableBottomOutgoing" },
         new BaseOptionRangeItem() { DescEn = "CableBottomOutgoing", DescZh = "电缆下出", Code = "cableBottomOutgoing" },
         new BaseOptionRangeItem() { DescEn = "CableBottomOutgoing", DescZh = "电缆下出", Code = "cableBottomOutgoing" },
         new BaseOptionRangeItem() { DescEn = "CableBottomOutgoing", DescZh = "电缆下出", Code = "cableBottomOutgoing" },
 } }
            }
            };
            list.Add(_QdyB);



            var _gZfu = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "出线方式",PageType = "CabinetSingle", OptionCode = "outgoingMethod", OptionValue = new BaseOptionRangeItem() { DescEn = "Busbar top outgoing", DescZh = "电缆上出", Code = "cableTopOutgoing" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "出线方式",
    OptionCode = "outgoingMethod",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "CableTopOutgoing", DescZh = "电缆上出", Code = "cableTopOutgoing" },
         new BaseOptionRangeItem() { DescEn = "CableTopOutgoing", DescZh = "电缆上出", Code = "cableTopOutgoing" },
         new BaseOptionRangeItem() { DescEn = "CableTopOutgoing", DescZh = "电缆上出", Code = "cableTopOutgoing" },
         new BaseOptionRangeItem() { DescEn = "CableTopOutgoing", DescZh = "电缆上出", Code = "cableTopOutgoing" },
 } }
            }
            };
            list.Add(_gZfu);



            var _kAoL = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "出线方式",PageType = "CabinetSingle", OptionCode = "outgoingMethod", OptionValue = new BaseOptionRangeItem() { DescEn = "Busbar top outgoing", DescZh = "母排上出", Code = "busbarTopOutgoing" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "出线方式",
    OptionCode = "outgoingMethod",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "BusbarTopOutgoing", DescZh = "母排上出", Code = "busbarTopOutgoing" },
         new BaseOptionRangeItem() { DescEn = "BusbarTopOutgoing", DescZh = "母排上出", Code = "busbarTopOutgoing" },
         new BaseOptionRangeItem() { DescEn = "BusbarTopOutgoing", DescZh = "母排上出", Code = "busbarTopOutgoing" },
         new BaseOptionRangeItem() { DescEn = "BusbarTopOutgoing", DescZh = "母排上出", Code = "busbarTopOutgoing" },
 } }
            }
            };
            list.Add(_kAoL);



            var _IuFJ = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "垂直母线系统极数",PageType = "CabinetSingle", OptionCode = "polesOfVerticalBusbar", OptionValue = new BaseOptionRangeItem() { DescEn = "4P", DescZh = "4P", Code = "4P" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直母线系统极数",
    OptionCode = "polesOfVerticalBusbar",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "4P", DescZh = "4P", Code = "4P" },
 } }
            }
            };
            list.Add(_IuFJ);



            var _DFpG = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "垂直母线系统极数",PageType = "CabinetSingle", OptionCode = "polesOfVerticalBusbar", OptionValue = new BaseOptionRangeItem() { DescEn = "3P", DescZh = "3P", Code = "3P" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直母线系统极数",
    OptionCode = "polesOfVerticalBusbar",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "3P", DescZh = "3P", Code = "3P" },
 } }
            }
            };
            list.Add(_DFpG);



            var _itvO = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "垂直母线短时耐受",PageType = "CabinetSingle", OptionCode = "verticalBusBarIcw", OptionValue = new BaseOptionRangeItem() { DescEn = "30kA/1S", DescZh = "30kA/1S", Code = "30kA/1S" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直母线短时耐受",
    OptionCode = "verticalBusBarIcw",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "30kA/1S", DescZh = "30kA/1S", Code = "30kA/1S" },
 } }
            }
            };
            list.Add(_itvO);



            var _KuZr = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "垂直母线短时耐受",PageType = "CabinetSingle", OptionCode = "verticalBusBarIcw", OptionValue = new BaseOptionRangeItem() { DescEn = "40kA/1S", DescZh = "40kA/1S", Code = "40kA/1S" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直母线短时耐受",
    OptionCode = "verticalBusBarIcw",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "40kA/1S", DescZh = "40kA/1S", Code = "40kA/1S" },
 } }
            }
            };
            list.Add(_KuZr);



            var _qiqG = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "垂直母线短时耐受",PageType = "CabinetSingle", OptionCode = "verticalBusBarIcw", OptionValue = new BaseOptionRangeItem() { DescEn = "50kA/1S", DescZh = "50kA/1S", Code = "50kA/1S" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直母线短时耐受",
    OptionCode = "verticalBusBarIcw",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "50kA/1S", DescZh = "50kA/1S", Code = "50kA/1S" },
 } }
            }
            };
            list.Add(_qiqG);



            var _myoF = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "垂直母线短时耐受",PageType = "CabinetSingle", OptionCode = "verticalBusBarIcw", OptionValue = new BaseOptionRangeItem() { DescEn = "65kA/1S", DescZh = "65kA/1S", Code = "65kA/1S" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直母线短时耐受",
    OptionCode = "verticalBusBarIcw",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "65kA/1S", DescZh = "65kA/1S", Code = "65kA/1S" },
 } }
            }
            };
            list.Add(_myoF);



            var _pGiP = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "垂直母线短时耐受",PageType = "CabinetSingle", OptionCode = "verticalBusBarIcw", OptionValue = new BaseOptionRangeItem() { DescEn = "85kA/1S", DescZh = "85kA/1S", Code = "85kA/1S" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直母线短时耐受",
    OptionCode = "verticalBusBarIcw",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "85kA/1S", DescZh = "85kA/1S", Code = "85kA/1S" },
 } }
            }
            };
            list.Add(_pGiP);



            var _zEJv = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "垂直母线短时耐受",PageType = "CabinetSingle", OptionCode = "verticalBusBarIcw", OptionValue = new BaseOptionRangeItem() { DescEn = "30kA/3S", DescZh = "30kA/3S", Code = "30kA/3S" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直母线短时耐受",
    OptionCode = "verticalBusBarIcw",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "30kA/3S", DescZh = "30kA/3S", Code = "30kA/3S" },
 } }
            }
            };
            list.Add(_zEJv);



            var _FlCR = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "垂直母线短时耐受",PageType = "CabinetSingle", OptionCode = "verticalBusBarIcw", OptionValue = new BaseOptionRangeItem() { DescEn = "40kA/3S", DescZh = "40kA/3S", Code = "40kA/3S" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直母线短时耐受",
    OptionCode = "verticalBusBarIcw",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "40kA/3S", DescZh = "40kA/3S", Code = "40kA/3S" },
 } }
            }
            };
            list.Add(_FlCR);



            var _lsgA = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "垂直母线短时耐受",PageType = "CabinetSingle", OptionCode = "verticalBusBarIcw", OptionValue = new BaseOptionRangeItem() { DescEn = "50kA/3S", DescZh = "50kA/3S", Code = "50kA/3S" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直母线短时耐受",
    OptionCode = "verticalBusBarIcw",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "50kA/3S", DescZh = "50kA/3S", Code = "50kA/3S" },
 } }
            }
            };
            list.Add(_lsgA);



            var _MihF = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "垂直母线短时耐受",PageType = "CabinetSingle", OptionCode = "verticalBusBarIcw", OptionValue = new BaseOptionRangeItem() { DescEn = "65kA/3S", DescZh = "65kA/3S", Code = "65kA/3S" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直母线短时耐受",
    OptionCode = "verticalBusBarIcw",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "65kA/3S", DescZh = "65kA/3S", Code = "65kA/3S" },
 } }
            }
            };
            list.Add(_MihF);



            var _liUR = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "垂直母线短时耐受",PageType = "CabinetSingle", OptionCode = "verticalBusBarIcw", OptionValue = new BaseOptionRangeItem() { DescEn = "85kA/3S", DescZh = "85kA/3S", Code = "85kA/3S" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直母线短时耐受",
    OptionCode = "verticalBusBarIcw",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "85kA/3S", DescZh = "85kA/3S", Code = "85kA/3S" },
 } }
            }
            };
            list.Add(_liUR);



            var _mIKS = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "垂直母线载流(A)",PageType = "CabinetSingle", OptionCode = "verticalBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "630", DescZh = "630", Code = "630" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直母线载流(A)",
    OptionCode = "verticalBusbarCurrent",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "630", DescZh = "630", Code = "630" },
         new BaseOptionRangeItem() { DescEn = "630", DescZh = "630", Code = "630" },
 } }
            }
            };
            list.Add(_mIKS);



            var _xvRA = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "垂直母线载流(A)",PageType = "CabinetSingle", OptionCode = "verticalBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直母线载流(A)",
    OptionCode = "verticalBusbarCurrent",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
 } }
            }
            };
            list.Add(_xvRA);



            var _iIyW = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "垂直母线载流(A)",PageType = "CabinetSingle", OptionCode = "verticalBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直母线载流(A)",
    OptionCode = "verticalBusbarCurrent",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
 } }
            }
            };
            list.Add(_iIyW);



            var _vGGZ = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "垂直母线载流(A)",PageType = "CabinetSingle", OptionCode = "verticalBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直母线载流(A)",
    OptionCode = "verticalBusbarCurrent",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
         new BaseOptionRangeItem() { DescEn = "1200", DescZh = "1200", Code = "1200" },
 } }
            }
            };
            list.Add(_vGGZ);



            var _hsHx = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "垂直母线载流(A)",PageType = "CabinetSingle", OptionCode = "verticalBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直母线载流(A)",
    OptionCode = "verticalBusbarCurrent",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
         new BaseOptionRangeItem() { DescEn = "1500", DescZh = "1500", Code = "1500" },
 } }
            }
            };
            list.Add(_hsHx);



            var _RZgt = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "垂直母线载流(A)",PageType = "CabinetSingle", OptionCode = "verticalBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "1800", DescZh = "1800", Code = "1800" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直母线载流(A)",
    OptionCode = "verticalBusbarCurrent",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1800", DescZh = "1800", Code = "1800" },
         new BaseOptionRangeItem() { DescEn = "1800", DescZh = "1800", Code = "1800" },
 } }
            }
            };
            list.Add(_RZgt);



            var _aikZ = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "垂直母线载流(A)",PageType = "CabinetSingle", OptionCode = "verticalBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "2000", DescZh = "2000", Code = "2000" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直母线载流(A)",
    OptionCode = "verticalBusbarCurrent",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "2000", DescZh = "2000", Code = "2000" },
         new BaseOptionRangeItem() { DescEn = "2000", DescZh = "2000", Code = "2000" },
 } }
            }
            };
            list.Add(_aikZ);



            var _UDLJ = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "垂直母线载流(A)",PageType = "CabinetSingle", OptionCode = "verticalBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "2200", DescZh = "2200", Code = "2200" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直母线载流(A)",
    OptionCode = "verticalBusbarCurrent",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "2200", DescZh = "2200", Code = "2200" },
         new BaseOptionRangeItem() { DescEn = "2200", DescZh = "2200", Code = "2200" },
 } }
            }
            };
            list.Add(_UDLJ);



            var _EEmB = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "垂直母线载流(A)",PageType = "CabinetSingle", OptionCode = "verticalBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "2500", DescZh = "2500", Code = "2500" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直母线载流(A)",
    OptionCode = "verticalBusbarCurrent",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "2500", DescZh = "2500", Code = "2500" },
         new BaseOptionRangeItem() { DescEn = "2500", DescZh = "2500", Code = "2500" },
 } }
            }
            };
            list.Add(_EEmB);



            var _GNUW = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "垂直母线载流(A)",PageType = "CabinetSingle", OptionCode = "verticalBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "3200", DescZh = "3200", Code = "3200" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直母线载流(A)",
    OptionCode = "verticalBusbarCurrent",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "3200", DescZh = "3200", Code = "3200" },
         new BaseOptionRangeItem() { DescEn = "3200", DescZh = "3200", Code = "3200" },
 } }
            }
            };
            list.Add(_GNUW);



            var _yUWQ = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "垂直接地母线载流(A)",PageType = "CabinetSingle", OptionCode = "verticalGroundBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "315", DescZh = "315", Code = "315" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直接地母线载流(A)",
    OptionCode = "verticalGroundBusbarCurrent",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "315", DescZh = "315", Code = "315" },
 } }
            }
            };
            list.Add(_yUWQ);



            var _xUBb = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "垂直接地母线载流(A)",PageType = "CabinetSingle", OptionCode = "verticalGroundBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "400", DescZh = "400", Code = "400" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直接地母线载流(A)",
    OptionCode = "verticalGroundBusbarCurrent",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "400", DescZh = "400", Code = "400" },
 } }
            }
            };
            list.Add(_xUBb);



            var _FjlP = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "垂直接地母线载流(A)",PageType = "CabinetSingle", OptionCode = "verticalGroundBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直接地母线载流(A)",
    OptionCode = "verticalGroundBusbarCurrent",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
 } }
            }
            };
            list.Add(_FjlP);



            var _LLyJ = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "垂直接地母线载流(A)",PageType = "CabinetSingle", OptionCode = "verticalGroundBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直接地母线载流(A)",
    OptionCode = "verticalGroundBusbarCurrent",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
 } }
            }
            };
            list.Add(_LLyJ);



            var _EYFY = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "垂直接地母线载流(A)",PageType = "CabinetSingle", OptionCode = "verticalGroundBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "750", DescZh = "750", Code = "750" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直接地母线载流(A)",
    OptionCode = "verticalGroundBusbarCurrent",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "750", DescZh = "750", Code = "750" },
 } }
            }
            };
            list.Add(_EYFY);



            var _SjTx = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "垂直接地母线载流(A)",PageType = "CabinetSingle", OptionCode = "verticalGroundBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直接地母线载流(A)",
    OptionCode = "verticalGroundBusbarCurrent",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
 } }
            }
            };
            list.Add(_SjTx);



            var _sVMb = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "垂直接地母线载流(A)",PageType = "CabinetSingle", OptionCode = "verticalGroundBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直接地母线载流(A)",
    OptionCode = "verticalGroundBusbarCurrent",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
 } }
            }
            };
            list.Add(_sVMb);



            var _IxnD = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "垂直接地母线载流(A)",PageType = "CabinetSingle", OptionCode = "verticalGroundBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "1250", DescZh = "1250", Code = "1250" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直接地母线载流(A)",
    OptionCode = "verticalGroundBusbarCurrent",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1250", DescZh = "1250", Code = "1250" },
 } }
            }
            };
            list.Add(_IxnD);



            var _EaxT = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "垂直接地母线载流(A)",PageType = "CabinetSingle", OptionCode = "verticalGroundBusbarCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直接地母线载流(A)",
    OptionCode = "verticalGroundBusbarCurrent",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" },
 } }
            }
            };
            list.Add(_EaxT);



            var _Yzib = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "垂直中性母线载流(A)",PageType = "CabinetSingle", OptionCode = "verticalNeutralBusCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "315", DescZh = "315", Code = "315" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直中性母线载流(A)",
    OptionCode = "verticalNeutralBusCurrent",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "315", DescZh = "315", Code = "315" },
         new BaseOptionRangeItem() { DescEn = "315", DescZh = "315", Code = "315" },
 } }
            }
            };
            list.Add(_Yzib);



            var _XmpF = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "垂直中性母线载流(A)",PageType = "CabinetSingle", OptionCode = "verticalNeutralBusCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "400", DescZh = "400", Code = "400" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直中性母线载流(A)",
    OptionCode = "verticalNeutralBusCurrent",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "400", DescZh = "400", Code = "400" },
         new BaseOptionRangeItem() { DescEn = "400", DescZh = "400", Code = "400" },
 } }
            }
            };
            list.Add(_XmpF);



            var _kVtn = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "垂直中性母线载流(A)",PageType = "CabinetSingle", OptionCode = "verticalNeutralBusCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直中性母线载流(A)",
    OptionCode = "verticalNeutralBusCurrent",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
 } }
            }
            };
            list.Add(_kVtn);



            var _Nukg = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "垂直中性母线载流(A)",PageType = "CabinetSingle", OptionCode = "verticalNeutralBusCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直中性母线载流(A)",
    OptionCode = "verticalNeutralBusCurrent",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
         new BaseOptionRangeItem() { DescEn = "600", DescZh = "600", Code = "600" },
 } }
            }
            };
            list.Add(_Nukg);



            var _LSpt = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "垂直中性母线载流(A)",PageType = "CabinetSingle", OptionCode = "verticalNeutralBusCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "750", DescZh = "750", Code = "750" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直中性母线载流(A)",
    OptionCode = "verticalNeutralBusCurrent",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "750", DescZh = "750", Code = "750" },
         new BaseOptionRangeItem() { DescEn = "750", DescZh = "750", Code = "750" },
 } }
            }
            };
            list.Add(_LSpt);



            var _WPzQ = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "垂直中性母线载流(A)",PageType = "CabinetSingle", OptionCode = "verticalNeutralBusCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直中性母线载流(A)",
    OptionCode = "verticalNeutralBusCurrent",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
         new BaseOptionRangeItem() { DescEn = "900", DescZh = "900", Code = "900" },
 } }
            }
            };
            list.Add(_WPzQ);



            var _Neqk = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "垂直中性母线载流(A)",PageType = "CabinetSingle", OptionCode = "verticalNeutralBusCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直中性母线载流(A)",
    OptionCode = "verticalNeutralBusCurrent",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
 } }
            }
            };
            list.Add(_Neqk);



            var _ZDdN = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "垂直中性母线载流(A)",PageType = "CabinetSingle", OptionCode = "verticalNeutralBusCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "1250", DescZh = "1250", Code = "1250" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直中性母线载流(A)",
    OptionCode = "verticalNeutralBusCurrent",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1250", DescZh = "1250", Code = "1250" },
         new BaseOptionRangeItem() { DescEn = "1250", DescZh = "1250", Code = "1250" },
 } }
            }
            };
            list.Add(_ZDdN);



            var _nwLl = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "垂直中性母线载流(A)",PageType = "CabinetSingle", OptionCode = "verticalNeutralBusCurrent", OptionValue = new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "垂直中性母线载流(A)",
    OptionCode = "verticalNeutralBusCurrent",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" },
         new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" },
 } }
            }
            };
            list.Add(_nwLl);



            var _AyRL = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "水平主母线连接方向",PageType = "CabinetSingle", OptionCode = "mianHorizontalBusbarContectMethod", OptionValue = new BaseOptionRangeItem() { DescEn = "Left tie", DescZh = "左母联", Code = "leftTie" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "水平主母线连接方向",
    OptionCode = "mianHorizontalBusbarContectMethod",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "LeftTie", DescZh = "左母联", Code = "leftTie" },
 } }
            }
            };
            list.Add(_AyRL);



            var _IbZT = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "水平主母线连接方向",PageType = "CabinetSingle", OptionCode = "mianHorizontalBusbarContectMethod", OptionValue = new BaseOptionRangeItem() { DescEn = "Right tie", DescZh = "右母联", Code = "rightTie" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "水平主母线连接方向",
    OptionCode = "mianHorizontalBusbarContectMethod",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "RightTie", DescZh = "右母联", Code = "rightTie" },
 } }
            }
            };
            list.Add(_IbZT);



            var _eARA = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "仪表室模数(E)",PageType = "CabinetSingle", OptionCode = "modulusOfMeterRoom", OptionValue = new BaseOptionRangeItem() { DescEn = "12", DescZh = "12", Code = "12" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "仪表室模数(E)",
    OptionCode = "modulusOfMeterRoom",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "12", DescZh = "12", Code = "12" },
 } }
            }
            };
            list.Add(_eARA);



            var _nPsH = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "仪表室模数(E)",PageType = "CabinetSingle", OptionCode = "modulusOfMeterRoom", OptionValue = new BaseOptionRangeItem() { DescEn = "14", DescZh = "14", Code = "14" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "仪表室模数(E)",
    OptionCode = "modulusOfMeterRoom",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "14", DescZh = "14", Code = "14" },
 } }
            }
            };
            list.Add(_nPsH);



            var _bvsw = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "仪表室模数(E)",PageType = "CabinetSingle", OptionCode = "modulusOfMeterRoom", OptionValue = new BaseOptionRangeItem() { DescEn = "16", DescZh = "16", Code = "16" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "仪表室模数(E)",
    OptionCode = "modulusOfMeterRoom",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "16", DescZh = "16", Code = "16" },
 } }
            }
            };
            list.Add(_bvsw);



            var _dqEV = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "仪表室模数(E)",PageType = "CabinetSingle", OptionCode = "modulusOfMeterRoom", OptionValue = new BaseOptionRangeItem() { DescEn = "18", DescZh = "18", Code = "18" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "仪表室模数(E)",
    OptionCode = "modulusOfMeterRoom",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "18", DescZh = "18", Code = "18" },
 } }
            }
            };
            list.Add(_dqEV);



            var _tVoL = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "仪表室模数(E)",PageType = "CabinetSingle", OptionCode = "modulusOfMeterRoom", OptionValue = new BaseOptionRangeItem() { DescEn = "20", DescZh = "20", Code = "20" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "仪表室模数(E)",
    OptionCode = "modulusOfMeterRoom",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "20", DescZh = "20", Code = "20" },
 } }
            }
            };
            list.Add(_tVoL);



            var _LRrD = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "功能单元额定电流(A)",PageType = "FunctionUnit", OptionCode = "ratedCurrentOfFunctionUnit", OptionValue = new BaseOptionRangeItem() { DescEn = "7500", DescZh = "7500", Code = "7500" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "功能单元额定电流(A)",
    OptionCode = "ratedCurrentOfFunctionUnit",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "7500", DescZh = "7500", Code = "7500" },
         new BaseOptionRangeItem() { DescEn = "7500", DescZh = "7500", Code = "7500" },
         new BaseOptionRangeItem() { DescEn = "7500", DescZh = "7500", Code = "7500" },
 } }
            }
            };
            list.Add(_LRrD);



            var _zNfo = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "功能单元额定电流(A)",PageType = "FunctionUnit", OptionCode = "ratedCurrentOfFunctionUnit", OptionValue = new BaseOptionRangeItem() { DescEn = "6300", DescZh = "6300", Code = "6300" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "功能单元额定电流(A)",
    OptionCode = "ratedCurrentOfFunctionUnit",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "6300", DescZh = "6300", Code = "6300" },
         new BaseOptionRangeItem() { DescEn = "6300", DescZh = "6300", Code = "6300" },
         new BaseOptionRangeItem() { DescEn = "6300", DescZh = "6300", Code = "6300" },
 } }
            }
            };
            list.Add(_zNfo);



            var _GUiV = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "功能单元额定电流(A)",PageType = "FunctionUnit", OptionCode = "ratedCurrentOfFunctionUnit", OptionValue = new BaseOptionRangeItem() { DescEn = "5000", DescZh = "5000", Code = "5000" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "功能单元额定电流(A)",
    OptionCode = "ratedCurrentOfFunctionUnit",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "5000", DescZh = "5000", Code = "5000" },
         new BaseOptionRangeItem() { DescEn = "5000", DescZh = "5000", Code = "5000" },
         new BaseOptionRangeItem() { DescEn = "5000", DescZh = "5000", Code = "5000" },
 } }
            }
            };
            list.Add(_GUiV);



            var _EeHQ = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "功能单元额定电流(A)",PageType = "FunctionUnit", OptionCode = "ratedCurrentOfFunctionUnit", OptionValue = new BaseOptionRangeItem() { DescEn = "4000", DescZh = "4000", Code = "4000" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "功能单元额定电流(A)",
    OptionCode = "ratedCurrentOfFunctionUnit",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "4000", DescZh = "4000", Code = "4000" },
         new BaseOptionRangeItem() { DescEn = "4000", DescZh = "4000", Code = "4000" },
         new BaseOptionRangeItem() { DescEn = "4000", DescZh = "4000", Code = "4000" },
 } }
            }
            };
            list.Add(_EeHQ);



            var _McZF = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "功能单元额定电流(A)",PageType = "FunctionUnit", OptionCode = "ratedCurrentOfFunctionUnit", OptionValue = new BaseOptionRangeItem() { DescEn = "3200", DescZh = "3200", Code = "3200" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "功能单元额定电流(A)",
    OptionCode = "ratedCurrentOfFunctionUnit",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "3200", DescZh = "3200", Code = "3200" },
         new BaseOptionRangeItem() { DescEn = "3200", DescZh = "3200", Code = "3200" },
         new BaseOptionRangeItem() { DescEn = "3200", DescZh = "3200", Code = "3200" },
 } }
            }
            };
            list.Add(_McZF);



            var _lnTq = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "功能单元额定电流(A)",PageType = "FunctionUnit", OptionCode = "ratedCurrentOfFunctionUnit", OptionValue = new BaseOptionRangeItem() { DescEn = "2500", DescZh = "2500", Code = "2500" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "功能单元额定电流(A)",
    OptionCode = "ratedCurrentOfFunctionUnit",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "2500", DescZh = "2500", Code = "2500" },
         new BaseOptionRangeItem() { DescEn = "2500", DescZh = "2500", Code = "2500" },
         new BaseOptionRangeItem() { DescEn = "2500", DescZh = "2500", Code = "2500" },
 } }
            }
            };
            list.Add(_lnTq);



            var _ziiK = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "功能单元额定电流(A)",PageType = "FunctionUnit", OptionCode = "ratedCurrentOfFunctionUnit", OptionValue = new BaseOptionRangeItem() { DescEn = "2000", DescZh = "2000", Code = "2000" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "功能单元额定电流(A)",
    OptionCode = "ratedCurrentOfFunctionUnit",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "2000", DescZh = "2000", Code = "2000" },
         new BaseOptionRangeItem() { DescEn = "2000", DescZh = "2000", Code = "2000" },
         new BaseOptionRangeItem() { DescEn = "2000", DescZh = "2000", Code = "2000" },
 } }
            }
            };
            list.Add(_ziiK);



            var _lBza = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "功能单元额定电流(A)",PageType = "FunctionUnit", OptionCode = "ratedCurrentOfFunctionUnit", OptionValue = new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "功能单元额定电流(A)",
    OptionCode = "ratedCurrentOfFunctionUnit",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" },
         new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" },
         new BaseOptionRangeItem() { DescEn = "1600", DescZh = "1600", Code = "1600" },
 } }
            }
            };
            list.Add(_lBza);



            var _rHaJ = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "功能单元额定电流(A)",PageType = "FunctionUnit", OptionCode = "ratedCurrentOfFunctionUnit", OptionValue = new BaseOptionRangeItem() { DescEn = "1250", DescZh = "1250", Code = "1250" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "功能单元额定电流(A)",
    OptionCode = "ratedCurrentOfFunctionUnit",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1250", DescZh = "1250", Code = "1250" },
         new BaseOptionRangeItem() { DescEn = "1250", DescZh = "1250", Code = "1250" },
         new BaseOptionRangeItem() { DescEn = "1250", DescZh = "1250", Code = "1250" },
 } }
            }
            };
            list.Add(_rHaJ);



            var _soVT = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "功能单元额定电流(A)",PageType = "FunctionUnit", OptionCode = "ratedCurrentOfFunctionUnit", OptionValue = new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "功能单元额定电流(A)",
    OptionCode = "ratedCurrentOfFunctionUnit",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
         new BaseOptionRangeItem() { DescEn = "1000", DescZh = "1000", Code = "1000" },
 } }
            }
            };
            list.Add(_soVT);



            var _Jlww = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "功能单元额定电流(A)",PageType = "FunctionUnit", OptionCode = "ratedCurrentOfFunctionUnit", OptionValue = new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "功能单元额定电流(A)",
    OptionCode = "ratedCurrentOfFunctionUnit",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
         new BaseOptionRangeItem() { DescEn = "800", DescZh = "800", Code = "800" },
 } }
            }
            };
            list.Add(_Jlww);



            var _NaqC = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "功能单元额定电流(A)",PageType = "FunctionUnit", OptionCode = "ratedCurrentOfFunctionUnit", OptionValue = new BaseOptionRangeItem() { DescEn = "630", DescZh = "630", Code = "630" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "功能单元额定电流(A)",
    OptionCode = "ratedCurrentOfFunctionUnit",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "630", DescZh = "630", Code = "630" },
         new BaseOptionRangeItem() { DescEn = "630", DescZh = "630", Code = "630" },
         new BaseOptionRangeItem() { DescEn = "630", DescZh = "630", Code = "630" },
         new BaseOptionRangeItem() { DescEn = "630", DescZh = "630", Code = "630" },
         new BaseOptionRangeItem() { DescEn = "630", DescZh = "630", Code = "630" },
 } }
            }
            };
            list.Add(_NaqC);



            var _qhcf = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "功能单元额定电流(A)",PageType = "FunctionUnit", OptionCode = "ratedCurrentOfFunctionUnit", OptionValue = new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "功能单元额定电流(A)",
    OptionCode = "ratedCurrentOfFunctionUnit",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
         new BaseOptionRangeItem() { DescEn = "500", DescZh = "500", Code = "500" },
 } }
            }
            };
            list.Add(_qhcf);



            var _AOZF = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "功能单元额定电流(A)",PageType = "FunctionUnit", OptionCode = "ratedCurrentOfFunctionUnit", OptionValue = new BaseOptionRangeItem() { DescEn = "400", DescZh = "400", Code = "400" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "功能单元额定电流(A)",
    OptionCode = "ratedCurrentOfFunctionUnit",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "400", DescZh = "400", Code = "400" },
         new BaseOptionRangeItem() { DescEn = "400", DescZh = "400", Code = "400" },
         new BaseOptionRangeItem() { DescEn = "400", DescZh = "400", Code = "400" },
         new BaseOptionRangeItem() { DescEn = "400", DescZh = "400", Code = "400" },
         new BaseOptionRangeItem() { DescEn = "400", DescZh = "400", Code = "400" },
 } }
            }
            };
            list.Add(_AOZF);



            var _ctjS = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "功能单元额定电流(A)",PageType = "FunctionUnit", OptionCode = "ratedCurrentOfFunctionUnit", OptionValue = new BaseOptionRangeItem() { DescEn = "300", DescZh = "300", Code = "300" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "功能单元额定电流(A)",
    OptionCode = "ratedCurrentOfFunctionUnit",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "300", DescZh = "300", Code = "300" },
         new BaseOptionRangeItem() { DescEn = "300", DescZh = "300", Code = "300" },
 } }
            }
            };
            list.Add(_ctjS);



            var _dqjo = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "功能单元额定电流(A)",PageType = "FunctionUnit", OptionCode = "ratedCurrentOfFunctionUnit", OptionValue = new BaseOptionRangeItem() { DescEn = "250", DescZh = "250", Code = "250" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "功能单元额定电流(A)",
    OptionCode = "ratedCurrentOfFunctionUnit",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "250", DescZh = "250", Code = "250" },
         new BaseOptionRangeItem() { DescEn = "250", DescZh = "250", Code = "250" },
 } }
            }
            };
            list.Add(_dqjo);



            var _BxbS = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "功能单元额定电流(A)",PageType = "FunctionUnit", OptionCode = "ratedCurrentOfFunctionUnit", OptionValue = new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "功能单元额定电流(A)",
    OptionCode = "ratedCurrentOfFunctionUnit",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
         new BaseOptionRangeItem() { DescEn = "200", DescZh = "200", Code = "200" },
 } }
            }
            };
            list.Add(_BxbS);



            var _RIHP = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "功能单元额定电流(A)",PageType = "FunctionUnit", OptionCode = "ratedCurrentOfFunctionUnit", OptionValue = new BaseOptionRangeItem() { DescEn = "125", DescZh = "125", Code = "125" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "功能单元额定电流(A)",
    OptionCode = "ratedCurrentOfFunctionUnit",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "125", DescZh = "125", Code = "125" },
         new BaseOptionRangeItem() { DescEn = "125", DescZh = "125", Code = "125" },
 } }
            }
            };
            list.Add(_RIHP);



            var _Ihyq = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "功能单元额定电流(A)",PageType = "FunctionUnit", OptionCode = "ratedCurrentOfFunctionUnit", OptionValue = new BaseOptionRangeItem() { DescEn = "63", DescZh = "63", Code = "63" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "功能单元额定电流(A)",
    OptionCode = "ratedCurrentOfFunctionUnit",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "63", DescZh = "63", Code = "63" },
         new BaseOptionRangeItem() { DescEn = "63", DescZh = "63", Code = "63" },
 } }
            }
            };
            list.Add(_Ihyq);



            var _rEBy = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "功能单元极数",PageType = "FunctionUnit", OptionCode = "polesOfFunctionUnit", OptionValue = new BaseOptionRangeItem() { DescEn = "3P", DescZh = "3P", Code = "3P" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "功能单元极数",
    OptionCode = "polesOfFunctionUnit",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "3P", DescZh = "3P", Code = "3P" },
         new BaseOptionRangeItem() { DescEn = "3P", DescZh = "3P", Code = "3P" },
         new BaseOptionRangeItem() { DescEn = "3P", DescZh = "3P", Code = "3P" },
         new BaseOptionRangeItem() { DescEn = "3P", DescZh = "3P", Code = "3P" },
         new BaseOptionRangeItem() { DescEn = "3P", DescZh = "3P", Code = "3P" },
 } }
            }
            };
            list.Add(_rEBy);



            var _cLmC = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "功能单元极数",PageType = "FunctionUnit", OptionCode = "polesOfFunctionUnit", OptionValue = new BaseOptionRangeItem() { DescEn = "4P", DescZh = "4P", Code = "4P" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "功能单元极数",
    OptionCode = "polesOfFunctionUnit",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "4P", DescZh = "4P", Code = "4P" },
         new BaseOptionRangeItem() { DescEn = "4P", DescZh = "4P", Code = "4P" },
         new BaseOptionRangeItem() { DescEn = "4P", DescZh = "4P", Code = "4P" },
         new BaseOptionRangeItem() { DescEn = "4P", DescZh = "4P", Code = "4P" },
         new BaseOptionRangeItem() { DescEn = "4P", DescZh = "4P", Code = "4P" },
 } }
            }
            };
            list.Add(_cLmC);



            var _VaWl = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "功能单元极数",PageType = "FunctionUnit", OptionCode = "polesOfFunctionUnit", OptionValue = new BaseOptionRangeItem() { DescEn = "3P+N", DescZh = "3P+N", Code = "3P+N" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "功能单元极数",
    OptionCode = "polesOfFunctionUnit",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "3P+N", DescZh = "3P+N", Code = "3P+N" },
         new BaseOptionRangeItem() { DescEn = "3P+N", DescZh = "3P+N", Code = "3P+N" },
         new BaseOptionRangeItem() { DescEn = "3P+N", DescZh = "3P+N", Code = "3P+N" },
         new BaseOptionRangeItem() { DescEn = "3P+N", DescZh = "3P+N", Code = "3P+N" },
         new BaseOptionRangeItem() { DescEn = "3P+N", DescZh = "3P+N", Code = "3P+N" },
 } }
            }
            };
            list.Add(_VaWl);



            var _MawT = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "功能单元安装方式",PageType = "FunctionUnit", OptionCode = "functionInstallationType", OptionValue = new BaseOptionRangeItem() { DescEn = "ACB withdraw", DescZh = "ACB抽出式", Code = "acbWithDraw" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "功能单元安装方式",
    OptionCode = "functionInstallationType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "ACB withdraw", DescZh = "ACB抽出式", Code = "acbWithDraw" },
         new BaseOptionRangeItem() { DescEn = "ACB withdraw", DescZh = "ACB抽出式", Code = "acbWithDraw" },
         new BaseOptionRangeItem() { DescEn = "ACB withdraw", DescZh = "ACB抽出式", Code = "acbWithDraw" },
         new BaseOptionRangeItem() { DescEn = "ACB withdraw", DescZh = "ACB抽出式", Code = "acbWithDraw" },
 } }
            }
            };
            list.Add(_MawT);



            var _aztX = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "功能单元安装方式",PageType = "FunctionUnit", OptionCode = "functionInstallationType", OptionValue = new BaseOptionRangeItem() { DescEn = "ACB fixed", DescZh = "ACB固定式", Code = "acbFixed" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "功能单元安装方式",
    OptionCode = "functionInstallationType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "ACB fixed", DescZh = "ACB固定式", Code = "acbFixed" },
         new BaseOptionRangeItem() { DescEn = "ACB fixed", DescZh = "ACB固定式", Code = "acbFixed" },
         new BaseOptionRangeItem() { DescEn = "ACB fixed", DescZh = "ACB固定式", Code = "acbFixed" },
         new BaseOptionRangeItem() { DescEn = "ACB fixed", DescZh = "ACB固定式", Code = "acbFixed" },
 } }
            }
            };
            list.Add(_aztX);



            var _GnVb = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "功能单元安装方式",PageType = "FunctionUnit", OptionCode = "functionInstallationType", OptionValue = new BaseOptionRangeItem() { DescEn = "MCCB withdraw", DescZh = "MCCB抽屉式", Code = "mccbWithDraw" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "功能单元安装方式",
    OptionCode = "functionInstallationType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "MCCB withdraw", DescZh = "MCCB抽屉式", Code = "mccbWithDraw" },
         new BaseOptionRangeItem() { DescEn = "MCCB withdraw", DescZh = "MCCB抽屉式", Code = "mccbWithDraw" },
 } }
            }
            };
            list.Add(_GnVb);



            var _eLZv = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "功能单元安装方式",PageType = "FunctionUnit", OptionCode = "functionInstallationType", OptionValue = new BaseOptionRangeItem() { DescEn = "MCCB fixed separately", DescZh = "MCCB固定分隔式", Code = "mccbFixedSeparately" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "功能单元安装方式",
    OptionCode = "functionInstallationType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "MCCB fixed separately", DescZh = "MCCB固定分隔式", Code = "mccbFixedSeparately" },
         new BaseOptionRangeItem() { DescEn = "MCCB fixed separately", DescZh = "MCCB固定分隔式", Code = "mccbFixedSeparately" },
 } }
            }
            };
            list.Add(_eLZv);



            var _GNgG = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "功能单元安装方式",PageType = "FunctionUnit", OptionCode = "functionInstallationType", OptionValue = new BaseOptionRangeItem() { DescEn = "MCCB fixed", DescZh = "MCCB固定式", Code = "mccbFixed" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "功能单元安装方式",
    OptionCode = "functionInstallationType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "MCCB fixed", DescZh = "MCCB固定式", Code = "mccbFixed" },
         new BaseOptionRangeItem() { DescEn = "MCCB fixed", DescZh = "MCCB固定式", Code = "mccbFixed" },
 } }
            }
            };
            list.Add(_GNgG);



            var _eyUu = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "功能单元安装方式",PageType = "FunctionUnit", OptionCode = "functionInstallationType", OptionValue = new BaseOptionRangeItem() { DescEn = "MCB fixed", DescZh = "MCB固定式", Code = "mcbFixed" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "功能单元安装方式",
    OptionCode = "functionInstallationType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "MCB fixed", DescZh = "MCB固定式", Code = "mcbFixed" },
         new BaseOptionRangeItem() { DescEn = "MCB fixed", DescZh = "MCB固定式", Code = "mcbFixed" },
 } }
            }
            };
            list.Add(_eyUu);



            var _nWdy = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "功能单元安装方式",PageType = "FunctionUnit", OptionCode = "functionInstallationType", OptionValue = new BaseOptionRangeItem() { DescEn = "MCB withdraw", DescZh = "MCB抽屉式", Code = "mcbWithDraw" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "功能单元安装方式",
    OptionCode = "functionInstallationType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "MCB withdraw", DescZh = "MCB抽屉式", Code = "mcbWithDraw" },
         new BaseOptionRangeItem() { DescEn = "MCB withdraw", DescZh = "MCB抽屉式", Code = "mcbWithDraw" },
 } }
            }
            };
            list.Add(_nWdy);



            var _nUCl = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "模数(E)",PageType = "FunctionUnit", OptionCode = "modular", OptionValue = new BaseOptionRangeItem() { DescEn = "6/2", DescZh = "6/2", Code = "6/2" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "模数(E)",
    OptionCode = "modular",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "6/2", DescZh = "6/2", Code = "6/2" },
         new BaseOptionRangeItem() { DescEn = "6/2", DescZh = "6/2", Code = "6/2" },
         new BaseOptionRangeItem() { DescEn = "6/2", DescZh = "6/2", Code = "6/2" },
         new BaseOptionRangeItem() { DescEn = "6/2", DescZh = "6/2", Code = "6/2" },
         new BaseOptionRangeItem() { DescEn = "6/2", DescZh = "6/2", Code = "6/2" },
 } }
            }
            };
            list.Add(_nUCl);



            var _WGtf = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "模数(E)",PageType = "FunctionUnit", OptionCode = "modular", OptionValue = new BaseOptionRangeItem() { DescEn = "8/2", DescZh = "8/2", Code = "8/2" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "模数(E)",
    OptionCode = "modular",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "8/2", DescZh = "8/2", Code = "8/2" },
         new BaseOptionRangeItem() { DescEn = "8/2", DescZh = "8/2", Code = "8/2" },
         new BaseOptionRangeItem() { DescEn = "8/2", DescZh = "8/2", Code = "8/2" },
         new BaseOptionRangeItem() { DescEn = "8/2", DescZh = "8/2", Code = "8/2" },
         new BaseOptionRangeItem() { DescEn = "8/2", DescZh = "8/2", Code = "8/2" },
 } }
            }
            };
            list.Add(_WGtf);



            var _smfH = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "模数(E)",PageType = "FunctionUnit", OptionCode = "modular", OptionValue = new BaseOptionRangeItem() { DescEn = "6", DescZh = "6", Code = "6" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "模数(E)",
    OptionCode = "modular",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "6", DescZh = "6", Code = "6" },
         new BaseOptionRangeItem() { DescEn = "6", DescZh = "6", Code = "6" },
         new BaseOptionRangeItem() { DescEn = "6", DescZh = "6", Code = "6" },
         new BaseOptionRangeItem() { DescEn = "6", DescZh = "6", Code = "6" },
         new BaseOptionRangeItem() { DescEn = "6", DescZh = "6", Code = "6" },
 } }
            }
            };
            list.Add(_smfH);



            var _YZlO = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "模数(E)",PageType = "FunctionUnit", OptionCode = "modular", OptionValue = new BaseOptionRangeItem() { DescEn = "8", DescZh = "8", Code = "8" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "模数(E)",
    OptionCode = "modular",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "8", DescZh = "8", Code = "8" },
         new BaseOptionRangeItem() { DescEn = "8", DescZh = "8", Code = "8" },
         new BaseOptionRangeItem() { DescEn = "8", DescZh = "8", Code = "8" },
         new BaseOptionRangeItem() { DescEn = "8", DescZh = "8", Code = "8" },
         new BaseOptionRangeItem() { DescEn = "8", DescZh = "8", Code = "8" },
 } }
            }
            };
            list.Add(_YZlO);



            var _Gvql = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "模数(E)",PageType = "FunctionUnit", OptionCode = "modular", OptionValue = new BaseOptionRangeItem() { DescEn = "12", DescZh = "12", Code = "12" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "模数(E)",
    OptionCode = "modular",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "12", DescZh = "12", Code = "12" },
         new BaseOptionRangeItem() { DescEn = "12", DescZh = "12", Code = "12" },
         new BaseOptionRangeItem() { DescEn = "12", DescZh = "12", Code = "12" },
         new BaseOptionRangeItem() { DescEn = "12", DescZh = "12", Code = "12" },
         new BaseOptionRangeItem() { DescEn = "12", DescZh = "12", Code = "12" },
 } }
            }
            };
            list.Add(_Gvql);



            var _kvgk = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "模数(E)",PageType = "FunctionUnit", OptionCode = "modular", OptionValue = new BaseOptionRangeItem() { DescEn = "16", DescZh = "16", Code = "16" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "模数(E)",
    OptionCode = "modular",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "16", DescZh = "16", Code = "16" },
         new BaseOptionRangeItem() { DescEn = "16", DescZh = "16", Code = "16" },
         new BaseOptionRangeItem() { DescEn = "16", DescZh = "16", Code = "16" },
         new BaseOptionRangeItem() { DescEn = "16", DescZh = "16", Code = "16" },
         new BaseOptionRangeItem() { DescEn = "16", DescZh = "16", Code = "16" },
 } }
            }
            };
            list.Add(_kvgk);



            var _XgRV = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "模数(E)",PageType = "FunctionUnit", OptionCode = "modular", OptionValue = new BaseOptionRangeItem() { DescEn = "20", DescZh = "20", Code = "20" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "模数(E)",
    OptionCode = "modular",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "20", DescZh = "20", Code = "20" },
         new BaseOptionRangeItem() { DescEn = "20", DescZh = "20", Code = "20" },
         new BaseOptionRangeItem() { DescEn = "20", DescZh = "20", Code = "20" },
         new BaseOptionRangeItem() { DescEn = "20", DescZh = "20", Code = "20" },
         new BaseOptionRangeItem() { DescEn = "20", DescZh = "20", Code = "20" },
 } }
            }
            };
            list.Add(_XgRV);



            var _KLYk = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "模数(E)",PageType = "FunctionUnit", OptionCode = "modular", OptionValue = new BaseOptionRangeItem() { DescEn = "24", DescZh = "24", Code = "24" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "模数(E)",
    OptionCode = "modular",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "24", DescZh = "24", Code = "24" },
         new BaseOptionRangeItem() { DescEn = "24", DescZh = "24", Code = "24" },
         new BaseOptionRangeItem() { DescEn = "24", DescZh = "24", Code = "24" },
         new BaseOptionRangeItem() { DescEn = "24", DescZh = "24", Code = "24" },
         new BaseOptionRangeItem() { DescEn = "24", DescZh = "24", Code = "24" },
 } }
            }
            };
            list.Add(_KLYk);



            var _EYFX = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "模数(E)",PageType = "FunctionUnit", OptionCode = "modular", OptionValue = new BaseOptionRangeItem() { DescEn = "28", DescZh = "28", Code = "28" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "模数(E)",
    OptionCode = "modular",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "28", DescZh = "28", Code = "28" },
         new BaseOptionRangeItem() { DescEn = "28", DescZh = "28", Code = "28" },
         new BaseOptionRangeItem() { DescEn = "28", DescZh = "28", Code = "28" },
         new BaseOptionRangeItem() { DescEn = "28", DescZh = "28", Code = "28" },
         new BaseOptionRangeItem() { DescEn = "28", DescZh = "28", Code = "28" },
 } }
            }
            };
            list.Add(_EYFX);



            var _iChw = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "模数(E)",PageType = "FunctionUnit", OptionCode = "modular", OptionValue = new BaseOptionRangeItem() { DescEn = "32", DescZh = "32", Code = "32" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "模数(E)",
    OptionCode = "modular",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "32", DescZh = "32", Code = "32" },
         new BaseOptionRangeItem() { DescEn = "32", DescZh = "32", Code = "32" },
         new BaseOptionRangeItem() { DescEn = "32", DescZh = "32", Code = "32" },
         new BaseOptionRangeItem() { DescEn = "32", DescZh = "32", Code = "32" },
         new BaseOptionRangeItem() { DescEn = "32", DescZh = "32", Code = "32" },
 } }
            }
            };
            list.Add(_iChw);



            var _EgBT = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "水平主母线位置",PageType = "CabinetGroup", OptionCode = "mainBusbarPosition", OptionValue = new BaseOptionRangeItem() { DescEn = "Upper front", DescZh = "顶部前", Code = "upperFront" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "水平主母线位置",
    OptionCode = "mainBusbarPosition",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "UpperFront", DescZh = "顶部前", Code = "upperFront" },
 } }
            }
            };
            list.Add(_EgBT);



            var _syAS = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "水平主母线位置",PageType = "CabinetGroup", OptionCode = "mainBusbarPosition", OptionValue = new BaseOptionRangeItem() { DescEn = "Upper rear", DescZh = "顶部后", Code = "upperRear" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "水平主母线位置",
    OptionCode = "mainBusbarPosition",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "UpperRear", DescZh = "顶部后", Code = "upperRear" },
 } }
            }
            };
            list.Add(_syAS);



            var _ePLv = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "水平主母线位置",PageType = "CabinetGroup", OptionCode = "mainBusbarPosition", OptionValue = new BaseOptionRangeItem() { DescEn = "Rear", DescZh = "后部", Code = "rear" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "水平主母线位置",
    OptionCode = "mainBusbarPosition",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "Rear", DescZh = "后部", Code = "rear" },
 } }
            }
            };
            list.Add(_ePLv);



            var _MOte = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "维护方式",PageType = "CabinetGroup", OptionCode = "maintenanceMethods", OptionValue = new BaseOptionRangeItem() { DescEn = "Front maintenance", DescZh = "前维护", Code = "frontMaintenance" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "后门类型",
    OptionCode = "rearDoorType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "Cover", DescZh = "盖板", Code = "cover" },
 } }
            }
            };
            list.Add(_MOte);



            var _mwhn = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "维护方式",PageType = "CabinetGroup", OptionCode = "maintenanceMethods", OptionValue = new BaseOptionRangeItem() { DescEn = "Rear maintenance", DescZh = "后维护", Code = "rearMaintenance" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "后门类型",
    OptionCode = "rearDoorType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "Door", DescZh = "门板", Code = "door" },
 } }
            }
            };
            list.Add(_mwhn);



            var _PIaV = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "功能单元类型",PageType = "FunctionUnit", OptionCode = "functionUnitType", OptionValue = new BaseOptionRangeItem() { DescEn = "ACB", DescZh = "ACB", Code = "acb" }  },
   new OptionsItem()
   { OptionNameZh = "断路器安装方式",PageType = "FunctionUnit", OptionCode = "installationTypeOfCB", OptionValue = new BaseOptionRangeItem() { DescEn = "Withdrawabled", DescZh = "抽出式", Code = "withDrawable" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "功能单元安装方式",
    OptionCode = "functionInstallationType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "ACB withdraw", DescZh = "ACB抽出式", Code = "acbWithDraw" },
         new BaseOptionRangeItem() { DescEn = "ACB withdraw", DescZh = "ACB抽出式", Code = "acbWithDraw" },
         new BaseOptionRangeItem() { DescEn = "ACB withdraw", DescZh = "ACB抽出式", Code = "acbWithDraw" },
         new BaseOptionRangeItem() { DescEn = "ACB withdraw", DescZh = "ACB抽出式", Code = "acbWithDraw" },
 } }
            }
            };
            list.Add(_PIaV);



            var _jqmg = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "功能单元类型",PageType = "FunctionUnit", OptionCode = "functionUnitType", OptionValue = new BaseOptionRangeItem() { DescEn = "ACB", DescZh = "ACB", Code = "acb" }  },
   new OptionsItem()
   { OptionNameZh = "断路器安装方式",PageType = "FunctionUnit", OptionCode = "installationTypeOfCB", OptionValue = new BaseOptionRangeItem() { DescEn = "Fixed", DescZh = "固定式", Code = "fixed" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "功能单元安装方式",
    OptionCode = "functionInstallationType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "ACB fixed", DescZh = "ACB固定式", Code = "acbFixed" },
         new BaseOptionRangeItem() { DescEn = "ACB fixed", DescZh = "ACB固定式", Code = "acbFixed" },
         new BaseOptionRangeItem() { DescEn = "ACB fixed", DescZh = "ACB固定式", Code = "acbFixed" },
         new BaseOptionRangeItem() { DescEn = "ACB fixed", DescZh = "ACB固定式", Code = "acbFixed" },
 } }
            }
            };
            list.Add(_jqmg);



            var _fBBu = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "维护方式",PageType = "CabinetGroup", OptionCode = "maintenanceMethods", OptionValue = new BaseOptionRangeItem() { DescEn = "Front maintenance", DescZh = "前维护", Code = "frontMaintenance" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "维护方式",
    OptionCode = "maintenanceMethod",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "FrontMaintenance", DescZh = "前维护", Code = "frontMaintenance" },
 } }
            }
            };
            list.Add(_fBBu);



            var _IlZl = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "维护方式",PageType = "CabinetGroup", OptionCode = "maintenanceMethods", OptionValue = new BaseOptionRangeItem() { DescEn = "Rear maintenance", DescZh = "后维护", Code = "rearMaintenance" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "维护方式",
    OptionCode = "maintenanceMethod",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "RearMaintenance", DescZh = "后维护", Code = "rearMaintenance" },
 } }
            }
            };
            list.Add(_IlZl);



            var _VlOr = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "进线方式",PageType = "CabinetSingle", OptionCode = "incomingType", OptionValue = new BaseOptionRangeItem() { DescEn = "Busbar top incoming", DescZh = "母排上进", Code = "busbarTopIncoming" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "进出线方式",
    OptionCode = "incomingAndOutGoingType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "BusbarTopIncoming", DescZh = "母排上进", Code = "busbarTopIncoming" },
 } }
            }
            };
            list.Add(_VlOr);



            var _KjAS = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "进线方式",PageType = "CabinetSingle", OptionCode = "incomingType", OptionValue = new BaseOptionRangeItem() { DescEn = "Busbar side incoming", DescZh = "母排侧进", Code = "busbarSideIncoming" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "进出线方式",
    OptionCode = "incomingAndOutGoingType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "BusbarSideIncoming", DescZh = "母排侧进", Code = "busbarSideIncoming" },
 } }
            }
            };
            list.Add(_KjAS);



            var _RhqI = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "进线方式",PageType = "CabinetSingle", OptionCode = "incomingType", OptionValue = new BaseOptionRangeItem() { DescEn = "Cable top incoming", DescZh = "电缆上进", Code = "cableTopIncoming" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "进出线方式",
    OptionCode = "incomingAndOutGoingType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "CableTopIncoming", DescZh = "电缆上进", Code = "cableTopIncoming" },
 } }
            }
            };
            list.Add(_RhqI);



            var _BRev = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "进线方式",PageType = "CabinetSingle", OptionCode = "incomingType", OptionValue = new BaseOptionRangeItem() { DescEn = "Cable bottom incoming", DescZh = "电缆下进", Code = "cableBottomIncoming" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "进出线方式",
    OptionCode = "incomingAndOutGoingType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "CableBottomIncoming", DescZh = "电缆下进", Code = "cableBottomIncoming" },
 } }
            }
            };
            list.Add(_BRev);



            var _xDqi = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "出线方式",PageType = "CabinetSingle", OptionCode = "outgoingMethod", OptionValue = new BaseOptionRangeItem() { DescEn = "Busbar top outgoing", DescZh = "电缆上出", Code = "cableTopOutgoing" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "进出线方式",
    OptionCode = "incomingAndOutGoingType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "CableTopOutgoing", DescZh = "电缆上出", Code = "cableTopOutgoing" },
 } }
            }
            };
            list.Add(_xDqi);



            var _ZczX = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "出线方式",PageType = "CabinetSingle", OptionCode = "outgoingMethod", OptionValue = new BaseOptionRangeItem() { DescEn = "Cable top outgoing", DescZh = "电缆下出", Code = "cableBottomOutgoing" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "进出线方式",
    OptionCode = "incomingAndOutGoingType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "CableBottomOutgoing", DescZh = "电缆下出", Code = "cableBottomOutgoing" },
 } }
            }
            };
            list.Add(_ZczX);



            var _HwTr = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "出线方式",PageType = "CabinetSingle", OptionCode = "outgoingMethod", OptionValue = new BaseOptionRangeItem() { DescEn = "Busbar top outgoing", DescZh = "母排上出", Code = "busbarTopOutgoing" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "进出线方式",
    OptionCode = "incomingAndOutGoingType",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "BusbarTopOutgoing", DescZh = "母排上出", Code = "busbarTopOutgoing" },
 } }
            }
            };
            list.Add(_HwTr);



            var _dpaQ = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "单/双层门",PageType = "CabinetGroup", OptionCode = "doorType", OptionValue = new BaseOptionRangeItem() { DescEn = "Single", DescZh = "单层门", Code = "single" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "并柜排长度(mm)",
    OptionCode = "connectionBarLength",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "110", DescZh = "110", Code = "110" },
 } }
            }
            };
            list.Add(_dpaQ);



            var _WLAi = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                                    {
   new OptionsItem()
   { OptionNameZh = "单/双层门",PageType = "CabinetGroup", OptionCode = "doorType", OptionValue = new BaseOptionRangeItem() { DescEn = "Double", DescZh = "双层门", Code = "double" }  },}
                },
                Actions = new List<ActionsItem>()
 {
   new ActionsItem()
     {
   OptionNameZh = "并柜排长度(mm)",
    OptionCode = "connectionBarLength",
     OptionValueRanges = new List<BaseOptionRangeItem>()
     {
         new BaseOptionRangeItem() { DescEn = "150", DescZh = "150", Code = "150" },
 } }
            }
            };
            list.Add(_WLAi);



            return list;
        }


        #endregion
    }

    #endregion

    #region model
    public class OptionsConstraintCabinetEntitySpec
    {
        public Condition Condition { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<ActionsItem> Actions { get; set; }
    }

    public class ActionsItem
    {
        public string OptionNameZh { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string OptionCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<BaseOptionRangeItem> OptionValueRanges { get; set; }
    }

    public class Condition
    {
        /// <summary>
        /// 
        /// </summary>
        public List<OptionsItem> Options { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Logic { get; set; }
    }

    public class OptionsItem
    {

        public string OptionNameZh { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public string OptionCode { get; set; }

        public string PageType { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public BaseOptionRangeItem OptionValue { get; set; }
    }


    public class BaseOption
    {
        /// <summary>
        /// 输入类型input/select
        /// </summary>
        public required string OptionType { set; get; }

        /// <summary>
        /// 规格名称
        /// </summary>
        public required string NameZh { get; set; }

        /// <summary>
        /// 规格名称
        /// </summary>
        public required string NameEn { get; set; }

        /// <summary>
        /// 规格代码
        /// </summary>
        public required string Code { get; set; }

        /// <summary>
        /// 规格范围
        /// </summary>
        public required ICollection<BaseOptionRangeItem> Ranges { get; set; }
    }


    /// <summary>
    /// 基础规格项
    /// </summary>
    public class BaseOptionRangeItem
    {
        /// <summary>
        /// 规格值
        /// </summary>
        public required string Code { set; get; }

        /// <summary>
        /// 规格值中文描述
        /// </summary>
        public required string DescZh { get; set; }

        /// <summary>
        /// 规格值英文描述
        /// </summary>
        public required string DescEn { get; set; }
        /// <summary>
        /// 规划中的选型值，暂时隐藏,等BOM有数据了，则去掉这个隐藏字段
        /// </summary>
        public bool IsHide { get; set; }
    }
    #endregion
}
