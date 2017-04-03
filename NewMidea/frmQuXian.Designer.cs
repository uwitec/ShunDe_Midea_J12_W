namespace NewMideaProgram
{
    partial class frmQuXian
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmQuXian));
            this.QuXianShangShuo = new System.Windows.Forms.Timer(this.components);
            //this.axiPlotX1 = new AxiPlotLibrary.AxiPlotX();
            //((System.ComponentModel.ISupportInitialize)(this.axiPlotX1)).BeginInit();
            this.SuspendLayout();
            // 
            // QuXianShangShuo
            // 
            this.QuXianShangShuo.Interval = 200;
            this.QuXianShangShuo.Tick += new System.EventHandler(this.QuXianShangShuo_Tick);
            // 
            // axiPlotX1
            // 
            //this.axiPlotX1.Dock = System.Windows.Forms.DockStyle.Fill;
            //this.axiPlotX1.Enabled = true;
            //this.axiPlotX1.Location = new System.Drawing.Point(0, 0);
            //this.axiPlotX1.Name = "axiPlotX1";
            //this.axiPlotX1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axiPlotX1.OcxState")));
            //this.axiPlotX1.Size = new System.Drawing.Size(565, 394);
            //this.axiPlotX1.TabIndex = 6;
            //this.axiPlotX1.OnGotFocusChannel += new AxiPlotLibrary.IiPlotXEvents_OnGotFocusChannelEventHandler(this.axiPlotX1_OnGotFocusChannel);
            // 
            // frmQuXian
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(565, 394);
            //this.Controls.Add(this.axiPlotX1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmQuXian";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "frmQuXian";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmQuXian_Load);
            //((System.ComponentModel.ISupportInitialize)(this.axiPlotX1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer QuXianShangShuo;
        //private AxiPlotLibrary.AxiPlotX axiPlotX1;
    }
}