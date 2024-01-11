using Teigha.Core;
using Teigha.TD;

namespace WinFormDemo
{
    public partial class Form1 : Form
    {
        CustomServices _hostApp;
        OdRxSystemServices _sysSrv;
        OdDbDatabase CurDb = null;

        public Form1()
        {
            #region initialization
            InitializeComponent();
            _sysSrv = new ExSystemServices();
            _hostApp = new CustomServices();
            TD_Db.odInitialize(_sysSrv);
            #endregion
        }


        /// <summary>
        /// 导出框，不可删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (openDwgDialog.ShowDialog() == DialogResult.OK)
            {
                bool bLoaded = true;
                try
                {
                    OdDbDatabase db = _hostApp.readFile(openDwgDialog.FileName);

                    CurDb = db;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    bLoaded = false;
                }
                if (bLoaded)
                {
                    UpdateMainMenu(true);

                }
            }
        }

        /// <summary>
        /// 选择文件后，再导出
        /// </summary>
        /// <param name="flag"></param>
        private void UpdateMainMenu(bool flag)
        {
            toolStripSeparator1.Visible = flag;
            ExportSVG.Visible = flag;
            printToolStripMenuItem.Enabled = true;
            printToolStripMenuItem.Visible = flag;
        }



        private void ExportSVG_Click(object sender, EventArgs e)
        {
            OdGsModule pModule = (OdGsModule)Teigha.Core.Globals.odrxDynamicLinker().loadModule("TD_SvgExport");
            if (null == pModule)
            {
                MessageBox.Show("TD_SvgExport.tx is missing");
                return;
            }
            ExecuteCommand("svgout 1 6 \n\n.png sans-serif 768 1024 Yes Yes\n", false);
        }


        String rec_command = String.Empty;
        OdEdBaseIO cmdIO(String strIO)
        {
            return ExStringIO.create(strIO);
        }
        OdDbCommandContext cmdCtx(String strIO)
        {
            return ExDbCommandContext.createObject(cmdIO(strIO), CurDb);
        }

        public String recentCmdName()
        {
            return rec_command;
        }

        private void ExecuteCommand(String sCmd, bool bEcho)
        {
            OdDbCommandContext pExCmdCtx = cmdCtx(sCmd);
            try
            {
                OdEdCommandStack pCommands = Globals.odedRegCmds();

                {
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
                                s = recentCmdName();
                            }
                            pCommands.executeCommand(s, pExCmdCtx);
                        }
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

        private void openDwgDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
    public class CustomServices : ExHostAppServices
    {
        /// <summary>
        /// 重写方法，不可删除
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
