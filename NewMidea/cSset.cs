using System;
using System.Collections.Generic;
using System.Text;
using NewMideaProgram;
using System.IO.Ports;
namespace System
{
    class cSset
    {
        cStandarBoard mStandarBoard;
        public static string mName = "圣斯尔";
        SerialPort comPort;//端口
        /// <summary>
        /// 圣斯尔使用的串口
        /// </summary>
        public SerialPort ComPort
        {
            get { return comPort; }
            set { comPort = value; }
        }
        string errStr = "Sset";//出错信息
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
        byte SsetAddress = 0;//LGPLC地址
        bool mSsetIsInit = false;//PLC初始化结果
        /// <summary>
        /// LGPLC的构造函数
        /// </summary>
        /// <param name="mComPort">LGPLC使用的串口</param>
        /// <param name="mAddress">LGPLC的地址</param>
        public cSset(SerialPort mComPort, byte mAddress)
        {
            comPort = mComPort;
            SsetAddress = mAddress;
            mStandarBoard = new cStandarBoard(comPort, SsetAddress, timeOut);
        }
        public bool SsetInit()
        {
            mStandarBoard.mStandarBoardInit = true;
            double[] d = new double[10];
            mSsetIsInit = SsetRead(ref d);
            mStandarBoard.mStandarBoardInit = mSsetIsInit;
            if (!mSsetIsInit)
            {
                if (errStr.IndexOf(mStandarBoard.ErrStr) < 0)
                {
                    errStr = errStr + mStandarBoard.ErrStr;
                }
            }
            return mSsetIsInit;
        }
        public bool SsetRead(ref double[] mReadBuff)
        {
            bool returnValue = true;
            long[] tempLong = new long[5];
            returnValue = mStandarBoard.StandarBoardRead(0x10, 5, ref tempLong);
            if (returnValue)
            {
                if (tempLong[1] < 10000)//大于10000就大于标称电流
                {
                    mReadBuff[0] = (double)tempLong[0] * 250 / 10000.000;
                    mReadBuff[3] = (double)tempLong[1] * 20 / 10000.000;
                    mReadBuff[6] = (double)tempLong[2] * 20 * 250 / 10000.00;
                }
                else
                {
                    returnValue = false;
                }
            }
            return returnValue;
        }
    }
}
