using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Text.RegularExpressions;
namespace PcProgram
{
    public partial class frmSys : Form
    {
        public frmSys()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void initGrid1()
        {
            //对窗体与表格进行动态初始化

            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1;
            dataGridViewCellStyle1 = new DataGridViewCellStyle();
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;

            Grid1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;


            DataTable tmpDt = new DataTable();
            tmpDt.Columns.Add("txt1", typeof(string));
            tmpDt.Columns.Add("data1", typeof(string));
            tmpDt.Columns.Add("txt2", typeof(string));
            tmpDt.Columns.Add("data2", typeof(string));

            string[] tempStr = cMain.XiTongStr.Split(',');
            for (int i = 0; i < 10; i++)
            {
                DataRow tmpDr = tmpDt.NewRow();
                tmpDr["txt1"] = tempStr[i];
                tmpDr["txt2"] = tempStr[i + 10];
                tmpDt.Rows.Add(tmpDr);
            }
            Grid1.DataSource = tmpDt;
            Grid1.Columns[0].HeaderText = "数据名称";
            Grid1.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Grid1.Columns[1].HeaderText = "数据值";
            Grid1.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Grid1.Columns[2].HeaderText = "数据名称";
            Grid1.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Grid1.Columns[3].HeaderText = "数据值";
            Grid1.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            for (int i = 0; i < Grid1.Columns.Count; i++)
            {
                Grid1.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            DataGridViewComboBoxCell DgCbb = new DataGridViewComboBoxCell();
            DgCbb.Items.Add("全过程收氟保护");
            DgCbb.Items.Add("收氟过程保护");
            DgCbb.Items.Add("不保护");
            Grid1.Rows[2].Cells[1] = DgCbb;
            DataGridViewComboBoxCell DgCbb1 = new DataGridViewComboBoxCell();
            DgCbb1.Items.Add("报警");
            DgCbb1.Items.Add("停机");
            Grid1.Rows[3].Cells[1] = DgCbb1;
        }
        private void frmSys_Load(object sender, EventArgs e)
        {
            initGrid1();
            DataSet ds = cData.readData("select * from SysSet where id=1", cData.ConnMain);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = (DataTable)Grid1.DataSource;
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        dt.Rows[j][i * 2 + 1] = ds.Tables[0].Rows[0][j + i * 10 + 1].ToString();
                    }
                }
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                cData.upData("delete from sysset where id=1", cData.ConnMain);
                string updataStr = "insert into sysset values(1,'{0}','{1}','{2}','{3}',{4},{5},{6},{7},'{8}','{9}'," +
                                        "'{10}','{11}',{12},{13},{14},{15},{16},{17},{18},{19})";
                DataTable tmpDt = (DataTable)Grid1.DataSource;
                updataStr = string.Format(updataStr, tmpDt.Rows[0][1],
                    tmpDt.Rows[1][1], tmpDt.Rows[2][1],
                    tmpDt.Rows[3][1], tmpDt.Rows[4][1],
                    tmpDt.Rows[5][1], tmpDt.Rows[6][1],
                    tmpDt.Rows[7][1], tmpDt.Rows[8][1],
                    tmpDt.Rows[9][1], tmpDt.Rows[0][3],
                    tmpDt.Rows[1][3], tmpDt.Rows[2][3],
                    tmpDt.Rows[3][3], tmpDt.Rows[4][3],
                    tmpDt.Rows[5][3], tmpDt.Rows[6][3],
                    tmpDt.Rows[7][3], tmpDt.Rows[8][3],
                    tmpDt.Rows[9][3]);
                if (cData.upData(updataStr, cData.ConnMain) > 0)
                {
                    MessageBox.Show("数据保存成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            catch (Exception exc)
            {
                cMain.WriteErrorToLog("FrmSys btnSave_click is error " + exc.ToString());
            }
            MessageBox.Show("数据保存失败,确认每个空都填写,备用处写0", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void btnDown_Click(object sender, EventArgs e)
        {
            string strSend = "S~";
            DataTable tmpDT=(DataTable)Grid1.DataSource;
            for (short i = 0; i < 2; i++)
            {
                for (short j = 0; j < 10; j++)
                {
                    strSend = strSend + tmpDT.Rows[j][i * 2 + 1] + "~";
                }
            }
            strSend = strSend.Replace("全过程收氟保护", "0");
            strSend = strSend.Replace("收氟过程保护", "1");
            strSend = strSend.Replace("不保护", "2");
            strSend = strSend.Replace("报警", "0");
            strSend = strSend.Replace("停机", "1");
            frmSend f = new frmSend(strSend, frmSend.SendValue.SendSystem);
            f.ShowDialog();
        }
    }
}