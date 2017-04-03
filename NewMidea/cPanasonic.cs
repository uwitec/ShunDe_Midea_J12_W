using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Threading;
using NewMideaProgram;
namespace System
{
    class cPanasonic
    {
        public static string mName = "PanasonicPlc\r\n" +
                                    "PanasonicPlc:初始化PLC \r\n" +
                                    "PanasonicPlc_Read:以MODEBUF方式读取PLC\r\n" +
                                    "PanasonicPlc_ReadD:读取PLC单个D点\r\n" +
                                    "PanasonicPlc_ReadM:读取PLC单个M点\r\n" +
                                    "PanasonicPlc_WriteM:写入PLC单个M点\r\n" +
                                    "PanasonicPlc_WriteD:写入PLC单个D点\r\n";
        SerialPort comPort;//端口
        /// <summary>
        /// PanasonicPlc使用的串口
        /// </summary>
        public SerialPort ComPort
        {
            get { return comPort; }
            set { comPort = value; }
        }
        string errStr = "松下PLC:";//出错信息
        /// <summary>
        /// 错误返回信息
        /// </summary>
        public string ErrStr
        {
            get { return errStr; }
            set { errStr = value; }
        }
        int timeOut = 500;//超时时间(ms)
        /// <summary>
        ///读写串口超时时间,单位(ms)
        /// </summary>
        public int TimeOut
        {
            get { return timeOut; }
            set { timeOut = value; }
        }
        byte PanasonicPlcAddress = 0;//PanasonicPlc地址
        bool mPanasonicPlcIsInit = false;//PLC初始化结果
        /// <summary>
        /// PanasonicPlc的构造函数
        /// </summary>
        /// <param name="mComPort">PanasonicPlc使用的串口</param>
        /// <param name="mAddress">PanasonicPlc的地址</param>
        public cPanasonic(SerialPort mComPort, byte mAddress)
        {
            comPort = mComPort;
            PanasonicPlcAddress = mAddress;
        }
        public bool Init()
        {
            if (cMain.isDebug)
            {
                mPanasonicPlcIsInit = true;
                return true;
            }
            string WriteBuff = "";//发送数据
            byte[] ReadBuff = new byte[20];//接收数据
            int ReturnByte = 0;//返回数据
            bool IsReturn = false;//是否成功返回
            bool IsTimeOut = false;//是否超时
            DateTime NowTime = DateTime.Now;//当前时间
            string CrcHi = "", CrcLo = "";
            TimeSpan ts;//时间差
            try
            {
                if (!comPort.IsOpen)
                {
                    comPort.Open();
                }
                WriteBuff = string.Format("<{0:D2}#RCSX0000", PanasonicPlcAddress);
                Crc(WriteBuff, ref CrcLo, ref CrcHi);
                WriteBuff = WriteBuff + CrcLo + CrcHi + "\r\n";
                comPort.DiscardInBuffer();
                comPort.Write(WriteBuff);
                NowTime = DateTime.Now;
                do
                {
                    Thread.Sleep(20);
                    if (comPort.BytesToRead >= 10)//收到数据
                    {
                        ReturnByte = comPort.BytesToRead;
                        IsReturn = true;
                    }
                    ts = DateTime.Now - NowTime;
                    if (ts.TotalMilliseconds > timeOut)//时间超时
                    {
                        IsTimeOut = true;
                    }
                } while (!IsReturn && !IsTimeOut);
                if (!IsReturn && IsTimeOut)//超时
                {
                    if (ErrStr.IndexOf("接收数据已超时") < 0)
                    {
                        ErrStr = ErrStr + DateTime.Now.ToString() + ",PanasonicPlcInit," + "初始失败,接收数据已超时" + (char)13 + (char)10;
                    }
                    return false;
                }
                else
                {
                    comPort.Read(ReadBuff, 0, ReturnByte);
                    string readStr = Encoding.ASCII.GetString(ReadBuff, 0, ReturnByte);
                    if (readStr.IndexOf('!') > 0)//数据检验失败
                    {
                        if (ErrStr.IndexOf("接收数据错误") < 0)
                        {
                            ErrStr = ErrStr + DateTime.Now.ToString() + ",PanasonicPlcInit," + ":初始失败,接收数据错误" + (char)13 + (char)10;
                        }
                        return false;
                    }
                    else
                    {
                        mPanasonicPlcIsInit = true;

                    }
                }
            }
            catch (Exception exc)
            {
                if (ErrStr.IndexOf(exc.ToString()) < 0)
                {
                    ErrStr = ErrStr + DateTime.Now.ToString() + ",StandarBoardInit," + ":" + exc.ToString() + (char)13 + (char)10;
                }
                return false;
            }
            return true;
        }
        public bool ReadR(int RValue, ref bool value)
        {
            bool[] b = new bool[1];
            bool isOk = false;
            isOk = ReadR(RValue, 1, ref b);
            value = b[0];
            return isOk;
        }
        public bool ReadR(int RValue, int length, ref bool[] value)
        {
            if (!mPanasonicPlcIsInit)
            {
                Init();
                return false;
            }
            bool isOK = false;
            string WriteStr = "";//发送数据
            byte[] WriteBuff = new byte[17];
            byte[] ReadBuff = new byte[20];//接收数据
            int ReturnByte = 0;//返回数据
            bool IsReturn = false;//是否成功返回
            bool IsTimeOut = false;//是否超时
            DateTime NowTime = DateTime.Now;//当前时间
            string CrcHi = "", CrcLo = "";
            TimeSpan ts;//时间差
            try
            {
                if (!comPort.IsOpen)
                {
                    comPort.Open();
                }
                WriteStr = string.Format("<{0:D2}#RCP{1}", PanasonicPlcAddress, length);
                for (int i = 0; i < length; i++)
                {
                    WriteStr = WriteStr + string.Format("R{0:X4}", RValue + i);
                }
                Crc(WriteStr, ref CrcLo, ref CrcHi);
                WriteStr = WriteStr + CrcLo + CrcHi + "\r\n";
                comPort.DiscardInBuffer();
                WriteBuff = Encoding.ASCII.GetBytes(WriteStr);
                comPort.Write(WriteBuff, 0, WriteBuff.Length);
                NowTime = DateTime.Now;
                do
                {
                    Thread.Sleep(20);
                    if (comPort.BytesToRead >= 9 + length)//收到数据
                    {
                        ReturnByte = comPort.BytesToRead;
                        IsReturn = true;
                    }
                    ts = DateTime.Now - NowTime;
                    if (ts.TotalMilliseconds > timeOut)//时间超时
                    {
                        IsTimeOut = true;
                    }
                } while (!IsReturn && !IsTimeOut);
                if (!IsReturn && IsTimeOut)//超时
                {
                    if (ErrStr.IndexOf("接收数据已超时") < 0)
                    {
                        ErrStr = ErrStr + DateTime.Now.ToString() + ",PanasonicPlcRead," + "初始失败,接收数据已超时" + (char)13 + (char)10;
                    }
                    isOK = false;
                }
                else
                {
                    comPort.Read(ReadBuff, 0, ReturnByte);
                    if (ReadBuff[0] != WriteBuff[0] || ReadBuff[1] != WriteBuff[1] || ReadBuff[2] != WriteBuff[2] || ReadBuff[3] == 33)//数据检验失败
                    {
                        if (ErrStr.IndexOf("接收数据错误") < 0)
                        {
                            ErrStr = ErrStr + DateTime.Now.ToString() + ",PanasonicPlcRead," + ":初始失败,接收数据错误" + (char)13 + (char)10;
                        }
                        isOK = false;
                    }
                    else
                    {
                        for (int i = 0; i < length; i++)
                        {
                            if (ReadBuff[6 + i] == 49)
                            {
                                value[i] = true;
                            }
                            else
                            {
                                value[i] = false;
                            }
                        }
                        isOK = true;
                    }
                }
            }
            catch (Exception exc)
            {
                if (ErrStr.IndexOf(exc.ToString()) < 0)
                {
                    ErrStr = ErrStr + DateTime.Now.ToString() + ",StandarBoardInit," + ":" + exc.ToString() + (char)13 + (char)10;
                }
                isOK = false;
            }
            return isOK;
        }
        public bool WriteD(int DStartValue, int Length, long[] value)
        {

            if (!mPanasonicPlcIsInit)
            {
                Init();
                return false;
            }
            bool isOK = false;
            string WriteStr = "";//发送数据
            byte[] WriteBuff = new byte[20 + Length * 4];
            byte[] ReadBuff = new byte[20];//接收数据
            int ReturnByte = 0;//返回数据
            bool IsReturn = false;//是否成功返回
            bool IsTimeOut = false;//是否超时
            DateTime NowTime = DateTime.Now;//当前时间
            string CrcHi = "", CrcLo = "";
            TimeSpan ts;//时间差
            try
            {
                if (!comPort.IsOpen)
                {
                    comPort.Open();
                }
                WriteStr = string.Format("<{0:D2}#WDD{1:D5}{2:D5}", PanasonicPlcAddress, DStartValue, DStartValue + Length - 1);
                for (int i = 0; i < Length; i++)
                {
                    WriteStr = WriteStr + string.Format("{0:X4}", value[i]).Substring(2, 2) + string.Format("{0:X4}", value[i]).Substring(0, 2);
                }
                Crc(WriteStr, ref CrcLo, ref CrcHi);
                WriteStr = WriteStr + CrcLo + CrcHi + "\r\n";
                comPort.DiscardInBuffer();
                WriteBuff = Encoding.ASCII.GetBytes(WriteStr);
                comPort.Write(WriteBuff, 0, WriteBuff.Length);
                NowTime = DateTime.Now;
                do
                {
                    Thread.Sleep(20);
                    if (comPort.BytesToRead >= 9)//收到数据
                    {
                        ReturnByte = comPort.BytesToRead;
                        IsReturn = true;
                    }
                    ts = DateTime.Now - NowTime;
                    if (ts.TotalMilliseconds > timeOut)//时间超时
                    {
                        IsTimeOut = true;
                    }
                } while (!IsReturn && !IsTimeOut);
                if (!IsReturn && IsTimeOut)//超时
                {
                    if (ErrStr.IndexOf("接收数据已超时") < 0)
                    {
                        ErrStr = ErrStr + DateTime.Now.ToString() + ",PanasonicPlcRead," + "初始失败,接收数据已超时" + (char)13 + (char)10;
                    }
                    isOK = false;
                }
                else
                {
                    comPort.Read(ReadBuff, 0, ReturnByte);
                    if (ReadBuff[0] != WriteBuff[0] || ReadBuff[1] != WriteBuff[1] || ReadBuff[2] != WriteBuff[2] || ReadBuff[3] == 33)//数据检验失败
                    {
                        if (ErrStr.IndexOf("接收数据错误") < 0)
                        {
                            ErrStr = ErrStr + DateTime.Now.ToString() + ",PanasonicPlcRead," + ":初始失败,接收数据错误" + (char)13 + (char)10;
                        }
                        isOK = false;
                    }
                    else
                    {
                        isOK = true;
                    }
                }
            }
            catch (Exception exc)
            {
                if (ErrStr.IndexOf(exc.ToString()) < 0)
                {
                    ErrStr = ErrStr + DateTime.Now.ToString() + ",StandarBoardInit," + ":" + exc.ToString() + (char)13 + (char)10;
                }
                isOK = false;
            }
            return isOK;
        }

