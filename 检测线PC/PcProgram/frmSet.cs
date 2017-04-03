using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Text.RegularExpressions;
namespace PcProgram
{
    public partial class frmSet : Form
    {
        public frmSet()
        {
            InitializeComponent();
        }
        private void btnId_Click(object sender, EventArgs e)
        {
            frmList fl = new frmList();
            if (fl.ShowDialog() == DialogResult.Yes)
            {
                ClassToFrm(cModeSet.Read(fl.ReturnId));
            }
        }
        private cModeSet FrmToClass()
        {
            cModeSet result = new cModeSet();
            result.mId = txtID.Text;
            result.mMode = txtMode.Text;
            result.mDescript = txtDescribe.Text;
            result.mElect = cbbVol.SelectedIndex;
            result.mBiaoZhunJi = cbbBzz.SelectedIndex;
            result.m24V = cbb24V.SelectedIndex;
            result.mJiQi = cbbJiQi.SelectedIndex;

            for (int i = 0; i < cMain.DataProtect / 2; i++)
            {
                result.mProtect[i] = Num.SingleParse(Grid1.Rows[i].Cells[1].Value);
                result.mProtect[cMain.DataProtect / 2 + i] = Num.SingleParse(Grid1.Rows[i].Cells[3].Value);
            }

            for (int i = 0; i < result.mStepId.Length && i < Grid2.Rows.Count; i++)
            {
                result.mStepId[i] = Grid2.Rows[i].Cells["RunMode"].Value.ToString();
                result.mSetTime[i] = Num.IntParse(Grid2.Rows[i].Cells["Times"].Value);
                result.mSendStr[i] = Grid2.Rows[i].Cells["Code"].Value.ToString();
                for (int j = 0; j < cMain.DataKaiGuang; j++)
                {
                    result.mKaiGuan[i, j] = Num.BoolParse(Grid2.Rows[i].Cells[string.Format("KaiGuan{0}", j + 1)].Value);
                }
                for (int j = 0; j < cMain.DataShow; j++)
                {
                    result.mLowData[i, j] = Num.SingleParse(Grid2.Rows[i].Cells[string.Format("Data{0}", j * 2 + 1)].Value);
                    result.mHighData[i, j] = Num.SingleParse(Grid2.Rows[i].Cells[string.Format("Data{0}", j * 2 + 2)].Value);
                }
            }
            for (int i = 0; i < cMain.DataShow; i++)
            {
                result.mShow[i] = Num.BoolParse(Grid3.Rows[0].Cells[i].Value);
            }

            return result;
        }
        private void ClassToFrm(cModeSet modeSet)
        {
            try
            {
                txtID.Text = modeSet.mId;
                txtMode.Text = modeSet.mMode;
                txtDescribe.Text = modeSet.mDescript;
                cbbVol.SelectedIndex = modeSet.mElect;
                cbbBzz.SelectedIndex = modeSet.mBiaoZhunJi;
                cbb24V.SelectedIndex = modeSet.m24V;
                cbbJiQi.SelectedIndex = modeSet.mJiQi;
                //保护
                for (int i = 0; i < cMain.DataProtect; i++)
                {
                    if (Grid1.Rows[i % (cMain.DataProtect / 2)].Cells[(int)Math.Floor((float)i*2 / cMain.DataProtect) * 2].Value.ToString().Length > 0)
                    {
                        Grid1.Rows[i % (cMain.DataProtect / 2)].Cells[(int)Math.Floor((float)i*2 / cMain.DataProtect) * 2 + 1].Value = modeSet.mProtect[i];
                    }
                }
                //步骤
                for (int i = 0; i < Grid2.Rows.Count && i < modeSet.mStepId.Length; i++)
                {
                    Grid2.Rows[i].Cells["RunMode"].Value = modeSet.mStepId[i];
                    Grid2.Rows[i].Cells["Times"].Value = modeSet.mSetTime[i];
                    Grid2.Rows[i].Cells["Code"].Value = modeSet.mSendStr[i];
                    for (int j = 0; j < cMain.DataKaiGuang; j++)
                    {
                        Grid2.Rows[i].Cells[string.Format("KaiGuan{0}", j + 1)].Value = modeSet.mKaiGuan[i, j];
                    }
                    for (int j = 0; j < cMain.DataShow; j++)
                    {
                        Grid2.Rows[i].Cells[string.Format("Data{0}", j * 2 + 1)].Value = modeSet.mLowData[i, j];
                        Grid2.Rows[i].Cells[string.Format("Data{0}", j * 2 + 2)].Value = modeSet.mHighData[i, j];
                    }
                }
                //显示
                for (int i = 0; i < cMain.DataShow; i++)
                {
                    Grid3.Rows[0].Cells[i].Value = modeSet.mShow[i];
                }
            }
            catch (Exception e)
            {
                cMain.WriteErrorToLog(e.ToString());
            }
        }
        #region//表格初始化
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

