using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO.Ports;
namespace PcProgram
{
    public class cAiNuo9641B
    {
        const string mName = "AiNuo9641B模块";
        SerialPort comPort;//端口
        /// <summary>
        /// AiNuo9641B使用的串口
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
        int timeOut = 1000;//超时时间(ms)
        /// <summary>
        ///读写串口超时时间,单位(ms)
        /// </summary>
        public int TimeOut
        {
            get { return timeOut; }
            set { timeOut = value; }
        }
        byte[] _AiNuo9641BAddress =new byte[3];//AiNuo9641B地址
        bool mAiNuo9641Init = false;//AiNuo9641B初始化结果
        #region//读取参数
        public enum TestNameEnum:int
        {
            JieDi,JueYuan,NaiYa,XieLou,None
        }
        public enum TestStepResultEnum:int
        {
            None,OK,NG1,PT,NG2
        }
        public enum TestStatueEnum:int
        {
            Before=0,Run=1,Over=2,None=7
        }
        public struct AiNuo9641StepData
        {
            public TestNameEnum mTestName;
            public double mTestData;
            public TestStepResultEnum mTestResult;
        }
        public struct AiNuo9641Data
        {
            public AiNuo9641StepData[] mAiNuo9641Data;
            public TestStatueEnum mAiNuoStatue; 
        }
        AiNuo9641Data aiNuo9641Data = new AiNuo9641Data();
        Regex NumRex = new Regex("^[0-9]*$");
        #endregion
#region//设置参数
        public struct SetStepData
        {
            /// <summary>
            /// 项目名,接地,绝缘,耐压,泄漏
            /// </summary>
            public TestNameEnum mTestName;
            /// <summary>
            /// 参数1
            /// </summary>
            public int Data;
            /// <summary>
            /// 参数2,下限
            /// </summary>
            public double DataLo;
            /// <summary>
            /// 参数3,上限
            /// </summary>
            public double DataHi;
            /// <summary>
            /// 时间
            /// </summary>
            public int mTime;
            /// <summary>
            /// 条件,true=动,false=静
            /// </summary>
            public bool isActive;
        }
        public struct SetData
        {
            public SetStepData[] mSetStepData;
            public bool isGoOn;
            public int stepIndex;
        }
#endregion
        public cAiNuo9641B(SerialPort mSerialPort,byte mAddress )
        {
            comPort = mSerialPort;
            _AiNuo9641BAddress = Encoding.ASCII.GetBytes(string.Format("{0:D3}", mAddress));
            aiNuo9641Data.mAiNuo9641Data = new AiNuo9641StepData[7];
        }
        public bool AiNuo9641Stop()
        {
            bool returnValue = false;
            if (!mAiNuo9641Init)
            {
                AiNuo9641Init();
                return false;
            }
            byte[] WriteBuff = new byte[9];//发送数据
            byte[] ReadBuff = new byte[50];//接收数据
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
                WriteBuff[0] = 0x7B;
                WriteBuff[1] = _AiNuo9641BAddress[0];
                WriteBuff[2] = _AiNuo9641BAddress[1];
                WriteBuff[3] = _AiNuo9641BAddress[2];
                WriteBuff[4] = 0x32;
                WriteBuff[5] = 0x30;
                WriteBuff[6] = 0x30;
                WriteBuff[7] = 0x7D;

                comPort.DiscardInBuffer();
                comPort.Write(WriteBuff, 0, 8);
                NowTime = DateTime.Now;
                do
                {
                    if (comPort.BytesToRead >=9)//收到数据
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
                    if (ErrStr.IndexOf("停止失败,接收数据已超时") < 0)
                    {
                        ErrStr = ErrStr + DateTime.Now.ToString() + mName + ":停止失败,接收数据已超时" + (char)13 + (char)10;
                    }
                    return false;
                }
                else
                {
                    comPort.Read(ReadBuff, 0, ReturnByte);
                    if ((ReadBuff[0] != WriteBuff[0]) || (ReadBuff[1] != WriteBuff[1]) || (ReadBuff[2] != WriteBuff[2]))//数据检验失败
                    {
                        //comPort.Close(); 
                        if (ErrStr.IndexOf("停止失败,接收数据错误") < 0)
                        {
                            ErrStr = ErrStr + DateTime.Now.ToString() + mName + ":停止失败,接收数据错误" + (char)13 + (char)10;
                        }
                        return false;
                    }
                }
                returnValue = true;
            }
            catch (Exception exc)
            {
                if (ErrStr.IndexOf(exc.ToString()) < 0)
                {
                    ErrStr = ErrStr + DateTime.Now.ToString() + mName + ":" + exc.ToString() + (char)13 + (char)10;
                }
            }
            return returnValue;
        }
        public bool AiNuo9641Init()
        {
            mAiNuo9641Init = true;
            mAiNuo9641Init = AiNuo9641Stop();
            return mAiNuo9641Init;
        }
        public bool AiNuo9641Set(SetData sSetData)
        {
            bool returnValue = false;
            if (!mAiNuo9641Init)
            {
                AiNuo9641Init();
                return false;
            }
            byte[] WriteBuff = new byte[209];//发送数据
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
                WriteBuff[0] = 0x7B;
                WriteBuff[1] = _AiNuo9641BAddress[0];
                WriteBuff[2] = _AiNuo9641BAddress[1];
                WriteBuff[3] = _AiNuo9641BAddress[2];
                WriteBuff[4] = 0x37;

                for (int i = 0; i < 8; i++)
                {
                    switch (sSetData.mSetStepData[i].mTestName)
                    {
                        case TestNameEnum.JieDi:
                            WriteBuff[5 + 18 * i] = 0x31;
                            sSetData.mSetStepData[i].DataHi = (int)sSetData.mSetStepData[i].DataHi;
                            sSetData.mSetStepData[i].DataLo = (int)sSetData.mSetStepData[i].DataLo;
                            WriteBuff[22 + 18 * i] = 0x30;
                            break;
                        case TestNameEnum.JueYuan:
                            WriteBuff[5 + 18 * i] = 0x32;
                            sSetData.mSetStepData[i].DataHi = (int)(sSetData.mSetStepData[i].DataHi * 10);
                            sSetData.mSetStepData[i].DataLo = (int)(sSetData.mSetStepData[i].DataLo * 10); 
                            WriteBuff[22 + 18 * i] = 0x30;
                            break;
                        case TestNameEnum.NaiYa:
                            WriteBuff[5 + 18 * i] = 0x33;
                            sSetData.mSetStepData[i].DataHi = (int)(sSetData.mSetStepData[i].DataHi * 100);
                            sSetData.mSetStepData[i].DataLo = (int)(sSetData.mSetStepData[i].DataLo * 100);
                            if (sSetData.mSetStepData[i].isActive)
                            { WriteBuff[22 + 18 * i] = 0x31; }
                            else 
                            { WriteBuff[22 + 18 * i] = 0x30; }
                            break;
                        case TestNameEnum.XieLou:
                            WriteBuff[5 + 18 * i] = 0x34;
                            sSetData.mSetStepData[i].DataHi = (int)(sSetData.mSetStepData[i].DataHi * 1000);
                            sSetData.mSetStepData[i].DataLo = (int)(sSetData.mSetStepData[i].DataLo * 1000);
                            if (sSetData.mSetStepData[i].isActive)
                            { WriteBuff[22 + 18 * i] = 0x31; }
                            else 
                            { WriteBuff[22 + 18 * i] = 0x30; }
                            break;
                        case TestNameEnum.None:
                        default:
                            WriteBuff[5 + 18 * i] = 0x30; 
                            WriteBuff[22 + 18 * i] = 0x30;
                            break;
                    }
                    sSetData.mSetStepData[i].mTime = (int)sSetData.mSetStepData[i].mTime * 10;

                    string[] tempStr=new string[4];
                    tempStr[0] = string.Format("{0:D4}", (int)sSetData.mSetStepData[i].Data);
                    tempStr[1] = string.Format("{0:D4}", (int)sSetData.mSetStepData[i].DataLo);
                    tempStr[2] = string.Format("{0:D4}", (int)sSetData.mSetStepData[i].DataHi);
                    tempStr[3] = string.Format("{0:D4}", (int)sSetData.mSetStepData[i].mTime);
                    byte[] tempByt0 = Encoding.ASCII.GetBytes(tempStr[0]);
                    byte[] tempByt1 = Encoding.ASCII.GetBytes(tempStr[1]);
                    byte[] tempByt2 = Encoding.ASCII.GetBytes(tempStr[2]);
                    byte[] tempByt3 = Encoding.ASCII.GetBytes(tempStr[3]);
                    if (tempByt2.Length > 4)
                    {
                        tempByt2 = Encoding.ASCII.GetBytes(AiNuoData((int)sSetData.mSetStepData[i].DataHi));
                    }
                    for (int j = 0; j < 4; j++)
                    {
                        WriteBuff[6 + 18 * i + j] = tempByt0[j];
                        WriteBuff[10 + 18 * i + j] = tempByt1[j];
                        WriteBuff[14 + 18 * i + j] = tempByt2[j];
                        WriteBuff[18 + 18 * i + j] = tempByt3[j];
                    }
                }
                if (sSetData.isGoOn)
                { WriteBuff[149] = 0x31; }
                else
                { WriteBuff[149] = 0x30; }
                for (int i = 150; i < 205; i++)
                {
                    WriteBuff[i] = 0x30;
                }
                WriteBuff[205] = (byte)(0x30 + sSetData.stepIndex);
                WriteBuff[206] = 0x30;
                WriteBuff[207] = 0x30;
                WriteBuff[208] = 0x7D;
                comPort.DiscardInBuffer();
                comPort.Write(WriteBuff, 0, 209);
                string tepm = Encoding.ASCII.GetString(WriteBuff);
                NowTime = DateTime.Now;
                do
                {
                    if (comPort.BytesToRead >=9)//收到数据
                    {
                        ReturnByte = comPort.BytesToRead;
                        IsReturn = true;
                    }
                    ts = DateTime.Now - NowTime;
                    if (ts.TotalMilliseconds > 1500)//时间超时
                    {
                        IsTimeOut = true;
                    }
                } while (!IsReturn && !IsTimeOut);
                if (!IsReturn && IsTimeOut)//超时
                {
                    if (ErrStr.IndexOf("设置失败,接收数据已超时") < 0)
                    {
                        ErrStr = ErrStr + DateTime.Now.ToString() + mName + ":设置失败,接收数据已超时" + (char)13 + (char)10;
                    }
                    return false;
                }
                else
                {
                    comPort.Read(ReadBuff, 0, ReturnByte);
                    if ((ReadBuff[0] != WriteBuff[0]) || (ReadBuff[1] != WriteBuff[1]) || (ReadBuff[2] != WriteBuff[2]))//数据检验失败
                    {
                        //comPort.Close(); 
                        if (ErrStr.IndexOf("设置失败,接收数据错误") < 0)
                        {
                            ErrStr = ErrStr + DateTime.Now.ToString() + mName + ":设置失败,接收数据错误" + (char)13 + (char)10;
                        }
                        return false;
                    }
                    else
                    {
                        if (ReadBuff[5] == 0x32)
                        {
                            if (ErrStr.IndexOf("设置失败,安规处于设置状态,或未知错误") < 0)
                            {
                                ErrStr = ErrStr + DateTime.Now.ToString() + mName + ":设置失败,安规处于设置状态,或未知错误" + (char)13 + (char)10;
                            }
                            return false;
                        }
                    }
                }
                returnValue = true;
            }
            catch (Exception exc)
            {
                if (ErrStr.IndexOf(exc.ToString()) < 0)
                {
                    ErrStr = ErrStr + DateTime.Now.ToString() + mName + ":" + exc.ToString() + (char)13 + (char)10;
                }
            }
            return returnValue;
        }
        public string AiNuoData(int data)
        {
            string returnData = "";

            byte a = (byte)(data / 1000);
            byte b = (byte)((data % 1000) / 100);
            byte c = (byte)((data % 100) / 10);
            byte d = (byte)((data % 10));

            byte[] bt = new byte[4];
            bt[0] = (byte)(0x30 + a);
            bt[1] = (byte)(0x30 + b);
            bt[2] = (byte)(0x30 + c);
            bt[3] = (byte)(0x30 + d);
            returnData = Encoding.ASCII.GetString(bt);
            return returnData;
        }
        public bool AiNuo9641Read(out AiNuo9641Data _aiNuo9641Data)
        {
            aiNuo9641Data.mAiNuoStatue = TestStatueEnum.None;
            for (int i = 0; i < aiNuo9641Data.mAiNuo9641Data.Length; i++)
            {
                aiNuo9641Data.mAiNuo9641Data[i].mTestData = 0;
                aiNuo9641Data.mAiNuo9641Data[i].mTestName = TestNameEnum.None;
                aiNuo9641Data.mAiNuo9641Data[i].mTestResult = TestStepResultEnum.None;
            }
            bool returnValue = false;
            if (!mAiNuo9641Init)
            {
                AiNuo9641Init();
                _aiNuo9641Data = aiNuo9641Data;
                return false;
            }
            byte[] WriteBuff = new byte[9];//发送数据
            byte[] ReadBuff = new byte[400];//接收数据
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
                WriteBuff[0] = 0x7B;
                WriteBuff[1] = _AiNuo9641BAddress[0];
                WriteBuff[2] = _AiNuo9641BAddress[1];
                WriteBuff[3] = _AiNuo9641BAddress[2];
                WriteBuff[4] = 0x30;
                WriteBuff[5] = 0x30;
                WriteBuff[6] = 0x30;
                WriteBuff[7] = 0x7D;

                comPort.DiscardInBuffer();
                comPort.Write(WriteBuff, 0, 8);
                NowTime = DateTime.Now;
                do
                {
                    if (comPort.BytesToRead >= 193)//收到数据
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
                    if (ErrStr.IndexOf("读数失败,接收数据已超时") < 0)
                    {
                        ErrStr = ErrStr + DateTime.Now.ToString() + mName + ":读数失败,接收数据已超时" + (char)13 + (char)10;
                    }
                    returnValue= false;
                }
                else
                {
                    comPort.Read(ReadBuff, 0, ReturnByte);
                    if ((ReadBuff[0] != WriteBuff[0]) || (ReadBuff[1] != WriteBuff[1]) || (ReadBuff[2] != WriteBuff[2]))//数据检验失败
                    {
                        //comPort.Close(); 
                        if (ErrStr.IndexOf("读数失败,接收数据错误") < 0)
                        {
                            ErrStr = ErrStr + DateTime.Now.ToString() + mName + ":读数失败,接收数据错误" + (char)13 + (char)10;
                        }
                        returnValue =false;
                    }
                    else
                    {
                        switch (ReadBuff[189])
                        {
                            case 0x30:
                                aiNuo9641Data.mAiNuoStatue = TestStatueEnum.Before;
                                break;
                            case 0x31:
                                aiNuo9641Data.mAiNuoStatue = TestStatueEnum.Run;
                                break;
                            case 0x32:
                                aiNuo9641Data.mAiNuoStatue = TestStatueEnum.Over;
                                break;
                            case 0x37:
                                aiNuo9641Data.mAiNuoStatue = TestStatueEnum.None;
                                break;
                            default:
                                aiNuo9641Data.mAiNuoStatue = TestStatueEnum.None;
                                break;
                        }
                        if (aiNuo9641Data.mAiNuoStatue==TestStatueEnum.Over)
                        {
                            for (int i = 0; i < 7; i++)
                            {
                                int DataStart = 6;
                                switch (ReadBuff[4 + i * 23 + 1])
                                {
                                    case 0x31:
                                        aiNuo9641Data.mAiNuo9641Data[i].mTestName = TestNameEnum.JieDi;
                                        break;
                                    case 0x32:
                                        aiNuo9641Data.mAiNuo9641Data[i].mTestName = TestNameEnum.JueYuan;
                                        break;
                                    case 0x33:
                                        aiNuo9641Data.mAiNuo9641Data[i].mTestName = TestNameEnum.NaiYa;
                                        break;
                                    case 0x34:
                                        aiNuo9641Data.mAiNuo9641Data[i].mTestName = TestNameEnum.XieLou;
                                        break;
                                    case 0:
                                    default:
                                        aiNuo9641Data.mAiNuo9641Data[i].mTestName = TestNameEnum.None ;
                                        break;
                                }
                                switch (ReadBuff[4 + i * 23 + 23])
                                {
                                    case 0x30:
                                        aiNuo9641Data.mAiNuo9641Data[i].mTestResult = TestStepResultEnum.None;
                                        break;
                                    case 0x31:
                                        aiNuo9641Data.mAiNuo9641Data[i].mTestResult = TestStepResultEnum.OK;
                                        break;
                                    case 0x32:
                                        aiNuo9641Data.mAiNuo9641Data[i].mTestResult = TestStepResultEnum.NG1;
                                        break;
                                    case 0x33:
                                        aiNuo9641Data.mAiNuo9641Data[i].mTestResult = TestStepResultEnum.PT;
                                        break;
                                    case 0x34:
                                        aiNuo9641Data.mAiNuo9641Data[i].mTestResult = TestStepResultEnum.NG2;
                                        break;
                                    default :
                                        aiNuo9641Data.mAiNuo9641Data[i].mTestResult = TestStepResultEnum.None;
                                        break;
                                }
                                byte[] tmpByte=new byte[4];
                                string tmpStr = "";
                                for (int j = 0; j < 4; j++)
                                {
                                    tmpByte[j] = ReadBuff[4 + i * 23 + DataStart+j];
                                }
                                tmpStr = Encoding.ASCII.GetString(tmpByte).Trim();
                                if (NumRex.IsMatch(tmpStr))
                                {
                                    aiNuo9641Data.mAiNuo9641Data[i].mTestData = Num.DoubleParse(tmpStr);
                                    switch (aiNuo9641Data.mAiNuo9641Data[i].mTestName)
                                    {
                                        case TestNameEnum.JieDi:
                                        case TestNameEnum.JueYuan:
                                            aiNuo9641Data.mAiNuo9641Data[i].mTestData /= 10;
                                            break;
                                        case TestNameEnum.NaiYa:
                                            aiNuo9641Data.mAiNuo9641Data[i].mTestData /= 100;
                                            break;
                                        case TestNameEnum.XieLou:
                                            aiNuo9641Data.mAiNuo9641Data[i].mTestData /= 1000;
                                            break;
                                        default:
                                            aiNuo9641Data.mAiNuo9641Data[i].mTestData /= 10;
                                            break;
                                    }
                                }
                                else
                                {
                                    aiNuo9641Data.mAiNuo9641Data[i].mTestData = 9999;
                                }
                            }
                        }
                        returnValue = true;
                    }
                }
            }
            catch (Exception exc)
            {
                if (ErrStr.IndexOf(exc.ToString()) < 0)
                {
                    ErrStr = ErrStr + DateTime.Now.ToString() + mName + ":" + exc.ToString() + (char)13 + (char)10;
                }
            }
            _aiNuo9641Data = aiNuo9641Data;
            return returnValue;
        }
        public bool AiNuo9641Start()
        {
            bool returnValue = false;
            if (!mAiNuo9641Init)
            {
                AiNuo9641Init();
                return false;
            }
            byte[] WriteBuff = new byte[9];//发送数据
            byte[] ReadBuff = new byte[50];//接收数据
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
                WriteBuff[0] = 0x7B;
                WriteBuff[1] = _AiNuo9641BAddress[0];
                WriteBuff[2] = _AiNuo9641BAddress[1];
                WriteBuff[3] = _AiNuo9641BAddress[2];
                WriteBuff[4] = 0x31;
                WriteBuff[5] = 0x30;
                WriteBuff[6] = 0x30;
                WriteBuff[7] = 0x7D;

                comPort.DiscardInBuffer();
                comPort.Write(WriteBuff, 0, 8);
                NowTime = DateTime.Now;
                do
                {
                    if (comPort.BytesToRead >= 9)//收到数据
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
                    if (ErrStr.IndexOf("停止失败,接收数据已超时") < 0)
                    {
                        ErrStr = ErrStr + DateTime.Now.ToString() + mName + ":停止失败,接收数据已超时" + (char)13 + (char)10;
                    }
                    return false;
                }
                else
                {
                    comPort.Read(ReadBuff, 0, ReturnByte);
                    if ((ReadBuff[0] != WriteBuff[0]) || (ReadBuff[1] != WriteBuff[1]) || (ReadBuff[2] != WriteBuff[2]))//数据检验失败
                    {
                        //comPort.Close(); 
                        if (ErrStr.IndexOf("停止失败,接收数据错误") < 0)
                        {
                            ErrStr = ErrStr + DateTime.Now.ToString() + mName + ":停止失败,接收数据错误" + (char)13 + (char)10;
                        }
                        return false;
                    }
                }
                returnValue = true;
            }
            catch (Exception exc)
            {
                if (ErrStr.IndexOf(exc.ToString()) < 0)
                {
                    ErrStr = ErrStr + DateTime.Now.ToString() + mName + ":" + exc.ToString() + (char)13 + (char)10;
                }
            }
           
            return returnValue;
        }
    }
}
