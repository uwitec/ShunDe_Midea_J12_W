using System;
using System.Collections.Generic;
using System.ComponentModel;
//using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Threading;
using System.IO.Ports;
using System.Net.Sockets;
using System.IO;
using System.Diagnostics;
using System.Data;
namespace NewMideaProgram
{
    public delegate bool GetCheckBoxValueHandle(CheckBox chk);
    public partial class frmMain : Form
    {
        cLowVolOneByOne.Client lowVolOneByOne;
        frmPassWord fPassWord;
        Chanel[] chanelCurve = new Chanel[4];//电流,功率,压力,温差
        public static cMain mMain = new cMain();
        public static cMain.NowStatue nowStatue = new cMain.NowStatue();
        int Read2010Err = 0;//, ReadWenDuErr2 = 0, ReadWenDuErr3 = 0, ReadWenDuErr4 = 0, ReadWenDuErr5 = 0;
        int ReadPlcErr = 0, readDataBarErr = 0;
        long HighElectErr = 0, LowElectErr = 0, HighPresErr = 0, LowPresErr = 0;
        int UpdataCount = 0;//网络更新按键计数
        byte[] realData = new byte[1024];
        public static bool[] ReadPlcMValue = new bool[160];
        cUdpSock udpSend;
        bool is_R_OK = false, is_J_OK = false;
        long PreFlushDataTime = 0;
        public string ErrStr = "";//出错信息
        bool IsOutSystem = false;//是否要退出程序
        bool IsStartOut = false;//是否完成启动输出,在等待低启时,以免重复输出 写PLC数据
        bool IsEndStepOut = false;//是否完成步骤结束控制.
        string ReadBarCode = "", ReadDataBarCode = "";
        bool Plc_Start = false, Handle_Start = false, Net_Start = false;
        bool Plc_Stop = false, Handle_Stop = false, Net_Stop = false, Pro_Stop = false, ZYSF_Stop = false;
        bool Plc_Next = false, Handle_Next = false, Net_Next = false;
        bool Plc_SF = false, Handle_SF = false;
        //PLC M点输出
        public static Dictionary<string, bool> Plc_Out_MPoint = new Dictionary<string, bool>();
        public static Dictionary<string, bool> Temp_Out_MPoint = new Dictionary<string, bool>();
        public static Dictionary<string, int> Plc_M_MPoint = new Dictionary<string, int>();
        //Plc M点读取
        public static Dictionary<string, bool> Plc_Out_MReadPoint = new Dictionary<string, bool>();
        public static Dictionary<string, bool> Temp_Out_MReadPoint = new Dictionary<string, bool>();
        public static Dictionary<string, int> Plc_M_MReadPoint = new Dictionary<string, int>();
        //plc d点输出
        public static Dictionary<string, int> Plc_Out_DPoint = new Dictionary<string, int>();
        public static Dictionary<string, bool> Temp_Out_DPoint = new Dictionary<string, bool>();
        public static Dictionary<string, int> Plc_D_DPoint = new Dictionary<string, int>();
        //要读的D点
        //电参数
        public static double[] Ft2010_Read = new double[10];//电参数
        //SN板
        public static double[] SnBoard_Show = new double[6];//SN板
        public static double[] SnBoard_Show1 = new double[6];//SN板
        public static double[] SnBoard_Show2 = new double[6];//SN板
        //压力
        public static double[] Plc_In_YaLi = new double[4];//压力
        //温度
        public static double[] Plc_In_WenDu = new double[8];//温度

