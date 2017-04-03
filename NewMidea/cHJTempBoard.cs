using System;
using System.Collections.Generic;
using System.Text;
using NewMideaProgram;
using System.IO.Ports;
namespace System
{
    class cHJTempBoard
    {
        public static string mName = "侯氏温度板\r\n"+
            "cHJTempBoard:构造函数\r\n"+
            "cHJTempBoardIni:初始化侯氏温度板\r\n"+
            "cHJReadTemp:读取侯氏温度板\r\n"+
            "cHJReadIO:读取侯氏温度板IO输入\r\n"+
            "cHJWriteIO:写入侯氏温度板IO输出";
        SerialPort comPort;//端口
        /// <summary>
        /// 标准MODEBUS使用的串口
        /// </summary>
        public SerialPort ComPort
        {
            get { return comPort; }
            set { comPort = value; }
        }
        string errStr = "HJTempBoard";//出错信息
        /// <summary>
        /// 错误返回信息
        /// </summary>
        public string ErrStr
        {
            get { return errStr; }
            set { errStr = value; }
        }
        int timeOut = 350;//超时时间(ms)
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

        bool mHJBoardInit = false;//PLC初始化结果
        cStandarBoard mStandarBoard;
        public cHJTempBoard(SerialPort mComPort, byte mAddress)
        {
            comPort = mComPort;
            _StandarModebusAddress = mAddress;
            mStandarBoard = new cStandarBoard(comPort, mAddress,timeOut);
        }
        public bool cHJTempBoardIni()
        {
            mHJBoardInit = mStandarBoard.StandarBoardInit();
            if (!mHJBoardInit)
            {
                if (errStr.IndexOf(mStandarBoard.ErrStr) < 0)
                {
                    errStr = errStr + mStandarBoard.ErrStr;
                }
            }
            return mHJBoardInit;
        }
        public bool cHJReadTemp(int readLen, ref double[] mReturnBuff)
        {
            int i = 0;
            bool returnValue = false;
            if (readLen > mReturnBuff.Length || readLen >8)
            {
                readLen = mReturnBuff.Length;
            }
            long[] ReturnBuff = new long[readLen];
            if (cMain.isDebug)
            {
                for (i = 0; i < readLen; i++)
                {
                    mReturnBuff[i] = 20 + i + Num.Rand();
                }
                returnValue = true;
            }
            else
            {
                returnValue = mStandarBoard.StandarBoardRead(0, readLen, ref ReturnBuff);
                if (returnValue)
                {
                    for (i = 0; i < readLen; i++)
                    {
                        mReturnBuff[i] = ReturnBuff[i] / 10.000;
                    }
                }
                else
                {
                    if (errStr.IndexOf(mStandarBoard.ErrStr) < 0)
                    {
                        errStr = errStr + mStandarBoard.ErrStr;

                    }
                }
            }
            return returnValue;
        }
        public bool cHJReadTemp(ref double[] mReturnBuff)
        {
            int i = 0;
            bool returnValue = false;
            long[] ReturnBuff = new long[mReturnBuff.Length];
            if (cMain.isDebug)
            {
                for (i = 0; i < mReturnBuff.Length; i++)
                {
                    mReturnBuff[i] = 10 + i + Num.Rand();
                }
                returnValue= true;
            }
            else
            {
                returnValue = mStandarBoard.StandarBoardRead(0, 8, ref ReturnBuff);
                if (returnValue)
                {
                    for (i = 0; i < mReturnBuff.Length; i++)
                    {
                        if (ReturnBuff[i] >= 32768)
                        {
                            ReturnBuff[i] = -(ReturnBuff[i] - 32768);
                        }
                        mReturnBuff[i] = ReturnBuff[i] / 10.000;
                    }
                }
                else
                {
                    if (errStr.IndexOf(mStandarBoard.ErrStr) < 0)
                    {
                        errStr = errStr + mStandarBoard.ErrStr;
                        
                    }
                }
            }
            return returnValue;
        }
        public bool cHJWriteIO(bool Led1, bool Led2, bool Led3)
        {
            bool returnValue = false;
            byte WriteValue = 0;
            if (Led1)
            {
                WriteValue = (byte)(WriteValue + 1);
            }
            if (Led2)
            {
                WriteValue = (byte)(WriteValue + 2);
            }
            if (Led3)
            {
                WriteValue = (byte)(WriteValue + 4);
            }
            returnValue = mStandarBoard.StandarBoardWritePoint(8, WriteValue,6);
            if (!returnValue)
            {
                if (errStr.IndexOf(mStandarBoard.ErrStr) < 0)
                {
                    errStr = errStr + mStandarBoard.ErrStr;
                }
            }
            return returnValue;
        }
        public bool cHJReadIO(out bool Led1, out bool Led2, out bool Led3)
        {
            Led1 = false;Led2 = false;Led3 = false;
            bool returnValue = false;
            long LedValue=0;
            returnValue = mStandarBoard.StandarBoardRead(9, ref LedValue);
            if (returnValue)
            {
                if ((LedValue & 0x01) == 0x01)
                {
                    Led1 = true;
                }
                else
                {
                    Led1 = false;
                }
                if ((LedValue & 0x02) == 0x02)
                {
                    Led2 = true;
                }
                else
                {
                    Led2 = false;
                }
                if ((LedValue & 0x04) == 0x04)
                {
                    Led3 = true;
                }
                else
                {
                    Led3 = false;
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
