using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace 获取Action中Item的数据源
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<OptionsConstraintCabinetEntitySpec> source = OptionsConstraintCabinetEntitySpecSource2.GetSource();

            //1.需要因素：柜体高度(mm)	整体深度(mm)	防护等级	是否开孔	水平主母线位置	水平主母线规格(mm)	左/右端封板
            var productCode = "whetherWithHole";//是否开孔

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
                .GroupBy(spec => string.Join("_", spec.Condition.Options.Select(option => option.OptionCode)))
                .ToDictionary(g => g.Key, g => g.ToList());


            Dictionary<string, List<BaseOptionRangeItem>> save = new Dictionary<string, List<BaseOptionRangeItem>>();

            //组柜参数
            //var groupEntityDefineParas = groupEntity.DefineParas;
            foreach (var spec in groupedData)
            {
                var key = spec.Key;//进线方式 或者 ACB下端子方案

                //strCode是界面上，客户选择的值
                var strCode = "busbarTopIncoming";

                var sd = spec.Value;

                var filteredSpecs = sd
                                   .Where(spec => spec.Condition.Options
                                   .Any(option => option.OptionValue.Code == strCode))
                                   .ToList();

                if (filteredSpecs.Count > 1)
                {
                    throw new Exception("这里是映射错误，比如【进线方式】的【母排上进】对应的M列有多个值");
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

                var d = dd.Count;

                save.Add(key, dd);
            }

            if (save.Count > 0)
            {
                var firstCodeList = save.First().Value.Select(item => item.Code).ToList();

                // 比较 save 中每个 List<BaseOptionRangeItem> 的 Code 值是否与 firstCodeList 一致
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
                            { "mergedKey", save.First().Value }
                        };
                }
            }




            Console.WriteLine(filteredList);
        }
    }

    #region 逻辑

    public class OptionsConstraintCabinetEntitySpecSource2
    {

        public static List<OptionsConstraintCabinetEntitySpec> GetSource()
        {

            List<OptionsConstraintCabinetEntitySpec> list = new List<OptionsConstraintCabinetEntitySpec>();
            var _msvD = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                {
                    new OptionsItem()
                    {
                        OptionNameZh = "进线方式", PageType = "CabinetSingle", OptionCode = "incomingType", OptionValue = new BaseOptionRangeItem()
                        {
                            DescEn = "Busbar top incoming", DescZh = "母排上进", Code = "busbarTopIncoming"
                        }
                    },
                }
                },
                Actions = new List<ActionsItem>()
        {
            new ActionsItem()
            {
                OptionNameZh = "是否开孔",
                    OptionCode = "whetherWithHole",
                    OptionValueRanges = new List < BaseOptionRangeItem > ()
                    {
                        new BaseOptionRangeItem()
                        {
                            //DescEn = "", DescZh = "", Code = ""
                             DescEn = "No", DescZh = "否", Code = "no"
                        },
                    }
            }
        }
            };
            list.Add(_msvD);
            var _rfvJ = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                {
                    new OptionsItem()
                    {
                        OptionNameZh = "进线方式", PageType = "CabinetSingle", OptionCode = "incomingType", OptionValue = new BaseOptionRangeItem()
                        {
                            DescEn = "Busbar side incoming", DescZh = "母排侧进", Code = "busbarSideIncoming"
                        }
                    },
                }
                },
                Actions = new List<ActionsItem>()
        {
            new ActionsItem()
            {
                OptionNameZh = "是否开孔",
                    OptionCode = "whetherWithHole",
                    OptionValueRanges = new List < BaseOptionRangeItem > ()
                    {
                        new BaseOptionRangeItem()
                        {
                            DescEn = "Yes", DescZh = "是", Code = "yes"
                        },
                    }
            }
        }
            };
            list.Add(_rfvJ);
            var _hfVE = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                {
                    new OptionsItem()
                    {
                        OptionNameZh = "进线方式", PageType = "CabinetSingle", OptionCode = "incomingType", OptionValue = new BaseOptionRangeItem()
                        {
                            DescEn = "Cable top incoming", DescZh = "电缆上进", Code = "cableTopIncoming"
                        }
                    },
                }
                },
                Actions = new List<ActionsItem>()
        {
            new ActionsItem()
            {
                OptionNameZh = "是否开孔",
                    OptionCode = "whetherWithHole",
                    OptionValueRanges = new List < BaseOptionRangeItem > ()
                    {
                        new BaseOptionRangeItem()
                        {
                            DescEn = "No", DescZh = "否", Code = "no"
                        },
                    }
            }
        }
            };
            list.Add(_hfVE);
            var _xhkB = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                {
                    new OptionsItem()
                    {
                        OptionNameZh = "进线方式", PageType = "CabinetSingle", OptionCode = "incomingType", OptionValue = new BaseOptionRangeItem()
                        {
                            DescEn = "Cable bottom incoming", DescZh = "电缆下进", Code = "cableBottomIncoming"
                        }
                    },
                }
                },
                Actions = new List<ActionsItem>()
        {
            new ActionsItem()
            {
                OptionNameZh = "是否开孔",
                    OptionCode = "whetherWithHole",
                    OptionValueRanges = new List < BaseOptionRangeItem > ()
                    {
                        new BaseOptionRangeItem()
                        {
                            DescEn = "No", DescZh = "否", Code = "no"
                        },
                    }
            }
        }
            };
            list.Add(_xhkB);
            var _ekuM = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                {
                    new OptionsItem()
                    {
                        OptionNameZh = "ACB下端子方案", PageType = "FunctionUnit", OptionCode = "lowerTerminalACB", OptionValue = new BaseOptionRangeItem()
                        {
                            DescEn = "Left tie", DescZh = "左母联", Code = "leftTie"
                        }
                    },
                }
                },
                Actions = new List<ActionsItem>()
        {
            new ActionsItem()
            {
                OptionNameZh = "是否开孔",
                    OptionCode = "whetherWithHole",
                    OptionValueRanges = new List < BaseOptionRangeItem > ()
                    {
                        new BaseOptionRangeItem()
                        {
                            DescEn = "No", DescZh = "否", Code = "no"
                        },
                    }
            }
        }
            };
            list.Add(_ekuM);
            var _dwHR = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                {
                    new OptionsItem()
                    {
                        OptionNameZh = "ACB下端子方案", PageType = "FunctionUnit", OptionCode = "lowerTerminalACB", OptionValue = new BaseOptionRangeItem()
                        {
                            DescEn = "Right tie", DescZh = "右母联", Code = "rightTie"
                        }
                    },
                }
                },
                Actions = new List<ActionsItem>()
        {
            new ActionsItem()
            {
                OptionNameZh = "是否开孔",
                    OptionCode = "whetherWithHole",
                    OptionValueRanges = new List < BaseOptionRangeItem > ()
                    {
                        new BaseOptionRangeItem()
                        {
                            DescEn = "No", DescZh = "否", Code = "no"
                        },
                    }
            }
        }
            };
            list.Add(_dwHR);
            var _UHnw = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                {
                    new OptionsItem()
                    {
                        OptionNameZh = "ACB下端子方案", PageType = "FunctionUnit", OptionCode = "lowerTerminalACB", OptionValue = new BaseOptionRangeItem()
                        {
                            DescEn = "Extend top of panel", DescZh = "伸出柜顶", Code = "extendTopOfPanel"
                        }
                    },
                }
                },
                Actions = new List<ActionsItem>()
        {
            new ActionsItem()
            {
                OptionNameZh = "是否开孔",
                    OptionCode = "whetherWithHole",
                    OptionValueRanges = new List < BaseOptionRangeItem > ()
                    {
                        new BaseOptionRangeItem()
                        {
                            DescEn = "No", DescZh = "否", Code = "no"
                        },
                    }
            }
        }
            };
            list.Add(_UHnw);
            var _Pcuv = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                {
                    new OptionsItem()
                    {
                        OptionNameZh = "ACB下端子方案", PageType = "FunctionUnit", OptionCode = "lowerTerminalACB", OptionValue = new BaseOptionRangeItem()
                        {
                            DescEn = "Connected with cable", DescZh = "电缆连接", Code = "connectedWithCable"
                        }
                    },
                }
                },
                Actions = new List<ActionsItem>()
        {
            new ActionsItem()
            {
                OptionNameZh = "是否开孔",
                    OptionCode = "whetherWithHole",
                    OptionValueRanges = new List < BaseOptionRangeItem > ()
                    {
                        new BaseOptionRangeItem()
                        {
                            DescEn = "No", DescZh = "否", Code = "no"
                        },
                    }
            }
        }
            };
            list.Add(_Pcuv);
            var _SnTm = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                {
                    new OptionsItem()
                    {
                        OptionNameZh = "ACB下端子方案", PageType = "FunctionUnit", OptionCode = "lowerTerminalACB", OptionValue = new BaseOptionRangeItem()
                        {
                            DescEn = "Extend from side", DescZh = "伸出侧面", Code = "extendFromSide"
                        }
                    },
                }
                },
                Actions = new List<ActionsItem>()
        {
            new ActionsItem()
            {
                OptionNameZh = "是否开孔",
                    OptionCode = "whetherWithHole",
                    OptionValueRanges = new List < BaseOptionRangeItem > ()
                    {
                        new BaseOptionRangeItem()
                        {
                            DescEn = "Yes", DescZh = "是", Code = "yes"
                        },
                    }
            }
        }
            };
            list.Add(_SnTm);
            var _oiUp = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                {
                    new OptionsItem()
                    {
                        OptionNameZh = "ACB上端子方案", PageType = "FunctionUnit", OptionCode = "upperTerminalACB", OptionValue = new BaseOptionRangeItem()
                        {
                            DescEn = "Connect horizontal bar", DescZh = "连接上水平排", Code = "connectHorizontalBar"
                        }
                    },
                }
                },
                Actions = new List<ActionsItem>()
        {
            new ActionsItem()
            {
                OptionNameZh = "上桩头/下桩头",
                    OptionCode = "busbarTerminal",
                    OptionValueRanges = new List < BaseOptionRangeItem > ()
                    {
                        new BaseOptionRangeItem()
                        {
                            DescEn = "Upper", DescZh = "上桩头", Code = "upper"
                        },
                    }
            }
        }
            };
            list.Add(_oiUp);
            var _vrlI = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                {
                    new OptionsItem()
                    {
                        OptionNameZh = "ACB上端子方案", PageType = "FunctionUnit", OptionCode = "upperTerminalACB", OptionValue = new BaseOptionRangeItem()
                        {
                            DescEn = "Connect vertical bar", DescZh = "连接垂直排", Code = "connectVerticalBar"
                        }
                    },
                }
                },
                Actions = new List<ActionsItem>()
        {
            new ActionsItem()
            {
                OptionNameZh = "上桩头/下桩头",
                    OptionCode = "busbarTerminal",
                    OptionValueRanges = new List < BaseOptionRangeItem > ()
                    {
                        new BaseOptionRangeItem()
                        {
                            DescEn = "Upper", DescZh = "上桩头", Code = "upper"
                        },
                    }
            }
        }
            };
            list.Add(_vrlI);


            #region 水平主母线
            var _elcD = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                {
                    new OptionsItem()
                        {
                            OptionNameZh = "水平主母线载流(A)", PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "1250", DescZh = "1250", Code = "1250"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "通风类型", PageType = "CabinetGroup", OptionCode = "ventilationType", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "Natural ventilation", DescZh = "自然通风", Code = "naturalVentilation"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "防护等级", PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "IP40", DescZh = "IP40", Code = "ip40"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "环境温度(℃)", PageType = "CabinetGroup", OptionCode = "ambientTemperature", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "35", DescZh = "35", Code = "35"
                            }
                        },
                }
                },
                Actions = new List<ActionsItem>()
        {
            new ActionsItem()
            {
                OptionNameZh = "水平主母线规格(mm)",
                    OptionCode = "mainBusbarSize",
                    OptionValueRanges = new List < BaseOptionRangeItem > ()
                    {
                        new BaseOptionRangeItem()
                        {
                            DescEn = "3-80×10+40×10", DescZh = "3-80×10+40×10", Code = "3-80×10+40×10"
                        },
                    }
            }
        }
            };
            list.Add(_elcD);
            var _zXcE = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                {
                    new OptionsItem()
                        {
                            OptionNameZh = "水平主母线载流(A)", PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "1250", DescZh = "1250", Code = "1250"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "通风类型", PageType = "CabinetGroup", OptionCode = "ventilationType", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "Forced ventilation", DescZh = "强制通风", Code = "forcedVentilation"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "防护等级", PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "IP40", DescZh = "IP40", Code = "ip40"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "环境温度(℃)", PageType = "CabinetGroup", OptionCode = "ambientTemperature", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "50", DescZh = "50", Code = "50"
                            }
                        },
                }
                },
                Actions = new List<ActionsItem>()
        {
            new ActionsItem()
            {
                OptionNameZh = "水平主母线规格(mm)",
                    OptionCode = "mainBusbarSize",
                    OptionValueRanges = new List < BaseOptionRangeItem > ()
                    {
                        new BaseOptionRangeItem()
                        {
                            DescEn = "3-80×10+40×10", DescZh = "3-80×10+40×10", Code = "3-80×10+40×10"
                        },
                    }
            }
        }
            };
            list.Add(_zXcE);
            var _bXzi = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                {
                    new OptionsItem()
                        {
                            OptionNameZh = "水平主母线载流(A)", PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "1250", DescZh = "1250", Code = "1250"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "通风类型", PageType = "CabinetGroup", OptionCode = "ventilationType", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "Natural ventilation", DescZh = "自然通风", Code = "naturalVentilation"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "防护等级", PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "IP54", DescZh = "IP54", Code = "ip54"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "环境温度(℃)", PageType = "CabinetGroup", OptionCode = "ambientTemperature", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "35", DescZh = "35", Code = "35"
                            }
                        },
                }
                },
                Actions = new List<ActionsItem>()
        {
            new ActionsItem()
            {
                OptionNameZh = "水平主母线规格(mm)",
                    OptionCode = "mainBusbarSize",
                    OptionValueRanges = new List < BaseOptionRangeItem > ()
                    {
                        new BaseOptionRangeItem()
                        {
                            DescEn = "3-100×10+50×10", DescZh = "3-100×10+50×10", Code = "3-100×10+50×10"
                        },
                    }
            }
        }
            };
            list.Add(_bXzi);
            var _hdMZ = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                {
                    new OptionsItem()
                        {
                            OptionNameZh = "水平主母线载流(A)", PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "1250", DescZh = "1250", Code = "1250"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "通风类型", PageType = "CabinetGroup", OptionCode = "ventilationType", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "Natural ventilation", DescZh = "自然通风", Code = "naturalVentilation"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "防护等级", PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "IP54", DescZh = "IP54", Code = "ip54"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "环境温度(℃)", PageType = "CabinetGroup", OptionCode = "ambientTemperature", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "50", DescZh = "50", Code = "50"
                            }
                        },
                }
                },
                Actions = new List<ActionsItem>()
        {
            new ActionsItem()
            {
                OptionNameZh = "水平主母线规格(mm)",
                    OptionCode = "mainBusbarSize",
                    OptionValueRanges = new List < BaseOptionRangeItem > ()
                    {
                        new BaseOptionRangeItem()
                        {
                            DescEn = "3-100×10+50×10", DescZh = "3-100×10+50×10", Code = "3-100×10+50×10"
                        },
                    }
            }
        }
            };
            list.Add(_hdMZ);
            var _ijLz = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                {
                    new OptionsItem()
                        {
                            OptionNameZh = "水平主母线载流(A)", PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "1250", DescZh = "1250", Code = "1250"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "通风类型", PageType = "CabinetGroup", OptionCode = "ventilationType", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "Forced ventilation", DescZh = "强制通风", Code = "forcedVentilation"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "防护等级", PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "IP54", DescZh = "IP54", Code = "ip54"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "环境温度(℃)", PageType = "CabinetGroup", OptionCode = "ambientTemperature", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "35", DescZh = "35", Code = "35"
                            }
                        },
                }
                },
                Actions = new List<ActionsItem>()
        {
            new ActionsItem()
            {
                OptionNameZh = "水平主母线规格(mm)",
                    OptionCode = "mainBusbarSize",
                    OptionValueRanges = new List < BaseOptionRangeItem > ()
                    {
                        new BaseOptionRangeItem()
                        {
                            DescEn = "3-80×10+40×10", DescZh = "3-80×10+40×10", Code = "3-80×10+40×10"
                        },
                    }
            }
        }
            };
            list.Add(_ijLz);
            var _nNIc = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                {
                    new OptionsItem()
                        {
                            OptionNameZh = "水平主母线载流(A)", PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "1250", DescZh = "1250", Code = "1250"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "通风类型", PageType = "CabinetGroup", OptionCode = "ventilationType", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "Forced ventilation", DescZh = "强制通风", Code = "forcedVentilation"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "防护等级", PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "IP54", DescZh = "IP54", Code = "ip54"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "环境温度(℃)", PageType = "CabinetGroup", OptionCode = "ambientTemperature", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "50", DescZh = "50", Code = "50"
                            }
                        },
                }
                },
                Actions = new List<ActionsItem>()
        {
            new ActionsItem()
            {
                OptionNameZh = "水平主母线规格(mm)",
                    OptionCode = "mainBusbarSize",
                    OptionValueRanges = new List < BaseOptionRangeItem > ()
                    {
                        new BaseOptionRangeItem()
                        {
                            DescEn = "3-80×10+40×10", DescZh = "3-80×10+40×10", Code = "3-80×10+40×10"
                        },
                    }
            }
        }
            };
            list.Add(_nNIc);
            var _jwCa = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                {
                    new OptionsItem()
                        {
                            OptionNameZh = "水平主母线载流(A)", PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "1600", DescZh = "1600", Code = "1600"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "通风类型", PageType = "CabinetGroup", OptionCode = "ventilationType", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "Natural ventilation", DescZh = "自然通风", Code = "naturalVentilation"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "防护等级", PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "IP40", DescZh = "IP40", Code = "ip40"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "环境温度(℃)", PageType = "CabinetGroup", OptionCode = "ambientTemperature", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "35", DescZh = "35", Code = "35"
                            }
                        },
                }
                },
                Actions = new List<ActionsItem>()
        {
            new ActionsItem()
            {
                OptionNameZh = "水平主母线规格(mm)",
                    OptionCode = "mainBusbarSize",
                    OptionValueRanges = new List < BaseOptionRangeItem > ()
                    {
                        new BaseOptionRangeItem()
                        {
                            DescEn = "3-100×10+50×10", DescZh = "3-100×10+50×10", Code = "3-100×10+50×10"
                        },
                    }
            }
        }
            };
            list.Add(_jwCa);
            var _NxxX = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                {
                    new OptionsItem()
                        {
                            OptionNameZh = "水平主母线载流(A)", PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "1600", DescZh = "1600", Code = "1600"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "通风类型", PageType = "CabinetGroup", OptionCode = "ventilationType", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "Forced ventilation", DescZh = "强制通风", Code = "forcedVentilation"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "防护等级", PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "IP40", DescZh = "IP40", Code = "ip40"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "环境温度(℃)", PageType = "CabinetGroup", OptionCode = "ambientTemperature", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "50", DescZh = "50", Code = "50"
                            }
                        },
                }
                },
                Actions = new List<ActionsItem>()
        {
            new ActionsItem()
            {
                OptionNameZh = "水平主母线规格(mm)",
                    OptionCode = "mainBusbarSize",
                    OptionValueRanges = new List < BaseOptionRangeItem > ()
                    {
                        new BaseOptionRangeItem()
                        {
                            DescEn = "3-100×10+50×10", DescZh = "3-100×10+50×10", Code = "3-100×10+50×10"
                        },
                    }
            }
        }
            };
            list.Add(_NxxX);
            var _FDfg = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                {
                    new OptionsItem()
                        {
                            OptionNameZh = "水平主母线载流(A)", PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "1600", DescZh = "1600", Code = "1600"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "通风类型", PageType = "CabinetGroup", OptionCode = "ventilationType", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "Forced ventilation", DescZh = "强制通风", Code = "forcedVentilation"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "防护等级", PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "IP54", DescZh = "IP54", Code = "ip54"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "环境温度(℃)", PageType = "CabinetGroup", OptionCode = "ambientTemperature", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "35", DescZh = "35", Code = "35"
                            }
                        },
                }
                },
                Actions = new List<ActionsItem>()
        {
            new ActionsItem()
            {
                OptionNameZh = "水平主母线规格(mm)",
                    OptionCode = "mainBusbarSize",
                    OptionValueRanges = new List < BaseOptionRangeItem > ()
                    {
                        new BaseOptionRangeItem()
                        {
                            DescEn = "3-100×10+50×10", DescZh = "3-100×10+50×10", Code = "3-100×10+50×10"
                        },
                    }
            }
        }
            };
            list.Add(_FDfg);
            var _lVcJ = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                {
                    new OptionsItem()
                        {
                            OptionNameZh = "水平主母线载流(A)", PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "1600", DescZh = "1600", Code = "1600"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "通风类型", PageType = "CabinetGroup", OptionCode = "ventilationType", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "Forced ventilation", DescZh = "强制通风", Code = "forcedVentilation"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "防护等级", PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "IP54", DescZh = "IP54", Code = "ip54"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "环境温度(℃)", PageType = "CabinetGroup", OptionCode = "ambientTemperature", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "50", DescZh = "50", Code = "50"
                            }
                        },
                }
                },
                Actions = new List<ActionsItem>()
        {
            new ActionsItem()
            {
                OptionNameZh = "水平主母线规格(mm)",
                    OptionCode = "mainBusbarSize",
                    OptionValueRanges = new List < BaseOptionRangeItem > ()
                    {
                        new BaseOptionRangeItem()
                        {
                            DescEn = "3-100×10+50×10", DescZh = "3-100×10+50×10", Code = "3-100×10+50×10"
                        },
                    }
            }
        }
            };
            list.Add(_lVcJ);
            var _SRwh = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                {
                    new OptionsItem()
                        {
                            OptionNameZh = "水平主母线载流(A)", PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "2000", DescZh = "2000", Code = "2000"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "通风类型", PageType = "CabinetGroup", OptionCode = "ventilationType", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "Natural ventilation", DescZh = "自然通风", Code = "naturalVentilation"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "防护等级", PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "IP40", DescZh = "IP40", Code = "ip40"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "环境温度(℃)", PageType = "CabinetGroup", OptionCode = "ambientTemperature", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "35", DescZh = "35", Code = "35"
                            }
                        },
                }
                },
                Actions = new List<ActionsItem>()
        {
            new ActionsItem()
            {
                OptionNameZh = "水平主母线规格(mm)",
                    OptionCode = "mainBusbarSize",
                    OptionValueRanges = new List < BaseOptionRangeItem > ()
                    {
                        new BaseOptionRangeItem()
                        {
                            DescEn = "3-2×60×10+60×10", DescZh = "3-2×60×10+60×10", Code = "3-2×60×10+60×10"
                        },
                    }
            }
        }
            };
            list.Add(_SRwh);
            var _NYmT = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                {
                    new OptionsItem()
                        {
                            OptionNameZh = "水平主母线载流(A)", PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "2000", DescZh = "2000", Code = "2000"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "通风类型", PageType = "CabinetGroup", OptionCode = "ventilationType", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "Forced ventilation", DescZh = "强制通风", Code = "forcedVentilation"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "防护等级", PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "IP40", DescZh = "IP40", Code = "ip40"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "环境温度(℃)", PageType = "CabinetGroup", OptionCode = "ambientTemperature", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "50", DescZh = "50", Code = "50"
                            }
                        },
                }
                },
                Actions = new List<ActionsItem>()
        {
            new ActionsItem()
            {
                OptionNameZh = "水平主母线规格(mm)",
                    OptionCode = "mainBusbarSize",
                    OptionValueRanges = new List < BaseOptionRangeItem > ()
                    {
                        new BaseOptionRangeItem()
                        {
                            DescEn = "3-2×60×10+60×10", DescZh = "3-2×60×10+60×10", Code = "3-2×60×10+60×10"
                        },
                    }
            }
        }
            };
            list.Add(_NYmT);
            var _QHoW = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                {
                    new OptionsItem()
                        {
                            OptionNameZh = "水平主母线载流(A)", PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "2000", DescZh = "2000", Code = "2000"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "通风类型", PageType = "CabinetGroup", OptionCode = "ventilationType", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "Forced ventilation", DescZh = "强制通风", Code = "forcedVentilation"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "防护等级", PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "IP54", DescZh = "IP54", Code = "ip54"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "环境温度(℃)", PageType = "CabinetGroup", OptionCode = "ambientTemperature", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "35", DescZh = "35", Code = "35"
                            }
                        },
                }
                },
                Actions = new List<ActionsItem>()
        {
            new ActionsItem()
            {
                OptionNameZh = "水平主母线规格(mm)",
                    OptionCode = "mainBusbarSize",
                    OptionValueRanges = new List < BaseOptionRangeItem > ()
                    {
                        new BaseOptionRangeItem()
                        {
                            DescEn = "3-2×60×10+60×10", DescZh = "3-2×60×10+60×10", Code = "3-2×60×10+60×10"
                        },
                    }
            }
        }
            };
            list.Add(_QHoW);
            var _xQVw = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                {
                    new OptionsItem()
                        {
                            OptionNameZh = "水平主母线载流(A)", PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "2000", DescZh = "2000", Code = "2000"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "通风类型", PageType = "CabinetGroup", OptionCode = "ventilationType", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "Forced ventilation", DescZh = "强制通风", Code = "forcedVentilation"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "防护等级", PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "IP54", DescZh = "IP54", Code = "ip54"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "环境温度(℃)", PageType = "CabinetGroup", OptionCode = "ambientTemperature", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "50", DescZh = "50", Code = "50"
                            }
                        },
                }
                },
                Actions = new List<ActionsItem>()
        {
            new ActionsItem()
            {
                OptionNameZh = "水平主母线规格(mm)",
                    OptionCode = "mainBusbarSize",
                    OptionValueRanges = new List < BaseOptionRangeItem > ()
                    {
                        new BaseOptionRangeItem()
                        {
                            DescEn = "3-2×60×10+60×10", DescZh = "3-2×60×10+60×10", Code = "3-2×60×10+60×10"
                        },
                    }
            }
        }
            };
            list.Add(_xQVw);
            var _hEct = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                {
                    new OptionsItem()
                        {
                            OptionNameZh = "水平主母线载流(A)", PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "3200", DescZh = "3200", Code = "3200"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "通风类型", PageType = "CabinetGroup", OptionCode = "ventilationType", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "Natural ventilation", DescZh = "自然通风", Code = "naturalVentilation"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "防护等级", PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "IP40", DescZh = "IP40", Code = "ip40"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "环境温度(℃)", PageType = "CabinetGroup", OptionCode = "ambientTemperature", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "35", DescZh = "35", Code = "35"
                            }
                        },
                }
                },
                Actions = new List<ActionsItem>()
        {
            new ActionsItem()
            {
                OptionNameZh = "水平主母线规格(mm)",
                    OptionCode = "mainBusbarSize",
                    OptionValueRanges = new List < BaseOptionRangeItem > ()
                    {
                        new BaseOptionRangeItem()
                        {
                            DescEn = "3-3×80×10+120×10", DescZh = "3-3×80×10+120×10", Code = "3-3×80×10+120×10"
                        },
                    }
            }
        }
            };
            list.Add(_hEct);
            var _Wybq = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                {
                    new OptionsItem()
                        {
                            OptionNameZh = "水平主母线载流(A)", PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "3200", DescZh = "3200", Code = "3200"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "通风类型", PageType = "CabinetGroup", OptionCode = "ventilationType", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "Forced ventilation", DescZh = "强制通风", Code = "forcedVentilation"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "防护等级", PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "IP40", DescZh = "IP40", Code = "ip40"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "环境温度(℃)", PageType = "CabinetGroup", OptionCode = "ambientTemperature", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "50", DescZh = "50", Code = "50"
                            }
                        },
                }
                },
                Actions = new List<ActionsItem>()
        {
            new ActionsItem()
            {
                OptionNameZh = "水平主母线规格(mm)",
                    OptionCode = "mainBusbarSize",
                    OptionValueRanges = new List < BaseOptionRangeItem > ()
                    {
                        new BaseOptionRangeItem()
                        {
                            DescEn = "3-3×80×10+120×10", DescZh = "3-3×80×10+120×10", Code = "3-3×80×10+120×10"
                        },
                    }
            }
        }
            };
            list.Add(_Wybq);
            var _PfKo = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                {
                    new OptionsItem()
                        {
                            OptionNameZh = "水平主母线载流(A)", PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "3200", DescZh = "3200", Code = "3200"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "通风类型", PageType = "CabinetGroup", OptionCode = "ventilationType", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "Forced ventilation", DescZh = "强制通风", Code = "forcedVentilation"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "防护等级", PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "IP54", DescZh = "IP54", Code = "ip54"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "环境温度(℃)", PageType = "CabinetGroup", OptionCode = "ambientTemperature", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "35", DescZh = "35", Code = "35"
                            }
                        },
                }
                },
                Actions = new List<ActionsItem>()
        {
            new ActionsItem()
            {
                OptionNameZh = "水平主母线规格(mm)",
                    OptionCode = "mainBusbarSize",
                    OptionValueRanges = new List < BaseOptionRangeItem > ()
                    {
                        new BaseOptionRangeItem()
                        {
                            DescEn = "3-3×80×10+120×10", DescZh = "3-3×80×10+120×10", Code = "3-3×80×10+120×10"
                        },
                    }
            }
        }
            };
            list.Add(_PfKo);
            var _WIwf = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                {
                    new OptionsItem()
                        {
                            OptionNameZh = "水平主母线载流(A)", PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "3200", DescZh = "3200", Code = "3200"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "通风类型", PageType = "CabinetGroup", OptionCode = "ventilationType", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "Forced ventilation", DescZh = "强制通风", Code = "forcedVentilation"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "防护等级", PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "IP54", DescZh = "IP54", Code = "ip54"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "环境温度(℃)", PageType = "CabinetGroup", OptionCode = "ambientTemperature", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "50", DescZh = "50", Code = "50"
                            }
                        },
                }
                },
                Actions = new List<ActionsItem>()
        {
            new ActionsItem()
            {
                OptionNameZh = "水平主母线规格(mm)",
                    OptionCode = "mainBusbarSize",
                    OptionValueRanges = new List < BaseOptionRangeItem > ()
                    {
                        new BaseOptionRangeItem()
                        {
                            DescEn = "3-3×80×10+120×10", DescZh = "3-3×80×10+120×10", Code = "3-3×80×10+120×10"
                        },
                    }
            }
        }
            };
            list.Add(_WIwf);
            var _gtKX = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                {
                    new OptionsItem()
                        {
                            OptionNameZh = "水平主母线载流(A)", PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "4000", DescZh = "4000", Code = "4000"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "通风类型", PageType = "CabinetGroup", OptionCode = "ventilationType", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "Natural ventilation", DescZh = "自然通风", Code = "naturalVentilation"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "防护等级", PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "IP40", DescZh = "IP40", Code = "ip40"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "环境温度(℃)", PageType = "CabinetGroup", OptionCode = "ambientTemperature", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "35", DescZh = "35", Code = "35"
                            }
                        },
                }
                },
                Actions = new List<ActionsItem>()
        {
            new ActionsItem()
            {
                OptionNameZh = "水平主母线规格(mm)",
                    OptionCode = "mainBusbarSize",
                    OptionValueRanges = new List < BaseOptionRangeItem > ()
                    {
                        new BaseOptionRangeItem()
                        {
                            DescEn = "3-3×100×10+2×80×10", DescZh = "3-3×100×10+2×80×10", Code = "3-3×100×10+2×80×10"
                        },
                    }
            }
        }
            };
            list.Add(_gtKX);
            var _FjOP = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                {
                    new OptionsItem()
                        {
                            OptionNameZh = "水平主母线载流(A)", PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "4000", DescZh = "4000", Code = "4000"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "通风类型", PageType = "CabinetGroup", OptionCode = "ventilationType", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "Forced ventilation", DescZh = "强制通风", Code = "forcedVentilation"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "防护等级", PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "IP40", DescZh = "IP40", Code = "ip40"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "环境温度(℃)", PageType = "CabinetGroup", OptionCode = "ambientTemperature", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "50", DescZh = "50", Code = "50"
                            }
                        },
                }
                },
                Actions = new List<ActionsItem>()
        {
            new ActionsItem()
            {
                OptionNameZh = "水平主母线规格(mm)",
                    OptionCode = "mainBusbarSize",
                    OptionValueRanges = new List < BaseOptionRangeItem > ()
                    {
                        new BaseOptionRangeItem()
                        {
                            DescEn = "3-3×100×10+2×80×10", DescZh = "3-3×100×10+2×80×10", Code = "3-3×100×10+2×80×10"
                        },
                    }
            }
        }
            };
            list.Add(_FjOP);
            var _QLyh = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                {
                    new OptionsItem()
                        {
                            OptionNameZh = "水平主母线载流(A)", PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "4000", DescZh = "4000", Code = "4000"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "通风类型", PageType = "CabinetGroup", OptionCode = "ventilationType", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "Forced ventilation", DescZh = "强制通风", Code = "forcedVentilation"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "防护等级", PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "IP54", DescZh = "IP54", Code = "ip54"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "环境温度(℃)", PageType = "CabinetGroup", OptionCode = "ambientTemperature", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "35", DescZh = "35", Code = "35"
                            }
                        },
                }
                },
                Actions = new List<ActionsItem>()
        {
            new ActionsItem()
            {
                OptionNameZh = "水平主母线规格(mm)",
                    OptionCode = "mainBusbarSize",
                    OptionValueRanges = new List < BaseOptionRangeItem > ()
                    {
                        new BaseOptionRangeItem()
                        {
                            DescEn = "3-3×100×10+2×80×10", DescZh = "3-3×100×10+2×80×10", Code = "3-3×100×10+2×80×10"
                        },
                    }
            }
        }
            };
            list.Add(_QLyh);
            var _tAtg = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                {
                    new OptionsItem()
                        {
                            OptionNameZh = "水平主母线载流(A)", PageType = "CabinetGroup", OptionCode = "mainBusbarCurrent", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "4000", DescZh = "4000", Code = "4000"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "通风类型", PageType = "CabinetGroup", OptionCode = "ventilationType", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "Forced ventilation", DescZh = "强制通风", Code = "forcedVentilation"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "防护等级", PageType = "CabinetGroup", OptionCode = "ingressProtectionDegree", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "IP54", DescZh = "IP54", Code = "ip54"
                            }
                        },
                        new OptionsItem()
                        {
                            OptionNameZh = "环境温度(℃)", PageType = "CabinetGroup", OptionCode = "ambientTemperature", OptionValue = new BaseOptionRangeItem()
                            {
                                DescEn = "50", DescZh = "50", Code = "50"
                            }
                        },
                }
                },
                Actions = new List<ActionsItem>()
        {
            new ActionsItem()
            {
                OptionNameZh = "水平主母线规格(mm)",
                    OptionCode = "mainBusbarSize",
                    OptionValueRanges = new List < BaseOptionRangeItem > ()
                    {
                        new BaseOptionRangeItem()
                        {
                            DescEn = "3-3×100×10+2×80×10", DescZh = "3-3×100×10+2×80×10", Code = "3-3×100×10+2×80×10"
                        },
                    }
            }
        }
            };
            list.Add(_tAtg);
            var _SmaK = new OptionsConstraintCabinetEntitySpec()
            {
                Condition = new Condition()
                {
                    Logic = "and",
                    Options = new List<OptionsItem>()
                {
                    new OptionsItem()
                    {
                        OptionNameZh = "水平主母线短时耐受", PageType = "CabinetGroup", OptionCode = "mainbusbarIcw", OptionValue = new BaseOptionRangeItem()
                        {
                            DescEn = "25kA/1S", DescZh = "25kA/1S", Code = "25kA/1S"
                        }
                    },
                }
                },
                Actions = new List<ActionsItem>()
        {
            new ActionsItem()
            {
                OptionNameZh = "水平主母线短时耐受",
                    OptionCode = "mainBusbarIcw",
                    OptionValueRanges = new List < BaseOptionRangeItem > ()
                    {
                        new BaseOptionRangeItem()
                        {
                            DescEn = "25kA/1S", DescZh = "25kA/1S", Code = "25kA/1S"
                        },
                    }
            }
        }
            };

            #endregion





            return list;
        }
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
