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
    public partial class frmHelp : Form
    {
        public frmHelp()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 因为主frmMdi页面添加此页面过去时不能自动调用本页的Load(object sender,EventArgs e)事件，所以要添加此全局函数供frmMdi手动调用
        /// </summary>
        public void StartLoad()
        {
            if (File.Exists(cMain.AppPath + "\\help\\help.doc"))
            {
                richTextBox1.LoadFile(cMain.AppPath + "\\help\\help.rtf");
            }
        }
        
    }
}