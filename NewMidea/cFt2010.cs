using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using NewMideaProgram;
namespace System
{
    class cFt2010
    {
        public static string mName = "FT2010\r\n" +
            "cFt2010:构造函数\r\n" +
            "Ft2010Init:初始化2010模块\r\n" +
            "Ft2010Read:2010数据读取";
        cStandarBoard mStandarBoard;
        SerialPort comPort;//端口
        /// <summary>
        /// LGPLC使用的串口
        /// </summary>
        public SerialPort ComPort
        {
            get { return comPort; }
            set { comPort = value; }
        }
        string errStr = "FT2000";//出错信息
        /// <summary>
        /// 错误返回信息
        /// </summary>
        public string ErrStr
        {
            get { return errStr; }
            set { errStr = value; }
        }
        double curBase = 6;//电流互感比
        /// <summary>
        /// 电流互感比
        /// </summary>
        public double CurBase
        {
            get { return curBase; }
            set { curBase = value; }
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
        byte Ft2010Address = 0;//LGPLC地址
        public bool mFt2010Init = false;//PLC初始化结果
        /// <summary>
        /// FT20101构造构造函数
        /// </summary>
        /// <param name="mComPort">操作FT2010的串口</param>
        /// <param name="mAddress">FT2010的地址</param>
        /// <param name="mCurBase">电流互感比,一般为6</param>
        public cFt2010(SerialPort mComPort, byte mAddress, double mCurBase)
        {
            mStandarBoard = new cStandarBoard(mComPort, mAddress, timeOut);
            comPort = mComPort;
            Ft2010Address = mAddress;
            curBase = mCurBase;
        }
        /// <summary>
        /// FT2010初始化
        /// </summary>
        /// <returns>bool,返回初始结果</returns>
        public bool Ft2010Init()
        {
            mFt2010Init = mStandarBoard.StandarBoardInit();
            if (!mFt2010Init)
            {
                if (errStr.IndexOf(mStandarBoard.ErrStr) < 0)
                {
                    errStr = errStr + mStandarBoard.ErrStr;
                }
            }
            return mFt2010Init;
            /*
            if (cMain.isDebug)
            {
                mFt2010Init = true;
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
                WriteBuff[0] = (byte)(Ft2010Address & 0xFF);
                WriteBuff[1] = 0x03;
                WriteBuff[2] = 0x00;
                WriteBuff[3] = 0x00;
                WriteBuff[4] = 0x00;
                WriteBuff[5] = 0x01;
                cMain.CRC_16(WriteBuff, 6, ref CrcLo, ref CrcHi);
                WriteBuff[6] = CrcLo;
                WriteBuff[7] = CrcHi;
                comPort.DiscardOutBuffer();
                comPort.Write(WriteBuff, 0, 8);
                NowTime = DateTime.Now;
                do
                {
                    if (comPort.BytesToRead >= WriteBuff[5] * 2 + 5)//收到数据
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
                        ErrStr = ErrStr + DateTime.Now.ToString() + mName + mName + ":初始失败,接收数据已超时" + (char)13 + (char)10;
                    }

                    return false;
                }
                else
                {
                    comPort.Read(ReadBuff, 0, ReturnByte);
                    if ((ReadBuff[0] != WriteBuff[0]) || (ReadBuff[1] != WriteBuff[1]))//数据检验失败
                    {
                         if (ErrStr.IndexOf("接收数据错误") < 0)
                        {
                            ErrStr = ErrStr + DateTime.Now.ToString() + mName + ":初始失败,接收数据错误" + (char)13 + (char)10;
                        }
                        return false;
                    }
                    else
                    {
                        mFt2010Init = true;
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
             * */
        }
        /// <summary>
        /// 读取2010模块数据
        /// </summary>
        /// <param name="ReturnBuff">返回读取9元素数组,0,1,2为电压,3,4,5为电流,6,7,8为功率</param>
        /// <returns>返回读取数据是否成功</returns>
        public bool Ft2010Read(ref double[] ReturnBuff)
        {
            //if (cMain.isDebug)
            //{
            //    ReturnBuff[0] = 220 + 10 * Num.Rand();
            //    ReturnBuff[1] = 220 + 10 * Num.Rand();
            //    ReturnBuff[2] = 220 + 10 * Num.Rand();
            //    ReturnBuff[3] = 4;// +5 * Num.Rand();
            //    ReturnBuff[4] = 6;// +5 * Num.Rand();
            //    ReturnBuff[5] = 8;// +5 * Num.Rand();
            //    ReturnBuff[6] = ReturnBuff[0] * ReturnBuff[3];
            //    ReturnBuff[7] = ReturnBuff[1] * ReturnBuff[4];
            //    ReturnBuff[8] = ReturnBuff[2] * ReturnBuff[5];
            //    return true;
            //}
            bool returnValue = false;
            long[] mReturnBuff = new long[35];
            if (!mStandarBoard.mStandarBoardInit)
            {
                mStandarBoard.StandarBoardInit();
            }
            float volBase = 0.25f;
            returnValue = mStandarBoard.StandarBoardRead(0x030A, 1, ref mReturnBuff);
            if (returnValue)
            {
                if (mReturnBuff[0] > 0)
                {
                    volBase = 1;
                }
            }
            returnValue = mStandarBoard.StandarBoardRead(0, 35, ref mReturnBuff);
            if (returnValue)
            {
                ReturnBuff[0] = mReturnBuff[0] / (double)100;//电压
                ReturnBuff[1] = mReturnBuff[8] / (double)100;
                ReturnBuff[2] = mReturnBuff[16] / (double)100;
                ReturnBuff[3] = mReturnBuff[2] * curBase / 10000;//电流
                ReturnBuff[4] = mReturnBuff[10] * curBase / 10000;
                ReturnBuff[5] = mReturnBuff[18] * curBase / 10000;
                if (mReturnBuff[4] >= 32768)
                {
                    mReturnBuff[4] = -((mReturnBuff[4] ^ 0xFFFF) + 1);
                }
                if (mReturnBuff[12] >= 32768)
                {
                    mReturnBuff[12] = -((mReturnBuff[12] ^ 0xFFFF) + 1);
                }
                if (mReturnBuff[20] >= 32768)
                {
                    mReturnBuff[20] = -((mReturnBuff[20] ^ 0xFFFF) + 1);
                }
                ReturnBuff[6] = mReturnBuff[04] * curBase * 0.4f * volBase;//功率
                ReturnBuff[7] = mReturnBuff[12] * curBase * 0.4f * volBase;//功率
                ReturnBuff[8] = mReturnBuff[20] * curBase * 0.4f * volBase;//功率

                if (ReturnBuff.Length > 10)
                {
                    //当三相电流都大于0.1A即三相有电流时,功率因素取总功率因素
                    if (Num.DoubleMin(ReturnBuff[3], ReturnBuff[4], ReturnBuff[5]) > 0.1)
                    {
                        ReturnBuff[9] = mReturnBuff[29] / 10000;
                    }
                    else//否则取最大电流的功率因素
                    {
                        ReturnBuff[9] = mReturnBuff[Num.IndexMax(ReturnBuff[3], ReturnBuff[4], ReturnBuff[5]) * 8 + 5] / 10000;
                    }
                }

            }
            else
            {
                if (errStr.IndexOf(mStandarBoard.ErrStr) < 0)
                {
                    errStr = errStr + mStandarBoard.ErrStr;
                }
            }
            return returnValue;
        }

    }
}
