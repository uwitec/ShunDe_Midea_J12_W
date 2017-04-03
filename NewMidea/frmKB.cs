using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Data.OleDb;
namespace NewMideaProgram
{
    public partial class frmKB : Form
    {
        //frmTest ft = new frmTest();
        public static string ErrStr = "";
        public frmKB()//构造函数
        {
            InitializeComponent();
        }
        private void btnOk_Click(object sender, EventArgs e)//确定按钮,将KB值赋给变量.并写入文件保存
        {
            KBGrid.EndEdit();
            if (DataFrmToClass(this, out cMain.mKBValue))
            {
                if (DataClassToFile(cMain.mKBValue))
                {
                    MessageBox.Show("Successfully Saved,数据保存成功".Split(',')[cMain.IndexLanguage], "OK,成功".Split(',')[cMain.IndexLanguage], MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            MessageBox.Show("Fail To Save Check Please,数据保存错误,请检测数据是否正确后重新保存".Split(',')[cMain.IndexLanguage], "Error,错误".Split(',')[cMain.IndexLanguage], MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void frmInit()
        {
            cMain.initFrom(panel1.Controls);
            for (int i = 0; i < KBGrid.ColumnCount; i++)
            {
                KBGrid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        private void frmKB_Load(object sender, EventArgs e)
        {
            StartLoad();
        }
        /// <summary>
        /// 因为主frmMdi页面添加此页面过去时不能自动调用本页的Load(object sender,EventArgs e)事件，所以要添加此全局函数供frmMdi手动调用
        /// </summary>
        public void StartLoad()
        {
            frmInit();
            DataFileToClass(cMain.AppPath+"\\KBValue.txt", out cMain.mKBValue);
            DataClassToFrm(this, cMain.mKBValue);
            timer1.Enabled = true;
        }
        /// <summary>
        /// 将类中的数据显示到界面
        /// </summary>
        /// <param name="mFrmKb">frmKB,要显示的界面,好像没什么用,就当备用,用的时候传this就行了</param>
        /// <param name="mKBValue">cKBValue,存储了KB值的类</param>
        /// <returns>bool,返回是否显示成功</returns>
        public static bool DataClassToFrm(frmKB mFrmKb,cKBValue mKBValue)//将KB显示在界面上.
        {
            bool isOk = false;
            try
            {
                int i = 0;
                int rowCount = 0;
                rowCount = (int)Math.Ceiling((double)(cMain.DataAll / 2.000));
                for (i = 0; i < cMain.DataAll; i++)
                {
                    DataGridViewRow dr;
                    if (i < rowCount)
                    {
                        dr = mFrmKb.KBGrid.Rows[i];
                        dr.Cells[3].Value = mKBValue.valueK[i];
                        dr.Cells[4].Value = mKBValue.valueB[i];
                    }
                    else
                    {
                        dr = mFrmKb.KBGrid.Rows[i - rowCount];
                        dr.Cells[9].Value =  mKBValue.valueK[i];
                        dr.Cells[10].Value = mKBValue.valueB[i];
                    }
                }
                isOk = true;
            }
            catch (Exception exc)
            {
                cMain.WriteErrorToLog("FrmKb DataClassToFrm is Error " + exc.ToString());
                isOk = false;
            }
            return isOk;
        }
        public static bool DataClassToFile(cKBValue KBValue)
        {
            bool isOk = false;
            string TempStr="";
            int i=0;
            try
            {
                for (i = 0; i < cMain.DataAll; i++)
                {
                    TempStr = TempStr + KBValue.valueK[i].ToString() + "~";
                    TempStr = TempStr + KBValue.valueB[i].ToString() + "~";
                }
                cMain.WriteFile(cMain.AppPath+"\\KBValue.txt", TempStr, false);
                isOk = true;
            }
            catch (Exception exc)
            {
                cMain.WriteErrorToLog("FrmKb DataClassToFile is Error " + exc.ToString());
                isOk = false;
            }
            return isOk;
        }
        public static bool DataFileToClass(string FileStr, out cKBValue KBValue)//从文件加载KB值到变量
        {
            bool isOk = false;
            cKBValue mKBValue;
            string readFile = cMain.ReadFile(FileStr);
            string[] tempStr;
            int i = 0;
            try
            {
                mKBValue = new cKBValue();
                tempStr = readFile.Split('~');
                for (i = 0; i < cMain.DataAll; i++)
                {
                    mKBValue.valueK[i] = Num.DoubleParse(tempStr[2 * i]);
                    mKBValue.valueB[i] = Num.DoubleParse(tempStr[2 * i + 1]);
                }
                isOk = true;
            }
            catch (Exception exc)
            {
                mKBValue = new cKBValue();
                cMain.WriteErrorToLog("FrmKb DataFileToClass is Error " + exc.ToString());
                isOk = false;
            }
            KBValue = mKBValue;
            return isOk;
        }
         public static bool DataFrmToClass(frmKB mfrmKb,out cKBValue KBValue)
        {
            bool isOk = false;
            cKBValue mKBValue = new cKBValue();
            int rowCount = 0;
            rowCount = (int)Math.Ceiling((double)(cMain.DataAll / 2.000));
            int i;
            try
            {
                for (i = 0; i < cMain.DataAll; i++)
                {
                    DataGridViewRow dr;
                    if (i < rowCount)
                    {
                        dr = mfrmKb.KBGrid.Rows[i];
                        mKBValue.valueK[i] = Num.DoubleParse(dr.Cells[3].Value);
                        mKBValue.valueB[i] = Num.DoubleParse(dr.Cells[4].Value);
                    }
                    else
                    {
                        dr = mfrmKb.KBGrid.Rows[i - rowCount];
                        mKBValue.valueK[i] = Num.DoubleParse(dr.Cells[9].Value);
                        mKBValue.valueB[i] = Num.DoubleParse(dr.Cells[10].Value);
                    }
                }
                isOk = true;
            }
            catch(Exception exc)
            {
                cMain.WriteErrorToLog("FrmKb DataFrmToClass is Error " + exc.ToString());
                isOk = false;
            }
            KBValue = mKBValue;
            return isOk;
        }
        private void btnTest_Click(object sender, EventArgs e)
        {
            //if (cMain.isNeedPassWord)
            //{
            //    frmPassWord fp = new frmPassWord();
            //    if (fp.ShowDialog() != DialogResult.Yes)
            //    {
            //        return;
            //    }
            //}
            //ft.Show();
            //ft.Visible = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int i;
            int rowCount = 0;
            rowCount = (int)Math.Ceiling((double)(cMain.DataAll / 2.000));
            for (i = 0; i < cMain.DataAll; i++)
            {
                DataGridViewRow dr;
                if (i < rowCount)
                {
                    dr = KBGrid.Rows[i];
                    dr.Cells[2].Value = frmMain.dataRead[i];
                    dr.Cells[5].Value = (frmMain.dataRead[i] * Num.DoubleParse(dr.Cells[3].Value) + Num.DoubleParse(dr.Cells[4].Value));
                }
                else
                {
                    dr = KBGrid.Rows[i - rowCount];
                    dr.Cells[8].Value = frmMain.dataRead[i];
                    dr.Cells[11].Value = (frmMain.dataRead[i] * Num.DoubleParse(dr.Cells[9].Value) + Num.DoubleParse(dr.Cells[10].Value));
                }
            }
        }

        private void toolBtnJiLiang_Click(object sender, EventArgs e)
        {
            KBGrid.EndEdit();
            int jiLiangCount = 0;
            bool[] isJiLiang = new bool[cMain.DataAll];
            int tempLen=(int)Math.Ceiling(cMain.DataAll/2.000);
            for (int i = 0; i < cMain.DataAll; i++)
            {
                DataGridViewCell dc;
                if (i < tempLen)
                {
                    dc = KBGrid.Rows[i].Cells[1];
                }
                else
                {
                    dc = KBGrid.Rows[i - tempLen].Cells[7];
                }
                if (dc.Value != null)
                {
                    if (dc.Value.Equals(true))
                    {
                        jiLiangCount++;
                        isJiLiang[i] = true;
                    }
                    else
                    {
                        isJiLiang[i] = false;
                    }
                }
                else
                {
                    isJiLiang[i] = false;
                }
            }
            if (jiLiangCount == 0)
            {
                MessageBox.Show("Have No Choice,当前没有选中要计量的项目,请选择要计量的项目".Split(',')[cMain.IndexLanguage], "Info,提示".Split(',')[cMain.IndexLanguage], MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            timer1.Enabled = false;
            frmJiLiang fj = new frmJiLiang(isJiLiang);
            fj.ShowDialog();
            DataFileToClass(cMain.AppPath + "\\KBValue.txt", out cMain.mKBValue);
            DataClassToFrm(this, cMain.mKBValue);
            timer1.Enabled = true;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //"A相电压(V),B相电压(V),B相电压(V),A相电流(A),B相电流(A),C相电流(A),A相功率(W),B相功率(W),C相功率(W),"+
            //"R22进管温度(℃),R22出管温度(℃),R410进管温度(℃),R410出管温度(℃),吸气温度(℃),排气温度(℃),R22进风温度(℃),"+
            //"R22出风温度(℃),R410进风温度(℃),R410出风温度(℃),R22高压压力(Mpa),R22低压压力(Mpa),R410高压压力(Mpa),R410低压压力(Mpa),"+
            //"1#进风温度(℃),1#出风温度(℃),2#进风温度(℃),2#出风温度(℃),3#进风温度(℃),3#出风温度(℃),4#进风温度(℃),4#出风温度(℃),"+
            //"1#进管压力(Mpa),1#出管压力(Mpa),2#进管压力(Mpa),2#出管压力(Mpa),3#进管压力(Mpa),3#出管压力(Mpa),4#进管压力(Mpa),4#出管压力(Mpa),"+
            //"1#进管温度(℃),1#出管温度(℃),2#进管温度(℃),2#出管温度(℃),3#进管温度(℃),3#出管温度(℃),4#进管温度(℃),4#出管温度(℃)"
        
            if (File.Exists(".\\Data\\KBValue.mdb"))
            {
                OleDbConnection KbData = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|\\Data\\KBValue.mdb;Persist Security Info=True");
                KbData.Open();
                string[] title=cMain.DataAllTitleStr[cMain.IndexLanguage].Split(',');
                Dictionary<string, int> oldKBValueIndex = new Dictionary<string, int>();
                for (int i = 0; i < cMain.DataAll; i++)
                {
                    oldKBValueIndex.Add(title[i], i+1);
                }
                DataSet ds;
                cJiLiang tmpJiLiang = new cJiLiang();
                string error;
                List<double> x = new List<double>();
                List<double> y = new List<double>();
                for (int i = 0; i < cMain.DataAll; i++)
                {
                    x.Clear();
                    y.Clear();
                    if (!oldKBValueIndex.ContainsKey(title[i]))
                    {
                        continue;
                    }
                    ds = cData.readData(string.Format("select X,Y From TValue{0}", oldKBValueIndex[title[i]]), KbData);
                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                    {
                        tmpJiLiang.jiLiangTempClass[i].IsUseData[j] = true;
                        tmpJiLiang.jiLiangTempClass[i].ReadData[j] = Num.DoubleParse(ds.Tables[0].Rows[j]["X"]);
                        tmpJiLiang.jiLiangTempClass[i].CureData[j] = Num.DoubleParse(ds.Tables[0].Rows[j]["Y"]);
                        x.Add(Num.DoubleParse(ds.Tables[0].Rows[j]["X"]));
                        y.Add(Num.DoubleParse(ds.Tables[0].Rows[j]["Y"]));
                    }
                    frmJiLiang.JiLiangByXianXing(x, y, out cMain.mKBValue.valueK[i], out cMain.mKBValue.valueB[i], out error);
                }
                KbData.Close();
                KbData.Dispose();
                frmJiLiang.DataClassToFile(tmpJiLiang);
                frmKB.DataClassToFile(cMain.mKBValue);
                DataClassToFrm(this, cMain.mKBValue);
            }
            else
            {
                MessageBox.Show("安装目录Data文件夹下不存在KBValue.mdb文件", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolBtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}