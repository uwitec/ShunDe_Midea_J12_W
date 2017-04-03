using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using NewMideaProgram;
using System.IO.Ports;
namespace System
{
    class cBar
    {
        string errStr = "条码";
        public string ErrStr
        {
            get { return errStr; }
        }
        SerialPort mSerialPort;
        int mTimeOut = 0;
        public cBar(SerialPort _SerialPort, int _TimeOut)
        {
            mSerialPort = _SerialPort;
            mTimeOut = _TimeOut;
        }
        /// <summary>
        /// 读条码
        /// </summary>
        /// <param name="barCode">string,读取到的条码</param>
        /// <returns>bool,是否读取成功</returns>
        public bool readBarCode(ref string barCode)
        {
            if (cMain.isDebug)
            {
                //barCode = "DEF1234567890123";
                return true;
            }
            bool returnValue = false;
            byte[] readBuff;
            try
            {
                int buffLen = mSerialPort.BytesToRead;
                if (buffLen == 0)
                {
                    barCode = "";
                    returnValue = true;
                }
                else
                {
                    bool isReturn = false;
                    bool isTimeOut = false;
                    DateTime NowTime = DateTime.Now;//当前时间
                    TimeSpan ts;
                    do
                    {
                        Thread.Sleep(30);
                        if (buffLen == mSerialPort.BytesToRead)
                        {
                            isReturn = true;
                        }
                        else
                        {
                            buffLen = mSerialPort.BytesToRead;
                        }
                        ts = DateTime.Now - NowTime;
                        if (ts.TotalMilliseconds > mTimeOut)//时间超时
                        {
                            isTimeOut = true;
                        }
                    } while (!isReturn && !isTimeOut);
                    if (!isReturn && isTimeOut)
                    {
                        if (errStr.IndexOf("读取条码数据超时") < 0)
                        {
                            errStr = errStr + DateTime.Now.ToString() + "读取条码数据超时" + (char)13 + (char)10;
                        }
                    }
                    else
                    {
                        readBuff = new byte[buffLen];
                        mSerialPort.Read(readBuff, 0, buffLen);
                        barCode = Encoding.ASCII.GetString(readBuff, 0, readBuff.Length);
                        returnValue = true;
                    }
                }
            }
            catch (Exception exc)
            {
                if (errStr.IndexOf(exc.Message) < 0)
                {
                    errStr = errStr + DateTime.Now.ToString() + exc.Message + (char)13 + (char)10;
                }
            }
            return returnValue;
        }
    }
}
