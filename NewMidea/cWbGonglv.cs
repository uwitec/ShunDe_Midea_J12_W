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
            "cWbGonglv:���캯��\r\n"+
            "WbGonglvInit:��ʼ��ά��ģ��\r\n"+
            "WbGonglvRead:ά�����ݶ�ȡ";
        cStandarBoard mStandarBoard;
        SerialPort comPort;//�˿�
        /// <summary>
        /// LGPLCʹ�õĴ���
        /// </summary>
        public SerialPort ComPort
        {
            get { return comPort; }
            set { comPort = value; }
        }
        string errStr = "WbGonglv";//������Ϣ
        /// <summary>
        /// ���󷵻���Ϣ
        /// </summary>
        public string ErrStr
        {
            get { return errStr; }
            set { errStr = value; }
        }
        double curBase = 6;//�������б�
        /// <summary>
        /// �������б�
        /// </summary>
        public double CurBase
        {
            get { return curBase; }
            set { curBase = value; }
        }
        int timeOut = 400;//��ʱʱ��(ms)
        /// <summary>
        ///��д���ڳ�ʱʱ��,��λ(ms)
        /// </summary>
        public int TimeOut
        {
            get { return timeOut; }
            set { timeOut = value; }
        }
        byte WbGonglvAddress = 0;//LGPLC��ַ
        public bool mWbGonglvInit = false;//PLC��ʼ�����
        /// <summary>
        /// WbGonglv1���칹�캯��
        /// </summary>
        /// <param name="mComPort">����WbGonglv�Ĵ���</param>
        /// <param name="mAddress">WbGonglv�ĵ�ַ</param>
        /// <param name="mCurBase">�������б�,һ��Ϊ6</param>
        public cWbGonglv(SerialPort mComPort, byte mAddress, double mCurBase)
        {
            mStandarBoard = new cStandarBoard(mComPort, mAddress,timeOut);
            comPort = mComPort;
            WbGonglvAddress = mAddress;
            curBase = mCurBase;
        }
        /// <summary>
        /// WbGonglv��ʼ��
        /// </summary>
        /// <returns>bool,���س�ʼ���</returns>
        public bool WbGonglvInit()
        {
            if (cMain.isDebug)
            {
                mWbGonglvInit = true;
                return true;
            }
            byte[] WriteBuff = new byte[10];//��������
            byte[] ReadBuff = new byte[20];//��������
            int ReturnByte = 0;//��������
            bool IsReturn = false;//�Ƿ�ɹ�����
            bool IsTimeOut = false;//�Ƿ�ʱ
            DateTime NowTime = DateTime.Now;//��ǰʱ��
            TimeSpan ts;//ʱ���
           
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
                    if (comPort.BytesToRead >= 14)//�յ�����
                    {
                        ReturnByte = comPort.BytesToRead;
                        IsReturn = true;
                    }
                    ts = DateTime.Now - NowTime;
                    if (ts.TotalMilliseconds > timeOut)//ʱ�䳬ʱ
                    {
                        IsTimeOut = true;
                    }
                } while (!IsReturn && !IsTimeOut);
                if (!IsReturn && IsTimeOut)//��ʱ
                {
                    if (ErrStr.IndexOf("���������ѳ�ʱ") < 0)
                    {
                        ErrStr = ErrStr + DateTime.Now.ToString() + ",mWbGonglvInit," + "��ʼʧ��,���������ѳ�ʱ" + (char)13 + (char)10;
                    }
                    cMain.WriteErrorToLog(ReturnByte.ToString());
                    return false;
                }
                else
                {
                    comPort.Read(ReadBuff, 0, ReturnByte);
                    if ((ReadBuff[0] != WriteBuff[0]) || (ReadBuff[1] != WriteBuff[1]))//���ݼ���ʧ��
                    {
                        if (ErrStr.IndexOf("�������ݴ���") < 0)
                        {
                            ErrStr = ErrStr + DateTime.Now.ToString() + ",mWbGonglvInit," + ":��ʼʧ��,�������ݴ���" + (char)13 + (char)10;
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
        /// ��ȡWbGonglvģ������
        /// </summary>
        /// <param name="ReturnBuff">���ض�ȡ9Ԫ������,0,1,2Ϊ��ѹ,3,4,5Ϊ����,6,7,8Ϊ����</param>
        /// <returns>���ض�ȡ�����Ƿ�ɹ�</returns>
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
            byte[] WriteBuff = new byte[10];//��������
            byte[] ReadBuff = new byte[16];//��������
            int ReturnByte = 0;//��������
            bool IsReturn = false;//�Ƿ�ɹ�����
            bool IsTimeOut = false;//�Ƿ�ʱ
            DateTime NowTime;//��ǰʱ��
            TimeSpan ts;//ʱ���
            

            
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
                comPort.DiscardInBuffer();//ˢ�´���
                comPort.Write(WriteBuff, 0, 6);
                NowTime = DateTime.Now;
                do
                {
                    //System.Windows.Forms.Application.DoEvents();
                    System.Threading.Thread.Sleep(100);

                    if (comPort.BytesToRead >= 14)//�յ�����
                    {
                        ReturnByte = comPort.BytesToRead;
                        IsReturn = true;
                    }
                    ts = DateTime.Now - NowTime;
                    if (ts.TotalMilliseconds > timeOut)//ʱ�䳬ʱ
                    {
                        IsTimeOut = true;
                    }
                } while (!IsReturn && !IsTimeOut);
                if (!IsReturn && IsTimeOut)//��ʱ
                {
                    if (ErrStr.IndexOf("���������ѳ�ʱ") < 0)
                    {
                        ErrStr = ErrStr + DateTime.Now.ToString() + ",WbGonglvRead," + ":��ȡʧ��,���������ѳ�ʱ" + (char)13 + (char)10;
                    }
                    returnValue = false;
                    return returnValue;
                }
                ReadBuff = new byte[ReturnByte];
                comPort.Read(ReadBuff, 0, ReturnByte);
                if ((ReadBuff[0] != WriteBuff[0]) || (ReadBuff[1] != WriteBuff[1]))//���ݼ���ʧ��
                {
                    //string tempStr = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{3},{14},{15},{16},{17},{18},{19},{20}",
                    //    ReadBuff[0], ReadBuff[1], ReadBuff[2], ReadBuff[3], ReadBuff[4], ReadBuff[5], ReadBuff[6], ReadBuff[7], ReadBuff[8],
                    //    ReadBuff[9], ReadBuff[10], ReadBuff[11], ReadBuff[12], ReadBuff[13], ReadBuff[14], ReadBuff[15], ReadBuff[16], ReadBuff[17]
                    //    , ReadBuff[18], ReadBuff[19], ReadBuff[20]);
                    //cMain.WriteErrorToLog(tempStr);
                    if (ErrStr.IndexOf("�������ݴ���") < 0)
                    {
                        ErrStr = ErrStr + DateTime.Now.ToString() + ",WbGonglvRead," + ":��ȡʧ��,�������ݴ���" + (char)13 + (char)10;
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
                        if ((ReadBuff[i * 2 + 7] >> 7) == 1)//����Ǹ���
                        {
                            ReturnBuff1[i] = -(ReturnBuff1[i] & 0xFFFF + 1);
                        }
                        mReturnBuff[i] = ReturnBuff1[i];
                    }
                    //for (i = 0; i < ReturnBuff1.Length; i++)
                    //{
                    //    ReturnBuff1[i] = (ReadBuff[i * 2 + 7] & 0xFF) * 256 + (ReadBuff[i * 2 + 6] & 0xFF);
                    //    if ((ReadBuff[i * 2 + 7] >> 7) == 1)//����Ǹ���
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
                ReturnBuff[0] = mReturnBuff[2] * 300 / (double)10000;//��ѹ
                ReturnBuff[1] = mReturnBuff[3] * 300 / (double)10000;//��ѹ
                ReturnBuff[2] = mReturnBuff[4] * 300 / (double)10000;//��ѹ
                ReturnBuff[3] = mReturnBuff[5] * 30 / (double)10000;//����
                ReturnBuff[4] = mReturnBuff[6] * 30 / (double)10000;//��ѹ
                ReturnBuff[5] = mReturnBuff[7] * 30 / (double)10000;//��ѹ

                ReturnBuff[6] = mReturnBuff[0] * 9000 *3/ (double)10000;//����


                //ReturnBuff[7] = mReturnBuff[9] * 9000 *3 / (double)10000;//����


                //ReturnBuff[8] = mReturnBuff[10] * 9000 *3 / (double)10000;//����
                

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
        /// ��ȡ2010ģ������
        /// </summary>
        /// <param name="ReturnBuff">���ض�ȡ9Ԫ������,0,1,2Ϊ��ѹ,3,4,5Ϊ����,6,7,8Ϊ����</param>
        /// <returns>���ض�ȡ�����Ƿ�ɹ�</returns>
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
            byte[] WriteBuff = new byte[10];//��������
            byte[] ReadBuff = new byte[16];//��������
            int ReturnByte = 0;//��������
            bool IsReturn = false;//�Ƿ�ɹ�����
            bool IsTimeOut = false;//�Ƿ�ʱ
            DateTime NowTime;//��ǰʱ��
            TimeSpan ts;//ʱ���
            


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
                    if (comPort.BytesToRead >= 14)//�յ�����
                    {
                        ReturnByte = comPort.BytesToRead;
                        IsReturn = true;
                    }
                    ts = DateTime.Now - NowTime;
                    if (ts.TotalMilliseconds > timeOut)//ʱ�䳬ʱ
                    {
                        IsTimeOut = true;
                    }
                } while (!IsReturn && !IsTimeOut);
                if (!IsReturn && IsTimeOut)//��ʱ
                {
                    if (ErrStr.IndexOf("���������ѳ�ʱ") < 0)
                    {
                        ErrStr = ErrStr + DateTime.Now.ToString() + ",WbGonglvRead," + ":��ȡʧ��,���������ѳ�ʱ" + (char)13 + (char)10;
                    }
                    returnValue = false;
                    return returnValue;
                }
                comPort.Read(ReadBuff, 0, ReturnByte);
                if ((ReadBuff[0] != WriteBuff[0]) || (ReadBuff[1] != WriteBuff[1]))//���ݼ���ʧ��
                {
                    //string tempStr = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{3},{14},{15},{16},{17},{18},{19},{20}",
                    //    ReadBuff[0], ReadBuff[1], ReadBuff[2], ReadBuff[3], ReadBuff[4], ReadBuff[5], ReadBuff[6], ReadBuff[7], ReadBuff[8],
                    //    ReadBuff[9], ReadBuff[10], ReadBuff[11], ReadBuff[12], ReadBuff[13], ReadBuff[14], ReadBuff[15], ReadBuff[16], ReadBuff[17]
                    //    , ReadBuff[18], ReadBuff[19], ReadBuff[20]);
                    //cMain.WriteErrorToLog(tempStr);
                    if (ErrStr.IndexOf("�������ݴ���") < 0)
                    {
                        ErrStr = ErrStr + DateTime.Now.ToString() + ",WbGonglvRead," + ":��ȡʧ��,�������ݴ���" + (char)13 + (char)10;
                    }
                    returnValue = false;
                    return returnValue;
                }
                else
                {
                    for (i = 0; i < ReturnBuff1.Length; i++)
                    {
                        ReturnBuff1[i] = (ReadBuff[i * 2 + 7] & 0xFF) * 256 + (ReadBuff[i * 2 + 6] & 0xFF);
                        //if ((ReadBuff[i * 2 + 4] >> 7) == 1)//����Ǹ���
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
                ReturnBuff[0] = mReturnBuff[2] * 300 / (double)10000;//��ѹ
                ReturnBuff[1] = 0;
                ReturnBuff[2] = 0;
                ReturnBuff[3] = mReturnBuff[3] * 25 / (double)10000;//����
                ReturnBuff[4] = 0;
                ReturnBuff[5] = 0;

                ReturnBuff[6] = mReturnBuff[0] * 7500 / (double)10000;//����


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
        /// ��ȡ2010ģ������
        /// </summary>
        /// <param name="ReturnBuff">���ض�ȡ9Ԫ������,0,1,2Ϊ��ѹ,3,4,5Ϊ����,6,7,8Ϊ����</param>
        /// <returns>���ض�ȡ�����Ƿ�ɹ�</returns>
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
            byte[] WriteBuff = new byte[10];//��������
            byte[] ReadBuff = new byte[16];//��������

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
                comPort.DiscardInBuffer();//ˢ�´���
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
