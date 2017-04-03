using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using NewMideaProgram;
namespace System
{
    class cAiNuo97010
    {
        public static string mName = "AINUO97010";
        SerialPort comPort;//端口
        /// <summary>
        /// LGPLC使用的串口
        /// </summary>
        public SerialPort ComPort
        {
            get { return comPort; }
            set { comPort = value; }
        }
        string errStr = "Omron";//出错信息
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
        byte AiNuoAddress = 0;//LGPLC地址
        public bool mAiNuoIsInit = false;//PLC初始化结果      
        public cAiNuo97010(SerialPort mComPort, byte mAddress)
        {
            comPort = mComPort;
            AiNuoAddress = mAddress;
        }
        public bool Init()
        {
            mAiNuoIsInit = true;
            mAiNuoIsInit = Stop();
            return mAiNuoIsInit;
        }
        public bool Stop()
        {
            if (cMain.isDebug)
            {
                return true;
            }
            if (!mAiNuoIsInit)
            {
                Init();
                return false;
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
                WriteBuff[0] = 0x7B;// Encoding.ASCII.GetBytes(new char[] { '@' });
                WriteBuff[1] = 0x07;
                WriteBuff[2] = (byte)((AiNuoAddress & 0xFF00) >> 8);
                WriteBuff[3] = (byte)(AiNuoAddress & 0xFF);
                WriteBuff[4] = 0x43;
                WriteBuff[5] = 0x53;
                WriteBuff[6] = 0x50;
                WriteBuff[7] = 0x2A;
                WriteBuff[8] = Sum_DianYuan(WriteBuff, 1, 7);
                WriteBuff[9] = 0x7D;
                //string a = Encoding.ASCII.GetString(WriteBuff);
                comPort.DiscardInBuffer();
                comPort.Write(WriteBuff, 0, 10);
                NowTime = DateTime.Now;
                do
                {
                    //System.Windows.Forms.Application.DoEvents();
                    Threading.Thread.Sleep(20);
                    if (comPort.BytesToRead >= 13)//收到数据
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
                        ErrStr = ErrStr + DateTime.Now.ToString() + mName + ",init," + "初始失败,接收数据已超时" + (char)13 + (char)10;
                    }
                    return false;
                }
                else
                {
                    comPort.Read(ReadBuff, 0, ReturnByte);
                    if ((ReadBuff[0] != WriteBuff[0]) || (ReadBuff[4] !=0x43) || (ReadBuff[7] != 0x3D) || (ReadBuff[8] != 0x3D))//数据检验失败
                    {
                        if (ErrStr.IndexOf("接收数据错误") < 0)
                        {
                            ErrStr = ErrStr + DateTime.Now.ToString() + mName + ",init," + ":初始失败,接收数据错误" + (char)13 + (char)10;
                        }
                        return false;
                    }
                }
            }
            catch (Exception exc)
            {
                if (ErrStr.IndexOf(exc.ToString()) < 0)
                {
                    ErrStr = ErrStr + DateTime.Now.ToString() + mName + ",init," + ":" + exc.ToString() + (char)13 + (char)10;
                }
                return false;
            }
            return true;
        }
        public bool Start()
        {
            if (cMain.isDebug)
            {
                return true;
            }
            if (!mAiNuoIsInit)
            {
                Init();
                return false;
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
                WriteBuff[0] = 0x7B;// Encoding.ASCII.GetBytes(new char[] { '@' });
                WriteBuff[1] = 0x07;
                WriteBuff[2] = (byte)((AiNuoAddress & 0xFF00) >> 8);
                WriteBuff[3] = (byte)(AiNuoAddress & 0xFF);
                WriteBuff[4] = 0x43;
                WriteBuff[5] = 0x53;
                WriteBuff[6] = 0x54;
                WriteBuff[7] = 0x2A;
                WriteBuff[8] = Sum_DianYuan(WriteBuff, 1, 7);
                WriteBuff[9] = 0x7D;
                //string a = Encoding.ASCII.GetString(WriteBuff);
                comPort.DiscardInBuffer();
                comPort.Write(WriteBuff, 0, 10);
                NowTime = DateTime.Now;
                do
                {
                    //System.Windows.Forms.Application.DoEvents();
                    Threading.Thread.Sleep(20);
                    if (comPort.BytesToRead >= 13)//收到数据
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
                        ErrStr = ErrStr + DateTime.Now.ToString() + mName + ",init," + "初始失败,接收数据已超时" + (char)13 + (char)10;
                    }
                    return false;
                }
                else
                {
                    comPort.Read(ReadBuff, 0, ReturnByte);
                    if ((ReadBuff[0] != WriteBuff[0]) || (ReadBuff[4] != 0x43) || (ReadBuff[7] != 0x3D) || (ReadBuff[8] != 0x3D))//数据检验失败
                    {
                        if (ErrStr.IndexOf("接收数据错误") < 0)
                        {
                            ErrStr = ErrStr + DateTime.Now.ToString() + mName + ",init," + ":初始失败,接收数据错误" + (char)13 + (char)10;
                        }
                        return false;
                    }
                }
            }
            catch (Exception exc)
            {
                if (ErrStr.IndexOf(exc.ToString()) < 0)
                {
                    ErrStr = ErrStr + DateTime.Now.ToString() + mName + ",init," + ":" + exc.ToString() + (char)13 + (char)10;
                }
                return false;
            }
            return true;
        }
        public bool Set(int Vol, int Hz, int Group)
        {
            if (cMain.isDebug)
            {
                return true;
            }
            if (!mAiNuoIsInit)
            {
                Init();
                return false;
            }
            bool isReadOk = false;
            int ReadVol, ReadHz, ReadGroup;
            byte[] WriteBuff = new byte[29];//发送数据
            byte[] ReadBuff = new byte[20];//接收数据
            int ReturnByte = 0;//返回数据
            bool IsReturn = false;//是否成功返回
            bool IsTimeOut = false;//是否超时
            DateTime NowTime = DateTime.Now;//当前时间
            string WriteVolStr;
            string WriteHzStr;
            TimeSpan ts;//时间差
            try
            {
                if (!comPort.IsOpen)
                {
                    comPort.Open();
                }
                WriteVolStr = string.Format("{0:D3}", Vol);
                WriteHzStr = string.Format("{0:D4}", Hz * 10);
                WriteBuff[0] = 0x7B;// Encoding.ASCII.GetBytes(new char[] { '@' });
                WriteBuff[1] = 0x1A;
                WriteBuff[2] = (byte)((AiNuoAddress & 0xFF00) >> 8);
                WriteBuff[3] = (byte)(AiNuoAddress & 0xFF);
                WriteBuff[4] = 0x53;
                WriteBuff[5] = 0x4E;
                WriteBuff[6] = 0x4F;
                WriteBuff[7] = 0x3D;

                WriteBuff[8] = Encoding.ASCII.GetBytes(WriteVolStr)[0];//电压
                WriteBuff[9] = Encoding.ASCII.GetBytes(WriteVolStr)[1];
                WriteBuff[10] = Encoding.ASCII.GetBytes(WriteVolStr)[2];

                WriteBuff[11] = 0x2C;

                WriteBuff[12] = Encoding.ASCII.GetBytes(WriteHzStr)[0];//频率
                WriteBuff[13] = Encoding.ASCII.GetBytes(WriteHzStr)[1];
                WriteBuff[14] = Encoding.ASCII.GetBytes(WriteHzStr)[2];
                WriteBuff[15] = Encoding.ASCII.GetBytes(WriteHzStr)[3];


                WriteBuff[16] = 0x2C;

                WriteBuff[17] = 0x33;//上浮
                WriteBuff[18] = 0x30;

                WriteBuff[19] = 0x2C;

                WriteBuff[20] = 0x33;//下浮
                WriteBuff[21] = 0x30;

                WriteBuff[22] = 0x2C;

                WriteBuff[23] = Encoding.ASCII.GetBytes(Group.ToString("D1"))[0];//组别

                WriteBuff[24] = 0x2C;

                WriteBuff[25] = 0x30;//高档不锁定

                WriteBuff[26] = 0x2A;

                WriteBuff[27] = Sum_DianYuan(WriteBuff, 1, 26);
                WriteBuff[28] = 0x7D;
                comPort.DiscardInBuffer();
                comPort.Write(WriteBuff, 0, 29);
                NowTime = DateTime.Now;
                do
                {
                    //System.Windows.Forms.Application.DoEvents();
                    Threading.Thread.Sleep(20);
                    if (comPort.BytesToRead >= 13)//收到数据
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
                        ErrStr = ErrStr + DateTime.Now.ToString() + mName + ",init," + "初始失败,接收数据已超时" + (char)13 + (char)10;
                    }
                    return false;
                }
                else
                {
                    comPort.Read(ReadBuff, 0, ReturnByte);
                    //string a = Encoding.ASCII.GetString(ReadBuff);
                    if ((ReadBuff[0] != WriteBuff[0]) || (ReadBuff[4] != 0x53) || (ReadBuff[7] != 0x3D) || (ReadBuff[8] != 0x3D))//数据检验失败
                    {
                        if (ErrStr.IndexOf("接收数据错误") < 0)
                        {
                            ErrStr = ErrStr + DateTime.Now.ToString() + mName + ",init," + ":初始失败,接收数据错误" + (char)13 + (char)10;
                        }
                        return false;
                    }
                }
            }
            catch (Exception exc)
            {
                if (ErrStr.IndexOf(exc.ToString()) < 0)
                {
                    ErrStr = ErrStr + DateTime.Now.ToString() + mName + ",init," + ":" + exc.ToString() + (char)13 + (char)10;
                }
                return false;
            }
            Threading.Thread.Sleep(300);
            isReadOk = Read(out ReadVol, out ReadHz, out ReadGroup);
            if ((ReadVol != Vol) || (ReadHz != Hz) || (ReadGroup != Group))
            {
                isReadOk = false;
            }
            return isReadOk;
        }
        public bool Read(out int Vol,out int Hz,out int Group)
        {
            Vol = 0;
            Hz = 0;
            Group = 1;
            if (cMain.isDebug)
            {
                return true;
            }
            if (!mAiNuoIsInit)
            {
                Init();
                return false;
            }
            byte[] WriteBuff = new byte[10];//发送数据
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
                WriteBuff[0] = 0x7B;// Encoding.ASCII.GetBytes(new char[] { '@' });
                WriteBuff[1] = 0x07;
                WriteBuff[2] = (byte)((AiNuoAddress & 0xFF00) >> 8);
                WriteBuff[3] = (byte)(AiNuoAddress & 0xFF);
                WriteBuff[4] = 0x52;
                WriteBuff[5] = 0x4E;
                WriteBuff[6] = 0x53;
                WriteBuff[7] = 0x2A;
                WriteBuff[8] = Sum_DianYuan(WriteBuff, 1, 7);
                WriteBuff[9] = 0x7D;
                comPort.DiscardInBuffer();
                comPort.Write(WriteBuff, 0, 10);
                NowTime = DateTime.Now;
                do
                {
                    //System.Windows.Forms.Application.DoEvents();
                    Threading.Thread.Sleep(20);
                    if (comPort.BytesToRead >= 29)//收到数据
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
                        ErrStr = ErrStr + DateTime.Now.ToString() + mName + ",init," + "初始失败,接收数据已超时" + (char)13 + (char)10;
                    }
                    return false;
                }
                else
                {
                    comPort.Read(ReadBuff, 0, ReturnByte);
                    //string a = Encoding.ASCII.GetString(ReadBuff);
                    if ((ReadBuff[0] != WriteBuff[0]) || (ReadBuff[4] != 0x52) || (ReadBuff[7] != 0x3D))//数据检验失败
                    {
                        if (ErrStr.IndexOf("接收数据错误") < 0)
                        {
                            ErrStr = ErrStr + DateTime.Now.ToString() + mName + ",init," + ":初始失败,接收数据错误" + (char)13 + (char)10;
                        }
                        return false;
                    }
                    else
                    {
                        Vol = Num.IntParse(Encoding.ASCII.GetString(ReadBuff, 8, 3));
                        Hz = (int)Num.SingleParse(Encoding.ASCII.GetString(ReadBuff, 12, 4));
                        Group = Num.IntParse(Encoding.ASCII.GetString(ReadBuff, 23, 1));
                    }
                }
            }
            catch (Exception exc)
            {
                if (ErrStr.IndexOf(exc.ToString()) < 0)
                {
                    ErrStr = ErrStr + DateTime.Now.ToString() + mName + ",init," + ":" + exc.ToString() + (char)13 + (char)10;
                }
                return false;
            }
            return true;
        }
        private static byte Sum_DianYuan(byte[] tmpByte, int start, int mLen)
        {
            int sum = 0;
            byte returnValue = 0;
            for (int i = start; i < start + mLen; i++)
            {
                sum = sum + tmpByte[i];
            }
            returnValue = (byte)(sum & 0xFF);
            return returnValue;
        }
    }
}
