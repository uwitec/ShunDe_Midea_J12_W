using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Data.OleDb;
namespace PcProgram
{
    public partial class frmSaveDataReport : Form
    {
        public frmSaveDataReport()
        {
            InitializeComponent();
        }
       
        DataSet dsSearch;
        DataSet dsShow;
        DataTable dtSource;
        string SqlSearchStr = "";
        int currentPage, pageSize = 20, rowCount, pageCount, recNo;
        private void frmReport_Load(object sender, EventArgs e)
        {
            initCheckBox();
            initMode();
        }
        private void initMode()
        {
            cbbMode.Items.Clear();
            string sqlStr = "Select DISTINCT ModeId from InitParaSave ";
            DataSet ds = cData.readData(sqlStr, cData.ConnMain);
            int i, rowCount;
            rowCount = ds.Tables[0].Rows.Count;
            if (rowCount > 0)
            {
                for (i = 0; i < rowCount; i++)
                {
                    cbbMode.Items.Add(ds.Tables[0].Rows[i][0]);
                }
            }
        }
        #region 数据处理
        /// <summary>
        /// 得到期初数据
        /// </summary>
        private void getFillDateset()
        {
            try
            {
                //得到最大记录数
                rowCount = dtSource.Rows.Count;
                //共有多少页
                pageCount = (rowCount / pageSize);
                //取余数
                if ((rowCount % pageSize) > 0)
                {
                    pageCount++;
                }
                //默认第一页
                currentPage = 1;
                recNo = 0;
                LoadPage();
            }
            catch
            {
                DataTable dt = new DataTable();
                dataGridView1.DataSource = dt;
                lblDataCount.Text = "共0条记录/共0页记录/当前第0页";
        
            }
        }


        /// <summary>
        /// 判断是否数据已经加载
        /// </summary>
        /// <returns></returns>
        private bool CheckFillButton()
        {
            if (pageSize == 0) return false;
            else return true;
        }

        /// <summary>
        /// 取DataTable的数据
        /// </summary>
        private void LoadPage()
        {
            int startRec;
            int endRec;
            DataTable dtTemp;


            dtTemp = dtSource.Clone();
            if (currentPage == pageCount) endRec = rowCount;
            else endRec = pageSize * currentPage;
            startRec = recNo;
            for (int i = startRec; i < endRec; i++)
            {
                dtTemp.ImportRow(dtSource.Rows[i]);
                recNo++;
            }
            this.dataGridView1.DataSource = dtTemp;
            dataGridView1.AutoResizeColumns();//自动缩放列宽

            int tempIndex = dsSearch.Tables[0].Rows.Count - startRec;
            if (tempIndex > pageSize)
            {
                tempIndex = pageSize;
            }
            for (int i = 0; i < tempIndex; i++)
            {
                DataGridViewRow dr = dataGridView1.Rows[i]; 
                DataRow tempDr = dsSearch.Tables[0].Rows[startRec + i];
                //for (int j = 0; j < cMain.DataShow; j++)
                //{
                //    cell = dr.Cells[7 + j];
                //    if ((bool)tempDr["b" + j.ToString()])
                //    {
                //        cell.Style.BackColor = Color.White;
                //    }
                //    else
                //    {
                //        cell.Style.BackColor = Color.Red;
                //    }
                //}
                //cell = dr.Cells["是否合格"];
                //if (tempDr["isPass"].ToString().ToUpper() == "TRUE")
                //{
                //    cell.Style.BackColor = Color.White;
                //}
                //else
                //{
                //    cell.Style.BackColor = Color.Red;
                //}
            }
            lblDataCount.Text = string.Format("共{0}条记录/共{1}页记录/当前第{2}页", rowCount, pageCount, currentPage);
        }

        private void changepage(object sender, EventArgs e)
        {
            if (!CheckFillButton()) return;
            int myint = Convert.ToInt16((string)(sender as Button).Tag);
            switch (myint)
            {
                case 0:
                    currentPage = 1;
                    recNo = 0;
                    LoadPage();
                    break;
                case 1:
                    if (currentPage == pageCount)
                        recNo = pageSize * (currentPage - 2);
                    currentPage--;
                    recNo = pageSize * (currentPage - 1);
                    LoadPage();
                    break;
                case 2:
                    currentPage++;
                    if (currentPage > pageCount)
                    {
                        currentPage = pageCount;
                        if (recNo == rowCount)
                        {
                            return;
                        }
                        else
                            recNo = pageSize * (currentPage + 1);
                    }
                    LoadPage();
                    break;
                case 3:
                    if (!CheckFillButton()) return;
                    if (recNo == rowCount)
                    {
                        return;
                    }
                    currentPage = pageCount;
                    recNo = pageSize * (currentPage - 1);
                    LoadPage();
                    break;
            }
            if (recNo == rowCount)
            {
                btnLast.Enabled = false;
                btnNex.Enabled = false;
            }
            else
            {
                btnNex.Enabled = true;
                btnLast.Enabled = true;
            }
            if (currentPage <= 1)
            {
                btnFirst.Enabled = false;
                btnPre.Enabled = false;
            }
            else
            {
                btnFirst.Enabled = true;
                btnPre.Enabled = true;
            }
        }