            string[] tempStr;
            Grid1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            tempStr = cMain.BaoHuStr.Split(',');
            for (int i = 0; i < 2; i++)
            {
                DataGridViewTextBoxColumn tmpDC1 = new DataGridViewTextBoxColumn();
                tmpDC1.DataPropertyName = "txt" + string.Format("{0}", i + 1);
                tmpDC1.HeaderText = "设置名称";
                Grid1.Columns.AddRange(new DataGridViewColumn[] { tmpDC1 });
                DataGridViewTextBoxColumn tmpDC = new DataGridViewTextBoxColumn();
                tmpDC.DefaultCellStyle.Format = "#.###";
                tmpDC.DataPropertyName = "data" + string.Format("{0}", i + 1);
                tmpDC.HeaderText = "设置值";
                Grid1.Columns.AddRange(new DataGridViewColumn[] { tmpDC });
            }
            DataTable tmpDt = new DataTable();
            tmpDt.Columns.Add("txt1", typeof(string));
            tmpDt.Columns.Add("data1", typeof(double));
            tmpDt.Columns.Add("txt2", typeof(string));
            tmpDt.Columns.Add("data2", typeof(double));

            for (int i = 0; i < 10; i++)
            {
                DataRow tmpDr = tmpDt.NewRow();
                tmpDr["txt1"] = tempStr[i];
                tmpDr["txt2"] = tempStr[i + 10];
                tmpDt.Rows.Add(tmpDr);
            }
            Grid1.DataSource = tmpDt;

            for (int i = 0; i < Grid1.Columns.Count; i++)
            {
                Grid1.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            tempStr = cMain.BiaoZhunJiStr.Split(',');
            for (int i = 0; i < tempStr.Length; i++)
            {
                cbbBzz.Items.Add(tempStr[i]);
            }
            tempStr = cMain.DianYuanStr.Split(',');
            for (int i = 0; i < tempStr.Length; i++)
            {
                cbbVol.Items.Add(tempStr[i]);
            }
            tempStr = cMain.JiQiStr.Split(',');
            for (int i = 0; i < tempStr.Length; i++)
            {
                cbbJiQi.Items.Add(tempStr[i]);
            }
            tempStr = cMain.DianXinHao.Split(',');
            for (int i = 0; i < tempStr.Length; i++)
            {
                cbb24V.Items.Add(tempStr[i]);
            }
        }
        private void initGrid2()
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

            string[] tempStr;
            Grid2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            //DataTable tmpDt = new DataTable();
            DataGridViewComboBoxColumn Dgcbb = new DataGridViewComboBoxColumn();
            tempStr = cMain.BuZhouMingStr.Split(',');
            for (int i = 0; i < tempStr.Length; i++)
            {
                Dgcbb.Items.Add(tempStr[i]);
            }
            Dgcbb.Name = "RunMode";
            Dgcbb.HeaderText = "步骤名";
            Grid2.Columns.AddRange(new DataGridViewColumn[] { Dgcbb });

