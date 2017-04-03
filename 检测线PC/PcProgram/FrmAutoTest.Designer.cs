namespace PcProgram
{
    partial class FrmAutoTest
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAutoTest));
            this.PanLoop = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblJiQi = new System.Windows.Forms.Label();
            this.PanMid = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.LblTestNo = new System.Windows.Forms.Label();
            this.LblMode = new System.Windows.Forms.Label();
            this.lblResult = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblStep = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lable2 = new System.Windows.Forms.Label();
            this.LblId = new System.Windows.Forms.Label();
            this.LblBar = new System.Windows.Forms.Label();
            this.PanBot = new System.Windows.Forms.Panel();
            this.btnStep = new System.Windows.Forms.Button();
            this.btnNow = new System.Windows.Forms.Button();
            this.PanSelectCheck = new System.Windows.Forms.Panel();
            this.panErr = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lstError = new System.Windows.Forms.ListBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblNowTestNo = new System.Windows.Forms.Label();
            this.lblNowICCard = new System.Windows.Forms.Label();
            this.lblNowBar = new System.Windows.Forms.Label();
            this.chkAutoRun = new System.Windows.Forms.CheckBox();
            this.ChkPrint = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lblNoPass = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblPass = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lblTotle = new System.Windows.Forms.Label();
            this.chkBarStart = new System.Windows.Forms.CheckBox();
            this.TimeData = new System.Windows.Forms.Timer(this.components);
            this.loopCon1 = new System.loopCon();
            this.PanLoop.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.PanBot.SuspendLayout();
            this.panErr.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanLoop
            // 
            this.PanLoop.Controls.Add(this.loopCon1);
            this.PanLoop.Controls.Add(this.panel1);
            this.PanLoop.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanLoop.Location = new System.Drawing.Point(0, 0);
            this.PanLoop.Name = "PanLoop";
            this.PanLoop.Size = new System.Drawing.Size(1350, 280);
            this.PanLoop.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1350, 114);
            this.panel1.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTitle.ForeColor = System.Drawing.Color.Red;
            this.lblTitle.Location = new System.Drawing.Point(207, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(245, 29);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "美的外机检测系统";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 382);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1350, 347);
            this.panel2.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanel1);
            this.groupBox2.Controls.Add(this.PanBot);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1350, 347);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "数据查看:";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Outset;
            this.tableLayoutPanel1.ColumnCount = 8;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.lblJiQi, 7, 1);
            this.tableLayoutPanel1.Controls.Add(this.PanMid, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label9, 6, 1);
            this.tableLayoutPanel1.Controls.Add(this.LblTestNo, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.LblMode, 5, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblResult, 7, 0);
            this.tableLayoutPanel1.Controls.Add(this.label4, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.label7, 6, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblStep, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.lable2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.LblId, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.LblBar, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 17);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1344, 284);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // lblJiQi
            // 
            this.lblJiQi.BackColor = System.Drawing.Color.White;
            this.lblJiQi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblJiQi.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblJiQi.Location = new System.Drawing.Point(1122, 45);
            this.lblJiQi.Name = "lblJiQi";
            this.lblJiQi.Size = new System.Drawing.Size(217, 41);
            this.lblJiQi.TabIndex = 28;
            this.lblJiQi.Text = "定频机";
            this.lblJiQi.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PanMid
            // 
            this.PanMid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.tableLayoutPanel1.SetColumnSpan(this.PanMid, 8);
            this.PanMid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanMid.Location = new System.Drawing.Point(5, 91);
            this.PanMid.Name = "PanMid";
            this.PanMid.Size = new System.Drawing.Size(1334, 188);
            this.PanMid.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(5, 2);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 41);
            this.label5.TabIndex = 23;
            this.label5.Text = "小车号";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(1010, 45);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(104, 41);
            this.label9.TabIndex = 27;
            this.label9.Text = "机器:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LblTestNo
            // 
            this.LblTestNo.BackColor = System.Drawing.Color.White;
            this.LblTestNo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LblTestNo.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LblTestNo.Location = new System.Drawing.Point(117, 2);
            this.LblTestNo.Name = "LblTestNo";
            this.LblTestNo.Size = new System.Drawing.Size(215, 41);
            this.LblTestNo.TabIndex = 24;
            this.LblTestNo.Text = "18";
            this.LblTestNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LblMode
            // 
            this.LblMode.BackColor = System.Drawing.Color.White;
            this.LblMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LblMode.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LblMode.Location = new System.Drawing.Point(787, 45);
            this.LblMode.Name = "LblMode";
            this.LblMode.Size = new System.Drawing.Size(215, 41);
            this.LblMode.TabIndex = 22;
            this.LblMode.Text = "KFT-BP2N1/BP35";
            this.LblMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblResult
            // 
            this.lblResult.BackColor = System.Drawing.Color.White;
            this.lblResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblResult.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblResult.Location = new System.Drawing.Point(1122, 2);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(217, 41);
            this.lblResult.TabIndex = 26;
            this.lblResult.Text = "OK";
            this.lblResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(675, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 41);
            this.label4.TabIndex = 21;
            this.label4.Text = "机型:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(1010, 2);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(104, 41);
            this.label7.TabIndex = 25;
            this.label7.Text = "检测结果:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(675, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 41);
            this.label1.TabIndex = 16;
            this.label1.Text = "检测步骤:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblStep
            // 
            this.lblStep.BackColor = System.Drawing.Color.White;
            this.lblStep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStep.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblStep.Location = new System.Drawing.Point(787, 2);
            this.lblStep.Name = "lblStep";
            this.lblStep.Size = new System.Drawing.Size(215, 41);
            this.lblStep.TabIndex = 18;
            this.lblStep.Text = "制热";
            this.lblStep.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(340, 2);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 41);
            this.label3.TabIndex = 19;
            this.label3.Text = "ID:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lable2
            // 
            this.lable2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lable2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lable2.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lable2.Location = new System.Drawing.Point(5, 45);
            this.lable2.Name = "lable2";
            this.lable2.Size = new System.Drawing.Size(104, 41);
            this.lable2.TabIndex = 15;
            this.lable2.Text = "条码:";
            this.lable2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LblId
            // 
            this.LblId.BackColor = System.Drawing.Color.White;
            this.LblId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LblId.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LblId.Location = new System.Drawing.Point(452, 2);
            this.LblId.Name = "LblId";
            this.LblId.Size = new System.Drawing.Size(215, 41);
            this.LblId.TabIndex = 20;
            this.LblId.Text = "A123";
            this.LblId.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LblBar
            // 
            this.LblBar.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.SetColumnSpan(this.LblBar, 3);
            this.LblBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LblBar.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LblBar.Location = new System.Drawing.Point(117, 45);
            this.LblBar.Name = "LblBar";
            this.LblBar.Size = new System.Drawing.Size(550, 41);
            this.LblBar.TabIndex = 17;
            this.LblBar.Text = "A12345678901234567890123";
            this.LblBar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PanBot
            // 
            this.PanBot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.PanBot.Controls.Add(this.btnStep);
            this.PanBot.Controls.Add(this.btnNow);
            this.PanBot.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PanBot.Location = new System.Drawing.Point(3, 301);
            this.PanBot.Name = "PanBot";
            this.PanBot.Size = new System.Drawing.Size(1344, 43);
            this.PanBot.TabIndex = 3;
            this.PanBot.Visible = false;
            // 
            // btnStep
            // 
            this.btnStep.Location = new System.Drawing.Point(693, 10);
            this.btnStep.Name = "btnStep";
            this.btnStep.Size = new System.Drawing.Size(61, 23);
            this.btnStep.TabIndex = 1;
            this.btnStep.Tag = "9";
            this.btnStep.Text = "N/A";
            this.btnStep.UseVisualStyleBackColor = true;
            this.btnStep.Visible = false;
            this.btnStep.Click += new System.EventHandler(this.btnNow_Click);
            // 
            // btnNow
            // 
            this.btnNow.Location = new System.Drawing.Point(3, 10);
            this.btnNow.Name = "btnNow";
            this.btnNow.Size = new System.Drawing.Size(61, 23);
            this.btnNow.TabIndex = 0;
            this.btnNow.Tag = "-1";
            this.btnNow.Text = "实时数据";
            this.btnNow.UseVisualStyleBackColor = true;
            this.btnNow.Click += new System.EventHandler(this.btnNow_Click);
            // 
            // PanSelectCheck
            // 
            this.PanSelectCheck.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.PanSelectCheck.Location = new System.Drawing.Point(69, 4);
            this.PanSelectCheck.Name = "PanSelectCheck";
            this.PanSelectCheck.Size = new System.Drawing.Size(67, 33);
            this.PanSelectCheck.TabIndex = 20;
            // 
            // panErr
            // 
            this.panErr.Controls.Add(this.groupBox1);
            this.panErr.Dock = System.Windows.Forms.DockStyle.Top;
            this.panErr.Location = new System.Drawing.Point(0, 280);
            this.panErr.Name = "panErr";
            this.panErr.Size = new System.Drawing.Size(1350, 102);
            this.panErr.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lstError);
            this.groupBox1.Controls.Add(this.panel3);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1350, 102);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "错误信息提示";
            // 
            // lstError
            // 
            this.lstError.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstError.FormattingEnabled = true;
            this.lstError.ItemHeight = 12;
            this.lstError.Location = new System.Drawing.Point(3, 17);
            this.lstError.Name = "lstError";
            this.lstError.Size = new System.Drawing.Size(1199, 76);
            this.lstError.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lblNowTestNo);
            this.panel3.Controls.Add(this.lblNowICCard);
            this.panel3.Controls.Add(this.lblNowBar);
            this.panel3.Controls.Add(this.chkAutoRun);
            this.panel3.Controls.Add(this.ChkPrint);
            this.panel3.Controls.Add(this.groupBox4);
            this.panel3.Controls.Add(this.chkBarStart);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(1202, 17);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(145, 82);
            this.panel3.TabIndex = 0;
            // 
            // lblNowTestNo
            // 
            this.lblNowTestNo.AutoSize = true;
            this.lblNowTestNo.Location = new System.Drawing.Point(157, 82);
            this.lblNowTestNo.Name = "lblNowTestNo";
            this.lblNowTestNo.Size = new System.Drawing.Size(71, 12);
            this.lblNowTestNo.TabIndex = 13;
            this.lblNowTestNo.Text = "当前小车号:";
            this.lblNowTestNo.Visible = false;
            // 
            // lblNowICCard
            // 
            this.lblNowICCard.AutoSize = true;
            this.lblNowICCard.Location = new System.Drawing.Point(157, 65);
            this.lblNowICCard.Name = "lblNowICCard";
            this.lblNowICCard.Size = new System.Drawing.Size(53, 12);
            this.lblNowICCard.TabIndex = 12;
            this.lblNowICCard.Text = "当前IC卡";
            this.lblNowICCard.Visible = false;
            // 
            // lblNowBar
            // 
            this.lblNowBar.AutoSize = true;
            this.lblNowBar.Location = new System.Drawing.Point(15, 50);
            this.lblNowBar.Name = "lblNowBar";
            this.lblNowBar.Size = new System.Drawing.Size(59, 12);
            this.lblNowBar.TabIndex = 11;
            this.lblNowBar.Text = "当前条码:";
            this.lblNowBar.Visible = false;
            // 
            // chkAutoRun
            // 
            this.chkAutoRun.AutoSize = true;
            this.chkAutoRun.Location = new System.Drawing.Point(16, 3);
            this.chkAutoRun.Name = "chkAutoRun";
            this.chkAutoRun.Size = new System.Drawing.Size(120, 16);
            this.chkAutoRun.TabIndex = 8;
            this.chkAutoRun.Text = "错误信息自动滚动";
            this.chkAutoRun.UseVisualStyleBackColor = true;
            this.chkAutoRun.Visible = false;
            // 
            // ChkPrint
            // 
            this.ChkPrint.AutoSize = true;
            this.ChkPrint.Location = new System.Drawing.Point(0, 3);
            this.ChkPrint.Name = "ChkPrint";
            this.ChkPrint.Size = new System.Drawing.Size(132, 16);
            this.ChkPrint.TabIndex = 10;
            this.ChkPrint.Text = "条码后安规自动启动";
            this.ChkPrint.UseVisualStyleBackColor = true;
            this.ChkPrint.Visible = false;
            this.ChkPrint.CheckedChanged += new System.EventHandler(this.ChkPrint_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lblNoPass);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.lblPass);
            this.groupBox4.Controls.Add(this.label14);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.lblTotle);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox4.Location = new System.Drawing.Point(122, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(23, 82);
            this.groupBox4.TabIndex = 9;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "当天数据统计";
            this.groupBox4.Visible = false;
            // 
            // lblNoPass
            // 
            this.lblNoPass.AutoSize = true;
            this.lblNoPass.Location = new System.Drawing.Point(83, 65);
            this.lblNoPass.Name = "lblNoPass";
            this.lblNoPass.Size = new System.Drawing.Size(0, 12);
            this.lblNoPass.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 65);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 9;
            this.label6.Text = "不合格数量";
            // 
            // lblPass
            // 
            this.lblPass.AutoSize = true;
            this.lblPass.Location = new System.Drawing.Point(83, 44);
            this.lblPass.Name = "lblPass";
            this.lblPass.Size = new System.Drawing.Size(0, 12);
            this.lblPass.TabIndex = 8;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(24, 44);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(53, 12);
            this.label14.TabIndex = 7;
            this.label14.Text = "合格数量";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(24, 23);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(53, 12);
            this.label12.TabIndex = 6;
            this.label12.Text = "测试数量";
            // 
            // lblTotle
            // 
            this.lblTotle.AutoSize = true;
            this.lblTotle.Location = new System.Drawing.Point(83, 23);
            this.lblTotle.Name = "lblTotle";
            this.lblTotle.Size = new System.Drawing.Size(0, 12);
            this.lblTotle.TabIndex = 5;
            // 
            // chkBarStart
            // 
            this.chkBarStart.AutoSize = true;
            this.chkBarStart.Location = new System.Drawing.Point(17, 31);
            this.chkBarStart.Name = "chkBarStart";
            this.chkBarStart.Size = new System.Drawing.Size(108, 16);
            this.chkBarStart.TabIndex = 7;
            this.chkBarStart.Text = "条码后自动启动";
            this.chkBarStart.UseVisualStyleBackColor = true;
            this.chkBarStart.CheckedChanged += new System.EventHandler(this.chkBarStart_CheckedChanged);
            // 
            // TimeData
            // 
            this.TimeData.Interval = 500;
            this.TimeData.Tick += new System.EventHandler(this.TimeData_Tick);
            // 
            // loopCon1
            // 
            this.loopCon1.ArrowVisiable = false;
            this.loopCon1.BoardIndex = 7;
            this.loopCon1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.loopCon1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.loopCon1.LabBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.loopCon1.LabelTurnMethod = System.loopCon.turnMethod.notByTime;
            this.loopCon1.LengthCount = 7;
            this.loopCon1.Location = new System.Drawing.Point(0, 114);
            this.loopCon1.MachineIn = 10;
            this.loopCon1.MachineOut = 2;
            this.loopCon1.Name = "loopCon1";
            this.loopCon1.PicBackColor = System.Drawing.SystemColors.Control;
            this.loopCon1.Size = new System.Drawing.Size(1350, 166);
            this.loopCon1.StationCount = 7;
            this.loopCon1.TabIndex = 1;
            this.loopCon1.TurnMethod = System.loopCon.turnMethod.byTime;
            this.loopCon1.LabelClick += new System.EventHandler(this.loopCon1_LabelClick);
            // 
            // FrmAutoTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1350, 729);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panErr);
            this.Controls.Add(this.PanLoop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "FrmAutoTest";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "自动检测";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmHandleTest_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmAutoTest_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmAutoTest_KeyDown);
            this.PanLoop.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.PanBot.ResumeLayout(false);
            this.panErr.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        

        #endregion

        private System.Windows.Forms.Panel PanLoop;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel PanBot;
        private System.Windows.Forms.Label lblJiQi;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label LblTestNo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label LblMode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label LblId;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblStep;
        private System.Windows.Forms.Label LblBar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lable2;
        private System.Windows.Forms.Button btnStep;
        private System.Windows.Forms.Button btnNow;
        private System.Windows.Forms.Panel PanSelectCheck;
        private System.Windows.Forms.Panel PanMid;
        private System.Windows.Forms.Panel panErr;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.loopCon loopCon1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Timer TimeData;
        private System.Windows.Forms.ListBox lstError;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label lblNoPass;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblPass;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lblTotle;
        private System.Windows.Forms.CheckBox chkAutoRun;
        private System.Windows.Forms.CheckBox chkBarStart;
        private System.Windows.Forms.CheckBox ChkPrint;
        private System.Windows.Forms.Label lblNowTestNo;
        private System.Windows.Forms.Label lblNowICCard;
        private System.Windows.Forms.Label lblNowBar;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;







    }
}