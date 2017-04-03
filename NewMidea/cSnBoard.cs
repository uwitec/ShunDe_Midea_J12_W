using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using NewMideaProgram;
namespace System
{
    public class cSnBoard
    {
        cStandarBoard mStandarBoard;
        public static string mName = "变频板\r\n" +
                                    "cSnBoard:构构造函数\r\n" +
                                    "SnBoardInit:对变频板进行初始化\r\n" +
                                    "SnBoardLed:让变频板LED灯闪烁一次\r\n" +
                                    "SnBoardReadCmd:读取变频板返回数据\r\n" +
                                    "SnBoardSend:变频板给外机发送一次数据\r\n" +
                                    "SnBoardWriteInit:向变频板写运行初始化\r\n" +
                                    "SnWriteCode:将发送指令写入变频板\r\n" +
                                    "TurnTemp:将读取的温度AD值转化为实际温度";
        SerialPort comPort;//端口
        /// <summary>
        /// SN板使用的串口
        /// </summary>
        public SerialPort ComPort
        {
            get { return comPort; }
            set { comPort = value; }
        }
        string errStr = "SnBoard";//出错信息
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
        string snCode = "";//SN版本号
        /// <summary>
        /// 只读,SN版本号
        /// </summary>
        public string SnCode
        {
            get { return snCode; }
        }
        byte StandarModebusAddress = 0;//LGPLC地址
        bool mSnBoardInit = false;//PLC初始化结果
        /// <summary>
        /// 构造函数,实例化SN板
        /// </summary>
        /// <param name="mComPort">SN板使用的串口</param>
        /// <param name="mAddress">SN板地址</param>
        public cSnBoard(SerialPort mComPort, byte mAddress)//构造函数
        {
            comPort = mComPort;
            StandarModebusAddress = mAddress;
            mStandarBoard = new cStandarBoard(comPort, StandarModebusAddress, timeOut);
        }
        int intLed = 0;//记录LED灯的次数
        public static string ReadSnStr = "";
        public static string SendSnStr = "";
        /// <summary>
        /// 初始化SN板,返回初始化结果.初始化过程,读取SN版本号.
        /// </summary>
        /// <returns>bool,返回初始化结果</returns>
        public bool SnBoardInit()//初始化SN板,返回初始化结果
        {
            mSnBoardInit = mStandarBoard.StandarBoardInit();
            if (!mSnBoardInit)
            {
                if (errStr.IndexOf(mStandarBoard.ErrStr) < 0)
                {
                    errStr = errStr + mStandarBoard.ErrStr;
                }
            }
            return mSnBoardInit;
            /*
            if (cMain.isDebug)
            {
                mSnBoardInit = true;
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
                WriteBuff[0] = (byte)(StandarModebusAddress & 0xFF);
                WriteBuff[1] = 0x03;
                WriteBuff[2] = 0x00;
                WriteBuff[3] = 0x00;
                WriteBuff[4] = 0x00;
                WriteBuff[5] = 0x01;
                cMain.CRC_16(WriteBuff, 6, ref CrcLo, ref CrcHi);
                WriteBuff[6] = CrcLo;
                WriteBuff[7] = CrcHi;
                comPort.DiscardOutBuffer();//刷新串口
                comPort.Write(WriteBuff, 0, 8);
                NowTime = DateTime.Now;
                do
                {
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
                    if (ErrStr.IndexOf("初始失败,接收数据已超时") < 0)
                    {
                        ErrStr = ErrStr + DateTime.Now.ToString() + mName + ":初始失败,接收数据已超时" + (char)13 + (char)10;
                    }
                    return false;
                }
                else
                {
                    comPort.Read(ReadBuff, 0, ReturnByte);
                    if ((ReadBuff[0] != WriteBuff[0]) || (ReadBuff[1] != WriteBuff[1]))//数据检验失败
                    {
                        if (ErrStr.IndexOf("初始失败,接收数据错误") < 0)
                        {
                            ErrStr = ErrStr + DateTime.Now.ToString() + mName + ":初始失败,接收数据错误" + (char)13 + (char)10;
                        }
                        return false;
                    }
                    else
                    {
                        snCode = ReadBuff[3].ToString();//SN版本号;
                        mSnBoardInit = true;
                    }
                }
            }
            catch (Exception exc)
            {
                if (ErrStr.IndexOf(exc.ToString()) < 0)
                {
                    ErrStr = ErrStr + DateTime.Now.ToString() + mName + ":" + exc.ToString() + (char)13 + (char)10;
                }
                return false;
            }
            return true;
             */
        }
        /// <summary>
        ///将要发送的命令写入到SN板中
        /// </summary>
        /// <param name="BaudRate">要发送机型的波特率</param>
        /// <param name="SendCmd">要发送的命令</param>
        /// <returns>返回发送是否成功</returns>
        public bool SnWriteCode(int BaudRate, string SendCmd)
        {
            if (cMain.isDebug)
            {
                return true;
            }
            SendCmd = SendCmd.Trim();
            int CurLen = SendCmd.Length;
            if ((SendCmd.Length % 2) == 1)
            {
                if (ErrStr.IndexOf("指令长度不正确") < 0)
                {
                    ErrStr = ErrStr + DateTime.Now.ToShortDateString() + ":指令长度不正确" + "\n" + "\r";
                }
                return false;
            }
            else 
            {
                SendCmd = SendCmd + "0000000000000000000000000000000000000000000000000000";
                SendCmd = SendCmd.Substring(0, 44);
            }
            if (!mSnBoardInit)
            {
                 SnBoardInit();
                 return false;
            }
            int i;
            bool returnValue = false;
            byte[] WriteBuff = new byte[50];//发送数据
            WriteBuff[0] = 0x00;//写延时
            WriteBuff[1] = 0x02;
            WriteBuff[2] = 0x0A;//读延时
            WriteBuff[3] = 0x40;
            WriteBuff[4] = 0x00;//
            WriteBuff[5] = 0x10;
            WriteBuff[6] = 0x00;//
            WriteBuff[7] = 0x2C;
            switch (BaudRate)
            {
                case 600:
                    if (CurLen == 40)
                    {
                        WriteBuff[8] = 0x2;
                        WriteBuff[9] = 0x14;
                    }
                    else
                    {
                        WriteBuff[8] = 0x2;
                        WriteBuff[9] = 0x12;
                    }
                    WriteBuff[10] = 0x09;
                    WriteBuff[11] = 0xF2;
                    break;
                case 1023:
                    WriteBuff[8] = 0x02;
                    WriteBuff[9] = 0x06;
                    WriteBuff[10] = 0x05;
                    WriteBuff[11] = 0xC9;
                    break;
                case 1200:
                    WriteBuff[8] = 0x02;
                    WriteBuff[9] = 0x08;
                    WriteBuff[10] = 0x04;
                    WriteBuff[11] = 0xF5;
                    break;
                default:
                    WriteBuff[8] = 0x02;
                    WriteBuff[9] = 0x12;
                    WriteBuff[10] = 0x09;
                    WriteBuff[11] = 0xF2;
                    break;
            }
            returnValue = mStandarBoard.StandarBoardWritePoint(1, 6, WriteBuff);
            if (!returnValue)
            {
                if (errStr.IndexOf(mStandarBoard.ErrStr) < 0)
                {
                    errStr = errStr + mStandarBoard.ErrStr;
                }
                return false;
            }
            for (i = 0; i < (SendCmd.Length / 2); i++)
            {
                WriteBuff[i] = Num.ByteParseFromHex(SendCmd.Substring(2 * i, 2));
            }
            SendSnStr = SendCmd.Trim();
            returnValue = mStandarBoard.StandarBoardWritePoint(8, 11, WriteBuff);
            if (!returnValue)
            {
                if (errStr.IndexOf(mStandarBoard.ErrStr) < 0)
                {
                    errStr = errStr + mStandarBoard.ErrStr;
                }
            }
            return returnValue;
            //try
            //{
            //    if (!comPort.IsOpen)
            //    {
            //        comPort.Open();
            //    }
            //    WriteBuff[0] = (byte)(StandarModebusAddress & 0xFF);//物理地址
            //    WriteBuff[1] = 0x10;//命令码
            //    WriteBuff[2] = 0x00;//地址高位
            //    WriteBuff[3] = 0x01;//地址低位
            //    WriteBuff[4] = 0x00;//数据长度高字节
            //    WriteBuff[5] = 0x06;//数据长度低字节
            //    WriteBuff[6] = 0x0C;//数据长度字节数
            //    WriteBuff[7] = 0x00;//写延时
            //    WriteBuff[8] = 0x02;
            //    WriteBuff[9] = 0x0A;//读延时
            //    WriteBuff[10] = 0x40;
            //    WriteBuff[11] = 0x00;//
            //    WriteBuff[12] = 0x10;
            //    WriteBuff[13] = 0x00;//
            //    WriteBuff[14] = 0x2C;
            //    //WriteBuff[15] = 0x00;//
            //    //WriteBuff[16] = 0x10;
            //    //WriteBuff[17] = 0x00;//
            //    //WriteBuff[18] = 0x2C;
            //    cMain.CRC_16(WriteBuff, 19, ref CrcLo, ref CrcHi);
            //    WriteBuff[19] = CrcLo;
            //    WriteBuff[20] = CrcHi;
            //    comPort.DiscardOutBuffer();//刷新串口
            //    comPort.Write(WriteBuff, 0, WriteBuff[5] * 2 + 9);
            //    NowTime = DateTime.Now;
            //    do
            //    {
            //        if (comPort.BytesToRead >= 8)//收到数据
            //        {
            //            ReturnByte = comPort.BytesToRead;
            //            IsReturn = true;
            //        }
            //        ts = DateTime.Now - NowTime;
            //        if (ts.TotalMilliseconds > timeOut)//时间超时
            //        {
            //            IsTimeOut = true;
            //        }
            //    } while (!IsReturn && !IsTimeOut);
            //    if (!IsReturn && IsTimeOut)//超时
            //    {
            //        if (ErrStr.IndexOf("写入失败,接收数据已超时") < 0)
            //        {
            //            ErrStr = ErrStr + DateTime.Now.ToString() + mName + ":写入失败,接收数据已超时" + (char)13 + (char)10;
            //        }
            //        return false;
            //    }
            //    else
            //    {
            //        comPort.Read(ReadBuff, 0, ReturnByte);
            //        if ((ReadBuff[0] != WriteBuff[0]) || (ReadBuff[1] != WriteBuff[1]))//数据检验失败
            //        {
            //            if (ErrStr.IndexOf("写入失败,接收数据错误") < 0)
            //            {
            //                ErrStr = ErrStr + DateTime.Now.ToString() + mName + ":写入失败,接收数据错误" + (char)13 + (char)10;
            //            }
            //            return false;
            //        }
            //    }
            //}
            //catch (Exception exc)
            //{
            //    if (ErrStr.IndexOf(exc.ToString()) < 0)
            //    {
            //        ErrStr = ErrStr + DateTime.Now.ToString() + mName + ":" + exc.ToString() + (char)13 + (char)10;
            //    }
            //    return false;
            //}
            //try
            //{
            //    if (!comPort.IsOpen)
            //    {
            //        comPort.Open();
            //    }
            //    WriteBuff[0] = (byte)(StandarModebusAddress & 0xFF);//物理地址
            //    WriteBuff[1] = 0x10;//命令码
            //    WriteBuff[2] = 0x00;//地址高位
            //    WriteBuff[3] = 0x08;//地址低位
            //    WriteBuff[4] = 0x00;//数据长度高字节
            //    WriteBuff[5] = 0x0B;//数据长度低字节
            //    WriteBuff[6] = 0x16;//数据长度字节数
            //    for (i = 0; i < (SendCmd.Length / 2); i++)
            //    {
            //        WriteBuff[7 + i] = Num.ByteParseFromHex(SendCmd.Substring(2 * i, 2));
            //    }
            //    cMain.CRC_16(WriteBuff, (7 + SendCmd.Length / 2), ref CrcLo, ref CrcHi);
            //    WriteBuff[(7 + SendCmd.Length / 2)] = CrcLo;
            //    WriteBuff[(7 + SendCmd.Length / 2) + 1] = CrcHi;
            //    comPort.DiscardOutBuffer();//刷新串口
            //    comPort.Write(WriteBuff, 0, (9 + WriteBuff[5]));
            //    NowTime = DateTime.Now;
            //    do
            //    {
            //        if (comPort.BytesToRead >= 8)//收到数据
            //        {
            //            ReturnByte = comPort.BytesToRead;
            //            IsReturn = true;
            //        }
            //        ts = DateTime.Now - NowTime;
            //        if (ts.TotalMilliseconds > timeOut)//时间超时
            //        {
            //            IsTimeOut = true;
            //        }
            //    } while (!IsReturn && !IsTimeOut);
            //    if (!IsReturn && IsTimeOut)//超时
            //    {
            //        if (ErrStr.IndexOf("写入指令失败,接收数据已超时") < 0)
            //        {
            //            ErrStr = ErrStr + DateTime.Now.ToString() + mName + ":写入指令失败,接收数据已超时" + (char)13 + (char)10;
            //        }
            //        return false;
            //    }
            //    else
            //    {
            //        comPort.Read(ReadBuff, 0, ReturnByte);
            //        if ((ReadBuff[0] != WriteBuff[0]) || (ReadBuff[1] != WriteBuff[1]))//数据检验失败
            //        {
            //            if (ErrStr.IndexOf("写入指令失败,接收数据错误") < 0)
            //            {
            //                ErrStr = ErrStr + DateTime.Now.ToString() + mName + ":写入指令失败,接收数据错误" + (char)13 + (char)10;
            //            }
            //            return false;
            //        }
            //    }
            //}
            //catch (Exception exc)
            //{
            //    if (ErrStr.IndexOf(exc.ToString()) < 0)
            //    {
            //        ErrStr = ErrStr + DateTime.Now.ToString() + mName + ":" + exc.ToString() + (char)13 + (char)10;
            //    }
            //    return false;
            //}
            //SendSnStr = SendCmd;
            //return true;
        }
        /// <summary>
        /// 要求SN板发送一次指令给外机,不管发送结果是否成功
        /// </summary>
        /// <returns>此次操作是否成功</returns>
        public bool SnBoardSend()
        {
            bool returnValue = false;
            returnValue = mStandarBoard.StandarBoardWritePoint(7, 2, 6);
            if (!returnValue)
            {
                if (errStr.IndexOf(mStandarBoard.ErrStr) < 0)
                {
                    errStr = errStr + mStandarBoard.ErrStr;
                }
            }
            return returnValue;
            /*
            if (cMain.isDebug)
            {
                return true;
            }
            if (!mSnBoardInit)
            {
                SnBoardInit();
                return false;
            }
            byte[] WriteBuff = new byte[10];//发送数据
            byte[] ReadBuff = new byte[10];//接收数据
            byte CrcHi = 0, CrcLo = 0;//CRC校验
            try
            {
                if (!comPort.IsOpen)
                {
                    comPort.Open();
                }
                WriteBuff[0] = (byte)(StandarModebusAddress & 0xFF);
                WriteBuff[1] = 0x06;
                WriteBuff[2] = 0x00;
                WriteBuff[3] = 0x07;
                WriteBuff[4] = 0x00;
                WriteBuff[5] = 0x02;
                cMain.CRC_16(WriteBuff, 6, ref CrcLo, ref CrcHi);
                WriteBuff[6] = CrcLo;
                WriteBuff[7] = CrcHi;
                comPort.DiscardOutBuffer();//刷新串口
                comPort.Write(WriteBuff, 0, 8);
            }
            catch (Exception exc)
            {
                if (ErrStr.IndexOf(exc.ToString()) < 0)
                {
                    ErrStr = ErrStr + DateTime.Now.ToString() + mName + ":" + exc.ToString() + (char)13 + (char)10;
                }
                return false;
            }
            return true;
             */
        }
        ///// <summary>
        ///// 要求SN板发送一次指令给外机
        ///// </summary>
        ///// <returns>返回此次操作是否成功</returns>
        //public bool SnBoardSendDelay()//SN板发送一次命令
        //{
        //    if (cMain.isDebug)
        //    {
        //        return true;
        //    }
        //    if (!mSnBoardInit)
        //    {
        //        SnBoardInit();
        //        return false;
        //    }
        //    byte[] WriteBuff = new byte[10];//发送数据
        //    byte[] ReadBuff = new byte[10];//接收数据
        //    int ReturnByte = 0;//返回数据
        //    bool IsReturn = false;//是否成功返回
        //    bool IsTimeOut = false;//是否超时
        //    DateTime NowTime = DateTime.Now;//当前时间
        //    TimeSpan ts;//时间差
        //    byte CrcHi = 0, CrcLo = 0;//CRC校验
        //    try
        //    {
        //        if (!comPort.IsOpen)
        //        {
        //            comPort.Open();
        //        }
        //        WriteBuff[0] = (byte)(StandarModebusAddress & 0xFF);
        //        WriteBuff[1] = 0x06;
        //        WriteBuff[2] = 0x00;
        //        WriteBuff[3] = 0x07;
        //        WriteBuff[4] = 0x00;
        //        WriteBuff[5] = 0x02;
        //        cMain.CRC_16(WriteBuff, 6, ref CrcLo, ref CrcHi);
        //        WriteBuff[6] = CrcLo;
        //        WriteBuff[7] = CrcHi;
        //        comPort.DiscardOutBuffer();//刷新串口
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
        //            if (ErrStr.IndexOf("SN发送失败,接收数据已超时") < 0)
        //            {
        //                ErrStr = ErrStr + DateTime.Now.ToString() + mName + ":发送失败,接收数据已超时" + (char)13 + (char)10;
        //            }
        //            return false;
        //        }
        //        else
        //        {
        //            comPort.Read(ReadBuff, 0, ReturnByte);
        //            if ((ReadBuff[0] != WriteBuff[0]) || (ReadBuff[1] != WriteBuff[1]))//数据检验失败
        //            {
        //                if (ErrStr.IndexOf("SN发送失败,接收数据错误") < 0)
        //                {
        //                    ErrStr = ErrStr + DateTime.Now.ToString() + mName + ":发送失败,接收数据错误" + (char)13 + (char)10;
        //                }
        //                return false;
        //            }
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
        /// 读取SN板上外机返回指令,前6位数据(接收指令长度,频率,冷凝,室温,排气温度,T3)
        /// </summary>
        /// <param name="ReturnBuff">数组,将要返回的数据</param>
        /// <returns>返回读取变频板是否成功,-1返回数据不正确,0读取数据不正确,1正确</returns>
        public int SnBoardReadCmd(out long[] ReturnBuff)//读SN板
        {
            ReturnBuff=new long[10];
            int i;
            for (i = 0; i < ReturnBuff.Length; i++)
            {
                ReturnBuff[i] = 0;
            }
            ReadSnStr = "";
            if (cMain.isDebug)
            {
                ReturnBuff[0] = 18;
                ReturnBuff[1] = 0x32 + (int)(Num.Rand() * 10);
                ReturnBuff[2] = 0x7D + (int)(Num.Rand() * 10);
                ReturnBuff[3] = 0x7D + (int)(Num.Rand() * 10);
                ReturnBuff[4] = 0x7D + (int)(Num.Rand() * 10);
                return 1;
            }
            bool returnValue = false;
            byte[] readValue=new byte[30];
            long SnLength = 0;
            returnValue = mStandarBoard.StandarBoardRead(0x15, 0x0F,ref readValue);
            if (!returnValue)
            {
                if (errStr.IndexOf(mStandarBoard.ErrStr) < 0)
                {
                    errStr = errStr + mStandarBoard.ErrStr;
                }
                return 0;
            }
            else
            {
                SnLength = readValue[1];
                for (i = 0; i < SnLength; i++)
                {
                    if (i < readValue.Length-2)
                    {
                        ReadSnStr = ReadSnStr + string.Format("{0:X2}", readValue[i + 2]);
                    }
                }
                switch (SnLength)
                {
                    case 18:
                        ReturnBuff[1] = readValue[7];
                        ReturnBuff[2] = readValue[10];
                        ReturnBuff[3] = readValue[11];
                        ReturnBuff[4] = readValue[12];
                        ReturnBuff[5] = 0;
                        break;
                    case 20:
                        ReturnBuff[1] = readValue[8];
                        ReturnBuff[2] = readValue[11];
                        ReturnBuff[3] = readValue[12];
                        ReturnBuff[4] = readValue[13];
                        ReturnBuff[5] = 0;
                        break;
                    case 6:
                        ReturnBuff[1] = 0;
                        ReturnBuff[2] = 0;
                        ReturnBuff[3] = 0;
                        ReturnBuff[4] = 0;
                        ReturnBuff[5] = readValue[2];
                        break;
                    case 8:
                        ReturnBuff[1] = 0;
                        ReturnBuff[2] = readValue[4];
                        ReturnBuff[3] = readValue[5];
                        ReturnBuff[4] = 0;
                        ReturnBuff[5] = 0;
                        break;
                    default :
                        return -1;
                }
            }
            return 1;
            /*
            if (!mSnBoardInit)//没有初始化
            {
                SnBoardInit();
                return false;
            }
            if (ReturnBuff.Length < 10)
            {
                if (ErrStr.IndexOf("输入数组大小不能小于") < 0)
                {
                    ErrStr = ErrStr + DateTime.Now.ToString() + mName + ":" + "输入数组大小不能小于10" + (char)13 + (char)10;
                }
                return false;
            }
            byte[] WriteBuff = new byte[10];//发送数据
            byte[] ReadBuff = new byte[80];//接收数据
            int[] ReadTempBuff = new int[35];//临时数据
            int ReturnByte = 0;//返回数据
            bool IsReturn = false;//是否成功返回
            bool IsTimeOut = false;//是否超时
            DateTime NowTime;//当前时间
            TimeSpan ts;//时间差
            byte CrcHi = 0, CrcLo = 0;//CRC校验
            int i;
            try
            {
                WriteBuff[0] = StandarModebusAddress;
                WriteBuff[1] = 0x03;
                WriteBuff[2] = 0x00;
                WriteBuff[3] = 0x15;
                WriteBuff[4] = 0x00;
                WriteBuff[5] = 0x0B;
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
                    if (ErrStr.IndexOf("读取失败,接收数据已超时") < 0)
                    {
                        ErrStr = ErrStr + DateTime.Now.ToString() + mName + ":读取失败,接收数据已超时" + (char)13 + (char)10;
                    }
                    return false;
                }
                comPort.Read(ReadBuff, 0, ReturnByte);
                if ((ReadBuff[0] != WriteBuff[0]) || (ReadBuff[1] != WriteBuff[1]))//数据检验失败
                {
                    if (ErrStr.IndexOf("读取失败,接收数据错误") < 0)
                    {
                        ErrStr = ErrStr + DateTime.Now.ToString() + mName + ":读取失败,接收数据错误" + (char)13 + (char)10;
                    }
                    return false;
                }
                else
                {
                    for (i = 0; i < WriteBuff[5]; i++)
                    {
                        ReadTempBuff[i] = (ReadBuff[i * 2 + 3] & 0xFF) * 256 + (ReadBuff[i * 2 + 4] & 0xFF);

                    }
                    for (i = 1; i < 10; i++)
                    {
                        ReadSnStr = ReadSnStr + string.Format("{0:X2}", ReadBuff[i * 2 + 3]);
                        ReadSnStr = ReadSnStr + string.Format("{0:X2}", ReadBuff[i * 2 + 4]);
                    }
                }
            }
            catch (Exception exc)
            {
                if (ErrStr.IndexOf(exc.ToString()) < 0)
                {
                    ErrStr = ErrStr + DateTime.Now.ToString() + mName + ":" + exc.ToString() + (char)13 + (char)10;
                }
                return false;
            }
            for (i = 0; i < ReturnBuff.Length; i++)
            {
                ReturnBuff[i] = ReadBuff[i + 3];
            }
            return true;
             */
        }
        /// <summary>
        /// 要求SN板上的LED闪烁一次
        /// </summary>
        /// <returns>此次操作是否成功</returns>
        public bool SnBoardLed()//使变频板的LED灯闪烁一次
        {
            bool returnValue = false;
            if (cMain.isDebug)
            {
                return true;
            }
            intLed = intLed + 1;
            if (intLed > 5)
            {
                intLed = 0;
            }
            int WriteBuff = (intLed * 5) << 8;
            returnValue = mStandarBoard.StandarBoardWritePoint(0x14, WriteBuff, 6);
            if (!returnValue)
            {
                if (errStr.IndexOf(mStandarBoard.ErrStr) < 0)
                {
                    errStr = errStr + mStandarBoard.ErrStr;
                }
            }
            return returnValue;
            //byte CrcHi = 0, CrcLo = 0;//CRC校验
            //try
            //{
            //    if (!comPort.IsOpen)
            //    {
            //        comPort.Open();
            //    }
            //    WriteBuff[0] = (byte)(StandarModebusAddress & 0xFF);
            //    WriteBuff[1] = 0x06;
            //    WriteBuff[2] = 0x00;
            //    WriteBuff[3] = 0x14;
            //    WriteBuff[4] = (byte)(intLed * 5);
            //    WriteBuff[5] = 0x00;
            //    cMain.CRC_16(WriteBuff, 6, ref CrcLo, ref CrcHi);
            //    WriteBuff[6] = CrcLo;
            //    WriteBuff[7] = CrcHi;
            //    comPort.DiscardOutBuffer();//刷新串口
            //    comPort.Write(WriteBuff, 0, 8);
            //    DateTime NowTime = DateTime.Now;
            //    TimeSpan ts = new TimeSpan();
            //    bool IsReturn = false;
            //    bool IsTimeOut = false;
            //    int ReturnByte = 0;
            //    do
            //    {
            //        if (comPort.BytesToRead >= 8)//收到数据
            //        {
            //            ReturnByte = comPort.BytesToRead;
            //            IsReturn = true;
            //        }
            //        ts = DateTime.Now - NowTime;
            //        if (ts.TotalMilliseconds > timeOut)//时间超时
            //        {
            //            IsTimeOut = true;
            //        }
            //    } while (!IsReturn && !IsTimeOut);
            //}
            //catch (Exception exc)
            //{
            //    if (ErrStr.IndexOf(exc.ToString()) < 0)
            //    {
            //        ErrStr = ErrStr + DateTime.Now.ToString() + mName + ":" + exc.ToString() + (char)13 + (char)10;
            //    }
            //    return false;
            //}
            //return true;
        }
        /// <summary>
        /// 将读到的数据转化为温度数据
        /// </summary>
        /// <param name="ReadTemp">long,读到的数据</param>
        /// <param name="isPaiQi">bool,是不是排气温度,因为排气温度的算法不同</param>
        /// <returns>double,返回转化后的实际温度数据</returns>
        public static double TurnTemp(long ReadTemp, bool isPaiQi)
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
        /// 向板里面写 运行初始化
        /// </summary>
        /// <returns></returns>
        public bool SnBoardWriteInit()
        {
            bool returnValue = false;
            if (cMain.isDebug)
            {
                return true;
            }
            if (!mSnBoardInit)
            {
                SnBoardInit();
                returnValue = false;
            }
            byte[] WriteBuff = new byte[50];//发送数据
            WriteBuff[0] = 0x00;
            WriteBuff[1] = 0x02;
            WriteBuff[2] = 0x0A;
            WriteBuff[3] = 0x40;
            returnValue = mStandarBoard.StandarBoardWritePoint(1, 2, WriteBuff);
            if (!returnValue)
            {
                if (errStr.IndexOf(mStandarBoard.ErrStr) < 0)
                {
                    errStr = errStr + mStandarBoard.ErrStr;
                }
            }
            return returnValue;

            //byte[] ReadBuff = new byte[10];//接收数据
            //int ReturnByte = 0;//返回数据
            //bool IsReturn = false;//是否成功返回
            //bool IsTimeOut = false;//是否超时
            //DateTime NowTime = DateTime.Now;//当前时间
            //TimeSpan ts;//时间差
            //byte CrcHi = 0, CrcLo = 0;//CRC校验
            //try
            //{
            //    if (!comPort.IsOpen)
            //    {
            //        comPort.Open();
            //    }
            //    WriteBuff[0] = (byte)(StandarModebusAddress & 0xFF);//物理地址
            //    WriteBuff[1] = 0x10;//命令码
            //    WriteBuff[2] = 0x00;//地址高位
            //    WriteBuff[3] = 0x01;//地址低位
            //    WriteBuff[4] = 0x00;//数据长度高字节
            //    WriteBuff[5] = 0x02;
            //    WriteBuff[6] = 0x04;
            //    WriteBuff[7] = 0x00;
            //    WriteBuff[8] = 0x02;
            //    WriteBuff[9] = 0x0A;
            //    WriteBuff[10] = 0x40;
            //    cMain.CRC_16(WriteBuff, 11, ref CrcLo, ref CrcHi);
            //    WriteBuff[11] = CrcLo;
            //    WriteBuff[12] = CrcHi;
            //    comPort.DiscardOutBuffer();//刷新串口
            //    comPort.Write(WriteBuff, 0, WriteBuff[5] * 2 + 9);
            //    NowTime = DateTime.Now;
            //    do
            //    {
            //        if (comPort.BytesToRead >= 8)//收到数据
            //        {
            //            ReturnByte = comPort.BytesToRead;
            //            IsReturn = true;
            //        }
            //        ts = DateTime.Now - NowTime;
            //        if (ts.TotalMilliseconds > timeOut)//时间超时
            //        {
            //            IsTimeOut = true;
            //        }
            //    } while (!IsReturn && !IsTimeOut);
            //    if (!IsReturn && IsTimeOut)//超时
            //    {
            //        if (ErrStr.IndexOf("写入指令失败,接收数据已超时") < 0)
            //        {
            //            ErrStr = ErrStr + DateTime.Now.ToString() + mName + ":写入指令失败,接收数据已超时" + (char)13 + (char)10;
            //        }
            //        return false;
            //    }
            //    else
            //    {
            //        comPort.Read(ReadBuff, 0, ReturnByte);
            //        if ((ReadBuff[0] != WriteBuff[0]) || (ReadBuff[1] != WriteBuff[1]))//数据检验失败
            //        {
            //            if (ErrStr.IndexOf("写入指令失败,接收数据错误") < 0)
            //            {
            //                ErrStr = ErrStr + DateTime.Now.ToString() + mName + ":写入指令失败,接收数据错误" + (char)13 + (char)10;
            //            }
            //            return false;
            //        }
            //    }
            //}
            //catch (Exception exc)
            //{
            //    if (ErrStr.IndexOf(exc.ToString()) < 0)
            //    {
            //        ErrStr = ErrStr + DateTime.Now.ToString() + mName + ":" + exc.ToString() + (char)13 + (char)10;
            //    }
            //    return false;
            //}

            //return true;
        }

    }
}
