using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
namespace NewMideaProgram
{
    public partial class frmSet : Form
    {
        public static ArrayList controlList = new ArrayList();
        public static Panel labelCollectionPanel = new Panel();
        public  frmMain mFrmMain;
        CheckBox[] chkIndoor = new CheckBox[6];
        //static int ModeJiQi = 0;
        public frmSet()
        {
            //mFrmMain = FrmMain;
            InitializeComponent();
        }
        private void frmSet_Load(object sender, EventArgs e)
        {
            StartLoad();
            tabControl1.SelectedIndex = 1;
        }
        /// <summary>
        /// 因为主frmMdi页面添加此页面过去时不能自动调用本页的Load(object sender,EventArgs e)事件，所以要添加此全局函数供frmMdi手动调用
        /// </summary>
        public void StartLoad()
        {
            frmInit();
            DataClassToFrm(this, cMain.mModeSet);
            chkIndoor[0] = chkBZJ1;
            chkIndoor[1] = chkBZJ2;
            chkIndoor[2] = chkBZJ3;
            chkIndoor[3] = chkBZJ4;
            chkIndoor[4] = chkBZJ5;
            chkIndoor[5] = chkBZJ6;
            cbbBZJ.SelectedIndexChanged += new EventHandler(cbbBZJ_SelectedIndexChanged);
            cbbBZJ_SelectedIndexChanged(cbbBZJ, new EventArgs());
        }

