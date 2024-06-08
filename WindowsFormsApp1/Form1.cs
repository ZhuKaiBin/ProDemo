using System;
using System.Windows.Forms;
using Teigha.Core;
using Teigha.TD;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        CustomServices _hostApp;
        OdRxSystemServices _sysSrv;
        OdDbDatabase CurDb = null;

        public Form1()
        {
            //初始化
            #region initialization
            InitializeComponent();
            _sysSrv = new ExSystemServices();
            _hostApp = new CustomServices();
            TD_Db.odInitialize(_sysSrv);
            #endregion
        }
      
        private void openToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //选择文件
            //if (openDwgDialog.ShowDialog() == DialogResult.OK)
            //{
                //var path = openDwgDialog.FileName;
                //得到文件路径后,填充本地的OdDb
                //OdDbDatabase db = _hostApp.readFile(path);
                OdDbDatabase db = _hostApp.readFile(@"D:\cad\simple.dwg");
                CurDb = db;
                toolStripSeparator1.Visible = true;
                ExportSVG.Visible = true;
                printToolStripMenuItem.Enabled = true;
                printToolStripMenuItem.Visible = true;
            //}
        }

        private void ExportSVG_Click(object sender, EventArgs e)
        {
            OdGsModule pModule = (OdGsModule)Teigha.Core.Globals.odrxDynamicLinker().loadModule("TD_SvgExport");
            if (null == pModule)
            {
                MessageBox.Show("TD_SvgExport.tx is missing");
                return;
            }
            ExecuteCommand("svgout 8 6 \n\n.png Helvetica 768 1024 Yes Yes\n");
        }

        private void ExecuteCommand(String sCmd)
        {
            OdDbCommandContext pExCmdCtx = ExDbCommandContext.createObject(ExStringIO.create(sCmd), CurDb);
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
                        catch (OdEdEmptyInput eEmptyInput)
                        {
                        }
                        pCommands.executeCommand(s, pExCmdCtx);
                    }
                }
            }
            catch (OdEdEmptyInput eEmptyInput)
            {
            }
            catch (OdEdCancel eCanc)
            {
            }
            catch (OdError err)
            {

            }
        }

    }
    public class CustomServices : ExHostAppServices
    {
        /// <summary>
        /// 将转化好的，选择保存的位置
        /// </summary>
        /// <param name="flags"></param>
        /// <param name="dialogCaption"></param>
        /// <param name="defExt"></param>
        /// <param name="defFilename"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public override string fileDialog(int flags, string dialogCaption, string defExt, string defFilename, string filter)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.DefaultExt = defExt;
            dlg.Title = dialogCaption;
            dlg.FileName = defFilename;
            dlg.Filter = filter;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                return dlg.FileName;
            }
            return String.Empty;
        }
    }
}
