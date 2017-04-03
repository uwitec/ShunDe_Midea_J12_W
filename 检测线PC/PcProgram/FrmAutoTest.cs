using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Threading;
using System.IO.Ports;
namespace PcProgram
{
    public delegate void SendBarHandle(string BarCode);
    public partial class FrmAutoTest : Form
    {
        public FrmAutoTest()
        {
            InitializeComponent();
        }
        delegate void SetLabelTextHandle(Label lbl, string str);
        //bool AnGuiStart = false;
        //bool TempAnGuiStart = false;
        //delegate void SetWenShiDuHandle(int index, double data);
        SerialPort comICCard;
        SerialPort comBar;
        cBar mBar;
        //cAiNuo9641B mAiNuo9641;
        //cAiNuo9641B.AiNuo9641Data mAiNuoData;
        //int StartAnGuiTime = 0;
        string ReadBarCode = "";
        string SaveBarCode = "";
        //bool isStartAnGui = false;
        int readBarErr = 0;
        Label[] ShowTitle = new Label[cMain.DataShow];
        Label[] ShowData = new Label[cMain.DataShow];
        Button[] ShowButton = new Button[cModeSet.StepCount];
        double OldXSize, OldYSize;
        double xSize, ySize;
        string[] mJiQi;
        int LookIndex = 0, LookStep = -1;
        bool isLookCurrentStep = true;
        Thread thGetData;
        bool isCloseWindows = false;
        static Thread thSendBar;

