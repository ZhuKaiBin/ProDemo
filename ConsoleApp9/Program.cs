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


            List<TestClassLeave> testClassLeaves = new List<TestClassLeave>();
            int index = 1;
            foreach (var item in testClasses)
            {

                var materialCode = item.MaterialCode;
                var subMaterialCode = item.SubMaterialCode;

                string currentHierarchy = $"{parentHierarchy}.{index}";

                testClassLeaves.Add(new TestClassLeave() { MaterialCode = materialCode, leave = currentHierarchy });
                testClassLeaves.Add(new TestClassLeave() { MaterialCode = subMaterialCode, leave = $"{currentHierarchy}.1" });
                index++;

                //查看下子零部件号在零部件号中是否存在

                var sd = testClasses.Where(x => x.MaterialCode == subMaterialCode).ToList();

            }



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
