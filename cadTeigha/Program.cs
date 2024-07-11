////3.02及以下版本命名空间应将Teigha换为DWGdirect
//#if Teigha302Dnow
//using DWGdirect.DatabaseServices;
//using DWGdirect.Geometry;
//using DWGdirect.Colors;
//using DWGdirect.Export_Import;
//using DWGdirect.GraphicsInterface;
//using DWGdirect.GraphicsSystem;
//using DWGdirect.Runtime;
//#else
//using System.IO;
//using Teigha.DatabaseServices;
//using Teigha.Runtime;
//#endif

//namespace cadTeigha
//{
//    internal class Program
//    {
//        static void Main(string[] args)
//        {
//            //打开、新建、保存数据库
//            //处理数据库之前,需要启用Teigha的服务程序.一个应用程序加上一个就行了,否则出错
//            using (Services ser = new Services())
//            {
//                //打开数据库(dwg文件)
//                using (Database pDb = new Database(false, false))//不加参数会出错
//                {
//                    //pDb.ReadDwgFile(@"D:\ProjectDemo\ProDemo\cadTeigha\files\5320db1794en.dxf", FileOpenMode.OpenForReadAndWriteNoShare, false, "");
//                    ////SaveType pSavetype = SaveType.Save12;
//                    //DwgVersion dwgver = DwgVersion.Current;
//                    //pDb.SaveAs("4.svg", dwgver);

//                    pDb.ReadDwgFile(@"D:\ProjectDemo\ProDemo\cadTeigha\files\5320db1794en.dxf", FileOpenMode.OpenForReadAndWriteNoShare, false, "");

//                    // 将 DWG 文件转换成 SVG 格式

//                    //SvgExport svgExport = new SvgExport();
//                    //MemoryStream svgStream = new MemoryStream();
//                    //svgExport.Export(pDb, svgStream);
//                    //svgStream.Position = 0;
//                    //string svgContent;
//                    //using (StreamReader reader = new StreamReader(svgStream))
//                    //{
//                    //    svgContent = reader.ReadToEnd();
//                    //}

//                    //// 保存 SVG 文件
//                    //File.WriteAllText("3.svg", svgContent);
//                }

//            }
//        }
//    }
//}