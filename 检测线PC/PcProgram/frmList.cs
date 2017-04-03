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
    public partial class frmList : Form
    {
        public string ReturnId = "";
        public frmList()
        {
            InitializeComponent();
        }

        private void frmList_Load(object sender, EventArgs e)
        {
            listView1.HideSelection = true;
            initListBox("", "");
        }
        private void initListBox(string _id,string _mode)
        {
            listView1.Items.Clear();
            string[] ShowText=new string[3];
            DataSet ds = cData.readData("select id,mode,about from Mode where id like '%"+_id+"%' and mode like '%"+_mode+"%' order by mTime desc", cData.ConnMain);
            int dsCount = ds.Tables[0].Rows.Count;
            for(int i=0;i<dsCount;i++)
            {
                ShowText[0]=ds.Tables[0].Rows[i][0].ToString();
                ShowText[1]=ds.Tables[0].Rows[i][1].ToString();
                ShowText[2]=ds.Tables[0].Rows[i][2].ToString();
                ListViewItem lv = new ListViewItem(ShowText);
                listView1.Items.Add(lv);
            }
            if (listView1.Items.Count > 0)
            {
                btnOk.Enabled = true;
                try
                {
                    listView1.Items[0].Selected = true;
                    ReturnId = listView1.SelectedItems[0].SubItems[0].Text;
                    lblId.Text = ReturnId;

                }
                catch (Exception exc)
                {
                    cMain.WriteErrorToLog("frmList initListBox is error " + exc.ToString());
                }
            }
            else
            {
                lblId.Text = "";
                btnOk.Enabled = false;
            }
        }
        private void txtId_TextChanged(object sender, EventArgs e)
        {
            initListBox(txtId.Text, txtMode.Text);
        }

        private void listView1_Click(object sender, EventArgs e)
        {
            try
            {
                ReturnId = listView1.SelectedItems[0].SubItems[0].Text;
                lblId.Text = ReturnId;
            }
            catch (Exception exc)
            {
                cMain.WriteErrorToLog("frmList initListBox is error " + exc.ToString());
            }

        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (lblId.Text == "")
            {
                MessageBox.Show("还没正确选择机型,请重新选择或取消选择", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.DialogResult = DialogResult.Yes;
            this.Close();   
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }
    }
}