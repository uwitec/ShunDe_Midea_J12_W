namespace PcProgram
{
    partial class frmPassWord
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPassWord));
            this.btnOk = new System.Windows.Forms.Button();
            this.txtPW3 = new System.Windows.Forms.TextBox();
            this.txtPW2 = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtPW1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(87, 201);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 26);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "确定";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // txtPW3
            // 
            this.txtPW3.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtPW3.Location = new System.Drawing.Point(145, 140);
            this.txtPW3.Name = "txtPW3";
            this.txtPW3.PasswordChar = '*';
            this.txtPW3.Size = new System.Drawing.Size(214, 29);
            this.txtPW3.TabIndex = 2;
            // 
            // txtPW2
            // 
            this.txtPW2.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtPW2.Location = new System.Drawing.Point(145, 82);
            this.txtPW2.Name = "txtPW2";
            this.txtPW2.PasswordChar = '*';
            this.txtPW2.Size = new System.Drawing.Size(214, 29);
            this.txtPW2.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(249, 201);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 26);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtPW1
            // 
            this.txtPW1.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtPW1.Location = new System.Drawing.Point(145, 24);
            this.txtPW1.Name = "txtPW1";
            this.txtPW1.Size = new System.Drawing.Size(214, 29);
            this.txtPW1.TabIndex = 0;
            // 
            // frmPassWord
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(371, 240);
            this.Controls.Add(this.txtPW1);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtPW3);
            this.Controls.Add(this.txtPW2);
            this.Controls.Add(this.btnCancel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmPassWord";
            this.Text = "密码修改";
            this.Load += new System.EventHandler(this.frmPassWord_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmPassWord_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.TextBox txtPW3;
        private System.Windows.Forms.TextBox txtPW2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtPW1;
    }
}