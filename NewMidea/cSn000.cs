using System;
using System.Collections.Generic;
using System.Text;
using NewMideaProgram;
using System.IO;
using System.IO.Ports;
namespace System
{
    class cSn000
    {
        public class BaoHeDu
        {
            float press = 0;
            public float Press
            {
                get { return press; }
                set { press = value; }
            }
            float temp = 0;

            public float Temp
            {
                get { return temp; }
                set { temp = value; }
            }
            public BaoHeDu(float press, float temp)
            {
                this.Press = press;
                this.Temp = temp;
            }
        }
        static List<BaoHeDu> TR410BaoHeDu = new List<BaoHeDu>();
        SerialPort comPort;//端口
        /// <summary>
        /// 
        /// </summary>
        public SerialPort ComPort
        {
            get { return comPort; }
            set { comPort = value; }
        }
        string errStr = "LGPLC";//出错信息
        /// <summary>
        /// 错误返回信息
        /// </summary>
        public string ErrStr
        {
            get { return errStr; }
            set { errStr = value; }
        }
        int sendDataCount = 0;
        public cSn000(SerialPort mComPort)
        {
            comPort = mComPort;
        }
        public bool Set(int BoTeLv, Parity XiaoYan, int ShuJu, StopBits TingZhi)
        {
            bool isOk = false;
            try
            {
                if (comPort.IsOpen)
                {
                    comPort.Close();
                }
                comPort.BaudRate = BoTeLv;
                comPort.Parity = XiaoYan;
                comPort.DataBits = ShuJu;
                comPort.StopBits = TingZhi;
                comPort.Open();
                isOk = true;
            }
            catch
            {
                isOk = false;
            }
            return isOk;
        }
        public void Send(byte[] send)
        {
            if (comPort != null && send != null)
            {
                if (comPort.IsOpen)
                {
                    comPort.DiscardInBuffer();
                    comPort.Write(send, 0, send.Length);
                }
            }
        }
        public void Send(string send)
        {
            if (comPort != null)
            {
                if (comPort.IsOpen)
                {
                    send = send.Trim();
                    if ((send.Length % 2) == 1)
                    {
                        send = string.Format("{0}0", send);
                    }
                    Send(send.ToBytes());
                }
            }
        }
        /// <summary>
        /// 读取SN的5个数据，频率，冷凝，环境，排气，备用
        /// </summary>
        /// <param name="ReturnBuff"></param>
        /// <returns></returns>
        public bool Read(out double[] ReturnBuff)
        {
            bool isShowData;
            return Read(out ReturnBuff,out isShowData);
        }
        public bool Read(out double[] ReturnBuff, out bool isShowData)
        {
            int len;
            return Read(out ReturnBuff, out isShowData, out len);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ReturnBuff"></param>
        /// <param name="isShowData">bool,是否为显示数据||错误代码</param>
        /// <returns></returns>
        public bool Read(out double[] ReturnBuff, out bool isShowData, out int len)
        {
            long[] tmpBuff;
            bool isOk = false;
            ReturnBuff = new double[5];
            isShowData = true;
            isOk = SnBoardReadCmd(out tmpBuff, out len, out isShowData);
            if (isOk)
            {
                switch (len)
                {
                    case 36:
                    case 40:
                        if (isShowData)
                        {
                            ReturnBuff[0] = tmpBuff[1];//频率
                            ReturnBuff[1] = TurnTemp(tmpBuff[2], false);//冷凝
                            ReturnBuff[2] = TurnTemp(tmpBuff[3], false);//室温
                            ReturnBuff[3] = TurnTemp(tmpBuff[4], true);//排气
                            ReturnBuff[4] = 0;
                        }
                        else
                        {
                            ReturnBuff[0] = tmpBuff[1];//错误一
                            ReturnBuff[1] = tmpBuff[2];//错误一
                            ReturnBuff[2] = tmpBuff[3];//错误一
                            ReturnBuff[3] = tmpBuff[4];//错误四
                            ReturnBuff[4] = tmpBuff[5];//错误五
                        }
                        break;
                    case 12://1023波特率 
                        ReturnBuff[0] = 0;//频率
                        ReturnBuff[1] = TurnTemp(tmpBuff[2], false);
                        ReturnBuff[2] = 0;
                        ReturnBuff[3] = 0;
                        ReturnBuff[4] = 0;
                        break;
                    case 16://1200波特率
                        ReturnBuff[0] = 0;//频率
                        ReturnBuff[1] = TurnTemp(tmpBuff[2], false);
                        ReturnBuff[2] = TurnTemp(tmpBuff[3], false);
                        ReturnBuff[3] = 0;
                        ReturnBuff[4] = 0;
                        break;
                    case 28:
                        ReturnBuff[0] = tmpBuff[0];//物料编码
                        ReturnBuff[1] = tmpBuff[1];//版本号
                        ReturnBuff[2] = 0;
                        ReturnBuff[3] = 0;
                        ReturnBuff[4] = 0;
                        break;
                    default:
                        isOk = false;
                        break;
                }
            }
            return isOk;
        }
        /// <summary>
        /// 读取SN板上外机返回指令,前6位数据(接收指令长度,频率,冷凝,室温,排气温度,T3)
        /// </summary>
        /// <param name="ReturnBuff">数组,将要返回的数据</param>
        /// <returns>返回读取变频板是否成功</returns>
        private bool SnBoardReadCmd(out long[] ReturnBuff, out int len, out bool isShowData)//读SN板
        {
            ReturnBuff = new long[6];
            len = 0;
            isShowData = true;
            try
            {
                int returnCount = comPort.BytesToRead;
                byte[] readBuff = new byte[returnCount];
                comPort.Read(readBuff, 0, returnCount);
                if (returnCount <= 0)
                {
                    return false;
                }
                //cMain.WriteErrorToLog(string.Format("{0}_Read_{1}", comPort.PortName, readBuff.ToHexString()));
                if ((returnCount >= (sendDataCount * 2)) && (returnCount >= 10))
                {
                    switch (returnCount)
                    {
                        case 36:
                            ReturnBuff[1] = readBuff[5 + 18];
                            ReturnBuff[2] = readBuff[8 + 18];
                            ReturnBuff[3] = readBuff[9 + 18];
                            ReturnBuff[4] = readBuff[10 + 18];
                            ReturnBuff[5] = 0;
                            len = 36;
                            break;
                        case 40:
                            if (readBuff[3] == 0x20)
                            {
                                ReturnBuff[1] = readBuff[6 + 20];
                                ReturnBuff[2] = readBuff[9 + 20];
                                ReturnBuff[3] = readBuff[10 + 20];
                                ReturnBuff[4] = readBuff[11 + 20];
                                ReturnBuff[5] = 0;
                            }
                            if (readBuff[3] == 0x50)
                            {
                                ReturnBuff[1] = readBuff[11 + 20];
                                ReturnBuff[2] = readBuff[6 + 20];
                                ReturnBuff[3] = readBuff[7 + 20];
                                ReturnBuff[4] = readBuff[8 + 20];
                                ReturnBuff[5] = readBuff[9 + 20];
                                isShowData = false;
                            }
                            len = 40;
                            break;
                        case 28:
                            if (readBuff[3] == 0x25)
                            {
                                switch (readBuff[5])
                                {
                                    case 0x09://读版本号
                                        ReturnBuff[0] = ((readBuff[19] & 0x80) << 16) + (readBuff[18] << 8) + readBuff[17];
                                        ReturnBuff[1] = (readBuff[19] & 0x7F);//版本号
                                        break;
                                }
                            }
                            len = 28;
                            break;
                        case 12:
                            ReturnBuff[1] = 0;
                            ReturnBuff[2] = readBuff[1 + 6];
                            ReturnBuff[3] = 0;
                            ReturnBuff[4] = 0;
                            ReturnBuff[5] = 0;
                            len = 12;
                            break;
                        case 16:
                            ReturnBuff[1] = 0;
                            ReturnBuff[2] = readBuff[2 + 8];
                            ReturnBuff[3] = readBuff[3 + 8];
                            ReturnBuff[4] = 0;
                            ReturnBuff[5] = 0;
                            len = 16;
                            break;
                    }
                    return true;
                }
            }
            catch (Exception exc)
            {
                cMain.WriteErrorToLog("cSn000:SnBoardReadCmd." + exc.ToString());
                return false;
            }
            return false;
        }
        /// <summary>
        /// 将读到的数据转化为温度数据
        /// </summary>
        /// <param name="ReadTemp">long,读到的数据</param>
        /// <param name="isPaiQi">bool,是不是排气温度,因为排气温度的算法不同</param>
        /// <returns>double,返回转化后的实际温度数据</returns>
        private double TurnTemp(long ReadTemp, bool isPaiQi)
        {
            double returnValue = 0;
            double R0 = 8.06;
            double T25 = 25;
            double R25 = 0;
            double B = 0;
            double Rt = 0;
            if (isPaiQi)
            {
                R25 = 56.1048;
                B = 3950;
            }
            else
            {
                R25 = 10;
                B = 4100;
            }
            Rt = ((double)255 / (double)ReadTemp - (double)1) * R0;
            returnValue = (T25 + 273.15) * B / (B - (T25 + 273.15) * Math.Log(R25 / Rt)) - 273.15;
            return returnValue;
        }
        /// <summary>
        /// 将错误指令码转化为错误信息
        /// </summary>
        /// <param name="errData">double[],外机返回的错误指令码</param>
        /// <returns>读取到的错误信息</returns>
        public static string FlushError(double[] errData)
        {
            string errShow = "";
            if (errData[0] != 0)
            {
                if (((int)errData[0] & 1) == 1)
                {
                    errShow = errShow + "室外E方故障";
                }
                if (((int)errData[0] & 2) == 2)
                {
                    errShow = errShow + "室外T3传感器故障";
                }
                if (((int)errData[0] & 4) == 4)
                {
                    errShow = errShow + "室外T4传感器故障";
                }
                if (((int)errData[0] & 8) == 8)
                {
                    errShow = errShow + "室外排气传感器故障";
                }
                if (((int)errData[0] & 16) == 16)
                {
                    errShow = errShow + "室外回气传感器故障";
                }
                if (((int)errData[0] & 32) == 32)
                {
                    errShow = errShow + "压顶传感器温度故障";
                }
                if (((int)errData[0] & 64) == 64)
                {
                    errShow = errShow + "室外直流风机故障";
                }
                if (((int)errData[0] & 128) == 128)
                {
                    errShow = errShow + "输入交流电流采样故障";
                }
            }
            if (errData[1] != 0)
            {
                if (((int)errData[1] & 1) == 1)
                {
                    errShow = errShow + "主控芯片与驱动芯片通信故障";
                }
                if (((int)errData[1] & 2) == 2)
                {
                    errShow = errShow + "压机电流采样电路故障";
                }
                if (((int)errData[1] & 4) == 4)
                {
                    errShow = errShow + "压机启动故障";
                }
                if (((int)errData[1] & 8) == 8)
                {
                    errShow = errShow + "压机缺相保护";
                }
                if (((int)errData[1] & 16) == 16)
                {
                    errShow = errShow + "压机零速保护";
                }
                if (((int)errData[1] & 32) == 32)
                {
                    errShow = errShow + "室外341主芯片驱动同步故障";
                }
                if (((int)errData[1] & 64) == 64)
                {
                    errShow = errShow + "压机失速保护";
                }
                if (((int)errData[1] & 128) == 128)
                {
                    errShow = errShow + "压机锁定保护";
                }
            }
            if (errData[2] != 0)
            {
                if (((int)errData[2] & 1) == 1)
                {
                    errShow = errShow + "压机脱调保护";
                }
                if (((int)errData[2] & 2) == 2)
                {
                    errShow = errShow + "压机过电流故障";
                }
                if (((int)errData[2] & 4) == 4)
                {
                    errShow = errShow + "室外IPM模块保护";
                }
                if (((int)errData[2] & 8) == 8)
                {
                    errShow = errShow + "电压过低保护";
                }
                if (((int)errData[2] & 16) == 16)
                {
                    errShow = errShow + "电压过高保护";
                }
                if (((int)errData[2] & 32) == 32)
                {
                    errShow = errShow + "室外直流侧电压保护";
                }
                if (((int)errData[2] & 64) == 64)
                {
                    errShow = errShow + "室外电流保护";
                }
            }
            if (errData[3] != 0)
            {
                if (((int)errData[3] & 16) == 16)
                {
                    errShow = errShow + "系统高压限频";
                }
                if (((int)errData[3] & 32) == 32)
                {
                    errShow = errShow + "系统高压保护";
                }
                if (((int)errData[3] & 64) == 64)
                {
                    errShow = errShow + "系统低压限频";
                }
                if (((int)errData[3] & 128) == 128)
                {
                    errShow = errShow + "系统低压保护";
                }
            }
            if (errData[4] != 0)
            {
                if (((int)errData[4] & 1) == 1)
                {
                    errShow = errShow + "电压限频";
                }
                if (((int)errData[4] & 2) == 2)
                {
                    errShow = errShow + "电流限频";
                }
                if (((int)errData[4] & 4) == 4)
                {
                    errShow = errShow + "PFC模块开关停机";
                }
                if (((int)errData[4] & 8) == 8)
                {
                    errShow = errShow + "PFC模块故障限频";
                }
                if (((int)errData[4] & 16) == 16)
                {
                    errShow = errShow + "341MCE故障";
                }
                if (((int)errData[4] & 32) == 32)
                {
                    errShow = errShow + "341同步故障";
                }
            }
            return errShow;
        }
        public static float R410BaoHeDu(double yaLi)
        {
            if (TR410BaoHeDu.Count <= 0)
            {
                InitBaoHeDu();
            }
            return R410BaoHeDu(0, TR410BaoHeDu.Count - 1, yaLi+0.1f);
        }
        /// <summary>
        /// 中值法查表
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="yaLi"></param>
        /// <returns></returns>
        static float R410BaoHeDu(int start, int end, double yaLi)
        {
            float result = 0;
            if (start >= 0 && start < TR410BaoHeDu.Count && end >= 0 && end < TR410BaoHeDu.Count)
            {
                if ((end - start) >= 2)
                {
                    int midIndex = (int)((end + start) / 2);
                    if (TR410BaoHeDu[midIndex].Press < yaLi)
                    {
                        result = R410BaoHeDu(midIndex, end, yaLi);
                    }
                    else
                    {
                        result = R410BaoHeDu(start, midIndex, yaLi);
                    }
                }
                else
                {
                    float midPress = (TR410BaoHeDu[end].Press/2.0f + TR410BaoHeDu[start].Press/2.0f);//取2个压力中间值
                    if (midPress < yaLi)//靠后面，则取下一个值
                    {
                        result = TR410BaoHeDu[end].Temp;
                    }
                    else
                    {
                        result = TR410BaoHeDu[start].Temp;
                    }                    
                }
            }
            return result;
        }
        #region//R410
        public static void InitBaoHeDu()
        {
            TR410BaoHeDu.Add(new BaoHeDu(0.052f, -65));
            TR410BaoHeDu.Add(new BaoHeDu(0.054f, -64));
            TR410BaoHeDu.Add(new BaoHeDu(0.057f, -63));
            TR410BaoHeDu.Add(new BaoHeDu(0.061f, -62));
            TR410BaoHeDu.Add(new BaoHeDu(0.064f, -61));
            TR410BaoHeDu.Add(new BaoHeDu(0.068f, -60));
            TR410BaoHeDu.Add(new BaoHeDu(0.072f, -59));
            TR410BaoHeDu.Add(new BaoHeDu(0.076f, -58));
            TR410BaoHeDu.Add(new BaoHeDu(0.080f, -57));
            TR410BaoHeDu.Add(new BaoHeDu(0.084f, -56));
            TR410BaoHeDu.Add(new BaoHeDu(0.089f, -55));
            TR410BaoHeDu.Add(new BaoHeDu(0.093f, -54));
            TR410BaoHeDu.Add(new BaoHeDu(0.098f, -53));
            TR410BaoHeDu.Add(new BaoHeDu(0.103f, -52));
            TR410BaoHeDu.Add(new BaoHeDu(0.108f, -51));
            TR410BaoHeDu.Add(new BaoHeDu(0.113f, -50));
            TR410BaoHeDu.Add(new BaoHeDu(0.119f, -49));
            TR410BaoHeDu.Add(new BaoHeDu(0.125f, -48));
            TR410BaoHeDu.Add(new BaoHeDu(0.131f, -47));
            TR410BaoHeDu.Add(new BaoHeDu(0.138f, -46));
            TR410BaoHeDu.Add(new BaoHeDu(0.144f, -45));
            TR410BaoHeDu.Add(new BaoHeDu(0.151f, -44));
            TR410BaoHeDu.Add(new BaoHeDu(0.157f, -43));
            TR410BaoHeDu.Add(new BaoHeDu(0.165f, -42));
            TR410BaoHeDu.Add(new BaoHeDu(0.172f, -41));
            TR410BaoHeDu.Add(new BaoHeDu(0.181f, -40));
            TR410BaoHeDu.Add(new BaoHeDu(0.188f, -39));
            TR410BaoHeDu.Add(new BaoHeDu(0.196f, -38));
            TR410BaoHeDu.Add(new BaoHeDu(0.206f, -37));
            TR410BaoHeDu.Add(new BaoHeDu(0.215f, -36));
            TR410BaoHeDu.Add(new BaoHeDu(0.224f, -35));
            TR410BaoHeDu.Add(new BaoHeDu(0.235f, -34));
            TR410BaoHeDu.Add(new BaoHeDu(0.243f, -33));
            TR410BaoHeDu.Add(new BaoHeDu(0.255f, -32));
            TR410BaoHeDu.Add(new BaoHeDu(0.264f, -31));
            TR410BaoHeDu.Add(new BaoHeDu(0.275f, -30));
            TR410BaoHeDu.Add(new BaoHeDu(0.286f, -29));
            TR410BaoHeDu.Add(new BaoHeDu(0.298f, -28));
            TR410BaoHeDu.Add(new BaoHeDu(0.311f, -27));
            TR410BaoHeDu.Add(new BaoHeDu(0.324f, -26));
            TR410BaoHeDu.Add(new BaoHeDu(0.334f, -25));
            TR410BaoHeDu.Add(new BaoHeDu(0.348f, -24));
            TR410BaoHeDu.Add(new BaoHeDu(0.363f, -23));
            TR410BaoHeDu.Add(new BaoHeDu(0.375f, -22));
            TR410BaoHeDu.Add(new BaoHeDu(0.391f, -21));
            TR410BaoHeDu.Add(new BaoHeDu(0.404f, -20));
            TR410BaoHeDu.Add(new BaoHeDu(0.424f, -19));
            TR410BaoHeDu.Add(new BaoHeDu(0.435f, -18));
            TR410BaoHeDu.Add(new BaoHeDu(0.453f, -17));
            TR410BaoHeDu.Add(new BaoHeDu(0.468f, -16));
            TR410BaoHeDu.Add(new BaoHeDu(0.483f, -15));
            TR410BaoHeDu.Add(new BaoHeDu(0.504f, -14));
            TR410BaoHeDu.Add(new BaoHeDu(0.520f, -13));
            TR410BaoHeDu.Add(new BaoHeDu(0.538f, -12));
            TR410BaoHeDu.Add(new BaoHeDu(0.556f, -11));
            TR410BaoHeDu.Add(new BaoHeDu(0.579f, -10));
            TR410BaoHeDu.Add(new BaoHeDu(0.598f, -09));
            TR410BaoHeDu.Add(new BaoHeDu(0.618f, -08));
            TR410BaoHeDu.Add(new BaoHeDu(0.639f, -07));
            TR410BaoHeDu.Add(new BaoHeDu(0.660f, -06));
            TR410BaoHeDu.Add(new BaoHeDu(0.682f, -05));
            TR410BaoHeDu.Add(new BaoHeDu(0.705f, -04));
            TR410BaoHeDu.Add(new BaoHeDu(0.728f, -03));
            TR410BaoHeDu.Add(new BaoHeDu(0.752f, -02));
            TR410BaoHeDu.Add(new BaoHeDu(0.777f, -01));
            TR410BaoHeDu.Add(new BaoHeDu(0.803f, 00));
            TR410BaoHeDu.Add(new BaoHeDu(0.823f, 01));
            TR410BaoHeDu.Add(new BaoHeDu(0.851f, 02));
            TR410BaoHeDu.Add(new BaoHeDu(0.879f, 03));
            TR410BaoHeDu.Add(new BaoHeDu(0.903f, 04));
            TR410BaoHeDu.Add(new BaoHeDu(0.937f, 05));
            TR410BaoHeDu.Add(new BaoHeDu(0.962f, 06));
            TR410BaoHeDu.Add(new BaoHeDu(0.994f, 07));
            TR410BaoHeDu.Add(new BaoHeDu(1.02f, 08));
            TR410BaoHeDu.Add(new BaoHeDu(1.05f, 09));
            TR410BaoHeDu.Add(new BaoHeDu(1.09f, 10));
            TR410BaoHeDu.Add(new BaoHeDu(1.11f, 11));
            TR410BaoHeDu.Add(new BaoHeDu(1.15f, 12));
            TR410BaoHeDu.Add(new BaoHeDu(1.18f, 13));
            TR410BaoHeDu.Add(new BaoHeDu(1.22f, 14));
            TR410BaoHeDu.Add(new BaoHeDu(1.25f, 15));
            TR410BaoHeDu.Add(new BaoHeDu(1.28f, 16));
            TR410BaoHeDu.Add(new BaoHeDu(1.32f, 17));
            TR410BaoHeDu.Add(new BaoHeDu(1.35f, 18));
            TR410BaoHeDu.Add(new BaoHeDu(1.40f, 19));
            TR410BaoHeDu.Add(new BaoHeDu(1.44f, 20));
            TR410BaoHeDu.Add(new BaoHeDu(1.47f, 21));
            TR410BaoHeDu.Add(new BaoHeDu(1.52f, 22));
            TR410BaoHeDu.Add(new BaoHeDu(1.56f, 23));
            TR410BaoHeDu.Add(new BaoHeDu(1.60f, 24));
            TR410BaoHeDu.Add(new BaoHeDu(1.64f, 25));
            TR410BaoHeDu.Add(new BaoHeDu(1.68f, 26));
            TR410BaoHeDu.Add(new BaoHeDu(1.73f, 27));
            TR410BaoHeDu.Add(new BaoHeDu(1.78f, 28));
            TR410BaoHeDu.Add(new BaoHeDu(1.82f, 29));
            TR410BaoHeDu.Add(new BaoHeDu(1.88f, 30));
            TR410BaoHeDu.Add(new BaoHeDu(1.91f, 31));
            TR410BaoHeDu.Add(new BaoHeDu(1.96f, 32));
            TR410BaoHeDu.Add(new BaoHeDu(2.03f, 33));
            TR410BaoHeDu.Add(new BaoHeDu(2.08f, 34));
            TR410BaoHeDu.Add(new BaoHeDu(2.13f, 35));
            TR410BaoHeDu.Add(new BaoHeDu(2.18f, 36));
            TR410BaoHeDu.Add(new BaoHeDu(2.24f, 37));
            TR410BaoHeDu.Add(new BaoHeDu(2.29f, 38));
            TR410BaoHeDu.Add(new BaoHeDu(2.35f, 39));
            TR410BaoHeDu.Add(new BaoHeDu(2.41f, 40));
            TR410BaoHeDu.Add(new BaoHeDu(2.46f, 41));
            TR410BaoHeDu.Add(new BaoHeDu(2.51f, 42));
            TR410BaoHeDu.Add(new BaoHeDu(2.58f, 43));
            TR410BaoHeDu.Add(new BaoHeDu(2.65f, 44));
            TR410BaoHeDu.Add(new BaoHeDu(2.71f, 45));
            TR410BaoHeDu.Add(new BaoHeDu(2.77f, 46));
            TR410BaoHeDu.Add(new BaoHeDu(2.84f, 47));
            TR410BaoHeDu.Add(new BaoHeDu(2.91f, 48));
            TR410BaoHeDu.Add(new BaoHeDu(2.98f, 49));
            TR410BaoHeDu.Add(new BaoHeDu(3.05f, 50));
            TR410BaoHeDu.Add(new BaoHeDu(3.10f, 51));
            TR410BaoHeDu.Add(new BaoHeDu(3.18f, 52));
            TR410BaoHeDu.Add(new BaoHeDu(3.25f, 53));
            TR410BaoHeDu.Add(new BaoHeDu(3.32f, 54));
            TR410BaoHeDu.Add(new BaoHeDu(3.40f, 55));
            TR410BaoHeDu.Add(new BaoHeDu(3.48f, 56));
            TR410BaoHeDu.Add(new BaoHeDu(3.54f, 57));
            TR410BaoHeDu.Add(new BaoHeDu(3.63f, 58));
            TR410BaoHeDu.Add(new BaoHeDu(3.72f, 59));
            TR410BaoHeDu.Add(new BaoHeDu(3.78f, 60));
            TR410BaoHeDu.Add(new BaoHeDu(3.90f, 61));
            TR410BaoHeDu.Add(new BaoHeDu(3.97f, 62));
        }
        #endregion
    }
}
