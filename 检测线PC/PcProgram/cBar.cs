using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using PcProgram;
using System.IO.Ports;
namespace System
{
    class cBar
    {
        string errStr = "����";
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
        /// ������
        /// </summary>
        /// <param name="barCode">string,��ȡ��������</param>
        /// <returns>bool,�Ƿ��ȡ�ɹ�</returns>
        public bool readBarCode(ref string barCode)
        {
            if (cMain.isDebug)
            {
                barCode = "DEF1234567890123";
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
                    DateTime NowTime = DateTime.Now;//��ǰʱ��
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
                        if (ts.TotalMilliseconds > mTimeOut)//ʱ�䳬ʱ
                        {
                            isTimeOut = true;
                        }
                    } while (!isReturn && !isTimeOut);
                    if (!isReturn && isTimeOut)
                    {
                        if (errStr.IndexOf("��ȡ�������ݳ�ʱ") < 0)
                        {
                            errStr = errStr + DateTime.Now.ToString() + "��ȡ�������ݳ�ʱ" + (char)13 + (char)10;
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
