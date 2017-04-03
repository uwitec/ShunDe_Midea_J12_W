namespace PcProgram
{
		partial class frmAbout
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
                    System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAbout));
                    this.label1 = new System.Windows.Forms.Label();
                    this.label2 = new System.Windows.Forms.Label();
                    this.label5 = new System.Windows.Forms.Label();
                    this.label6 = new System.Windows.Forms.Label();
                    this.label7 = new System.Windows.Forms.Label();
                    this.label3 = new System.Windows.Forms.Label();
                    this.label4 = new System.Windows.Forms.Label();
                    this.label8 = new System.Windows.Forms.Label();
                    this.SuspendLayout();
                    // 
                    // label1
                    // 
                    this.label1.AutoSize = true;
                    this.label1.Location = new System.Drawing.Point(83, 23);
                    this.label1.Name = "label1";
                    this.label1.Size = new System.Drawing.Size(161, 12);
                    this.label1.TabIndex = 1;
                    this.label1.Text = "广州弘科自动化工程有限公司";
                    // 
                    // label2
                    // 
                    this.label2.AutoSize = true;
                    this.label2.Location = new System.Drawing.Point(42, 23);
                    this.label2.Name = "label2";
                    this.label2.Size = new System.Drawing.Size(41, 12);
                    this.label2.TabIndex = 3;
                    this.label2.Text = "制 作:";
                    // 
                    // label5
                    // 
                    this.label5.AutoSize = true;
                    this.label5.Location = new System.Drawing.Point(42, 51);
                    this.label5.Name = "label5";
                    this.label5.Size = new System.Drawing.Size(41, 12);
                    this.label5.TabIndex = 6;
                    this.label5.Text = "电 话:";
                    // 
                    // label6
                    // 
                    this.label6.AutoSize = true;
                    this.label6.Location = new System.Drawing.Point(83, 51);
                    this.label6.Name = "label6";
                    this.label6.Size = new System.Drawing.Size(149, 12);
                    this.label6.TabIndex = 7;
                    this.label6.Text = "13902223851,020-89888208";
                    // 
                    // label7
                    // 
                    this.label7.AutoSize = true;
                    this.label7.Location = new System.Drawing.Point(42, 107);
                    this.label7.Name = "label7";
                    this.label7.Size = new System.Drawing.Size(41, 12);
                    this.label7.TabIndex = 8;
                    this.label7.Text = "地 址:";
                    // 
                    // label3
                    // 
                    this.label3.AutoSize = true;
                    this.label3.Location = new System.Drawing.Point(83, 107);
                    this.label3.Name = "label3";
                    this.label3.Size = new System.Drawing.Size(185, 12);
                    this.label3.TabIndex = 9;
                    this.label3.Text = "广州市海珠区土华华洲路109号C幢";
                    // 
                    // label4
                    // 
                    this.label4.AutoSize = true;
                    this.label4.Location = new System.Drawing.Point(83, 79);
                    this.label4.Name = "label4";
                    this.label4.Size = new System.Drawing.Size(59, 12);
                    this.label4.TabIndex = 11;
                    this.label4.Text = "312637477";
                    // 
                    // label8
                    // 
                    this.label8.AutoSize = true;
                    this.label8.Location = new System.Drawing.Point(42, 79);
                    this.label8.Name = "label8";
                    this.label8.Size = new System.Drawing.Size(41, 12);
                    this.label8.TabIndex = 10;
                    this.label8.Text = " Q Q :";
                    // 
                    // frmAbout
                    // 
                    this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
                    this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                    this.ClientSize = new System.Drawing.Size(307, 139);
                    this.Controls.Add(this.label4);
                    this.Controls.Add(this.label8);
                    this.Controls.Add(this.label3);
                    this.Controls.Add(this.label7);
                    this.Controls.Add(this.label6);
                    this.Controls.Add(this.label5);
                    this.Controls.Add(this.label2);
                    this.Controls.Add(this.label1);
                    this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
                    this.MaximizeBox = false;
                    this.MinimizeBox = false;
                    this.Name = "frmAbout";
                    this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
                    this.Text = "关于";
                    this.TopMost = true;
                    this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmAbout_KeyPress);
                    this.ResumeLayout(false);
                    this.PerformLayout();

				}

				#endregion

            private System.Windows.Forms.Label label1;
            private System.Windows.Forms.Label label2;
				private System.Windows.Forms.Label label5;
				private System.Windows.Forms.Label label6;
				private System.Windows.Forms.Label label7;
            private System.Windows.Forms.Label label3;
            private System.Windows.Forms.Label label4;
            private System.Windows.Forms.Label label8;
		}
}