            DataGridViewTextBoxColumn DgTxtTimes = new DataGridViewTextBoxColumn();
            DgTxtTimes.Name = "Times";
            DgTxtTimes.HeaderText = "时间";
            Grid2.Columns.AddRange(new DataGridViewColumn[] { DgTxtTimes });
            DataGridViewTextBoxColumn DgTxtCodes = new DataGridViewTextBoxColumn();
            DgTxtCodes.Name = "Code";
            DgTxtCodes.HeaderText = "SN指令";
            Grid2.Columns.AddRange(new DataGridViewColumn[] { DgTxtCodes });
            tempStr = cMain.KaiGuangStr.Split(',');
            for (int i = 0; i < cMain.DataKaiGuang; i++)
            {
                DataGridViewCheckBoxColumn DgChk = new DataGridViewCheckBoxColumn();
                DgChk.Name = "KaiGuan" + string.Format("{0}", i + 1);
                if (i < tempStr.Length)
                {
                    DgChk.HeaderText = tempStr[i];
                }
                else
                {
                    DgChk.Visible = false;
                }
                Grid2.Columns.AddRange(new DataGridViewColumn[] { DgChk });
            }
            tempStr = cMain.DataShowTitleStr.Split(',');
            for (int i = 0; i < cMain.DataShow; i++)
            {
                DataGridViewTextBoxColumn DgTxtTDown = new DataGridViewTextBoxColumn();
                DgTxtTDown.Name = "data" + string.Format("{0}", i * 2 + 1);
                DgTxtTDown.HeaderText = tempStr[i] + "Min";
                DgTxtTDown.DefaultCellStyle.Format = "#.###";
                Grid2.Columns.AddRange(new DataGridViewColumn[] { DgTxtTDown });
                DataGridViewTextBoxColumn DgTxtTUp = new DataGridViewTextBoxColumn();
                DgTxtTUp.Name = "data" + string.Format("{0}", i * 2 + 2);
                DgTxtTUp.HeaderText = tempStr[i] + "Max";
                DgTxtTUp.DefaultCellStyle.Format = "#.###";
                Grid2.Columns.AddRange(new DataGridViewColumn[] { DgTxtTUp });
            }
            Grid2.Rows.Add(cModeSet.StepCount);
        }
        private void initGrid3()
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
            string[] tempStr = cMain.DataShowTitleStr.Split(',');
            Grid3.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            DataGridViewCheckBoxColumn dgchk;
            for (int i = 0; i < cMain.DataShow; i++)
            {
                dgchk = new DataGridViewCheckBoxColumn();
                dgchk.HeaderText = tempStr[i];
                Grid3.Columns.Add(dgchk);
            }
            Grid3.Rows.Add();
        }
        #endregion
        private void frmSet_Load(object sender, EventArgs e)
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            this.UpdateStyles();
            initGrid1();
            initGrid2();
            initGrid3();
            ClassToFrm(cModeSet.Read(cModeSet.LastID()));
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            Grid1.EndEdit();
            Grid2.EndEdit();
            Grid3.EndEdit();
            if (!FrmToClass().Save())
            {
                MessageBox.Show("数据保存失败,请检测步骤设置数据格式与填写", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("数据保存成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            cNetModeSet netModeSet = new cNetModeSet();
            netModeSet.mBar = "";
            netModeSet.isStart = false;
            netModeSet.ModeSet = cModeSet.Read(txtID.Text);
            frmSend f = new frmSend(netModeSet.GetStr(), frmSend.SendValue.SendMode);
            f.Show();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (txtID.Text == "")
            {
                MessageBox.Show("当前要删除的机型ID为空,不能删除", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            cModeSet.Delete(txtID.Text);

            MessageBox.Show("删除机型" + txtID.Text + "成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClassToFrm(cModeSet.Read(cModeSet.LastID()));
        }

        private void DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("当前输入数据格式不正确,请重新输入", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void Grid2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                using (frmMideaSn fm = new frmMideaSn(Grid2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), txtID.Text, e.RowIndex, cbbJiQi.SelectedIndex))
                {
                    if (fm.ShowDialog() == DialogResult.Yes)
                    {
                        Grid2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = fm.Sn;
                    }
                }
            }
        }

        private void btnIC_Click(object sender, EventArgs e)
        {
            frmIC fic = new frmIC();
            fic.Show();
        }

        private void btnUpAndDown_Click(object sender, EventArgs e)
        {
            frmAutoUpAndDown fa = new frmAutoUpAndDown();
            fa.Show();
        }
    }
}