        #endregion
        private void initCheckBox()
        {
            int index = 0;
            string[] tempStr;
            tempStr = cMain.DataShowTitleStr.Split(',');
            IEnumerator ie = panel4.Controls.GetEnumerator();
            while (ie.MoveNext())
            {
                CheckBox cb = (CheckBox)ie.Current;
                index = Num.IntParse(cb.Tag);
                if (index >= 0)
                {
                    //if (index < cMain.DataShow)
                    //{
                    //    string[] temp = tempStr[index].Split('(');
                    //    cb.Text = temp[0] + "不合格";
                    //}
                    //else
                    //{
                        cb.Visible = false;
                        cb.Checked = false;
                    //}
                }
            }
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string sqlStr = "select * from InitParaSave where";
            string NeedStr1 = "";
            string NeedStr2 = "";
            string OrderStr = " order by SaveTime,ModeID,StepId";
            if (rbbTime.Checked)
            {
                NeedStr1 = string.Format(" SaveTime between #{0} 00:00:01# and #{1} 23:59:59#", dateTimePicker1.Text, dateTimePicker2.Text);
            }
            if (rbbBar.Checked)
            {
                if (txtBar.Text.IndexOf("'") >= 0)
                {
                    MessageBox.Show("请不要输入非法字符", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                NeedStr1 = string.Format(" bar like '%{0}%'", txtBar.Text);
            }
            if (rbbMode.Checked)
            {
                NeedStr1 = string.Format(" ModeID like '%{0}%'", cbbMode.Text);
            }
            if (rbbNo.Checked)
            {
                NeedStr1 = string.Format(" testNo={0}", Num.IntParse(txtNo.Text));
            }
            IEnumerator ie = panel4.Controls.GetEnumerator();
            while (ie.MoveNext())
            {
                CheckBox cb = (CheckBox)ie.Current;
                int index = Num.IntParse(cb.Tag);
                if (index >= 0)
                {
                    if (cb.Checked)
                    {
                        NeedStr2 = NeedStr2 + " and b" + cb.Tag.ToString() + "=false";
                    }
                }
                else
                {
                    if (index == -2 && checkBox1.Checked)
                    {
                        NeedStr2 = NeedStr2 + " and isPass='true'";
                    }
                    if (index == -1 && checkBox2.Checked)
                    {
                        NeedStr2 = NeedStr2 + " and isPass='false'";
                    }
                }
            }
            sqlStr = sqlStr + NeedStr1 + NeedStr2+OrderStr;
            SqlSearchStr = sqlStr;
            dsSearch = null;
            dsShow = null;
            dsSearch = cData.readData(sqlStr, cData.ConnMain);
            dsShow = cData.readData(sqlStr, cData.ConnMain);
            initDataset();
            dtSource = dsShow.Tables[0];
            getFillDateset();
            btnFirst.Enabled = false;
            btnPre.Enabled = false;
            btnNex.Enabled = true;
            btnLast.Enabled = true;
        }
        private void initDataset()
        {
            dsShow.Tables[0].Columns["SaveTime"].ColumnName = "设置时间";
            dsShow.Tables[0].Columns["ModeId"].ColumnName = "机型ID";
            dsShow.Tables[0].Columns["stepId"].ColumnName = "步骤号";
            dsShow.Tables[0].Columns["RunMode"].ColumnName = "步骤名称";
            dsShow.Tables[0].Columns["Mode"].ColumnName = "机型";
            string[] tempStr = cMain.DataShowTitleStr.Split(',');
            for (int i = 0; i < cMain.DataShow; i++)
            {
                dsShow.Tables[0].Columns[string.Format("Data{0}", i * 2 + 1)].ColumnName = string.Format("{0}_Min", tempStr[i]);
                dsShow.Tables[0].Columns[string.Format("Data{0}", i * 2 + 2)].ColumnName = string.Format("{0}_Max", tempStr[i]);
            }
            for (int i = cMain.DataShow; i < 20; i++)
            {
                dsShow.Tables[0].Columns.Remove(string.Format("Data{0}", i * 2 + 1));
                dsShow.Tables[0].Columns.Remove(string.Format("Data{0}", i * 2 + 2));
            }
            dsShow.Tables[0].Columns["设置时间"].SetOrdinal(0);
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                IEnumerator ie = panel4.Controls.GetEnumerator();
                while (ie.MoveNext())
                {
                    CheckBox cb = (CheckBox)ie.Current;
                    int index = Num.IntParse(cb.Tag);
                    if (index > -2)
                    {
                        cb.Checked = false;
                    }
                }
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            if ((Num.IntParse(cb.Tag) > -2) && cb.Checked)
            {
                checkBox1.Checked = false;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            string sqlStr = "delete from Alldata where";
            string NeedStr1 = "";
            string NeedStr2 = "";
            if (rbbTime.Checked)
            {
                NeedStr1 = string.Format(" TestTime between #{0} 00:00:01# and #{1} 23:59:59#", dateTimePicker1.Text, dateTimePicker2.Text);
            }
            if (rbbBar.Checked)
            {
                if (txtBar.Text.IndexOf("'") >= 0)
                {
                    MessageBox.Show("请不要输入非法字符", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                NeedStr1 = string.Format(" bar like '%{0}%'", txtBar.Text);
            }
            if (rbbMode.Checked)
            {
                NeedStr1 = string.Format(" mode like '%{0}%'", cbbMode.Text);
            }
            if (rbbNo.Checked)
            {
                NeedStr1 = string.Format(" testNo={0}", Num.IntParse(txtNo.Text));
            }
            IEnumerator ie = panel4.Controls.GetEnumerator();
            while (ie.MoveNext())
            {
                CheckBox cb = (CheckBox)ie.Current;
                int index = Num.IntParse(cb.Tag);
                if (index >= 0)
                {
                    if (cb.Checked)
                    {
                        NeedStr2 = NeedStr2 + " and b" + cb.Tag.ToString() + "=false";
                    }
                }
                else
                {
                    if (index == -2 && checkBox1.Checked)
                    {
                        NeedStr2 = NeedStr2 + " and isPass='true'";
                    }
                    if (index == -1 && checkBox2.Checked)
                    {
                        NeedStr2 = NeedStr2 + " and isPass='false'";
                    }
                }
            }
            sqlStr = sqlStr + NeedStr1 + NeedStr2;
            int changeRow = cData.upData(sqlStr, cData.ConnData);
            if (changeRow > 0)
            {
                MessageBox.Show("删除成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("删除失败或没有数据可删除", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnOutToExcel_Click(object sender, EventArgs e)
        {
            string SaveFilename = "";
            if (SaveExcel.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            else 
            {
                SaveFilename = SaveExcel.FileName;
            }
            string errstr="";
            DataGridViewExportToExcel(dsShow.Tables[0], SaveFilename, ref errstr);
        }
        public static bool DataGridViewExportToExcel(DataTable mDataTable, String strFileName, ref String strMsg)
        {
            strMsg = "";
            // 创建Excel对象                  
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.ApplicationClass();
            if (xlApp == null)
            {
                strMsg = "Excel无法启动";
                return false;
            }
            // 创建Excel工作薄
            Microsoft.Office.Interop.Excel.Workbook xlBook = xlApp.Workbooks.Add(true);
            Microsoft.Office.Interop.Excel.Worksheet xlSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlBook.Worksheets[1];

            Microsoft.Office.Interop.Excel.Range range = null;
            /*
                        // 设置标题
                       range = xlSheet.get_Range(xlApp.Cells[1,1],xlApp.Cells[1,ts.GridColumnStyles.Count]);
                       range.MergeCells = true;
                       xlApp.ActiveCell.FormulaR1C1 = p_ReportName;
                       xlApp.ActiveCell.Font.Size = 20;
                       xlApp.ActiveCell.Font.Bold = true;
                       xlApp.ActiveCell.HorizontalAlignment = Excel.Constants.xlCenter;
            */

            // 列索引，行索引，总列数，总行数
            int colIndex = 0;
            int RowIndex = 0;
            int colCount = mDataTable.Columns.Count;
            int RowCount = mDataTable.Rows.Count + 1;
            
            // 创建缓存数据
            object[,] objData = new object[RowCount + 1, colCount];
            // 获取列标题
            foreach (DataColumn dc in mDataTable.Columns)
            {
                objData[RowIndex, colIndex++] = dc.ColumnName;
            }
            // 获取数据
            for (RowIndex = 1; RowIndex < RowCount; RowIndex++)
            {
                for (colIndex = 0; colIndex < colCount; colIndex++)
                {
                    objData[RowIndex, colIndex] = mDataTable.Rows[RowIndex - 1][colIndex].ToString();
                }
            }
            // 写入Excel
            //((Excel.Range)xlSheet.Columns["A:A ",System.Reflection.Missing.Value]).ColumnWidth = 16;
            range = xlSheet.get_Range(xlApp.Cells[1, 1], xlApp.Cells[RowCount, colCount]);
            range.Value2 = objData;
            range = (Microsoft.Office.Interop.Excel.Range)xlSheet.Columns["A:B", System.Type.Missing];
            range.ColumnWidth = 20;
            // 保存
            try
            {
                xlBook.Saved = true;
                xlBook.SaveCopyAs(strFileName);
                MessageBox.Show("数据转存为Excel成功","成功",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            catch (Exception ee)
            {
                strMsg = ee.Message;
                return false;
            }
            finally
            {
                range = null;
                xlSheet = null;
                xlBook = null;
                xlApp.Quit();
                xlApp = null;
                GC.Collect();
            }
            return true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}