        Thread thReadBar;
        Thread thReadICCard;
        private void SetLabelText(Label lbl, string str)
        {
            if (lbl.InvokeRequired)
            {
                lbl.Invoke(new SetLabelTextHandle(SetLabelText), lbl, str);
            }
            else
            {
                lbl.Text = str;
            }
        }
        private void FrmHandleTest_Load(object sender, EventArgs e)
        {
            //this.MaximumSize = new Size(1366, 768);
            OldXSize = this.Width;
            OldYSize = this.Height;
            this.WindowState = FormWindowState.Maximized;
            for (int i = 0; i < cMain.DataShow; i++)
            {
                ShowTitle[i] = new Label();
                ShowData[i] = new Label();
            }
            for (int i = 0; i < cModeSet.StepCount; i++)
            {
                ShowButton[i] = new Button();
            }
            initData();
            initFrm();
            this.SizeChanged += new System.EventHandler(this.FrmAutoTest_SizeChanged);
            initComAndUdp();
            cMain.mUdp.IsSetSendBarFrm = true;
            ChkPrint.Checked = cMain.isPrint;
            chkBarStart.Checked = cMain.isAutoStart;
        }
        private void initData()
        {
            loopCon1.StationCount = cMain.AllCount;
            mJiQi = cMain.JiQiStr.Split(',');
            LblTestNo.Text = "";
            LblBar.Text = "";
            lblStep.Text = "";
            lblResult.Text = "";
            LblId.Text = "";
            LblMode.Text = "";
            lblJiQi.Text = "";
            //mAiNuoData.mAiNuo9641Data = new cAiNuo9641B.AiNuo9641StepData[7];
        }
        private void initComAndUdp()
        {
            //thReadBar = new Thread(new ThreadStart(ReadBar));
            //thReadBar.IsBackground = true;
            //thReadBar.Start();
            //thReadICCard = new Thread(new ThreadStart(ReadICCard));
            //thReadICCard.IsBackground = true;
            //thReadICCard.Start();
            TimeData.Enabled = true;
            thGetData = new Thread(new ThreadStart(SendD));
            thGetData.IsBackground = true;
            thGetData.Start();
        }
        private void ReadBar()
        {
            bool isOpen = false;
            comBar = new SerialPort(cMain.comBar, 9600, Parity.None, 8, StopBits.One);
            try
            {
                if (comBar.IsOpen)
                {
                    comBar.Close();
                }
                comBar.Open();
                isOpen = comBar.IsOpen;
            }
            catch
            {
                isOpen = false;
                MsgBox("条码串口打开失败");
            }
            mBar = new cBar(comBar, 400);
            while (!isCloseWindows && isOpen)
            {
                if (!mBar.readBarCode(ref ReadBarCode))
                {
                    readBarErr++;
                    if (readBarErr > 3)
                    {
                        MsgBox("读取条码失败");
                    }
                }
                else
                {
                    if (ReadBarCode != "")
                    {
                        ReadBarCode = Num.trim(ReadBarCode);
                        SetLabelText(lblNowBar, "当前条码:" + ReadBarCode);
                        if (ReadBarCode != "")
                        {
                            SendBarMethod(ReadBarCode);
                        }
                        readBarErr = 0;
                        SaveBarCode = ReadBarCode;
                        ReadBarCode = "";
                    }
                }
                Thread.Sleep(1000);
            }
        }
        private void ReadICCard()
        {
            int tmpBuffCount = 0;
            byte[] tmpReadBuff;
            string tmpReadICValue="";
            bool isOpen = false;
            comICCard = new SerialPort(cMain.comICCard, 9600, Parity.None, 8, StopBits.One);
            try
            {
                if (comICCard.IsOpen)
                {
                    comICCard.Close();
                }
                comICCard.Open();
                isOpen = comICCard.IsOpen;
            }
            catch
            {
                isOpen = false;
                MsgBox("IC卡串口打开失败");
            }
            while (!isCloseWindows && isOpen)
            {
                if (comICCard.BytesToRead > 5)
                {
                    Thread.Sleep(200);
                    tmpBuffCount = comICCard.BytesToRead;
                    tmpReadBuff = new byte[tmpBuffCount];
                    comICCard.Read(tmpReadBuff, 0, tmpBuffCount);
                    tmpReadICValue = Num.trim(Encoding.ASCII.GetString(tmpReadBuff, 0, tmpBuffCount));
                    SetLabelText(lblNowICCard, "当前卡号:" + tmpReadICValue);
                    for (int i = 0; i < cMain.TestIcCardNum.Length; i++)
                    {
                        if (tmpReadICValue.IndexOf(Num.trim(cMain.TestIcCardNum[i])) >= 0)
                        {
                            cMain.mUdp.IndexAtBar = i;
                            SetLabelText(lblNowTestNo, "当前小车:" + string.Format("{0}", i + 1));
                            break;
                        }
                    }
                }
                Thread.Sleep(500);
            }
        }
        private void SendD()
        {
            while (!isCloseWindows)
            {
                int sleepTime = 1000 / cMain.AllCount;
                for (int i = 0; i < cMain.AllCount; i++)
                {
                    cMain.mUdp.McgsUdp[i].fUdpSend("D");
                    Thread.Sleep(sleepTime);
                    if ((Environment.TickCount - cSendData.lastGetDataTime[i]) > 7000)
                    {
                        cMain.isUdpInitError[i] = false;
                    }
                }
            }
        }
        private void closeAndUdp()
        {
            isCloseWindows = true;
            Thread.Sleep(500);
            if (comBar != null)
            {
                comBar.Close();
            }
            if (comICCard != null)
            {
                comICCard.Close();
            }
            if (thReadBar != null)
            {
                if (thReadBar.ThreadState == ThreadState.Running)
                {
                    thReadBar.Abort();
                }
                thReadBar = null;
            }
            if (thReadICCard != null)
            {
                if (thReadICCard.ThreadState == ThreadState.Running)
                {
                    thReadICCard.Abort();
                }
                thReadICCard = null;
            }
            if (thGetData != null)
            {
                if (thGetData.ThreadState == ThreadState.Running)
                {
                    thGetData.Abort();
                }
                thGetData = null;
            }
            if (thSendBar != null)
            {
                if (thSendBar.ThreadState == ThreadState.Running)
                {
                    thSendBar.Abort();
                }
                thSendBar = null;
            }
        }
        private void initFrm()//窗体大小初始化
        {
            xSize = this.Width / OldXSize;
            ySize = this.Height / OldYSize;
            OldXSize = this.Width;
            OldYSize = this.Height;
            initControl(PanBot.Controls);
            //initControl(PanTop.Controls);
            PanSelectCheck.Width = btnNow.Width + 10;
            PanSelectCheck.Height = btnNow.Height + 10;
            PanSelectCheck.Left = btnNow.Left - 5;
            PanSelectCheck.Top = btnNow.Top - 5;
            Font font = new Font(cMain.privateFontsShaoNv.Families[0], 50);
            lblTitle.Font = new Font(font, FontStyle.Bold);
            lblTitle.Text = cMain.Title;
            lblTitle.Left = (panel1.Width - lblTitle.Width) / 2;
            initShowData();
        }
        private void initShowData()//数据显示区域初始化
        {
            int i;
            int rowCount = 0;
            int labelWidth, labelHeight;
            string[] tempStr;
            rowCount = (int)Math.Ceiling((double)cMain.DataShow / 2);
            labelWidth = (int)((double)(Screen.PrimaryScreen.WorkingArea.Width - 10 - 10) * 16.000 / (17.000 * rowCount));
            labelHeight = (PanMid.Height - 10 - 10) / 4;
            tempStr = cMain.DataShowTitleStr.Split(',');
            for (i = 0; i < cModeSet.StepCount; i++)
            {
                PanBot.Controls.Add(ShowButton[i]);
                ShowButton[i].Location = new Point((btnStep.Left - btnNow.Left) * (i + 1) / cModeSet.StepCount + btnNow.Left, btnNow.Top);
                ShowButton[i].Size = new Size(btnNow.Width, btnNow.Height);
                ShowButton[i].Tag = i.ToString();
                ShowButton[i].Text = "N/A";
                ShowButton[i].TabIndex = i + 2;
                ShowButton[i].Click += new System.EventHandler(this.btnNow_Click);
                ShowButton[i].UseVisualStyleBackColor = true;
            }
            PanBot.Controls.Add(this.PanSelectCheck);
            for (i = 0; i < cMain.DataShow; i++)
            {
                ShowTitle[i].Width = labelWidth;
                ShowTitle[i].Height = labelHeight - 10;
                ShowTitle[i].BackColor = Color.FromArgb(192, 255, 192);
                ShowTitle[i].Text = tempStr[i];
                ShowTitle[i].TextAlign = ContentAlignment.MiddleCenter;
                ShowTitle[i].Font = new Font("宋体", 10, FontStyle.Regular);
                if (i < rowCount)
                {
                    ShowTitle[i].Location = new Point(10 + (labelWidth * 17 / 16) * i, 10);
                }
                else
                {
                    ShowTitle[i].Location = new Point(10 + (labelWidth * 17 / 16) * (i - rowCount), 10 + labelHeight * 2);
                }
                PanMid.Controls.Add(ShowTitle[i]);

                ShowData[i].Width = labelWidth;
                ShowData[i].Height = labelHeight - 10;
                ShowData[i].BackColor = Color.FromArgb(192, 255, 192);
                ShowData[i].TextAlign = ContentAlignment.MiddleCenter;
                ShowData[i].Font = new Font("宋体", 10, FontStyle.Regular);
                if (i < rowCount)
                {
                    ShowData[i].Location = new Point(10 + (labelWidth * 17 / 16) * i, 10 + labelHeight);
                }
                else
                {
                    ShowData[i].Location = new Point(10 + (labelWidth * 17 / 16) * (i - rowCount), 10 + labelHeight * 3);
                }
                PanMid.Controls.Add(ShowData[i]);
            }
        }
        private void initControl(Control.ControlCollection cc)//控件大小初始化
        {
            IEnumerator ie = cc.GetEnumerator();
            while (ie.MoveNext())
            {
                if (ie.Current.GetType() == typeof(Button))
                {
                    Button NowControl = (Button)ie.Current;
                    NowControl.Left = (int)(NowControl.Left * xSize);
                    NowControl.Width = (int)(NowControl.Width * xSize);
                    NowControl.Top = (int)(NowControl.Top * ySize);
                    NowControl.Height = (int)(NowControl.Height * ySize);
                }
                if (ie.Current.GetType() == typeof(Panel))
                {
                    Panel NowControl = (Panel)ie.Current;
                    NowControl.Left = (int)(NowControl.Left * xSize);
                    NowControl.Width = (int)(NowControl.Width * xSize);
                    NowControl.Top = (int)(NowControl.Top * ySize);
                    NowControl.Height = (int)(NowControl.Height * ySize);
                }
                if (ie.Current.GetType() == typeof(Label))
                {
                    Label NowControl = (Label)ie.Current;
                    NowControl.Left = (int)(NowControl.Left * xSize);
                    NowControl.Width = (int)(NowControl.Width * xSize);
                    NowControl.Top = (int)(NowControl.Top * ySize);
                    NowControl.Height = (int)(NowControl.Height * ySize);
                }
            }

        }
        private void btnNow_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            LookStep = Num.IntParse(btn.Tag);
            PanSelectCheck.Left = btnNow.Left - 5 + (btnStep.Left - btnNow.Left) * (LookStep + 1) / cModeSet.StepCount;
            ShowTestData(LookIndex, LookStep);
        }
        private void FrmAutoTest_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal || this.WindowState == FormWindowState.Maximized)
            {
                initFrm();
            }
        }
        public void SendBarMethod(string bar)
        {
            cSendBar cs = new cSendBar(bar);
            cs.sb = new SendBarHandle(SendSet);
            thSendBar = new Thread(new ThreadStart(cs.SendBarFunction));
            thSendBar.IsBackground = true;
            thSendBar.Start();
        }
        private  void SendSet(string bar)
        {
            string modeId = "";
            string ErrorStr;
            cMain.mNetModeSet.mBar = bar;
            cMain.mNetModeSet.isStart = cMain.isAutoStart;
            if (cData.GetIdFromBar(bar, ref modeId, out ErrorStr))
            {
                cMain.mNetModeSet.ModeSet = cModeSet.Read(modeId);
                if (modeId != cMain.mNetModeSet.ModeSet.mId)
                {
                    MsgBox(string.Format("没有找到机型代号:{0},指定的设置", modeId));
                    return;
                }
                bool isSendOk = false;
                for (int i = 0; i < 3; i++)
                {
                    cMain.mUdp.Is_B_OK[cMain.mUdp.IndexAtBar] = false;
                    cMain.mUdp.McgsUdp[cMain.mUdp.IndexAtBar].fUdpSend(cMain.mNetModeSet.GetStr());
                    bool isTimeOut = false;
                    long StartTime = Environment.TickCount;
                    do
                    {
                        if (cMain.mUdp.Is_B_OK[cMain.mUdp.IndexAtBar])
                        {
                            isSendOk = true;
                            break;
                        }
                        if ((Environment.TickCount - StartTime) > 2000)
                        {
                            isTimeOut = true;
                        }
                    }
                    while (!isSendOk && !isTimeOut);
                    if (isSendOk)
                    {
                        break;
                    }
                }
                if (!isSendOk)
                {
                    MsgBox("数据发送失败,网络不通或者网络不好");
                }
            }
            else
            {
                MsgBox(ErrorStr);
            }
            if (thSendBar != null)
            {
                thSendBar.Abort();
                thSendBar = null;
            }
        }
        public void MsgBox(string MsgStr)
        {
            if (lstError.InvokeRequired)
            {
                lstError.BeginInvoke(new EventHandler(msgShowText), MsgStr);
            }
            else
            {
                lstError.Items.Add(MsgStr);
            }
            if (chkAutoRun.Checked)
            {
                lstError.SelectedIndex = lstError.Items.Count - 1;
            }
        }
        private void msgShowText(object sender, EventArgs e)
        {
            lstError.Items.Add(sender.ToString());
        }

        private void loopCon1_LabelClick(object sender, EventArgs e)
        {
            Label che = (Label)sender;
            LookIndex = Num.IntParse(che.Tag);
            if (cMain.isUdpInitError[LookIndex])
            {
                ShowTestData(LookIndex, LookStep);
            }
            else
            {
                MsgBox((LookIndex+1).ToString()+"号没有连接失败,不能显示数据");
            }
        }
        private void ShowTestData(int index, int step)
        {
            if (step < 0)
            {
                isLookCurrentStep = true;
            }
            else
            {
                isLookCurrentStep = false;
                for (int i = 0; i < cMain.DataShow; i++)
                {
                    ShowData[i].Text = cMain.mTempNetResult[index].StepResult[step].mData[i].ToString();
                    ShowData[i].BackColor = cMain.mTempNetResult[index].StepResult[step].mIsDataPass[i] == 0 ? Color.Red : Color.Green;
                }
            } 
        }

        private void TimeData_Tick(object sender, EventArgs e)
        {
            lblTotle.Text = cMain.mTodayData.TestCount[0].ToString();
            lblPass.Text = cMain.mTodayData.TestCount[1].ToString();
            lblNoPass.Text = cMain.mTodayData.TestCount[2].ToString();

            LblTestNo.Text = string.Format("{0}", LookIndex + 1);
            LblBar.Text = cMain.mCurrentData[LookIndex].RunResult.mBar;

            if (cMain.mCurrentData[LookIndex].RunResult.mStepId == cModeSet.StepCount)
            {
                lblStep.Text = "收氟";
            }
            else
            {
                lblStep.Text = cMain.mCurrentData[LookIndex].RunResult.mStep;
            }
            lblResult.Text = cMain.mCurrentData[LookIndex].RunResult.mIsPass ? "OK" : "NG";
            LblId.Text = cMain.mCurrentData[LookIndex].RunResult.mId;
            LblMode.Text = cMain.mCurrentData[LookIndex].RunResult.mMode;
            lblJiQi.Text = mJiQi[cMain.mCurrentData[LookIndex].RunResult.mJiQi];

            if (isLookCurrentStep)
            {
                for (int i = 0; i < cMain.DataShow; i++)
                {
                    ShowData[i].Text = cMain.mCurrentData[LookIndex].StepResult.mData[i].ToString();
                    ShowData[i].BackColor = cMain.mCurrentData[LookIndex].StepResult.mIsDataPass[i] == 0 ? Color.Pink : Color.White;
                }
            }
            //loopCon1.labelInit(cMain.mUdp.IndexAtBar);
            for (int i = 0; i < cMain.AllCount; i++)
            {
                if (cMain.mCurrentData[i].RunResult.mStepId == -1)
                {
                    for (int j = 0; j < cModeSet.StepCount; j++)
                    {
                        for(int k=0;k<cMain.DataShow;k++)
                        {
                            cMain.mTempNetResult[i].StepResult[j].mData[k] = 0;
                            cMain.mTempNetResult[i].StepResult[j].mIsDataPass[k] = -1;
                        }
                        cMain.mTempNetResult[i].StepResult[j].mIsStepPass = -1;
                        cMain.mTempNetResult[i].RunResult[j].mStep = "N/A";
                    }
                }
                if (cMain.isUdpInitError[i])
                {
                    if(cMain.mCurrentData[i].RunResult.mStepId>=0)
                    {
                        if (cMain.mCurrentData[i].RunResult.mIsPass)
                        {
                            loopCon1.labelChangeColor(i, Color.LightSeaGreen);
                        }
                        else
                        {
                            loopCon1.labelChangeColor(i, Color.Pink);
                        }
                    }
                    else
                    {
                        loopCon1.labelChangeColor(i, Color.White);
                    }
                }
                else
                {
                    loopCon1.labelChangeColor(i, Color.LightSteelBlue);
                }
            }
            for (int i = 0; i < cModeSet.StepCount; i++)
            {

                if (cMain.mCurrentData[LookIndex].RunResult.mStepId == -1)
                {
                    ShowButton[i].Text = "N/A";
                    ShowButton[i].BackColor = Color.White;
                }
                if (i < cMain.mCurrentData[LookIndex].RunResult.mStepId)
                {
                    ShowButton[i].Text = cMain.mTempNetResult[LookIndex].RunResult[i].mStep;
                    if (cMain.mTempNetResult[LookIndex].RunResult[i].mStep == "")
                    {
                        ShowButton[i].BackColor = Color.White;
                    }
                    else
                    {
                        ShowButton[i].BackColor = cMain.mTempNetResult[LookIndex].StepResult[i].mIsStepPass == 0 ? Color.Pink : Color.White;
                    }
                }
                if (i == cMain.mCurrentData[LookIndex].RunResult.mStepId)
                {
                    if (LookStep == i)
                    {
                        for (int j = 0; j < cMain.DataShow; j++)
                        {
                            ShowData[j].Text = cMain.mCurrentData[LookIndex].StepResult.mData[j].ToString();
                            ShowData[j].BackColor = cMain.mCurrentData[LookIndex].StepResult.mIsDataPass[j] == 0 ? Color.Pink : Color.White;
                        }
                    }
                    ShowButton[i].Text = cMain.mCurrentData[LookIndex].RunResult.mStep;
                }
                if (i > cMain.mCurrentData[LookIndex].RunResult.mStepId)
                {
                    ShowButton[i].Text = "N/A";
                    ShowButton[i].BackColor = Color.White;
                }
            }      
        }

        private void FrmAutoTest_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.WindowState = FormWindowState.Minimized;
        }
        private void FrmAutoTest_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F10)
            {
                lstError.Items.Clear();
            }
        }

        private void ChkPrint_CheckedChanged(object sender, EventArgs e)
        {
            cMain.isPrint = ChkPrint.Checked;
        }

        private void chkBarStart_CheckedChanged(object sender, EventArgs e)
        {
            cMain.isAutoStart = chkBarStart.Checked;
        }
    }
    class cSendBar
    {
        string _barCode;
        public SendBarHandle sb;
        public cSendBar(string barCode)
        {
            _barCode = barCode;
        }
        public void SendBarFunction()
        {
            sb(_barCode);
        }
    }
}