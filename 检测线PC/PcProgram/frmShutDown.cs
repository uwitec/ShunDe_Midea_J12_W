using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PcProgram
{
    public partial class frmShutDown : Form
    {
        public frmShutDown()
        {
            InitializeComponent();
        }

        private void frmShutDown_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < cMain.AllCount; i++)
            {
                comboBox1.Items.Add(i + 1);
                comboBox2.Items.Add(i + 1);
            }
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = cMain.AllCount - 1;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            int start = Num.IntParse(comboBox1.Text);
            int end = Num.IntParse(comboBox2.Text);
            for (int i = start; i <= end; i++)
            {
                cMain.mUdp.McgsUdp[i-1].fUdpSend("E");
            }
        }
    }
}