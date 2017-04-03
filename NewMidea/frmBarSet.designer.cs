namespace NewMideaProgram
{
    partial class frmBarSet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBarSet));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel4 = new System.Windows.Forms.Panel();
            this.chkAutoStart = new System.Windows.Forms.CheckBox();
            this.rbtWince = new System.Windows.Forms.RadioButton();
            this.rbtComputer = new System.Windows.Forms.RadioButton();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolBtnSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolBtnCancel = new System.Windows.Forms.ToolStripButton();
            this.panel3 = new System.Windows.Forms.Panel();
            this.BarCodeSet = new System.Windows.Forms.DataGridView();
            this.panel4.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BarCodeSet)).BeginInit();
            this.SuspendLayout();
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.chkAutoStart);
            this.panel4.Controls.Add(this.rbtWince);
            this.panel4.Controls.Add(this.rbtComputer);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 340);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(638, 35);
            this.panel4.TabIndex = 0;
            // 
            // chkAutoStart
            // 
            this.chkAutoStart.Checked = true;
            this.chkAutoStart.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoStart.Location = new System.Drawing.Point(468, 5);
            this.chkAutoStart.Name = "chkAutoStart";
            this.chkAutoStart.Size = new System.Drawing.Size(154, 27);
            this.chkAutoStart.TabIndex = 8;
            this.chkAutoStart.Text = "扫描条码自动启动";
            // 
            // rbtWince
            // 
            this.rbtWince.Checked = true;
            this.rbtWince.Location = new System.Drawing.Point(187, 7);
            this.rbtWince.Name = "rbtWince";
            this.rbtWince.Size = new System.Drawing.Size(163, 22);
            this.rbtWince.TabIndex = 6;
            this.rbtWince.TabStop = true;
            this.rbtWince.Text = "使用本工位条码识别";
            // 
            // rbtComputer
            // 
            this.rbtComputer.Location = new System.Drawing.Point(27, 7);
            this.rbtComputer.Name = "rbtComputer";
            this.rbtComputer.Size = new System.Drawing.Size(154, 22);
            this.rbtComputer.TabIndex = 5;
            this.rbtComputer.Text = "使用主机条码识别";
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.toolBtnSave,
            this.toolStripSeparator2,
            this.toolBtnCancel});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(638, 57);
            this.toolStrip1.TabIndex = 56;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 57);
            // 
            // toolBtnSave
            // 
            this.toolBtnSave.Font = new System.Drawing.Font("Tahoma", 12F);
            this.toolBtnSave.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnSave.Image")));
            this.toolBtnSave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolBtnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnSave.Name = "toolBtnSave";
            this.toolBtnSave.Size = new System.Drawing.Size(114, 54);
            this.toolBtnSave.Text = "保存(&S)";
            this.toolBtnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 57);
            // 
            // toolBtnCancel
            // 
            this.toolBtnCancel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolBtnCancel.Font = new System.Drawing.Font("Tahoma", 12F);
            this.toolBtnCancel.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnCancel.Image")));
            this.toolBtnCancel.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolBtnCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnCancel.Name = "toolBtnCancel";
            this.toolBtnCancel.Size = new System.Drawing.Size(114, 54);
            this.toolBtnCancel.Text = "退出(&E)";
            this.toolBtnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.BarCodeSet);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 57);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(638, 283);
            this.panel3.TabIndex = 57;
            // 
            // BarCodeSet
            // 
            this.BarCodeSet.AllowUserToAddRows = false;
            this.BarCodeSet.AllowUserToDeleteRows = false;
            this.BarCodeSet.AllowUserToResizeRows = false;
            this.BarCodeSet.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.BarCodeSet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BarCodeSet.Location = new System.Drawing.Point(0, 0);
            this.BarCodeSet.MultiSelect = false;
            this.BarCodeSet.Name = "BarCodeSet";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.BarCodeSet.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.BarCodeSet.RowHeadersWidth = 60;
            this.BarCodeSet.RowTemplate.Height = 23;
            this.BarCodeSet.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.BarCodeSet.Size = new System.Drawing.Size(638, 283);
            this.BarCodeSet.TabIndex = 6;
            this.BarCodeSet.Tag = "BarCodeSet";
            // 
            // frmBarSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(638, 375);
            this.ControlBox = false;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.panel4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmBarSet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "条码设置";
            this.Load += new System.EventHandler(this.frmBarSet_Load);
            this.panel4.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.BarCodeSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.CheckBox chkAutoStart;
        private System.Windows.Forms.RadioButton rbtWince;
        private System.Windows.Forms.RadioButton rbtComputer;
        public System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolBtnSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolBtnCancel;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DataGridView BarCodeSet;
    }
}