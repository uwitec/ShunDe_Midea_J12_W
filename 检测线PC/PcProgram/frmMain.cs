using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Text;
using System.Diagnostics;
using System.Threading;
namespace PcProgram
{
    public partial class frmMain : Form
    {
        FrmAutoTest fAutoTest = new FrmAutoTest();
        frmSys fSys = new frmSys();
        frmSet fSet = new frmSet();
        frmBar fBar = new frmBar();
        frmDataShow fReport = new frmDataShow();
        bool isSwitch = false;
        Process pp;//FTP进程
        bool isExit = false;
        public frmMain()
        {
            InitializeComponent();
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            initIcon();
            initSystem();
            initFrm();
        }
        private void initIcon()
        {
            toolStrip1.Height = 36;
            Icon iconBar = new Icon(cMain.path + "\\pic\\barcode.ico", new Size(48, 48));
            Icon iconSet = new Icon(cMain.path + "\\pic\\set.ico", new Size(48, 48));
            Icon iconRun = new Icon(cMain.path + "\\pic\\run.ico", new Size(48, 48));
            Icon iconRep = new Icon(cMain.path + "\\pic\\report.ico", new Size(48, 48));
            Icon iconAbo = new Icon(cMain.path + "\\pic\\about.ico", new Size(48, 48));
            toolBtnBar.Image = iconBar.ToBitmap();
            toolBtnMode.Image = iconSet.ToBitmap();
            toolBtnRun.Image = iconRun.ToBitmap();
            toolBtnReport.Image = iconRep.ToBitmap();
            toolBtnAbout.Image = iconAbo.ToBitmap();
        }
        private void initFrm()
        {
            cMain.privateFontsShaoNv.AddFontFile(cMain.path + "\\font\\幼圆.ttc");//加载字体
            cMain.privateFontsYouYuan.AddFontFile(cMain.path + "\\font\\幼圆.ttc");//加载字体
            Font font = new Font(cMain.privateFontsShaoNv.Families[0], 50);
            lblTitle.Font = new Font(font, FontStyle.Bold);
            lblTitle.Text = cMain.Title;
            lblTitle.Left = (this.Width - lblTitle.Width) / 2;
            this.Text = cMain.Title;
            cMain.mUdp.SendBarFrm = fAutoTest;
            if (cMain.strLanguage == "en-US")
            {
                中文CToolStripMenuItem.Checked = false;
                engLishEToolStripMenuItem.Checked = true;
            }
            else
            {
                engLishEToolStripMenuItem.Checked = false; 
                中文CToolStripMenuItem.Checked = true;
            }
            //isLogin(this, true, true);
        }
        private void initSystem()
        {
            if (!cTodayData.ReadToday(ref cMain.mTodayData.TodayTime, ref cMain.mTodayData.TestCount))
            {
                cMain.WriteErrorToLog("读取当日测量数据失败");
            }
            else
            {
                DateTime todayStart = new DateTime(cMain.mTodayData.TodayTime.Year, cMain.mTodayData.TodayTime.Month, cMain.mTodayData.TodayTime.Day);
                cTodayData.WriteToday("TodayTime", todayStart.ToString());
                TimeSpan ts = DateTime.Now - todayStart;
                if (ts.Days >= 1)
                {
                    cTodayData.WriteToday("TodayTime", DateTime.Now.ToString());
                    for (int i = 0; i < cMain.mTodayData.TestCount.Length; i++)
                    {
                        cMain.mTodayData.TestCount[i] = 0;
                        cTodayData.WriteToday("TestCount" + i.ToString(), "0");
                    }
                }
            }
            cMain.isDebug = bool.Parse(cMain.ReadIni("System", "IsDebug", "false"));
            cMain.comICCard = cMain.ReadIni("System", "ComICCard", "COM1");
            cMain.comBar = cMain.ReadIni("System", "comBar", "COM2");
            cMain.ReadAnGuiDelay = Num.IntParse(cMain.ReadIni("System", "ReadAnGuiDelay", "6000"));
            cMain._Modeuser = cMain.ReadIni("System", "UserName", "Admin");
            cMain.isAutoSn = Num.BoolParse(cMain.ReadIni("System", "isAutoSn", "true"));
            cData.GetICCard(out cMain.TestIcCardNum);
            if (!cMain.isDebug)
            {
                if (cPingMcgs.localHostIp() != "192.168.1.100")
                {
                    MessageBox.Show("本机IP地址错误,请将本机IP地址修改为192.168.1.100", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            for (int i = 0; i < cMain.AllCount; i++)
            {
                cMain.mTempNetResult[i] = new cTempResult();
            }
            for (int i = 0; i < cMain.AllCount; i++)
            {
                cMain.mCurrentData[i] = new cNetResult();
            }

            cMain.DataAllTitle = cMain.DataAllTitleStr.Split(',');
            cMain.DataShowTitle = cMain.DataShowTitleStr.Split(',');
            foreach (Process p in Process.GetProcesses())
            {
                if (p.ProcessName == "ftpserver")
                {
                    p.Kill();
                }
            }
            if (System.IO.File.Exists(cMain.path + "\\exe\\ftpserver.exe"))
            {
                pp = new Process();
                pp.StartInfo.FileName = cMain.path + "\\Exe\\ftpserver.exe";
                pp.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                pp.StartInfo.CreateNoWindow = true;
                pp.Start();
            }
        }
        private void 退出系统XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isSwitch)
            {
                if (!isExit)
                {
                    DialogResult d = MessageBox.Show("是否确定退出系统?", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (d == DialogResult.No)
                    {
                        e.Cancel = true;
                        return;
                    }
                    isExit = true;
                }
            }
            Application.Exit();
        }
        private void 帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (System.IO.File.Exists(cMain.path + "\\help\\help.doc"))
            {
                Process.Start(cMain.path + "\\help\\help.doc");
            }
        }
        private void 系统登陆LToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUser f = new frmUser(this,cMain._Modeuser);
            if (f.ShowDialog() == DialogResult.Yes)
            {
                isLogin(this, true,f.IsAdmin);
            }
            else 
            {
                isLogin(this, false,f.IsAdmin);
            }
            cMain._isSystemUser = f.IsAdmin;
        }
        public static void isLogin(frmMain mFrmMain,bool login,bool isAdmin)
        {
            if (login||isAdmin)
            {
                mFrmMain.条码设置BToolStripMenuItem.Enabled = true;
                mFrmMain.参数设置SToolStripMenuItem.Enabled = true;
                mFrmMain.系统设置ToolStripMenuItem1.Enabled = true;
                mFrmMain.系统登出EToolStripMenuItem.Enabled = true;
                mFrmMain.系统登陆LToolStripMenuItem.Enabled = false;
                mFrmMain.密码修改ToolStripMenuItem.Enabled = true;
                mFrmMain.toolBtnBar.Enabled = true;
                mFrmMain.toolBtnMode.Enabled = true;
            }
            else
            {
                mFrmMain.条码设置BToolStripMenuItem.Enabled = false;
                mFrmMain.参数设置SToolStripMenuItem.Enabled = false;
                mFrmMain.系统设置ToolStripMenuItem1.Enabled = false;
                mFrmMain.系统登出EToolStripMenuItem.Enabled = false;
                mFrmMain.系统登陆LToolStripMenuItem.Enabled = true;
                mFrmMain.密码修改ToolStripMenuItem.Enabled = false;
                mFrmMain.toolBtnBar.Enabled = false;
                mFrmMain.toolBtnMode.Enabled = false;
            }
            if (isAdmin)
            {
                mFrmMain.手动检测HToolStripMenuItem.Visible = true;
            }
            else
            {
                mFrmMain.手动检测HToolStripMenuItem.Visible = false;
            }
        }