        void cbbBZJ_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        private void frmInit()
        {
            if (!cMain.isComPuter)
            {
                this.Height = Screen.PrimaryScreen.Bounds.Height;
                this.Width = Screen.PrimaryScreen.Bounds.Width;
            }
            cMain.initFrom(groupBox1.Controls);
            cMain.initFrom(tabPage1.Controls);
            cMain.initFrom(tabPage3.Controls);
            cMain.initFrom(groupBox3.Controls);
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.TopMost = false;
            this.Visible = false;
            //frmMain.initTestData(mFrmMain);启动后点取消 已没数据会消失,所以要注释掉.
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            frmBarSet fb = new frmBarSet();
            fb.ShowDialog();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            this.dataGridProtect.CurrentCell = null;
            this.dataGridSet.CurrentCell = null;
            this.dataGridShow.CurrentCell = null;
            frmList fl = new frmList();
            if (fl.ShowDialog() == DialogResult.Yes  )
            {
                if (!fl.isError)
                {
                    cModeSet ModeSet;
                    if (DataFileToClass(fl.ReturnId, out ModeSet,true))
                    {
                        if (DataClassToFrm(this, ModeSet))
                        {
                            frmMideaSn.mSnSet = (cSnSet)cXml.readXml(string.Format("{0}{1}.xml", frmMideaSn.SnIdDirectory, ModeSet.mId), typeof(cSnSet), frmMideaSn.mSnSet);
                            return;
                        }
                    }
                    MessageBox.Show("数据加载错误,将自动生成默认数据", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (File.Exists(fl.ReturnId))
                    {
                        File.Delete(fl.ReturnId);
                    }
                    cModeSet modeSet = new cModeSet();
                    DataClassToFile(modeSet);
                    DataClassToFrm(this, modeSet);
                }
            }
            fl.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.dataGridProtect.EndEdit();
            this.dataGridSet.EndEdit();
            this.dataGridShow.EndEdit();
            if (DataFrmToClass(this, out cMain.mModeSet))
            {
                frmMideaSn.mSnSet.id = cMain.mModeSet.mId;
                if (DataClassToFile(cMain.mModeSet))
                {
                    cXml.saveXml(string.Format("{0}{1}.xml", frmMideaSn.SnIdDirectory, cMain.mModeSet.mId), typeof(cSnSet), frmMideaSn.mSnSet);
                    MessageBox.Show("数据保存", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            MessageBox.Show("数据保存错误,请检测数据是否正确后重新保存", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        /// <summary>
        /// 将界面上的值写入到设定类
        /// </summary>
        /// <param name="ModeSet">界面上的值写入到类,返回设置</param>
        /// <returns>bool,返回是否转换成功</returns>
        public static bool DataFrmToClass(frmSet mfrmSet,out cModeSet ModeSet)
        {
            
            cModeSet mModeSet = new cModeSet();
            bool isOk = false;
            int i, j;
            //DataTable dt = new DataTable();
            //dt = (DataTable)mfrmSet.dataGridProtect.DataSource;
            DataGridViewRow dr;
            try
            {
                mfrmSet.dataGridProtect.EndEdit();
                mfrmSet.dataGridSet.EndEdit();
                mModeSet.mId = mfrmSet.cbbId.Text;
                mModeSet.mMode = mfrmSet.txtMode.Text;
                mModeSet.mElect = mfrmSet.cbbElect.SelectedIndex;
                mModeSet.mBiaoZhunJi = mfrmSet.cbbBZJ.SelectedIndex;
                mModeSet.mBiaoZhunJi1 = mfrmSet.chkBZJ1.Checked;
                mModeSet.mBiaoZhunJi2 = mfrmSet.chkBZJ2.Checked;
                mModeSet.mBiaoZhunJi3 = mfrmSet.chkBZJ3.Checked;
                mModeSet.mBiaoZhunJi4 = mfrmSet.chkBZJ4.Checked;
                mModeSet.mBiaoZhunJi5 = mfrmSet.chkBZJ5.Checked;
                mModeSet.mBiaoZhunJi6 = mfrmSet.chkBZJ6.Checked;
                mModeSet.m24V = mfrmSet.cbb24V.SelectedIndex;
                mModeSet.mJiQi = mfrmSet.cbbMachine.SelectedIndex;
                for (i = 0; i < cMain.DataProtect; i++)
                {
                    if (i < 10)
                    {
                        dr = mfrmSet.dataGridProtect.Rows[i];
                            mModeSet.mProtect[i] = Num.SingleParse(dr.Cells[1].Value);
                        
                    }
                    else
                    {
                        dr = mfrmSet.dataGridProtect.Rows[i - 10];
                        mModeSet.mProtect[i] =Num.SingleParse(dr.Cells[3].Value);
                    }
                }


                for ( i = 0; i < cModeSet.StepCount; i++)
                {
                    dr = mfrmSet.dataGridSet.Rows[i];
                    mModeSet.mStepId[i] = dr.Cells[0].Value.ToString();
                    mModeSet.mSetTime[i] = Num.IntParse(dr.Cells[1].Value);
                    mModeSet.mSendStr[i] = dr.Cells[2].Value.ToString(); ;
                    for (j = 0; j < cMain.DataKaiGuang; j++)
                    {
                        mModeSet.mKaiGuan[i, j] = (bool)dr.Cells[j + 3].Value;
                    }
                    for (j = 0; j < cMain.DataShow * 2; j++)
                    {
                        if (j % 2 == 0)
                        {
                            mModeSet.mLowData[i, j / 2] = Num.SingleParse(dr.Cells[j + 3 + cMain.DataKaiGuang].Value);
                        }
                        else
                        {
                            mModeSet.mHighData[i, (j - 1) / 2] = Num.SingleParse(dr.Cells[j + 3 + cMain.DataKaiGuang].Value);
                        }
                    }
                }
                dr=mfrmSet.dataGridShow.Rows[0];
                for (i = 0; i < cMain.DataShow; i++)
                {
                    mModeSet.mShow[i] = (bool)dr.Cells[i].Value;
                }
                isOk = true;
            }
            catch(Exception exc)
            {
                cMain.WriteErrorToLog("FrmSet DataFrmToClass is Error " + exc.ToString());
                isOk = false;
            }
            ModeSet = mModeSet;
            return isOk;
        }
        public static bool DataClassToFile(cModeSet ModeSet, out string Str)
        {
            string temp = "";
            string writeStr = "";
            bool isOk = false;
            int i, j;
            string tempTime = "", tempSn = "";
            try
            {
                writeStr = writeStr + ModeSet.mId + "~";
                writeStr = writeStr + ModeSet.mMode + "~";
                writeStr = writeStr + ModeSet.mElect.ToString() + "~";
                writeStr = writeStr + ModeSet.mBiaoZhunJi.ToString() + "~";
                writeStr = writeStr + ModeSet.mBiaoZhunJi1.ToString() + "~";
                writeStr = writeStr + ModeSet.mBiaoZhunJi2.ToString() + "~";
                writeStr = writeStr + ModeSet.mBiaoZhunJi3.ToString() + "~";
                writeStr = writeStr + ModeSet.mBiaoZhunJi4.ToString() + "~";
                writeStr = writeStr + ModeSet.mBiaoZhunJi5.ToString() + "~";
                writeStr = writeStr + ModeSet.mBiaoZhunJi6.ToString() + "~";
                writeStr = writeStr + ModeSet.m24V.ToString() + "~";
                writeStr = writeStr + ModeSet.mJiQi.ToString() + "~";
                for (i = 0; i < cMain.DataProtect; i++)
                {
                    writeStr = writeStr + ModeSet.mProtect[i].ToString() + "~";
                }
                for (i = 0; i < cModeSet.StepCount; i++)
                {
                    writeStr = writeStr + ModeSet.mStepId[i] + "~";
                    tempTime = tempTime + ModeSet.mSetTime[i].ToString() + "~";
                    tempSn = tempSn + ModeSet.mSendStr[i] + "~";
                }
                writeStr = writeStr + tempTime + tempSn;
                for (i = 0; i < cMain.DataKaiGuang; i++)
                {
                    for (j = 0; j < cModeSet.StepCount; j++)
                    {

                        if (ModeSet.mKaiGuan[j, i])
                        {
                            writeStr = writeStr + "√" + "~";
                        }
                        else
                        {
                            writeStr = writeStr + "×" + "~";
                        }
                    }
                }
                for (i = 0; i < cMain.DataShow * 2; i++)
                {
                    for (j = 0; j < cModeSet.StepCount; j++)
                    {
                        if (i % 2 == 0)
                        {
                            writeStr = writeStr + ModeSet.mLowData[j, i / 2].ToString() + "~";
                        }
                        else
                        {
                            writeStr = writeStr + ModeSet.mHighData[j, (i - 1) / 2].ToString() + "~";
                        }
                    }
                }
                for (i = 0; i < cMain.DataShow; i++)
                {
                    writeStr = writeStr + ModeSet.mShow[i].ToString() + "~";
                }
                temp = writeStr;
                cMain.WriteFile(cMain.AppPath+"\\ID\\" + ModeSet.mId + ".txt", writeStr, false);
                isOk = true;
            }
            catch (Exception exc)
            {
                MessageBox.Show("FrmSet DataFrmToFile is Error " + exc.ToString());
                temp = "";
                isOk = false;
            }
            Str = temp;
            return isOk;
        }
        /// <summary>
        /// 将设置类转换成文本并保存起来
        /// </summary>
        /// <param name="ModeSet">设置</param>
        /// <returns>是否保存成功</returns>
        public static bool DataClassToFile(cModeSet ModeSet)//将界面上的数据保存
        {
            string temp;
            return DataClassToFile(ModeSet, out temp);
        }
        public static bool DataFileToClass(string FileStr,out cModeSet ModeSet,bool isPath)
        {
            cModeSet mModeset = new cModeSet();
            string readStr;
            if (isPath)
            {
                readStr = cMain.ReadFile(FileStr);
            }
            else
            {
                readStr = FileStr;
            }
            try
            {
                string[] tempStr;
                tempStr = readStr.Split('~');
                int i, j, index = 0;
                mModeset.mId = tempStr[index++];
                mModeset.mMode = tempStr[index++];
                mModeset.mElect = Num.IntParse(tempStr[index++]);
                mModeset.mBiaoZhunJi = Num.IntParse(tempStr[index++]);
                mModeset.mBiaoZhunJi1 = Num.BoolParse(tempStr[index++]);
                mModeset.mBiaoZhunJi2 = Num.BoolParse(tempStr[index++]);
                mModeset.mBiaoZhunJi3 = Num.BoolParse(tempStr[index++]);
                mModeset.mBiaoZhunJi4 = Num.BoolParse(tempStr[index++]);
                mModeset.mBiaoZhunJi5 = Num.BoolParse(tempStr[index++]);
                mModeset.mBiaoZhunJi6 = Num.BoolParse(tempStr[index++]);
                mModeset.m24V = Num.IntParse(tempStr[index++]);
                mModeset.mJiQi = Num.IntParse(tempStr[index++]);
                for (i = 0; i < cMain.DataProtect; i++)
                {
                    mModeset.mProtect[i] = Num.SingleParse(tempStr[index++]);
                }

                for (i = 0; i < cModeSet.StepCount; i++)
                {
                    mModeset.mStepId[i] = tempStr[index++];
                }
                for (i = 0; i < cModeSet.StepCount; i++)
                {
                    mModeset.mSetTime[i] = Num.IntParse(tempStr[index++]);
                }
                for (i = 0; i < cModeSet.StepCount; i++)
                {
                    mModeset.mSendStr[i] = tempStr[index++];
                }
                for (i = 0; i < cMain.DataKaiGuang; i++)
                {
                    for (j = 0; j < cModeSet.StepCount; j++)
                    {
                        if (tempStr[index++] == "√")
                        {
                            mModeset.mKaiGuan[j, i] = true;
                        }
                        else
                        {
                            mModeset.mKaiGuan[j, i] = false;
                        }
                    }
                }
                for (i = 0; i < cMain.DataShow * 2; i++)
                {
                    for (j = 0; j < cModeSet.StepCount; j++)
                    {
                        if (i % 2 == 0)
                        {
                            mModeset.mLowData[j, i / 2] = Num.SingleParse(tempStr[index++]);
                        }
                        else
                        {
                            mModeset.mHighData[j, i / 2] = Num.SingleParse(tempStr[index++]);
                        }
                    }
                }
                for (i = 0; i < cMain.DataShow; i++)
                {
                    mModeset.mShow[i] = Num.BoolParse(tempStr[index++]);
                }
            }
            catch 
            {
            }
            ModeSet = mModeset;
            return true;
        }
        public static bool DataClassToFrm(frmSet mfrmSet,cModeSet mModeSet)
        {
            bool isOk = false;
            int i, j;
            //try
            //{
                mfrmSet.cbbId.Text = mModeSet.mId;
                mfrmSet.txtMode.Text = mModeSet.mMode;
                mfrmSet.cbbElect.SelectedIndex = mModeSet.mElect;
                mfrmSet.cbbBZJ.SelectedIndex = mModeSet.mBiaoZhunJi;
                mfrmSet.chkBZJ1.Checked = mModeSet.mBiaoZhunJi1;
                mfrmSet.chkBZJ2.Checked = mModeSet.mBiaoZhunJi2;
                mfrmSet.chkBZJ3.Checked = mModeSet.mBiaoZhunJi3;
                mfrmSet.chkBZJ4.Checked = mModeSet.mBiaoZhunJi4;
                mfrmSet.chkBZJ5.Checked = mModeSet.mBiaoZhunJi5;
                mfrmSet.chkBZJ6.Checked = mModeSet.mBiaoZhunJi6;
                mfrmSet.cbb24V.SelectedIndex = mModeSet.m24V;
                mfrmSet.cbbMachine.SelectedIndex = mModeSet.mJiQi;

                for (i = 0; i < cMain.DataProtect; i++)
                {
                    DataGridViewRow dr;
                    if (i < 10)
                    {
                        if (mfrmSet.dataGridProtect.Rows[i].Cells[0].Value == null || 
                            mfrmSet.dataGridProtect.Rows[i].Cells[0].Value.ToString() == "")
                        {
                            continue;
                        }
                        dr = mfrmSet.dataGridProtect.Rows[i];
                        dr.Cells[1].Value = mModeSet.mProtect[i].ToString();
                    }
                    else
                    {
                        if (mfrmSet.dataGridProtect.Rows[i - 10].Cells[2].Value == null ||
                            mfrmSet.dataGridProtect.Rows[i - 10].Cells[2].Value.ToString() == "")
                        {
                            continue;
                        }
                        dr = mfrmSet.dataGridProtect.Rows[i - 10];
                        dr.Cells[3].Value = mModeSet.mProtect[i].ToString();
                    }
                }
                for (i = 0; i < cModeSet.StepCount; i++)
                {
                    DataGridViewRow dr = mfrmSet.dataGridSet.Rows[i];
                    dr.Cells[0].Value = mModeSet.mStepId[i];
                    dr.Cells[1].Value = mModeSet.mSetTime[i];
                    dr.Cells[2].Value = mModeSet.mSendStr[i];
                    for (j = 0; j < cMain.DataKaiGuang; j++)
                    {
                        dr.Cells[j + 3].Value = mModeSet.mKaiGuan[i, j];
                      
                    }
                    for (j = 0; j < cMain.DataShow * 2; j++)
                    {
                        if (j % 2 == 0)
                        {
                            dr.Cells[j + 3 + cMain.DataKaiGuang].Value = mModeSet.mLowData[i, j / 2];
                        }
                        else
                        {
                            dr.Cells[j + 3 + cMain.DataKaiGuang].Value = mModeSet.mHighData[i, (j - 1) / 2];
                        }
                    }
                }
                DataGridViewRow drShow = mfrmSet.dataGridShow.Rows[0];
                for (i = 0; i < cMain.DataShow; i++)
                {
                    drShow.Cells[i].Value = mModeSet.mShow[i];
                }
                isOk = true;
            //}
            //catch (Exception exc)
            //{
            //    cMain.WriteErrorToLog("FrmSet DataClssToFrm is Error " + exc.ToString());
            //    isOk = false;
            //}
            return isOk;
        }
        private void btnSend_Click(object sender, EventArgs e)
        {
            cModeSet modeSet;
            frmSet.DataFileToClass(cMain.AppPath+"\\ID\\" + cbbId.Text + ".txt", out modeSet, true);
            frmSend f = new frmSend(modeSet);
            f.ShowDialog();
        }

        private void dataGridSet_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("请输入正确格式的数据", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            
        }

        private void dataGridSet_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                frmMideaSn fm = new frmMideaSn(dataGridSet.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(),
                    cbbId.Text, e.RowIndex, cbbMachine.SelectedIndex);
                if (fm.ShowDialog() == DialogResult.Yes)
                {
                    dataGridSet.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = fm.Sn;
                    dataGridSet.Columns[e.ColumnIndex].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }
                fm.Dispose();
            }
        }

        private void dataGridShow_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
        }
        private void IndoorCheckBox_Changed(object sender, EventArgs e)
        {
        }
        private void cbbMachine_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridShow_CellClick(object sender, DataGridViewCellEventArgs e)
        {
        }
        void setShowColumns()
        {
            for (int i = 0; i < dataGridShow.ColumnCount; i++)
            {
                if (dataGridShow.Rows[0].Cells[i].Value != null)
                {
                    dataGridSet.Columns[13 + i * 2].Visible = Num.BoolParse(dataGridShow.Rows[0].Cells[i].Value);
                    dataGridSet.Columns[14 + i * 2].Visible = Num.BoolParse(dataGridShow.Rows[0].Cells[i].Value);
                }
            }
        }

        private void dataGridShow_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            setShowColumns();

        }
    }
}