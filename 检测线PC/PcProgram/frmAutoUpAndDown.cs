using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
namespace PcProgram
{
    public partial class frmAutoUpAndDown : Form
    {
        CheckBox[] chkData = new CheckBox[cMain.DataShow];

        double[] MaxValue = new double[cMain.DataShow];
        double[] MinValue = new double[cMain.DataShow];
        double[] SumValue = new double[cMain.DataShow];
        bool[] isSelect = new bool[100];//本来应该是20个的.但数据上传过来时,时间可能一样,即可能找出不只20个数据
        enum FlushDataGridView
        {
            FlushAll,
            FlushSaveData,
            FlushStepData
        }
        public frmAutoUpAndDown()
        {
            InitializeComponent();
        }

        private void frmAutoUpAndDown_Load(object sender, EventArgs e)
        {
            initData();
            for (int i = 0; i < isSelect.Length; i++)
            {
                isSelect[i] = false;
            }
        }
        private void initData()
        {
            //初始化CHEKCBOX
            IEnumerator FormEnum = gpData.Controls.GetEnumerator();
            int nowCheckBoxIndex;
            CheckBox nowCheckBox;
            string[] tmpStr;
            while (FormEnum.MoveNext())
            {
                if (FormEnum.Current is CheckBox)
                {
                    nowCheckBox=(CheckBox)FormEnum.Current;
                    nowCheckBoxIndex = Num.IntParse(nowCheckBox.Text.Substring(8));
                    if (nowCheckBoxIndex <= cMain.DataShow)
                    {
                        nowCheckBox.Visible = true;
                        chkData[nowCheckBoxIndex - 1] = nowCheckBox;
                    }
                    else
                    {
                        nowCheckBox.Visible = false;
                    }
                }
            }
            tmpStr = cMain.DataShowTitleStr.Split(',');
            for (int i = 0; i < cMain.DataShow; i++)
            {
                chkData[i].Text = tmpStr[i];
                if (tmpStr[i].IndexOf("功率") >= 0)
                {
                    chkData[i].Checked = true;
                }
                if (tmpStr[i].IndexOf("压力") >= 0)
                {
                    chkData[i].Checked = true;
                }
            }
            //初始化cbbId
            string sqlCommand = "Select distinct id from allData";
            DataSet tmpDs = cData.readData(sqlCommand, cData.ConnData);
            cbbId.Items.Clear();
            if (tmpDs.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in tmpDs.Tables[0].Rows)
                {
                    cbbId.Items.Add(dr["ID"]);
                }
            }
            cbbStepId.Items.Clear();
            for (int i = 0; i < 10; i++)
            {
                cbbStepId.Items.Add(string.Format("{0}", i + 1));
            }
            cbbStepId.Text = "1";
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            dataGridView1.SuspendLayout();
            dataGridView1.EndEdit();
            if (dataGridView1.Rows.Count > 0)
            {
                for (int i = 2; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Cells[0].Value != null)
                    {
                        isSelect[i - 2] = (bool)dataGridView1.Rows[i].Cells[0].Value;
                    }
                    else
                    {
                        isSelect[i - 2] = false;
                    }
                }
            }
            LoadData(FlushDataGridView.FlushAll);
            if (dataGridView1.Rows.Count > 0)
            {
                for (int i = 2; i < dataGridView1.Rows.Count; i++)
                {
                    dataGridView1.Rows[i].Cells[0].Value = isSelect[i - 2];
                }
            }
            dataGridView1.ResumeLayout();
        }
        private void LoadData(FlushDataGridView flushDataGridView)
        {
            DataGridViewTextBoxColumn dataGridViewTextBoxColumn;
            DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn;
            string[] tmpName = cMain.DataShowTitleStr.Split(',');
            DataSet tmpDs;
            string sqlCommand = "";
            //将机型上下限数据写入到表格2中
            if (flushDataGridView == FlushDataGridView.FlushStepData
                || flushDataGridView == FlushDataGridView.FlushAll)
            {
                sqlCommand = string.Format("Select * from initPara where ModeId='{0}' order by StepId", cbbId.Text);
                tmpDs = cData.readData(sqlCommand, cData.ConnMain);
                dataGridView2.Rows.Clear();
                dataGridView2.Columns.Clear();

                for (int i = 0; i < cMain.DataShow; i++)
                {
                    dataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
                    dataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dataGridViewTextBoxColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridViewTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridViewTextBoxColumn.HeaderText = tmpName[i];
                    dataGridViewTextBoxColumn.ValueType = typeof(double);
                    dataGridView2.Columns.Add(dataGridViewTextBoxColumn);
                }
                dataGridView2.RowHeadersWidth = 150;
                if (tmpDs.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < tmpDs.Tables[0].Rows.Count; i++)
                    {
                        dataGridView2.Rows.Add();
                        dataGridView2.Rows[i * 2].HeaderCell.Value = string.Format("第{0}步数据下限", i + 1);
                        dataGridView2.Rows[i * 2].DefaultCellStyle.BackColor = Color.Pink;

                        dataGridView2.Rows.Add();
                        dataGridView2.Rows[i * 2 + 1].HeaderCell.Value = string.Format("第{0}步数据上限", i + 1);
                        dataGridView2.Rows[i * 2 + 1].DefaultCellStyle.BackColor = Color.LightCyan;

                        for (int j = 0; j < cMain.DataShow; j++)
                        {
                            DataRow dr = tmpDs.Tables[0].Rows[i];
                            dataGridView2.Rows[i * 2].Cells[j].Value = dr[string.Format("Data{0}", j * 2 + 1)];
                            dataGridView2.Rows[i * 2 + 1].Cells[j].Value = dr[string.Format("Data{0}", j * 2 + 2)];
                        }
                    }
                }
                else
                {
                    MessageBox.Show(string.Format("当前没有{0}机型参数设置,请先设置{0}机型参数后,再计算上下限", cbbId.Text), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            //取最后20条数据
            if (flushDataGridView == FlushDataGridView.FlushSaveData
                || flushDataGridView == FlushDataGridView.FlushAll)
            {
                sqlCommand = string.Format("Select top 20 * from alldata where Id='{0}' and StepID={1} order by TestTime DESC", cbbId.Text, Num.IntParse(cbbStepId.Text) - 1);
                tmpDs = cData.readData(sqlCommand, cData.ConnData);
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();
                if (tmpDs.Tables[0].Rows.Count > 0)
                {
                    txtStepName.Text = tmpDs.Tables[0].Rows[0]["Step"].ToString();
                    txtMode.Text = tmpDs.Tables[0].Rows[0]["Mode"].ToString();
                    dataGridView1.RowHeadersWidth = 100;

                    dataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
                    dataGridViewCheckBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dataGridViewCheckBoxColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridViewCheckBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridViewCheckBoxColumn.HeaderText = "是否选用当前行";

                    dataGridViewCheckBoxColumn.ValueType = typeof(bool);
                    dataGridView1.Columns.Add(dataGridViewCheckBoxColumn);

                    for (int i = 0; i < cMain.DataShow; i++)
                    {
                        dataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
                        dataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        dataGridViewTextBoxColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridViewTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridViewTextBoxColumn.HeaderText = tmpName[i];
                        dataGridViewTextBoxColumn.DefaultCellStyle.Format = "0.000";
                        //dataGridViewTextBoxColumn.ReadOnly = true;
                        dataGridViewTextBoxColumn.ValueType = typeof(double);
                        dataGridView1.Columns.Add(dataGridViewTextBoxColumn);
                    }
                    dataGridView1.Rows.Add(2);
                    dataGridView1.Rows[0].HeaderCell.Value = "下限";
                    dataGridView1.Rows[1].HeaderCell.Value = "上限";
                    dataGridView1.Rows[0].Cells[0].ReadOnly = true;
                    dataGridView1.Rows[1].Cells[0].ReadOnly = true;
                    dataGridView1.Rows[1].DefaultCellStyle.BackColor = Color.LightCyan;
                    for (int i = 0; i < tmpDs.Tables[0].Rows.Count; i++)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[i + 2].HeaderCell.Value = string.Format("{0}", i + 1);
                        for (int j = 0; j < cMain.DataShow; j++)
                        {
                            dataGridView1.Rows[i + 2].Cells[1 + j].Value = tmpDs.Tables[0].Rows[i][string.Format("d{0}", j)];
                        }
                        if ((i % 2) == 1)
                        {
                            dataGridView1.Rows[i + 2].DefaultCellStyle.BackColor = Color.LightCyan;
                        }
                    }
                }
            }
            timer1.Enabled = true;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            int SumRows = 0;
            for (int i = 0; i < cMain.DataShow; i++)
            {
                MaxValue[i] = -65535;//初始化一个默认值
                MinValue[i] = 65535;
                SumValue[i] = 0;
            }
            for (int i = 2; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[0].Value != null && Num.BoolParse(dataGridView1.Rows[i].Cells[0].Value))
                {
                    for (int j = 0; j < cMain.DataShow; j++)
                    {
                        double tmpValue = Num.DoubleParse(dataGridView1.Rows[i].Cells[1 + j].Value);
                        SumValue[j] = SumValue[j] + tmpValue;
                        MaxValue[j] = Num.DoubleMax(MaxValue[j], tmpValue);
                        MinValue[j] = Num.DoubleMin(MinValue[j], tmpValue);
                    }
                    SumRows++;
                }
            }
            if (SumRows >= 3)//最少要3行,(最高值,最低值,中间值)最少3行
            {
                for (int i = 0; i < cMain.DataShow; i++)
                {
                    double tmpValue = Num.DoubleParse(txtData.Text);
                    SumValue[i] = SumValue[i] - MaxValue[i] - MinValue[i];
                    if (chkData[i].Checked)
                    {
                        dataGridView1.Rows[0].Cells[i + 1].Value = (1 - tmpValue * 0.010) * SumValue[i] / (SumRows - 2.000);
                        dataGridView1.Rows[1].Cells[i + 1].Value = (1 + tmpValue * 0.010) * SumValue[i] / (SumRows - 2.000);
                    }
                    else
                    {
                        dataGridView1.Rows[0].Cells[i + 1].Value = 0;
                        dataGridView1.Rows[1].Cells[i + 1].Value = 0;
                    }
                }
            }
            else
            {
                if (dataGridView1.Rows.Count >= 2)
                {
                    for (int i = 0; i < cMain.DataShow; i++)
                    {
                        dataGridView1.Rows[0].Cells[i + 1].Value = 0;
                        dataGridView1.Rows[1].Cells[i + 1].Value = 0;
                    }
                }
            }
        }

        private void dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            dataGridView1.EndEdit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool isDelFirst = false;//是否要先删除原有数据
            try
            {
                double[] SaveValue = new double[cMain.DataShow * 2];
                for (int i = 0; i < cMain.DataShow; i++)
                {
                    SaveValue[i * 2] = Num.DoubleParse(dataGridView1.Rows[0].Cells[i + 1].Value);
                    SaveValue[i * 2 + 1] = Num.DoubleParse(dataGridView1.Rows[1].Cells[i + 1].Value);
                }

                DataSet ds = cData.readData(string.Format("select * from initPara where ModeId='{0}' and StepId={1}", cbbId.Text, Num.IntParse(cbbStepId.Text) - 1), cData.ConnMain);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < SaveValue.Length; i++)
                    {
                        ds.Tables[0].Rows[0][string.Format("Data{0}", i + 1)] = SaveValue[i];
                    }
                    isDelFirst = true;
                }
                else
                {
                    isDelFirst=false;
                }
                string sqlCommand = "Insert into initPara Values('";
                sqlCommand = sqlCommand + ds.Tables[0].Rows[0]["ModeID"].ToString() + "'";
                sqlCommand = sqlCommand + "," + ds.Tables[0].Rows[0]["StepID"].ToString();
                sqlCommand = sqlCommand + ",'" + ds.Tables[0].Rows[0]["RunMode"].ToString() + "'";
                sqlCommand = sqlCommand + "," + ds.Tables[0].Rows[0]["Times"].ToString();
                sqlCommand = sqlCommand + ",'" + ds.Tables[0].Rows[0]["Code"].ToString() + "'";
                for (int i = 0; i < cMain.DataKaiGuang; i++)
                {
                    sqlCommand = sqlCommand + "," + ds.Tables[0].Rows[0][string.Format("KaiGuan{0}", i + 1)].ToString();
                }
                for (int i = 0; i < 40; i++)
                {
                    sqlCommand = sqlCommand + "," + ds.Tables[0].Rows[0][string.Format("Data{0}", i + 1)].ToString();
                }
                sqlCommand = sqlCommand + ")";
                if (isDelFirst)
                {
                    cData.upData(string.Format("delete from initPara where ModeId='{0}' and StepId={1}", cbbId.Text, Num.IntParse(cbbStepId.Text) - 1), cData.ConnMain);
                }
                cData.upData(sqlCommand, cData.ConnMain);

                sqlCommand = "Insert into initParaSave values('";
                sqlCommand = sqlCommand + ds.Tables[0].Rows[0]["ModeID"].ToString() + "'";
                sqlCommand = sqlCommand + "," + ds.Tables[0].Rows[0]["StepID"].ToString();
                sqlCommand = sqlCommand + ",#" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "#";
                sqlCommand = sqlCommand + ",'" + txtMode.Text + "'";
                sqlCommand = sqlCommand + ",'" + ds.Tables[0].Rows[0]["RunMode"].ToString() + "'";
                for (int i = 0; i < 40; i++)
                {
                    sqlCommand = sqlCommand + "," + ds.Tables[0].Rows[0][string.Format("Data{0}", i + 1)].ToString();
                }
                sqlCommand = sqlCommand + ")";
                cData.upData(sqlCommand, cData.ConnMain);

                MessageBox.Show("数据保存成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData(FlushDataGridView.FlushStepData);
            }
            catch (Exception exc)
            {
                cMain.WriteErrorToLog("frmAutoUpAndDown btnSave_Click is Error:" + exc.ToString());
                MessageBox.Show("数据保存失败,请检查后重试", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cNetModeSet netModeSet = new cNetModeSet();
            netModeSet.mBar = "";
            netModeSet.isStart = false;
            //string errorStr = "";
            //if (cData.GetSetFromId(cbbId.Text, ref netModeSet.ModeSet, out errorStr))
            //{
            //    string SendStr = cSendData.DataNetSetToFile(netModeSet);
            //    frmSend f = new frmSend(SendStr, frmSend.SendValue.SendMode);
            //    f.Show();
            //}
            //else
            //{
            //    MessageBox.Show(errorStr);
            //}
        }
    }
}