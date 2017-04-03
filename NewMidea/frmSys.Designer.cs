namespace NewMideaProgram
{
    partial class frmSys
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSys));
            this.panMdi = new System.Windows.Forms.Panel();
            this.panSysGrid = new System.Windows.Forms.Panel();
            this.GridSys = new System.Windows.Forms.DataGridView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolBtnLanguage = new System.Windows.Forms.ToolStripButton();
            this.toolBtnSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolBtnCancel = new System.Windows.Forms.ToolStripButton();
            this.panMdi.SuspendLayout();
            this.panSysGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridSys)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panMdi
            // 
            this.panMdi.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panMdi.Controls.Add(this.panSysGrid);
            this.panMdi.Controls.Add(this.toolStrip1);
            resources.ApplyResources(this.panMdi, "panMdi");
            this.panMdi.Name = "panMdi";
            // 
            // panSysGrid
            // 
            this.panSysGrid.Controls.Add(this.GridSys);
            resources.ApplyResources(this.panSysGrid, "panSysGrid");
            this.panSysGrid.Name = "panSysGrid";
            // 
            // GridSys
            // 
            this.GridSys.AllowUserToAddRows = false;
            this.GridSys.AllowUserToDeleteRows = false;
            this.GridSys.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.GridSys.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.GridSys.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.GridSys, "GridSys");
            this.GridSys.MultiSelect = false;
            this.GridSys.Name = "GridSys";
            this.GridSys.RowHeadersVisible = false;
            this.GridSys.RowTemplate.Height = 23;
            this.GridSys.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.GridSys.Tag = "SystemSet";
            this.GridSys.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.GridSys_DataError);
            // 
            // toolStrip1
            // 
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolBtnLanguage,
            this.toolBtnSave,
            this.toolStripSeparator2,
            this.toolBtnCancel});
            this.toolStrip1.Name = "toolStrip1";
            // 
            // toolBtnLanguage
            // 
            resources.ApplyResources(this.toolBtnLanguage, "toolBtnLanguage");
            this.toolBtnLanguage.Name = "toolBtnLanguage";
            this.toolBtnLanguage.Click += new System.EventHandler(this.toolBtnLanguage_Click);
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
            this.toolBtnCancel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            resources.ApplyResources(this.toolBtnCancel, "toolBtnCancel");
            this.toolBtnCancel.Name = "toolBtnCancel";
            this.toolBtnCancel.Click += new System.EventHandler(this.toolBtnCancel_Click);
            // 
            // frmSys
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.panMdi);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmSys";
            this.Load += new System.EventHandler(this.frmSys_Load);
            this.panMdi.ResumeLayout(false);
            this.panSysGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GridSys)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel panMdi;
        public System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolBtnLanguage;
        private System.Windows.Forms.ToolStripButton toolBtnSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.Panel panSysGrid;
        private System.Windows.Forms.DataGridView GridSys;
        private System.Windows.Forms.ToolStripButton toolBtnCancel;

    }
}