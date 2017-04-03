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
    public partial class frmList : Form
    {
        public string ReturnId = "";
        public bool isError = false;
        public frmList()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
            try
            {
                ReturnId = cMain.AppPath + "\\ID\\" + listView1.FocusedItem.SubItems[0].Text + ".txt";
                isError = false;
            }
            catch
            {
                isError = true;
            }
            this.Close();
        }

        private void frmList_Load(object sender, EventArgs e)
        {
            frmInit();
            AddItem();
        }
        private void frmInit()
        {
            if (!cMain.isComPuter)
            {
                this.Height = Screen.PrimaryScreen.Bounds.Height;
                this.Width = Screen.PrimaryScreen.Bounds.Width;
            }
            cMain.initFrom(this.Controls);
        }
        public static void GetXml(out string[] fileName,out string[] mode)
        {
            List<string> _fileName = new List<string>();
            List<string> _mode = new List<string>();
            DirectoryInfo di = new DirectoryInfo(cMain.AppPath + "\\ID\\");
            foreach (FileInfo fi in di.GetFiles("*.txt"))
            {
                try
                {
                    string[] tempStr;
                    tempStr = cMain.ReadFile(fi.FullName).Split('~');
                    _fileName.Add(fi.Name.Substring(0, fi.Name.IndexOf(".")));
                    _mode.Add(tempStr[1]);//机型
                }
                catch (Exception exc)
                {
                    cMain.WriteErrorToLog("FrmList GetXml is Error " + exc.ToString());
                }
            }
            fileName = new string[_fileName.Count];
            mode = new string[_mode.Count];
            for (int i = 0; i < fileName.Length; i++)
            {
                fileName[i] = _fileName[i];
            }
            for (int i = 0; i < mode.Length; i++)
            {
                mode[i] = _mode[i];
            }
 
        }
        private void AddItem()
        {
            string[] fileName, mode;
            listView1.Items.Clear();
            GetXml(out fileName, out mode);
            for (int i = 0; i < fileName.Length; i++)
            {
                string[] ShowText = new string[2];
                ShowText[0] = fileName[i];
                ShowText[1] = mode[i];
                ListViewItem li = new ListViewItem(ShowText);
                listView1.Items.Add(li);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView1.FocusedItem.SubItems[0].Text == "DEF")
                {
                    MessageBox.Show("Can't Be Del,默认机型不能删除".Split(',')[cMain.IndexLanguage], "Error,错误".Split(',')[cMain.IndexLanguage], MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                } 
                DialogResult dr = MessageBox.Show("请确认是否要删除 【" + listView1.FocusedItem.SubItems[0].Text + "】 该机型", "Yes,确认".Split(',')[cMain.IndexLanguage], MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                if (dr == DialogResult.Yes)
                {
                    File.Delete(cMain.AppPath + "\\ID\\" + listView1.FocusedItem.SubItems[0].Text + ".txt");
                    AddItem();
                }
            }
            catch(Exception exc)
            {
                cMain.WriteErrorToLog("btnDelete_Click is Error " + exc.ToString());
            }
            
        }
    }
}