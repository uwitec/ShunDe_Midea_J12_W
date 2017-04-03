using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
namespace PcProgram
{
    public partial class frmIC : Form
    {
        public frmIC()
        {
            InitializeComponent();
        }
        int rowCount = 0;
        private void frmIC_Load(object sender, EventArgs e)
        {
            initFrm();
            initData();
            timer1.Enabled = true;
        }
        private void initData()
        {
            string[] ICCard;
            cData.GetICCard(out ICCard);
            int rowIndex = 0, colIndex = 0;
            for (int i = 0; i < cMain.AllCount; i++)
            {
                rowIndex = i % 10;
                colIndex = (int)Math.Floor(i / 10.0);
                ICView.Rows[rowIndex].Cells[colIndex * 2 + 1].Value = ICCard[i];
            }
        }
        private void initFrm()
        {
            int rowIndex = 0, colIndex = 0;
            cbbTestIndex.Items.Clear();
            for (int i = 0; i < cMain.AllCount; i++)
            {
                cbbTestIndex.Items.Add(string.Format("{0:D2}", i + 1));
            }
            cbbTestIndex.Text = "01";
            rowCount = (int)Math.Ceiling(cMain.AllCount / 10.000);
            DataGridViewTextBoxColumn dataGridViewTextBoxColumn;
            for (int i = 0; i < rowCount; i++)
            {
                dataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
                dataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridViewTextBoxColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewTextBoxColumn.DefaultCellStyle.BackColor = System.Drawing.Color.LightPink;
                dataGridViewTextBoxColumn.HeaderText = "小车号";
                dataGridViewTextBoxColumn.ValueType = typeof(string);
                dataGridViewTextBoxColumn.ReadOnly = true;
                ICView.Columns.Add(dataGridViewTextBoxColumn);

                dataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
                dataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridViewTextBoxColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewTextBoxColumn.HeaderText = "IC卡号";
                dataGridViewTextBoxColumn.ValueType = typeof(string);
                ICView.Columns.Add(dataGridViewTextBoxColumn);


            }
            ICView.Rows.Add(10);
            
            for (int i = 0; i < cMain.AllCount; i++)
            {
                rowIndex = i % 10;
                colIndex = (int)Math.Floor(i / 10.0);
                ICView.Rows[rowIndex].Cells[2 * colIndex].Value = string.Format("{0:D2}", i + 1);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblIC.Text = cMain.ReadNowICCard;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            int SelectCard = Num.IntParse(cbbTestIndex.Text)-1;
            int colIndex = (int)Math.Floor(SelectCard / 10.00);
            int rowIndex = SelectCard % 10;
            ICView.Rows[rowIndex].Cells[colIndex * 2 + 1].Value = lblIC.Text;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int colIndex = 0;
            int rowIndex = 0;
            if (cData.upData("delete * from CardSet", cData.ConnMain) <= 0)
            {
                MessageBox.Show("删除原始保存数据失败,请重新尝试", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                for (int i = 0; i < cMain.AllCount; i++)
                {
                    rowIndex = i % 10;
                    colIndex = (int)Math.Floor(i / 10.0);
                    string testNum = string.Format("{0:D2}", i + 1);
                    string testCard = ICView.Rows[rowIndex].Cells[colIndex * 2 + 1].Value.ToString();
                    cData.upData(string.Format("insert into CardSet Values('{0}','{1}')", testNum, testCard), cData.ConnMain);
                }
                MessageBox.Show("数据保存成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            catch(Exception exc)
            {
                cMain.WriteErrorToLog("frmIC btnSave_Click is Error:" + exc.Message);
                MessageBox.Show("数据保存失败", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}