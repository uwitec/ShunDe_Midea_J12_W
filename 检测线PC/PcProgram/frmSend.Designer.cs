namespace PcProgram
{
    partial class frmSend
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSend));
            this.panTop = new System.Windows.Forms.Panel();
            this.gbSend = new System.Windows.Forms.GroupBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rbbUpdata = new System.Windows.Forms.RadioButton();
            this.cbbMode = new System.Windows.Forms.ComboBox();
            this.rbbMode = new System.Windows.Forms.RadioButton();
            this.rbbSys = new System.Windows.Forms.RadioButton();
            this.rbbStop = new System.Windows.Forms.RadioButton();
            this.rbbNext = new System.Windows.Forms.RadioButton();
            this.rbbStart = new System.Windows.Forms.RadioButton();
            this.rbbShutDown = new System.Windows.Forms.RadioButton();
            this.panTop.SuspendLayout();
            this.gbSend.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTop
            // 
            this.panTop.Controls.Add(this.gbSend);
            this.panTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTop.Location = new System.Drawing.Point(0, 36);
            this.panTop.Name = "panTop";
            this.panTop.Size = new System.Drawing.Size(820, 56);
            this.panTop.TabIndex = 1;
            // 
            // gbSend
            // 
            this.gbSend.Controls.Add(this.btnExit);
            this.gbSend.Controls.Add(this.btnStop);
            this.gbSend.Controls.Add(this.btnSend);
            this.gbSend.Controls.Add(this.comboBox2);
            this.gbSend.Controls.Add(this.label2);
            this.gbSend.Controls.Add(this.comboBox1);
            this.gbSend.Controls.Add(this.label1);
            this.gbSend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbSend.Location = new System.Drawing.Point(0, 0);
            this.gbSend.Name = "gbSend";
            this.gbSend.Size = new System.Drawing.Size(820, 56);
            this.gbSend.TabIndex = 0;
            this.gbSend.TabStop = false;
            // 
            // btnExit
            // 
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.Location = new System.Drawing.Point(582, 23);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 23);
            this.btnExit.TabIndex = 6;
            this.btnExit.Text = "退出(&E)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(495, 23);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(72, 23);
            this.btnStop.TabIndex = 5;
            this.btnStop.Text = "停止(&T)";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(403, 23);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(72, 23);
            this.btnSend.TabIndex = 4;
            this.btnSend.Text = "发送(&S)";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // comboBox2
            // 
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(242, 25);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(136, 20);
            this.comboBox2.TabIndex = 3;
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(207, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "到";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(45, 25);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(140, 20);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "从";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.listBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 92);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(820, 422);
            this.panel1.TabIndex = 2;
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(0, 0);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(820, 412);
            this.listBox1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.rbbShutDown);
            this.panel2.Controls.Add(this.rbbUpdata);
            this.panel2.Controls.Add(this.cbbMode);
            this.panel2.Controls.Add(this.rbbMode);
            this.panel2.Controls.Add(this.rbbSys);
            this.panel2.Controls.Add(this.rbbStop);
            this.panel2.Controls.Add(this.rbbNext);
            this.panel2.Controls.Add(this.rbbStart);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(820, 36);
            this.panel2.TabIndex = 0;
            // 
            // rbbUpdata
            // 
            this.rbbUpdata.AutoSize = true;
            this.rbbUpdata.Location = new System.Drawing.Point(219, 11);
            this.rbbUpdata.Name = "rbbUpdata";
            this.rbbUpdata.Size = new System.Drawing.Size(71, 16);
            this.rbbUpdata.TabIndex = 6;
            this.rbbUpdata.TabStop = true;
            this.rbbUpdata.Tag = "5";
            this.rbbUpdata.Text = "系统更新";
            this.rbbUpdata.UseVisualStyleBackColor = true;
            this.rbbUpdata.CheckedChanged += new System.EventHandler(this.rbbStart_CheckedChanged);
            // 
            // cbbMode
            // 
            this.cbbMode.FormattingEnabled = true;
            this.cbbMode.Location = new System.Drawing.Point(472, 11);
            this.cbbMode.Name = "cbbMode";
            this.cbbMode.Size = new System.Drawing.Size(107, 20);
            this.cbbMode.TabIndex = 5;
            this.cbbMode.Visible = false;
            // 
            // rbbMode
            // 
            this.rbbMode.AutoSize = true;
            this.rbbMode.Location = new System.Drawing.Point(395, 12);
            this.rbbMode.Name = "rbbMode";
            this.rbbMode.Size = new System.Drawing.Size(71, 16);
            this.rbbMode.TabIndex = 4;
            this.rbbMode.TabStop = true;
            this.rbbMode.Tag = "4";
            this.rbbMode.Text = "参数设置";
            this.rbbMode.UseVisualStyleBackColor = true;
            this.rbbMode.Visible = false;
            this.rbbMode.CheckedChanged += new System.EventHandler(this.rbbStart_CheckedChanged);
            // 
            // rbbSys
            // 
            this.rbbSys.AutoSize = true;
            this.rbbSys.Location = new System.Drawing.Point(308, 12);
            this.rbbSys.Name = "rbbSys";
            this.rbbSys.Size = new System.Drawing.Size(71, 16);
            this.rbbSys.TabIndex = 3;
            this.rbbSys.TabStop = true;
            this.rbbSys.Tag = "3";
            this.rbbSys.Text = "系统设置";
            this.rbbSys.UseVisualStyleBackColor = true;
            this.rbbSys.Visible = false;
            this.rbbSys.CheckedChanged += new System.EventHandler(this.rbbStart_CheckedChanged);
            // 
            // rbbStop
            // 
            this.rbbStop.AutoSize = true;
            this.rbbStop.Location = new System.Drawing.Point(155, 11);
            this.rbbStop.Name = "rbbStop";
            this.rbbStop.Size = new System.Drawing.Size(47, 16);
            this.rbbStop.TabIndex = 2;
            this.rbbStop.TabStop = true;
            this.rbbStop.Tag = "2";
            this.rbbStop.Text = "停止";
            this.rbbStop.UseVisualStyleBackColor = true;
            this.rbbStop.CheckedChanged += new System.EventHandler(this.rbbStart_CheckedChanged);
            // 
            // rbbNext
            // 
            this.rbbNext.AutoSize = true;
            this.rbbNext.Location = new System.Drawing.Point(82, 11);
            this.rbbNext.Name = "rbbNext";
            this.rbbNext.Size = new System.Drawing.Size(59, 16);
            this.rbbNext.TabIndex = 1;
            this.rbbNext.TabStop = true;
            this.rbbNext.Tag = "1";
            this.rbbNext.Text = "下一步";
            this.rbbNext.UseVisualStyleBackColor = true;
            this.rbbNext.CheckedChanged += new System.EventHandler(this.rbbStart_CheckedChanged);
            // 
            // rbbStart
            // 
            this.rbbStart.AutoSize = true;
            this.rbbStart.Location = new System.Drawing.Point(19, 11);
            this.rbbStart.Name = "rbbStart";
            this.rbbStart.Size = new System.Drawing.Size(47, 16);
            this.rbbStart.TabIndex = 0;
            this.rbbStart.TabStop = true;
            this.rbbStart.Tag = "0";
            this.rbbStart.Text = "开始";
            this.rbbStart.UseVisualStyleBackColor = true;
            this.rbbStart.CheckedChanged += new System.EventHandler(this.rbbStart_CheckedChanged);
            // 
            // rbbShutDown
            // 
            this.rbbShutDown.AutoSize = true;
            this.rbbShutDown.Location = new System.Drawing.Point(594, 11);
            this.rbbShutDown.Name = "rbbShutDown";
            this.rbbShutDown.Size = new System.Drawing.Size(47, 16);
            this.rbbShutDown.TabIndex = 7;
            this.rbbShutDown.TabStop = true;
            this.rbbShutDown.Tag = "6";
            this.rbbShutDown.Text = "关机";
            this.rbbShutDown.UseVisualStyleBackColor = true;
            this.rbbShutDown.Visible = false;
            // 
            // frmSend
            // 
            this.AcceptButton = this.btnSend;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnExit;
            this.ClientSize = new System.Drawing.Size(820, 514);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTop);
            this.Controls.Add(this.panel2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmSend";
            this.Load += new System.EventHandler(this.frmSend_Load);
            this.panTop.ResumeLayout(false);
            this.gbSend.ResumeLayout(false);
            this.gbSend.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTop;
        private System.Windows.Forms.GroupBox gbSend;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton rbbMode;
        private System.Windows.Forms.RadioButton rbbSys;
        private System.Windows.Forms.RadioButton rbbStop;
        private System.Windows.Forms.RadioButton rbbNext;
        private System.Windows.Forms.RadioButton rbbStart;
        private System.Windows.Forms.ComboBox cbbMode;
        private System.Windows.Forms.RadioButton rbbUpdata;
        private System.Windows.Forms.RadioButton rbbShutDown;
    }
}