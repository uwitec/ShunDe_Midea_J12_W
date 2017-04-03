namespace PcProgram
{
    partial class frmMideaSn
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMideaSn));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.chkIsAuto = new System.Windows.Forms.CheckBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.txtNengLi = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.chkDanLen = new System.Windows.Forms.CheckBox();
            this.chkKuaiJian = new System.Windows.Forms.CheckBox();
            this.cbbNengJi = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbbFengSu = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPinLv = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbbMoshi = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbbMachine = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.txtSn = new System.Windows.Forms.TextBox();
            this.lblSn = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Silver;
            this.panel1.Controls.Add(this.panel5);
            this.panel1.Controls.Add(this.panel3);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.Silver;
            this.panel5.Controls.Add(this.chkIsAuto);
            resources.ApplyResources(this.panel5, "panel5");
            this.panel5.Name = "panel5";
            // 
            // chkIsAuto
            // 
            this.chkIsAuto.Checked = true;
            this.chkIsAuto.CheckState = System.Windows.Forms.CheckState.Checked;
            resources.ApplyResources(this.chkIsAuto, "chkIsAuto");
            this.chkIsAuto.Name = "chkIsAuto";
            this.chkIsAuto.CheckStateChanged += new System.EventHandler(this.chkIsAuto_CheckStateChanged);
            this.chkIsAuto.CheckedChanged += new System.EventHandler(this.chkIsAuto_CheckedChanged);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Silver;
            this.panel3.Controls.Add(this.btnClear);
            this.panel3.Controls.Add(this.btnOk);
            this.panel3.Controls.Add(this.btnCancel);
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Name = "panel3";
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.Silver;
            resources.ApplyResources(this.btnClear, "btnClear");
            this.btnClear.Name = "btnClear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.Silver;
            resources.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Silver;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Silver;
            this.panel2.Controls.Add(this.lblTitle);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.Silver;
            resources.ApplyResources(this.lblTitle, "lblTitle");
            this.lblTitle.ForeColor = System.Drawing.Color.Red;
            this.lblTitle.Name = "lblTitle";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Silver;
            this.panel4.Controls.Add(this.txtNengLi);
            this.panel4.Controls.Add(this.label6);
            this.panel4.Controls.Add(this.chkDanLen);
            this.panel4.Controls.Add(this.chkKuaiJian);
            this.panel4.Controls.Add(this.cbbNengJi);
            this.panel4.Controls.Add(this.label5);
            this.panel4.Controls.Add(this.cbbFengSu);
            this.panel4.Controls.Add(this.label4);
            this.panel4.Controls.Add(this.txtPinLv);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Controls.Add(this.cbbMoshi);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Controls.Add(this.cbbMachine);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Controls.Add(this.panel6);
            resources.ApplyResources(this.panel4, "panel4");
            this.panel4.Name = "panel4";
            // 
            // txtNengLi
            // 
            this.txtNengLi.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.txtNengLi, "txtNengLi");
            this.txtNengLi.Name = "txtNengLi";
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Silver;
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // chkDanLen
            // 
            this.chkDanLen.BackColor = System.Drawing.Color.Silver;
            resources.ApplyResources(this.chkDanLen, "chkDanLen");
            this.chkDanLen.Name = "chkDanLen";
            this.chkDanLen.UseVisualStyleBackColor = false;
            // 
            // chkKuaiJian
            // 
            this.chkKuaiJian.BackColor = System.Drawing.Color.Silver;
            resources.ApplyResources(this.chkKuaiJian, "chkKuaiJian");
            this.chkKuaiJian.Name = "chkKuaiJian";
            this.chkKuaiJian.UseVisualStyleBackColor = false;
            // 
            // cbbNengJi
            // 
            this.cbbNengJi.BackColor = System.Drawing.Color.White;
            this.cbbNengJi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cbbNengJi, "cbbNengJi");
            this.cbbNengJi.Name = "cbbNengJi";
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Silver;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // cbbFengSu
            // 
            this.cbbFengSu.BackColor = System.Drawing.Color.White;
            this.cbbFengSu.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cbbFengSu, "cbbFengSu");
            this.cbbFengSu.Name = "cbbFengSu";
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Silver;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // txtPinLv
            // 
            this.txtPinLv.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.txtPinLv, "txtPinLv");
            this.txtPinLv.Name = "txtPinLv";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Silver;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // cbbMoshi
            // 
            this.cbbMoshi.BackColor = System.Drawing.Color.White;
            this.cbbMoshi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cbbMoshi, "cbbMoshi");
            this.cbbMoshi.Name = "cbbMoshi";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Silver;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // cbbMachine
            // 
            this.cbbMachine.BackColor = System.Drawing.Color.White;
            this.cbbMachine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cbbMachine, "cbbMachine");
            this.cbbMachine.Name = "cbbMachine";
            this.cbbMachine.SelectedIndexChanged += new System.EventHandler(this.cbbMachine_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Silver;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.Silver;
            this.panel6.Controls.Add(this.txtSn);
            this.panel6.Controls.Add(this.lblSn);
            resources.ApplyResources(this.panel6, "panel6");
            this.panel6.Name = "panel6";
            // 
            // txtSn
            // 
            this.txtSn.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.txtSn, "txtSn");
            this.txtSn.Name = "txtSn";
            // 
            // lblSn
            // 
            this.lblSn.BackColor = System.Drawing.Color.Silver;
            resources.ApplyResources(this.lblSn, "lblSn");
            this.lblSn.Name = "lblSn";
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // frmMideaSn
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ControlBox = false;
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmMideaSn";
            this.Load += new System.EventHandler(this.frmSn_Load);
            this.panel1.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label lblSn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSn;
        private System.Windows.Forms.ComboBox cbbMachine;
        private System.Windows.Forms.ComboBox cbbMoshi;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbbNengJi;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbbFengSu;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPinLv;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkKuaiJian;
        private System.Windows.Forms.CheckBox chkDanLen;
        private System.Windows.Forms.TextBox txtNengLi;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkIsAuto;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnClear;
    }
}