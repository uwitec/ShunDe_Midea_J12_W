namespace NewMideaProgram
{
    partial class frmReport
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
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.panBot = new System.Windows.Forms.Panel();
            this.btnDel = new System.Windows.Forms.Button();
            this.panTop = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtBar = new System.Windows.Forms.TextBox();
            this.rbtBar = new System.Windows.Forms.RadioButton();
            this.rbtTime = new System.Windows.Forms.RadioButton();
            this.panMid = new System.Windows.Forms.Panel();
            this.gridView = new System.Windows.Forms.DataGrid();
            this.btnExit = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.Time = new System.Windows.Forms.ColumnHeader();
            this.Bar = new System.Windows.Forms.ColumnHeader();
            this.Mode = new System.Windows.Forms.ColumnHeader();
            this.panBot.SuspendLayout();
            this.panTop.SuspendLayout();
            this.panMid.SuspendLayout();
            this.SuspendLayout();
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "yyyy-MM-dd";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(91, 3);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(93, 24);
            this.dateTimePicker1.TabIndex = 0;
            // 
            // panBot
            // 
            this.panBot.BackColor = System.Drawing.SystemColors.Info;
            this.panBot.Controls.Add(this.btnExit);
            this.panBot.Controls.Add(this.btnDel);
            this.panBot.Location = new System.Drawing.Point(1, 338);
            this.panBot.Name = "panBot";
            this.panBot.Size = new System.Drawing.Size(637, 36);
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(13, 6);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(71, 23);
            this.btnDel.TabIndex = 0;
            this.btnDel.Text = "删除(&D)";
            // 
            // panTop
            // 
            this.panTop.BackColor = System.Drawing.SystemColors.Info;
            this.panTop.Controls.Add(this.btnSearch);
            this.panTop.Controls.Add(this.txtBar);
            this.panTop.Controls.Add(this.rbtBar);
            this.panTop.Controls.Add(this.rbtTime);
            this.panTop.Controls.Add(this.dateTimePicker1);
            this.panTop.Location = new System.Drawing.Point(0, 0);
            this.panTop.Name = "panTop";
            this.panTop.Size = new System.Drawing.Size(637, 33);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(575, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(57, 24);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "查找(&F)";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtBar
            // 
            this.txtBar.Location = new System.Drawing.Point(277, 4);
            this.txtBar.Name = "txtBar";
            this.txtBar.Size = new System.Drawing.Size(294, 23);
            this.txtBar.TabIndex = 3;
            // 
            // rbtBar
            // 
            this.rbtBar.Location = new System.Drawing.Point(202, 9);
            this.rbtBar.Name = "rbtBar";
            this.rbtBar.Size = new System.Drawing.Size(69, 17);
            this.rbtBar.TabIndex = 2;
            this.rbtBar.TabStop = false;
            this.rbtBar.Text = "按条码";
            // 
            // rbtTime
            // 
            this.rbtTime.Checked = true;
            this.rbtTime.Location = new System.Drawing.Point(16, 9);
            this.rbtTime.Name = "rbtTime";
            this.rbtTime.Size = new System.Drawing.Size(69, 17);
            this.rbtTime.TabIndex = 1;
            this.rbtTime.Text = "按时间";
            // 
            // panMid
            // 
            this.panMid.Controls.Add(this.listView1);
            this.panMid.Controls.Add(this.gridView);
            this.panMid.Location = new System.Drawing.Point(1, 34);
            this.panMid.Name = "panMid";
            this.panMid.Size = new System.Drawing.Size(637, 304);
            // 
            // gridView
            // 
            this.gridView.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.gridView.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gridView.Location = new System.Drawing.Point(0, 141);
            this.gridView.Name = "gridView";
            this.gridView.RowHeadersVisible = false;
            this.gridView.Size = new System.Drawing.Size(637, 163);
            this.gridView.TabIndex = 1;
            this.gridView.Tag = "5";
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(549, 6);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(71, 23);
            this.btnExit.TabIndex = 1;
            this.btnExit.Text = "退出(&E)";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // listView1
            // 
            this.listView1.Columns.Add(this.Time);
            this.listView1.Columns.Add(this.Bar);
            this.listView1.Columns.Add(this.Mode);
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(637, 141);
            this.listView1.TabIndex = 3;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // Time
            // 
            this.Time.Text = "时间";
            this.Time.Width = 130;
            // 
            // Bar
            // 
            this.Bar.Text = "条码";
            this.Bar.Width = 190;
            // 
            // Mode
            // 
            this.Mode.Text = "机型";
            this.Mode.Width = 300;
            // 
            // frmReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(638, 375);
            this.ControlBox = false;
            this.Controls.Add(this.panMid);
            this.Controls.Add(this.panTop);
            this.Controls.Add(this.panBot);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmReport";
            this.Text = "frmReport";
            this.Load += new System.EventHandler(this.frmReport_Load);
            this.panBot.ResumeLayout(false);
            this.panTop.ResumeLayout(false);
            this.panMid.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Panel panBot;
        private System.Windows.Forms.Panel panTop;
        private System.Windows.Forms.TextBox txtBar;
        private System.Windows.Forms.RadioButton rbtBar;
        private System.Windows.Forms.RadioButton rbtTime;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Panel panMid;
        private System.Windows.Forms.DataGrid gridView;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader Time;
        private System.Windows.Forms.ColumnHeader Bar;
        private System.Windows.Forms.ColumnHeader Mode;
    }
}