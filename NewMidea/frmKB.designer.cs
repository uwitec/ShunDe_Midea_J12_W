namespace NewMideaProgram
{
    partial class frmKB
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmKB));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panMdi = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.KBGrid = new System.Windows.Forms.DataGridView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolBtnJiLiang = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolBtnSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolBtnCancel = new System.Windows.Forms.ToolStripButton();
            this.panMdi.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.KBGrid)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 1500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // panMdi
            // 
            this.panMdi.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panMdi.Controls.Add(this.panel1);
            this.panMdi.Controls.Add(this.toolStrip1);
            this.panMdi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panMdi.Location = new System.Drawing.Point(0, 0);
            this.panMdi.Name = "panMdi";
            this.panMdi.Size = new System.Drawing.Size(712, 409);
            this.panMdi.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.KBGrid);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 57);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(708, 348);
            this.panel1.TabIndex = 57;
            // 
            // KBGrid
            // 
            this.KBGrid.AllowUserToAddRows = false;
            this.KBGrid.AllowUserToDeleteRows = false;
            this.KBGrid.AllowUserToResizeRows = false;
            this.KBGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.KBGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.KBGrid.Location = new System.Drawing.Point(0, 0);
            this.KBGrid.MultiSelect = false;
            this.KBGrid.Name = "KBGrid";
            this.KBGrid.RowHeadersWidth = 60;
            this.KBGrid.RowTemplate.Height = 23;
            this.KBGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.KBGrid.Size = new System.Drawing.Size(708, 348);
            this.KBGrid.TabIndex = 58;
            this.KBGrid.Tag = "KB";
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolBtnJiLiang,
            this.toolStripSeparator1,
            this.toolBtnSave,
            this.toolStripSeparator2,
            this.toolStripButton1,
            this.toolBtnCancel});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(708, 57);
            this.toolStrip1.TabIndex = 56;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolBtnJiLiang
            // 
            this.toolBtnJiLiang.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolBtnJiLiang.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnJiLiang.Image")));
            this.toolBtnJiLiang.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.toolBtnJiLiang.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolBtnJiLiang.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnJiLiang.Name = "toolBtnJiLiang";
            this.toolBtnJiLiang.Size = new System.Drawing.Size(162, 54);
            this.toolBtnJiLiang.Text = "计量选中项(&S)";
            this.toolBtnJiLiang.ToolTipText = "计量选中项(&S)";
            this.toolBtnJiLiang.Click += new System.EventHandler(this.toolBtnJiLiang_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 57);
            // 
            // toolBtnSave
            // 
            this.toolBtnSave.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolBtnSave.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnSave.Image")));
            this.toolBtnSave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolBtnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnSave.Name = "toolBtnSave";
            this.toolBtnSave.Size = new System.Drawing.Size(178, 54);
            this.toolBtnSave.Text = "手动计量保存(&S)";
            this.toolBtnSave.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 57);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(178, 54);
            this.toolStripButton1.Text = "兼容老版计量(&S)";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
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
            this.toolBtnCancel.Click += new System.EventHandler(this.toolBtnCancel_Click);
            // 
            // frmKB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(712, 409);
            this.Controls.Add(this.panMdi);
            this.Name = "frmKB";
            this.Text = "计量选择";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmKB_Load);
            this.panMdi.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.KBGrid)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        public System.Windows.Forms.Panel panMdi;
        public System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolBtnJiLiang;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolBtnSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView KBGrid;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolBtnCancel;
    }
}