using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using NewMideaProgram;

namespace NewMideaProgram
{
    class cWbGonglv
    {
        public static string mName = "WbGonglv\r\n"+
            "cWbGonglv:构造函数\r\n"+
            "WbGonglvInit:初始化维博模块\r\n"+
            "WbGonglvRead:维博数据读取";
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
        string errStr = "WbGonglv";//出错信息
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
        int timeOut = 400;//超时时间(ms)
        /// <summary>
        ///读写串口超时时间,单位(ms)
        /// </summary>
        public int TimeOut
        {
            get { return timeOut; }
            set { timeOut = value; }
        }
        byte WbGonglvAddress = 0;//LGPLC地址
        public bool mWbGonglvInit = false;//PLC初始化结果
        /// <summary>
        /// WbGonglv1构造构造函数
        /// </summary>
        /// <param name="mComPort">操作WbGonglv的串口</param>
        /// <param name="mAddress">WbGonglv的地址</param>
        /// <param name="mCurBase">电流互感比,一般为6</param>
        public cWbGonglv(SerialPort mComPort, byte mAddress, double mCurBase)
        {
            mStandarBoard = new cStandarBoard(mComPort, mAddress,timeOut);
            comPort = mComPort;
            WbGonglvAddress = mAddress;
            curBase = mCurBase;
        }
        /// <summary>
        /// WbGonglv初始化
        /// </summary>
        /// <returns>bool,返回初始结果</returns>
        public bool WbGonglvInit()
        {
            if (cMain.isDebug)
            {
                mWbGonglvInit = true;
                return true;
            }
            byte[] WriteBuff = new byte[10];//发送数据
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
                WriteBuff[0] = 0x7E;
                WriteBuff[1] = 0x01;
                WriteBuff[2] = 0xFF;
                WriteBuff[3] = 0x50;
                WriteBuff[4] = 0xB0;
                WriteBuff[5] = 0x0D;
         
                comPort.DiscardInBuffer();
                comPort.Write(WriteBuff, 0, 6);
                NowTime = DateTime.Now;
                do
                {
                    //System.Windows.Forms.Application.DoEvents();
                    System.Threading.Thread.Sleep(100);
                    if (comPort.BytesToRead >= 14)//收到数据
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
                        ErrStr = ErrStr + DateTime.Now.ToString() + ",mWbGonglvInit," + "初始失败,接收数据已超时" + (char)13 + (char)10;
                    }
                    cMain.WriteErrorToLog(ReturnByte.ToString());
                    return false;
                }
                else
                {
                    comPort.Read(ReadBuff, 0, ReturnByte);
                    if ((ReadBuff[0] != WriteBuff[0]) || (ReadBuff[1] != WriteBuff[1]))//数据检验失败
                    {
                        if (ErrStr.IndexOf("接收数据错误") < 0)
                        {
                            ErrStr = ErrStr + DateTime.Now.ToString() + ",mWbGonglvInit," + ":初始失败,接收数据错误" + (char)13 + (char)10;
                        }
                        return false;
                    }
                    else
                    {
                        mWbGonglvInit = true;
                    }
                }
            }
            catch (Exception exc)
            {
                if (ErrStr.IndexOf(exc.ToString()) < 0)
                {
                    ErrStr = ErrStr + DateTime.Now.ToString() + ",mWbGonglvInit," + ":" + exc.ToString() + (char)13 + (char)10;
                }
                return false;
            }
            return true;
            
        }
        /// <summary>
        /// 读取WbGonglv模块数据
        /// </summary>
        /// <param name="ReturnBuff">返回读取9元素数组,0,1,2为电压,3,4,5为电流,6,7,8为功率</param>
        /// <returns>返回读取数据是否成功</returns>
        public bool WbGonglvRead(ref double[] ReturnBuff)
        {
            if (cMain.isDebug)
            {
                ReturnBuff[0] = 220 + 10 * Num.Rand();
                ReturnBuff[1] = 220 + 10 * Num.Rand();
                ReturnBuff[2] = 220 + 10 * Num.Rand();
                ReturnBuff[3] = 4;// +5 * Num.Rand();
                ReturnBuff[4] = 6;// +5 * Num.Rand();
                ReturnBuff[5] = 8;// +5 * Num.Rand();
                ReturnBuff[6] = ReturnBuff[0] * ReturnBuff[3];
                ReturnBuff[7] = ReturnBuff[1] * ReturnBuff[4];
                ReturnBuff[8] = ReturnBuff[2] * ReturnBuff[5];
                return true;
            }
            bool returnValue = false;
            long[] mReturnBuff=new long[35];
            
            int i;
            long[] ReturnBuff1 = new long[9];
            byte[] WriteBuff = new byte[10];//发送数据
            byte[] ReadBuff = new byte[16];//接收数据
            int ReturnByte = 0;//返回数据
            bool IsReturn = false;//是否成功返回
            bool IsTimeOut = false;//是否超时
            DateTime NowTime;//当前时间
            TimeSpan ts;//时间差
            

            
            try
            {
                //05 03	24 10 00 06 CF 79
                WriteBuff[0] = 0x7E;
                WriteBuff[1] = 0x01;
                WriteBuff[2] = 0xFF;
                WriteBuff[3] = 0x50;
                WriteBuff[4] = 0xB0;
                WriteBuff[5] = 0x0D;
                if (!comPort.IsOpen)
                {
                    comPort.Open();
                }
                comPort.DiscardInBuffer();//刷新串口
                comPort.Write(WriteBuff, 0, 6);
                NowTime = DateTime.Now;
                do
                {
                    //System.Windows.Forms.Application.DoEvents();
                    System.Threading.Thread.Sleep(100);

                    if (comPort.BytesToRead >= 14)//收到数据
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
                        ErrStr = ErrStr + DateTime.Now.ToString() + ",WbGonglvRead," + ":读取失败,接收数据已超时" + (char)13 + (char)10;
                    }
                    returnValue = false;
                    return returnValue;
                }
                ReadBuff = new byte[ReturnByte];
                comPort.Read(ReadBuff, 0, ReturnByte);
                if ((ReadBuff[0] != WriteBuff[0]) || (ReadBuff[1] != WriteBuff[1]))//数据检验失败
                {
                    //string tempStr = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{3},{14},{15},{16},{17},{18},{19},{20}",
                    //    ReadBuff[0], ReadBuff[1], ReadBuff[2], ReadBuff[3], ReadBuff[4], ReadBuff[5], ReadBuff[6], ReadBuff[7], ReadBuff[8],
                    //    ReadBuff[9], ReadBuff[10], ReadBuff[11], ReadBuff[12], ReadBuff[13], ReadBuff[14], ReadBuff[15], ReadBuff[16], ReadBuff[17]
                    //    , ReadBuff[18], ReadBuff[19], ReadBuff[20]);
                    //cMain.WriteErrorToLog(tempStr);
                    if (ErrStr.IndexOf("接收数据错误") < 0)
                    {
                        ErrStr = ErrStr + DateTime.Now.ToString() + ",WbGonglvRead," + ":读取失败,接收数据错误" + (char)13 + (char)10;
                    }
                    returnValue = false;
                    return returnValue;
                }
                else
                {
                    int j;
                    for (i = 1; i < ReturnByte - 1; i++)
                    {
                        if (ReadBuff[i] == 5)
                        {
                            ReadBuff[i] = (byte)((ReadBuff[i]) + (ReadBuff[i + 1]));
                            for (j = i + 1; j < ReturnByte - 1; j++)
                            {
                                ReadBuff[j] = ReadBuff[j + 1];
                            }
                        }

                    }
                    for (i = 0; i < ReturnBuff1.Length; i++)
                    {
                        ReturnBuff1[i] = (ReadBuff[i * 2 + 7] & 0xFF) * 256 + (ReadBuff[i * 2 + 6] & 0xFF);
                        if ((ReadBuff[i * 2 + 7] >> 7) == 1)//如果是负数
                        {
                            ReturnBuff1[i] = -(ReturnBuff1[i] & 0xFFFF + 1);
                        }
                        mReturnBuff[i] = ReturnBuff1[i];
                    }
                    //for (i = 0; i < ReturnBuff1.Length; i++)
                    //{
                    //    ReturnBuff1[i] = (ReadBuff[i * 2 + 7] & 0xFF) * 256 + (ReadBuff[i * 2 + 6] & 0xFF);
                    //    if ((ReadBuff[i * 2 + 7] >> 7) == 1)//如果是负数
                    //    {
                    //        ReturnBuff1[i] = -(ReturnBuff1[i] & 0xFFFF + 1);
                    //    }
                    //    mReturnBuff[i] = ReturnBuff1[i];
                    //}
                }
            }
            catch (Exception exc)
            {
                if (ErrStr.IndexOf(exc.ToString()) < 0)
                {
                    ErrStr = ErrStr + DateTime.Now.ToString() + ",WbGonglvRead," + ":" + exc.ToString() + (char)13 + (char)10;
                }
                returnValue = false;
                return returnValue;
            }
            returnValue = true;


            if (returnValue)
            {
                ReturnBuff[0] = mReturnBuff[2] * 300 / (double)10000;//电压
                ReturnBuff[1] = mReturnBuff[3] * 300 / (double)10000;//电压
                ReturnBuff[2] = mReturnBuff[4] * 300 / (double)10000;//电压
                ReturnBuff[3] = mReturnBuff[5] * 30 / (double)10000;//电流
                ReturnBuff[4] = mReturnBuff[6] * 30 / (double)10000;//电压
                ReturnBuff[5] = mReturnBuff[7] * 30 / (double)10000;//电压

                ReturnBuff[6] = mReturnBuff[0] * 9000 *3/ (double)10000;//功率


                //ReturnBuff[7] = mReturnBuff[9] * 9000 *3 / (double)10000;//功率


                //ReturnBuff[8] = mReturnBuff[10] * 9000 *3 / (double)10000;//功率
                

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
        /// <summary>
        /// 读取2010模块数据
        /// </summary>
        /// <param name="ReturnBuff">返回读取9元素数组,0,1,2为电压,3,4,5为电流,6,7,8为功率</param>
        /// <returns>返回读取数据是否成功</returns>
        public bool WbGonglvRead_R(ref double[] ReturnBuff)
        {
            if (cMain.isDebug)
            {
                ReturnBuff[0] = 220 + 10 * Num.Rand();
                ReturnBuff[1] = 220 + 10 * Num.Rand();
                ReturnBuff[2] = 220 + 10 * Num.Rand();
                ReturnBuff[3] = 4;// +5 * Num.Rand();
                ReturnBuff[4] = 6;// +5 * Num.Rand();
                ReturnBuff[5] = 8;// +5 * Num.Rand();
                ReturnBuff[6] = ReturnBuff[0] * ReturnBuff[3];
                ReturnBuff[7] = ReturnBuff[1] * ReturnBuff[4];
                ReturnBuff[8] = ReturnBuff[2] * ReturnBuff[5];
                return true;
            }
            bool returnValue = false;
            long[] mReturnBuff = new long[35];

            int i;
            long[] ReturnBuff1 = new long[4];
            byte[] WriteBuff = new byte[10];//发送数据
            byte[] ReadBuff = new byte[16];//接收数据
            int ReturnByte = 0;//返回数据
            bool IsReturn = false;//是否成功返回
            bool IsTimeOut = false;//是否超时
            DateTime NowTime;//当前时间
            TimeSpan ts;//时间差
            


            try
            {
                //05 03	24 10 00 06 CF 79
                WriteBuff[0] = 0x7E;
                WriteBuff[1] = 0x01;
                WriteBuff[2] = 0xFF;
                WriteBuff[3] = 0x50;
                WriteBuff[4] = 0xB0;
                WriteBuff[5] = 0x0D;
                
                NowTime = DateTime.Now;
                do
                {
                    //System.Windows.Forms.Application.DoEvents();
                    //Threading.Thread.Sleep(20);
                    if (comPort.BytesToRead >= 14)//收到数据
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
                        ErrStr = ErrStr + DateTime.Now.ToString() + ",WbGonglvRead," + ":读取失败,接收数据已超时" + (char)13 + (char)10;
                    }
                    returnValue = false;
                    return returnValue;
                }
                comPort.Read(ReadBuff, 0, ReturnByte);
                if ((ReadBuff[0] != WriteBuff[0]) || (ReadBuff[1] != WriteBuff[1]))//数据检验失败
                {
                    //string tempStr = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{3},{14},{15},{16},{17},{18},{19},{20}",
                    //    ReadBuff[0], ReadBuff[1], ReadBuff[2], ReadBuff[3], ReadBuff[4], ReadBuff[5], ReadBuff[6], ReadBuff[7], ReadBuff[8],
                    //    ReadBuff[9], ReadBuff[10], ReadBuff[11], ReadBuff[12], ReadBuff[13], ReadBuff[14], ReadBuff[15], ReadBuff[16], ReadBuff[17]
                    //    , ReadBuff[18], ReadBuff[19], ReadBuff[20]);
                    //cMain.WriteErrorToLog(tempStr);
                    if (ErrStr.IndexOf("接收数据错误") < 0)
                    {
                        ErrStr = ErrStr + DateTime.Now.ToString() + ",WbGonglvRead," + ":读取失败,接收数据错误" + (char)13 + (char)10;
                    }
                    returnValue = false;
                    return returnValue;
                }
                else
                {
                    for (i = 0; i < ReturnBuff1.Length; i++)
                    {
                        ReturnBuff1[i] = (ReadBuff[i * 2 + 7] & 0xFF) * 256 + (ReadBuff[i * 2 + 6] & 0xFF);
                        //if ((ReadBuff[i * 2 + 4] >> 7) == 1)//如果是负数
                        //{
                        //    ReturnBuff1[i] = -(ReturnBuff1[i] & 0xFFFF + 1);
                        //}
                        mReturnBuff[i] = ReturnBuff1[i];
                    }
                }
            }
            catch (Exception exc)
            {
                if (ErrStr.IndexOf(exc.ToString()) < 0)
                {
                    ErrStr = ErrStr + DateTime.Now.ToString() + ",WbGonglvRead," + ":" + exc.ToString() + (char)13 + (char)10;
                }
                returnValue = false;
                return returnValue;
            }
            returnValue = true;


            if (returnValue)
            {
                ReturnBuff[0] = mReturnBuff[2] * 300 / (double)10000;//电压
                ReturnBuff[1] = 0;
                ReturnBuff[2] = 0;
                ReturnBuff[3] = mReturnBuff[3] * 25 / (double)10000;//电流
                ReturnBuff[4] = 0;
                ReturnBuff[5] = 0;

                ReturnBuff[6] = mReturnBuff[0] * 7500 / (double)10000;//功率


                ReturnBuff[7] = 0;


                ReturnBuff[8] = 0;


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
        /// <summary>
        /// 读取2010模块数据
        /// </summary>
        /// <param name="ReturnBuff">返回读取9元素数组,0,1,2为电压,3,4,5为电流,6,7,8为功率</param>
        /// <returns>返回读取数据是否成功</returns>
        public bool WbGonglvRead_W(ref double[] ReturnBuff)
        {
            if (cMain.isDebug)
            {
                ReturnBuff[0] = 220 + 10 * Num.Rand();
                ReturnBuff[1] = 220 + 10 * Num.Rand();
                ReturnBuff[2] = 220 + 10 * Num.Rand();
                ReturnBuff[3] = 4;// +5 * Num.Rand();
                ReturnBuff[4] = 6;// +5 * Num.Rand();
                ReturnBuff[5] = 8;// +5 * Num.Rand();
                ReturnBuff[6] = ReturnBuff[0] * ReturnBuff[3];
                ReturnBuff[7] = ReturnBuff[1] * ReturnBuff[4];
                ReturnBuff[8] = ReturnBuff[2] * ReturnBuff[5];
                return true;
            }
            bool returnValue = false;
            long[] mReturnBuff = new long[35];

            long[] ReturnBuff1 = new long[5];
            byte[] WriteBuff = new byte[10];//发送数据
            byte[] ReadBuff = new byte[16];//接收数据

            try
            {
                //05 03	24 10 00 06 CF 79
                WriteBuff[0] = 0x7E;
                WriteBuff[1] = 0x01;
                WriteBuff[2] = 0xFF;
                WriteBuff[3] = 0x50;
                WriteBuff[4] = 0xB0;
                WriteBuff[5] = 0x0D;
                if (!comPort.IsOpen)
                {
                    comPort.Open();
                }
                comPort.DiscardInBuffer();//刷新串口
                comPort.Write(WriteBuff, 0, 6);
                
            }
            catch (Exception exc)
            {
                if (ErrStr.IndexOf(exc.ToString()) < 0)
                {
                    ErrStr = ErrStr + DateTime.Now.ToString() + ",WbGonglvRead," + ":" + exc.ToString() + (char)13 + (char)10;
                }
                returnValue = false;
                return returnValue;
            }
            returnValue = true;

            return returnValue;
        }
    }
}
