using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace NewMideaProgram
{
    public partial class frmSys : Form
    {
        public frmSys()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            GridSys.EndEdit();
            if (DataFrmToClass(this, out cMain.mSysSet))
            {
                if (DataClassToFile(cMain.mSysSet))
                {
                    MessageBox.Show("数据保存成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }
            MessageBox.Show("数据保存失败,请检查数据输入是否有效","错误",MessageBoxButtons.OK,MessageBoxIcon.Error);
            
        }

        private void frmSys_Load(object sender, EventArgs e)
        {
            StartLoad();
        }
        /// <summary>
        /// 因为主frmMdi页面添加此页面过去时不能自动调用本页的Load(object sender,EventArgs e)事件，所以要添加此全局函数供frmMdi手动调用
        /// </summary>
        public void StartLoad()
        {
            frmInit();
            cSystemSet css;
            DataFileToClass(cMain.AppPath+"\\SystemInfo.txt", out css, true);
            DataClassToFrm(this, css);
        }
        private void frmInit()
        {
            cMain.initFrom(panSysGrid.Controls);
        }
        public static bool DataClassToFrm(frmSys mFrmSys, cSystemSet SystemSet)
        {
            bool isOk = false;
            int i = 0, index = 0;
            try
            {
                mFrmSys.GridSys.Rows[index++].Cells[1].Value = SystemSet.mPrevBar;
                mFrmSys.GridSys.Rows[index++].Cells[1].Value = SystemSet.mPrevId;
                mFrmSys.GridSys.Rows[index++].Cells[1].Value = SystemSet.mPLCCOM;
                mFrmSys.GridSys.Rows[index++].Cells[1].Value = SystemSet.mBarCom;
                mFrmSys.GridSys.Rows[index++].Cells[1].Value = SystemSet.m485COM;
                mFrmSys.GridSys.Rows[index++].Cells[1].Value = SystemSet.mSnCom;
                mFrmSys.GridSys.Rows[index++].Cells[1].Value = SystemSet.mPQCom;
                mFrmSys.GridSys.Rows[index++].Cells[1].Value = SystemSet.mPassWord;
                for (i = 0; i < cMain.mSysSet.PressK.Length; i++)
                {
                    mFrmSys.GridSys.Rows[index++].Cells[1].Value = SystemSet.PressK[i];
                }
                for (i = 0; i < cMain.mSysSet.PressB.Length; i++)
                {
                    mFrmSys.GridSys.Rows[index++].Cells[1].Value = SystemSet.PressB[i];
                }
                mFrmSys.GridSys.Rows[index++].Cells[1].Value = SystemSet.ZYSFArea;
                mFrmSys.GridSys.Rows[index++].Cells[1].Value = SystemSet.ZYSFDoing;
                isOk = true;
            }
            catch (Exception exc)
            {
                cMain.WriteErrorToLog("FrmSys DataClassToFrm is Error " + exc.ToString());
                isOk = false;
            }
            return isOk;
        }
        public static bool DataFrmToClass(frmSys mFrmSys, out cSystemSet SystemSet)
        {
            bool isOk = false;
            cSystemSet mSystemSet = new cSystemSet();
            int i;
            int index = 0;
            try
            {
                mSystemSet.mPrevBar = mFrmSys.GridSys.Rows[index++].Cells[1].Value.ToString();
                mSystemSet.mPrevId = mFrmSys.GridSys.Rows[index++].Cells[1].Value.ToString();
                mSystemSet.mPLCCOM = mFrmSys.GridSys.Rows[index++].Cells[1].Value.ToString();
                mSystemSet.mBarCom = mFrmSys.GridSys.Rows[index++].Cells[1].Value.ToString();
                mSystemSet.m485COM = mFrmSys.GridSys.Rows[index++].Cells[1].Value.ToString();
                mSystemSet.mSnCom = mFrmSys.GridSys.Rows[index++].Cells[1].Value.ToString();
                mSystemSet.mPQCom = mFrmSys.GridSys.Rows[index++].Cells[1].Value.ToString();
                mSystemSet.mPassWord = Num.StringParse(mFrmSys.GridSys.Rows[index++].Cells[1].Value);
                for (i = 0; i < mSystemSet.PressK.Length; i++)
                {
                    mSystemSet.PressK[i] = Num.DoubleParse(mFrmSys.GridSys.Rows[index++].Cells[1].Value);
                }
                for (i = 0; i < mSystemSet.PressB.Length; i++)
                {
                    mSystemSet.PressB[i] = Num.DoubleParse(mFrmSys.GridSys.Rows[index++].Cells[1].Value);
                }
                mSystemSet.ZYSFArea = mFrmSys.GridSys.Rows[index++].Cells[1].Value.ToString().ToInt();
                mSystemSet.ZYSFDoing = mFrmSys.GridSys.Rows[index++].Cells[1].Value.ToString().ToInt();
                isOk = true;
            }
            catch (Exception exc)
            {
                cMain.WriteErrorToLog("FrmSys DataFrmToClass is Error " + exc.ToString());
                isOk = false;
            }
            SystemSet = mSystemSet;
            return isOk;
        }

        public static bool DataFileToClass(string FileStr, out cSystemSet SystemSet, bool isPath)
        {
            bool isOk = false;
            cSystemSet mSystemSet=new cSystemSet();
            string readFile;
            int index = 0;
            if (isPath)
            {
                readFile = cMain.ReadFile(FileStr);
            }
            else
            {
                readFile = FileStr;
            }
            try
            {
                string[] tempStr;
                tempStr = readFile.Split('~');
                int i;
                mSystemSet.mPrevBar = tempStr[index++];
                mSystemSet.mPrevId = tempStr[index++];
                mSystemSet.mPLCCOM = tempStr[index++];
                mSystemSet.mBarCom = tempStr[index++];
                mSystemSet.m485COM = tempStr[index++];
                mSystemSet.mSnCom = tempStr[index++];
                mSystemSet.mPQCom = tempStr[index++];
                mSystemSet.mPassWord = tempStr[index++];
                for (i = 0; i < mSystemSet.PressK.Length; i++)
                {
                    mSystemSet.PressK[i] = Num.DoubleParse(tempStr[index++]);
                }
                for (i = 0; i < mSystemSet.PressK.Length; i++)
                {
                    mSystemSet.PressB[i] = Num.DoubleParse(tempStr[index++]);
                }
                mSystemSet.ZYSFArea = tempStr[index++].ToInt();
                mSystemSet.ZYSFDoing = tempStr[index++].ToInt();
                isOk = true;
            }
            catch (Exception exc)
            {
                //mSystemSet = new cSystemSet();
                cMain.WriteErrorToLog("FrmSys DataFileToClass is Error " + exc.ToString());
                isOk = false;
            }
            SystemSet = mSystemSet;
            return isOk;
        }
        public static bool DataClassToFile(cSystemSet SystemSet)
        {
            bool isOk = false;
            string tempStr = "";
            int i = 0;
            try
            {
                tempStr = tempStr + SystemSet.mPrevBar + "~";
                tempStr = tempStr + SystemSet.mPrevId + "~";
                tempStr = tempStr + SystemSet.mPLCCOM + "~";
                tempStr = tempStr + SystemSet.mBarCom + "~";
                tempStr = tempStr + SystemSet.m485COM + "~";
                tempStr = tempStr + SystemSet.mSnCom + "~";
                tempStr = tempStr + SystemSet.mPQCom + "~";
                tempStr = tempStr + SystemSet.mPassWord + "~";
                for (i = 0; i < SystemSet.PressK.Length; i++)
                {
                    tempStr = tempStr + SystemSet.PressK[i].ToString() + "~";
                }
                for (i = 0; i < SystemSet.PressK.Length; i++)
                {
                    tempStr = tempStr + SystemSet.PressB[i].ToString() + "~";
                }
                tempStr = tempStr + SystemSet.ZYSFArea + "~";
                tempStr = tempStr + SystemSet.ZYSFDoing + "~";
                cMain.WriteFile(cMain.AppPath+"\\SystemInfo.txt", tempStr, false);
                isOk = true;
            }
            catch (Exception exc)
            {
                cMain.WriteErrorToLog("FrmSys DataClassToFile is Error " + exc.ToString());
                isOk = false;
            }
            return isOk;
        }

        private void GridSys_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void toolBtnLanguage_Click(object sender, EventArgs e)
        {
            frmSys.DataClassToFile(cMain.mSysSet);
            Application.Exit();
            System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
        }

        private void toolBtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}