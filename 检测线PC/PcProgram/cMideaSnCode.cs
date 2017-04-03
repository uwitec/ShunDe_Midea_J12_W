using System;
using System.Collections.Generic;
using System.Text;
namespace System
{
    class cMideaSnCode
    {
        public enum CrcList
        {
            NewSn600,
            OldSn600,
            TianHua1200,
            ShuMa1023,
            PQ
        }
        private static string CurrentSn(byte[] ByteSn, ref string OldSn, CrcList mCrcList)
        {
            byte lo = 0, hi = 0;
            Crc(ByteSn, mCrcList, ref lo, ref hi);
            switch (mCrcList)
            {
                case CrcList.NewSn600:
                    if (ByteSn.Length >= 20)
                    {
                        ByteSn[18] = lo;
                        ByteSn[19] = hi;
                        OldSn = "";
                        for (int i = 0; i < 20; i++)
                        {
                            OldSn = OldSn + string.Format("{0:X2}", ByteSn[i]);
                        }
                    }
                    break;
                case CrcList.OldSn600:
                    if (ByteSn.Length >= 18)
                    {
                        ByteSn[16] = lo;
                        OldSn = "";
                        for (int i = 0; i < 18; i++)
                        {
                            OldSn = OldSn + string.Format("{0:X2}", ByteSn[i]);
                        }
                    }
                    break;
            }
            return OldSn;
        }
        /// <summary>
        /// 修改当前指令中的频率
        /// </summary>
        /// <param name="Hz"></param>
        /// <param name="OldSn"></param>
        /// <returns></returns>
        public static string CurrentSnPinLv(byte Hz, ref string OldSn)
        {
            OldSn = OldSn.Trim();
            int LenSn = OldSn.Length;
            byte[] bb = new byte[(int)Math.Ceiling(OldSn.Length / 2.000)];
            for (int i = 0; i < OldSn.Length; i = i + 2)
            {
                bb[i / 2] = Convert.ToByte(OldSn.Substring(i, 2), 16);
            }
            switch (LenSn)
            {
                case 36:
                    bb[6] = Hz;
                    CurrentSn(bb, ref OldSn, CrcList.OldSn600);
                    break;
                case 40:
                    bb[7] = Hz;
                    CurrentSn(bb, ref OldSn, CrcList.NewSn600);
                    break;
            }
            return OldSn;
        }
        /// <summary>
        /// 修改当前指令中的外风机开关
        /// </summary>
        /// <param name="FengJi"></param>
        /// <param name="OldSn"></param>
        /// <returns></returns>
        public static string CurrentSnFengJi(byte FengJi, ref string OldSn)
        {
            OldSn = OldSn.Trim();
            int LenSn = OldSn.Length;
            byte[] bb = new byte[(int)Math.Ceiling(OldSn.Length / 2.000)];
            for (int i = 0; i < OldSn.Length; i = i + 2)
            {
                bb[i / 2] = Convert.ToByte(OldSn.Substring(i, 2), 16);
            }
            switch (LenSn)
            {
                case 36:
                    bb[7] = (byte)(((bb[7] & 0xF1) ^ FengJi) & 0xFF);
                    CurrentSn(bb, ref OldSn, CrcList.OldSn600);
                    break;
                case 40:
                    bb[8] = (byte)(((bb[8] & 0xF1) ^ FengJi) & 0xFF);
                    CurrentSn(bb, ref OldSn, CrcList.NewSn600);
                    break;
            }
            return OldSn;
        }
        /// <summary>
        /// 获取当前指令中的频率
        /// </summary>
        /// <param name="OldSn"></param>
        /// <returns></returns>
        public static byte CurrentPinLv(string OldSn)
        {
            OldSn = OldSn.Trim();
            int LenSn = OldSn.Length;
            byte PinLv = 0;
            byte[] bb = new byte[(int)Math.Ceiling(OldSn.Length / 2.000)];
            for (int i = 0; i < OldSn.Length; i = i + 2)
            {
                bb[i / 2] = Convert.ToByte(OldSn.Substring(i, 2), 16);
            }
            switch (LenSn)
            {
                case 36:
                    PinLv = bb[6];
                    break;
                case 40:
                    PinLv = bb[7];
                    break;
            }
            return PinLv;
        }
        /// <summary>
        /// 计算美的指令校验
        /// </summary>
        /// <param name="mByte">传入美的指令,加入前缀(oldSn的mByte[0]应为AA,newSn的mByte[0]应为A0)</param>
        /// <param name="mCrcList">计算SN类型</param>
        /// <param name="CrcLo">校验的低字节</param>
        /// <param name="CrcHi">校验的高字节,部分指令没有</param>
        /// <returns>返回计算是否成功</returns>
        public static bool Crc(byte[] mByte, CrcList mCrcList, ref byte CrcLo)
        {
            byte b = 0;
            return Crc(mByte, mCrcList, ref CrcLo, ref b);
        }
        /// <summary>
        /// 计算美的指令校验
        /// </summary>
        /// <param name="mByte">传入美的指令,加入前缀(oldSn的mByte[0]应为AA,newSn的mByte[0]应为A0)</param>
        /// <param name="mCrcList">计算SN类型</param>
        /// <param name="CrcLo">校验的低字节</param>
        /// <param name="CrcHi">校验的高字节,部分指令没有</param>
        /// <returns>返回计算是否成功</returns>
        public static bool Crc(byte[] mByte, CrcList mCrcList, ref byte CrcLo, ref byte CrcHi)
        {
            bool isOk = false;
            CrcLo = 0;
            CrcHi = 0;
            int sum = 0;
            int start = 0;
            switch (mCrcList)
            {
                case CrcList.OldSn600:
                    if (mByte[0] == 0xAA)
                    {
                        start = 1;
                    }
                    for (int i = start; i <= 14 + start; i++)
                    {
                        sum = sum + mByte[i];
                    }
                    CrcLo = (byte)((((sum & 0xFF) ^ 0xFF) + 1) & 0xFF);
                    break;
                case CrcList.NewSn600:
                    for (int i = 0; i <= 17; i++)
                    {
                        sum = sum + mByte[i];
                    }
                    CrcLo = (byte)((((sum & 0xFFFF) ^ 0xFFFF) + 1) & 0xFF);
                    CrcHi = (byte)((((sum & 0xFFFF) ^ 0xFFFF) + 1) >> 8);
                    break;
                case CrcList.TianHua1200:
                    for (int i = 1; i <= 5; i++)
                    {
                        sum = sum + mByte[i];
                    }
                    CrcLo = (byte)((((sum & 0xFF) ^ 0xFF) + 1) & 0xFF);
                    break;
                case CrcList.ShuMa1023:
                    for (int i = 0; i <= 4; i++)
                    {
                        sum = sum + mByte[i];
                    }
                    CrcLo = (byte)((((sum & 0xFF) ^ 0xFF) + 1) & 0xFF);
                    break;
                case CrcList.PQ:
                    if (mByte[0] == 0xAA)
                    {
                        start = 1;
                    }
                    for (int i = start; i <= 6 + start; i++)
                    {
                        sum = sum + mByte[i];
                    }
                    CrcLo = (byte)((((sum & 0xFF) ^ 0xFF) + 1) & 0xFF);
                    break;
            }
            return isOk;
        }

        internal static void Crc()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
