using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Threading;
namespace NewMideaProgram
{
    public class cHuaWeiMachine
    {
        /// <summary>
        /// 使用的串口
        /// </summary>
        private SerialPort com;

        public SerialPort Com
        {
            get { return com; }
            set { com = value; }
        }
        /// <summary>
        /// 错误信息
        /// </summary>
        private string error = "";

        public string Error
        {
            get { return error; }
            set { error = value; }
        }
        /// <summary>
        /// 环境温度
        /// </summary>
        private float huangJingWenDu = 0;

        public float HuangJingWenDu
        {
            get { return huangJingWenDu; }
            set { huangJingWenDu = value; }
        }
        /// <summary>
        /// 排气温度
        /// </summary>
        private float paiQiWenDu = 0;

        public float PaiQiWenDu
        {
            get { return paiQiWenDu; }
            set { paiQiWenDu = value; }
        }
        /// <summary>
        /// 吸气温度
        /// </summary>
        private float xiQiWenDu = 0;

        public float XiQiWenDu
        {
            get { return xiQiWenDu; }
            set { xiQiWenDu = value; }
        }
        /// <summary>
        /// 排气压力
        /// </summary>
        private float paiQiYaLi = 0;

        public float PaiQiYaLi
        {
            get { return paiQiYaLi; }
            set { paiQiYaLi = value; }
        }
        /// <summary>
        /// 高压开关
        /// </summary>
        private float gaoYaKaiGuan = 0;

        public float GaoYaKaiGuan
        {
            get { return gaoYaKaiGuan; }
            set { gaoYaKaiGuan = value; }
        }
        /// <summary>
        /// 低压开关
        /// </summary>
        private float diYaKaiGuan = 0;

        public float DiYaKaiGuan
        {
            get { return diYaKaiGuan; }
            set { diYaKaiGuan = value; }
        }
        bool start = false;

        public bool Start
        {
            get { return start; }
            set { start = value; }
        }
        string readBarcode = "";

        public string Barcode
        {
            get { return readBarcode; }
        }

        cStandarBoard fengJi;
        cStandarBoard yaSuoJi;
        Thread thRead;

        string barcode = "";
        bool writeBarcode = false;

        bool writeStart = false;


        int fengJiErrorCount = 0;
        int yaSuoJiErrorCount = 0;
        bool exit = false;
        public cHuaWeiMachine(string com)
        {
            this.Com = new SerialPort(com, 9600, Parity.None, 8, StopBits.One);
        }
        public void Init()
        {
            try
            {
                this.Com.BaudRate = 9600;
                this.Com.Parity = Parity.None;
                this.Com.DataBits = 8;
                this.Com.StopBits = StopBits.One;

                this.Com.Open();
                exit = false;
                start = false;
                readBarcode = "";
                yaSuoJi = new cStandarBoard(Com, 1, 400);
                fengJi = new cStandarBoard(Com, 2, 400);

                thRead = new Thread(new ThreadStart(Flush));
                thRead.IsBackground = true;
                thRead.Start();
            }
            catch (Exception e)
            {
                Error = e.Message;
            }
        }
        public void Close()
        {
            start = false;
            exit = true;
            readBarcode = "";
            if (thRead != null)
            {
                thRead.Abort();
                thRead.Join(100);
                thRead = null;
            }
            if (this.Com != null && this.Com.IsOpen)
            {
                this.Com.Close();
            }
        }
        public void WriteBar(string barcode)
        {
            barcode = barcode.Trim();
            if ((barcode.Length % 2) == 1)
            {
                barcode = string.Format("{0} ", barcode);
            }
            if (barcode.Length > 30)
            {
                barcode = barcode.Substring(0, 30);
            }
            this.barcode = barcode;
            writeBarcode = true;
        }
        public void WriteStart()
        {
            writeStart = true;
        }
        public void Flush()
        {
            long[] tmpValue;
            byte[] tmpBuff;
            while (!exit)
            {
                if (writeBarcode)
                {
                    if (yaSuoJi.StandarBoardWritePoint(2926, barcode.Length / 2, Encoding.ASCII.GetBytes(barcode)))
                    {
                        writeBarcode = false;
                        Thread.Sleep(50);
                    }
                    else
                    {
                        Error = yaSuoJi.ErrStr;
                    }
                }
                tmpBuff = new byte[32];
                if (yaSuoJi.StandarBoardRead(2926, 16,ref tmpBuff))
                {
                    readBarcode = Encoding.ASCII.GetString(tmpBuff).Trim();
                }
                if (writeStart)
                {
                    if (yaSuoJi.StandarBoardWritePoint(1929, 1, 6))//开始整机快检
                    {
                        writeStart = false;
                        Thread.Sleep(50);
                    }
                    else
                    {
                        Error = yaSuoJi.ErrStr;
                    }
                }
                tmpValue = new long[1];
                if (yaSuoJi.StandarBoardRead(1929, 1, ref tmpValue))
                {
                    if (tmpValue[0] == 1)
                    {
                        start = true;
                    }
                    else
                    {
                        start = false;
                    }
                    Thread.Sleep(50);
                }
                tmpValue = new long[1];
                if (fengJi.StandarBoardRead(204, 1, ref tmpValue))
                {
                    HuangJingWenDu = tmpValue[0] / 10.0f;
                    fengJiErrorCount = 0;
                }
                else
                {
                    fengJiErrorCount++;
                    if (fengJiErrorCount > 3)
                    {
                        fengJiErrorCount = 3;
                        HuangJingWenDu = -99;
                    }
                }
                Thread.Sleep(50);
                tmpValue = new long[8];
                if (yaSuoJi.StandarBoardRead(2971, 8, ref tmpValue))
                {
                    PaiQiWenDu = tmpValue[0] / 10.0f;
                    XiQiWenDu = tmpValue[1] / 10.0f;
                    PaiQiYaLi = tmpValue[2] / 100.0f;
                    GaoYaKaiGuan = tmpValue[5];
                    DiYaKaiGuan = tmpValue[6];
                    yaSuoJiErrorCount = 0;
                }
                else
                {
                    yaSuoJiErrorCount++;
                    if (yaSuoJiErrorCount > 3)
                    {
                        yaSuoJiErrorCount = 3;
                        PaiQiWenDu = -99;
                        PaiQiYaLi = -99;
                        XiQiWenDu = -99;
                    }
                }
                Thread.Sleep(100);
            }
        }
    }
}
