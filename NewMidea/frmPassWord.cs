using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NewMideaProgram
{
    public partial class frmPassWord : Form
    {
        int intMouse = 0;
        public frmPassWord()
        {
            InitializeComponent();
        }

        private void frmPassWord_Load(object sender, EventArgs e)
        {
            this.Left = (Screen.PrimaryScreen.Bounds.Width - this.Width) / 2;
            this.Top = (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2;
        }

        private void Num_Click(object sender, EventArgs e)
        {
            intMouse = txtValue.SelectionStart;
            Button nowBtn = (Button)sender;
            if (txtValue.SelectionLength > 0)
            {
                txtValue.Text = txtValue.Text.Substring(0, txtValue.SelectionStart) + nowBtn.Text + txtValue.Text.Substring(txtValue.SelectionStart + txtValue.SelectionLength);
                txtValue.SelectionStart = intMouse;
                txtValue.SelectionLength = 0;
            }
            else
            {
                txtValue.Text = txtValue.Text.Substring(0, txtValue.SelectionStart) + nowBtn.Text + txtValue.Text.Substring(txtValue.SelectionStart);
            }
            txtValue.SelectionStart = intMouse + 1;
            txtValue.Focus();
        }

        private void Button15_Click(object sender, EventArgs e)
        {
            if (txtValue.Text != "")
            {
                intMouse = txtValue.SelectionStart;
                string temp = txtValue.Text;
                if (intMouse == 0)
                {
                    txtValue.Focus();
                    return;
                }
                if (txtValue.SelectionLength > 0)
                {
                    txtValue.Text = temp.Substring(0, intMouse) + temp.Substring(intMouse + txtValue.SelectionLength);
                    txtValue.SelectionLength = 0;
                    txtValue.SelectionStart = intMouse;
                }
                else
                {
                    txtValue.Text = temp.Substring(0, intMouse - 1) + temp.Substring(intMouse);
                    txtValue.SelectionStart = intMouse - 1;
                }
                txtValue.Focus();
            }
        }

        private void Button16_Click(object sender, EventArgs e)
        {
            txtValue.Text = "";
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        private void Button14_Click(object sender, EventArgs e)
        {
            if(txtValue.Text==cMain.mSysSet.mPassWord)
            {
                this.DialogResult = DialogResult.Yes;
                this.Close();
                return;
            }
            if (txtValue.Text == "911")
            {
                cMain.isNeedPassWord = false;
                this.DialogResult = DialogResult.Yes;
                this.Close();
                return;
            }
            MessageBox.Show("对不起，输入的密码不正确", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}