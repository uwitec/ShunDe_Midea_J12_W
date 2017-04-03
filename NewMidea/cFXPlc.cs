using System;
using System.Collections.Generic;
using System.Text;
using NewMideaProgram;
using System.IO.Ports;
namespace System
{
    /// <summary>
    /// 三菱FX系统PLC控制
    /// </summary>
    public class cFxplc
    {
        public static string mName = "FXPLC";
        SerialPort comPort;//端口
        /// <summary>
        /// FXPLC使用的串口
        /// </summary>
        public SerialPort ComPort
        {
            get { return comPort; }
            set { comPort = value; }
        }
        string errStr = "FXPLC";//出错信息
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
        public bool mFXPLCIsInit = false;//PLC初始化结果
        /// <summary>
        /// FXPLC的构造函数
        /// </summary>
        /// <param name="mPort">FXPLC使用的PLC串口</param>
        /// <param name="mTimeOut">FXPLC的超时时间</param>
        public cFxplc(SerialPort mPort, int mTimeOut)
        {
            comPort = mPort;
            timeOut = mTimeOut;
        }
        /// <summary>
        /// 初始化FXPLC
        /// </summary>
        /// <returns>bool,返回初始化结果</returns>
        public bool FxPlcInit()
        {
            if (cMain.isDebug)
            {
                mFXPLCIsInit = true;
                return true;
            }
            byte[] WriteBuff = new byte[1];//发送数据
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
                WriteBuff[0] = 5;
                comPort.DiscardInBuffer();
                comPort.Write(WriteBuff, 0, 1);
                NowTime = DateTime.Now;
                do
                {
                    //System.Windows.Forms.Application.DoEvents();
                    Threading.Thread.Sleep(50);
                    if (comPort.BytesToRead >= 1)//收到数据
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
                        ErrStr = ErrStr + DateTime.Now.ToString() + ",FxPlcInit," + "初始失败,接收数据已超时" + (char)13 + (char)10;
                    }
                    return false;
                }
                else
                {
                    comPort.Read(ReadBuff, 0, ReturnByte);
                    if (ReadBuff[0] != 6)//数据检验失败
                    {
                        if (ErrStr.IndexOf("接收数据错误") < 0)
                        {
                            ErrStr = ErrStr + DateTime.Now.ToString() + ",FxPlcInit," + ":初始失败,接收数据错误" + (char)13 + (char)10;
                        }
                        return false;
                    }
                    else
                    {
                        mFXPLCIsInit = true;
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
        /// <summary>
        /// 读取M点状态，按字节读取，即读取1位状态
        /// </summary>
        /// <param name="StartPoint">int,M点</param>
        /// <param name="ReadValue">bool,返回单个M点状态</param>
        /// <returns>bool,读取是否成功</returns>
        public bool FxPlc_ReadM(int StartPoint, out bool ReadValue)
        {
            bool[] tmpValue;
            bool isOk = FxPlc_ReadM(StartPoint, out tmpValue);
            ReadValue = tmpValue[StartPoint % 8];
            return isOk;
        }
        /// <summary>
        /// 读取M点状态，按字读取，即读取8位状态
        /// </summary>
        /// <param name="StartPoint">int,M点</param>
        /// <param name="ReadValue">bool[],M点所在字状态</param>
        /// <returns>bool,读取是否成功</returns>
        public bool FxPlc_ReadM(int StartPoint, out bool[] ReadValue)
        {
            uint readBuff = 0;
            ReadValue=new bool[8];
            bool isReadOk = FxPlc_ReadM(StartPoint, 1, out readBuff);
            if (isReadOk)
            {
                for (int i = 0; i < 8; i++)
                {
                    int xx =(int) Math.Pow(2, i);
                    if ((readBuff & xx) == xx)
                    {
                        ReadValue[i] = true;
                    }
                    else
                    {
                        ReadValue[i] = false;
                    }
                }
            }
            return isReadOk;
        }
        /// <summary>
        /// 读取M点状态，按字读取，即读取8位状态
        /// </summary>
        /// <param name="StartPoint">int,M点</param>
        /// <param name="ReadValue">int,M点所在字状态</param>
        /// <returns>bool,操作是否成功</returns>
        public bool FxPlc_ReadM(int StartPoint, out uint ReadValue)
        {
            return FxPlc_ReadM(StartPoint, 1, out ReadValue);
        }
        /// <summary>
        /// 读取M点状态，按字读取
        /// </summary>
        /// <param name="StartPoint">int,M点</param>
        /// <param name="len">int,读取M点字数，如值为1，则读8个M点，值为2，则读16个M点</param>
        /// <param name="ReadValue">int,读取M点所在字长度内的状态</param>
        /// <returns>bool,操作是否成功</returns>
        public bool FxPlc_ReadM(int StartPoint,int len, out UInt32 ReadValue)
        {
            byte[] WriteBuff = new byte[11];//发送数据
            byte[] ReadBuff = new byte[20];//接收数据
            int ReturnByte = 0;//返回数据
            bool IsReturn = false;//是否成功返回
            bool IsTimeOut = false;//是否超时
            DateTime NowTime;//当前时间
            TimeSpan ts;//时间差
            ReadValue = 0;
            len = Math.Min(len, 4);
            byte CrcHi = 0, CrcLo = 0;//CRC校验

            if (cMain.isDebug)
            {
                ReadValue = 0;
                return true;
            }
            if (!mFXPLCIsInit)//没有初始化
            {
                FxPlcInit();
                return false;
            }
            try
            {
                StartPoint = StartPoint / 8;
                WriteBuff[0] = 0x02;
                WriteBuff[1] = 0x30;
                WriteBuff[2] = 0x30;
                WriteBuff[3] = 0x31;
                WriteBuff[4] = Encoding.ASCII.GetBytes(string.Format("{0:X1}", (int)(StartPoint / 16) & 0x0F))[0];
                WriteBuff[5] = Encoding.ASCII.GetBytes(string.Format("{0:X1}", (int)(StartPoint / 1) & 0x0F))[0];
                WriteBuff[6] = Encoding.ASCII.GetBytes(string.Format("{0:X2}", len))[0];
                WriteBuff[7] = Encoding.ASCII.GetBytes(string.Format("{0:X2}", len))[1];
                WriteBuff[8] = 0x03;
                FxPlcCrc(WriteBuff, out CrcLo, out CrcHi);
                WriteBuff[9] = CrcLo;
                WriteBuff[10] = CrcHi;
                if (!comPort.IsOpen)
                {
                    comPort.Open();
                }
                comPort.DiscardInBuffer();//刷新串口
                comPort.Write(WriteBuff, 0, 11);
                NowTime = DateTime.Now;
                do
                {
                    Threading.Thread.Sleep(50);
                    if (comPort.BytesToRead >= (4 + len * 2))//收到数据
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
                        ErrStr = ErrStr + DateTime.Now.ToString() + ",FxPlc_ReadM," + ":读取失败,接收数据已超时" + (char)13 + (char)10;
                    }
                    return false;
                }
                comPort.Read(ReadBuff, 0, ReturnByte);
                ReadValue = Convert.ToUInt32(Encoding.ASCII.GetString(ReadBuff, 1, 2 * len), 16);
            }
            catch (Exception exc)
            {
                if (ErrStr.IndexOf(exc.ToString()) < 0)
                {
                    ErrStr = ErrStr + DateTime.Now.ToString() + ",FxPlc_ReadM," + ":" + exc.ToString() + (char)13 + (char)10;
                }
                return false;
            }
            return true;

        }
        /// <summary>
        /// 写入M点状态
        /// </summary>
        /// <param name="StartPoint">int,M点</param>
        /// <param name="Value">bool,写入M点值</param>
        /// <returns>bool,操作是否成功</returns>
        public bool FxPlc_WriteM(int StartPoint, bool Value)
        {
            byte[] WriteBuff = new byte[15];//发送数据
            byte[] ReadBuff = new byte[20];//接收数据
            int ReturnByte = 0;//返回数据
            bool IsReturn = false;//是否成功返回
            bool IsTimeOut = false;//是否超时
            DateTime NowTime;//当前时间
            TimeSpan ts;//时间差
            byte CrcHi = 0, CrcLo = 0;//CRC校验
            int tempAddress = 0x800 + StartPoint;
            byte[] Address = Encoding.ASCII.GetBytes(string.Format("{0:X4}", tempAddress));
            if (cMain.isDebug)
            {
                Value = false;
                return true;
            }
            if (!mFXPLCIsInit)//没有初始化
            {
                FxPlcInit();
                return false;
            }
            try
            {
                WriteBuff[0] = 0x02;
                WriteBuff[1] = (byte)(Value ? 0x37 : 0x38);
                WriteBuff[2] = Address[2];
                WriteBuff[3] = Address[3];
                WriteBuff[4] = Address[0];
                WriteBuff[5] = Address[1];
                WriteBuff[6] = 0x03;
                FxPlcCrc(WriteBuff, out CrcLo, out CrcHi);
                WriteBuff[7] = CrcLo;
                WriteBuff[8] = CrcHi;
                if (!comPort.IsOpen)
                {
                    comPort.Open();
                }
                comPort.DiscardInBuffer();//刷新串口
                comPort.Write(WriteBuff, 0, 9);
                NowTime = DateTime.Now;
                do
                {
                    Threading.Thread.Sleep(50);
                    if (comPort.BytesToRead >= 1)//收到数据
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
                        ErrStr = ErrStr + DateTime.Now.ToString() + ",FxPlc_WriteD," + ":写入失败,接收数据已超时" + (char)13 + (char)10;
                    }
                    return false;
                }
                comPort.Read(ReadBuff, 0, ReturnByte);
                if (ReadBuff[0] != 06)
                {
                    if (ErrStr.IndexOf("写入数据失败") < 0)
                    {
                        ErrStr = ErrStr + DateTime.Now.ToString() + ",FxPlc_WriteD," + ":写入失败,写入数据失败" + (char)13 + (char)10;
                    }
                    return false;
                }
            }
            catch (Exception exc)
            {
                if (ErrStr.IndexOf(exc.ToString()) < 0)
                {
                    ErrStr = ErrStr + DateTime.Now.ToString() + ",FxPlc_WriteD," + ":" + exc.ToString() + (char)13 + (char)10;
                }
                return false;
            }
            return true;
        }
        /// <summary>
        /// 写入D点值
        /// </summary>
        /// <param name="StartPoint">int,D点</param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public bool FxPlc_WriteD(int StartPoint, int Value)
        {
            byte[] WriteBuff = new byte[15];//发送数据
            byte[] ReadBuff = new byte[20];//接收数据
            int ReturnByte = 0;//返回数据
            bool IsReturn = false;//是否成功返回
            bool IsTimeOut = false;//是否超时
            DateTime NowTime;//当前时间
            TimeSpan ts;//时间差
            byte CrcHi = 0, CrcLo = 0;//CRC校验
            string tempValueStr = "";
            byte[] tempValueByte;
            if (cMain.isDebug)
            {
                Value = 0;
                return true;
            }
            if (!mFXPLCIsInit)//没有初始化
            {
                FxPlcInit();
                return false;
            }
            try
            {
                if (Value < 0)
                {
                    Value = (0xFFFF & ((-(Value) - 1) ^ 0xFFFF));
                }
                StartPoint = StartPoint * 2 + 0x1000;
                WriteBuff[0] = 0x02;
                WriteBuff[1] = 0x31;
                tempValueStr = string.Format("{0:X4}", StartPoint);
                tempValueByte = Encoding.ASCII.GetBytes(tempValueStr);
                WriteBuff[2] = tempValueByte[0];
                WriteBuff[3] = tempValueByte[1];
                WriteBuff[4] = tempValueByte[2];
                WriteBuff[5] = tempValueByte[3];
                WriteBuff[6] = 0x30;
                WriteBuff[7] = 0x32;
                tempValueStr = string.Format("{0:X4}", Value);
                tempValueByte = Encoding.ASCII.GetBytes(tempValueStr);
                WriteBuff[8] = tempValueByte[2];
                WriteBuff[9] = tempValueByte[3];
                WriteBuff[10] = tempValueByte[0];
                WriteBuff[11] = tempValueByte[1];
                WriteBuff[12] = 0x03;
                FxPlcCrc(WriteBuff, out CrcLo, out CrcHi);
                WriteBuff[13] = CrcLo;
                WriteBuff[14] = CrcHi;
                if (!comPort.IsOpen)
                {
                    comPort.Open();
                }
                comPort.DiscardInBuffer();//刷新串口
                comPort.Write(WriteBuff, 0, 15);
                NowTime = DateTime.Now;
                do
                {
                    Threading.Thread.Sleep(50);
                    if (comPort.BytesToRead >= 1)//收到数据
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
                        ErrStr = ErrStr + DateTime.Now.ToString() + ",FxPlc_WriteD," + ":写入失败,接收数据已超时" + (char)13 + (char)10;
                    }
                    return false;
                }
                comPort.Read(ReadBuff, 0, ReturnByte);
                if (ReadBuff[0] != 06)
                {
                    if (ErrStr.IndexOf("写入数据失败") < 0)
                    {
                        ErrStr = ErrStr + DateTime.Now.ToString() + ",FxPlc_WriteD," + ":写入失败,写入数据失败" + (char)13 + (char)10;
                    }
                    return false;
                }
            }
            catch (Exception exc)
            {
                if (ErrStr.IndexOf(exc.ToString()) < 0)
                {
                    ErrStr = ErrStr + DateTime.Now.ToString() + ",FxPlc_WriteD," + ":" + exc.ToString() + (char)13 + (char)10;
                }
                return false;
            }
            return true;
        }
        public bool FxPlc_ReadD(int StartPoint,int length ,out long[] ReadValue)
        {
            byte[] WriteBuff = new byte[11];//发送数据
            byte[] ReadBuff = new byte[100];//接收数据
            int ReturnByte = 0;//返回数据
            bool IsReturn = false;//是否成功返回
            bool IsTimeOut = false;//是否超时
            DateTime NowTime;//当前时间
            TimeSpan ts;//时间差
            ReadValue = new long[length] ;
            byte CrcHi = 0, CrcLo = 0;//CRC校验
            if (cMain.isDebug)
            {
                return true;
            }
            if (!mFXPLCIsInit)//没有初始化
            {
                FxPlcInit();
                return false;
            }
            try
            {
                StartPoint = StartPoint * 2 + 0x1000;
                string startPoint = string.Format("{0:X4}", StartPoint);
                byte[] point = Encoding.ASCII.GetBytes(startPoint);
                WriteBuff[0] = 0x02;
                WriteBuff[1] = 0x30;
                WriteBuff[2] = point[0];
                WriteBuff[3] = point[1];
                WriteBuff[4] = point[2];
                WriteBuff[5] = point[3];
                WriteBuff[6] = Encoding.ASCII.GetBytes(string.Format("{0:X2}", length * 2))[0];
                WriteBuff[7] = Encoding.ASCII.GetBytes(string.Format("{0:X2}", length * 2))[1];
                WriteBuff[8] = 0x03;
                FxPlcCrc(WriteBuff, out CrcLo, out CrcHi);
                WriteBuff[9] = CrcLo;
                WriteBuff[10] = CrcHi;
                if (!comPort.IsOpen)
                {
                    comPort.Open();
                }
                comPort.DiscardInBuffer();//刷新串口
                comPort.Write(WriteBuff, 0, 11);
                NowTime = DateTime.Now;
                do
                {
                    Threading.Thread.Sleep(50);
                    if (comPort.BytesToRead >= 4 + 4 * length)//收到数据
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
                        ErrStr = ErrStr + DateTime.Now.ToString() + ",FxPlc_ReadD," + ":读取失败,接收数据已超时" + (char)13 + (char)10;
                    }
                    return false;
                }
                comPort.Read(ReadBuff, 0, ReturnByte);
                for (int i = 0; i < length; i++)
                {
                    ReadValue[i] = Convert.ToInt32(Encoding.ASCII.GetString(ReadBuff, 1 + i * 4, 2), 16) +
                          256 * Convert.ToInt32(Encoding.ASCII.GetString(ReadBuff, 3 + i * 4, 2), 16);
                }
            }
            catch (Exception exc)
            {
                if (ErrStr.IndexOf(exc.ToString()) < 0)
                {
                    ErrStr = ErrStr + DateTime.Now.ToString() + ",FxPlc_ReadD," + ":" + exc.ToString() + (char)13 + (char)10;
                }
                return false;
            }
            return true;
        }
        public bool FxPlc_ReadX(int StartPoint, out bool[] ReadValue)
        {
            int readValue = 0;
            ReadValue = new bool[8];
            bool isOk = FxPlc_ReadX(StartPoint, out readValue);
            if (isOk)
            {
                for (int i = 0; i < 8; i++)
                {
                    int xx = (int)Math.Pow(2, i);
                    if ((readValue & xx) == xx)
                    {
                        ReadValue[i] = true;
                    }
                    else
                    {
                        ReadValue[i] = false;
                    }
                } 
            }
            return isOk;
        }
        public bool FxPlc_ReadX(int StartPoint, out bool ReadValue)
        {
            int startPoint = StartPoint / 8;
            int indexPoint = StartPoint % 8;
            int readValue=0;
            ReadValue = false;
            bool isOk = FxPlc_ReadX(startPoint, out readValue);
            if (isOk)
            {
                int xx = (int)Math.Pow(2, indexPoint);
                if ((readValue & xx) == xx)
                {
                    ReadValue = true;
                }
            }
            return isOk;
        }
        public bool FxPlc_ReadX(int StartPoint, out int ReadValue)
        {
            return FxPlc_ReadX(StartPoint, 1, out ReadValue);
        }
        public bool FxPlc_ReadX(int StartPoint, int len, out int ReadValue)
        {
            byte[] WriteBuff = new byte[11];//发送数据
            byte[] ReadBuff = new byte[20];//接收数据
            int ReturnByte = 0;//返回数据
            bool IsReturn = false;//是否成功返回
            bool IsTimeOut = false;//是否超时
            DateTime NowTime;//当前时间
            TimeSpan ts;//时间差
            ReadValue = 0;
            len = Math.Min(len, 4);
            byte CrcHi = 0, CrcLo = 0;//CRC校验

            if (cMain.isDebug)
            {
                ReadValue = 0;
                return true;
            }
            if (!mFXPLCIsInit)//没有初始化
            {
                FxPlcInit();
                return false;
            }
            try
            {
                StartPoint = 0x80 + StartPoint / 8;
                WriteBuff[0] = 0x02;
                WriteBuff[1] = 0x30;
                WriteBuff[2] = Encoding.ASCII.GetBytes(string.Format("{0:X1}", (int)(StartPoint / 4096) & 0x0F))[0];
                WriteBuff[3] = Encoding.ASCII.GetBytes(string.Format("{0:X1}", (int)(StartPoint / 256) & 0x0F))[0];
                WriteBuff[4] = Encoding.ASCII.GetBytes(string.Format("{0:X1}", (int)(StartPoint / 16) & 0x0F))[0];
                WriteBuff[5] = Encoding.ASCII.GetBytes(string.Format("{0:X1}", (int)(StartPoint / 1) & 0x0F))[0];
                WriteBuff[6] = Encoding.ASCII.GetBytes(string.Format("{0:X2}", len))[0];
                WriteBuff[7] = Encoding.ASCII.GetBytes(string.Format("{0:X2}", len))[1];
                WriteBuff[8] = 0x03;
                FxPlcCrc(WriteBuff, out CrcLo, out CrcHi);
                WriteBuff[9] = CrcLo;
                WriteBuff[10] = CrcHi;
                if (!comPort.IsOpen)
                {
                    comPort.Open();
                }
                comPort.DiscardInBuffer();//刷新串口
                comPort.Write(WriteBuff, 0, 11);
                NowTime = DateTime.Now;
                do
                {
                    Threading.Thread.Sleep(50);
                    if (comPort.BytesToRead >= (4 + len * 2))//收到数据
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
                        ErrStr = ErrStr + DateTime.Now.ToString() + ",FxPlc_ReadM," + ":读取失败,接收数据已超时" + (char)13 + (char)10;
                    }
                    return false;
                }
                comPort.Read(ReadBuff, 0, ReturnByte);
                ReadValue = Convert.ToInt32(Encoding.ASCII.GetString(ReadBuff, 1, 2 * len), 16);
            }
            catch (Exception exc)
            {
                if (ErrStr.IndexOf(exc.ToString()) < 0)
                {
                    ErrStr = ErrStr + DateTime.Now.ToString() + ",FxPlc_ReadM," + ":" + exc.ToString() + (char)13 + (char)10;
                }
                return false;
            }
            return true;

        }

        private bool FxPlcCrc(byte[] mByte, out byte Crc_L, out byte Crc_H)
        {
            bool isOk = false;
            Crc_L = 0; Crc_H = 0;
            try
            {
                int i;
                int count = 0;
                for (i = 1; i < mByte.Length; i++)
                {
                    count = (byte)((count + mByte[i]) & 0xFF);
                }
                Crc_L = (byte)(Convert.ToChar(string.Format("{0:X1}", ((count / 16) & 0xF))));
                Crc_H = (byte)(Convert.ToChar(string.Format("{0:X1}", (count & 0xF))));
                isOk = true;
            }
            catch
            {
            }
            return isOk;
        }
    }
}
