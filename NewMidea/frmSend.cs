using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
namespace NewMideaProgram
{
    public delegate void SendText(int startId, int endId);

    public partial class frmSend : Form
    {
        cModeSet modeSet;
        public frmSend(cModeSet mModeSet)
        {
            modeSet = mModeSet;
            InitializeComponent();
        }
        bool is_C_Ok = false;
        bool is_P_Ok = false;
        bool is_B_Ok = false;
        bool is_TimeOut = false;
        long timeOut = 1500;
        cUdpSock mUdp;
        tempClass tc;
        Thread SendThread;
        private void frmSend_Load(object sender, EventArgs e)
        {
            int i;
            cbbStart.Items.Clear();
            cbbEnd.Items.Clear();
            for (i = 101; i < cMain.AllCount + 101; i++)
            {
                cbbStart.Items.Add(i);
                cbbEnd.Items.Add(i);
            }
            cbbStart.SelectedIndex = 0;
            cbbEnd.SelectedIndex = cbbEnd.Items.Count - 1;

            frmInit();

        }
        private void frmInit()
        {
            if (!cMain.isComPuter)
            {
                this.Height = Screen.PrimaryScreen.Bounds.Height;
                this.Width = Screen.PrimaryScreen.Bounds.Width;
            }
            cMain.initFrom(this.Controls);
            btnStop.Enabled = false;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            if (mUdp != null)
            {
                mUdp.fUdpClose();
                mUdp = null;
            }
            if (SendThread != null)
            {
                SendThread.Abort();
                SendThread = null;
            }
            tc = null;
            this.Close();
            this.Dispose();

        }
        delegate void SetBtnEnableHandle(ToolStripButton sender, bool value);
        private void SetBtnEnable(ToolStripButton sender, bool value)
        {
            if (toolStrip1.InvokeRequired)
            {
                toolStrip1.Invoke(new SetBtnEnableHandle(SetBtnEnable),sender, value);
            }
            else
            {
                sender.Enabled = value;
            }
        }
        private void listAdditem(object sender, EventArgs e)
        {
            string itemStr = sender.ToString();
            if (itemStr == "Clear")
            {
                listBox1.Items.Clear();
            }
            else
            {
                listBox1.Items.Add(itemStr);
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
            }
        }
        private void SendModeSet(int startId, int endId)
        {
            SetBtnEnable(btnSend,false);
            //btnSend.Enabled = false;// Invoke(new EventHandler(isEnableSendButton), "false");
            int i;// startId, endId;
            string remotHost = "  ";
            string[] tempStr;
            //startId = Convert.ToInt16(readText(cbbStart));
            //endId = Convert.ToInt16(readText(cbbEnd));
            mUdp = new cUdpSock(10000);
            mUdp.mDataReciveString += new cUdpSock.mDataReciveStringEventHandle(mUdp_mDataReciveString);
            string ModeStr = "";
            //string SnStr = "";
            //SnStr = DataClassToStr(cSnSet.DataXmlToClass(modeSet.mId));
            ModeStr = DataClassToStr(modeSet);
            tempStr = cMain.RemoteHostName.Split('.');
            if (tempStr.Length == 4)
            {
                remotHost = tempStr[0] + "." + tempStr[1] + "." + tempStr[2] + ".";
            }
            long startTime = 0;
            listBox1.BeginInvoke(new EventHandler(listAdditem), "Clear");
            for (i = startId; i <= endId; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    is_P_Ok = false;
                    mUdp.fUdpSend(remotHost + i.ToString(), 3000, "P");
                    listBox1.BeginInvoke(new EventHandler(listAdditem), string.Format("正在第{0}次连接{1}号小车", j+1, remotHost + i.ToString()));
                    startTime = Environment.TickCount;
                    is_TimeOut = false;
                    do
                    {
                        //Application.DoEvents();
                        Thread.Sleep(20);
                        if (Environment.TickCount - startTime > timeOut)
                        {
                            is_TimeOut = true;
                        }
                    } while (!is_P_Ok && !is_TimeOut);
                    if (is_P_Ok)
                    {
                        j = 3;
                        listBox1.BeginInvoke(new EventHandler(listAdditem), "     " + remotHost + i.ToString() + "号连接成功,开始发送数据....");

                        for (int k = 0; k < 3; k++)
                        {
                            is_B_Ok = false;
                            mUdp.fUdpSend(remotHost + i.ToString(), 3000, ModeStr);
                            startTime = Environment.TickCount;
                            is_TimeOut = false;
                            do
                            {
                                //Application.DoEvents();
                                Thread.Sleep(20);
                                if (Environment.TickCount - startTime > timeOut)
                                {
                                    is_TimeOut = true;
                                }
                            } while (!is_B_Ok && !is_TimeOut);
                            if (is_B_Ok)
                            {
                                k = 3;
                                listBox1.BeginInvoke(new EventHandler(listAdditem), "               " + remotHost + i.ToString() + "号检测数据发送成功....");
                            }
                            else
                            {
                                listBox1.BeginInvoke(new EventHandler(listAdditem), "     " + remotHost + i.ToString() + "号检测数据发送失败..................");
                            }
                        }
                        Thread.Sleep(100);
                        //for (int k = 0; k < 3; k++)
                        //{
                        //    is_C_Ok = false;
                        //    mUdp.fUdpSend(remotHost + i.ToString(), 3000, SnStr);
                        //    startTime = Environment.TickCount;
                        //    is_TimeOut = false;
                        //    do
                        //    {
                        //        //Application.DoEvents();
                        //        Thread.Sleep(20);
                        //        if (Environment.TickCount - startTime > timeOut)
                        //        {
                        //            is_TimeOut = true;
                        //        }
                        //    } while (!is_C_Ok && !is_TimeOut);
                        //    if (is_C_Ok)
                        //    {
                        //        k = 3;
                        //        listBox1.BeginInvoke(new EventHandler(listAdditem), "               " + remotHost + i.ToString() + "号指令数据发送成功....");
                        //    }
                        //    else
                        //    {
                        //        listBox1.BeginInvoke(new EventHandler(listAdditem), "     " + remotHost + i.ToString() + "号指令数据发送失败..................");
                        //    }
                        //}
                    }
                    else
                    {
                        listBox1.BeginInvoke(new EventHandler(listAdditem), string.Format("{0}号第{1}次连接失败,无法发送数据....................", remotHost + i.ToString(),j+1));
                    }
                }
            }
            SetBtnEnable(btnSend, true);
            SetBtnEnable(btnStop, false);
            //btnSend.BeginInvoke(new EventHandler(isEnableSendButton), "true");
            if (mUdp != null)
            {
                mUdp.fUdpClose();
                mUdp = null;
            }
        }
        private void btnSend_Click(object sender, EventArgs e)
        {
            btnSend.Enabled = false;
            tc = new tempClass(Convert.ToInt16(cbbStart.Text), Convert.ToInt16(cbbEnd.Text));
            tc.st = new SendText(SendModeSet);
            SendThread = new Thread(new ThreadStart(tc.send));
            SendThread.IsBackground = true;
            SendThread.Start();
            btnStop.Enabled = true;
        }
        private void mUdp_mDataReciveString(object o, RecieveStringArgs e)
        {
            if (e.ReadStr == "P~OK")
            {
                is_P_Ok = true;
            }
            if (e.ReadStr == "B~OK")
            {
                is_B_Ok = true;
            }
            if (e.ReadStr == "C~OK")
            {
                is_C_Ok = true;
            }
        }
        //private string DataClassToStr(cSnSet snSet)
        //{
        //    string SendStr = "C~";
        //    SendStr += cSnSet.DataClassToStr(snSet);
        //    return SendStr;
        //}
        private string DataClassToStr(cModeSet modeSet)
        {
            string SendStr = "B~~0~";
            string str;
            frmSet.DataClassToFile(modeSet, out str);
            SendStr = SendStr + str;
            return SendStr;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            btnStop.Enabled = false;
            if (mUdp != null)
            {
                mUdp.fUdpClose();
                mUdp = null;
            }
            if (SendThread != null)
            {
                SendThread.Abort();
                SendThread = null;
            }
            tc = null;
            btnSend.Enabled = true;
        }
    }
    public class tempClass
    {
        public SendText st;
        int _StartId, _EndId;
        public tempClass(int startId, int endId)
        {
            _EndId = endId;
            _StartId = startId;
        }
        public void send()
        {
            st(_StartId, _EndId);
        }
    }
}