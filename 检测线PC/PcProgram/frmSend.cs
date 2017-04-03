using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
namespace PcProgram
{
    public delegate void sendText(int startId,int stopId);
    public partial class frmSend : Form
    {
        tempClass tc;
        public enum SendValue
        {
            SendSystem=0,
            SendMode=1,
            SendStop=2,
            SendStart=3,
            SendNext=4,
            SendUpdata=5,
            SendShutDown=6
        }
        string _sendStr = "";
        SendValue _sendTitle;
        Thread thsend;
        public string SendStr
        {
            get { return _sendStr; }
            set { _sendStr = value; }
        }
        public frmSend(string sendStr,SendValue title)
        {
            _sendStr = sendStr;
            _sendTitle = title;
            InitializeComponent();
        }
        private void initSystemSend(bool isSystemUser)
        {
            if (isSystemUser)
            {
                cbbMode.Items.Clear();
                string sqlStr = "select id from mode";
                DataSet ds = cData.readData(sqlStr, cData.ConnMain);
                if(ds.Tables[0].Rows.Count>0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        cbbMode.Items.Add(ds.Tables[0].Rows[i][0]);
                    }
                }
            }
            else
            {
                panel2.Height = 0;
            }
        }
        private void frmSend_Load(object sender, EventArgs e)
        {
            initSystemSend(cMain._isSystemUser);
            string title = "";
            for(int i=1;i<=cMain.AllCount;i++)
            {
                comboBox1.Items.Add(i);
                comboBox2.Items.Add(i);
            }
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = comboBox2.Items.Count - 1;
            switch (_sendTitle)
            {
                case SendValue.SendMode:
                    rbbMode.Checked = true;
                    title = "参数设置";
                    break;
                case SendValue.SendNext:
                    rbbNext.Checked = true;
                    title = "下一步命令";
                    break;
                case SendValue.SendStart:
                    rbbStart.Checked = true;
                    title = "开机命令";
                    break;
                case SendValue.SendStop:
                    rbbStop.Checked = true;
                    title = "停机命令";
                    break;
                case SendValue.SendSystem:
                    rbbSys.Checked = true;
                    title = "系统设置";
                    break;
                case SendValue.SendUpdata:
                    rbbUpdata.Checked = true;
                    title = "系统更新命令";
                    break; 
                case SendValue.SendShutDown:
                    rbbShutDown.Checked = true;
                    title = "关机命令";
                    break;
            }
            gbSend.Text = "发送" + title;
            this.Text = "发送" + title;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (thsend != null)
            {
                thsend.Abort();
                thsend = null;
            }
            tc = null;
            this.Close();
        }
        private void listAdditem(string str)
        {
            if (listBox1.InvokeRequired)
            {
                listBox1.BeginInvoke(new EventHandler(addItem), str);
            }
            else
            {
                listBox1.Items.Add(str);
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
            }
        }
        private void addItem(object sender, EventArgs e)
        {
            listBox1.Items.Add(sender.ToString());
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
        }
        private void isEnableSendButton(object sender, EventArgs e)
        {
            bool isEnable = bool.Parse(sender.ToString());
            if (isEnable)
            {
                btnSend.Enabled = true;
            }
            else
            {
                btnSend.Enabled = false;
            }
        }
        private void btnSend_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            int startNum, stopNum, minNum;
            startNum = Num.IntParse(comboBox1.Text);
            stopNum = Num.IntParse(comboBox2.Text);
            if (stopNum <startNum)
            {
                minNum = startNum; startNum = stopNum; stopNum = minNum;
            }
            btnSend.Enabled = false;
            tc = new tempClass(startNum, stopNum);
            tc.st = new sendText(SendTextToMcgs);
            thsend = new Thread(new ThreadStart(tc.SendStrToMcgs));
            thsend.IsBackground = true;
            thsend.Start();
            btnStop.Enabled = true;
        }
        private void SendTextToMcgs(int startNum, int stopNum)
        {
            int i,j;
            for (i = startNum - 1; i < stopNum; i++)
            {
                bool isOk = false;
                listAdditem("开始往  " + (i+1).ToString() + "  号发送数据....");
                for (j = 0; j < 3; j++)
                {
                    listAdditem("    开始尝试第" + (j + 1).ToString() + "发送数据");
                    switch (_sendTitle)
                    {
                        case SendValue.SendMode:
                            cMain.mUdp.Is_B_OK[i] = false;
                            break;
                        case SendValue.SendNext:
                            cMain.mUdp.Is_N_OK = false;
                            break;
                        case SendValue.SendStart:
                            cMain.mUdp.Is_K_OK = false;
                            break;
                        case SendValue.SendStop:
                            cMain.mUdp.Is_T_OK = false;
                            break;
                        case SendValue.SendSystem:
                            cMain.mUdp.Is_S_OK = false;
                            break;
                        case SendValue.SendUpdata:
                            cMain.mUdp.Is_U_OK = false;
                            break;
                        case SendValue.SendShutDown:
                            cMain.mUdp.Is_E_Ok = false;
                            break;
                    }
                    cMain.mUdp.McgsUdp[i].fUdpSend(_sendStr);
                    long startTime=Environment.TickCount;
                    bool isTimeOut=false;
                    do
                    {
                        switch (_sendTitle)
                        {
                            case SendValue.SendMode:
                                if (cMain.mUdp.Is_B_OK[i])
                                {
                                    isOk = true;
                                }
                                break;
                            case SendValue.SendNext:
                                if (cMain.mUdp.Is_N_OK)
                                {
                                    isOk = true;
                                }
                                break;
                            case SendValue.SendStart:
                                if (cMain.mUdp.Is_K_OK)
                                {
                                    isOk = true;
                                }
                                break;
                            case SendValue.SendStop:
                                if (cMain.mUdp.Is_T_OK)
                                {
                                    isOk = true;
                                }
                                break;
                            case SendValue.SendSystem:
                                if (cMain.mUdp.Is_S_OK)
                                {
                                    isOk = true;
                                }
                                break;
                            case SendValue.SendUpdata:
                                if (cMain.mUdp.Is_U_OK)
                                {
                                    isOk = true;
                                }
                                break;
                            case SendValue.SendShutDown:
                                if (cMain.mUdp.Is_E_Ok)
                                {
                                    isOk = true;
                                }
                                break;
                        }

                        if (Environment.TickCount - startTime > 500)
                        {
                            isTimeOut = true;
                        }
                    } while (!isTimeOut && !isOk);
                    if (isOk)
                    {
                        break;
                    }
                    if (isTimeOut)
                    {
                        listAdditem("     第" + (j + 1).ToString() + "发送数据超时");
                    }
                }
                if (isOk)
                {
                    listAdditem("     数据发送成功....");
                }
                else
                {
                    listAdditem("     " + (i+1).ToString() + "号发送数据失败,请检查网络或重新发送....");
                }

            }
            btnSend.BeginInvoke(new EventHandler(isEnableSendButton), "true");
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            btnStop.Enabled = false;
            if (thsend != null)
            {
                thsend.Abort();
                thsend = null;
            }
            tc = null;
            btnSend.Enabled = true;
        }

        private void rbbStart_CheckedChanged(object sender, EventArgs e)
        {
            string title = ""; ;
            RadioButton cb = (RadioButton)sender;
            int pressCb = Num.IntParse(cb.Tag);
            if (pressCb == 4)
            {
                cbbMode.Enabled = true;
            }
            else
            {
                cbbMode.Enabled = false;
            }
            switch (pressCb)
            {
                case 0:
                    title = "开机命令";
                    _sendStr = "K";
                    _sendTitle = SendValue.SendStart;
                    break;
                case 1:
                    title = "下一步命令";
                    _sendStr = "N";
                    _sendTitle = SendValue.SendNext;
                    break;
                case 2: 
                    title = "停机命令";
                    _sendStr = "T";
                    _sendTitle = SendValue.SendStop;
                    break;
                case 3:
                    title = "系统设置";
                    _sendTitle = SendValue.SendSystem;
                    break;
                case 4: 
                    title = "参数设置";
                    _sendTitle = SendValue.SendMode;
                    break;
                case 5:
                    title = "系统更新命令";
                    _sendStr = "U";
                    _sendTitle=SendValue.SendUpdata;
                    break;
                case 6:
                    title = "关机命令";
                    _sendStr = "E";
                    _sendTitle = SendValue.SendShutDown;
                    break;
            }
            gbSend.Text = "发送" + title;
            this.Text = "发送" + title;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
    class tempClass
    {
        public sendText st;
        int _startId, _stopId;
        public tempClass(int startId, int stopId)
        {
            _startId = startId;
            _stopId = stopId;
        }
        public void SendStrToMcgs()
        {
            st(_startId, _stopId);
        }
    }
}