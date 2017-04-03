namespace NewMideaProgram
{
    partial class frmList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmList));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolBtnDel = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolBtnCancel = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolBtnOk = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.panel2 = new System.Windows.Forms.Panel();
            this.listView1 = new System.Windows.Forms.ListView();
            this.ColId = new System.Windows.Forms.ColumnHeader();
            this.ColMode = new System.Windows.Forms.ColumnHeader();
            this.toolStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.AccessibleDescription = null;
            this.toolStrip1.AccessibleName = null;
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.BackgroundImage = null;
            this.toolStrip1.Font = null;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolBtnDel,
            this.toolStripSeparator2,
            this.toolBtnCancel,
            this.toolStripSeparator3,
            this.toolBtnOk,
            this.toolStripSeparator1});
            this.toolStrip1.Name = "toolStrip1";
            // 
            // toolBtnDel
            // 
            this.toolBtnDel.AccessibleDescription = null;
            this.toolBtnDel.AccessibleName = null;
            resources.ApplyResources(this.toolBtnDel, "toolBtnDel");
            this.toolBtnDel.BackgroundImage = null;
            this.toolBtnDel.Name = "toolBtnDel";
            this.toolBtnDel.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.AccessibleDescription = null;
            this.toolStripSeparator2.AccessibleName = null;
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            // 
            // toolBtnCancel
            // 
            this.toolBtnCancel.AccessibleDescription = null;
            this.toolBtnCancel.AccessibleName = null;
            this.toolBtnCancel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            resources.ApplyResources(this.toolBtnCancel, "toolBtnCancel");
            this.toolBtnCancel.BackgroundImage = null;
            this.toolBtnCancel.Name = "toolBtnCancel";
            this.toolBtnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.AccessibleDescription = null;
            this.toolStripSeparator3.AccessibleName = null;
            this.toolStripSeparator3.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            // 
            // toolBtnOk
            // 
            this.toolBtnOk.AccessibleDescription = null;
            this.toolBtnOk.AccessibleName = null;
            this.toolBtnOk.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            resources.ApplyResources(this.toolBtnOk, "toolBtnOk");
            this.toolBtnOk.BackgroundImage = null;
            this.toolBtnOk.Name = "toolBtnOk";
            this.toolBtnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.AccessibleDescription = null;
            this.toolStripSeparator1.AccessibleName = null;
            this.toolStripSeparator1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            // 
            // panel2
            // 
            this.panel2.AccessibleDescription = null;
            this.panel2.AccessibleName = null;
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackgroundImage = null;
            this.panel2.Controls.Add(this.listView1);
            this.panel2.Font = null;
            this.panel2.Name = "panel2";
            // 
            // listView1
            // 
            this.listView1.AccessibleDescription = null;
            this.listView1.AccessibleName = null;
            resources.ApplyResources(this.listView1, "listView1");
            this.listView1.BackgroundImage = null;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColId,
            this.ColMode});
            this.listView1.FullRowSelect = true;
            this.listView1.Name = "listView1";
            this.listView1.Tag = "1";
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // ColId
            // 
            resources.ApplyResources(this.ColId, "ColId");
            // 
            // ColMode
            // 
            resources.ApplyResources(this.ColMode, "ColMode");
            // 
            // frmList
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackgroundImage = null;
            this.ControlBox = false;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.toolStrip1);
            this.Font = null;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = null;
            this.Name = "frmList";
            this.Load += new System.EventHandler(this.frmList_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolBtnDel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolBtnCancel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolBtnOk;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader ColId;
        private System.Windows.Forms.ColumnHeader ColMode;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;

    }
}