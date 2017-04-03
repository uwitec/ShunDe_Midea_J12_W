namespace NewMideaProgram
{
    partial class frmJiLiang
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmJiLiang));
            this.panMdi = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.KBGrid = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.readGrid = new System.Windows.Forms.DataGridView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolBtnReutrn = new System.Windows.Forms.ToolStripButton();
            this.toolBtnSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolBtnCancel = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolBtnFlush = new System.Windows.Forms.ToolStripButton();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panMdi.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.KBGrid)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.readGrid)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panMdi
            // 
            this.panMdi.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panMdi.Controls.Add(this.groupBox2);
            this.panMdi.Controls.Add(this.groupBox1);
            this.panMdi.Controls.Add(this.toolStrip1);
            resources.ApplyResources(this.panMdi, "panMdi");
            this.panMdi.Name = "panMdi";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.KBGrid);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // KBGrid
            // 
            this.KBGrid.AllowUserToAddRows = false;
            this.KBGrid.AllowUserToDeleteRows = false;
            this.KBGrid.AllowUserToResizeRows = false;
            this.KBGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.KBGrid, "KBGrid");
            this.KBGrid.MultiSelect = false;
            this.KBGrid.Name = "KBGrid";
            this.KBGrid.RowTemplate.Height = 23;
            this.KBGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.KBGrid.Tag = "JiLiang";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.readGrid);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // readGrid
            // 
            this.readGrid.AllowUserToAddRows = false;
            this.readGrid.AllowUserToDeleteRows = false;
            this.readGrid.AllowUserToResizeRows = false;
            this.readGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.readGrid, "readGrid");
            this.readGrid.MultiSelect = false;
            this.readGrid.Name = "readGrid";
            this.readGrid.ReadOnly = true;
            this.readGrid.RowTemplate.Height = 23;
            this.readGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.readGrid.Tag = "JiLiang";
            // 
            // toolStrip1
            // 
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolBtnReutrn,
            this.toolBtnSave,
            this.toolStripSeparator2,
            this.toolBtnCancel,
            this.toolStripSeparator3,
            this.toolBtnFlush});
            this.toolStrip1.Name = "toolStrip1";
            // 
            // toolBtnReutrn
            // 
            resources.ApplyResources(this.toolBtnReutrn, "toolBtnReutrn");
            this.toolBtnReutrn.Name = "toolBtnReutrn";
            this.toolBtnReutrn.Click += new System.EventHandler(this.toolBtnJiLiang_Click);
            // 
            // toolBtnSave
            // 
            resources.ApplyResources(this.toolBtnSave, "toolBtnSave");
            this.toolBtnSave.Name = "toolBtnSave";
            this.toolBtnSave.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // toolBtnCancel
            // 
            resources.ApplyResources(this.toolBtnCancel, "toolBtnCancel");
            this.toolBtnCancel.Name = "toolBtnCancel";
            this.toolBtnCancel.Click += new System.EventHandler(this.toolBtnCancel_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // toolBtnFlush
            // 
            resources.ApplyResources(this.toolBtnFlush, "toolBtnFlush");
            this.toolBtnFlush.Name = "toolBtnFlush";
            this.toolBtnFlush.Tag = "Stop";
            this.toolBtnFlush.Click += new System.EventHandler(this.toolBtnFlush_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // frmJiLiang
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.panMdi);
            this.Name = "frmJiLiang";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmKB_Load);
            this.panMdi.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.KBGrid)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.readGrid)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel panMdi;
        public System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolBtnReutrn;
        private System.Windows.Forms.ToolStripButton toolBtnSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolBtnCancel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView KBGrid;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView readGrid;
        private System.Windows.Forms.ToolStripButton toolBtnFlush;
        private System.Windows.Forms.Timer timer1;
    }
}