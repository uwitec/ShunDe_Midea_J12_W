namespace PcProgram
{
    partial class frmIC
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.cbbTestIndex = new System.Windows.Forms.ComboBox();
            this.lblIC = new System.Windows.Forms.Label();
            this.ICView = new System.Windows.Forms.DataGridView();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ICView)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnExit);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Controls.Add(this.cbbTestIndex);
            this.panel1.Controls.Add(this.lblIC);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 284);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(716, 40);
            this.panel1.TabIndex = 1;
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(606, 9);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(96, 24);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "退出(&E)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(504, 9);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(96, 24);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "保存(&S)";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(402, 9);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(96, 24);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "添加(&A)";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // cbbTestIndex
            // 
            this.cbbTestIndex.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbTestIndex.FormattingEnabled = true;
            this.cbbTestIndex.Location = new System.Drawing.Point(261, 12);
            this.cbbTestIndex.Name = "cbbTestIndex";
            this.cbbTestIndex.Size = new System.Drawing.Size(120, 20);
            this.cbbTestIndex.TabIndex = 1;
            // 
            // lblIC
            // 
            this.lblIC.AutoSize = true;
            this.lblIC.Location = new System.Drawing.Point(26, 16);
            this.lblIC.Name = "lblIC";
            this.lblIC.Size = new System.Drawing.Size(0, 12);
            this.lblIC.TabIndex = 0;
            // 
            // ICView
            // 
            this.ICView.AllowUserToAddRows = false;
            this.ICView.AllowUserToDeleteRows = false;
            this.ICView.AllowUserToResizeRows = false;
            this.ICView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ICView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ICView.Location = new System.Drawing.Point(0, 0);
            this.ICView.MultiSelect = false;
            this.ICView.Name = "ICView";
            this.ICView.RowHeadersVisible = false;
            this.ICView.RowTemplate.Height = 23;
            this.ICView.ShowCellErrors = false;
            this.ICView.ShowCellToolTips = false;
            this.ICView.ShowEditingIcon = false;
            this.ICView.ShowRowErrors = false;
            this.ICView.Size = new System.Drawing.Size(716, 284);
            this.ICView.TabIndex = 2;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // frmIC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(716, 324);
            this.Controls.Add(this.ICView);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmIC";
            this.Text = "IC卡设置";
            this.Load += new System.EventHandler(this.frmIC_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ICView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ComboBox cbbTestIndex;
        private System.Windows.Forms.Label lblIC;
        private System.Windows.Forms.DataGridView ICView;
        private System.Windows.Forms.Timer timer1;
    }
}