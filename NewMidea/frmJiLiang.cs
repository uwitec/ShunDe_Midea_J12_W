using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;
namespace NewMideaProgram
{
    public partial class frmJiLiang : Form
    {
        //frmTest ft = new frmTest();
        //delegate void txtClickRaise(object sender,EventArgs e);
        //event txtClickRaise EventTxtClick; 
        public static string ErrStr = "";
        bool[] isJiLiang;
        int jiLiangCount = 0;
        cJiLiang mJiLiang = new cJiLiang();
        bool isAutoFlush = true;
        public frmJiLiang(bool[] IsJiLiang)//构造函数
        {
            isJiLiang = IsJiLiang;
            InitializeComponent();
        }
        private void btnOk_Click(object sender, EventArgs e)//确定按钮,将KB值赋给变量.并写入文件保存
        {
            string error="";
            KBGrid.EndEdit();
            if (DataFrmToClass(this))
            {
                if (DataClassToFile(mJiLiang))
                {
                    string[] tempStr = cMain.DataAllTitleStr[cMain.IndexLanguage].Split(',');
                    for (int i = 0; i < readGrid.Columns.Count; i++)
                    {
                        string tempName = readGrid.Columns[i].HeaderText;
                        for (int j = 0; j < cMain.DataAll; j++)
                        {
                            if (tempStr[j] == tempName)
                            {
                                List<double> x = new List<double>();
                                List<double> y = new List<double>();
                                for (int k = 0; k < 10; k++)
                                {
                                    DataGridViewRow dr = KBGrid.Rows[k];
                                    if ((dr.Cells[i * 3].Value == null) ? false : (bool)dr.Cells[i * 3].Value)
                                    {
                                        x.Add(Num.DoubleParse(dr.Cells[i * 3 + 1].Value));
                                        y.Add(Num.DoubleParse(dr.Cells[i * 3 + 2].Value));
                                    }
                                }
                                JiLiangByXianXing(x, y, out cMain.mKBValue.valueK[j], out cMain.mKBValue.valueB[j], out error);
                                break;
                            }
                        }
                    }
                    frmKB.DataClassToFile(cMain.mKBValue);

                    MessageBox.Show("Successfully Saved,数据保存成功".Split(',')[cMain.IndexLanguage], "Ok,成功".Split(',')[cMain.IndexLanguage], MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            MessageBox.Show("Fail Saved. Check it,数据保存错误,请检测数据是否正确后重新保存".Split(',')[cMain.IndexLanguage], "Error,错误".Split(',')[cMain.IndexLanguage], MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void frmInit()
        {
            string[] tempStr = cMain.DataAllTitleStr[cMain.IndexLanguage].Split(',');
            jiLiangCount = 0;
            for (int i = 0; i < cMain.DataAll; i++)
            {
                if (isJiLiang[i])
                {
                    jiLiangCount++;
                    //计量Grid
                    DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
                    dataGridViewCheckBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewCheckBoxColumn.DefaultCellStyle.BackColor = Color.LightPink;
                    dataGridViewCheckBoxColumn.HeaderText = tempStr[i] + "Use?,是否启用".Split(',')[cMain.IndexLanguage];
                    dataGridViewCheckBoxColumn.ValueType = typeof(bool);
                    KBGrid.Columns.Add(dataGridViewCheckBoxColumn);

                    DataGridViewTextBoxColumn dataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
                    dataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewTextBoxColumn.DefaultCellStyle.Format = "0.000";
                    dataGridViewTextBoxColumn.HeaderText = tempStr[i] + "Read,读取值".Split(',')[cMain.IndexLanguage];
                    dataGridViewTextBoxColumn.ValueType = typeof(double);
                    KBGrid.Columns.Add(dataGridViewTextBoxColumn);

                    dataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
                    dataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewTextBoxColumn.DefaultCellStyle.Format = "0.000";
                    dataGridViewTextBoxColumn.HeaderText = tempStr[i] + "Reality,实际值".Split(',')[cMain.IndexLanguage];
                    dataGridViewTextBoxColumn.ValueType = typeof(double);
                    KBGrid.Columns.Add(dataGridViewTextBoxColumn);

                    //读取Grid
                    dataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
                    dataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dataGridViewTextBoxColumn.DefaultCellStyle.Format = "0.000";
                    dataGridViewTextBoxColumn.HeaderText = tempStr[i];
                    dataGridViewTextBoxColumn.ValueType = typeof(double);
                    readGrid.Columns.Add(dataGridViewTextBoxColumn);

                }
            }
            if (jiLiangCount > 0)
            {
                KBGrid.Rows.Add(10);
                KBGrid.RowHeadersWidth = 100;
                for (int i = 0; i < 10; i++)
                {
                    KBGrid.Rows[i].HeaderCell.Value = string.Format("Item {0:D2},第{0:D2}组数据".Split(',')[cMain.IndexLanguage], i + 1);
                }
                readGrid.Rows.Add(2);
                readGrid.RowHeadersWidth = 100;
                readGrid.Rows[0].HeaderCell.Value = "Read,读取值".Split(',')[cMain.IndexLanguage];
                readGrid.Rows[1].HeaderCell.Value = "Reality,计量值".Split(',')[cMain.IndexLanguage];
            }
        }
        private void frmKB_Load(object sender, EventArgs e)
        {
            StartLoad();
        }
        public void StartLoad()
        {
            frmInit();
            DataFileToClass(cMain.AppPath+"\\KBValue.xml", out mJiLiang);
            DataClassToFrm(this, mJiLiang);
        }
        /// <summary>
        /// 将类中的数据显示到界面
        /// </summary>
        /// <param name="mFrmKb">frmKB,要显示的界面,好像没什么用,就当备用,用的时候传this就行了</param>
        /// <param name="mKBValue">cKBValue,存储了KB值的类</param>
        /// <returns>bool,返回是否显示成功</returns>
        public static bool DataClassToFrm(frmJiLiang mFrmKb, cJiLiang mKBValue)//将KB显示在界面上.
        {
            bool isOk = false;
            try
            {
                for (int j = 0; j < 10; j++)
                {
                    DataGridViewRow dr = mFrmKb.KBGrid.Rows[j];
                    int index = 0;
                    for (int i = 0; i < cMain.DataAll; i++)
                    {
                        if (mFrmKb.isJiLiang[i])
                        {
                            dr.Cells[index * 3].Value = mKBValue.jiLiangTempClass[i].IsUseData[j];
                            dr.Cells[index * 3 + 1].Value = mKBValue.jiLiangTempClass[i].ReadData[j];
                            dr.Cells[index * 3 + 2].Value = mKBValue.jiLiangTempClass[i].CureData[j];
                            index++;
                        }
                    }
                }
                isOk = true;
            }
            catch (Exception exc)
            {
                cMain.WriteErrorToLog("FrmJiLiang DataClassToFrm is Error " + exc.ToString());
                isOk = false;
            }
            return isOk;
        }
        public static bool DataClassToFile(cJiLiang KBValue)
        {
            bool isOk = false;
            try
            {
                cXml.saveXml(cMain.AppPath+"\\KBValue.xml", typeof(cJiLiang), KBValue);
                isOk = true;
            }
            catch (Exception exc)
            {
                cMain.WriteErrorToLog("FrmJiLiang DataClassToFile is Error " + exc.ToString());
                isOk = false;
            }
            return isOk;
        }
        public static bool DataFileToClass(string FileStr, out cJiLiang KBValue)//从文件加载KB值到变量
        {
            bool isOk = false;
            cJiLiang mKBValue = new cJiLiang();
            try
            {
                mKBValue = (cJiLiang)cXml.readXml(FileStr, typeof(cJiLiang), mKBValue);
                isOk = true;
            }
            catch (Exception exc)
            {
                cMain.WriteErrorToLog("FrmJiLiang DataFileToClass is Error " + exc.Message);
                isOk = false;
            }
            KBValue = mKBValue;
            return isOk;
        }
        public static bool DataFrmToClass(frmJiLiang mfrmKb)
        {
            bool isOk = false;
            string[] tempStr = cMain.DataAllTitleStr[cMain.IndexLanguage].Split(',');
            try
            {
                for (int j = 0; j < mfrmKb.readGrid.Columns.Count; j++)
                {
                    for (int i = 0; i < cMain.DataAll; i++)
                    {
                        if (tempStr[i] == mfrmKb.readGrid.Columns[j].HeaderText)
                        {
                            for (int k = 0; k < 10; k++)
                            {
                                DataGridViewRow dr = mfrmKb.KBGrid.Rows[k];
                                mfrmKb.mJiLiang.jiLiangTempClass[i].IsUseData[k] = (dr.Cells[j * 3].Value == null) ? false : (bool)dr.Cells[j * 3].Value;
                                mfrmKb.mJiLiang.jiLiangTempClass[i].ReadData[k] = Num.DoubleParse(dr.Cells[j * 3 + 1].Value);
                                mfrmKb.mJiLiang.jiLiangTempClass[i].CureData[k] = Num.DoubleParse(dr.Cells[j * 3 + 2].Value);
                            }
                            break;
                        }
                    }
                }
                isOk = true;
            }
            catch(Exception exc)
            {
                cMain.WriteErrorToLog("FrmJiLiang DataFrmToClass is Error " + exc.ToString());
                isOk = false;
            }
            return isOk;
        }


        private void toolBtnJiLiang_Click(object sender, EventArgs e)
        {
            //frmKB fj = new frmKB();
            //fj.StartLoad();
            //fj.Show();
            //this.Close();
        }

        private void toolBtnCancel_Click(object sender, EventArgs e)
        {
            DataGridViewRow dr;
            for (int j = 0; j < 10; j++)
            {
                dr = KBGrid.Rows[j];
                for (int i = 0; i < readGrid.Columns.Count; i++)
                {
                    dr.Cells[i * 3].Value = false;
                    dr.Cells[i * 3 + 1].Value = 0;
                    dr.Cells[i * 3 + 2].Value = 0;
                }
            }
            DataFrmToClass(this);
            DataClassToFile(mJiLiang);

            //清除frmKb中的KB值
            string[] tempStr = cMain.DataAllTitleStr[cMain.IndexLanguage].Split(',');
            for (int i = 0; i < readGrid.Columns.Count; i++)
            {
                string tempName = readGrid.Columns[i].HeaderText;
                for (int j = 0; j < cMain.DataAll; j++)
                {
                    if (tempStr[j] == tempName)
                    {
                        cMain.mKBValue.valueK[j] = 1;
                        cMain.mKBValue.valueB[j] = 0;
                        break;
                    }
                }
            }
            frmKB.DataClassToFile(cMain.mKBValue);
        }

        private void toolBtnFlush_Click(object sender, EventArgs e)
        {
            if (toolBtnFlush.Tag.ToString().ToUpper() == "STOP")
            {
                isAutoFlush = false;
                toolBtnFlush.Tag = "Start";
                toolBtnFlush.Text = "自动刷新(&F)";
            }
            else
            {
                isAutoFlush = true;
                toolBtnFlush.Tag = "Stop";
                toolBtnFlush.Text = "停止自动刷新(&F)";
            }
        }
        public static bool JiLiangByXianXing(List<double> x, List<double> y, out double k, out double b, out string errorStr)
        {
            k = 1;
            b = 0;
            errorStr = "";
            bool isOk = false;
            int len = Math.Min(x.Count, y.Count);
            if (len < 2)
            {
                errorStr = "数据量太少,不能计量";
            }
            else
            {
                double Sx = 0, Sy = 0, AvgX = 0, AvgY = 0;
                x.ForEach(value => Sx += value);
                y.ForEach(value => Sy += value);
                AvgX = Sx / (double)len;
                AvgY = Sy / (double)len;
                Sx = 0; Sy = 0;
                for (int i = 0; i < len; i++)
                {
                    Sx += (AvgX - x[i]) * (AvgX - x[i]);
                    Sy += (AvgX - x[i]) * (AvgY - y[i]);
                }
                if (Sy != 0)
                {
                    k = Sy / Sx;
                    b = AvgY - AvgX * k;
                }
                isOk = true;
            }
            return isOk;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int index = 0;
            if (isAutoFlush)
            {
                for (int i = 0; i < cMain.DataAll; i++)
                {
                    if (isJiLiang[i])
                    {
                        DataGridViewRow dr = readGrid.Rows[0];
                        dr.Cells[index].Value = frmMain.dataRead[i];
                        dr = readGrid.Rows[1];
                        dr.Cells[index].Value = frmMain.dataRead[i] * cMain.mKBValue.valueK[i] + cMain.mKBValue.valueB[i];
                        index++;
                    }
                }
            }
        }
    }
    /// <summary>
    /// 计量数据类
    /// </summary>
    public class cJiLiang
    {
        public cJiLiangTempClass[] jiLiangTempClass = new cJiLiangTempClass[cMain.DataAll];
        public cJiLiang()
        {
            for (int i = 0; i < cMain.DataAll; i++)
            {
                jiLiangTempClass[i] = new cJiLiangTempClass();
            }
        }
    }
    /// <summary>
    /// 计量数据辅助类
    /// </summary>
    public class cJiLiangTempClass
    {
        public double[] ReadData = new double[10];
        public double[] CureData = new double[10];
        public bool[] IsUseData = new bool[10];
        public cJiLiangTempClass()
        {
            for (int j = 0; j < 10; j++)
            {
                IsUseData[j] = false;
                ReadData[j] = 0;
                CureData[j] = 0;
            }
        }
    }
}