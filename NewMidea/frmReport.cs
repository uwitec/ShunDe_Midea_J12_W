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
    public partial class frmReport : Form
    {
        string dataPath = "\\HardDisk\\Data\\";
        bool isInit = false;//是否初始化
        /// <summary>
        /// 保存的文件信息
        /// </summary>
        struct SaveDataInfo
        {
            /// <summary>
            /// 保存的文件名
            /// </summary>
            public string fileName;
            /// <summary>
            /// 保存的文件时间
            /// </summary>
            public int fileYear;
            /// <summary>
            /// 保存的文件时间
            /// </summary>
            public int fileMonth;
            /// <summary>
            /// 保存的文件时间
            /// </summary>
            public int fileDay;
            /// <summary>
            /// 保存的条码
            /// </summary>
            public string fileBar;
        }
        SaveDataInfo[] mSaveDataInfo;
        int index = 0;
        public frmReport()
        {
            InitializeComponent();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void frmReport_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now;
            if (!cMain.isComPuter)
            {
                this.Height = Screen.PrimaryScreen.Bounds.Height;
                this.Width = Screen.PrimaryScreen.Bounds.Width;
            }
            cMain.initFrom(this.Controls);
            cMain.initFrom(panTop.Controls);
            cMain.initFrom(panMid.Controls);
            cMain.initFrom(panBot.Controls);
            initListView();
            initGridView("");
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            initListView();
        }
        private void initGridView(string fileLook)
        {
            DataTable dt =(DataTable) gridView.DataSource;
            if ((fileLook == "") || (!File.Exists(dataPath + fileLook)))
            {
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        dt.Rows[i][j] = "";
                    }
                }
            }
            else
            {
                string[] temp = cMain.ReadFile(dataPath + fileLook).Split('~');
                for (int i = 0; i < 10; i++)
                {
                    if (5 + 32 * i <= temp.Length)
                    {
                        dt.Rows[i][0] = temp[3 + 32 * i];
                        dt.Rows[i][1] = (temp[31 + 32 * i] == "1") ? "合格" : "不合格";
                        for (int j = 0; j < cMain.DataShow; j++)
                        {
                            dt.Rows[i][2 + j] = temp[9 + 32 * i + j * 2];
                        }
                    }
                    else
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            dt.Rows[i][j] = "";
                        }
                    }
                }
            }

        }
        private void initListView()
        {
            listView1.Items.Clear();
            if (!isInit)
            {
                try
                {
                    isInit = true;
                    index = 0;
                    DirectoryInfo di = new DirectoryInfo(dataPath);
                    mSaveDataInfo = new SaveDataInfo[di.GetFiles().Length];
                    string[] tempStr;
                    foreach (FileInfo fi in di.GetFiles())
                    {
                        tempStr = fi.Name.Split('~');
                        mSaveDataInfo[index].fileName = fi.Name;
                        mSaveDataInfo[index].fileYear = Num.IntParse(tempStr[0]);
                        mSaveDataInfo[index].fileMonth = Num.IntParse(tempStr[1]);
                        mSaveDataInfo[index].fileDay = Num.IntParse(tempStr[2]);
                        mSaveDataInfo[index].fileBar=tempStr[5];
                        index++;
                    }
                }
                catch (Exception exc)
                {
                    cMain.WriteErrorToLog("数据查看初始化失败" + exc.Message);
                }
            }
            int timeYear, timeMonth, timeDay;
            string timeBar;
            timeYear = dateTimePicker1.Value.Year;
            timeMonth = dateTimePicker1.Value.Month;
            timeDay = dateTimePicker1.Value.Day;
            timeBar=txtBar.Text;
            for (int i = 0; i < index; i++)
            {
                if ((((timeYear == mSaveDataInfo[i].fileYear) &&
                    (timeMonth == mSaveDataInfo[i].fileMonth) &&
                    (timeDay == mSaveDataInfo[i].fileDay)) && rbtTime.Checked) ||
                    (rbtBar.Checked && timeBar==mSaveDataInfo[i].fileBar))
                {
                    string[] temp=mSaveDataInfo[i].fileName.Split('~');
                    string[] showStr = new string[3];
                    showStr[0] = string.Format("{0}-{1}-{2} {3}:{4}", temp[0], temp[1], temp[2], temp[3], temp[4]);
                    showStr[1] = temp[5];
                    showStr[2] = temp[6].Split('.')[0];
                    ListViewItem templistView = new ListViewItem(showStr);
                    listView1.Items.Add(templistView);
                }
            }       
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string filepath = "";
                string[] tempStr = listView1.FocusedItem.SubItems[0].Text.Split('-', ' ', ':');
                filepath = tempStr[0] + "~" + tempStr[1] + "~" + tempStr[2] + "~" + tempStr[3] + "~" + tempStr[4] + "~";
                filepath = filepath + listView1.FocusedItem.SubItems[1].Text + "~";
                filepath = filepath + listView1.FocusedItem.SubItems[2].Text;
                filepath = filepath + ".txt";
                initGridView(filepath);
            }
            catch
            { }
        }
    }
}