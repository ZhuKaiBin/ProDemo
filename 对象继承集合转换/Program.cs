using System;
using System.Collections.Generic;
using System.Linq;

namespace 对象继承集合转换
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var result = retBase();

            Console.WriteLine("Contains UnMatchedOptionError: " + result.OfType<BOM>().Any(b => b.UnMatchedOptionError != null));
        }

        public static IEnumerable<BaseClass> retBase()
        {
            List<BaseClass> baseClasses = new List<BaseClass>();
            List<BOM> bOMs = new List<BOM>();

            for (int i = 0; i < 10000; i++)
            {
                BOM bOM = new BOM
                {
                    name = i.ToString(),
                    materialCode = "0" + i.ToString()
                };
                // 为第30个元素设置 UnMatchedOptionError
                //if (i == 30)
                //{
                //    bOM.UnMatchedOptionError = new UnMatchedOptionErrorDto
                //    {
                //        matchedType = "error",
                //        matchedCode = "service"
                //    };

                //}

                bOMs.Add(bOM);


            }

            // 检查 bOMs 中是否有任何 UnMatchedOptionError 不为空的项
            bool hasUnmatchedOptionErrors = bOMs.Any(bom => bom.UnMatchedOptionError != null);

            if (hasUnmatchedOptionErrors)
            {
                // 存在 UnMatchedOptionError 不为空的项，直接返回包含完整 BOM 信息的列表
                return bOMs;
            }
            else
            {
                // 没有 UnMatchedOptionError，不包含 BOM 额外信息的 BaseClass 列表
                baseClasses.AddRange(bOMs.Select(bom => new BaseClass
                {
                    materialCode = bom.materialCode,
                    name = bom.name
                }));
                return baseClasses;
            }
        }
    }

    public class BaseClass
    {
        public string materialCode { set; get; }
        public string name { set; get; }
    }

    public class BOM : BaseClass
    {
        public UnMatchedOptionErrorDto? UnMatchedOptionError { get; set; }
    }

    public class UnMatchedOptionErrorDto
    {
        public string? matchedType { get; set; }
        public string? matchedCode { get; set; }
    }
}