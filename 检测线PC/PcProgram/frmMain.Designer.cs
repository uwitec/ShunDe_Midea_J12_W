namespace PcProgram
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolBtnBar = new System.Windows.Forms.ToolStripButton();
            this.toolBtnMode = new System.Windows.Forms.ToolStripButton();
            this.toolBtnRun = new System.Windows.Forms.ToolStripButton();
            this.toolBtnReport = new System.Windows.Forms.ToolStripButton();
            this.toolBtnAbout = new System.Windows.Forms.ToolStripButton();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.系统设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.条码设置BToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.参数设置SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.系统设置ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.语言LToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.engLishEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.中文CToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.退出系统XToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.系统运行RToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.自动检测AToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.手动检测HToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.用户UToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.系统登陆LToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.系统登出EToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.密码修改ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.数据与报表DToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.数据查看ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.安规数据AToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.历史设置SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关于AToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.公司ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.工具TToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.下位机连接CToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关机SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblTitle = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.Name = "statusStrip1";
            // 
            // toolStrip1
            // 
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(25, 25);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolBtnBar,
            this.toolBtnMode,
            this.toolBtnRun,
            this.toolBtnReport,
            this.toolBtnAbout});
            this.toolStrip1.Name = "toolStrip1";
            // 
            // toolBtnBar
            // 
            resources.ApplyResources(this.toolBtnBar, "toolBtnBar");
            this.toolBtnBar.Name = "toolBtnBar";
            this.toolBtnBar.Click += new System.EventHandler(this.toolBtnBar_Click);
            // 
            // toolBtnMode
            // 
            resources.ApplyResources(this.toolBtnMode, "toolBtnMode");
            this.toolBtnMode.Name = "toolBtnMode";
            this.toolBtnMode.Click += new System.EventHandler(this.toolBtnMode_Click);
            // 
            // toolBtnRun
            // 
            resources.ApplyResources(this.toolBtnRun, "toolBtnRun");
            this.toolBtnRun.Name = "toolBtnRun";
            this.toolBtnRun.Click += new System.EventHandler(this.toolBtnRun_Click);
            // 
            // toolBtnReport
            // 
            resources.ApplyResources(this.toolBtnReport, "toolBtnReport");
            this.toolBtnReport.Name = "toolBtnReport";
            this.toolBtnReport.Click += new System.EventHandler(this.toolBtnReport_Click);
            // 
            // toolBtnAbout
            // 
            resources.ApplyResources(this.toolBtnAbout, "toolBtnAbout");
            this.toolBtnAbout.Name = "toolBtnAbout";
            this.toolBtnAbout.Click += new System.EventHandler(this.toolBtnAbout_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.系统设置ToolStripMenuItem,
            this.系统运行RToolStripMenuItem,
            this.用户UToolStripMenuItem,
            this.数据与报表DToolStripMenuItem,
            this.关于AToolStripMenuItem,
            this.工具TToolStripMenuItem});
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            // 
            // 系统设置ToolStripMenuItem
            // 
            this.系统设置ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.条码设置BToolStripMenuItem,
            this.参数设置SToolStripMenuItem,
            this.系统设置ToolStripMenuItem1,
            this.toolStripSeparator3,
            this.语言LToolStripMenuItem,
            this.toolStripSeparator4,
            this.退出系统XToolStripMenuItem});
            this.系统设置ToolStripMenuItem.Name = "系统设置ToolStripMenuItem";
            resources.ApplyResources(this.系统设置ToolStripMenuItem, "系统设置ToolStripMenuItem");
            // 
            // 条码设置BToolStripMenuItem
            // 
            resources.ApplyResources(this.条码设置BToolStripMenuItem, "条码设置BToolStripMenuItem");
            this.条码设置BToolStripMenuItem.Name = "条码设置BToolStripMenuItem";
            this.条码设置BToolStripMenuItem.Click += new System.EventHandler(this.条码设置BToolStripMenuItem_Click);
            // 
            // 参数设置SToolStripMenuItem
            // 
            resources.ApplyResources(this.参数设置SToolStripMenuItem, "参数设置SToolStripMenuItem");
            this.参数设置SToolStripMenuItem.Name = "参数设置SToolStripMenuItem";
            this.参数设置SToolStripMenuItem.Click += new System.EventHandler(this.参数设置SToolStripMenuItem_Click);
            // 
            // 系统设置ToolStripMenuItem1
            // 
            resources.ApplyResources(this.系统设置ToolStripMenuItem1, "系统设置ToolStripMenuItem1");
            this.系统设置ToolStripMenuItem1.Name = "系统设置ToolStripMenuItem1";
            this.系统设置ToolStripMenuItem1.Click += new System.EventHandler(this.系统设置ToolStripMenuItem1_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // 语言LToolStripMenuItem
            // 
            this.语言LToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.engLishEToolStripMenuItem,
            this.中文CToolStripMenuItem});
            this.语言LToolStripMenuItem.Name = "语言LToolStripMenuItem";
            resources.ApplyResources(this.语言LToolStripMenuItem, "语言LToolStripMenuItem");
            // 
            // engLishEToolStripMenuItem
            // 
            this.engLishEToolStripMenuItem.Name = "engLishEToolStripMenuItem";
            resources.ApplyResources(this.engLishEToolStripMenuItem, "engLishEToolStripMenuItem");
            this.engLishEToolStripMenuItem.Click += new System.EventHandler(this.engLishEToolStripMenuItem_Click);
            // 
            // 中文CToolStripMenuItem
            // 
            this.中文CToolStripMenuItem.Checked = true;
            this.中文CToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.中文CToolStripMenuItem.Name = "中文CToolStripMenuItem";
            resources.ApplyResources(this.中文CToolStripMenuItem, "中文CToolStripMenuItem");
            this.中文CToolStripMenuItem.Click += new System.EventHandler(this.中文CToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            // 
            // 退出系统XToolStripMenuItem
            // 
            this.退出系统XToolStripMenuItem.Name = "退出系统XToolStripMenuItem";
            resources.ApplyResources(this.退出系统XToolStripMenuItem, "退出系统XToolStripMenuItem");
            this.退出系统XToolStripMenuItem.Click += new System.EventHandler(this.退出系统XToolStripMenuItem_Click);
            // 
            // 系统运行RToolStripMenuItem
            // 
            this.系统运行RToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.自动检测AToolStripMenuItem,
            this.手动检测HToolStripMenuItem});
            this.系统运行RToolStripMenuItem.Name = "系统运行RToolStripMenuItem";
            resources.ApplyResources(this.系统运行RToolStripMenuItem, "系统运行RToolStripMenuItem");
            // 
            // 自动检测AToolStripMenuItem
            // 
            this.自动检测AToolStripMenuItem.Name = "自动检测AToolStripMenuItem";
            resources.ApplyResources(this.自动检测AToolStripMenuItem, "自动检测AToolStripMenuItem");
            this.自动检测AToolStripMenuItem.Click += new System.EventHandler(this.自动检测AToolStripMenuItem_Click);
            // 
            // 手动检测HToolStripMenuItem
            // 
            this.手动检测HToolStripMenuItem.Name = "手动检测HToolStripMenuItem";
            resources.ApplyResources(this.手动检测HToolStripMenuItem, "手动检测HToolStripMenuItem");
            this.手动检测HToolStripMenuItem.Click += new System.EventHandler(this.手动检测HToolStripMenuItem_Click);
            // 
            // 用户UToolStripMenuItem
            // 
            this.用户UToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.系统登陆LToolStripMenuItem,
            this.系统登出EToolStripMenuItem,
            this.密码修改ToolStripMenuItem});
            this.用户UToolStripMenuItem.Name = "用户UToolStripMenuItem";
            resources.ApplyResources(this.用户UToolStripMenuItem, "用户UToolStripMenuItem");
            // 
            // 系统登陆LToolStripMenuItem
            // 
            this.系统登陆LToolStripMenuItem.Name = "系统登陆LToolStripMenuItem";
            resources.ApplyResources(this.系统登陆LToolStripMenuItem, "系统登陆LToolStripMenuItem");
            this.系统登陆LToolStripMenuItem.Click += new System.EventHandler(this.系统登陆LToolStripMenuItem_Click);
            // 
            // 系统登出EToolStripMenuItem
            // 
            resources.ApplyResources(this.系统登出EToolStripMenuItem, "系统登出EToolStripMenuItem");
            this.系统登出EToolStripMenuItem.Name = "系统登出EToolStripMenuItem";
            this.系统登出EToolStripMenuItem.Click += new System.EventHandler(this.系统登出EToolStripMenuItem_Click);
            // 
            // 密码修改ToolStripMenuItem
            // 
            resources.ApplyResources(this.密码修改ToolStripMenuItem, "密码修改ToolStripMenuItem");
            this.密码修改ToolStripMenuItem.Name = "密码修改ToolStripMenuItem";
            this.密码修改ToolStripMenuItem.Click += new System.EventHandler(this.密码修改ToolStripMenuItem_Click);
            // 
            // 数据与报表DToolStripMenuItem
            // 
            this.数据与报表DToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.数据查看ToolStripMenuItem,
            this.安规数据AToolStripMenuItem,
            this.历史设置SToolStripMenuItem});
            this.数据与报表DToolStripMenuItem.Name = "数据与报表DToolStripMenuItem";
            resources.ApplyResources(this.数据与报表DToolStripMenuItem, "数据与报表DToolStripMenuItem");
            // 
            // 数据查看ToolStripMenuItem
            // 
            this.数据查看ToolStripMenuItem.Name = "数据查看ToolStripMenuItem";
            resources.ApplyResources(this.数据查看ToolStripMenuItem, "数据查看ToolStripMenuItem");
            this.数据查看ToolStripMenuItem.Click += new System.EventHandler(this.数据查看ToolStripMenuItem_Click);
            // 
            // 安规数据AToolStripMenuItem
            // 
            this.安规数据AToolStripMenuItem.Name = "安规数据AToolStripMenuItem";
            resources.ApplyResources(this.安规数据AToolStripMenuItem, "安规数据AToolStripMenuItem");
            this.安规数据AToolStripMenuItem.Click += new System.EventHandler(this.安规数据AToolStripMenuItem_Click);
            // 
            // 历史设置SToolStripMenuItem
            // 
            this.历史设置SToolStripMenuItem.Name = "历史设置SToolStripMenuItem";
            resources.ApplyResources(this.历史设置SToolStripMenuItem, "历史设置SToolStripMenuItem");
            this.历史设置SToolStripMenuItem.Click += new System.EventHandler(this.历史设置SToolStripMenuItem_Click);
            // 
            // 关于AToolStripMenuItem
            // 
            this.关于AToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.帮助ToolStripMenuItem,
            this.toolStripSeparator1,
            this.公司ToolStripMenuItem});
            this.关于AToolStripMenuItem.Name = "关于AToolStripMenuItem";
            resources.ApplyResources(this.关于AToolStripMenuItem, "关于AToolStripMenuItem");
            // 
            // 帮助ToolStripMenuItem
            // 
            this.帮助ToolStripMenuItem.Name = "帮助ToolStripMenuItem";
            resources.ApplyResources(this.帮助ToolStripMenuItem, "帮助ToolStripMenuItem");
            this.帮助ToolStripMenuItem.Click += new System.EventHandler(this.帮助ToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // 公司ToolStripMenuItem
            // 
            this.公司ToolStripMenuItem.Name = "公司ToolStripMenuItem";
            resources.ApplyResources(this.公司ToolStripMenuItem, "公司ToolStripMenuItem");
            this.公司ToolStripMenuItem.Click += new System.EventHandler(this.公司ToolStripMenuItem_Click);
            // 
            // 工具TToolStripMenuItem
            // 
            this.工具TToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.下位机连接CToolStripMenuItem,
            this.关机SToolStripMenuItem});
            this.工具TToolStripMenuItem.Name = "工具TToolStripMenuItem";
            resources.ApplyResources(this.工具TToolStripMenuItem, "工具TToolStripMenuItem");
            // 
            // 下位机连接CToolStripMenuItem
            // 
            this.下位机连接CToolStripMenuItem.Name = "下位机连接CToolStripMenuItem";
            resources.ApplyResources(this.下位机连接CToolStripMenuItem, "下位机连接CToolStripMenuItem");
            this.下位机连接CToolStripMenuItem.Click += new System.EventHandler(this.下位机连接CToolStripMenuItem_Click);
            // 
            // 关机SToolStripMenuItem
            // 
            this.关机SToolStripMenuItem.Name = "关机SToolStripMenuItem";
            resources.ApplyResources(this.关机SToolStripMenuItem, "关机SToolStripMenuItem");
            this.关机SToolStripMenuItem.Click += new System.EventHandler(this.关机SToolStripMenuItem_Click);
            // 
            // lblTitle
            // 
            resources.ApplyResources(this.lblTitle, "lblTitle");
            this.lblTitle.BackColor = System.Drawing.Color.DarkGray;
            this.lblTitle.ForeColor = System.Drawing.Color.Red;
            this.lblTitle.Name = "lblTitle";
            // 
            // frmMain
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.Shown += new System.EventHandler(this.frmMain_Shown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMain_KeyDown);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 系统设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 用户UToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 系统登陆LToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 系统登出EToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 数据与报表DToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 数据查看ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关于AToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 公司ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem 帮助ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 密码修改ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 条码设置BToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 参数设置SToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 系统设置ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem 退出系统XToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 系统运行RToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 自动检测AToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 手动检测HToolStripMenuItem;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.ToolStripMenuItem 工具TToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 下位机连接CToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolBtnBar;
        private System.Windows.Forms.ToolStripButton toolBtnMode;
        private System.Windows.Forms.ToolStripButton toolBtnRun;
        private System.Windows.Forms.ToolStripButton toolBtnReport;
        private System.Windows.Forms.ToolStripButton toolBtnAbout;
        private System.Windows.Forms.ToolStripMenuItem 语言LToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem engLishEToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 中文CToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem 安规数据AToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 历史设置SToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关机SToolStripMenuItem;
    }
}

