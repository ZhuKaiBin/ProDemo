using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;

namespace ClassLibraryCADDemo1
{
    public class Class1
    {

        [CommandMethod("Hello")]
        public void Hello()
        {
            //1.获取当前打开的文档
            Document adoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            //2.获取当前文档编辑器
            Editor edt=adoc.Editor;
            edt.WriteMessage("我的第一个CAD测试类库");
            

        }

    }
}
