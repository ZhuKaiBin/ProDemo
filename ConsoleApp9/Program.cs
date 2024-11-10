using System;

namespace ConsoleApp9
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 示例数据
            var testClasses = new List<TestClass>
                             {
                                 new TestClass{MaterialCode="DX001", SubMaterialCode="DX0011"},
                                 new TestClass{MaterialCode="DX0011", SubMaterialCode="DX00111"},
                                 new TestClass{MaterialCode="DX00111", SubMaterialCode="DX001111"},
                                 new TestClass{MaterialCode="DX00111", SubMaterialCode="DX002"},

                                 new TestClass{MaterialCode="DX002", SubMaterialCode="DX0022"},
                                 new TestClass{MaterialCode="DX002", SubMaterialCode="DX00222"},
                                 new TestClass{MaterialCode="DX0022", SubMaterialCode="DX00223"}
                             }.ToList();


            var parentHierarchy = "1";


        }
    }

    public class TestClass
    {
        public string MaterialCode { get; set; }
        public string SubMaterialCode { get; set; }
    }

    public class TestClassLeave
    {
        public string MaterialCode { get; set; }

        public string leave { set; get; }

    }
}
