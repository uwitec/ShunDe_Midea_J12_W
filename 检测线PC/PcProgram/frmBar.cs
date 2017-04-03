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
    public partial class frmBar : Form
    {
        OleDbDataAdapter da;
        OleDbCommandBuilder odb;
        DataTable dt = new DataTable();
        public frmBar()
        {
            InitializeComponent();
        }
        enum saveError
        {
            sameLength,
            isStr,
            errorSet,
            none
        }
        private void frmBar_Load(object sender, EventArgs e)
        {
            init();
        }
        private void init()
        {
            dt.Rows.Clear();
            da = new OleDbDataAdapter("select * from barCodeSet",cData.ConnMain);
            odb = new OleDbCommandBuilder(da);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.Columns[0].HeaderText = "是否启用";
            dataGridView1.Columns[1].HeaderText = "条码长度";
            dataGridView1.Columns[2].HeaderText = "机型码起始位";
            dataGridView1.Columns[3].HeaderText = "机型码长度";
            dataGridView1.Columns[4].HeaderText = "描述";
            dataGridView1.Columns[0].Width = 80;
            dataGridView1.Columns[1].Width = 90;
            dataGridView1.Columns[2].Width = 100;
            dataGridView1.Columns[3].Width = 90;
            dataGridView1.Columns[4].Width = 130;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            saveError SaveError = checkBarlen();
            switch(SaveError)
            {
                case saveError.sameLength:
                    MessageBox.Show("条码长度不能相同,否则无法识别", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case saveError.isStr:
                    MessageBox.Show("条码长度中有字母,无法识别", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case saveError.errorSet:
                    MessageBox.Show("条码设置中有不合理数据", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
            if (SaveError != saveError.none)
            {
                return;
            }
            da.AcceptChangesDuringUpdate = false;
            if (da.Update((DataTable)dataGridView1.DataSource) > 0)
            {
                MessageBox.Show("条码设置保存成功", "成功");
            }
        }
        private saveError checkBarlen()
        {
            string tempStr = ",";
            saveError isOk = saveError.none;
            Regex reg=new Regex("^[0-9]*$");
            for (int i = 0; i < dataGridView1.Rows.Count-1; i++)
            {
                string str1 = dataGridView1[1, i].Value.ToString();
                string str2 = dataGridView1[2, i].Value.ToString();
                string str3 = dataGridView1[3, i].Value.ToString();
                if (!reg.IsMatch(str1) || !reg.IsMatch(str2) || !reg.IsMatch(str3))//字符串
                {
                    isOk = saveError.isStr;
                    break;
                }
                int i1, i2, i3;
                i1 = int.Parse(str1);
                i2 = int.Parse(str2);
                i3 = int.Parse(str3);
                if (i1 < (i2 + i3 -1))
                {
                    isOk = saveError.errorSet;
                    break;
                }
                if (tempStr.IndexOf("," + str1 + ",") > 0)
                {
                    isOk = saveError.sameLength;
                    break;
                }
                else
                {
                    tempStr = tempStr + str1 + ",";
                }

            }
            return isOk;
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedCells[dataGridView1.SelectedCells.Count - 1].RowIndex == dataGridView1.Rows.Count - 1)
                {
                    MessageBox.Show("当前行没有数据，不能删除", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                DataGridViewRow dr = new DataGridViewRow();
                dr = dataGridView1.Rows[dataGridView1.SelectedCells[dataGridView1.SelectedCells.Count - 1].RowIndex];
                dataGridView1.Rows.Remove(dr);
                btnSave_Click(sender, e);
            }
            catch (Exception exc)
            {
                cMain.WriteErrorToLog("frmBar btnDel_Click is Error " + exc.ToString());
            }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            DataGridViewCell dc = dataGridView1[e.ColumnIndex, e.RowIndex];
            string s=dc.EditedFormattedValue.ToString();
            MessageBox.Show(s + "不是数字,请重新输入", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}