namespace System
{
    partial class loopCon
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

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblRightOne = new System.Windows.Forms.Label();
            this.lblRightTwo = new System.Windows.Forms.Label();
            this.lblLeftOne = new System.Windows.Forms.Label();
            this.lblLeftTwo = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblOut = new System.Windows.Forms.Label();
            this.lblIn = new System.Windows.Forms.Label();
            this.lblStaticBar = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lblRightOne
            // 
            this.lblRightOne.AutoSize = true;
            this.lblRightOne.ForeColor = System.Drawing.Color.Cyan;
            this.lblRightOne.Location = new System.Drawing.Point(185, 148);
            this.lblRightOne.Name = "lblRightOne";
            this.lblRightOne.Size = new System.Drawing.Size(17, 12);
            this.lblRightOne.TabIndex = 3;
            this.lblRightOne.Text = "→";
            // 
            // lblRightTwo
            // 
            this.lblRightTwo.AutoSize = true;
            this.lblRightTwo.ForeColor = System.Drawing.Color.Cyan;
            this.lblRightTwo.Location = new System.Drawing.Point(154, 148);
            this.lblRightTwo.Name = "lblRightTwo";
            this.lblRightTwo.Size = new System.Drawing.Size(17, 12);
            this.lblRightTwo.TabIndex = 4;
            this.lblRightTwo.Text = "→";
            // 
            // lblLeftOne
            // 
            this.lblLeftOne.AutoSize = true;
            this.lblLeftOne.ForeColor = System.Drawing.Color.Cyan;
            this.lblLeftOne.Location = new System.Drawing.Point(185, 160);
            this.lblLeftOne.Name = "lblLeftOne";
            this.lblLeftOne.Size = new System.Drawing.Size(17, 12);
            this.lblLeftOne.TabIndex = 5;
            this.lblLeftOne.Text = "←";
            // 
            // lblLeftTwo
            // 
            this.lblLeftTwo.AutoSize = true;
            this.lblLeftTwo.ForeColor = System.Drawing.Color.Cyan;
            this.lblLeftTwo.Location = new System.Drawing.Point(154, 160);
            this.lblLeftTwo.Name = "lblLeftTwo";
            this.lblLeftTwo.Size = new System.Drawing.Size(17, 12);
            this.lblLeftTwo.TabIndex = 6;
            this.lblLeftTwo.Text = "←";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(256, 205);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Resize += new System.EventHandler(this.pictureBox1_Resize);
            // 
            // lblOut
            // 
            this.lblOut.AutoSize = true;
            this.lblOut.ForeColor = System.Drawing.Color.DeepPink;
            this.lblOut.Location = new System.Drawing.Point(195, 77);
            this.lblOut.Name = "lblOut";
            this.lblOut.Size = new System.Drawing.Size(29, 12);
            this.lblOut.TabIndex = 15;
            this.lblOut.Text = "↑↑";
            // 
            // lblIn
            // 
            this.lblIn.AutoSize = true;
            this.lblIn.ForeColor = System.Drawing.Color.DeepPink;
            this.lblIn.Location = new System.Drawing.Point(158, 77);
            this.lblIn.Name = "lblIn";
            this.lblIn.Size = new System.Drawing.Size(29, 12);
            this.lblIn.TabIndex = 14;
            this.lblIn.Text = "↓↓";
            // 
            // lblStaticBar
            // 
            this.lblStaticBar.AutoSize = true;
            this.lblStaticBar.ForeColor = System.Drawing.Color.Red;
            this.lblStaticBar.Location = new System.Drawing.Point(45, 115);
            this.lblStaticBar.Name = "lblStaticBar";
            this.lblStaticBar.Size = new System.Drawing.Size(29, 12);
            this.lblStaticBar.TabIndex = 13;
            this.lblStaticBar.Text = "▃▃";
            // 
            // loopCon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblOut);
            this.Controls.Add(this.lblIn);
            this.Controls.Add(this.lblStaticBar);
            this.Controls.Add(this.lblLeftTwo);
            this.Controls.Add(this.lblLeftOne);
            this.Controls.Add(this.lblRightTwo);
            this.Controls.Add(this.lblRightOne);
            this.Controls.Add(this.pictureBox1);
            this.Name = "loopCon";
            this.Size = new System.Drawing.Size(256, 205);
            this.Load += new System.EventHandler(this.loopCon_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblRightOne;
        private System.Windows.Forms.Label lblRightTwo;
        private System.Windows.Forms.Label lblLeftOne;
        private System.Windows.Forms.Label lblLeftTwo;
        private System.Windows.Forms.Label lblOut;
        private System.Windows.Forms.Label lblIn;
        private System.Windows.Forms.Label lblStaticBar;



    }
}
