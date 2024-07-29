using System.Data;
using System.Reflection;
using MiniExcelLibs;

namespace MiniExcelApp
{
    public class ModelA
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class ModelB
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            {
                var path = "demo.xlsx";
                var values = new[]
                {
                    new { A = "Github", B = DateTime.Parse("2021-01-01") },
                    new { A = "Microsoft", B = DateTime.Parse("2021-02-01") }
                };

                // create
                using (var stream = File.Create(path))
                    stream.SaveAs(values);

                // query dynamic
                {
                    var rows = MiniExcel.Query(path, useHeaderRow: true).ToList();
                    Console.WriteLine(rows[0].A); //Github
                    Console.WriteLine(rows[0].B); //2021-01-01 12:00:00 AM
                    Console.WriteLine(rows[1].A); //Microsoft
                    Console.WriteLine(rows[1].B); //2021-02-01 12:00:00 AM
                }
            }
            {
                var path = "demo444.xlsx";
                var users = new[]
                {
                    new { 宽 = 600, 深 = 800 },
                    new { 宽 = 800, 深 = 800 },
                    new { 宽 = 600, 深 = 1000 }
                };
                var department = new[]
                {
                    new { ID = "01", Name = "HR" },
                    new { ID = "02", Name = "IT" }
                };
                var sheets = new Dictionary<string, object>
                {
                    ["users"] = users,
                    ["department"] = department
                };
                //MiniExcel.SaveAs(path, sheets);
                var sheetNames = MiniExcel.GetSheetNames(path);
                #region
                //foreach (var sheetName in sheetNames)
                //{
                //    Console.WriteLine($"Sheet: {sheetName}");

                //    switch (sheetName)
                //    {
                //        case "users":
                //            ProcessSheet<ModelA>(path, sheetName);
                //            break;
                //        case "department":
                //            ProcessSheet<ModelB>(path, sheetName);
                //            break;
                //        // Add more cases for additional sheets
                //        default:
                //            Console.WriteLine("Unhandled sheet.");
                //            break;
                //    }
                //}
                #endregion
                #region
                //foreach (var sheetName in sheetNames)//骨架，眉头，
                //{
                //    var rows = MiniExcel.Query(path, useHeaderRow: true, sheetName: sheetName).ToList();

                //    for (int i = 0; i < rows.Count; i++)
                //    {
                //        Console.WriteLine(rows[i].); //Github
                //        Console.WriteLine(rows[i].Age);
                //    }
                //}
                //var path = "demo.xlsx";
                //var values = new[] { new { A = "Github", B = DateTime.Parse("2021-01-01") }, new { A = "Microsoft", B = DateTime.Parse("2021-02-01") } };
                //Console.WriteLine("Hello, World!");

                foreach (var sheetName in sheetNames)
                {
                    //第一个sheet的所有数据
                    var rows = MiniExcel
                        .Query(path, sheetName: sheetName, startCell: "A1")
                        .ToList();

                    var aData = rows.Select(x => x.A).ToList(); //A这一列的所有数据

                    var bData = rows.Select(x => x.B).ToList(); //B这一列所有数据

                    //获取到列
                    var AClo = rows[0].A;
                    var BClo = rows[0].B;
                    var CClo = rows[0].C;

                    foreach (var item in rows)
                    {
                        var a = item.A;
                        var b = item.B;
                    }

                    for (var i = 0; i < rows.Count; i++)
                    {
                        Console.WriteLine(rows[i].A + "/" + rows[i].B);
                    }
                }
            }

            ////create
            //using (var stream = File.Create(path))
            //    stream.SaveAs(values);

            //// query dynamic
            //{
            //    var rows = MiniExcel.Query(path, useHeaderRow: true).ToList();
            //    Console.WriteLine(rows[0].A); //Github
            //    Console.WriteLine(rows[0].B); //2021-01-01 12:00:00 AM
            //    Console.WriteLine(rows[1].A); //Microsoft
            //    Console.WriteLine(rows[1].B); //2021-02-01 12:00:00 AM
            //}
                #endregion
            //Dictionary<string, List<object>> sheetData = new Dictionary<string, List<object>>();

            //foreach (var sheetName in sheetNames)
            //{
            //    Console.WriteLine($"Sheet: {sheetName}");
            //    var rr = ProcessSheet2<ModelA>(path, sheetName);
            //    switch (sheetName)
            //    {
            //        case "users":
            //            //走插入数据的逻辑
            //            var sd = ProcessSheet2<ModelA>(path, sheetName);
            //            break;
            //        case "department":
            //            //走插入数据的逻辑
            //            ProcessSheet2<ModelB>(path, sheetName);
            //            break;
            //        default:
            //            Console.WriteLine("Unhandled sheet.");
            //            break;
            //    }
            //}

            //// Now you can access the data for each sheet from the sheetData dictionary
            //foreach (var kvp in sheetData)
            //{
            //    Console.WriteLine($"Data for Sheet '{kvp.Key}':");
            //    foreach (var item in kvp.Value)
            //    {
            //        Console.WriteLine(item);
            //    }
            //    Console.WriteLine();
            //}
        }

        static void ProcessSheet<T>(string path, string sheetName)
            where T : class, new()
        {
            var rows = MiniExcel.Query<T>(path, sheetName: sheetName).ToList();

            if (typeof(T) == typeof(ModelA))
            {
                foreach (var row in rows.Cast<ModelA>())
                {
                    Console.WriteLine($"Name: {row.Name}, Age: {row.Age}");
                }
            }
            else if (typeof(T) == typeof(ModelB))
            {
                foreach (var row in rows.Cast<ModelB>())
                {
                    Console.WriteLine($"Title: {row.ID} , Date:  {row.Name}");
                }
            }
            // Add more conditions for additional models
        }

        static object GetDynamicValue(dynamic obj, string propertyName)
        {
            Type type = obj.GetType();
            PropertyInfo property = type.GetProperty(propertyName);
            if (property != null)
            {
                return property.GetValue(obj);
            }
            else
            {
                throw new ArgumentException("Property not found");
            }
        }

        static List<T> ProcessSheet2<T>(string path, string sheetName)
            where T : class, new()
        {
            var rows = MiniExcel.Query<T>(path, sheetName: sheetName).ToList();
            return rows.Cast<T>().ToList();
        }
    }
}
