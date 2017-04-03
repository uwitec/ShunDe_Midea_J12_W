using System;
using System.Collections.Generic;
using System.Text;
using PcProgram;
using System.IO.Ports;
namespace System
{
    public class c7017
    { 
        public static string mName = "";
        SerialPort comPort;//端口
        /// <summary>
        /// c7017使用的串口
        /// </summary>
        public SerialPort ComPort
        {
            get { return comPort; }
            set { comPort = value; }
        }
        string errStr = "C7017";//出错信息
        /// <summary>
        /// 错误返回信息
        /// </summary>
        public string ErrStr
        {
            get { return errStr; }
            set { errStr = value; }
        }
        int timeOut = 300;//超时时间(ms)
        /// <summary>
        ///读写串口超时时间,单位(ms)
        /// </summary>
        public int TimeOut
        {
            get { return timeOut; }
            set { timeOut = value; }
        }
        byte c7017Address = 0;//LGPLC地址
        public bool c7017IsInit = false;//PLC初始化结果
        /// <summary>
        /// 构造7017
        /// </summary>
        /// <param name="mComPort">SerialPort,7017使用的串口</param>
        /// <param name="mAddress">byte,7017的地址</param>
        public c7017(SerialPort mComPort, byte mAddress,int TimeDelay)
        {
            comPort = mComPort;
            c7017Address = mAddress;
            timeOut = TimeDelay;
        }
        public bool init()
        {
            if (cMain.isDebug)
            {
                c7017IsInit = true;
                return true;
            }
            string WriteBuff ="";//发送数据
            byte[] ReadBuff = new byte[20];//接收数据
            int ReturnByte = 0;//返回数据
            bool IsReturn = false;//是否成功返回
            bool IsTimeOut = false;//是否超时
            DateTime NowTime = DateTime.Now;//当前时间
            TimeSpan ts;//时间差
            try
            {
                if (!comPort.IsOpen)
                {
                    comPort.Open();
                }
                WriteBuff = string.Format("#{0:D2}1" + Encoding.ASCII.GetString(new byte[] { 13 }), c7017Address);
                comPort.DiscardInBuffer();
                comPort.Write(WriteBuff);
                NowTime = DateTime.Now;
                do
                {
                    Threading.Thread.Sleep(20);
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
                        ErrStr = ErrStr + DateTime.Now.ToString() + ",StandarBoardInit," + "初始失败,接收数据已超时" + (char)13 + (char)10;
                    }
                    return false;
                }
                else
                {
                    comPort.Read(ReadBuff, 0, ReturnByte);
                    if ((ReadBuff[0] !=0x3E) || (ReadBuff[8] != 0x0D))//数据检验失败
                    {
                        if (ErrStr.IndexOf("接收数据错误") < 0)
                        {
                            ErrStr = ErrStr + DateTime.Now.ToString() + ",StandarBoardInit," + ":初始失败,接收数据错误" + (char)13 + (char)10;
                        }
                        return false;
                    }
                    else
                    {
                        c7017IsInit = true;
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
        public bool ReadData(ref double[] ReadData)
        {
            bool isOk = false;
            if (cMain.isDebug)
            {
                for (int i = 0; i < ReadData.Length; i++)
                {
                    ReadData[i] = (byte)(20 + (byte)(Num.Rand() * 10));
                }
                return true;
            }
            if (!c7017IsInit)//没有初始化
            {
                init();
                return false;
            }
            string tmpReadData;//收到数据
            string WriteBuff = "";//发送数据
            byte[] ReadBuff = new byte[60];//接收数据
            int ReturnByte = 0;//返回数据
            bool IsReturn = false;//是否成功返回
            bool IsTimeOut = false;//是否超时
            DateTime NowTime = DateTime.Now;//当前时间
            TimeSpan ts;//时间差
            try
            {
                if (!comPort.IsOpen)
                {
                    comPort.Open();
                }
                WriteBuff = string.Format("#{0:D2}" + Encoding.ASCII.GetString(new byte[] { 13 }), c7017Address);
                comPort.DiscardInBuffer();
                comPort.Write(WriteBuff);
                NowTime = DateTime.Now;
                do
                {
                    Threading.Thread.Sleep(20);
                    if (comPort.BytesToRead >= 58)//收到数据
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
                        ErrStr = ErrStr + DateTime.Now.ToString() + ",StandarBoardInit," + "初始失败,接收数据已超时" + (char)13 + (char)10;
                    }
                    return false;
                }
                else
                {
                    comPort.Read(ReadBuff, 0, ReturnByte);
                    if ((ReadBuff[0] != 0x3E) || (ReadBuff[57] != 0x0D))//数据检验失败
                    {
                        if (ErrStr.IndexOf("接收数据错误") < 0)
                        {
                            ErrStr = ErrStr + DateTime.Now.ToString() + ",StandarBoardInit," + ":初始失败,接收数据错误" + (char)13 + (char)10;
                        }
                        return false;
                    }
                    else
                    {
                        tmpReadData = Encoding.ASCII.GetString(ReadBuff, 0, ReturnByte);
                        string[] tmp = tmpReadData.Split(new char[] { '+', '-' });
                        int MinIndex = Math.Min(tmp.Length, ReadData.Length);
                        for (int i = 0; i < MinIndex; i++)
                        {
                            ReadData[i] = Num.DoubleParse(tmp[i + 1]);
                        }
                    }
                    isOk = true;
                }
            }
            catch (Exception exc)
            {
                if (ErrStr.IndexOf(exc.ToString()) < 0)
                {
                    ErrStr = ErrStr + DateTime.Now.ToString() + ",StandarBoardInit," + ":" + exc.ToString() + (char)13 + (char)10;
                }
                isOk = false;
            }
            return isOk;
        }
        public bool ReadData(int StartPoint, ref double ReadData)
        {
            bool isOk = false;
            if (cMain.isDebug)
            {
                ReadData = (byte)(20 + (byte)(Num.Rand() * 10));
                return true;
            } 
            if (!c7017IsInit)//没有初始化
            {
                init();
                return false;
            }
            string tmpReadData;//收到数据
            string WriteBuff = "";//发送数据
            byte[] ReadBuff = new byte[20];//接收数据
            int ReturnByte = 0;//返回数据
            bool IsReturn = false;//是否成功返回
            bool IsTimeOut = false;//是否超时
            DateTime NowTime = DateTime.Now;//当前时间
            TimeSpan ts;//时间差
            try
            {
                if (!comPort.IsOpen)
                {
                    comPort.Open();
                }
                WriteBuff = string.Format("#{0:D2}{1}" + Encoding.ASCII.GetString(new byte[] { 13 }), c7017Address,StartPoint);
                comPort.DiscardInBuffer();
                comPort.Write(WriteBuff);
                NowTime = DateTime.Now;
                do
                {
                    Threading.Thread.Sleep(20);
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
                        ErrStr = ErrStr + DateTime.Now.ToString() + ",StandarBoardInit," + "初始失败,接收数据已超时" + (char)13 + (char)10;
                    }
                    return false;
                }
                else
                {
                    comPort.Read(ReadBuff, 0, ReturnByte);
                    if ((ReadBuff[0] != 0x3E) || (ReadBuff[8] != 0x0D))//数据检验失败
                    {
                        if (ErrStr.IndexOf("接收数据错误") < 0)
                        {
                            ErrStr = ErrStr + DateTime.Now.ToString() + ",StandarBoardInit," + ":初始失败,接收数据错误" + (char)13 + (char)10;
                        }
                        return false;
                    }
                    else
                    {
                        tmpReadData = Encoding.ASCII.GetString(ReadBuff, 0, ReturnByte);
                        string[] tmp = tmpReadData.Split(new char[] { '+', '-' });
                        ReadData = Num.DoubleParse(tmp[1]);
                    }
                    isOk = true;
                }
            }
            catch (Exception exc)
            {
                if (ErrStr.IndexOf(exc.ToString()) < 0)
                {
                    ErrStr = ErrStr + DateTime.Now.ToString() + ",StandarBoardInit," + ":" + exc.ToString() + (char)13 + (char)10;
                }
                isOk= false;
            }
            return isOk;
        }
    }
}