        public static double[] dataRead = new double[cMain.DataAll];
        public static double[] dataShow = new double[cMain.DataShow];
        cError mError = new cError();
        cPQMachine mPQ;
        string Temp_Step_SnCode = "";
        public static int InitSnJiQi = 0;
        object fLock;
        public frmMain()//构造函数
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            StartLoad();
        }
        public void StartLoad()
        {
            //cMain.mAuto.AutoMathStatueChange += new cAuto.AutoMathStatueChangeHandle(mAuto_AutoMathStatueChange);
            //if (cMain.mAutoData.Auto)
            //{
            //    cMain.mAuto.Start();
            //}
            frmInit(this);//窗体初始化
            init();//参数初始化
            ComUdpInit();
            initTestData();
            initNowStatue();
            MainLoop.Enabled = true;
        }
        private static void frmInit(frmMain mFrmMain)//实现各控件自动位置,基本不用动
        {

            DataTable dt = new DataTable();
            mFrmMain.dataGridNow.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            for (int i = 0; i < cMain.DataShow; i++)
            {
                dt.Columns.Add(cMain.DataShowTitle[i], typeof(string));
            }
            DataRow dr = dt.NewRow();
            for (int i = 0; i < cMain.DataShow; i++)
            {
                dr[i] = "0.000";
            }
            dt.Rows.Add(dr);
            mFrmMain.dataGridNow.Font = new Font("宋体", 12);
            mFrmMain.dataGridNow.DataSource = dt;
            mFrmMain.dataGridNow.Rows[0].Height = 40;
            if (cMain.IndexLanguage == 1)
            {
                mFrmMain.dataGridNow.Rows[0].HeaderCell.Value = "数据";
            }
            else
            {
                mFrmMain.dataGridNow.Rows[0].HeaderCell.Value = "Data";
            }
            for (int i = 0; i < mFrmMain.dataGridNow.Columns.Count; i++)
            {
                mFrmMain.dataGridNow.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            mFrmMain.dataGridNow.CurrentCell = null;//将自动选择的单元去掉,被选择时,会有个自动样式,阻碍绘画背景红色
            for (int i = 0; i < mFrmMain.dataGridNow.ColumnCount; i++)
            {
                mFrmMain.dataGridNow.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                mFrmMain.dataGridNow.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                if (mFrmMain.dataGridNow.Columns[i].HeaderCell.Value.ToString().Contains("(Mpa)"))
                {
                    mFrmMain.dataGridNow.Columns[i].HeaderCell.Style.BackColor = Color.Cyan;
                    mFrmMain.dataGridNow.Columns[i].DefaultCellStyle.Font = new Font("宋体", 13, FontStyle.Bold);
                    mFrmMain.dataGridNow.Columns[i].HeaderCell.Style.Font = new Font("宋体", 14, FontStyle.Bold);
                }
            }
        }
        private void SetLabelText(Label lbl, string str)
        {
            this.CrossThreadCalls(() => lbl.Text = str);
        }
        private void SetCheckBox(CheckBox chk, bool value)
        {
            this.CrossThreadCalls(() => chk.Checked = value);
        }
        private bool GetCheckBoxValue(CheckBox chk)
        {
            bool result = false;
            if (chk.InvokeRequired)
            {
                result = (bool)chk.Invoke(new GetCheckBoxValueHandle(GetCheckBoxValue), chk);
            }
            else
            {
                result = chk.Checked;
            }
            return result;
        }
        private void FlushControlText(Label sender, string data, ErrDataEnum errDataEnum)
        {
            this.CrossThreadCalls(() =>
            {
                sender.Text = data;
                switch (errDataEnum)
                {
                    case ErrDataEnum.Info:
                        sender.ForeColor = Color.Black;
                        sender.BackColor = this.BackColor;
                        sender.Font = new Font(new Font("Times New Roman", 12), FontStyle.Regular);
                        break;
                    case ErrDataEnum.Error:
                        sender.ForeColor = Color.Red;
                        sender.BackColor = Color.LightGreen;
                        sender.Font = new Font(new Font("Times New Roman", 14), FontStyle.Bold);
                        break;
                    case ErrDataEnum.Protect:
                        sender.ForeColor = Color.Blue;
                        sender.BackColor = Color.Yellow;
                        sender.Font = new Font(new Font("Times New Roman", 14), FontStyle.Bold);
                        break;
                }
            });
        }
        private bool init()//程序开始初始化
        {
            int i;
            for (i = 0; i < dataRead.Length; i++)
            {
                dataRead[i] = -99;
            }
            for (i = 0; i < Plc_In_YaLi.Length; i++)
            {
                Plc_In_YaLi[i] = 0;
            }
            for (i = 0; i < Plc_In_WenDu.Length; i++)
            {
                Plc_In_WenDu[i] = 0;
            }
            for (i = 0; i < Ft2010_Read.Length; i++)
            {
                Ft2010_Read[i] = 0;
            }
            for (i = 0; i < cMain.DataPlcMPointStr.Length; i++)
            {
                Plc_Out_MPoint.Add(cMain.DataPlcMPointStr[i], false);
                Temp_Out_MPoint.Add(cMain.DataPlcMPointStr[i], false);
                Plc_M_MPoint.Add(cMain.DataPlcMPointStr[i], cMain.DataPlcMPointInt[i]);
            }
            for (i = 0; i < cMain.ReadPlcMPointInt.Length; i++)
            {
                Plc_Out_MReadPoint.Add(cMain.ReadPlcMPointStr[i], false);
                Temp_Out_MReadPoint.Add(cMain.ReadPlcMPointStr[i], false);
                Plc_M_MReadPoint.Add(cMain.ReadPlcMPointStr[i], cMain.ReadPlcMPointInt[i]);
            }
            for (i = 0; i < cMain.DataPlcDPointStr.Length; i++)
            {
                Plc_Out_DPoint.Add(cMain.DataPlcDPointStr[i], 0);
                Temp_Out_DPoint.Add(cMain.DataPlcDPointStr[i], false);
                Plc_D_DPoint.Add(cMain.DataPlcDPointStr[i], cMain.DataPlcDPointInt[i]);
            }
            chanelCurve[0] = new Chanel(true, Color.Red, false);//电流
            chanelCurve[1] = new Chanel(true, Color.Orange, true);//电流
            chanelCurve[2] = new Chanel(true, Color.Cyan, false);//电流
            chanelCurve[3] = new Chanel(true, Color.Pink, false);//电流

            chanelCurve[0].ChanelName = " -- 电流";
            chanelCurve[1].ChanelName = " -- 功率";
            chanelCurve[2].ChanelName = " -- 进管压力";
            chanelCurve[3].ChanelName = " -- 出管压力";

            quXianControl1.Chanel.Add(chanelCurve[0]);
            quXianControl1.Chanel.Add(chanelCurve[1]);
            quXianControl1.Chanel.Add(chanelCurve[2]);
            quXianControl1.Chanel.Add(chanelCurve[3]);
            quXianControl1.reSet();

            splitContainer1.SplitterDistance = splitContainer1.Width - 490;

            return true;
        }
        private bool ComUdpInit()//通讯初始化
        {
            bool returnValue = true;
            try
            {
                lblIP.Text = string.Format("IP:{0}", cUdpSock.LoaclIp());
                cMain.ThisNo = cUdpSock.LastIp().ToInt() % 100;
                int mRemotPort = cMain.ThisNo + 3000;
                udpSend = new cUdpSock(3000, mRemotPort, cMain.RemoteHostName);
                udpSend.fUdpSend("A~OK~OVER~" + cMain.ThisNo.ToString());
                udpSend.mDataReciveString += new cUdpSock.mDataReciveStringEventHandle(udpSend_mDataReciveString);
                if ((cMain.ThisNo % 100) == 1)
                {
                    cLowVolOneByOne.Admin admin = new cLowVolOneByOne.Admin();
                }
                lowVolOneByOne = new cLowVolOneByOne.Client(10100 + (cMain.ThisNo % 100), cMain.RemoteHostName);
            }
            catch
            {
                cMain.WriteErrorToLog("初始化UDP端口失败");
                mError.AddErrData("UDP_ERROR", "初始化UDP端口失败");//,Udp Error".Split(',')[cMain.IndexLanguage]);
                returnValue = false;
            }
            new Thread(new ThreadStart(doThreadCom1))
            {
                IsBackground = true
            }.Start();
            new Thread(new ThreadStart(doThreadCom2))
            {
                IsBackground = true
            }.Start();
            new Thread(new ThreadStart(doThreadCom3))
            {
                IsBackground = true
            }.Start();
            new Thread(new ThreadStart(doThreadCom4))
            {
                IsBackground = true
            }.Start();
            new Thread(new ThreadStart(doThreadCom5))
            {
                IsBackground = true
            }.Start();
            return returnValue;
        }
        private void udpSend_mDataReciveString(object o, RecieveStringArgs e)//UDP接收数据,(有点木马的原型 ^_^)
        {
            string GetString = e.ReadStr;
            if (GetString == string.Empty)
            {
                return;
            }
            string RemoteIp = udpSend.pRemoteHostIp;
            int RemotePort = udpSend.pRemoteHostPort;
            string RemotCmd = GetString.Substring(0, 1).ToUpper();
            switch (RemotCmd)
            {
                case "C":
                    //cSnSet.DataClassToXml(cSnSet.DataStrToClass(GetString));
                    //udpSend.fUdpSend(RemoteIp, RemotePort, "C~OK");
                    break;
                case "B"://条码与设置
                    if (DataStrToClass(GetString, out cMain.mNetModeSet))
                    {
                        if (!nowStatue.isStarting)
                        {
                            cMain.mModeSet = cMain.mNetModeSet.ModeSet;
                        }
                        udpSend.fUdpSend(RemoteIp, RemotePort, "B~OK");
                    }
                    else
                    {
                        udpSend.fUdpSend(RemoteIp, RemotePort, "B~ERROR");
                    }
                    frmSet.DataClassToFile(cMain.mNetModeSet.ModeSet);
                    break;
                case "S"://系统设置
                    if (DataStrToClass(GetString, out cMain.mSysSet))
                    {
                        udpSend.fUdpSend(RemoteIp, RemotePort, "S~OK");
                    }
                    else
                    {
                        udpSend.fUdpSend(RemoteIp, RemotePort, "S~ERROR");
                    }
                    frmSys.DataClassToFile(cMain.mSysSet);
                    break;
                case "R"://条码上传成功后,上位机会返回此标志
                    if (GetString.IndexOf("R~OK") >= 0)
                    {
                        is_R_OK = true;
                    }
                    break;
                case "J"://检测数据上传成功后,上位机会返回此标志
                    if (GetString.IndexOf("J~OK") >= 0)
                    {
                        is_J_OK = true;
                    }
                    break;
                case "X"://行程开关,这里不要了.
                    break;
                case "D"://询问当前数据
                    string sendData = "D~OK~" + DataClassToStr(cMain.mTempNetResult);
                    udpSend.fUdpSend(RemoteIp, RemotePort, sendData);
                    break;
                case "P"://WINCE没有PING命令,这相当于PING命令
                    udpSend.fUdpSend(RemoteIp, RemotePort, "P~OK");
                    break;
                case "U"://系统更新
                    this.BeginInvoke(new EventHandler(updataByNet));
                    break;
                case "T"://停止测试
                    Net_Stop = true;
                    udpSend.fUdpSend(RemoteIp, RemotePort, "T~OK");
                    break;
                case "N"://下一步
                    Net_Next = true;
                    udpSend.fUdpSend(RemoteIp, RemotePort, "N~OK");
                    break;
                case "K"://开始测试
                    Net_Start = true;
                    udpSend.fUdpSend(RemoteIp, RemotePort, "K~OK");
                    break;
                case "E"://Exit
                    udpSend.fUdpSend(RemoteIp, RemotePort, "E~OK");
                    System.Diagnostics.Process.Start("ShutDown", "-p -f");
                    Application.Exit();
                    break;
                case "Y":
                    Plc_Out_MPoint["总复位"] = true; Temp_Out_MPoint["总复位"] = true;
                    Thread.Sleep(1000);
                    System.Diagnostics.Process.Start("ShutDown", "-p -f");
                    break;
            }
        }
        private void updataByNet(object o, EventArgs e)
        {
            if (!cMain.isComPuter)
            {
                //显示任务栏
                int TaskBarHandle;
                TaskBarHandle = cAPI.FindWindowW("HHTaskBar", null);
                if (TaskBarHandle != 0)
                {
                    TaskBarHandle = cAPI.ShowWindow(TaskBarHandle, 4);//9为恢复原来
                }
            }
            udpSend.fUdpSend(udpSend.pRemoteHostIp, udpSend.pRemoteHostPort, "U~OK");
            if (File.Exists(cMain.AppPath + "\\UpdataByNet.exe"))
            {
                Process.Start(cMain.AppPath + "\\UpdataByNet.exe", "");
            }
            frmClose();
            this.Close();
            Application.Exit();
        }
        private void doThreadCom1()
        {
            bool[] readMValue = new bool[16];
            long[] readDValue = new long[20];
            bool conn = false;
            SerialPort com = new SerialPort();
            if (!cMain.isDebug)
            {
                try
                {
                    com = new SerialPort(cMain.mSysSet.mPLCCOM, 9600, Parity.Even, 7, StopBits.One);
                    if (com.IsOpen)
                    {
                        com.Close();
                    }
                    com.Open();
                }
                catch
                {
                    mError.AddErrData(cMain.mSysSet.mPLCCOM + "_ERROR", "打开通讯端口" + (cMain.mSysSet.mPLCCOM + "失败"));//,"+cMain.mSysSet.mPLCCOM+"ERROR").Split(',')[cMain.IndexLanguage]);
                }
            }
            cFxplc mFxplc = new cFxplc(com, 400);
            if (!mFxplc.FxPlcInit())
            {
                Thread.Sleep(100);
                if (!mFxplc.FxPlcInit())
                {
                    mError.AddErrData("PLC_ERROR", "PLC初始化失败");//,Plc Init Error".Split(',')[cMain.IndexLanguage]);
                }
            }
            while (!conn)
            {
                conn = mFxplc.FxPlcInit();
                Thread.Sleep(100);
            }

            mFxplc.FxPlc_WriteM(Plc_M_MPoint["总复位"], true);

            while (!IsOutSystem)
            {
                for (int i = 0; i < Temp_Out_MPoint.Count; i++)
                {
                    if (Temp_Out_MPoint[cMain.DataPlcMPointStr[i]])
                    {
                        WritePlc(() =>
                        {
                            if (!mFxplc.FxPlc_WriteM(Plc_M_MPoint[cMain.DataPlcMPointStr[i]], Plc_Out_MPoint[cMain.DataPlcMPointStr[i]]))
                            {
                                mFxplc.FxPlc_WriteM(Plc_M_MPoint[cMain.DataPlcMPointStr[i]], Plc_Out_MPoint[cMain.DataPlcMPointStr[i]]);
                            }
                        });
                        Temp_Out_MPoint[cMain.DataPlcMPointStr[i]] = false;
                    }
                }
                for (int i = 0; i < Temp_Out_MReadPoint.Count; i++)
                {
                    if (Temp_Out_MReadPoint[cMain.ReadPlcMPointStr[i]])
                    {
                        WritePlc(() =>
                        {
                            if (!mFxplc.FxPlc_WriteM(Plc_M_MReadPoint[cMain.ReadPlcMPointStr[i]], Plc_Out_MReadPoint[cMain.ReadPlcMPointStr[i]]))
                            {
                                mFxplc.FxPlc_WriteM(Plc_M_MReadPoint[cMain.ReadPlcMPointStr[i]], Plc_Out_MReadPoint[cMain.ReadPlcMPointStr[i]]);
                            }
                        });
                        Temp_Out_MReadPoint[cMain.ReadPlcMPointStr[i]] = false;
                    }
                }
                for (int i = 0; i < Temp_Out_DPoint.Count; i++)
                {
                    if (Temp_Out_DPoint[cMain.DataPlcDPointStr[i]])
                    {
                        WritePlc(() =>
                        {
                            if (!mFxplc.FxPlc_WriteD(Plc_D_DPoint[cMain.DataPlcDPointStr[i]], Plc_Out_DPoint[cMain.DataPlcDPointStr[i]]))
                            {
                                mFxplc.FxPlc_WriteD(Plc_D_DPoint[cMain.DataPlcDPointStr[i]], Plc_Out_DPoint[cMain.DataPlcDPointStr[i]]);
                            }
                        });
                        Temp_Out_DPoint[cMain.DataPlcDPointStr[i]] = false;
                    }
                }
                for (int i = 0; i < 15; i++)
                {
                    if (i == 0 || i == 3 || i == 4 || i == 5 || i == 6 || i == 7 || i == 8 || i == 9 || i == 10 || i == 11)//这些M点所在的位置,没有须要读取的M点,跳过
                    {
                        continue;
                    }
                    if (mFxplc.FxPlc_ReadM(i * 8, out readMValue))
                    {
                        Array.Copy(readMValue, 0, ReadPlcMValue, i * 8, 8);
                    }
                }
                if (mFxplc.FxPlc_ReadD(1, 2, out readDValue))
                {
                    Plc_Start = ReadPlcMValue[100];
                    Plc_Stop = ReadPlcMValue[101];
                    if (!Temp_Out_MReadPoint["收氟确认"])
                    {
                        Plc_SF = ReadPlcMValue[106];
                    }
                    ZYSF_Stop = ReadPlcMValue[114];
                    if (ReadPlcMValue[112])
                    {
                        mFxplc.FxPlc_WriteM(112, false);
                    }
                    if (ReadPlcMValue[113])
                    {
                        mFxplc.FxPlc_WriteM(113, false);
                    }
                    if (ReadPlcMValue[115])
                    {
                        mFxplc.FxPlc_WriteM(115, false);
                    }
                    mError.DelErrData("PLC_ERROR");
                    ReadPlcErr = 0;
                    for (int i = 0; i < 2; i++)
                    {
                        if ((readDValue[i] >> 15) == 1)
                        {
                            readDValue[i] = -((readDValue[i] ^ 0xFFFF) + 1);
                        }
                        dataRead[4 + i] = readDValue[i] * cMain.mSysSet.PressK[i] + cMain.mSysSet.PressB[i];
                    }
                }
                else
                {
                    ReadPlcErr++;
                    if (ReadPlcErr >= 3)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            dataRead[4 + i] = -99;//压力
                        }
                        mError.AddErrData("PLC_ERROR", "PLC通讯失败,请检查屏幕和PLC通讯连接");//,PLC Error".Split(',')[cMain.IndexLanguage]);
                    }
                }
                Thread.Sleep(100);
            }
        }
        private void doThreadCom2()
        {
            byte[] buff50 = new byte[20];
            byte[] buff25 = new byte[8];
            int oldInitSnJiQi = 0;
            SerialPort com = new SerialPort();
            try
            {
                com = new SerialPort(cMain.mSysSet.mSnCom, 600, Parity.Even, 8, StopBits.Two);
                if (com.IsOpen)
                {
                    com.Close();
                }
                com.Open();
            }
            catch
            {
                mError.AddErrData(cMain.mSysSet.mSnCom + "_ERROR", "打开通讯端口" + (cMain.mSysSet.mSnCom + "失败"));//," + cMain.mSysSet.m485COM + "ERROR").Split(',')[cMain.IndexLanguage]);
            }
            cSn000 mSn = new cSn000(com);

            buff50[0] = 0xA0;
            buff50[1] = 0x01;
            buff50[3] = 0x50;
            buff50[4] = 0x0C;
            cMideaSnCode.Crc(buff50, cMideaSnCode.CrcList.NewSn600, ref buff50[18], ref buff50[19]);
            buff25[0] = 0xA0;
            buff25[1] = 0x01;
            buff25[3] = 0x25;
            buff25[5] = 0x09;
            cMideaSnCode.Crc(buff25, cMideaSnCode.CrcList.NewSn600, ref buff25[6], ref buff25[7]);
            while (!IsOutSystem)
            {
                if (InitSnJiQi > 0 && InitSnJiQi != oldInitSnJiQi)
                {
                    switch (InitSnJiQi)
                    {
                        case 1:
                            com.BaudRate = 600;
                            break;
                        case 2:
                            com.BaudRate = 1200;
                            break;
                        case 3:
                            com.BaudRate = 1023;
                            break;
                    }
                    oldInitSnJiQi = InitSnJiQi;
                }
                if (nowStatue.isStarting && Temp_Step_SnCode.Length > 0 && (InitSnJiQi == 1 || InitSnJiQi == 2 || InitSnJiQi == 3))
                {
                    mSn.Send(Temp_Step_SnCode);
                    Thread.Sleep(1500);
                    if (mSn.Read(out SnBoard_Show))
                    {
                        dataShow[11] = SnBoard_Show[0];
                        dataShow[12] = SnBoard_Show[1];
                        dataShow[13] = SnBoard_Show[2];
                        dataShow[14] = SnBoard_Show[3];
                    }
                    if (Temp_Step_SnCode.Length > 2 && Temp_Step_SnCode.StartsWith("A0"))
                    {
                        mSn.Send(buff50);
                        Thread.Sleep(1500);
                        if (mSn.Read(out SnBoard_Show1))
                        {
                            dataShow[15] = SnBoard_Show1[0] * 8;
                        }
                        mSn.Send(buff25);
                        Thread.Sleep(1000);
                        if (mSn.Read(out SnBoard_Show2))
                        {
                            dataShow[16] = SnBoard_Show2[0];
                            dataShow[17] = SnBoard_Show2[1];
                        }
                    }

                }
                else
                {
                    dataShow[11] = 0;
                    dataShow[12] = 0;
                    dataShow[13] = 0;
                    dataShow[14] = 0;
                    dataShow[15] = 0;
                    dataShow[16] = 0;
                    dataShow[17] = 0;
                    Thread.Sleep(100);
                }
            }
        }
        private void doThreadCom3()
        {
            SerialPort com = new SerialPort();
            double[] tmp = new double[8];
            c7017[] m7017 = new c7017[2];
            int[] errorCount = new int[m7017.Length];
            int index = 0;
            if (!cMain.isDebug)
            {
                try
                {
                    com = new SerialPort(cMain.mSysSet.m485COM, 9600, Parity.None, 8, StopBits.One);
                    if (com.IsOpen)
                    {
                        com.Close();
                    }
                    com.DtrEnable = true;
                    com.RtsEnable = true;
                    com.Open();
                }
                catch
                {
                    mError.AddErrData(cMain.mSysSet.m485COM + "_ERROR", "打开通讯端口" + (cMain.mSysSet.m485COM + "失败"));//," + cMain.mSysSet.m485COM + "ERROR").Split(',')[cMain.IndexLanguage]);
                }
            }
            #region//初始化
            for (int i = 0; i < m7017.Length; i++)
            {
                m7017[i] = new c7017(com, (byte)(2 + i), 400);
                errorCount[i] = 0;
                if (!m7017[i].init())
                {
                    mError.AddErrData("7017_Error", "7017通讯失败");
                }
            }
            cSset m2010 = new cSset(com, 1, 250, 30, cSset.ListVol.SanXiang);
            if (!m2010.SsetInit())
            {
                mError.AddErrData("FT3000", "电参数表通讯失败");
            }
            //cFt2010 m2010 = new cFt2010(com, 1, 6);
            //if (!m2010.Ft2010Init())
            //{
            //    mError.AddErrData("FT3000", "2010通讯失败");
            //}
            #endregion
            while (!IsOutSystem)//系统没有退出
            {
                index = 0;
                //读温度
                for (int i = 0; i < m7017.Length; i++)
                {
                    if (m7017[i].ReadData(ref tmp))
                    {
                        mError.DelErrData("7017_Error");
                        for (int j = 0; j < 2; j++)
                        {
                            switch (i)
                            {
                                case 0://风温
                                    dataRead[index++] = tmp[j];
                                    break;
                                case 1://管温
                                    dataRead[index++] = tmp[j] * 45 - 75;
                                    break;
                            }
                        }
                    }
                    else
                    {
                        errorCount[i]++;
                        if (errorCount[i] >= 3)
                        {
                            for (int j = 0; j < 2; j++)
                            {
                                dataRead[index++] = -99;
                            }
                        }
                        else
                        {
                            for (int j = 0; j < 2; j++)
                            {
                                index++;
                            }
                        }
                    }
                    System.Threading.Thread.Sleep(100);
                }
                index = 6;
                //读电参数

                //if (m2010.Ft2010Read(ref Ft2010_Read))
                if (m2010.SsetRead(ref Ft2010_Read))
                {
                    Read2010Err = 0;
                    mError.DelErrData("FT3000");
                    for (int i = 0; i < 9; i++)
                    {
                        dataRead[index++] = Ft2010_Read[i];
                    }
                }
                else
                {
                    Read2010Err++;
                    if (Read2010Err >= 3)
                    {
                        Read2010Err = 3;
                        for (int i = 0; i < 9; i++)
                        {
                            dataRead[index++] = -99;
                        }
                        mError.AddErrData("FT3000", "读电参数模块失败");
                    }
                    else
                    {
                        for (int i = 0; i < 9; i++)
                        {
                            index++;
                        }
                    }
                }
                Thread.Sleep(150);
            }
        }
        private void doThreadCom5()
        {
            SerialPort com = new SerialPort();

            try
            {
                com = new SerialPort(cMain.mSysSet.mPQCom, 4800, Parity.None, 8, StopBits.One);
                if (com.IsOpen)
                {
                    com.Close();
                }
                com.Open();
            }
            catch
            {
                mError.AddErrData(cMain.mSysSet.mPQCom + "_ERROR", "打开通讯端口" + (cMain.mSysSet.mPQCom + "失败"));//," + cMain.mSysSet.m485COM + "ERROR").Split(',')[cMain.IndexLanguage]);
            }
            if (com.IsOpen)
            {
                com.Close();
            }
            mPQ = new cPQMachine(cMain.mSysSet.mPQCom);
        }
        private void doThreadCom4()
        {
            SerialPort com = new SerialPort();
            if (!cMain.isDebug)
            {
                try
                {
                    com = new SerialPort(cMain.mSysSet.mBarCom, 115200, Parity.None, 8, StopBits.One);
                    if (com.IsOpen)
                    {
                        com.Close();
                    }
                    com.Open();
                }
                catch
                {
                    mError.AddErrData(cMain.mSysSet.mBarCom + "_ERROR", "打开通讯端口" + (cMain.mSysSet.mBarCom + "失败"));//,"+cMain.mSysSet.mPLCCOM+"ERROR").Split(',')[cMain.IndexLanguage]);
                }
            }//ReadBarCodeByCom
            cBar mBarCode = new cBar(com, 400);
            while (!IsOutSystem)
            {
                if (!mBarCode.readBarCode(ref ReadDataBarCode))
                {
                    readDataBarErr++;
                    if (readDataBarErr >= 3)
                    {
                        //MsgBox("数据扫描条码读取失败");
                    }
                }
                else
                {
                    ReadDataBarCode = Num.trim(ReadDataBarCode);
                    if (ReadDataBarCode != "" && ReadDataBarCode.Length > 5)
                    {
                        //if (ReadDataBarCode.ToUpper() == "8888888888")
                        //{
                        //    this.CrossThreadCalls(() =>
                        //    {
                        //        if (fLock != null)
                        //        {
                        //            fLock.Close();
                        //            fLock.Dispose();
                        //            fLock = null;
                        //        }
                        //        if (fPassWord != null)
                        //        {
                        //            fPassWord.Super = true;
                        //            fPassWord.DialogResult = DialogResult.Yes;
                        //            fPassWord.Close();
                        //            //fPassWord.SuperBarCheck(ReadDataBarCode.ToUpper());
                        //        }
                        //    });
                        //}
                        //else
                        //{
                        ReadBarCodeOver(ReadDataBarCode);
                        //}
                    }
                }
                Thread.Sleep(500);
            }
        }
        /// <summary>
        /// 对PLC写入M点
        /// </summary>
        /// <param name="tempM">要写的M值</param>
        /// <param name="mPoint">M点的地址</param>
        /// <returns>返回写入是否成功</returns>
        private void WritePlc(ThreadStart t)
        {
            if (t != null)
            {
                t();
            }
        }
        private bool frmClose()//系统退出时结束打开端口
        {
            IsOutSystem = true;
            if (udpSend.IsOpen)
            {
                udpSend.fUdpClose();
            }
            udpSend = null;
            Dispose(true);
            return true;
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            if (nowStatue.isStarting)
            {
                Handle_Next = true;
            }
            if (UpdataCount == 10)
            {
                updataByNet(sender, e);
            }
        }
        private void btnStart_Click(object sender, EventArgs e)//启动按钮
        {
            Handle_Start = true;
        }

        private void btnStop_Click(object sender, EventArgs e)//停止按钮
        {
            Handle_Stop = true;
        }

        private void MainLoop_Tick(object sender, EventArgs e)
        {
            FlushData();
            if (Environment.TickCount - PreFlushDataTime > cMain.FlushDataTime)
            {
                PreFlushDataTime = Environment.TickCount;
                ShowData(false, nowStatue.CurrentId);
            }
            if (nowStatue.isStarting)
            {
                if (cMain.DataShowTitle[1].Contains("(A)"))
                {
                    CheckProtect(dataShow[1], new double[] { dataShow[3], dataShow[4] });//检测数据保护(过流,过压等)
                }
                else
                {
                    CheckProtect(dataShow[6],
                        new double[]{
                            dataShow[12],
                            dataShow[17],
                            dataShow[22],
                            dataShow[36],
                            dataShow[40]
                                });
                }
            }
            //PLC停止,屏幕手动停止,网络停止,保护停止
            if (!Handle_Start && (Plc_Stop || Handle_Stop || Net_Stop || Pro_Stop || ZYSF_Stop)) //Stoping
            {
                if (Plc_Stop)
                {
                    Plc_Out_MReadPoint["停止"] = false;
                    Temp_Out_MReadPoint["停止"] = true;
                }
                if (ZYSF_Stop)
                {
                    Plc_Out_MReadPoint["正压收氟"] = false;
                    Temp_Out_MReadPoint["正压收氟"] = true;
                    nowStatue.StopId = cMain.StopValue.ZYSFProtect;
                }
                ZYSF_Stop = false;
                Plc_Stop = false;
                Handle_Stop = false;
                Net_Stop = false;
                Pro_Stop = false;
                if (nowStatue.isStarting)
                {
                    toolBtnStop.Enabled = false;
                    toolBtnStart.Enabled = true;
                    Plc_Out_MPoint["总复位"] = true; Temp_Out_MPoint["总复位"] = true;
                    nowStatue.isStarting = false;
                    nowStatue.isStarted = false;
                    return;
                }
            }
            if (nowStatue.isStarting == false && nowStatue.isStoped == false)//Stoped
            {
                StopOut();
                switch (nowStatue.StopId)
                {
                    case cMain.StopValue.ZYSFProtect:
                        mError.AddErrData("PROTECT", "Stop By Auto,正压收氟停机".Split(',')[cMain.IndexLanguage]);// + StopPress[tempBiaoZhunJi].ToString(), false);//压力校准
                        break;
                    case cMain.StopValue.LowPressProtect:
                        mError.AddErrData("PROTECT", "Low pressure protection,压力过低保护请检查".Split(',')[cMain.IndexLanguage]);
                        break;
                    case cMain.StopValue.LowCurProtect:
                        mError.AddErrData("PROTECT", "Low pressure protection,电流过低保护请检查".Split(',')[cMain.IndexLanguage]);
                        break;
                    case cMain.StopValue.HighPressProtect:
                        mError.AddErrData("PROTECT", "High pressure protection,压力过高保护请检查".Split(',')[cMain.IndexLanguage]);
                        break;
                    case cMain.StopValue.HighCurProtect:
                        mError.AddErrData("PROTECT", "High current protection,电流过高保护请检查".Split(',')[cMain.IndexLanguage]);
                        break;
                }
                mError.AddErrData("INFO", "Press the [StartTest] button to test,请按[开始]按钮开始检测".Split(',')[cMain.IndexLanguage]);
                nowStatue.StopId = cMain.StopValue.IsOk;
                nowStatue.isStoped = true;//停止完成
                nowStatue.CurrentId = -1;
            }
            if (Plc_Start || Handle_Start || Net_Start)//Starting
            {
                if (Plc_Start)
                {
                    Plc_Out_MReadPoint["启动"] = false;
                    Temp_Out_MReadPoint["启动"] = true;
                }
                Plc_Start = false;
                Handle_Start = false;
                Net_Start = false;
                if (!nowStatue.isStarting)
                {
                    toolBtnStop.Enabled = true;
                    toolBtnStart.Enabled = false;
                    nowStatue.isStarting = true;//正在检测
                    nowStatue.isStoped = false;//
                }
            }
            if (nowStatue.isStarting == true && nowStatue.isStarted == false)//Started
            {
                if (!IsStartOut)
                {
                    StartOut();
                    IsStartOut = true;
                }
                mError.DelErrData("PROTECT");
                if (!StartStep())
                {
                    mError.AddErrData("INFO", "正在等待上位机低启允许");
                    return;
                }
                nowStatue.isStarted = true;//启动完成
            }
            if (fLock != null)
            {
                if (Plc_SF || Handle_SF)
                {
                    Plc_Out_MPoint["蜂鸣器"] = false; Temp_Out_MPoint["蜂鸣器"] = true;
                    fLock = null;
                }
                else
                {
                    return;
                }
            }
            if (nowStatue.CurrentId < cModeSet.StepCount && nowStatue.isStarting)
            {
                Plc_Out_MReadPoint["收氟确认"] = false; Temp_Out_MReadPoint["收氟确认"] = true;
                Plc_SF = false;
                Handle_SF = false;
            }

            if (nowStatue.isStarting && nowStatue.CurrentId > -1 && nowStatue.CurrentId < cModeSet.StepCount)
            {
                quXianControl1.AddPoint(chanelCurve[0], Environment.TickCount / 1000 - nowStatue.TestStartTime, dataShow[1]);
                quXianControl1.AddPoint(chanelCurve[1], Environment.TickCount / 1000 - nowStatue.TestStartTime, dataShow[2]);
                quXianControl1.AddPoint(chanelCurve[2], Environment.TickCount / 1000 - nowStatue.TestStartTime, dataShow[3]);
                quXianControl1.AddPoint(chanelCurve[3], Environment.TickCount / 1000 - nowStatue.TestStartTime, dataShow[4]);
                //下一步
                if ((nowStatue.StepCurTime >= cMain.mModeSet.mSetTime[nowStatue.CurrentId]) || Handle_Next || Plc_Next || Net_Next)
                {
                    nowStatue.HandlePause = false;
                    toolBtnCancel.Text = "暂停";
                    SetBtnCancel();
                    if ((cMain.mModeSet.mSetTime[nowStatue.CurrentId] > 0) && (!IsEndStepOut))
                    {
                        IsEndStepOut = true;
                        if (!EndStep())
                        {
                            //if (cMain.mModeSet.mStepId[nowStatue.CurrentId] == "制热")//制热不合格,直接跳窗提示
                            //{
                            fLock = new object();
                            mError.AddErrData("INFO", "测试出现不合格,按[确认]继续测试");
                            Plc_Out_MPoint["蜂鸣器"] = true; Temp_Out_MPoint["蜂鸣器"] = true;
                            return;
                            //}
                        }
                    }
                    if (!StartStep())
                    {
                        mError.AddErrData("INFO", "正在等待上位机低启允许");
                        return;
                    }
                    Handle_Next = false;
                    Net_Next = false;
                }
                int tmp = Environment.TickCount / 1000;
                if (!nowStatue.HandlePause)
                {
                    nowStatue.StepCurTime = tmp - nowStatue.StepStartTime;
                    LblCurTime.Text = nowStatue.StepCurTime.ToString();
                }
            }
        }
        private void FlushData()
        {
            int index = 0;
            for (int i = 0; i < 3; i++)
            {
                dataShow[index++] = cMain.mKBValue.ShowValue(6 + i * 3);
            }
            dataShow[index++] = cMain.mKBValue.ShowValue(4);//压力
            dataShow[index++] = cMain.mKBValue.ShowValue(5);//压力
            dataShow[index++] = cMain.mKBValue.ShowValue(0);
            dataShow[index++] = cMain.mKBValue.ShowValue(1);
            dataShow[index++] = Math.Abs(dataShow[index - 2] - dataShow[index - 3]);
            dataShow[index++] = cMain.mKBValue.ShowValue(2);
            dataShow[index++] = cMain.mKBValue.ShowValue(3);
            dataShow[index++] = Math.Abs(dataShow[index - 2] - dataShow[index - 3]);
            for (int i = 0; i < 4; i++)
            {
                dataShow[index++] = SnBoard_Show[i];
            }
            if ((Plc_SF || Handle_SF) && nowStatue.CurrentId >= cModeSet.StepCount)
            {
                Plc_Out_MReadPoint["收氟确认"] = false; Temp_Out_MReadPoint["收氟确认"] = true;
                Plc_Out_MPoint["蜂鸣器"] = false; Temp_Out_MPoint["蜂鸣器"] = true;
                Handle_SF = false;
                Plc_SF = false;
                if (nowStatue.isStarting)
                {
                    lblShouFu.Visible = false;

                    if (nowStatue.OutBeep)
                    {
                        if (chkPrint.Checked && !nowStatue.PrintOver)
                        {
                            btnPrint_Click(btnPrint, new EventArgs());
                            nowStatue.PrintOver = true;
                            Handle_Stop = true;
                        }
                    }
                }
            }
            if (dataShow[4] < cMain.mModeSet.mProtect[10] && nowStatue.CurrentId >= cModeSet.StepCount)
            {
                lblShouFu.Visible = true;
                lblShouFu.Text = "请关闭低压阀";
                if (!nowStatue.OutBeep)
                {
                    Plc_Out_MPoint["蜂鸣器"] = true; Temp_Out_MPoint["蜂鸣器"] = true;
                    nowStatue.OutBeep = true;
                }
                switch (lblShouFu.BackColor.ToKnownColor())
                {
                    case KnownColor.Red:
                        lblShouFu.BackColor = Color.Green;
                        break;
                    case KnownColor.Green:
                        lblShouFu.BackColor = Color.Blue;
                        break;
                    case KnownColor.Blue:
                        lblShouFu.BackColor = Color.Yellow;
                        break;
                    default:
                        lblShouFu.BackColor = Color.Red;
                        break;
                }
            }
        }
        /// <summary>
        /// 检测数据是否在保护范围内
        /// </summary>
        private void CheckProtect(double cur, double[] press)
        {
            //过压保护
            for (int i = 0; i < press.Length; i++)
            {
                if (cMain.mModeSet.mProtect[0] < press[i])
                {
                    if (HighPresErr == 0)
                    {
                        HighPresErr = Environment.TickCount;
                    }
                    else
                    {
                        if (Environment.TickCount - HighPresErr > cMain.mModeSet.mProtect[1] * 1000)
                        {
                            Pro_Stop = true;
                            nowStatue.StopId = cMain.StopValue.HighPressProtect;
                        }
                    }
                }
                else
                {
                    HighPresErr = 0;
                }
                //低压保护
                if (press[i] > -1)
                {
                    if ((cMain.mModeSet.mProtect[2] > press[i]) && (nowStatue.CurrentId < cModeSet.StepCount) && (nowStatue.CurrentId > -1))
                    {
                        if (LowPresErr == 0)
                        {
                            LowPresErr = Environment.TickCount;
                        }
                        else
                        {
                            if (Environment.TickCount - LowPresErr > cMain.mModeSet.mProtect[3] * 1000)
                            {
                                Pro_Stop = true;
                                nowStatue.StopId = cMain.StopValue.LowPressProtect;
                            }
                        }
                    }
                    else
                    {
                        LowPresErr = 0;
                    }
                }
            }
            //过流保护
            if ((cMain.mModeSet.mProtect[4] < cur) && (cur > 0))
            {
                if (HighElectErr == 0)
                {
                    HighElectErr = Environment.TickCount;
                }
                else
                {
                    if (Environment.TickCount - HighElectErr > cMain.mModeSet.mProtect[5] * 1000)
                    {
                        Pro_Stop = true;
                        nowStatue.StopId = cMain.StopValue.HighCurProtect;
                    }
                }
            }
            else
            {
                HighElectErr = 0;
            }
            //低流保护
            if (cur > -1)
            {
                if ((cMain.mModeSet.mProtect[6] > cur) && (cMain.mModeSet.mJiQi == 0) && (nowStatue.CurrentId < cModeSet.StepCount) && (nowStatue.CurrentId > -1) && (cMain.mModeSet.mStepId[nowStatue.CurrentId] != "停机"))
                {
                    if (LowElectErr == 0)
                    {
                        LowElectErr = Environment.TickCount;
                    }
                    else
                    {
                        if (Environment.TickCount - LowElectErr > cMain.mModeSet.mProtect[7] * 1000)
                        {
                            Pro_Stop = true;
                            nowStatue.StopId = cMain.StopValue.LowCurProtect;
                        }
                    }
                }
                else
                {
                    LowElectErr = 0;
                }
            }
        }
        /// <summary>
        /// 步骤结束输出
        /// </summary>
        private bool EndStep()
        {
            int i;
            bool isStepPass = true;//当前步骤是否合格

            nowStatue.PrevStep = cMain.mModeSet.mStepId[nowStatue.CurrentId];

            if (cMain.mModeSet.mSetTime[nowStatue.CurrentId] < 1)
            {
                for (i = 0; i < cMain.DataShow; i++)
                {
                    cMain.mNetResult.StepResult.mData[i] = 0;
                    cMain.mNetResult.StepResult.mIsDataPass[i] = 1;

                    cMain.mTestResult.StepResult[nowStatue.CurrentId].mData[i] = 0;
                    cMain.mTestResult.StepResult[nowStatue.CurrentId].mIsDataPass[i] = 1;

                }
                cMain.mNetResult.StepResult.mIsStepPass = 1;
                cMain.mTestResult.StepResult[nowStatue.CurrentId].mIsStepPass = 1;
            }
            else
            {
                for (i = 0; i < cMain.DataShow; i++)
                {
                    cMain.mNetResult.StepResult.mData[i] = dataShow[i];
                    cMain.mTestResult.StepResult[nowStatue.CurrentId].mData[i] = dataShow[i];

                    if (((cMain.mModeSet.mHighData[nowStatue.CurrentId, i] == 0) && (cMain.mModeSet.mLowData[nowStatue.CurrentId, i] == 0)) ||
                        ((dataShow[i] <= cMain.mModeSet.mHighData[nowStatue.CurrentId, i]) &&
                        (dataShow[i] >= cMain.mModeSet.mLowData[nowStatue.CurrentId, i])) ||
                        (cMain.mModeSet.mShow[i] == false))
                    {
                        cMain.mNetResult.StepResult.mIsDataPass[i] = 1;
                        cMain.mTestResult.StepResult[nowStatue.CurrentId].mIsDataPass[i] = 1;
                    }
                    else
                    {
                        cMain.mNetResult.StepResult.mIsDataPass[i] = 0;
                        cMain.mTestResult.StepResult[nowStatue.CurrentId].mIsDataPass[i] = 0;
                        isStepPass = false;
                    }
                }
                if (isStepPass)
                {
                    cMain.mNetResult.StepResult.mIsStepPass = 1;
                    cMain.mTestResult.StepResult[nowStatue.CurrentId].mIsStepPass = 1;
                }
                else
                {
                    cMain.mNetResult.StepResult.mIsStepPass = 0;
                    cMain.mTestResult.StepResult[nowStatue.CurrentId].mIsStepPass = 0;
                }
            }

            cMain.mNetResult.RunResult.mStep = cMain.mModeSet.mStepId[nowStatue.CurrentId];
            cMain.mNetResult.RunResult.mStepId = nowStatue.CurrentId;
            cMain.mNetResult.RunResult.mIsPass = isStepPass;
            cMain.mTempNetResult.RunResult.mIsPass = isStepPass;
            if (isStepPass)
            {
                for (i = 0; i < nowStatue.CurrentId; i++)//为了方便步骤转换,所以每一次的总合格都是从前面的检测中计算,而不用保存值
                {
                    if (cMain.mTestResult.StepResult[i].mIsStepPass == 0)
                    {
                        cMain.mNetResult.RunResult.mIsPass = false;
                        cMain.mTempNetResult.RunResult.mIsPass = false;
                        break;
                    }
                }
            }
            cMain.mAllResult.SetStepResult(nowStatue.CurrentId, cMain.mNetResult.StepResult);
            cMain.mAllResult.RunResult = cMain.mNetResult.RunResult;
            cMain.mMesResult.RunResult = cMain.mNetResult.RunResult;
            cMain.mNetResult.StepResult.Equals(cMain.mNetResult.StepResult);
            cData.SaveJianCeData(cMain.mNetResult);
            new Thread(() => SendValueToComputer("J~OK~" + DataClassToStr(cMain.mNetResult), ref is_J_OK))
            {
                IsBackground = true
            }.Start();
            nowStatue.isPass = cMain.mNetResult.RunResult.mIsPass;
            return isStepPass;
        }
        /// <summary>
        /// 启动输出
        /// </summary>
        private void StartOut()//启动输出
        {
            initTestFrm(-1);
            initTestData();
            cMain.mModeSet.mBiaoZhunJi = 0;
            lblShouFu.BackColor = Color.Silver;
            lblShouFu.Visible = false;
            mError.AddErrData("INFO", "Test Now,正在检测".Split(',')[cMain.IndexLanguage]);
            HighElectErr = 0;
            HighPresErr = 0;
            LowElectErr = 0;
            LowPresErr = 0;
            nowStatue.OutBeep = false;
            nowStatue.TestTime = DateTime.Now;
            nowStatue.TestStartTime = Environment.TickCount / 1000;
            nowStatue.HandlePause = false;
            Plc_Out_MPoint["正收开关"] = false; Temp_Out_MPoint["正收开关"] = true;
            SetBtnCancel();
            if (cMain.mModeSet.mJiQi == 4)
            {
                mPQ.Init(cPQMachine.MachineLists.真PQ);
            }
            if (cMain.mModeSet.mJiQi == 5)
            {
                mPQ.Init(cPQMachine.MachineLists.假PQ);
            }
            if (cMain.mModeSet.mJiQi == 1 || cMain.mModeSet.mJiQi == 2 || cMain.mModeSet.mJiQi == 3)
            {
                InitSnJiQi = cMain.mModeSet.mJiQi;
            }
            Plc_Out_DPoint["报警停机"] = 999;
            Temp_Out_DPoint["报警停机"] = true;
            if (cMain.mKBValue.valueK[4 + cMain.mModeSet.mBiaoZhunJi] != 0 && cMain.mSysSet.PressK[cMain.mModeSet.mBiaoZhunJi] != 0)
            {
                Plc_Out_DPoint["停机压力"] = (int)(((cMain.mModeSet.mProtect[8] - cMain.mKBValue.valueB[4 + cMain.mModeSet.mBiaoZhunJi]) / cMain.mKBValue.valueK[4 + cMain.mModeSet.mBiaoZhunJi] - cMain.mSysSet.PressB[cMain.mModeSet.mBiaoZhunJi]) / cMain.mSysSet.PressK[cMain.mModeSet.mBiaoZhunJi]);
                Temp_Out_DPoint["停机压力"] = true;
                Plc_Out_DPoint["停机延时"] = (int)(cMain.mModeSet.mProtect[9] * 100);
                Temp_Out_DPoint["停机延时"] = true;
                Plc_Out_DPoint["报警压力"] = (int)(((cMain.mModeSet.mProtect[10] - cMain.mKBValue.valueB[4 + cMain.mModeSet.mBiaoZhunJi]) / cMain.mKBValue.valueK[4 + cMain.mModeSet.mBiaoZhunJi] - cMain.mSysSet.PressB[cMain.mModeSet.mBiaoZhunJi]) / cMain.mSysSet.PressK[cMain.mModeSet.mBiaoZhunJi]);
                Temp_Out_DPoint["报警压力"] = true;
                Plc_Out_DPoint["报警延时"] = (int)(cMain.mModeSet.mProtect[11] * 100);
                Temp_Out_DPoint["报警延时"] = true;
                Plc_Out_DPoint["压力报警上限"] = (int)(((cMain.mModeSet.mProtect[0] - cMain.mKBValue.valueB[4 + cMain.mModeSet.mBiaoZhunJi]) / cMain.mKBValue.valueK[4 + cMain.mModeSet.mBiaoZhunJi] - cMain.mSysSet.PressB[cMain.mModeSet.mBiaoZhunJi]) / cMain.mSysSet.PressK[cMain.mModeSet.mBiaoZhunJi]);
                Temp_Out_DPoint["压力报警上限"] = true;
                Plc_Out_DPoint["压力报警下限"] = (int)(((cMain.mModeSet.mProtect[2] - cMain.mKBValue.valueB[4 + cMain.mModeSet.mBiaoZhunJi]) / cMain.mKBValue.valueK[4 + cMain.mModeSet.mBiaoZhunJi] - cMain.mSysSet.PressB[cMain.mModeSet.mBiaoZhunJi]) / cMain.mSysSet.PressK[cMain.mModeSet.mBiaoZhunJi]);
                Temp_Out_DPoint["压力报警下限"] = true;
            }

            Plc_Out_MPoint["黄灯"] = true; Temp_Out_MPoint["黄灯"] = true;

            Plc_Out_MPoint[string.Format("{0}#内机", cMain.mModeSet.mBiaoZhunJi + 1)] = true; Temp_Out_MPoint[string.Format("{0}#内机", cMain.mModeSet.mBiaoZhunJi + 1)] = true;

        }
        private bool StartStep()//中间输出
        {
            int tmpStepID = nowStatue.CurrentId;
            do
            {
                tmpStepID++;
            } while (tmpStepID < cModeSet.StepCount && cMain.mModeSet.mStepId[tmpStepID] != "" && cMain.mModeSet.mSetTime[tmpStepID] <= 0);

            if (tmpStepID < cModeSet.StepCount)
            {
                if (cMain.mModeSet.mStepId[tmpStepID] == "低启" ||
                    cMain.mModeSet.mStepId[tmpStepID] == "Low Vol")
                {
                    if (!lowVolOneByOne.GetAccess())
                    {
                        return false;
                    }
                }
                else
                {
                    if (!lowVolOneByOne.Del())
                    {
                        lowVolOneByOne.Del();
                    }
                }
            }
            nowStatue.CurrentId = tmpStepID;
            nowStatue.StepStartTime = Environment.TickCount / 1000;
            nowStatue.StepCurTime = 0;
            if (nowStatue.CurrentId >= cModeSet.StepCount)
            {
                EndOut();
                mError.AddErrData("INFO", "Test is Over,检测已完成 请收氟".Split(',')[cMain.IndexLanguage]);
                return true;
            }
            if (cMain.mModeSet.mSetTime[nowStatue.CurrentId] <= 0)
            {
                return true;
            }
            if ((cMain.mModeSet.mJiQi == 4 || cMain.mModeSet.mJiQi == 5) && cMain.mModeSet.mSendStr[nowStatue.CurrentId] != "")
            {
                mPQ.WritePQ(cMain.mModeSet.mSendStr[nowStatue.CurrentId]);
            }
            LblCurTime.Text = nowStatue.StepCurTime.ToString();
            cMain.mTempNetResult.RunResult.mStep = cMain.mModeSet.mStepId[nowStatue.CurrentId];
            cMain.mTempNetResult.RunResult.mStepId = nowStatue.CurrentId;
            mError.AddErrData("INFO", new string[] { "Now Test ", "当前检测步骤" }[cMain.IndexLanguage] + cMain.mModeSet.mStepId[nowStatue.CurrentId]);
            LblStep.Text = cMain.mModeSet.mStepId[nowStatue.CurrentId];
            lblStepId.Text = string.Format("一,二,三,四,五,六,七,八,九,十,十一,十二,十三,十四,十五,十六,十七,十八,十九,二十").Split(',')[nowStatue.CurrentId];
            LblSetTime.Text = cMain.mModeSet.mSetTime[nowStatue.CurrentId].ToString();

            Temp_Step_SnCode = cMain.mModeSet.mSendStr[nowStatue.CurrentId];

            switch (cMain.mModeSet.mStepId[nowStatue.CurrentId])
            {
                case "停机":
                    Plc_Out_MPoint["低启"] = false; Temp_Out_MPoint["低启"] = true;
                    Plc_Out_MPoint["上电"] = false; Temp_Out_MPoint["上电"] = true;
                    LblStep.BackColor = Color.White;
                    break;
                case "待机":
                    Plc_Out_MPoint["低启"] = true; Temp_Out_MPoint["低启"] = true;
                    Plc_Out_MPoint["上电"] = true; Temp_Out_MPoint["上电"] = true;
                    LblStep.BackColor = Color.White;
                    break;
                case "低启":
                    Plc_Out_MPoint["低启"] = false; Temp_Out_MPoint["低启"] = true;
                    Plc_Out_MPoint["上电"] = true; Temp_Out_MPoint["上电"] = true;
                    LblStep.BackColor = Color.White;
                    break;
                case "制热":
                    Plc_Out_MPoint["低启"] = true; Temp_Out_MPoint["低启"] = true;
                    Plc_Out_MPoint["上电"] = true; Temp_Out_MPoint["上电"] = true;
                    LblStep.BackColor = Color.Red;
                    break;
                default:
                    Plc_Out_MPoint["低启"] = true; Temp_Out_MPoint["低启"] = true;
                    Plc_Out_MPoint["上电"] = true; Temp_Out_MPoint["上电"] = true;
                    LblStep.BackColor = Color.Blue;
                    break;
            }
            Plc_Out_MPoint["低风"] = cMain.mModeSet.mKaiGuan[nowStatue.CurrentId, 0]; Temp_Out_MPoint["低风"] = true;
            Plc_Out_MPoint["四通阀"] = cMain.mModeSet.mKaiGuan[nowStatue.CurrentId, 1]; Temp_Out_MPoint["四通阀"] = true;
            Plc_Out_MPoint["压缩机"] = cMain.mModeSet.mKaiGuan[nowStatue.CurrentId, 2]; Temp_Out_MPoint["压缩机"] = true;
            Plc_Out_MPoint["备用"] = cMain.mModeSet.mKaiGuan[nowStatue.CurrentId, 3]; Temp_Out_MPoint["备用"] = true;

            IsStartOut = false;
            IsEndStepOut = false;
            return true;
        }
        private void EndOut()//收氟输出
        {
            nowStatue.PrintOver = false;
            lblShouFu.Text = "请关闭高压阀";
            lblShouFu.Visible = true;
            Plc_Out_MPoint["蜂鸣器"] = true; Temp_Out_MPoint["蜂鸣器"] = true;
            Plc_Out_MPoint["正收开关"] = cMain.mSysSet.ZYSFArea == 1; Temp_Out_MPoint["正收开关"] = true;
            Plc_Out_MPoint["正收动作"] = cMain.mSysSet.ZYSFDoing == 1; Temp_Out_MPoint["正收动作"] = true;
            LblStep.Text = "收氟";
            cMain.mTempNetResult.RunResult.mStepId = cModeSet.StepCount;
            cMain.mAllResult.Save();
            //cMain.mAuto.Add(cMain.mAllResult);
            Plc_Out_MPoint["黄灯"] = false; Temp_Out_MPoint["黄灯"] = true;
            if (!nowStatue.isPass)
            {
                initTestFrm(0);
                Plc_Out_MPoint["红灯"] = true; Temp_Out_MPoint["红灯"] = true;
            }
            else
            {
                initTestFrm(1);
                Plc_Out_MPoint["绿灯"] = true; Temp_Out_MPoint["绿灯"] = true;
            }
            LblStep.BackColor = Color.Yellow;
        }
        /// <summary>
        /// 停机输出
        /// </summary>
        private void StopOut()
        {
            Plc_Out_MReadPoint["收氟确认"] = false; Temp_Out_MReadPoint["收氟确认"] = true;
            Plc_SF = false;
            Handle_SF = false;
            fLock = null;
            lblShouFu.Visible = false;
            if (!lowVolOneByOne.Del())
            {
                lowVolOneByOne.Del();
            }
            LblStep.BackColor = Color.White;
            Handle_Next = false;
            Plc_Next = false;
            Net_Next = false;
            initTestFrm(-2);
            if (mPQ != null)
            {
                mPQ.Close();
            }
            nowStatue.HandlePause = false;
            SetBtnCancel();
            Plc_Out_MPoint["总复位"] = true; Temp_Out_MPoint["总复位"] = true;
            Net_Next = false;
            nowStatue.PrevStep = "";
            cMain.mTempNetResult.RunResult.mStepId = -1;
        }
        /// <summary>
        /// 将数据显示出来
        /// </summary>
        /// <param name="ShowLabel">label数组,要显示刷新实时数据,还是步骤数据</param>
        /// <param name="TestStepNo">要刷新的步骤号</param>
        private void ShowData(bool isStepData, int TestStepNo)
        {
            if (!isStepData)
            {
                DataGridViewRow dr = dataGridNow.Rows[0];
                DataGridViewCell cell;


                for (int i = 0; i < cMain.DataShow; i++)//刷新实时数据框
                {
                    cell = dr.Cells[i];
                    if (cMain.DataShowTitle[i].IndexOf("(A)") >= 0)
                    {
                        cell.Value = string.Format("{0:F2}", dataShow[i]);
                    }
                    else
                    {
                        if (cMain.DataShowTitle[i].IndexOf("(Mpa)") >= 0)
                        {
                            cell.Value = string.Format("{0:F2}", dataShow[i]);
                        }
                        else
                        {
                            cell.Value = string.Format("{0:F1}", dataShow[i]);
                        }
                    }
                    cMain.mTempNetResult.StepResult.mData[i] = dataShow[i];
                    if ((TestStepNo >= 0 && TestStepNo < cModeSet.StepCount) && cMain.mModeSet.mShow[i]
                        && (cMain.mModeSet.mHighData[TestStepNo, i] != 0 || cMain.mModeSet.mLowData[TestStepNo, i] != 0)
                        && (cMain.mModeSet.mHighData[TestStepNo, i] < dataShow[i] || cMain.mModeSet.mLowData[TestStepNo, i] > dataShow[i])
                        )
                    {
                        cMain.mTempNetResult.StepResult.mIsDataPass[i] = 0;
                        cell.Style.BackColor = Color.Pink;
                    }
                    else
                    {
                        cMain.mTempNetResult.StepResult.mIsDataPass[i] = 1;
                        cell.Style.BackColor = Color.White;
                    }
                }
                for (int i = 0; i < dataGridStep.Columns.Count; i++)
                {
                    if (i < nowStatue.CurrentId)
                    {
                        for (int j = 0; j < cMain.DataShow; j++)
                        {
                            dr = dataGridStep.Rows[j];
                            cell = dr.Cells[i];
                            if (cMain.DataShowTitle[j].IndexOf("(A)") >= 0)
                            {
                                cell.Value = string.Format("{0:F2}", cMain.mTestResult.StepResult[i].mData[j]);
                            }
                            else
                            {
                                if (cMain.DataShowTitle[j].IndexOf("(Mpa)") >= 0)
                                {
                                    cell.Value = string.Format("{0:F2}", cMain.mTestResult.StepResult[i].mData[j]);
                                }
                                else
                                {
                                    cell.Value = string.Format("{0:F1}", cMain.mTestResult.StepResult[i].mData[j]);
                                }
                            }
                            if (cMain.mTestResult.StepResult[i].mIsDataPass[j] == 0)
                            {
                                cell.Style.BackColor = Color.Pink;
                            }
                            else
                            {
                                cell.Style.BackColor = Color.White;
                            }
                        }
                        dr = dataGridStep.Rows[cMain.DataShow];
                        cell = dr.Cells[i];
                        if (cMain.mTestResult.StepResult[i].mIsStepPass == 0)
                        {
                            if (cMain.IndexLanguage == 1)
                            {
                                cell.Value = "不合格";
                            }
                            else
                            {
                                cell.Value = "NG";
                            }
                            cell.Style.BackColor = Color.Pink;
                        }
                        else
                        {
                            if (cMain.IndexLanguage == 1)
                            {
                                cell.Value = "合格";
                            }
                            else
                            {
                                cell.Value = "OK";
                            }
                            cell.Style.BackColor = Color.LightGreen;
                        }
                    }
                    else if (i == nowStatue.CurrentId)
                    {
                        for (int j = 0; j < cMain.DataShow; j++)//刷新实时数据框
                        {
                            dr = dataGridStep.Rows[j];
                            cell = dr.Cells[i];
                            if (cMain.DataShowTitle[j].IndexOf("(A)") >= 0)
                            {
                                cell.Value = string.Format("{0:F2}", cMain.mTempNetResult.StepResult.mData[j]);
                            }
                            else
                            {
                                if (cMain.DataShowTitle[j].IndexOf("(Mpa)") >= 0)
                                {
                                    cell.Value = string.Format("{0:F2}", cMain.mTempNetResult.StepResult.mData[j]);
                                }
                                else
                                {
                                    cell.Value = string.Format("{0:F1}", cMain.mTempNetResult.StepResult.mData[j]);
                                }
                            }
                            if (cMain.mTempNetResult.StepResult.mIsDataPass[j] == 0)
                            {
                                cell.Style.BackColor = Color.Pink;
                            }
                            else
                            {
                                cell.Style.BackColor = Color.White;
                            }
                        }
                    }
                }
            }
            dataGridStep.CurrentCell = null;//将自动选择的单元去掉,被选择时,会有个自动样式,阻碍绘画背景红色
        }
        private void initTestFrm(int isPass)//-1为空白,0为不合格,1为合格
        {
            switch (isPass)
            {
                case -2:
                    lblResult.Text = "待机中";
                    lblResult.BackColor = Color.White;
                    break;
                case -1:
                    lblResult.Text = "测试中";
                    lblResult.BackColor = Color.LightYellow;
                    break;
                case 0:
                    lblResult.Text = "不合格";
                    lblResult.BackColor = Color.Pink;
                    udpSend.fUdpSend("O~OK~" + cMain.ThisNo.ToString() + "~false");
                    break;
                case 1:
                    lblResult.Text = "合格";
                    lblResult.BackColor = Color.LightGreen;
                    udpSend.fUdpSend("O~OK~" + cMain.ThisNo.ToString() + "~true");
                    break;
            }
        }
        private void initNowStatue()
        {
            nowStatue.CurrentId = -1;
            nowStatue.firstColdId = 0;
            nowStatue.DiQiId = 0;
            nowStatue.firstHotId = 0;
            nowStatue.isPass = true;
            nowStatue.isStarted = false;
            nowStatue.isStarting = false;
            nowStatue.isStoped = false;
            nowStatue.lastColdId = 0;
            nowStatue.lastHotId = 0;
            nowStatue.mStepShowData = cMain.StepShowData.ShowByLastHot;
            nowStatue.StepCurTime = 0;
            nowStatue.StepStartTime = 0;
            nowStatue.StopId = cMain.StopValue.IsOk;
            nowStatue.TestTime = DateTime.Now;
            nowStatue.TestStartTime = 0;
            nowStatue.PrevStep = "";
            nowStatue.PrintOver = true;
            nowStatue.OutBeep = false;
        }
        /// <summary>
        /// 初始化所有数据
        /// </summary>
        private void initTestData()
        {
            quXianControl1.reSet();
            initTestData(this);
        }
        public static void initTestData(frmMain mFrmMain)//初始化所有数据
        {
            int i, j;
            frmSet.DataFileToClass(cMain.AppPath + "\\ID\\" + cMain.mSysSet.mPrevId + ".txt", out cMain.mModeSet, true);

            cMain.mNetResult.RunResult.mBar = cMain.mSysSet.mPrevBar;
            cMain.mNetResult.RunResult.mId = cMain.mSysSet.mPrevId;
            cMain.mNetResult.RunResult.mJiQi = cMain.mModeSet.mJiQi;
            cMain.mNetResult.RunResult.mMode = cMain.mModeSet.mMode;
            cMain.mNetResult.RunResult.mTestNo = cMain.ThisNo;
            cMain.mNetResult.RunResult.mTestTime = DateTime.Now;
            cMain.mNetResult.RunResult.mIsPass = true;
            cMain.mNetResult.RunResult.mStep = "";
            cMain.mNetResult.RunResult.mStepId = -1;

            cMain.mTempNetResult.RunResult.mBar = cMain.mSysSet.mPrevBar;
            cMain.mTempNetResult.RunResult.mId = cMain.mSysSet.mPrevId;
            cMain.mTempNetResult.RunResult.mJiQi = cMain.mModeSet.mJiQi;
            cMain.mTempNetResult.RunResult.mMode = cMain.mModeSet.mMode;
            cMain.mTempNetResult.RunResult.mTestNo = cMain.ThisNo;
            cMain.mTempNetResult.RunResult.mTestTime = DateTime.Now;
            cMain.mTempNetResult.RunResult.mIsPass = true;
            cMain.mTempNetResult.RunResult.mStep = "";
            cMain.mTempNetResult.RunResult.mStepId = -1;

            cMain.mMesResult.RunResult = cMain.mNetResult.RunResult;
            cMain.mMesResult.StepResult.Clear();
            for (i = 0; i < cMain.DataShow; i++)
            {
                if (cMain.mModeSet.mShow[i])
                {
                    mFrmMain.dataGridNow.Columns[i].Visible = true;
                }
                else
                {
                    mFrmMain.dataGridNow.Columns[i].Visible = false;
                }
            }

            mFrmMain.LblBar.Text = cMain.mSysSet.mPrevBar;
            mFrmMain.LblId.Text = cMain.mSysSet.mPrevId;
            mFrmMain.LblMode.Text = cMain.mModeSet.mMode;
            string[] tempStr = cMain.BiaoZhunJiStr[cMain.IndexLanguage].Split(',');
            mFrmMain.LblBiaoZJ.Text = tempStr[cMain.mModeSet.mBiaoZhunJi];

            mFrmMain.lblVol.Text = cMain.DianYuanStr.Split(',')[cMain.mModeSet.mElect];
            mFrmMain.LblSetTime.Text = "";
            mFrmMain.LblCurTime.Text = "";
            mFrmMain.LblStep.Text = "";
            //初始化显示DataStepGridView
            DataTable dt = new DataTable();
            int stepCount = 0;
            for (i = 0; i < cModeSet.StepCount; i++)
            {
                dt.Columns.Add("string" + i.ToString(), typeof(string));
                dt.Columns[stepCount].Caption = cMain.mModeSet.mStepId[i];
                stepCount++;
            }
            string[] tempData = new string[stepCount];
            for (i = 0; i < cMain.DataShow + 1; i++)
            {
                DataRow dr = dt.NewRow();
                dr.ItemArray = tempData;
                dt.Rows.Add(dr);
            }
            mFrmMain.dataGridStep.Font = new Font("宋体", 12);
            mFrmMain.dataGridStep.DataSource = dt;
            ///下面2行为货币管理 ,为了隐藏不须要的行
            mFrmMain.dataGridStep.CurrentCell = null;
            //BindingContext bc = new BindingContext();
            //CurrencyManager cm = (CurrencyManager)bc[mFrmMain.dataGridStep.DataSource];
            //cm.SuspendBinding();// 挂起数据绑定
            ///
            mFrmMain.dataGridStep.RowHeadersWidth = 180;
            for (i = 0; i < cMain.DataShow; i++)
            {
                mFrmMain.dataGridStep.Rows[i].Height = 24;
                mFrmMain.dataGridStep.Rows[i].HeaderCell.Value = cMain.DataShowTitle[i];
                mFrmMain.dataGridStep.Rows[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                mFrmMain.dataGridStep.Rows[i].Visible = cMain.mModeSet.mShow[i];
            }
            ///下面1行为货币管理恢复
            //cm.ResumeBinding();
            ///
            if (cMain.IndexLanguage == 1)
            {
                mFrmMain.dataGridStep.Rows[cMain.DataShow].HeaderCell.Value = "检测结果";
                mFrmMain.dataGridStep.Rows[cMain.DataShow].Height = 24;
            }
            else
            {
                mFrmMain.dataGridStep.Rows[cMain.DataShow].HeaderCell.Value = "Result";
            }
            mFrmMain.dataGridStep.Rows[cMain.DataShow].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            for (i = 0; i < mFrmMain.dataGridStep.Columns.Count; i++)
            {
                mFrmMain.dataGridStep.Columns[i].HeaderText = dt.Columns[i].Caption;
                if (cMain.mModeSet.mSetTime[i] <= 0 || cMain.mModeSet.mStepId[i] == "")
                {
                    mFrmMain.dataGridStep.Columns[i].Visible = false;
                }
                else
                {
                    mFrmMain.dataGridStep.Columns[i].Visible = true;
                }
                mFrmMain.dataGridStep.Columns[i].Width = 70;
            }
            for (i = 0; i < mFrmMain.dataGridStep.ColumnCount; i++)
            {
                mFrmMain.dataGridStep.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            for (i = cModeSet.StepCount - 1; i >= 0; i--)
            {
                if (cMain.mModeSet.mSetTime[i] > 0)
                {
                    if (cMain.mModeSet.mStepId[i] == "制冷" || cMain.mModeSet.mStepId[i] == "Cooling")
                    {
                        nowStatue.firstColdId = i;
                    }
                    if (cMain.mModeSet.mStepId[i] == "制热" || cMain.mModeSet.mStepId[i] == "Heating")
                    {
                        nowStatue.firstHotId = i;
                    }
                    if (cMain.mModeSet.mStepId[i] == "低启" || cMain.mModeSet.mStepId[i] == "Low Vol")
                    {
                        nowStatue.DiQiId = i;
                    }
                }
            }


            for (i = 0; i < cModeSet.StepCount; i++)
            {
                if (cMain.mModeSet.mSetTime[i] > 0)
                {
                    if (cMain.mModeSet.mStepId[i] == "制冷" || cMain.mModeSet.mStepId[i] == "Cooling")
                    {
                        nowStatue.lastColdId = i;
                    }
                    if (cMain.mModeSet.mStepId[i] == "制热" || cMain.mModeSet.mStepId[i] == "Heating")
                    {
                        nowStatue.lastHotId = i;
                    }
                }
            }
            for (i = 0; i < cMain.DataShow; i++)
            {
                cMain.mNetResult.StepResult.mData[i] = 0;
                cMain.mNetResult.StepResult.mIsDataPass[i] = -1;
                for (j = 0; j < cModeSet.StepCount; j++)
                {
                    cMain.mTestResult.StepResult[j].mData[i] = 0;
                    cMain.mTestResult.StepResult[j].mIsDataPass[i] = -1;
                }
            }
            cMain.mNetResult.StepResult.mIsStepPass = -1;
            for (i = 0; i < cModeSet.StepCount; i++)
            {
                cMain.mTestResult.StepResult[i].mIsStepPass = -1;
            }
            cMain.mAllResult.Init();
            cMain.mAllResult.ModeSet = cMain.mModeSet;
        }
        /// <summary>
        /// 将当前检测步骤数据发送到计算机
        /// </summary>
        /// <param name="NetResult"></param>
        private static string DataClassToStr(cNetResult NetResult)
        {
            int i = 0;
            string SendStr = "";
            try
            {
                SendStr = SendStr + NetResult.RunResult.mTestTime.ToString() + "~";//检测开始时间
                SendStr = SendStr + NetResult.RunResult.mTestNo.ToString() + "~";//台车号
                SendStr = SendStr + NetResult.RunResult.mStepId.ToString() + "~";
                SendStr = SendStr + NetResult.RunResult.mStep + "~";
                SendStr = SendStr + NetResult.RunResult.mMode + "~";
                SendStr = SendStr + NetResult.RunResult.mJiQi.ToString() + "~";
                SendStr = SendStr + NetResult.RunResult.mIsPass.ToString() + "~";
                SendStr = SendStr + NetResult.RunResult.mId + "~";
                SendStr = SendStr + NetResult.RunResult.mBar + "~";
                for (i = 0; i < cMain.DataShow; i++)
                {
                    SendStr = SendStr + Num.Format(NetResult.StepResult.mData[i], 3) + "~";
                    SendStr = SendStr + NetResult.StepResult.mIsDataPass[i].ToString() + "~";
                }
                SendStr = SendStr + NetResult.StepResult.mIsStepPass.ToString() + "~";
            }
            catch (Exception exc)
            {
                cMain.WriteErrorToLog("FrmMain DataClassToStr " + exc.ToString());
                SendStr = "";
            }
            return SendStr;
        }
        private bool LoadLocalIdByBarCode(string barCode)
        {
            bool returnResult = false;

            DirectoryInfo di = new DirectoryInfo(cMain.AppPath + "\\ID\\");
            int index = 0;
            string[] ListId = new string[di.GetFiles("*.txt").Length];
            foreach (FileInfo fi in di.GetFiles("*.txt"))
            {
                ListId[index] = fi.Name.Substring(0, fi.Name.IndexOf("."));
                index++;
            }
            int LenBar = barCode.Length;
            for (int i = 0; i < 10; i++)
            {
                if (cMain.mBarSet.mIsUse[i] && (LenBar == cMain.mBarSet.mIntBarLength[i])
                   && (cMain.mBarSet.mIntBarLength[i] > 0) && (cMain.mBarSet.mIntBarCount[i] > 0) && (cMain.mBarSet.mIntBarStart[i] > 0)
                   && (cMain.mBarSet.mIntBarLength[i] >= (cMain.mBarSet.mIntBarCount[i] + cMain.mBarSet.mIntBarStart[i] - 1))
                    )
                {
                    string fileName = barCode.Substring(cMain.mBarSet.mIntBarStart[i] - 1, cMain.mBarSet.mIntBarCount[i]);
                    bool isFind = false;
                    for (int j = 0; j < ListId.Length; j++)
                    {
                        if (ListId[j].ToUpper() == fileName.ToUpper())
                        {
                            isFind = true;
                            break;
                        }
                    }
                    if (isFind)
                    {
                        cMain.mSysSet.mPrevBar = barCode;
                        cMain.mSysSet.mPrevId = fileName;
                        frmSys.DataClassToFile(cMain.mSysSet);
                        if (cMain.mBarSet.mIsAutoStart)
                        {
                            Net_Start = true;
                        }
                        break;
                    }
                }
            }
            return returnResult;
        }
        private bool DataStrToClass(string NetString, out cNetModeSet mNetModeSet)
        {
            bool returnResult = false;
            cNetModeSet NetModeSet = new cNetModeSet();
            int i;
            try
            {
                string[] tempStr = NetString.Split('~');
                string tempNetString = "";
                for (i = 3; i < tempStr.Length; i++)
                {
                    tempNetString = tempNetString + tempStr[i] + "~";//tempStr[0]是B识别符,tempStr[1]是条码,tempStr[2]是启动符
                }
                if (tempStr[1] != "")
                {
                    NetModeSet.mBar = tempStr[1];
                }
                NetModeSet.isStart = (tempStr[2] == "1") ? true : false;

                frmSet.DataFileToClass(tempNetString, out NetModeSet.ModeSet, false);

                cMain.mSysSet.mPrevBar = NetModeSet.mBar;
                cMain.mSysSet.mPrevId = NetModeSet.ModeSet.mId;
                frmSys.DataClassToFile(cMain.mSysSet);
                if (NetModeSet.isStart)
                {
                    Net_Start = true;
                }
                returnResult = true;
            }
            catch (Exception exc)
            {
                cMain.WriteErrorToLog("FrmMain DataStrToClass " + exc.ToString());
            }
            mNetModeSet = NetModeSet;
            return returnResult;
        }
        private bool DataStrToClass(string NetString, out cSystemSet mSystemSet)
        {
            bool returnResult = false;
            cSystemSet SystemSet = new cSystemSet();
            int i;
            try
            {
                string[] tempStr = NetString.Split('~');
                string tempNetString = "";
                for (i = 1; i < tempStr.Length; i++)
                {
                    tempNetString = tempNetString + tempStr[i] + "~";//tempStr[0]为S识别符
                }
                frmSys.DataFileToClass(tempNetString, out SystemSet, false);
                returnResult = true;
            }
            catch (Exception exc)
            {
                cMain.WriteErrorToLog("FrmMain DataStrToClass " + exc.ToString());
            }
            mSystemSet = SystemSet;
            return returnResult;
        }
        private void frmMain_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar == (char)13)
            //{
            //    LblBar.Text = ReadBarCode;
            //    cBarCode tempBarCode = new cBarCode("R~OK~" + string.Format("{0}~", cMain.ThisNo) + ReadBarCode);
            //    tempBarCode.sendBarCodeHandle = new SendBarCodeHandle(SendBarToComputer);
            //    SendBarCode = new Thread(new ThreadStart(tempBarCode.sendBar));
            //    SendBarCode.IsBackground = true;
            //    SendBarCode.Priority = ThreadPriority.BelowNormal;
            //    SendBarCode.Start();
            //    ReadBarCode = "";
            //}
            //else
            //{
            //    ReadBarCode = ReadBarCode + e.KeyChar.ToString();
            //}
            //e.Handled = true;
        }
        private void DataErrorFlush_Tick(object sender, EventArgs e)
        {
            FlushControlText(lblInfo, mError.GetError(), mError.GetErrEnum());
        }

        private void panQuXianName_Paint(object sender, PaintEventArgs e)
        {

        }

        private void toolBtnList_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < cMain.AllCount; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    udpSend.fUdpSend(string.Format("192.168.1.{0}", 101 + i), 3000, "Y");
                }
            }
        }
        private void Chk_Click(object sender, EventArgs e)
        {
        }
        private void btnBar_Click(object sender, EventArgs e)
        {
            ReadBarCodeOver(LblBar.Text);
        }
        private void ReadBarCodeOver(string barcode)
        {
            string[] fileName, mode;
            bool isFindLength = false;//是否找到同长度条码
            bool isFindId = false;//是否找到对应ID
            string findId = "";//找到的ID号ef
            ReadBarCode = barcode.Trim();
            this.CrossThreadCalls(() => LblBar.Text = ReadBarCode);
            if (cMain.mBarSet.mIsWinCeBar)
            {
                for (int i = 0; i < cMain.mBarSet.mIsUse.Length; i++)
                {
                    if (cMain.mBarSet.mIsUse[i])
                    {
                        if (ReadBarCode.Length == cMain.mBarSet.mIntBarLength[i])
                        {
                            frmList.GetXml(out fileName, out mode);
                            foreach (string ss in fileName)
                            {
                                if (ReadBarCode.Substring(cMain.mBarSet.mIntBarStart[i] - 1, cMain.mBarSet.mIntBarCount[i])
                                    == ss)
                                {
                                    findId = ss;
                                    isFindId = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (isFindId)
                    {
                        cMain.mSysSet.mPrevBar = ReadBarCode;
                        cMain.mSysSet.mPrevId = findId;
                        frmSys.DataClassToFile(cMain.mSysSet);
                        frmMain.initTestData(this);
                        if (cMain.mBarSet.mIsAutoStart)
                        {
                            Handle_Start = true;
                        }
                        return;
                    }
                }
                if (!isFindLength)//没有找到相对应条码
                {
                    MessageBox.Show(string.Format("没有找到条码长度{0}:{1}的条码设置", ReadBarCode.Length, ReadBarCode), "错误",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!isFindId)//没有找到相对应的ID
                {
                    MessageBox.Show(string.Format("没有找到条码:{0}对应的ID机型", ReadBarCode), "错误", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                this.CrossThreadCalls(() => LblBar.Text = ReadBarCode);
                new Thread(() => SendValueToComputer("R~OK~" + string.Format("{0}~", cMain.ThisNo) + ReadBarCode, ref is_R_OK))
                {
                    IsBackground = true
                }.Start();
            }
        }
        private void SendValueToComputer(string sendStr, ref bool result)
        {
            long startTime = 0;
            int timeOut = 2000;
            bool isTimeOut = false;
            result = false;
            for (int i = 0; i < 3; i++)
            {
                isTimeOut = false;
                startTime = Environment.TickCount;
                udpSend.fUdpSend(cMain.RemoteHostName, 3000 + cMain.ThisNo, sendStr);
                do
                {
                    Thread.Sleep(100);
                    if ((Environment.TickCount - startTime) > timeOut)
                    {
                        isTimeOut = true;
                    }
                } while (!isTimeOut && (result));
                if (result)
                {
                    break;
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
            Thread.CurrentThread.Abort();
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            frmPassWord fp = new frmPassWord();
            if (fp.ShowDialog() == DialogResult.Yes)
            {
                frmSet fs = new frmSet();
                fs.Show();
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            fPassWord = new frmPassWord();
            if (fPassWord.ShowDialog() == DialogResult.Yes)
            {
                frmKB fs = new frmKB();
                fs.Show();
            }
            fPassWord.Dispose();
            fPassWord = null;
        }
        private void toolStripButton3_Click_1(object sender, EventArgs e)
        {
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            fPassWord = new frmPassWord();
            if (fPassWord.ShowDialog() == DialogResult.Yes)
            {
                frmSys fs = new frmSys();
                fs.Show();
            }
            fPassWord.Dispose();
            fPassWord = null;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            frmDataShow fd = new frmDataShow();
            fd.Show();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("是否确定要退出测试程序?", "请选择", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void btnHandle_Click(object sender, EventArgs e)
        {
            frmTest ft = new frmTest();
            ft.Show();
        }

        private void toolBtnCancel_Click(object sender, EventArgs e)
        {
            if (nowStatue.isStarting)
            {
                nowStatue.HandlePause = !nowStatue.HandlePause;
                if (!nowStatue.HandlePause)
                {
                    nowStatue.StepStartTime += Environment.TickCount / 1000 - nowStatue.HoldTime;
                }
                else
                {
                    nowStatue.HoldTime = Environment.TickCount / 1000;
                }
            }
            SetBtnCancel();
        }
        Image pause = null;
        Image resume = null;
        private void SetBtnCancel()
        {
            if (pause == null || resume == null)
            {
                pause = toolBtnCancel.Image;
                resume = toolBtnOk.Image;
            }
            toolBtnCancel.Text = (nowStatue.HandlePause ? "恢复" : "暂停");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            toolBtnCancel.Image = nowStatue.HandlePause ? resume : pause;
        }

        private void btnMath_Click(object sender, EventArgs e)
        {
        }

        private void btnList_Click(object sender, EventArgs e)
        {
            frmList fl = new frmList();
            if (fl.ShowDialog() == DialogResult.Yes)
            {
                if (!fl.isError)
                {
                    cMain.mSysSet.mPrevId = fl.ReturnId.Substring(fl.ReturnId.LastIndexOf('\\') + 1, fl.ReturnId.LastIndexOf('.') - fl.ReturnId.LastIndexOf('\\') - 1);
                    frmSys.DataClassToFile(cMain.mSysSet);
                    frmMain.initTestData(this);
                }
            }
            fl.Dispose();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            cPrint cp = new cPrint();
            cp.SetValue(cMain.mAllResult);
        }

        private void toolBtnOk_Click(object sender, EventArgs e)
        {
            Handle_SF = true;
        }

    }
}