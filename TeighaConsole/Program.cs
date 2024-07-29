using System;
using System.IO;
using Teigha.Core;
using Teigha.TD;

namespace TeighaConsole
{
    internal class Program
    {
        static CustomServices _hostApp;
        static OdRxSystemServices _sysSrv;
        static OdDbDatabase CurDb = null;

        static void Main(string[] args)
        {
            #region 初始化组件
            _sysSrv = new ExSystemServices();
            _hostApp = new CustomServices();
            TD_Db.odInitialize(_sysSrv);
            #endregion

            //路径
            string filename = @"D:\cad\5320db1794en.dxf";
            OdDbDatabase db = _hostApp.readFile(filename);
            CurDb = db;

            #region  执行转换逻辑
            OdGsModule pModule = (OdGsModule)
                Teigha.Core.Globals.odrxDynamicLinker().loadModule("TD_SvgExport");
            if (null == pModule)
            {
                throw new Exception("TD_SvgExport.tx is missing");
            }

            ExecuteCommand("svgout 800 600 \n\n.png Helvetica 768 1024 Yes Yes\n", CurDb);
            #endregion
        }

        private static void ExecuteCommand(String sCmd, OdDbDatabase CurDb)
        {
            OdDbCommandContext pExCmdCtx = ExDbCommandContext.createObject(
                ExStringIO.create(sCmd),
                CurDb
            );
            try
            {
                OdEdCommandStack pCommands = Globals.odedRegCmds();
                String s = sCmd.Substring(0, sCmd.IndexOf(" "));
                if (s.Length == sCmd.Length)
                {
                    s = s.ToUpper();
                    pCommands.executeCommand(s, pExCmdCtx);
                }
                else
                {
                    ExStringIO m_pMacro = ExStringIO.create(sCmd);
                    while (!m_pMacro.isEof())
                    {
                        try
                        {
                            s = pExCmdCtx.userIO().getString("Command:");
                            s = s.ToUpper();
                        }
                        catch (OdEdEmptyInput eEmptyInput) { }
                        pCommands.executeCommand(s, pExCmdCtx);
                    }
                }
            }
            catch (OdEdEmptyInput eEmptyInput) { }
            catch (OdEdCancel eCanc) { }
            catch (OdError err) { }
        }

        public class CustomServices : ExHostAppServices
        {
            public override string fileDialog(
                int flags,
                string dialogCaption,
                string defExt,
                string defFilename,
                string filter
            )
            {
                Console.Write("请输入文件名: ");
                string fileName = Console.ReadLine();
                // 检查文件名是否包含扩展名
                if (!fileName.EndsWith(defExt))
                {
                    fileName = Path.ChangeExtension(fileName, defExt);
                }

                Console.Write("请输入路径地址: ");
                string pathName = Console.ReadLine();
                var isExist = Directory.Exists(pathName);
                if (!isExist)
                {
                    throw new Exception("路径不存在，请重新输入有效的路径地址");
                }

                fileName = Path.Combine(pathName, fileName);
                return fileName;
            }
        }
    }
}