        public bool ReadD(int DStartValue, ref long value)
        {
            bool isOk = false;
            long[] l = new long[1];
            isOk = ReadD(DStartValue, 1, ref l);
            value = l[0];
            return isOk;
        }
        public bool ReadD(int DStartValue, int Length, ref long[] value)
        {
            if (!mPanasonicPlcIsInit)
            {
                Init();
                return false;
            }
            bool isOK = false;
            string WriteStr = "";//发送数据
            byte[] WriteBuff = new byte[17];
            byte[] ReadBuff = new byte[20];//接收数据
            int ReturnByte = 0;//返回数据
            bool IsReturn = false;//是否成功返回
            bool IsTimeOut = false;//是否超时
            DateTime NowTime = DateTime.Now;//当前时间
            string CrcHi = "", CrcLo = "";
            TimeSpan ts;//时间差
            try
            {
                if (!comPort.IsOpen)
                {
                    comPort.Open();
                }
                WriteStr = string.Format("<{0:D2}#RDD{1:D5}{2:D5}", PanasonicPlcAddress, DStartValue, DStartValue + Length - 1);
                Crc(WriteStr, ref CrcLo, ref CrcHi);
                WriteStr = WriteStr + CrcLo + CrcHi + "\r\n";
                comPort.DiscardInBuffer();
                WriteBuff = Encoding.ASCII.GetBytes(WriteStr);
                comPort.Write(WriteBuff, 0, WriteBuff.Length);
                NowTime = DateTime.Now;
                do
                {
                    Thread.Sleep(20);
                    if (comPort.BytesToRead >= 9 + Length * 4)//收到数据
                    {
                        ReturnByte = comPort.BytesToRead;
                        IsReturn = true;
                    }
                    ts = DateTime.Now - NowTime;
                    if (ts.TotalMilliseconds > timeOut)//时间超时
                    {
                        IsTimeOut = true;
                    }
                } while (!IsReturn && !IsTimeOut);
                if (!IsReturn && IsTimeOut)//超时
                {
                    if (ErrStr.IndexOf("接收数据已超时") < 0)
                    {
                        ErrStr = ErrStr + DateTime.Now.ToString() + ",PanasonicPlcRead," + "初始失败,接收数据已超时" + (char)13 + (char)10;
                    }
                    isOK = false;
                }
                else
                {
                    comPort.Read(ReadBuff, 0, ReturnByte);
                    if (ReadBuff[0] != WriteBuff[0] || ReadBuff[1] != WriteBuff[1] || ReadBuff[2] != WriteBuff[2] || ReadBuff[3] == 33)//数据检验失败
                    {
                        if (ErrStr.IndexOf("接收数据错误") < 0)
                        {
                            ErrStr = ErrStr + DateTime.Now.ToString() + ",PanasonicPlcRead," + ":初始失败,接收数据错误" + (char)13 + (char)10;
                        }
                        isOK = false;
                    }
                    else
                    {
                        for (int i = 0; i < Length; i++)
                        {
                            value[i] = Convert.ToInt32((Encoding.ASCII.GetString(ReadBuff, 8 + i * 4, 2) + Encoding.ASCII.GetString(ReadBuff, 6 + i * 4, 2)), 16);
                        }
                        isOK = true;
                    }
                }
            }
            catch (Exception exc)
            {
                if (ErrStr.IndexOf(exc.ToString()) < 0)
                {
                    ErrStr = ErrStr + DateTime.Now.ToString() + ",StandarBoardInit," + ":" + exc.ToString() + (char)13 + (char)10;
                }
                isOK = false;
            }
            return isOK;

        }
        public bool WriteR(int RValue, bool Value)
        {
            //if (!mPanasonicPlcIsInit)
            //{
            //    Init();
            //    return false;
            //}
            bool isOK = false;
            string WriteStr = "";//发送数据
            byte[] WriteBuff = new byte[17];
            byte[] ReadBuff = new byte[20];//接收数据
            int ReturnByte = 0;//返回数据
            bool IsReturn = false;//是否成功返回
            bool IsTimeOut = false;//是否超时
            DateTime NowTime = DateTime.Now;//当前时间
            string CrcHi = "", CrcLo = "";
            TimeSpan ts;//时间差
            try
            {
                if (!comPort.IsOpen)
                {
                    comPort.Open();
                }
                WriteStr = string.Format("<{0:D2}#WCSR{1:X4}{2}", PanasonicPlcAddress, RValue, Value ? 1 : 0); //string.Format("<{0:D2}#RCP1R{1:D4}", PanasonicPlcAddress, RValue);
                Crc(WriteStr, ref CrcLo, ref CrcHi);
                WriteStr = WriteStr + CrcLo + CrcHi + "\r\n";
                comPort.DiscardInBuffer();
                WriteBuff = Encoding.ASCII.GetBytes(WriteStr);
                comPort.Write(WriteBuff, 0, WriteBuff.Length);
                NowTime = DateTime.Now;
                do
                {
                    if (comPort.BytesToRead >= 9)//收到数据
                    {
                        ReturnByte = comPort.BytesToRead;
                        IsReturn = true;
                    }
                    ts = DateTime.Now - NowTime;
                    if (ts.TotalMilliseconds > timeOut)//时间超时
                    {
                        IsTimeOut = true;
                    }
                } while (!IsReturn && !IsTimeOut);
                if (!IsReturn && IsTimeOut)//超时
                {
                    if (ErrStr.IndexOf("接收数据已超时") < 0)
                    {
                        ErrStr = ErrStr + DateTime.Now.ToString() + ",PanasonicPlcRead," + "初始失败,接收数据已超时" + (char)13 + (char)10;
                    }
                    isOK = false;
                }
                else
                {
                    comPort.Read(ReadBuff, 0, ReturnByte);
                    if (ReadBuff[0] != WriteBuff[0] || ReadBuff[1] != WriteBuff[1] || ReadBuff[2] != WriteBuff[2] || ReadBuff[3] == 33)//数据检验失败
                    {
                        cMain.WriteErrorToLog("IsDataError");
                        if (ErrStr.IndexOf("接收数据错误") < 0)
                        {
                            ErrStr = ErrStr + DateTime.Now.ToString() + ",PanasonicPlcRead," + ":初始失败,接收数据错误" + (char)13 + (char)10;
                        }
                        isOK = false;
                    }
                    else
                    {
                        isOK = true;
                    }
                }
            }
            catch (Exception exc)
            {
                if (ErrStr.IndexOf(exc.ToString()) < 0)
                {
                    ErrStr = ErrStr + DateTime.Now.ToString() + ",StandarBoardInit," + ":" + exc.ToString() + (char)13 + (char)10;
                }
                isOK = false;
                cMain.WriteErrorToLog(string.Format("{0}",isOK));
            }
            return isOK;
        }
        public bool WriteD(int DStartValue, long value)
        {
            long[] l = new long[1];
            l[0] = value;
            return WriteD(DStartValue, 1, l);
        }
        private bool Crc(string value, ref string CrcLo, ref string CrcHi)
        {
            bool isOk = false;
            string tmpStr = "";
            byte tmpValue = 0;
            try
            {
                byte[] tmpByte = Encoding.ASCII.GetBytes(value);
                for (int i = 0; i < tmpByte.Length; i++)
                {
                    tmpValue = (byte)(tmpValue ^ tmpByte[i]);
                }
                tmpStr = string.Format("{0:X2}", tmpValue);
                CrcLo = tmpStr.Substring(0, 1);
                CrcHi = tmpStr.Substring(1, 1);
                isOk = true;
            }
            catch
            {
                CrcLo = "0";
                CrcHi = "0";
                isOk = false;
            }
            return isOk;
        }

    }
}
