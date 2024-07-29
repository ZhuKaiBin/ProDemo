using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;

namespace ClassLibrary1
{
    public class Class1
    {
        /*
         CommandMethod的特性，CAD在运行的时候会搜索所有有CommandMethod特性的方法来作为命令入口。
          所以如果在CAD中输入SayHello命令就会执行该方法内的动作。
         */
        [CommandMethod("SayHello")]
        public void SayHello()
        {
            Editor editor = Application.DocumentManager.MdiActiveDocument.Editor;
            editor.WriteMessage("Hello World");
        }
    }
}
