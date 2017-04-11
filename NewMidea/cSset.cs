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
        public enum ListVol
        {
            /// <summary>
            /// 单相模块
            /// </summary>
            DanXiang,
            /// <summary>
            /// 三相模块
            /// </summary>
            SanXiang
        }
        ListVol listVold = ListVol.DanXiang;
        byte SsetAddress = 0;//LGPLC地址
        int mVol, mCur;
        bool mSsetIsInit = false;//PLC初始化结果
        /// <summary>
        /// LGPLC的构造函数
        /// </summary>
        /// <param name="mComPort">LGPLC使用的串口</param>
        /// <param name="mAddress">LGPLC的地址</param>
        /// <param name="isDangXiang">是否单相</param>
        public cSset(SerialPort mComPort, byte mAddress, int Vol, int Cur, ListVol isDangXiang)
        {
            comPort = mComPort;
            SsetAddress = mAddress;
            mVol = Vol;
            mCur = Cur;
            listVold = isDangXiang;
            mStandarBoard = new cStandarBoard(comPort, SsetAddress, timeOut);
        }
        public bool SsetInit()
        {
            mStandarBoard.mStandarBoardInit = true;
            mSsetIsInit = true;
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
            if (!mSsetIsInit)
            {
                SsetInit();
                return false;
            }
            bool returnValue = true;
            if (listVold == ListVol.DanXiang)
            {
                long[] tempLong = new long[5];
                returnValue = mStandarBoard.StandarBoardRead(0x10, 5, ref tempLong);
                if (returnValue)
                {
                    mReadBuff[0] = (double)tempLong[0] * mVol / 10000f;
                    mReadBuff[3] = (double)tempLong[1] * mCur / 10000f;
                    mReadBuff[6] = (double)tempLong[2] * mCur * mVol / 10000f;
                }
            }
            if (listVold == ListVol.SanXiang)
            {
                long[] tempLong = new long[17];
                returnValue = mStandarBoard.StandarBoardRead(0x10, 17, ref tempLong);
                if (returnValue)
                {
                    mReadBuff[0] = (double)tempLong[0] * mVol / 10000f;
                    mReadBuff[1] = (double)tempLong[2] * mVol / 10000f;
                    mReadBuff[2] = (double)tempLong[4] * mVol / 10000f;

                    mReadBuff[3] = (double)tempLong[1] * mCur / 10000f;
                    mReadBuff[4] = (double)tempLong[3] * mCur / 10000f;
                    mReadBuff[5] = (double)tempLong[5] * mCur / 10000f;

                    if (tempLong[14] < 32768)
                    {
                        mReadBuff[6] = (double)tempLong[14] * mCur * mVol / 10000f;
                    }
                    if (tempLong[15] < 32768)
                    {
                        mReadBuff[7] = (double)tempLong[15] * mCur * mVol / 10000f;
                    }
                    if (tempLong[16] < 32768)
                    {
                        mReadBuff[8] = (double)tempLong[16] * mCur * mVol / 10000f;
                    }
                }
            }
            return returnValue;
        }
    }
}
