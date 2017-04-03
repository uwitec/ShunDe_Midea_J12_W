using System;
using System.Collections.Generic;
using System.Text;
using NewMideaProgram;
using System.IO.Ports;
namespace System
{
    /// <summary>
    /// 欧姆龙PLC
    /// </summary>
    public class cOmRonPlc
    {
        public static string mName = "欧姆龙PLC";
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
        //PLC状态表
        /// <summary>
        /// PLC状态表
        /// </summary>
        public enum PlcStatue
        {
            /// <summary>
            /// 编程
            /// </summary>
            BianCheng = 0,
            /// <summary>
            /// 监视
            /// </summary>
            JianShi = 2,
            /// <summary>
            /// 运行
            /// </summary>
            YunXing = 3
        }
        //PLC错误表
        /// <summary>
        /// PLC错误表
        /// </summary>
        public enum ErrorList
        {
            正常 = 0,
            运行方式不能执行 = 1,
            监视方式不能执行 = 2,
            地址超出区域 = 4,
            FSC校验错误 = 13,
            格式错误 = 14,
            入口码数据错误, 数据超出规定范围 = 15
        }
        byte OmronAddress = 0;//LGPLC地址
        public bool mOmronIsInit = false;//PLC初始化结果      
        /// <summary>
        /// 构造OmronPLC
        /// </summary>
        /// <param name="serialPort">SerialPort,PLC使用的串口</param>
        /// <param name="address">byte,PLC使用地址</param>
        /// <param name="timeOut">int,PLC通讯超时时间</param>
        public cOmRonPlc(SerialPort serialPort, byte address, int mTimeOut)
        {
            comPort = serialPort;
            OmronAddress = address;
            timeOut = mTimeOut;
        }
        //初始化PLC
        /// <summary>
        /// 初始化PLC
        /// </summary>
        /// <returns>bool,返回初始化是否成功</returns>
        public bool init()
        {
            if (cMain.isDebug)
            {
                mOmronIsInit = true;
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
                WriteBuff[0] = 64;// Encoding.ASCII.GetBytes(new char[] { '@' });
                WriteBuff[1] = Encoding.ASCII.GetBytes(string.Format("{0:D2}", OmronAddress))[0];// Encoding.ASCII.GetBytes(new char[] { '0' });
                WriteBuff[2] = Encoding.ASCII.GetBytes(string.Format("{0:D2}", OmronAddress))[1];// Encoding.ASCII.GetBytes(new char[] { '0' });
                WriteBuff[3] = 77;// Encoding.ASCII.GetBytes(new char[] { 'M' });
                WriteBuff[4] = 77;// Encoding.ASCII.GetBytes(new char[] { 'M' });
                OmronPlcCrc(WriteBuff, 0, 5, out CrcLo, out CrcHi);
                WriteBuff[5] = CrcLo;// 
                WriteBuff[6] = CrcHi;//
                WriteBuff[7] = 42;//*号
                WriteBuff[8] = 13;//回车结束符
                //string a = Encoding.ASCII.GetString(WriteBuff);
                comPort.DiscardInBuffer();
                comPort.Write(WriteBuff, 0, 9);
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
                    if ((ReadBuff[0] != WriteBuff[0]) || (ReadBuff[1] != WriteBuff[1]) || (ReadBuff[5] != 48) || (ReadBuff[6] != 48))//数据检验失败
                    {
                        if (ErrStr.IndexOf("接收数据错误") < 0)
                        {
                            ErrStr = ErrStr + DateTime.Now.ToString() + mName + ",init," + ":初始失败,接收数据错误" + (char)13 + (char)10;
                        }
                        return false;
                    }
                    else
                    {
                        mOmronIsInit = true;
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
            return  ChangeStatue(PlcStatue.JianShi);
        }
        //改变PLC运行状态
        /// <summary>
        /// 改变PLC运行状态
        /// </summary>
        /// <param name="mPlcStatue">PlcStatue,要设置的PLC状态</param>
        /// <returns>bool,设置是否写入成功</returns>
        public bool ChangeStatue(PlcStatue mPlcStatue)
        {
            if (cMain.isDebug)
            {
                return true;
            }
            if (!mOmronIsInit)
            {
                init();
                return false;
            }
            byte[] WriteBuff = new byte[12];//发送数据
            byte[] ReadBuff = new byte[20];//接收数据
            int ReturnByte = 0;//返回数据
            bool IsReturn = false;//是否成功返回
            bool IsTimeOut = false;//是否超时
            DateTime NowTime = DateTime.Now;//当前时间
            TimeSpan ts;//时间差
            string WriteDataStr;
            int ResultIndex;
            ErrorList ResultList;
            byte CrcHi = 0, CrcLo = 0;//CRC校验
            try
            {
                if (!comPort.IsOpen)
                {
                    comPort.Open();
                }
                WriteBuff[0] = Convert.ToByte('@');// Encoding.ASCII.GetBytes(new char[] { '@' })[0];
                WriteBuff[1] = Encoding.ASCII.GetBytes(string.Format("{0:D2}", OmronAddress))[0];// Encoding.ASCII.GetBytes(new char[] { '0' });
                WriteBuff[2] = Encoding.ASCII.GetBytes(string.Format("{0:D2}", OmronAddress))[1];// Encoding.ASCII.GetBytes(new char[] { '0' });
                WriteBuff[3] = Convert.ToByte('S');// Encoding.ASCII.GetBytes(new char[] { 'W' })[0];
                WriteBuff[4] = Convert.ToByte('C');// Encoding.ASCII.GetBytes(new char[] { 'D' })[0];
                WriteDataStr = string.Format("{0:D2}", (int)mPlcStatue);
                WriteBuff[5] = Encoding.ASCII.GetBytes(WriteDataStr)[0];
                WriteBuff[6] = Encoding.ASCII.GetBytes(WriteDataStr)[1];

                OmronPlcCrc(WriteBuff, 0, 7, out CrcLo, out CrcHi);
                WriteBuff[7] = CrcLo;// 
                WriteBuff[8] = CrcHi;//
                WriteBuff[9] = Convert.ToByte('*');//*号
                WriteBuff[10] = 13;//回车结束符
                comPort.DiscardInBuffer();
                comPort.Write(WriteBuff, 0, 11);
                NowTime = DateTime.Now;
                do
                {
                    Threading.Thread.Sleep(20);
                    if (comPort.BytesToRead >= 11)//收到数据
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
                        ErrStr = ErrStr + DateTime.Now.ToString() + mName + ",ChangeStatue," + "写入数据已超时" + (char)13 + (char)10;
                    }
                    return false;
                }
                else
                {
                    comPort.Read(ReadBuff, 0, ReturnByte);
                    if ((ReadBuff[0] != WriteBuff[0]) || (ReadBuff[1] != WriteBuff[1]))
                    {
                        if (ErrStr.IndexOf("接收数据错误") < 0)
                        {
                            ErrStr = ErrStr + DateTime.Now.ToString() + mName + ",ChangeStatue," + "写入数据错误" + (char)13 + (char)10;
                        }
                        return false;
                    }
                    else
                    {
                        ResultIndex = Num.IntParse(Encoding.ASCII.GetString(ReadBuff, 5, 2));
                        if (ResultIndex > 0)//数据检验失败
                        {
                            ResultList = (ErrorList)ResultIndex;
                            if (ErrStr.IndexOf(ResultList.ToString()) < 0)
                            {
                                ErrStr = ErrStr + DateTime.Now.ToString() + mName + ",ChangeStatue," + ResultList.ToString() + (char)13 + (char)10;
                            }
                            return false;
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                if (ErrStr.IndexOf(exc.ToString()) < 0)
                {
                    ErrStr = ErrStr + DateTime.Now.ToString() + mName + ",ChangeStatue," + ":" + exc.ToString() + (char)13 + (char)10;
                }
                return false;
            }
            return true;

        }
        //写入单个D点
        /// <summary>
        /// 写入单个D点
        /// </summary>
        /// <param name="StartPoint">int,写入D点的开始位</param>
        /// <param name="SendBuff">int,写入D点数值</param>
        /// <returns>bool,写入D点是否成功</returns>
        public bool WriteD(int StartPoint, int SendBuff)
        {
            return WriteD(StartPoint, 1, new int[] { SendBuff });
        }
        //批量写入D点
        /// <summary>
        /// 批量写入D点
        /// </summary>
        /// <param name="StartPoint">int,写入D点的开始位</param>
        /// <param name="WriteLength">int,写入D点的数据长度</param>
        /// <param name="SendBuff">int[],写入D点的数值</param>
        /// <returns>bool,写入D点是否成功</returns>
        public bool WriteD(int StartPoint, int WriteLength, int[] SendBuff)
        {
            if (cMain.isDebug)
            {
                mOmronIsInit = true;
                return true;
            }
            if (!mOmronIsInit)
            {
                init();
                return false;
            }
            WriteLength = Math.Min(WriteLength, SendBuff.Length);
            byte[] WriteBuff = new byte[20 + WriteLength * 4];//发送数据
            byte[] ReadBuff = new byte[20];//接收数据
            int ReturnByte = 0;//返回数据
            bool IsReturn = false;//是否成功返回
            bool IsTimeOut = false;//是否超时
            DateTime NowTime = DateTime.Now;//当前时间
            TimeSpan ts;//时间差
            string StartPointStr;
            string WriteLengthStr;
            string WriteDataStr;
            int ResultIndex;
            ErrorList ResultList;
            byte CrcHi = 0, CrcLo = 0;//CRC校验
            try
            {
                if (!comPort.IsOpen)
                {
                    comPort.Open();
                }
                StartPointStr = string.Format("{0:D4}", StartPoint);
                WriteLengthStr = string.Format("{0:D4}", WriteLength);
                WriteBuff[0] = Convert.ToByte('@');// Encoding.ASCII.GetBytes(new char[] { '@' })[0];
                WriteBuff[1] = Encoding.ASCII.GetBytes(string.Format("{0:D2}", OmronAddress))[0];// Encoding.ASCII.GetBytes(new char[] { '0' });
                WriteBuff[2] = Encoding.ASCII.GetBytes(string.Format("{0:D2}", OmronAddress))[1];// Encoding.ASCII.GetBytes(new char[] { '0' });
                WriteBuff[3] = Convert.ToByte('W');// Encoding.ASCII.GetBytes(new char[] { 'W' })[0];
                WriteBuff[4] = Convert.ToByte('D');// Encoding.ASCII.GetBytes(new char[] { 'D' })[0];
                WriteBuff[5] = Encoding.ASCII.GetBytes(StartPointStr)[0];
                WriteBuff[6] = Encoding.ASCII.GetBytes(StartPointStr)[1];
                WriteBuff[7] = Encoding.ASCII.GetBytes(StartPointStr)[2];
                WriteBuff[8] = Encoding.ASCII.GetBytes(StartPointStr)[3];
                for (int i = 0; i < WriteLength; i++)
                {
                    WriteDataStr = string.Format("{0:X4}", SendBuff[i]);
                    byte[] tmpByte = Encoding.ASCII.GetBytes(WriteDataStr);
                    WriteBuff[9 + 4 * i] = tmpByte[0];
                    WriteBuff[10 + 4 * i] = tmpByte[1];
                    WriteBuff[11 + 4 * i] = tmpByte[2];
                    WriteBuff[12 + 4 * i] = tmpByte[3];
                }
                OmronPlcCrc(WriteBuff, 0, 9 + 4 * WriteLength, out CrcLo, out CrcHi);
                WriteBuff[9 + 4 * WriteLength] = CrcLo;// 
                WriteBuff[10 + 4 * WriteLength] = CrcHi;//
                WriteBuff[11 + 4 * WriteLength] = Convert.ToByte('*');//*号
                WriteBuff[12 + 4 * WriteLength] = 13;//回车结束符
                comPort.DiscardInBuffer();
                comPort.Write(WriteBuff, 0, 13 + 4 * WriteLength);
                NowTime = DateTime.Now;
                do
                {
                    Threading.Thread.Sleep(20);
                    if (comPort.BytesToRead >= 11)//收到数据
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
                        ErrStr = ErrStr + DateTime.Now.ToString() + mName + ",WriteD," + "写入数据已超时" + (char)13 + (char)10;
                    }
                    return false;
                }
                else
                {
                    comPort.Read(ReadBuff, 0, ReturnByte);
                    if ((ReadBuff[0] != WriteBuff[0]) || (ReadBuff[1] != WriteBuff[1]))
                    {
                        if (ErrStr.IndexOf("接收数据错误") < 0)
                        {
                            ErrStr = ErrStr + DateTime.Now.ToString() + mName + ",WriteD," + "写入数据错误" + (char)13 + (char)10;
                        }
                        return false;
                    }
                    else
                    {
                        ResultIndex = Num.IntParse(Encoding.ASCII.GetString(ReadBuff, 5, 2));
                        if (ResultIndex > 0)//数据检验失败
                        {
                            ResultList = (ErrorList)ResultIndex;
                            if (ErrStr.IndexOf(ResultList.ToString()) < 0)
                            {
                                ErrStr = ErrStr + DateTime.Now.ToString() + mName + ",WriteD," + ResultList.ToString() + (char)13 + (char)10;
                            }
                            return false;
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                if (ErrStr.IndexOf(exc.ToString()) < 0)
                {
                    ErrStr = ErrStr + DateTime.Now.ToString() + mName + ",WriteD," + ":" + exc.ToString() + (char)13 + (char)10;
                }
                return false;
            }
            return true;
        }
        //读取单个D点
        /// <summary>
        /// 读取单个D点
        /// </summary>
        /// <param name="StartPoint">int,读取D点的开始位</param>
        /// <param name="ReturnBuff">ref int,读取D点的数值</param>
        /// <returns>bool,读取数据是否成功</returns>
        public bool ReadD(int StartPoint, ref int ReturnBuff)
        {
            bool returnResult = false;
            int[] temp = new int[1];
            temp[0] = 0;
            if (ReadD(StartPoint, 1, ref temp))
            {
                ReturnBuff = temp[0];
                returnResult = true;
            }
            else
            {
                returnResult = false;
            }
            return returnResult;
        }
        //批量读取D点
        /// <summary>
        /// 读取D点
        /// </summary>
        /// <param name="StartPoint">int,读取D点的开始位</param>
        /// <param name="ReadLength">int,读取D点的个数</param>
        /// <param name="ReturnBuff">ref int,读取D点的数值</param>
        /// <returns>bool,读取数据是否成功</returns>
        public bool ReadD(int StartPoint, int ReadLength, ref int[] ReturnBuff)
        {
            if (cMain.isDebug)
            {
                ReturnBuff[7] = 61;
                ReturnBuff[8] = 61;
                mOmronIsInit = true;
                return true;
            }
            if (!mOmronIsInit)
            {
                init();
                return false;
            }
            ReadLength = Math.Min(ReadLength, ReturnBuff.Length);
            byte[] WriteBuff = new byte[17];//发送数据
            byte[] ReadBuff = new byte[20 + ReadLength * 4];//接收数据
            int ReturnByte = 0;//返回数据
            bool IsReturn = false;//是否成功返回
            bool IsTimeOut = false;//是否超时
            DateTime NowTime = DateTime.Now;//当前时间
            TimeSpan ts;//时间差
            string StartPointStr;
            string ReadLengthStr;
            int ResultIndex;//PLC执行结果
            ErrorList ResultList;
            byte CrcHi = 0, CrcLo = 0;//CRC校验
            try
            {
                if (!comPort.IsOpen)
                {
                    comPort.Open();
                }
                StartPointStr = string.Format("{0:D4}", StartPoint);
                ReadLengthStr = string.Format("{0:D4}", ReadLength);
                WriteBuff[0] = Convert.ToByte('@');// Encoding.ASCII.GetBytes(new char[] { '@' })[0];
                WriteBuff[1] = Encoding.ASCII.GetBytes(string.Format("{0:D2}", OmronAddress))[0];// Encoding.ASCII.GetBytes(new char[] { '0' });
                WriteBuff[2] = Encoding.ASCII.GetBytes(string.Format("{0:D2}", OmronAddress))[1];// Encoding.ASCII.GetBytes(new char[] { '0' });
                WriteBuff[3] = Convert.ToByte('R');//Encoding.ASCII.GetBytes(new char[] { 'R' })[0];
                WriteBuff[4] = Convert.ToByte('D');//Encoding.ASCII.GetBytes(new char[] { 'D' })[0];
                WriteBuff[5] = Encoding.ASCII.GetBytes(StartPointStr)[0];
                WriteBuff[6] = Encoding.ASCII.GetBytes(StartPointStr)[1];
                WriteBuff[7] = Encoding.ASCII.GetBytes(StartPointStr)[2];
                WriteBuff[8] = Encoding.ASCII.GetBytes(StartPointStr)[3];
                WriteBuff[9] = Encoding.ASCII.GetBytes(ReadLengthStr)[0];
                WriteBuff[10] = Encoding.ASCII.GetBytes(ReadLengthStr)[1];
                WriteBuff[11] = Encoding.ASCII.GetBytes(ReadLengthStr)[2];
                WriteBuff[12] = Encoding.ASCII.GetBytes(ReadLengthStr)[3];
                OmronPlcCrc(WriteBuff, 0, 13, out CrcLo, out CrcHi);
                WriteBuff[13] = CrcLo;// 
                WriteBuff[14] = CrcHi;//
                WriteBuff[15] = Convert.ToByte('*');//*号
                WriteBuff[16] = 13;//回车结束符
                //string a = Encoding.ASCII.GetString(WriteBuff);
                comPort.DiscardInBuffer();
                comPort.Write(WriteBuff, 0, 17);
                NowTime = DateTime.Now;
                do
                {
                    //System.Windows.Forms.Application.DoEvents();
                    Threading.Thread.Sleep(20);
                    if (comPort.BytesToRead >= (11 + 4 * ReadLength))//收到数据
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
                        ErrStr = ErrStr + DateTime.Now.ToString() + mName + ",ReadD," + "接收数据已超时" + (char)13 + (char)10;
                    }
                    return false;
                }
                else
                {
                    //@{地址*2}{命令*2}{结果*2}{返回值*4}*个数{*}{回车}
                    comPort.Read(ReadBuff, 0, ReturnByte);
                    if ((ReadBuff[0] != WriteBuff[0]) || (ReadBuff[1] != WriteBuff[1]))
                    {
                        if (ErrStr.IndexOf("接收数据错误") < 0)
                        {
                            ErrStr = ErrStr + DateTime.Now.ToString() + mName + ",ReadD," + "接收数据错误" + (char)13 + (char)10;
                        }
                        return false;
                    }
                    else
                    {
                        ResultIndex = Num.IntParse(Encoding.ASCII.GetString(ReadBuff, 5, 2));
                        if (ResultIndex > 0)//数据检验失败
                        {
                            ResultList = (ErrorList)ResultIndex;
                            if (ErrStr.IndexOf(ResultList.ToString()) < 0)
                            {
                                ErrStr = ErrStr + DateTime.Now.ToString() + mName + ",ReadD," + ResultList.ToString() + (char)13 + (char)10;
                            }
                            return false;
                        }
                        else
                        {
                            for (int i = 0; i < ReadLength; i++)
                            {
                                
                                string tmp = Encoding.ASCII.GetString(ReadBuff, 7 + i * 4, 4);
                                ReturnBuff[i] = Convert.ToInt16(tmp, 16);
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                if (ErrStr.IndexOf(exc.ToString()) < 0)
                {
                    ErrStr = ErrStr + DateTime.Now.ToString() + mName + ",ReadD," + ":" + exc.ToString() + (char)13 + (char)10;
                }
                return false;
            }
            return true;

        }//写入W点
        /// <summary>
        /// 写入W点
        /// </summary>
        /// <param name="StartPoint">int,写入的W点,如W1.01,则StartPoint为101</param>
        /// <param name="SendBuff">bool,写入的值</param>
        /// <returns>bool,返回写入是否成功</returns>
        public bool WriteIR(int StartPoint, bool SendBuff)
        {
            if (cMain.isDebug)
            {
                mOmronIsInit = true;
                return true;
            }
            if (!mOmronIsInit)
            {
                init();
                return false;
            }
            int oldData = 0;
            int StartZi=0;//点的字
            int StartWei=0;//点的位
            StartZi=StartPoint/100;
            StartWei=StartPoint % 100;
            if (!ReadIR(StartZi, ref oldData))//先读原来的数据
            {
                return false;
            }
            else
            {
                int tmpWei=0;
                tmpWei = (int)(Math.Pow(2, StartWei));
                if (!SendBuff)
                {
                    tmpWei = 65535 - tmpWei;
                    oldData = oldData & tmpWei;
                }
                else
                {
                    oldData = oldData | tmpWei;
                }
            }
            byte[] WriteBuff = new byte[18];//发送数据
            byte[] ReadBuff = new byte[40];//接收数据
            int ReturnByte = 0;//返回数据
            bool IsReturn = false;//是否成功返回
            bool IsTimeOut = false;//是否超时
            DateTime NowTime = DateTime.Now;//当前时间
            TimeSpan ts;//时间差
            string StartPointStr;
            string StartValueStr;
            int ResultIndex;//PLC执行结果
            ErrorList ResultList;
            byte CrcHi = 0, CrcLo = 0;//CRC校验
            try
            {
                if (!comPort.IsOpen)
                {
                    comPort.Open();
                }
                StartPointStr = string.Format("{0:D4}", StartZi);
                StartValueStr = string.Format("{0:X4}", oldData);
                WriteBuff[0] = Convert.ToByte('@');// Encoding.ASCII.GetBytes(new char[] { '@' })[0];
                WriteBuff[1] = Encoding.ASCII.GetBytes(string.Format("{0:D2}", OmronAddress))[0];// Encoding.ASCII.GetBytes(new char[] { '0' });
                WriteBuff[2] = Encoding.ASCII.GetBytes(string.Format("{0:D2}", OmronAddress))[1];// Encoding.ASCII.GetBytes(new char[] { '0' });

                WriteBuff[3] = Convert.ToByte('W');
                WriteBuff[4] = Convert.ToByte('R');
                for (int i = 0; i < StartPointStr.Length; i++)
                {
                    WriteBuff[5 + i] = Encoding.ASCII.GetBytes(StartPointStr)[i];
                }
                for (int i = 0; i < StartValueStr.Length; i++)
                {
                    WriteBuff[9 + i] = Encoding.ASCII.GetBytes(StartValueStr)[i];
                }
                OmronPlcCrc(WriteBuff, 0, 13, out CrcLo, out CrcHi);
                WriteBuff[13] = CrcLo;// 
                WriteBuff[14] = CrcHi;//
                WriteBuff[15] = Convert.ToByte('*');//*号
                WriteBuff[16] = 13;//回车结束符
                //string a = Encoding.ASCII.GetString(WriteBuff);
                comPort.DiscardInBuffer();
                comPort.Write(WriteBuff, 0, 17);
                NowTime = DateTime.Now;
                do
                {
                    //System.Windows.Forms.Application.DoEvents();
                    Threading.Thread.Sleep(20);
                    if (comPort.BytesToRead >= 11)//收到数据
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
                        ErrStr = ErrStr + DateTime.Now.ToString() + mName + ",WriteIR," + "接收数据已超时" + (char)13 + (char)10;
                    }
                    return false;
                }
                else
                {
                    //@{地址*2}{命令*2}{结果*2}{返回值*4}*个数{*}{回车}
                    comPort.Read(ReadBuff, 0, ReturnByte);
                    if ((ReadBuff[0] != WriteBuff[0]) || (ReadBuff[1] != WriteBuff[1]))
                    {
                        if (ErrStr.IndexOf("接收数据错误") < 0)
                        {
                            ErrStr = ErrStr + DateTime.Now.ToString() + mName + ",WriteIR," + "接收数据错误" + (char)13 + (char)10;
                        }
                        return false;
                    }
                    else
                    {
                        //string b = Encoding.ASCII.GetString(ReadBuff);
                        ResultIndex = Num.IntParse(Encoding.ASCII.GetString(ReadBuff, 5, 2));
                        if (ResultIndex > 0)//数据检验失败
                        {
                            ResultList = (ErrorList)ResultIndex;
                            if (ErrStr.IndexOf(ResultList.ToString()) < 0)
                            {
                                ErrStr = ErrStr + DateTime.Now.ToString() + mName + ",WriteIR," + ResultList.ToString() + (char)13 + (char)10;
                            }
                            return false;
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                if (ErrStr.IndexOf(exc.ToString()) < 0)
                {
                    ErrStr = ErrStr + DateTime.Now.ToString() + mName + ",WriteIR," + ":" + exc.ToString() + (char)13 + (char)10;
                }
                return false;
            }
            return true;
        }
        ////写入W点
        ///// <summary>
        ///// 写入W点
        ///// </summary>
        ///// <param name="StartPoint">int,写入的W点,如W1.01,则StartPoint为101</param>
        ///// <param name="SendBuff">bool,写入的值</param>
        ///// <returns>bool,返回写入是否成功</returns>
        //public bool WriteIR(int StartPoint, bool SendBuff)
        //{
        //    if (cMain.isDebug)
        //    {
        //        mOmronIsInit = true;
        //        return true;
        //    }
        //    byte[] WriteBuff = new byte[51];//发送数据
        //    byte[] ReadBuff = new byte[40];//接收数据
        //    int ReturnByte = 0;//返回数据
        //    bool IsReturn = false;//是否成功返回
        //    bool IsTimeOut = false;//是否超时
        //    DateTime NowTime = DateTime.Now;//当前时间
        //    TimeSpan ts;//时间差
        //    string StartPointStr;
        //    string ConstStr = "FA08000020000000000FC0023010001";
        //    byte[] ConstBuff;
        //    int ResultIndex;//PLC执行结果
        //    ErrorList ResultList;
        //    byte CrcHi = 0, CrcLo = 0;//CRC校验
        //    try
        //    {
        //        if (!comPort.IsOpen)
        //        {
        //            comPort.Open();
        //        }
        //        StartPointStr = string.Format("{0:X6}", StartPoint);
        //        ConstBuff = Encoding.ASCII.GetBytes(ConstStr);
        //        WriteBuff[0] = Convert.ToByte('@');// Encoding.ASCII.GetBytes(new char[] { '@' })[0];
        //        WriteBuff[1] = Encoding.ASCII.GetBytes(string.Format("{0:D2}", OmronAddress))[0];// Encoding.ASCII.GetBytes(new char[] { '0' });
        //        WriteBuff[2] = Encoding.ASCII.GetBytes(string.Format("{0:D2}", OmronAddress))[1];// Encoding.ASCII.GetBytes(new char[] { '0' });
        //        for (int i = 0; i < ConstBuff.Length; i++)
        //        {
        //            WriteBuff[3 + i] = ConstBuff[i];
        //        }
        //        WriteBuff[3 + ConstBuff.Length] = 48;//'0';
        //        WriteBuff[4 + ConstBuff.Length] = 48;//'0';
        //        WriteBuff[5 + ConstBuff.Length] = 48;//'0';
        //        WriteBuff[6 + ConstBuff.Length] = (SendBuff) ? Convert.ToByte('1') : Convert.ToByte('0');
        //        WriteBuff[7 + ConstBuff.Length] = Convert.ToByte('3');
        //        WriteBuff[8 + ConstBuff.Length] = Convert.ToByte('1');
        //        for (int i = 0; i < StartPointStr.Length; i++)
        //        {
        //            WriteBuff[9 + ConstBuff.Length + i] = Encoding.ASCII.GetBytes(StartPointStr)[i];
        //        }
        //        OmronPlcCrc(WriteBuff, 0, 9 + ConstBuff.Length + StartPointStr.Length, out CrcLo, out CrcHi);
        //        WriteBuff[9 + ConstBuff.Length + StartPointStr.Length] = CrcLo;// 
        //        WriteBuff[10 + ConstBuff.Length + StartPointStr.Length] = CrcHi;//
        //        WriteBuff[11 + ConstBuff.Length + StartPointStr.Length] = Convert.ToByte('*');//*号
        //        WriteBuff[12 + ConstBuff.Length + StartPointStr.Length] = 13;//回车结束符
        //        //string a = Encoding.ASCII.GetString(WriteBuff);
        //        comPort.DiscardInBuffer();
        //        comPort.Write(WriteBuff, 0, 50);
        //        NowTime = DateTime.Now;
        //        do
        //        {
        //            //System.Windows.Forms.Application.DoEvents();
        //            Threading.Thread.Sleep(20);
        //            if (comPort.BytesToRead >= 39)//收到数据
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
        //            if (ErrStr.IndexOf("接收数据已超时") < 0)
        //            {
        //                ErrStr = ErrStr + DateTime.Now.ToString() + mName + ",WriteIR," + "接收数据已超时" + (char)13 + (char)10;
        //            }
        //            return false;
        //        }
        //        else
        //        {
        //            //@{地址*2}{命令*2}{结果*2}{返回值*4}*个数{*}{回车}
        //            comPort.Read(ReadBuff, 0, ReturnByte);
        //            if ((ReadBuff[0] != WriteBuff[0]) || (ReadBuff[1] != WriteBuff[1]))
        //            {
        //                if (ErrStr.IndexOf("接收数据错误") < 0)
        //                {
        //                    ErrStr = ErrStr + DateTime.Now.ToString() + mName + ",WriteIR," + "接收数据错误" + (char)13 + (char)10;
        //                }
        //                return false;
        //            }
        //            else
        //            {
        //                //string b = Encoding.ASCII.GetString(ReadBuff);
        //                ResultIndex = Num.IntParse(Encoding.ASCII.GetString(ReadBuff, 5, 2));
        //                if (ResultIndex > 0)//数据检验失败
        //                {
        //                    ResultList = (ErrorList)ResultIndex;
        //                    if (ErrStr.IndexOf(ResultList.ToString()) < 0)
        //                    {
        //                        ErrStr = ErrStr + DateTime.Now.ToString() + mName + ",WriteIR," + ResultList.ToString() + (char)13 + (char)10;
        //                    }
        //                    return false;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception exc)
        //    {
        //        if (ErrStr.IndexOf(exc.ToString()) < 0)
        //        {
        //            ErrStr = ErrStr + DateTime.Now.ToString() + mName + ",WriteIR," + ":" + exc.ToString() + (char)13 + (char)10;
        //        }
        //        return false;
        //    }
        //    return true;
        //}
        //读取单个IR点
        /// <summary>
        /// 读取单个IR点
        /// </summary>
        /// <param name="StartPoint">int,读取的IR点,如W1.01,则StartPoint为101</param>
        /// <param name="ReturnBuff">bool,读取到的值</param>
        /// <returns>bool,返回读取是否成功</returns>
        public bool ReadIR(int StartPoint, ref bool ReturnBuff)
        {
            bool returnResult = false;
            bool[] temp = new bool[16];
            int readIndex = StartPoint % 100;
            temp[0] = false;
            if (ReadIR(StartPoint, 16, ref temp))
            {
                ReturnBuff = temp[readIndex];
                returnResult = true;
            }
            else
            {
                returnResult = false;
            }
            return returnResult;
        }
        public bool ReadIR(int StartPoint, ref int ReturnBuff)
        {
            bool returnResult = false;
            int[] temp = new int[1];
            temp[0] = 0;
            if (ReadIR(StartPoint, 1, ref temp))
            {
                ReturnBuff = temp[0];
                returnResult = true;
            }
            else
            {
                returnResult = false;
            }
            return returnResult;
        }
        public bool ReadIR(int StartPoint, int ReadLength, ref int[] ReturnBuff)
        {
            if (cMain.isDebug)
            {
                mOmronIsInit = true;
                return true;
            }
            if (!mOmronIsInit)
            {
                init();
                return false;
            }
            byte[] WriteBuff = new byte[17];//发送数据
            byte[] ReadBuff = new byte[20 + ReadLength * 4];//接收数据
            int ReturnByte = 0;//返回数据
            bool IsReturn = false;//是否成功返回
            bool IsTimeOut = false;//是否超时
            DateTime NowTime = DateTime.Now;//当前时间
            TimeSpan ts;//时间差
            string StartPointStr;
            string ReadLengthStr;
            int ResultIndex;//PLC执行结果
            ErrorList ResultList;
            byte CrcHi = 0, CrcLo = 0;//CRC校验
            try
            {
                if (!comPort.IsOpen)
                {
                    comPort.Open();
                }
                StartPointStr = string.Format("{0:D4}", StartPoint);
                ReadLengthStr = string.Format("{0:D4}", ReadLength);
                WriteBuff[0] = Convert.ToByte('@');// Encoding.ASCII.GetBytes(new char[] { '@' })[0];
                WriteBuff[1] = Encoding.ASCII.GetBytes(string.Format("{0:D2}", OmronAddress))[0];// Encoding.ASCII.GetBytes(new char[] { '0' });
                WriteBuff[2] = Encoding.ASCII.GetBytes(string.Format("{0:D2}", OmronAddress))[1];// Encoding.ASCII.GetBytes(new char[] { '0' });
                WriteBuff[3] = Convert.ToByte('R');//Encoding.ASCII.GetBytes(new char[] { 'R' })[0];
                WriteBuff[4] = Convert.ToByte('R');//Encoding.ASCII.GetBytes(new char[] { 'D' })[0];
                WriteBuff[5] = Encoding.ASCII.GetBytes(StartPointStr)[0];
                WriteBuff[6] = Encoding.ASCII.GetBytes(StartPointStr)[1];
                WriteBuff[7] = Encoding.ASCII.GetBytes(StartPointStr)[2];
                WriteBuff[8] = Encoding.ASCII.GetBytes(StartPointStr)[3];
                WriteBuff[9] = Encoding.ASCII.GetBytes(ReadLengthStr)[0];
                WriteBuff[10] = Encoding.ASCII.GetBytes(ReadLengthStr)[1];
                WriteBuff[11] = Encoding.ASCII.GetBytes(ReadLengthStr)[2];
                WriteBuff[12] = Encoding.ASCII.GetBytes(ReadLengthStr)[3];
                OmronPlcCrc(WriteBuff, 0, 13, out CrcLo, out CrcHi);
                WriteBuff[13] = CrcLo;// 
                WriteBuff[14] = CrcHi;//
                WriteBuff[15] = Convert.ToByte('*');//*号
                WriteBuff[16] = 13;//回车结束符
                //string a = Encoding.ASCII.GetString(WriteBuff);
                comPort.DiscardInBuffer();
                comPort.Write(WriteBuff, 0, 17);
                NowTime = DateTime.Now;
                do
                {
                    //System.Windows.Forms.Application.DoEvents();
                    Threading.Thread.Sleep(20);
                    if (comPort.BytesToRead >= (11 + 4 * ReadLength))//收到数据
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
                        ErrStr = ErrStr + DateTime.Now.ToString() + mName + ",ReadIR," + "接收数据已超时" + (char)13 + (char)10;
                    }
                    return false;
                }
                else
                {
                    //@{地址*2}{命令*2}{结果*2}{返回值*4}*个数{*}{回车}
                    comPort.Read(ReadBuff, 0, ReturnByte);
                    if ((ReadBuff[0] != WriteBuff[0]) || (ReadBuff[1] != WriteBuff[1]))
                    {
                        if (ErrStr.IndexOf("接收数据错误") < 0)
                        {
                            ErrStr = ErrStr + DateTime.Now.ToString() + mName + ",ReadIR," + "接收数据错误" + (char)13 + (char)10;
                        }
                        return false;
                    }
                    else
                    {
                        ResultIndex = Num.IntParse(Encoding.ASCII.GetString(ReadBuff, 5, 2));
                        if (ResultIndex > 0)//数据检验失败
                        {
                            ResultList = (ErrorList)ResultIndex;
                            if (ErrStr.IndexOf(ResultList.ToString()) < 0)
                            {
                                ErrStr = ErrStr + DateTime.Now.ToString() + mName + ",ReadIR," + ResultList.ToString() + (char)13 + (char)10;
                            }
                            return false;
                        }
                        else
                        {
                            for (int i = 0; i < ReadLength; i++)
                            {
                                string tmpStr = Encoding.ASCII.GetString(ReadBuff, 7 + i * 4, 4);
                                int tmpInt = Convert.ToInt16(tmpStr, 16);
                                ReturnBuff[i] = tmpInt;
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                if (ErrStr.IndexOf(exc.ToString()) < 0)
                {
                    ErrStr = ErrStr + DateTime.Now.ToString() + mName + ",ReadIR," + ":" + exc.ToString() + (char)13 + (char)10;
                }
                return false;
            }
            return true;
        }
        //批量读取IR点
        /// <summary>
        /// 批量读取IR点
        /// </summary>
        /// <param name="StartPoint">int,读取的IR点,如W1.01,则StartPoint为101</param>
        /// <param name="ReadLength">int,要读取IR点的个数</param>
        /// <param name="ReturnBuff">bool,读取到的值</param>
        /// <returns>bool,返回读取是否成功</returns>
        public bool ReadIR(int StartPoint, int ReadLength, ref bool[] ReturnBuff)
        {
            bool isOk = false; 
            ReadLength = Math.Min((int)(Math.Ceiling(ReadLength / 16.000)), (int)(Math.Ceiling(ReturnBuff.Length / 16.000)));
            int[] tempInt=new int[ReadLength];
            isOk = ReadIR(StartPoint, ReadLength, ref tempInt);
            for (int i = 0; i < ReadLength; i++)
            {
                int tmpInt = tempInt[i];
                for (int j = 0; j < 16; j++)
                {
                    if ((i * 16 + j) < ReturnBuff.Length)
                    {
                        if ((tmpInt & (int)Math.Pow(2, j)) == (int)Math.Pow(2, j))
                        {
                            ReturnBuff[i * 16 + j] = true;
                        }
                        else
                        {
                            ReturnBuff[i * 16 + j] = false;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return isOk;
            
        }
        //计算PLC校验
        /// <summary>
        /// 计算PLC校验
        /// </summary>
        /// <param name="writeBuff">byte[],要写入的字符数组</param>
        /// <param name="start">int,计算校验的开始位,从0开始</param>
        /// <param name="len">int,计算校验的长度,不能大于总长</param>
        /// <param name="CrcLo">out byte,计算后的校验</param>
        /// <param name="CrcHi">out byte,计算后的校验</param>
        /// <returns>bool,返回计算校验是否成功</returns>
        private bool OmronPlcCrc(byte[] writeBuff, int start, int len, out byte CrcLo, out byte CrcHi)
        {
            bool isOk = false;
            byte returnValue = 0;
            try
            {
                for (int i = 0; i < len; i++)
                {
                    returnValue = (byte)(returnValue ^ writeBuff[i]);
                }
                isOk = true;
            }
            catch
            { }
            string tmpStr = string.Format("{0:X2}", returnValue);
            CrcLo = Encoding.ASCII.GetBytes(tmpStr.Substring(0, 1))[0];
            CrcHi = Encoding.ASCII.GetBytes(tmpStr.Substring(1, 1))[0];
            return isOk;
        }
    }
}
