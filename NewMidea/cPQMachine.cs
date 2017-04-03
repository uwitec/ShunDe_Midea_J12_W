using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Threading;
namespace NewMideaProgram
{
    public class cPQMachine
    {
        SerialPort com;

        public SerialPort Com
        {
            get { return com; }
            set { com = value; }
        }
        string error;

        public string Error
        {
            get { return error; }
            set { error = value; }
        }
        public enum MachineLists
        {
            真PQ,
            假PQ
        }
        string pqStr = "";
        MachineLists machine = MachineLists.真PQ;
        Thread thRead;
        bool exit = false;
        public cPQMachine(string com)
        {
            this.Com = new SerialPort(com, 4800, Parity.None, 8, StopBits.One);
            pqStr = cMideaSnCode.GetSnPQ(cMideaSnCode.StepName.Hot, 60);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m">机器类型</param>
        public void Init(MachineLists m)
        {
            try
            {
                switch (m)
                {
                    case MachineLists.真PQ:
                        this.Com.BaudRate = 4800;
                        this.Com.Parity = Parity.None;
                        this.Com.DataBits = 8;
                        this.Com.StopBits = StopBits.One;
                        break;
                    case MachineLists.假PQ:
                        this.Com.BaudRate = 1200;
                        this.Com.Parity = Parity.Even;
                        this.Com.DataBits = 8;
                        this.Com.StopBits = StopBits.Two;
                        break;
                }
                this.machine = m;
                this.Com.RtsEnable = true;
                this.Com.DtrEnable = true;
                this.Com.Open();
                exit = false;
                thRead = new Thread(new ThreadStart(Flush));
                thRead.IsBackground = true;
                thRead.Start();
            }
            catch(Exception e)
            {
                Error = e.Message;
            }
        }
        public void Close()
        {
            exit = true;
            if (thRead != null)
            {
                thRead.Abort();
                thRead.Join(100);
                thRead = null;
            }
            if (this.Com != null)
            {
                if (this.Com.IsOpen)
                {
                    this.Com.Close();
                }
            }
        }
        public void WritePQ(string PQ)
        {
            pqStr = PQ;
        }
        private byte[] GetBuffFromPQ(bool FE,byte door)
        {
            byte[] buff = Num.GetHexByte(pqStr);
            if (door == 3)
            {
                buff[6] = 0xFA;//写入正确的机型
            }
            else
            {
                buff[6] = 0x00;
            }
            cMideaSnCode.Crc(buff, cMideaSnCode.CrcList.PQ, ref buff[8]);//重算校验

            byte[] result = new byte[10];
            int index = 0;
            if (FE)//是否加前面的0xFE冗余码
            {
                result = new byte[11];
                result[index++] = 0xFE;
            }
            for (int i = index, j = 0; i < result.Length && j < buff.Length; i++, j++)
            {
                result[index++] = buff[j];
            }
            return result;
        }
        private void Flush()
        {
            int len = 0;
            byte[] buff;
            while (!exit)
            {
                switch (machine)
                {
                    case MachineLists.假PQ:
                        buff = pqStr.ToBytes();
                        this.Com.DiscardInBuffer();
                        this.Com.Write(buff, 0, buff.Length);
                        Thread.Sleep(2000);
                        break;
                    case MachineLists.真PQ:
                        len = this.Com.BytesToRead;
                        Thread.Sleep(30);
                        if (len > 0 && len == this.Com.BytesToRead)
                        {
                            switch (len)
                            {
                                case 10:
                                    buff = new byte[len];
                                    this.Com.Read(buff, 0, len);
                                    if (buff[0] == 0xAA && buff[9] == 0x55)
                                    {
                                        //this.com.Write(pqStr.ToBytes(), 0, pqStr.ToBytes().Length);
                                        this.Com.Write(GetBuffFromPQ(false, buff[7]), 0, 10);
                                    }
                                    break;
                                case 11:
                                case 12:
                                    buff = new byte[len];
                                    this.Com.Read(buff, 0, len);
                                    if (buff[0] == 0xFE && buff[1] == 0xAA && buff[10] == 0x55)
                                    {
                                        this.Com.Write(GetBuffFromPQ(true, buff[8]), 0, 11);
                                    }
                                    break;
                                default:
                                    this.Com.DiscardInBuffer();
                                    break;
                            }
                        }
                        break;
                }
            }
        }
        //private void DataReadCom4(object sender, SerialDataReceivedEventArgs e)
        //{
        //    try
        //    {
        //        Com4.Read(realData, 0, readCount);
        //        if (realData[0] == 0xAA)
        //        {
        //            if (readCount == 10 && Temp_Out_SNByte != null)
        //            {
        //                if (Temp_Out_SNByte[6] != realData[7])//设置机型与实际机型不符
        //                {
        //                    Temp_Out_SNByte[6] = realData[7];
        //                    cMideaSnCode.Crc(Temp_Out_SNByte, cMideaSnCode.CrcList.PQ, ref  Temp_Out_SNByte[8]);
        //                }
        //            }
        //            readCount = 10;
        //            Com4.DiscardInBuffer();
        //            if (Temp_Out_SNByte != null)
        //            {
        //                Com4.Write(Temp_Out_SNByte, 0, Temp_Out_SNByte.Length);
        //            }
        //        }
        //        else
        //        {
        //            for (int i = 1; i < readCount; i++)
        //            {
        //                if (realData[i] == 0xAA)
        //                {
        //                    readCount = i;
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception exc)
        //    {
        //    }
        //}
        
    }
}
