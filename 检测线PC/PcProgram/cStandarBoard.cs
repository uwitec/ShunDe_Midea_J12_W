using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using PcProgram;
namespace System
{
    public class cStandarBoard
    {
        //string mName = "标准MODBUS模块";
        SerialPort comPort;//端口
        /// <summary>
        /// 标准MODEBUS使用的串口
        /// </summary>
        public SerialPort ComPort
        {
            get { return comPort; }
            set { comPort = value; }
        }
        string errStr = "";//出错信息
        /// <summary>
        /// 错误返回信息
        /// </summary>
        public string ErrStr
        {
            get { return errStr; }
            set { errStr = value; }
        }
        int timeOut = 400;//超时时间(ms)
        /// <summary>
        ///读写串口超时时间,单位(ms)
        /// </summary>
        public int TimeOut
        {
            get { return timeOut; }
            set { timeOut = value; }
        }
        byte _StandarModebusAddress = 0;//LGPLC地址

        public byte StandarModebusAddress
        {
            get { return _StandarModebusAddress; }
            set { _StandarModebusAddress = value; }
        }

        bool mStandarBoardInit = false;//PLC初始化结果
        public cStandarBoard(SerialPort mComPort,byte mAddress,int mTimeOut)
        {
            timeOut = mTimeOut;
            comPort = mComPort;
            _StandarModebusAddress = mAddress;
        }
        public enum CodeList
        {
            Set,
            Get
        }
        /// <summary>
        /// 标准模块初始化
        /// </summary>
        /// <returns>返回初始化是否成功</returns>
        public bool StandarBoardInit()
        {
            if (cMain.isDebug)
            {
                mStandarBoardInit = true;
                return true;
            }
            byte[] WriteBuff = new byte[10];//发送数据
            byte[] ReadBuff = new byte[20];//接收数据
            int ReturnByte = 0;//返回数据
            bool IsReturn = false;//是否成功返回
            bool IsTimeOut = false;//是否超时
            DateTime NowTime = DateTime.Now;//当前时间
            TimeSpan ts;//时间差
            byte CrcHi = 0, CrcLo = 0;//CRC校验
            try
            {
                if (!comPort.IsOpen)
                {
                    comPort.Open();
                }
                WriteBuff[0] = (byte)(_StandarModebusAddress & 0xFF);
                WriteBuff[1] = 0x03;
                WriteBuff[2] = 0x00;
                WriteBuff[3] = 0x00;
                WriteBuff[4] = 0x00;
                WriteBuff[5] = 0x01;
                cMain.CRC_16(WriteBuff, 6, ref CrcLo, ref CrcHi);
                WriteBuff[6] = CrcLo;
                WriteBuff[7] = CrcHi;
                comPort.DiscardInBuffer();
                comPort.Write(WriteBuff, 0, 8);
                NowTime = DateTime.Now;
                do
                {
                    //System.Windows.Forms.Application.DoEvents();
                    Threading.Thread.Sleep(20);
                    if (comPort.BytesToRead >= WriteBuff[5]*2+5)//收到数据
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
                    comPort.Read(ReadBuff,0,  ReturnByte);
                    if ((ReadBuff[0] != WriteBuff[0]) || (ReadBuff[1] != WriteBuff[1]))//数据检验失败
                    {
                        if (ErrStr.IndexOf("接收数据错误") < 0)
                        {
                            ErrStr = ErrStr + DateTime.Now.ToString() + ",StandarBoardInit," + ":初始失败,接收数据错误" + (char)13 + (char)10;
                        }
                        return false;
                    }
                    else
                    {
                        mStandarBoardInit = true;
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
        ///// <summary>
        ///// 标准模块寄存器写入单个数值
        ///// </summary>
        ///// <param name="StartAdd">要写入寄存器地址</param>
        ///// <param name="mWriteBuff">要写入的值</param>
        ///// <returns>写入是否成功</returns>
        //public bool StandarBoardWritePoint(int StartAdd, int mWriteBuff)
        //{
        //    long[] ReturnBuff = new long[8];
        //    byte[] WriteBuff = new byte[8];//发送数据
        //    byte[] ReadBuff = new byte[21];//接收数据
        //    int ReturnByte = 0;//返回数据
        //    bool IsReturn = false;//是否成功返回
        //    bool IsTimeOut = false;//是否超时
        //    DateTime NowTime;//当前时间
        //    TimeSpan ts;//时间差
        //    byte CrcHi = 0, CrcLo = 0;//CRC校验
        //    if (cMain.isDebug)
        //    {
        //        return true;
        //    }
        //    if (!mStandarBoardInit)//没有初始化
        //    {
        //        StandarBoardInit();
        //        return false;
        //    }
        //    try
        //    {
        //        WriteBuff[0] = _StandarModebusAddress;
        //        WriteBuff[1] = 0x06;
        //        WriteBuff[2] = (byte)((StartAdd & 0xFF00) >> 8);
        //        WriteBuff[3] = (byte)(StartAdd & 0xFF);
        //        WriteBuff[4] = (byte)((mWriteBuff & 0xFF00) >>8);
        //        WriteBuff[5] = (byte)(mWriteBuff & 0xFF);
        //        cMain.CRC_16(WriteBuff, 6, ref CrcLo, ref CrcHi);
        //        WriteBuff[6] = CrcLo;
        //        WriteBuff[7] = CrcHi;
        //        if (!comPort.IsOpen)
        //        {
        //            comPort.Open();
        //        }
        //        comPort.DiscardInBuffer();//刷新串口
        //        comPort.Write(WriteBuff, 0, 8);
        //        NowTime = DateTime.Now;
        //        do
        //        {
        //            if (comPort.BytesToRead >= 8)//收到数据
        //            {
        //                ReturnByte = comPort.BytesToRead;
        //                IsReturn = true;
        //            }
        //            ts = DateTime.Now - NowTime;
        //            if (ts.TotalMilliseconds > timeOut)//时间超时
        //            {
        //                IsTimeOut = true;
        //            }
        //        } while (!IsReturn && !IsTimeOut);
        //        if (!IsReturn && IsTimeOut)//超时
        //        {
        //            if (ErrStr.IndexOf("接收数据已超时") < 0)
        //            {
        //                ErrStr = ErrStr + DateTime.Now.ToString() + mName + ":读取失败,接收数据已超时" + (char)13 + (char)10;
        //            }
        //            return false;
        //        }
        //        comPort.Read(ReadBuff, 0, ReturnByte);
        //        if ((ReadBuff[0] != WriteBuff[0]) || (ReadBuff[1] != WriteBuff[1]))//数据检验失败
        //        {
        //            if (ErrStr.IndexOf("接收数据错误") < 0)
        //            {
        //                ErrStr = ErrStr + DateTime.Now.ToString() + mName + ":读取失败,接收数据错误" + (char)13 + (char)10;
        //            }
        //            return false;
        //        }
        //    }
        //    catch (Exception exc)
        //    {
        //        if (ErrStr.IndexOf(exc.ToString()) < 0)
        //        {
        //            ErrStr = ErrStr + DateTime.Now.ToString() + mName + ":" + exc.ToString() + (char)13 + (char)10;
        //        }
        //        return false;
        //    }
        //    return true;
        //}
        /// <summary>
        /// 标准模块寄存器写入单个数值,专用于LGPLC写M点,功能码0x05,0x06
        /// </summary>
        /// <param name="StartAdd">要写入寄存器地址</param>
        /// <param name="mWriteBuff">要写入的值</param>
        /// <param name="code">命令码,5或者6,写线圈或写单个寄存器</param>
        /// <returns>写入是否成功</returns>
        public bool StandarBoardWritePoint(int StartAdd, int mWriteBuff,byte code)
        {
            long[] ReturnBuff = new long[8];
            byte[] WriteBuff = new byte[8];//发送数据
            byte[] ReadBuff = new byte[21];//接收数据
            int ReturnByte = 0;//返回数据
            bool IsReturn = false;//是否成功返回
            bool IsTimeOut = false;//是否超时
            DateTime NowTime;//当前时间
            TimeSpan ts;//时间差
            byte CrcHi = 0, CrcLo = 0;//CRC校验
            if (cMain.isDebug)
            {
                return true;
            }
            if (!mStandarBoardInit)//没有初始化
            {
                StandarBoardInit();
                return false;
            }
            try
            {
                WriteBuff[0] = _StandarModebusAddress;
                WriteBuff[1] = code;
                WriteBuff[2] = (byte)((StartAdd & 0xFF00) >> 8);
                WriteBuff[3] = (byte)(StartAdd & 0xFF);
                WriteBuff[4] = (byte)((mWriteBuff & 0xFF00) >> 8);
                WriteBuff[5] = (byte)(mWriteBuff & 0xFF);
                cMain.CRC_16(WriteBuff, 6, ref CrcLo, ref CrcHi);
                WriteBuff[6] = CrcLo;
                WriteBuff[7] = CrcHi;
                if (!comPort.IsOpen)
                {
                    comPort.Open();
                }
                comPort.DiscardInBuffer();//刷新串口
                comPort.Write(WriteBuff, 0, 8);
                NowTime = DateTime.Now;
                do
                {
                    //System.Windows.Forms.Application.DoEvents();
                    Threading.Thread.Sleep(20);
                    if (comPort.BytesToRead >= 8)//收到数据
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
                        ErrStr = ErrStr + DateTime.Now.ToString() + ",StandarBoardWritePoint," + ":读取失败,接收数据已超时" + (char)13 + (char)10;
                    }
                    return false;
                }
                comPort.Read(ReadBuff,0,  ReturnByte);
                if ((ReadBuff[0] != WriteBuff[0]) || (ReadBuff[1] != WriteBuff[1]))//数据检验失败
                {
                    if (ErrStr.IndexOf("接收数据错误") < 0)
                    {
                        ErrStr = ErrStr + DateTime.Now.ToString() + ",StandarBoardWritePoint," + ":读取失败,接收数据错误" + (char)13 + (char)10;
                    }
                    return false;
                }
            }
            catch (Exception exc)
            {
                if (ErrStr.IndexOf(exc.ToString()) < 0)
                {
                    ErrStr = ErrStr + DateTime.Now.ToString() + ",StandarBoardWritePoint," + ":" + exc.ToString() + (char)13 + (char)10;
                }
                return false;
            }
            return true;
        }
        /// <summary>
        /// 将多个数据写入到线圈,功能码为0x0F
        /// </summary>
        /// <param name="StartAdd">写入线圈的开始地址</param>
        /// <param name="mWriteBuff">写入的数据</param>
        /// <returns>返回写入是否成功</returns>
        public bool StandarBoardWritePoint(int StartAdd, bool[] mWriteBuff)
        {
            int PointCount = (int)Math.Ceiling((double)((double)(mWriteBuff.Length) / 8.00000));
            byte[] WriteBuff = new byte[PointCount + 9];
            byte[] ReadBuff = new byte[8];
            int ReturnByte = 0;//返回数据
            bool IsReturn = false;//是否成功返回
            bool IsTimeOut = false;//是否超时
            byte[] tWriteBuff=new byte[PointCount];
            DateTime NowTime;//当前时间
            TimeSpan ts;//时间差
            byte CrcHi = 0, CrcLo = 0;//CRC校验
            int i;
            if (cMain.isDebug)
            {
                return true;
            }
            if (!mStandarBoardInit)//没有初始化
            {
                StandarBoardInit();
                return false;
            }
            try
            {
                WriteBuff[0] = _StandarModebusAddress;
                WriteBuff[1] = 0x0F;
                WriteBuff[2] = (byte)((StartAdd & 0xFF00) >> 8);
                WriteBuff[3] = (byte)(StartAdd & 0xFF);
                WriteBuff[4] = (byte)((mWriteBuff.Length & 0xFF00) >> 8);
                WriteBuff[5] = (byte)(mWriteBuff.Length & 0xFF);
                WriteBuff[6] = (byte)(PointCount & 0xFF);
                for (i = 0; i < PointCount; i++)
                {
                    tWriteBuff[i] = 0;
                }
                for (int j = 0; j < PointCount; j++)
                {
                    for (i = 0; i < 8; i++)
                    {
                        if ((j * 8 + i) < mWriteBuff.Length)
                        {
                            if (mWriteBuff[j * 8 + i])
                            {
                                tWriteBuff[j] =(byte)(tWriteBuff[j] + 2 ^ i);
                            }
                        }
                    }
                }
                cMain.CRC_16(WriteBuff, PointCount + 7, ref CrcLo, ref CrcHi);
                WriteBuff[PointCount  + 7] = CrcLo;
                WriteBuff[PointCount  + 8] = CrcHi;
                if (!comPort.IsOpen)
                {
                    comPort.Open();
                }
                comPort.DiscardInBuffer();//刷新串口
                comPort.Write(WriteBuff, 0, PointCount + 9);
                NowTime = DateTime.Now;
                do
                {
                    //System.Windows.Forms.Application.DoEvents();
                    Threading.Thread.Sleep(20);
                    if (comPort.BytesToRead >= 8)//收到数据
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
                        ErrStr = ErrStr + DateTime.Now.ToString() + ",StandarBoardWritePoint," + ":读取失败,接收数据已超时" + (char)13 + (char)10;
                    }
                    return false;
                }
                comPort.Read(ReadBuff, 0, ReturnByte);
                if ((ReadBuff[0] != WriteBuff[0]) || (ReadBuff[1] != WriteBuff[1]))//数据检验失败
                {
                    if (ErrStr.IndexOf("接收数据错误") < 0)
                    {
                        ErrStr = ErrStr + DateTime.Now.ToString() + ",StandarBoardWritePoint," + ":读取失败,接收数据错误" + (char)13 + (char)10;
                    }
                    return false;
                }
            }
            catch (Exception exc)
            {
                if (ErrStr.IndexOf(exc.ToString()) < 0)
                {
                    ErrStr = ErrStr + DateTime.Now.ToString() + ",StandarBoardWritePoint," + ":" + exc.ToString() + (char)13 + (char)10;
                }
                return false;
            }
            return true;
 
        }
        /// <summary>
        /// 将多个数据写入到寄存器,功能码为Ox10
        /// </summary>
        /// <param name="StartAdd">写入寄存器开始地址</param>
        /// <param name="PointCount">写入数据长度</param>
        /// <param name="mWriteBuff">写入的数据,mWriteBuff.Length的个数不要小于PointCount</param>
        /// <returns>返回写入是否成功</returns>
        public bool StandarBoardWritePoint(int StartAdd, int PointCount, byte[] mWriteBuff)
        {
            long[] ReturnBuff = new long[8];
            byte[] WriteBuff = new byte[PointCount * 2 + 9];//发送数据
            byte[] ReadBuff = new byte[21];//接收数据
            int ReturnByte = 0;//返回数据
            bool IsReturn = false;//是否成功返回
            bool IsTimeOut = false;//是否超时
            DateTime NowTime;//当前时间
            TimeSpan ts;//时间差
            byte CrcHi = 0, CrcLo = 0;//CRC校验
            int i;
            if (cMain.isDebug)
            {
                return true;
            }
            if (!mStandarBoardInit)//没有初始化
            {
                StandarBoardInit();
                return false;
            }
            try
            {
                WriteBuff[0] = _StandarModebusAddress;
                WriteBuff[1] = 0x10;
                WriteBuff[2] = (byte)((StartAdd & 0xFF00) >>8);
                WriteBuff[3] = (byte)(StartAdd & 0xFF);
                WriteBuff[4] = (byte)((PointCount & 0xFF00) >> 8);
                WriteBuff[5] = (byte)(PointCount & 0xFF);
                WriteBuff[6] = (byte)((PointCount * 2) & 0xFF);
                for (i = 0; i < PointCount * 2; i++)
                {
                    WriteBuff[7 + i] = mWriteBuff[i];
                }
                cMain.CRC_16(WriteBuff, PointCount * 2 + 7, ref CrcLo, ref CrcHi);
                WriteBuff[PointCount * 2 + 7] = CrcLo;
                WriteBuff[PointCount * 2 + 8] = CrcHi;
                if (!comPort.IsOpen)
                {
                    comPort.Open();
                }
                comPort.DiscardInBuffer();//刷新串口
                comPort.Write(WriteBuff, 0, PointCount * 2 + 9);
                NowTime = DateTime.Now;
                do
                {
                    //System.Windows.Forms.Application.DoEvents();
                    Threading.Thread.Sleep(20);
                    if (comPort.BytesToRead >= 8)//收到数据
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
                        ErrStr = ErrStr + DateTime.Now.ToString() + ",StandarBoardWritePoint," + ":读取失败,接收数据已超时" + (char)13 + (char)10;
                    }
                    return false;
                }
                comPort.Read(ReadBuff,0, ReturnByte);
                if ((ReadBuff[0] != WriteBuff[0]) || (ReadBuff[1] != WriteBuff[1]))//数据检验失败
                {
                    if (ErrStr.IndexOf("接收数据错误") < 0)
                    {
                        ErrStr = ErrStr + DateTime.Now.ToString() + ",StandarBoardWritePoint," + ":读取失败,接收数据错误" + (char)13 + (char)10;
                    }
                    return false;
                }
            }
            catch (Exception exc)
            {
                if (ErrStr.IndexOf(exc.ToString()) < 0)
                {
                    ErrStr = ErrStr + DateTime.Now.ToString() + ",StandarBoardWritePoint," + ":" + exc.ToString() + (char)13 + (char)10;
                }
                return false;
            }
            return true;
 
        }
        /// <summary>
        /// 读取模块中单个数据
        /// </summary>
        /// <param name="StartAdd">读取数据寄存器</param>
        /// <param name="mReturnBuff">读取寄存器数据</param>
        /// <returns>读取数据是否成功</returns>
        public bool StandarBoardRead(int StartAdd, ref long mReturnBuff)
        {
            long[] tempReturn = new long[1];
            bool returnValue = false;
            returnValue = StandarBoardRead(StartAdd, 1, ref tempReturn);
            if (returnValue)
            {
                mReturnBuff = tempReturn[0];
            }
            return returnValue;

        }
        /// <summary>
        /// 专用于7188,将指定指令发送到模块,并返回设置是否成功
        /// </summary>
        /// <param name="mWriteBuff">byte[],发送7188的指令</param>
        /// <returns>bool,返回设置指令是否成功</returns>
        public bool StandarBoard7188Set(byte[] mWriteBuff)
        {
            byte[] mReadBuff = new byte[100];//接收数据
            int ReturnByte = 0;//返回数据
            bool IsReturn = false;//是否成功返回
            bool IsTimeOut = false;//是否超时
            DateTime NowTime;//当前时间
            TimeSpan ts;//时间差
            if (cMain.isDebug)
            {
                return true;
            }
            if (!mStandarBoardInit)//没有初始化
            {
                StandarBoardInit();
                return false;
            }
            try
            {
                if (!comPort.IsOpen)
                {
                    comPort.Open();
                }
                comPort.DiscardInBuffer();//刷新串口
                comPort.Write(mWriteBuff, 0, mWriteBuff.Length);
                NowTime = DateTime.Now;
                do
                {
                    //System.Windows.Forms.Application.DoEvents();
                    Threading.Thread.Sleep(20);
                    if (comPort.BytesToRead >= mWriteBuff.Length)//收到数据
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
                        ErrStr = ErrStr + DateTime.Now.ToString() + ",StandarBoardRead," + ":读取失败,接收数据已超时" + (char)13 + (char)10;
                    }
                    return false;
                }
                comPort.Read(mReadBuff, 0, ReturnByte);
                if ((mReadBuff[0] != mWriteBuff[0]) || (mReadBuff[1] != mWriteBuff[1]))//数据检验失败
                {
                    if (ErrStr.IndexOf("接收数据错误") < 0)
                    {
                        ErrStr = ErrStr + DateTime.Now.ToString() + ",StandarBoardRead," + ":读取失败,接收数据错误" + (char)13 + (char)10;
                    }
                    return false;
                }
            }
            catch (Exception exc)
            {
                if (ErrStr.IndexOf(exc.ToString()) < 0)
                {
                    ErrStr = ErrStr + DateTime.Now.ToString() + ",StandarBoardRead," + ":" + exc.ToString() + (char)13 + (char)10;
                }
                return false;
            }
            return true;
        }
        /// <summary>
        /// 专用于7188,将指定指令发送到模块,并返回所须要的数据
        /// </summary>
        /// <param name="mWriteBuff">byte[],须要发送出去的指令</param>
        /// <param name="mReadBuff">byte[] ,返回数据</param>
        /// <returns>bool,读取数据是否成功</returns>
        public bool StandarBoard7188Read(byte[] mWriteBuff,out byte[] ReadBuff)
        {
            int i,tempInt;
            ReadBuff = new byte[1];
            byte[] mReadBuff = new byte[100];//接收数据
            int ReturnByte = 0;//返回数据
            bool IsReturn = false;//是否成功返回
            bool IsTimeOut = false;//是否超时
            DateTime NowTime;//当前时间
            TimeSpan ts;//时间差
            if (!mStandarBoardInit)//没有初始化
            {
                StandarBoardInit();
                return false;
            }
            //byte NowByte;
            try
            {
                if (!comPort.IsOpen)
                {
                    comPort.Open();
                }
                comPort.DiscardInBuffer();//刷新串口
                comPort.Write(mWriteBuff, 0, mWriteBuff.Length);
                tempInt = 0;
                NowTime = DateTime.Now;
                do
                {
                    //System.Windows.Forms.Application.DoEvents();
                    Threading.Thread.Sleep(20);
                    tempInt = comPort.BytesToRead;
                    if (tempInt >= 9)//收到数据
                    {
                        Threading.Thread.Sleep(50);
                        ReturnByte = comPort.BytesToRead;
                        if (ReturnByte == tempInt)
                        {
                            IsReturn = true;
                        }
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
                        ErrStr = ErrStr + DateTime.Now.ToString() + ",StandarBoardRead," + ":读取失败,接收数据已超时" + (char)13 + (char)10;
                    }
                    return false;
                }

                if (ReturnByte > 100)
                {
                    if (errStr.IndexOf("数据长度错误") < 0)
                    {
                        errStr = errStr + DateTime.Now.ToString() + ":读取失败,数据长度错误或过长" + (char)13 + (char)10;
                    }
                }
                else
                {
                    comPort.Read(mReadBuff, 0, ReturnByte);
                }

                if ((mReadBuff[0] != mWriteBuff[0]) || (mReadBuff[1] != mWriteBuff[1]))//数据检验失败
                {
                    if (ErrStr.IndexOf("接收数据错误") < 0)
                    {
                        ErrStr = ErrStr + DateTime.Now.ToString() + ",StandarBoardRead," + ":读取失败,接收数据错误" + (char)13 + (char)10;
                    }
                    return false;
                }
                else
                {
                    if (mReadBuff[5] > 0)
                    {
                        ReadBuff = new byte[mReadBuff[6]];
                        for (i = 0; i < mReadBuff[6]; i++)
                        {
                            ReadBuff[i] = mReadBuff[i + 7];
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                if (ErrStr.IndexOf(exc.ToString()) < 0)
                {
                    ErrStr = ErrStr + DateTime.Now.ToString() + ",StandarBoardRead," + ":" + exc.ToString() + (char)13 + (char)10;
                }
                return false;
            }
            return true;
            
        }
        /// <summary>
        /// 读取寄存器数据
        /// </summary>
        /// <param name="StartAdd">int,寄存器开始地址</param>
        /// <param name="PointCount">int,读取个数</param>
        /// <param name="mReturnBuff">byte[],读取到的数据,mReturnBuff.Length应该为PointCount*2</param>
        /// <returns></returns>
        public bool StandarBoardRead(int StartAdd, int PointCount, ref byte[] mReturnBuff)
        {
            int i;
            byte[] WriteBuff = new byte[10];//发送数据
            byte[] ReadBuff = new byte[2 * PointCount + 5];//接收数据
            int ReturnByte = 0;//返回数据
            bool IsReturn = false;//是否成功返回
            bool IsTimeOut = false;//是否超时
            DateTime NowTime;//当前时间
            TimeSpan ts;//时间差
            byte CrcHi = 0, CrcLo = 0;//CRC校验

            if (cMain.isDebug)
            {
                for (i = 0; i < PointCount; i++)
                {
                    mReturnBuff[i] =(byte)( 20 + (byte)(Num.Rand() * 10));
                }
                return true;
            }
            if (!mStandarBoardInit)//没有初始化
            {
                StandarBoardInit();
                return false;
            }
            //byte NowByte;
            try
            {
                WriteBuff[0] = _StandarModebusAddress;
                WriteBuff[1] = 0x03;
                WriteBuff[2] = (byte)((StartAdd & 0xFF00) >> 8);
                WriteBuff[3] = (byte)(StartAdd & 0x00FF);
                WriteBuff[4] = (byte)((PointCount & 0xFF00) >> 8);
                WriteBuff[5] = (byte)(PointCount & 0x00FF);
                cMain.CRC_16(WriteBuff, 6, ref CrcLo, ref CrcHi);
                WriteBuff[6] = CrcLo;
                WriteBuff[7] = CrcHi;
                if (!comPort.IsOpen)
                {
                    comPort.Open();
                }
                comPort.DiscardInBuffer();//刷新串口
                comPort.Write(WriteBuff, 0, 8);
                NowTime = DateTime.Now;
                do
                {
                    //System.Windows.Forms.Application.DoEvents();
                    Threading.Thread.Sleep(20);
                    if (comPort.BytesToRead >= (2 * WriteBuff[5] + 5))//收到数据
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
                        ErrStr = ErrStr + DateTime.Now.ToString() + ",StandarBoardRead," + ":读取失败,接收数据已超时" + (char)13 + (char)10;
                    }
                    return false;
                }
                comPort.Read(ReadBuff,0,ReturnByte);
                if ((ReadBuff[0] != WriteBuff[0]) || (ReadBuff[1] != WriteBuff[1]))//数据检验失败
                {
                    if (ErrStr.IndexOf("接收数据错误") < 0)
                    {
                        ErrStr = ErrStr + DateTime.Now.ToString() + ",StandarBoardRead," + ":读取失败,接收数据错误" + (char)13 + (char)10;
                    }
                    return false;
                }
                else
                {
                    for (i = 0; i < PointCount*2; i++)
                    {
                        mReturnBuff[i] = ReadBuff[i + 3];
                    }
                }
            }
            catch (Exception exc)
            {
                if (ErrStr.IndexOf(exc.ToString()) < 0)
                {
                    ErrStr = ErrStr + DateTime.Now.ToString() + ",StandarBoardRead," + ":" + exc.ToString() + (char)13 + (char)10;
                }
                return false;
            }
            return true;
            
        }
        /// <summary>
        /// 读取模块中多个数据
        /// </summary>
        /// <param name="StartAdd">读取数据开始寄存器</param>
        /// <param name="PointCount">读取寄存器个数</param>
        /// <param name="mReturnBuff">读取寄存器数据</param>
        /// <returns>读取数据是否成功</returns>
        public bool StandarBoardRead(int StartAdd, int PointCount, ref long[] mReturnBuff)
        {
            int i;
            long[] ReturnBuff = new long[PointCount];
            byte[] WriteBuff = new byte[10];//发送数据
            byte[] ReadBuff = new byte[2 * PointCount + 5];//接收数据
            int ReturnByte = 0;//返回数据
            bool IsReturn = false;//是否成功返回
            bool IsTimeOut = false;//是否超时
            DateTime NowTime;//当前时间
            TimeSpan ts;//时间差
            byte CrcHi = 0, CrcLo = 0;//CRC校验

            if (cMain.isDebug)
            {
                for (i = 0; i < PointCount; i++)
                {
                    mReturnBuff[i] = 0 + (long)(Num.Rand() * 10);
                }
                return true;
            }
            if (!mStandarBoardInit)//没有初始化
            {
                StandarBoardInit();
                return false;
            }
            try
            {
                //05 03	24 10 00 06 CF 79
                WriteBuff[0] = _StandarModebusAddress;
                WriteBuff[1] = 0x03;
                WriteBuff[2] = (byte)((StartAdd & 0xFF00) >> 8);
                WriteBuff[3] = (byte)(StartAdd & 0x00FF);
                WriteBuff[4] = (byte)((PointCount & 0xFF00) >> 8);
                WriteBuff[5] = (byte)(PointCount & 0x00FF);
                cMain.CRC_16(WriteBuff, 6, ref CrcLo, ref CrcHi);
                WriteBuff[6] = CrcLo;
                WriteBuff[7] = CrcHi;
                if (!comPort.IsOpen)
                {
                    comPort.Open();
                }
                comPort.DiscardInBuffer();//刷新串口
                comPort.Write(WriteBuff, 0, 8);
                NowTime = DateTime.Now;
                do
                {
                    //System.Windows.Forms.Application.DoEvents();
                    Threading.Thread.Sleep(20);
                    if (comPort.BytesToRead >= (2 * WriteBuff[5] + 5))//收到数据
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
                        ErrStr = ErrStr + DateTime.Now.ToString() + ",StandarBoardRead," + ":读取失败,接收数据已超时" + (char)13 + (char)10;
                    }
                    return false;
                }
                comPort.Read(ReadBuff,0, ReturnByte);
                if ((ReadBuff[0] != WriteBuff[0]) || (ReadBuff[1] != WriteBuff[1]))//数据检验失败
                {
                    //string tempStr = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{3},{14},{15},{16},{17},{18},{19},{20}",
                    //    ReadBuff[0], ReadBuff[1], ReadBuff[2], ReadBuff[3], ReadBuff[4], ReadBuff[5], ReadBuff[6], ReadBuff[7], ReadBuff[8],
                    //    ReadBuff[9], ReadBuff[10], ReadBuff[11], ReadBuff[12], ReadBuff[13], ReadBuff[14], ReadBuff[15], ReadBuff[16], ReadBuff[17]
                    //    , ReadBuff[18], ReadBuff[19], ReadBuff[20]);
                    //cMain.WriteErrorToLog(tempStr);
                    if (ErrStr.IndexOf("接收数据错误") < 0)
                    {
                        ErrStr = ErrStr + DateTime.Now.ToString() + ",StandarBoardRead," + ":读取失败,接收数据错误" + (char)13 + (char)10;
                    }
                    return false;
                }
                else
                {
                    for (i = 0; i < ReturnBuff.Length; i++)
                    {
                        ReturnBuff[i] = (ReadBuff[i * 2 + 3] & 0xFF) * 256 + (ReadBuff[i * 2 + 4] & 0xFF);
                        //if ((ReadBuff[i * 2 + 3] >> 7) == 1)//如果是负数
                        //{
                        //    ReturnBuff[i] = -((ReturnBuff[i] ^ 65535) + 1);
                        //}
                        mReturnBuff[i] = ReturnBuff[i];
                    }
                }
            }
            catch (Exception exc)
            {
                if (ErrStr.IndexOf(exc.ToString()) < 0)
                {
                    ErrStr = ErrStr + DateTime.Now.ToString() + ",StandarBoardRead," + ":" + exc.ToString() + (char)13 + (char)10;
                }
                return false;
            }
            return true;
        }
       
        //private bool HJReadIO(out bool Led1, out bool Led2, out bool Led3)
        //{
        //    long[] ReturnBuff = new long[8];
        //    bool L1 = false, L2 = false, L3 = false;
        //    Led1 = L1; Led2 = L2; Led3 = L3;
        //    if (!mStandarBoardInit)//没有初始化
        //    {
        //        return StandarBoardInit();
        //    }
        //    byte[] WriteBuff = new byte[10];//发送数据
        //    byte[] ReadBuff = new byte[21];//接收数据
        //    int ReturnByte = 0;//返回数据
        //    bool IsReturn = false;//是否成功返回
        //    bool IsTimeOut = false;//是否超时
        //    int ReadValue = 0;
        //    DateTime NowTime;//当前时间
        //    TimeSpan ts;//时间差
        //    byte CrcHi = 0, CrcLo = 0;//CRC校验
        //    try
        //    {
        //        WriteBuff[0] = _StandarModebusAddress;
        //        WriteBuff[1] = 0x03;
        //        WriteBuff[2] = 0x00;
        //        WriteBuff[3] = 0x09;
        //        WriteBuff[4] = 0x00;
        //        WriteBuff[5] = 0x01;
        //        cMain.CRC_16(WriteBuff, 6, ref CrcLo, ref CrcHi);
        //        WriteBuff[6] = CrcLo;
        //        WriteBuff[7] = CrcHi;
        //        if (!comPort.IsOpen)
        //        {
        //            comPort.Open();
        //        }
        //        comPort.DiscardInBuffer();//刷新串口
        //        comPort.Write(WriteBuff, 0, 8);
        //        NowTime = DateTime.Now;
        //        do
        //        {
        //            if (comPort.BytesToRead >= WriteBuff[5]*2+5)//收到数据
        //            {
        //                ReturnByte = comPort.BytesToRead;
        //                IsReturn = true;
        //            }
        //            ts = DateTime.Now - NowTime;
        //            if (ts.TotalMilliseconds > timeOut)//时间超时
        //            {
        //                IsTimeOut = true;
        //            }
        //        } while (!IsReturn && !IsTimeOut);
        //        if (!IsReturn && IsTimeOut)//超时
        //        {
        //            if (ErrStr.IndexOf("接收数据已超时") < 0)
        //            {
        //                ErrStr = ErrStr + DateTime.Now.ToString() + mName + ":读取失败,接收数据已超时" + (char)13 + (char)10;
        //            }
        //            comPort.Close();
        //            return false;
        //        }
        //        comPort.Read(ReadBuff, 0, ReturnByte);
        //        if ((ReadBuff[0] != WriteBuff[0]) || (ReadBuff[1] != WriteBuff[1]))//数据检验失败
        //        {
        //            comPort.Close(); if (ErrStr.IndexOf("接收数据错误") < 0)
        //            {
        //                ErrStr = ErrStr + DateTime.Now.ToString() + mName + ":读取失败,接收数据错误" + (char)13 + (char)10;
        //            }
        //            return false;
        //        }
        //        else
        //        {
        //            ReadValue = ReadBuff[3] * 256 + ReadBuff[4];
        //            if ((ReadValue & 1) == 1)
        //            {
        //                L1 = true;
        //            }
        //            if ((ReadValue & 2) == 2)
        //            {
        //                L2 = true;
        //            }
        //            if ((ReadValue & 4) == 4)
        //            {
        //                L3 = true;
        //            }
        //        }
        //    }
        //    catch (Exception exc)
        //    {
        //        if (ErrStr.IndexOf(exc.ToString()) < 0)
        //        {
        //            ErrStr = ErrStr + DateTime.Now.ToString() + mName + ":" + exc.ToString() + (char)13 + (char)10;
        //        }
        //        if (comPort.IsOpen)
        //        {
        //            comPort.Close();
        //        }
        //        return false;
        //    }
        //    Led1 = L1; Led2 = L2; Led3 = L3;
        //    return true;
        //}
       
    }
}
