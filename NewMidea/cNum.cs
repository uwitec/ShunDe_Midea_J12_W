using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
namespace System
{
    public static class Num
    {
        /// <summary>
        /// 获取表格中整形值
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static int ToInt(this DataGridView dt, int row, int col)
        {
            if (row < 0 || col < 0 || dt == null || dt.Rows.Count <= row || dt.Columns.Count <= col || dt.Rows[row].Cells[col].Value == null)
            {
                return 0;
            }
            return dt.Rows[row].Cells[col].Value.ToString().ToInt();
        }
        /// <summary>
        /// 获取表格中的浮点数
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static float ToFloat(this DataGridView dt, int row, int col)
        {
            if (row < 0 || col < 0 || dt == null || dt.Rows.Count <= row || dt.Columns.Count <= col || dt.Rows[row].Cells[col].Value == null)
            {
                return 0;
            }
            return dt.Rows[row].Cells[col].Value.ToString().ToFloat();
        }
        /// <summary>
        /// 获取表格中双精度浮点数
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static double ToDouble(this DataGridView dt, int row, int col)
        {
            if (row < 0 || col < 0 || dt == null || dt.Rows.Count <= row || dt.Columns.Count <= col || dt.Rows[row].Cells[col].Value == null)
            {
                return 0;
            }
            return dt.Rows[row].Cells[col].Value.ToString().ToDouble();
        }
        /// <summary>
        /// 获取表格中布尔值
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static bool ToBool(this DataGridView dt, int row, int col)
        {
            if (row < 0 || col < 0 || dt == null || dt.Rows.Count <= row || dt.Columns.Count <= col || dt.Rows[row].Cells[col].Value == null)
            {
                return false;
            }
            return dt.Rows[row].Cells[col].Value.ToString().ToBool();
        }
        /// <summary>
        /// 获取表格中的时间值
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this DataGridView dt, int row, int col)
        {
            if (row < 0 || col < 0 || dt == null || dt.Rows.Count <= row || dt.Columns.Count <= col || dt.Rows[row].Cells[col].Value == null)
            {
                return DateTime.Now;
            }
            return dt.Rows[row].Cells[col].Value.ToString().ToDateTime();
        }
        /// <summary>
        /// 获取表格中的字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static string ToString(this DataGridView dt, int row, int col)
        {
            if (row < 0 || col < 0 || dt == null || dt.Rows.Count <= row || dt.Columns.Count <= col || dt.Rows[row].Cells[col].Value == null)
            {
                return "";
            }
            return dt.Rows[row].Cells[col].Value.ToString();
        }
        /// <summary>
        /// 将字符串转化为对应的数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] ToBytes(this string str)
        {
            if (str == null || str == "")
            {
                return null;
            }
            if ((str.Length % 2) == 1)
            {
                str = string.Format("{0}0", str);
            }
            str = str.Replace("O", "0");//将写错的字母"0"转化为数字"0"
            str = str.Replace("o", "0");
            byte[] result = new byte[str.Length / 2];
            try
            {
                for (int i = 0, j = 0; i < result.Length && j < str.Length; i++, j = j + 2)
                {
                    result[i] = Convert.ToByte(str.Substring(j, 2), 16);
                }
            }
            catch { }
            return result;
        }
        /// <summary>
        /// 将数组转化为字符串输出
        /// </summary>
        /// <param name="buff"></param>
        /// <returns></returns>
        public static string ToHexString(this byte[] buff)
        {
            if (buff == null)
            {
                return "";
            }
            string result = "";
            for (int i = 0; i < buff.Length; i++)
            {
                result = string.Format("{0}{1:X2}", result, buff[i]);
            }
            return result;
        }
        /// <summary>
        /// 获取字符串的绘画长度,即ASCII算一个字符,其他算二个字符
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int LenPaint(this string value)
        {
            if (value == null)
            {
                return 0;
            }
            return Encoding.GetEncoding("GB2312").GetBytes(value).Length;
        }
        /// <summary>
        /// 跨线程匿名委托调用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="t"></param>
        public static void CrossThreadCalls(this System.Windows.Forms.Form sender, Threading.ThreadStart t)
        {
            if (sender == null)
            {
                return;
            }
            lock (sender)
            {
                if (sender.InvokeRequired)
                {
                    sender.Invoke(t, null);
                }
                else
                {
                    t();
                }
            }
        }
        /// <summary>
        /// 获取表格中整形值
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static int ToInt(this DataTable dt, int row, int col)
        {
            if (row < 0 || col < 0 || dt == null || dt.Rows.Count <= row || dt.Columns.Count <= col || dt.Rows[row][col] == null)
            {
                return 0;
            }
            return dt.Rows[row][col].ToString().ToInt();
        }
        /// <summary>
        /// 获取表格中的浮点数
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static float ToFloat(this DataTable dt, int row, int col)
        {
            if (row < 0 || col < 0 || dt == null || dt.Rows.Count <= row || dt.Columns.Count <= col || dt.Rows[row][col] == null)
            {
                return 0;
            }
            return dt.Rows[row][col].ToString().ToFloat();
        }
        /// <summary>
        /// 获取表格中双精度浮点数
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static double ToDouble(this DataTable dt, int row, int col)
        {
            if (row < 0 || col < 0 || dt == null || dt.Rows.Count <= row || dt.Columns.Count <= col || dt.Rows[row][col] == null)
            {
                return 0;
            }
            return dt.Rows[row][col].ToString().ToDouble();
        }
        /// <summary>
        /// 获取表格中布尔值
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static bool ToBool(this DataTable dt, int row, int col)
        {
            if (row < 0 || col < 0 || dt == null || dt.Rows.Count <= row || dt.Columns.Count <= col || dt.Rows[row][col] == null)
            {
                return false;
            }
            return dt.Rows[row][col].ToString().ToBool();
        }
        /// <summary>
        /// 获取表格中的时间值
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this DataTable dt, int row, int col)
        {
            if (row < 0 || col < 0 || dt == null || dt.Rows.Count <= row || dt.Columns.Count <= col || dt.Rows[row][col] == null)
            {
                return DateTime.Now;
            }
            return dt.Rows[row][col].ToString().ToDateTime();
        }
        /// <summary>
        /// 获取表格中的字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static string ToString(this DataTable dt, int row, int col)
        {
            if (row < 0 || col < 0 || dt == null || dt.Rows.Count <= row || dt.Columns.Count <= col || dt.Rows[row][col] == null)
            {
                return "";
            }
            return dt.Rows[row][col].ToString();
        }
        /// <summary>
        /// 字符串转化为整形
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt(this string value)
        {
            if (value == string.Empty)
            {
                return 0;
            }
            int result = 0;
            try
            {
                result = int.Parse(value.Trim());
            }
            catch { }
            return result;
        }
        /// <summary>
        /// 字符串转化为浮点数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float ToFloat(this string value)
        {
            if (value == string.Empty)
            {
                return 0;
            }
            float result = 0;
            try
            {
                result = float.Parse(value.Trim());
            }
            catch
            { }
            return result;
        }
        public static float ToSingle(this string value)
        {
            return value.ToFloat();
        }
        /// <summary>
        /// 字符串转化为双精度浮点数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double ToDouble(this string value)
        {
            if (value == string.Empty)
            {
                return 0;
            }
            double result = 0;
            try
            {
                result = double.Parse(value.Trim());
            }
            catch { }
            return result;
        }
        /// <summary>
        /// 字符串转化为日期
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ToBool(this string value)
        {
            if (value == string.Empty)
            {
                return false;
            }
            bool result = false;
            try
            {
                result = bool.Parse(value.Trim());
            }
            catch
            { }
            return result;
        }
        /// <summary>
        /// 字符串转化为日期
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string value)
        {
            if (value == string.Empty)
            {
                return DateTime.Now;
            }
            DateTime result = DateTime.Now;
            try
            {
                result = DateTime.Parse(value.Trim());
            }
            catch
            { }
            return result;
        }
        /// <summary>
        /// 计算CRC校验
        /// </summary>
        /// <param name="mByte">要计算CRC的 buff数组</param>
        /// <param name="mLen">要计算CRC的 buff长度</param>
        /// <param name="CrcLo">CRC计算后返回低字节</param>
        /// <param name="CrcHi">CRC计算后返回高字节</param>//CRC校验说明
        public static void CRC_16(byte[] mByte, int mLen, ref byte CrcLo, ref byte CrcHi)//计算CRC校验
        {
            if (mLen <= 0)
            {
                return;
            }
            CrcHi = 0;
            CrcLo = 0;
            int i, j;
            long maa = 0xFFFF;
            long mbb = 0;
            for (i = 0; i < mLen; i++)
            {
                CrcHi = (byte)((maa >> 8) & 0xFF);
                CrcLo = (byte)((maa) & 0xFF);
                maa = (CrcHi << 8) & 0xFF00;
                maa = maa + (long)((CrcLo ^ mByte[i]) & 0xFF);
                for (j = 0; j < 8; j++)
                {
                    mbb = 0;
                    mbb = maa & 0x1;
                    maa = (maa >> 1) & 0x7FFF;
                    if (mbb != 0)
                    {
                        maa = (maa ^ 0xA001) & 0xFFFF;
                    }

                }

            }
            CrcLo = (byte)((byte)maa & (byte)0xFF);
            CrcHi = (byte)((byte)(maa >> 8) & (byte)0xFF);
        }
        public static string ToString(object num)
        {
            if (num == null)
            {
                return "";
            }
            return num.ToString();
        }
        /// <summary>
        /// 将指定字符串转化成单精度数据
        /// </summary>
        /// <param name="Num">object,要转化的字符</param>
        /// <returns>Single,转化后的单精度数据</returns>
        public static float ToFloat(object num)
        {
            if (num == null)
            {
                return 0;
            }
            return num.ToString().ToFloat();
        }

        /// <summary>
        /// 将指定字符串转化成整形数据
        /// </summary>
        /// <param name="Num">object,要转化的字符</param>
        /// <returns>int,转化后的整形数据</returns>
        public static int ToInt(object num)
        {
            if (num == null)
            {
                return 0;
            }
            return num.ToString().ToInt();
        }
        /// <summary>
        /// 将指定字符串转化成双精度数据
        /// </summary>
        /// <param name="Num">object,要转化的字符</param>
        /// <returns>double,转化后的又精度数据</returns>
        public static double ToDouble(object num)
        {
            if (num == null)
            {
                return 0;
            }
            return num.ToString().ToDouble();
        }
        /// <summary>
        /// 转化为布尔类型
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static bool ToBool(object num)
        {
            if (num == null)
            {
                return false;
            }
            return num.ToString().ToBool();
        }
        /// <summary>
        /// 返回三个数据中最大数据的序号
        /// </summary>
        /// <param name="Num1">int,数据一</param>
        /// <param name="Num2">int,数据二</param>
        /// <param name="Num3">int,数据三</param>
        /// <returns>int,数据最大的序号</returns>
        public static int IndexMax(int Num1, int Num2, int Num3)
        {
            return IndexMax((double)Num1, (double)Num2, (double)Num3);
        }
        /// <summary>
        /// 返回三个数据中最大数据的序号
        /// </summary>
        /// <param name="Num1">int,数据一</param>
        /// <param name="Num2">int,数据二</param>
        /// <param name="Num3">int,数据三</param>
        /// <returns>int,数据最大的序号</returns>
        public static int IndexMax(double Num1, double Num2, double Num3)
        {
            int returnValue = 0;
            if (Num1 >= Num2 && Num1 >= Num3)
            {
                returnValue = 0;
            }
            if (Num2 >= Num1 && Num2 >= Num3)
            {
                returnValue = 1;
            }
            if (Num3 >= Num1 && Num3 >= Num2)
            {
                returnValue = 2;
            }
            return returnValue;
        }
        /// <summary>
        /// 交换数值
        /// </summary>
        /// <param name="Num1"></param>
        /// <param name="Num2"></param>
        public static void SwitchNum(ref double Num1, ref double Num2)
        {
            if (Num1.Equals(Num2))
            {
                return;
            }
            Num1 = Num1 + Num2;
            Num2 = Num1 - Num2;
            Num1 = Num1 - Num2;
        }
        /// <summary>
        /// 返回三个数据中最小数据的序号
        /// </summary>
        /// <param name="Num1">double,数据一</param>
        /// <param name="Num2">double,数据二</param>
        /// <param name="Num3">double,数据三</param>
        /// <returns>int,数据最小的序号</returns>
        public static int IndexMin(double Num1, double Num2, double Num3)
        {
            int returnValue = 0;
            if (Num1 <= Num2 && Num1 <= Num3)
            {
                returnValue = 0;
            }
            if (Num2 <= Num1 && Num2 <= Num3)
            {
                returnValue = 1;
            }
            if (Num3 <= Num1 && Num3 <= Num2)
            {
                returnValue = 2;
            }
            return returnValue;
        }
        /// <summary>
        /// 取随机数
        /// </summary>
        /// <returns></returns>
        public static double Rand()
        {
            Guid g = new Guid();
            byte[] buff = g.ToByteArray();
            Random r = new Random((buff[0] << 24) + (buff[3] << 16) + (buff[7] << 8 + buff[12]));
            return r.NextDouble();
        }
        /// <summary>
        /// 将字符串转化为字节数组
        /// </summary>
        /// <param name="value">string,要转化的字符串</param>
        /// <returns>by[],转化后的字节数组</returns>
        public static byte[] GetHexByte(string value)
        {
            value = value.Trim();
            if ((value.Length % 2) != 0)
            {
                return null;
            }
            byte[] b = new byte[value.Length / 2];
            for (int i = 0; i < b.Length; i++)
            {
                b[i] = Convert.ToByte(value.Substring(i * 2, 2), 16);
            }
            return b;
        }
        /// <summary>
        /// 将已知的字节数组转化为字符串
        /// </summary>
        /// <param name="value">byte[],字节数组</param>
        /// <returns>string,转化后的字符串</returns>
        public static string GetHexString(byte[] value)
        {
            string s = "";
            for (int i = 0; i < value.Length; i++)
            {
                s = string.Format("{0}{1:X2}", s, value[i]);
            }
            return s;
        }
        public static bool[] Int2Bool(UInt32 value)
        {
            bool[] result = new bool[32];
            string tmpValue = Convert.ToString(value, 2).PadLeft(32, '0');
            for (int i = 0; i < result.Length; i++)
            {
                if (tmpValue.Substring(31 - i, 1) == "1")
                {
                    result[i] = true;
                }
                else
                {
                    result[i] = false;
                }
            }
            return result;
        }
        public static UInt32 ChangeHighAndLow(UInt32 value)
        {
            UInt32 result = 0;
            byte[] buff = new byte[4];
            buff[0] = (byte)((value >> 24) & 0xFF);
            buff[1] = (byte)((value >> 16) & 0xFF);
            buff[2] = (byte)((value >> 8) & 0xFF);
            buff[3] = (byte)((value >> 0) & 0xFF);
            result = (uint)((buff[3] << 24) + (buff[2] << 16) + (buff[1] << 8) + buff[0]);
            return result;
        }
        public static string trim(string s)
        {
            string result = "";
            byte[] b = Encoding.ASCII.GetBytes(s);
            int len = b.Length;
            for (int i = 0; i < len; i++)
            {
                if ((b[i] >= 0x30 && b[i] <= 0x39)
                    || (b[i] >= 65 && b[i] <= 90)
                    || (b[i] >= 97 && b[i] <= 122))
                {
                    result = result + Encoding.ASCII.GetString(b, i, 1);
                }
            }
            return result;
        }
        /// <summary>
        /// 将指定字符串转化成单精度数据
        /// </summary>
        /// <param name="Num">object,要转化的字符</param>
        /// <returns>Single,转化后的单精度数据</returns>
        [Obsolete]
        public static Single SingleParse(object Num)
        {
            Single s = 0;
            try
            {
                s = Single.Parse(Num.ToString());
            }
            catch
            { }
            return s;
        }
        [Obsolete]
        public static bool BoolParse(object Num)
        {
            bool b = false;
            try
            {
                b = bool.Parse(Num.ToString());
            }
            catch
            { }
            return b;
        }
        /// <summary>
        /// 将指定字符串转化成整形数据
        /// </summary>
        /// <param name="Num">object,要转化的字符</param>
        /// <returns>int,转化后的整形数据</returns>
        [Obsolete]
        public static int IntParse(object Num)
        {
            int s = 0;
            try
            {
                s = int.Parse(Num.ToString());
            }
            catch
            { }
            return s;
        }
        [Obsolete]
        public static string StringParse(object Num)
        {
            if (Num == null)
            {
                return "";
            }
            return Num.ToString();
        }

        /// <summary>
        /// 将指定字符串转化成双精度数据
        /// </summary>
        /// <param name="Num">object,要转化的字符</param>
        /// <returns>double,转化后的又精度数据</returns>
        [Obsolete]
        public static double DoubleParse(object Num)
        {
            double s = 0;
            try
            {
                s = double.Parse(Num.ToString());
            }
            catch
            { }
            return s;
        }
        [Obsolete]
        public static byte ByteParse(object Num)
        {
            byte b = 0;
            try
            {
                b = byte.Parse(Num.ToString());
            }
            catch { }
            return b;
        }
        [Obsolete]
        public static long LongParse(object Num)
        {
            long l = 0;
            try
            {
                l = long.Parse(Num.ToString());
            }
            catch { }
            return l;
        }
        /// <summary>
        /// 返回两个数据的最大值
        /// </summary>
        /// <param name="Num1">double,数据一</param>
        /// <param name="Num2">double,数据二</param>
        /// <returns>double,数据的最大值</returns>
        public static double DoubleMax(double Num1, double Num2)
        {
            return Math.Max(Num1, Num2);
        }
        public static double DoubleMax(double Num1, double Num2, double Num3)
        {
            return DoubleMax(DoubleMax(Num1, Num2), Num3);
        }
        public static double DoubleMin(double Num1, double Num2)
        {
            return Math.Min(Num1, Num2);
        }
        public static double DoubleMin(double Num1, double Num2, double Num3)
        {
            return DoubleMin(DoubleMin(Num1, Num2), Num3);
        }
        [Obsolete]
        public static byte ByteParseFromHex(string data)
        {
            byte returnValue = 0;
            try
            {
                returnValue = Convert.ToByte(data, 16);
            }
            catch (Exception exc)
            {
            }
            return returnValue;
        }
        [Obsolete]
        /// <summary>
        /// 十六进制数据转化为十进制数据
        /// </summary>
        /// <param name="data">要转化的十六进制数据</param>
        /// <returns>转化后的字节</returns>
        public static byte ByteParseFromHex(object data)
        {
            byte returnData = 0;
            string tempStr = data.ToString();
            if (tempStr.Length < 3)
            {
                for (int i = 0; i < tempStr.Length; i++)
                {
                    string temp = tempStr.Substring(i, 1);
                    switch (temp)
                    {
                        case "A":
                            returnData = (byte)(returnData * 16 + 10);
                            break;
                        case "B":
                            returnData = (byte)(returnData * 16 + 11);
                            break;
                        case "C":
                            returnData = (byte)(returnData * 16 + 12);
                            break;
                        case "D":
                            returnData = (byte)(returnData * 16 + 13);
                            break;
                        case "E":
                            returnData = (byte)(returnData * 16 + 14);
                            break;
                        case "F":
                            returnData = (byte)(returnData * 16 + 15);
                            break;
                        case "1":
                        case "2":
                        case "3":
                        case "4":
                        case "5":
                        case "6":
                        case "7":
                        case "8":
                        case "9":
                            returnData = (byte)(returnData + byte.Parse(temp));
                            break;
                        default:
                            returnData = (byte)(returnData * 16);
                            break;
                    }
                }
            }
            return returnData;
        }
        [Obsolete]
        public static string Format(object data, int Len)
        {
            string returnData = "";
            try
            {
                switch (Len)
                {
                    case 1:
                        returnData = string.Format("{0:F1}", data);
                        break;
                    case 2:
                        returnData = string.Format("{0:F2}", data);
                        break;
                    case 3:
                        returnData = string.Format("{0:F3}", data);
                        break;
                    default:
                        returnData = string.Format("{0:F0}", data);
                        break;
                }
            }
            catch (Exception exc)
            {
            }
            return returnData;
        }
    }
}