        private void 公司ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAbout fa = new frmAbout();
            fa.ShowDialog();
        }

        private void 系统登出EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isLogin(this, false, false);
        }

        private void 条码设置BToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fBar.ShowDialog();
        }

        private void 系统设置ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            fSys.ShowDialog();
        }

        private void 参数设置SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fSet.IsDisposed)
            {
                fSet = new frmSet();
            }
            fSet.Show();
        }

        private void 手动检测HToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void 自动检测AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fAutoTest.IsDisposed)
            {
                fAutoTest = new FrmAutoTest();
            }
            fAutoTest.Show();
            fAutoTest.WindowState = FormWindowState.Maximized;
            fAutoTest.BringToFront();
        }

        private void toolBtnBar_Click(object sender, EventArgs e)
        {
            fBar.ShowDialog();
        }

        private void toolBtnMode_Click(object sender, EventArgs e)
        {
            if (fSet.IsDisposed)
            {
                fSet = new frmSet();
            }
            fSet.Show();
        }

        private void toolBtnRun_Click(object sender, EventArgs e)
        {
            if (fAutoTest.IsDisposed)
            {
                fAutoTest = new FrmAutoTest();
            }
            fAutoTest.Show();
            fAutoTest.WindowState = FormWindowState.Maximized;
            fAutoTest.BringToFront();
        }

        private void toolBtnReport_Click(object sender, EventArgs e)
        {
            if (fReport.IsDisposed)
            {
                fReport = new frmDataShow();
            }
            fReport.Show();
        }

        private void toolBtnAbout_Click(object sender, EventArgs e)
        {
            frmAbout fa = new frmAbout();
            fa.ShowDialog();
        }

        private void 下位机连接CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (System.IO.File.Exists(cMain.path + "\\exe\\下位机连接测试.exe"))
            {
                Process.Start(cMain.path + "\\exe\\下位机连接测试.exe");
            }
        }

        private void 密码修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPassWord f = new frmPassWord();
            f.ShowDialog();
        }

        private void 数据查看ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fReport.IsDisposed)
            {
                fReport = new frmDataShow();
            }
            fReport.Show();
        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                if (fAutoTest.IsDisposed)
                {
                    fAutoTest = new FrmAutoTest();
                }
                fAutoTest.Show();
            }
            if (e.KeyCode == Keys.F6)
            {
                if (fReport.IsDisposed)
                {
                    fReport = new frmDataShow();
                }
                fReport.Show();
            }

        }

        private void engLishEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cMain.WriteIni("System", "Language", "en-US");
            isSwitch = true;
            Application.Exit();
            System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
        }

        private void 中文CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cMain.WriteIni("System", "Language", "zh-CN");
            isSwitch = true;
            Application.Exit();
            System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
        }

        private void 安规数据AToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void 历史设置SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSaveDataReport fs = new frmSaveDataReport();
            fs.Show();
        }

        private void frmMain_Shown(object sender, EventArgs e)
        {

            fAutoTest.Show();
            fAutoTest.BringToFront();
        }

        private void 关机SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSend fs = new frmSend("E", frmSend.SendValue.SendShutDown);
            fs.ShowDialog();
        }
    }
}