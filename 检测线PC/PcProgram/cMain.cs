using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Text;
using System.Data.OleDb;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using PcProgram;
namespace System
{
    class cMain
    {
        //总设置
        public static bool isDebug = false;//是否调试模式
        public const int DataAll = 15;//读的数据点总数(不超过40)
        public const int DataShow = 18;//显示数据点总数(不超过20)多了没地放
        public const int DataProtect = 20;//保护的个数(不超过20)多了要改界面 
        public const int DataUseProtect = 8;//正在使用的
        public const int DataSystem = 34;//系统设置个数
        public const int DataKaiGuang = 10;//开关量个数(为保持数据格式兼容,最好这里不要改)
        public const int DataPlcMPoint = 14;//PLC输出M点个数
        public const int DataPlcDPoint = 0;//Plc输出D点个数
        public static bool isDanXiang = false;//是否是单相
        public const int AllCount = 7;//小车总数
        public const string DataAllTitleStr = "进风温度(℃),出风温度(℃),进管温度(℃),出管温度(℃),进管压力(Mpa),出管压力(Mpa),A相电压(V),B相电压(V),C相电压(V),A相电流(A),B相电流(A),C相电流(A),A相功率(W),B相功率(W),C相功率(W)";
        public const string DataShowTitleStr = "电压(V),电流(A),功率(W),进管压力(Mpa),出管压力(Mpa),进风温度(℃),出风温度(℃),风温差(℃),进管温度(℃),出管温度(℃),管温差(℃),W.频率(Hz),W.冷凝(℃),W.环温(℃),W.排气(℃),W.外风机,W.物料编码,W.程序版本";
        public static string staticIsShow = "0,0,0,1,1," + "1,1,1,1,0," + "0,0,0,0,0," + "0,0,0,0,0,0,0,0,0,0,0";//1为要显示的曲线,0为不显示
        public const string BiaoZhunJiStr = "1#标准机,2#标准机,3#标准机";//标准机3(C),标准机4(D)
        public const string BuZhouMingStr = "低启,制热,制冷,停机,待机,检加热带";
        public const string KaiGuangStr = "内机高风,四通阀,压缩机,备用";
        public const string DianYuanStr = "220V,110V";
        public const string DianXinHao = "220V,24V";
        public const string ZYSFMode = "全过程正压收氟,收氟过程正压收氟,不作用";
        public const string ZYSFDoing = "报警,停机";
        public const string JiQiStr = "定频机,新变频,定频SN机(AA开头),定频SN机(55开头),PQ机";
        public const string BaoHuStr = "高压保护压力(Mpa),高压保护延时(s),低压保护压力(Mpa),低压保护延时(s),电流保护上限(A),电流保护上限延时(s),电流保护下限(A),电流保护下限延时(s),正收停机压力(Mpa),正收停机延时(s),正收报警压力(Mpa),正收报警延时(s),,,," +
            ",,,,";
        public const string XiTongStr = "上一次条码,上一次ID,PLC使用串口,条码使用串口,Rs485使用串口,变频板端口,PQ端口,通用密码,压力变比K1,压力变比K2,压力变比B1,压力变比B2,正压收氟(1使用/0不使用),正压收氟动作(1不停机/0停机)";
        public const string ViewAllStr = "步骤名,电流,功率,压力,温差,频率," +
            "步骤名,电流,功率,压力,温差,频率";

        public static cModeSet mModeSet = new cModeSet();//机型设置信息
        public static cSystemSet mSysSet = new cSystemSet();//系统设置信息
        public static cTestResult mTestResult = new cTestResult();//步骤检测数据,用来显示到界面
        public static cNetResult mNetResult = new cNetResult();//步骤检测数据,用来保存
        public static cNetResult[] mCurrentData = new cNetResult[AllCount];//实时数据
        public static cTempResult[] mTempNetResult = new cTempResult[AllCount];//步骤数据
        public static cNetModeSet mNetModeSet = new cNetModeSet();//检测条码等设置
        public static cTodayData mTodayData = new cTodayData();
        public static cOverResult mOverResult = new cOverResult();
        public static string strLanguage = "en-US";
        public static cSendData mUdp = new cSendData();
        public static bool isPrint = false;
        public static bool isAutoStart = false;
        public static string Title = "美的空调室外机检测系统";
        public static PrivateFontCollection privateFontsShaoNv = new PrivateFontCollection();//字体用
        public static PrivateFontCollection privateFontsYouYuan = new PrivateFontCollection();//字体用
        public static string path = Application.StartupPath;
        public static string _Modeuser = "";//
        public static string _Modetime = "";
        public static bool _isSystemUser = false;
        public static bool isAutoSn = true;
        public static string comICCard = "COM1";
        public static string comBar = "COM2";
        public static int ReadAnGuiDelay = 5000;
        public static int ReadDelay = 60000;
        public static string ReadNowICCard = "";
        public static double WenDuADLo = 0.996;
        public static double WenDuADHi = 4.98;
        public static double WenDuLo = 0;
        public static double WenDuHi = 50;

        public static double ShiDuADLo = 0.996;
        public static double ShiDuADHi = 4.98;
        public static double ShiDuLo = 0;
        public static double ShiDuHi = 100;
        public static string isAccessDiQi = ",";
        public static bool[] isUdpInitError = new bool[AllCount];
        public static string[] TestIcCardNum;
        public static string[] DataAllTitle = new string[40];
        public static string[] DataShowTitle = new string[20];
        public cMain()
        {
        }
        public static string ReadFile(string FileName)//读取文本文件
        {
            string returnStr = "";
            try
            {
                StreamReader sr = new StreamReader(FileName);
                returnStr = sr.ReadToEnd();
                sr.Close();
                sr = null;
            }
            catch (Exception exc)
            {
                WriteErrorToLog("cMain ReadFile is Error " + exc.ToString());
            }
            return returnStr;
        }
        /// <summary>
        /// 将指定字符串写入到指定的文件中
        /// </summary>
        /// <param name="FileName">string,文件名</param>
        /// <param name="WriteStr">string,字符串</param>
        /// <param name="append">bool,是否为追加模式,true,追加到文件中字符末尾,false,重写文件字符</param>
        public static void WriteFile(string FileName, string WriteStr, bool append)//写入文本文件
        {
            StreamWriter sr = new StreamWriter(FileName, append);
            sr.Write(WriteStr);
            sr.Close();
            sr = null;
        }
        /// <summary>
        /// 将出错信息写入日志
        /// </summary>
        /// <param name="ErrorStr">出错信息</param>
        public static void WriteErrorToLog(string ErrorStr)
        {
            FileInfo fi = new FileInfo(path + "\\Log\\Log.txt");
            WriteFile(path + "\\Log\\Log.txt", DateTime.Now.ToLocalTime() + "  " + ErrorStr + (char)13 + (char)10, true);
            fi = null;
        }
        /// <summary>
        /// 读取INI文件
        /// </summary>
        /// <param name="lpApplicationNode">要读取的小节名</param>
        /// <param name="lpKeyName">要读取的项目名</param>
        /// <returns>返回读取后的字符串</returns>
        public static string ReadIni(string lpApplicationNode, string lpKeyName)
        {
            return ReadIni(lpApplicationNode, lpKeyName, "");
        }
        public static string ReadIni(string lpApplicationNode, string lpKeyName, string defaultStr)
        {
            StringBuilder readResult = new StringBuilder(1024);
            cApi.GetPrivateProfileString(lpApplicationNode, lpKeyName, defaultStr, readResult, 1024, cMain.path + @"\Data\System.ini");
            return readResult.ToString();
        }
        public static void WriteIni(string lpApplicationNode, string lpKeyName, string lpValue, string filePath)
        {
            cApi.WritePrivateProfileString(lpApplicationNode, lpKeyName, lpValue, filePath);
        }
        public static void WriteIni(string lpApplicationNode, string lpKeyName, string lpValue)
        {
            cApi.WritePrivateProfileString(lpApplicationNode, lpKeyName, lpValue, cMain.path + @"\Data\System.ini");
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
    }

    #region

    public static class Num
    {
        /// <summary>
        /// 去除非显示字符
        /// </summary>
        /// <param name="s">string,要去除非显示字符的原始字符串</param>
        /// <returns>string,去除非显示字符后的字符串</returns>
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
        public static Single SingleParse(object Num)
        {
            Single s = 0;
            if (DBNull.Value == Num || Num == null)
            {
                return s;
            }
            try
            {
                s = Single.Parse(Num.ToString());
            }
            catch
            { }
            return s;
        }
        /// <summary>
        /// 将指定字符串转化成整形数据
        /// </summary>
        /// <param name="Num">object,要转化的字符</param>
        /// <returns>int,转化后的整形数据</returns>
        public static int IntParse(object Num)
        {
            int s = 0;
            if (DBNull.Value == Num || Num == null)
            {
                return s;
            }
            try
            {
                s = int.Parse(Num.ToString());
            }
            catch
            { }
            return s;
        }
        public static bool BoolParse(object Num)
        {
            bool b = true;
            if (DBNull.Value == Num || Num == null)
            {
                return b;
            }
            try
            {
                b = bool.Parse(Num.ToString());
            }
            catch
            { }
            return b;
        }
        /// <summary>
        /// 将指定字符串转化成双精度数据
        /// </summary>
        /// <param name="Num">object,要转化的字符</param>
        /// <returns>double,转化后的又精度数据</returns>
        public static double DoubleParse(object Num)
        {
            double s = 0;
            if (DBNull.Value == Num || Num == null)
            {
                return s;
            }
            try
            {
                s = double.Parse(Num.ToString());
            }
            catch
            { }
            return s;
        }
        public static long LongParse(object Num)
        {
            long l = 0;
            if (DBNull.Value == Num || Num == null)
            {
                return l;
            }
            try
            {
                l = long.Parse(Num.ToString());
            }
            catch
            { }
            return l;
        }
        public static DateTime DateTimeParse(object Num)
        {
            DateTime d = DateTime.Now;
            if (DBNull.Value == Num || Num == null)
            {
                return d;
            }
            try
            {
                d = DateTime.Parse(Num.ToString());
            }
            catch
            { }
            return d;
        }
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
        public static double Rand()
        {
            Random r = new Random();
            return r.NextDouble();
        }
        public static byte ByteParseFromHex(object data)
        {
            byte returnData = 0;
            string tempStr = data.ToString();
            try
            {
                returnData = Convert.ToByte(tempStr, 16);
            }
            catch
            {
                returnData = 0;
            }
            return returnData;
        }
    }
    #endregion
    /// <summary>
    /// 上位机发送设置
    /// </summary>
    public class cNetModeSet
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string mBar = "";
        /// <summary>
        /// 是否自动启动
        /// </summary>
        public bool isStart = false;
        /// <summary>
        /// 普通机型设置
        /// </summary>
        public cModeSet ModeSet = new cModeSet();

        public string GetStr()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("B~");
            sb.Append(string.Format("{0}~", this.mBar));
            sb.Append(string.Format("{0}~", (this.isStart ? "1" : "0")));
            sb.Append(string.Format("{0}", this.ModeSet.GetStr()));
            return sb.ToString();
        }
    }
    /// <summary>
    /// 机型设置
    /// </summary>
    public class cModeSet//机型设置
    {
        public const int StepCount = 20;
        /// <summary>
        /// 机型ID号
        /// </summary>
        public string mId = "DEF";
        /// <summary>
        /// 机型
        /// </summary>
        public string mMode = "";
        /// <summary>
        /// 描述
        /// </summary>
        public string mDescript = "";
        /// <summary>
        /// 电源选择
        /// </summary>
        public int mElect = 0;
        /// <summary>
        /// 标准机
        /// </summary>
        public int mBiaoZhunJi = 0;
        /// <summary>
        /// 标准机序号1
        /// </summary>
        public bool mBiaoZhunJi1 = false;
        /// <summary>
        /// 标准机序号1
        /// </summary>
        public bool mBiaoZhunJi2 = false;
        /// <summary>
        /// 标准机序号1
        /// </summary>
        public bool mBiaoZhunJi3 = false;
        /// <summary>
        /// 标准机序号1
        /// </summary>
        public bool mBiaoZhunJi4 = false;
        /// <summary>
        /// 标准机序号1
        /// </summary>
        public bool mBiaoZhunJi5 = false;
        /// <summary>
        /// 标准机序号1
        /// </summary>
        public bool mBiaoZhunJi6 = false;
        /// <summary>
        /// 是否24V电源
        /// </summary>
        public int m24V = 0;
        /// <summary>
        /// 机器,变频,定频,东芝等
        /// </summary>
        public int mJiQi = 0;
        /// <summary>
        /// 保护数据
        /// </summary>
        public Single[] mProtect = new Single[20];
        /// <summary>
        /// 步骤ID ,10元素1维数组,0为低启,1为制热,2为制冷,3为停机,4为待机
        /// </summary>
        public string[] mStepId = new string[StepCount]; //步骤ID 0~4
        /// <summary>
        /// 设定步骤检测时间,10元素1维数组
        /// </summary>
        public int[] mSetTime = new int[StepCount];//检测时间
        /// <summary>
        /// 开关量,10*cMain.DataKaiGuang元素2维数组
        /// </summary>
        public bool[,] mKaiGuan = new bool[StepCount, cMain.DataKaiGuang];
        /// <summary>
        /// 发送指令,10元素1维数组
        /// </summary>
        public string[] mSendStr = new string[StepCount];
        /// <summary>
        /// 数据上限,10*20元素2维数组
        /// </summary>
        public Single[,] mHighData = new Single[StepCount, cMain.DataShow];
        /// <summary>
        /// 数据下限,10*20元素2维数组
        /// </summary>
        public Single[,] mLowData = new Single[StepCount, cMain.DataShow];
        /// <summary>
        /// 是否显示数据
        /// </summary>
        public bool[] mShow = new bool[cMain.DataShow];
        public cModeSet()
        {
            int i, j;
            for (i = 0; i < 20; i++)
            {
                mProtect[i] = 0;
            }
            mProtect[0] = 4;
            mProtect[1] = 5;
            mProtect[2] = -1;//
            mProtect[3] = 5;//延时
            mProtect[4] = 50;//
            mProtect[5] = 5;//延时
            mProtect[6] = -1;//欠流保护
            mProtect[7] = 5;
            for (i = 0; i < StepCount; i++)
            {
                mStepId[i] =  "制热";//检测步骤
                mSetTime[i] = 0;//检测时间
                for (j = 0; j < cMain.DataKaiGuang; j++)
                {
                    mKaiGuan[i, j] = false;//开关量
                }
                mSendStr[i] = "";//SN指令
                for (j = 0; j < cMain.DataShow; j++)
                {
                    mHighData[i, j] = 0;//上限
                    mLowData[i, j] = 0;//下限
                }
            }
            for (i = 0; i < cMain.DataShow; i++)
            {
                mShow[i] = true;
            }
        }
        public static string LastID()
        {
            string result = "";
            using (DataSet ds = cData.readData("select top 1 * from Mode order by mTime desc", cData.ConnMain))
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    result = ds.Tables[0].Rows[0]["ID"].ToString();
                }
            }
            return result;
        }
        public static cModeSet Read(string id)
        {
            cModeSet result = new cModeSet();
            //保护
            using (DataSet ds = cData.readData(string.Format("select * from Mode where Id='{0}'", id), cData.ConnMain))
            {
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count>0)
                {
                    result.mId = id;
                    result.mMode = ds.Tables[0].Rows[0]["Mode"].ToString();
                    result.mDescript = ds.Tables[0].Rows[0]["About"].ToString();
                    result.mElect = Num.IntParse(ds.Tables[0].Rows[0]["Elect"]);
                    result.mBiaoZhunJi = Num.IntParse(ds.Tables[0].Rows[0]["BiaoZhunJi"]);
                    result.mBiaoZhunJi1 = Num.BoolParse(ds.Tables[0].Rows[0]["BiaoZhunJi1"]);
                    result.mBiaoZhunJi2 = Num.BoolParse(ds.Tables[0].Rows[0]["BiaoZhunJi2"]);
                    result.mBiaoZhunJi3 = Num.BoolParse(ds.Tables[0].Rows[0]["BiaoZhunJi3"]);
                    result.mBiaoZhunJi4 = Num.BoolParse(ds.Tables[0].Rows[0]["BiaoZhunJi4"]);
                    result.mBiaoZhunJi5 = Num.BoolParse(ds.Tables[0].Rows[0]["BiaoZhunJi5"]);
                    result.mBiaoZhunJi6 = Num.BoolParse(ds.Tables[0].Rows[0]["BiaoZhunJi6"]);
                    result.m24V = Num.IntParse(ds.Tables[0].Rows[0]["m24V"]);
                    result.mJiQi = Num.IntParse(ds.Tables[0].Rows[0]["JiQI"]);
                    for (int i = 0; i < cMain.DataProtect; i++)
                    {
                        result.mProtect[i] = Num.SingleParse(ds.Tables[0].Rows[0][string.Format("Prot{0}", i)]);
                    }

                }
            }
            //步骤
            using (DataSet ds = cData.readData(string.Format("select * from InitPara where modeid='{0}' order by stepId", id), cData.ConnMain))
            {
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count>0)
                {
                    for (int i = 0; i < StepCount && i < ds.Tables[0].Rows.Count; i++)
                    {
                        result.mStepId[i] = ds.Tables[0].Rows[i]["RunMode"].ToString();
                        result.mSetTime[i] = Num.IntParse(ds.Tables[0].Rows[i]["Times"]);
                        result.mSendStr[i] = ds.Tables[0].Rows[i]["Code"].ToString();
                        for (int j = 0; j < cMain.DataKaiGuang; j++)
                        {
                            result.mKaiGuan[i, j] = Num.BoolParse(ds.Tables[0].Rows[i][string.Format("KaiGuan{0}", j + 1)]);
                        }
                        for (int j = 0; j < cMain.DataShow; j++)
                        {
                            result.mLowData[i, j] = Num.SingleParse(ds.Tables[0].Rows[i][string.Format("Data{0}", j * 2 + 1)]);
                            result.mHighData[i, j] = Num.SingleParse(ds.Tables[0].Rows[i][string.Format("Data{0}", j * 2 + 2)]);
                        }
                    }
                }
            }
            //显示 
            using (DataSet ds = cData.readData(string.Format("select * from Show where id='{0}'", id), cData.ConnMain))
            {
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count>0)
                {
                    for (int i = 0; i < cMain.DataShow; i++)
                    {
                        result.mShow[i] = Num.BoolParse(ds.Tables[0].Rows[0][string.Format("Show{0}", i + 1)]);
                    }
                }
            }
            return result;
        }
        public static void Delete(string id)
        {
            cData.upData(string.Format("delete from Mode where id='{0}'", id), cData.ConnMain);
            cData.upData(string.Format("delete from InitPara where modeid='{0}'", id), cData.ConnMain);
            cData.upData(string.Format("delete from Show where id='{0}'", id), cData.ConnMain);
        }
        public bool Save()
        {
            bool result = true;
            Delete(this.mId);
            //保护
            string sql = "insert into Mode (Id,mode,about,elect,biaozhunji,biaozhunji1,biaozhunji2,biaozhunji3,biaozhunji4,biaozhunji5,biaozhunji6,m24v,jiqi,prot0,prot1,prot2,prot3,prot4,prot5,prot6,prot7,prot8,prot9,prot10,prot11,prot12,prot13,prot14,prot15,prot16,prot17,prot18,prot19,mTime) values ({0})";
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("'{0}'", this.mId));
            sb.Append(string.Format(",'{0}'", this.mMode));
            sb.Append(string.Format(",'{0}'", this.mDescript));
            sb.Append(string.Format(",{0}", this.mElect));
            sb.Append(string.Format(",{0}", this.mBiaoZhunJi));
            sb.Append(string.Format(",{0}", this.mBiaoZhunJi1));
            sb.Append(string.Format(",{0}", this.mBiaoZhunJi2));
            sb.Append(string.Format(",{0}", this.mBiaoZhunJi3));
            sb.Append(string.Format(",{0}", this.mBiaoZhunJi4));
            sb.Append(string.Format(",{0}", this.mBiaoZhunJi5));
            sb.Append(string.Format(",{0}", this.mBiaoZhunJi6));
            sb.Append(string.Format(",{0}", this.m24V));
            sb.Append(string.Format(",{0}", this.mJiQi));
            for (int i = 0; i < cMain.DataProtect; i++)
            {
                sb.Append(string.Format(",{0}", this.mProtect[i]));
            }
            sb.Append(string.Format(",#{0:yyyy-MM-dd HH:mm:ss}#", DateTime.Now));
            result = result && (cData.upData(string.Format(sql, sb.ToString()), cData.ConnMain) == 1);
            //步骤
            sql = "insert into InitPara values ({0})";
            for (int i = 0; i < StepCount; i++)
            {
                sb = new StringBuilder();
                sb.Append(string.Format("'{0}'", this.mId));
                sb.Append(string.Format(",{0}", i));
                sb.Append(string.Format(",'{0}'", this.mStepId[i]));
                sb.Append(string.Format(",{0}", this.mSetTime[i]));
                sb.Append(string.Format(",'{0}'", this.mSendStr[i]));
                for (int j = 0; j < cMain.DataKaiGuang; j++)
                {
                    sb.Append(string.Format(",{0}", this.mKaiGuan[i, j]));
                }
                for (int j = 0; j < cMain.DataShow; j++)
                {
                    sb.Append(string.Format(",{0}", this.mLowData[i, j]));
                    sb.Append(string.Format(",{0}", this.mHighData[i, j]));
                }
                for (int j = cMain.DataShow; j < 50; j++)
                {
                    sb.Append(string.Format(",0,0"));
                }
                result = result && (cData.upData(string.Format(sql, sb.ToString()), cData.ConnMain) == 1);
            }
            //显示
            sql = "insert into Show values({0})";
            sb = new StringBuilder();
            sb.Append(string.Format("'{0}'", this.mId));
            for (int i = 0; i < cMain.DataShow; i++)
            {
                sb.Append(string.Format(",{0}", this.mShow[i]));
            }
            for (int i = cMain.DataShow; i < 50; i++)
            {
                sb.Append(",false");
            }
            result = result && (cData.upData(string.Format(sql, sb.ToString()), cData.ConnMain) == 1);
            return result;
        }
        public string GetStr()
        {
            string writeStr = "";
            int i, j;
            string tempTime = "", tempSn = "";
            writeStr = writeStr + this.mId + "~";
            writeStr = writeStr + this.mMode + "~";
            writeStr = writeStr + this.mElect.ToString() + "~";
            writeStr = writeStr + this.mBiaoZhunJi.ToString() + "~";
            writeStr = writeStr + this.mBiaoZhunJi1.ToString() + "~";
            writeStr = writeStr + this.mBiaoZhunJi2.ToString() + "~";
            writeStr = writeStr + this.mBiaoZhunJi3.ToString() + "~";
            writeStr = writeStr + this.mBiaoZhunJi4.ToString() + "~";
            writeStr = writeStr + this.mBiaoZhunJi5.ToString() + "~";
            writeStr = writeStr + this.mBiaoZhunJi6.ToString() + "~";
            writeStr = writeStr + this.m24V.ToString() + "~";
            writeStr = writeStr + this.mJiQi.ToString() + "~";
            for (i = 0; i < cMain.DataProtect; i++)
            {
                writeStr = writeStr + this.mProtect[i].ToString() + "~";
            }
            for (i = 0; i < StepCount; i++)
            {
                writeStr = writeStr + this.mStepId[i] + "~";
                tempTime = tempTime + this.mSetTime[i].ToString() + "~";
                tempSn = tempSn + this.mSendStr[i] + "~";
            }
            writeStr = writeStr + tempTime + tempSn;
            for (i = 0; i < cMain.DataKaiGuang; i++)
            {
                for (j = 0; j < StepCount; j++)
                {

                    if (this.mKaiGuan[j, i])
                    {
                        writeStr = writeStr + "√" + "~";
                    }
                    else
                    {
                        writeStr = writeStr + "×" + "~";
                    }
                }
            }
            for (i = 0; i < cMain.DataShow * 2; i++)
            {
                for (j = 0; j < StepCount; j++)
                {
                    if (i % 2 == 0)
                    {
                        writeStr = writeStr + this.mLowData[j, i / 2].ToString() + "~";
                    }
                    else
                    {
                        writeStr = writeStr + this.mHighData[j, (i - 1) / 2].ToString() + "~";
                    }
                }
            }
            for (i = 0; i < cMain.DataShow; i++)
            {
                writeStr = writeStr + this.mShow[i].ToString() + "~";
            }
            return writeStr;
        }
    }
    public class cSystemSet//系统设置
    {
        /// <summary>
        /// 上一次的条码
        /// </summary>
        public string mPrevBar = "DEF1234567890123";
        /// <summary>
        /// 上一次的ID号;
        /// </summary>
        public string mPrevId = "DEF";
        /// <summary>
        /// 电压多少后才开始启动
        /// </summary>
        public string mPLCCOM = "COM1";
        /// <summary>
        /// 条码使用端口
        /// </summary>
        public string mBarCom = "COM2";
        /// <summary>
        /// 报警模式
        /// </summary>
        public string m485COM = "COM3";
        /// <summary>
        /// PQ端口
        /// </summary>
        public string mPQ = "COM4";
        /// <summary>
        /// Sn端口
        /// </summary>
        public string mSnCom = "COM5";
        /// <summary>
        /// 是否自动计算指令
        /// </summary>
        public string mPassWord = "";
        /// <summary>
        /// 压力变比K,4个元素的1维数组
        /// </summary>
        public double[] PressK = new double[5];
        /// <summary>
        /// 压力变比B,4个元素的1维数组
        /// </summary>
        public double[] PressB = new double[5];
        public cSystemSet()
        {
            int i;
            for (i = 0; i < PressK.Length; i++)
            {
                PressK[i] = 1.275f;
                PressB[i] = -1.375f;
            }

        }
    }
    public class cKBValue//KB值
    {
        public double[] valueK = new double[cMain.DataAll];
        public double[] valueB = new double[cMain.DataAll];
        public cKBValue()
        {
            int i;
            for (i = 0; i < cMain.DataAll; i++)
            {
                valueK[i] = 1;
                valueB[i] = 0;
            }
        }
    }
    public class cNetResult//传回上位机数据
    {
        public cRunResult RunResult = new cRunResult();
        public cStepResult StepResult = new cStepResult();
    }
    public class cTempResult//实时数据
    {
        public cRunResult[] RunResult = new cRunResult[cModeSet.StepCount];
        public cStepResult[] StepResult = new cStepResult[cModeSet.StepCount];
        public cTempResult()
        {
            for (int i = 0; i < cModeSet.StepCount; i++)
            {
                StepResult[i] = new cStepResult();
                RunResult[i] = new cRunResult();
            }
        }
    }
    public class cPrintResult
    {
        public cNetResult NetResult = new cNetResult();
        public cModeSet ModeSet = new cModeSet();
    }
    public class cTestResult//当前检测所有步骤数据
    {
        /// <summary>
        /// 当前检测步数的结果,10元素1维数组
        /// </summary>
        public cStepResult[] StepResult = new cStepResult[cModeSet.StepCount];
        public cTestResult()
        {
            int i;
            for (i = 0; i < cModeSet.StepCount; i++)
            {
                StepResult[i] = new cStepResult();
            }
        }
    }
    public class cRunResult//正在运行机器信息结果
    {
        /// <summary>
        /// 检测日期,时间
        /// </summary>
        public DateTime mTestTime = DateTime.Now;
        /// <summary>
        /// 条码
        /// </summary>
        public string mBar = "";
        /// <summary>
        /// 机型ID号
        /// </summary>
        public string mId = "";
        /// <summary>
        /// 机型
        /// </summary>
        public string mMode = "";
        /// <summary>
        /// 检测台车号
        /// </summary>
        public int mTestNo = 0;
        /// <summary>
        /// 机器,变频,定频,东芝等
        /// </summary>
        public int mJiQi = 0;
        /// <summary>
        /// 检测总结果是否合格
        /// </summary>
        public bool mIsPass = true;
        /// <summary>
        /// 步骤ID ,当前检测第几步
        /// </summary>
        public int mStepId = -1; //步骤ID 1~StepCount
        /// <summary>
        /// 步骤号,当前检测步骤检测项目
        /// </summary>
        public string mStep = ""; //步骤名
    }

    /// <summary>
    /// 单步检测数据
    /// </summary>
    public class cStepResult
    {
        /// <summary>
        /// 检测数据,20元素1维数组
        /// </summary>
        public double[] mData = new double[cMain.DataShow];
        /// <summary>
        /// 检测各项数据是否合格,20元素1维数组,1为合格,0为不合格,-1为未开始检测
        /// </summary>
        public int[] mIsDataPass = new int[cMain.DataShow];
        /// <summary>
        /// 当前步骤是否合格,1为合格,0为不合格,-1为未开始检测
        /// </summary>
        public int mIsStepPass = -1;
        public cStepResult()
        {
            int i;
            for (i = 0; i < cMain.DataShow; i++)
            {
                mData[i] = 0;
                mIsDataPass[i] = -1;
            }
        }
    }
    /// <summary>
    /// 每台小车检测结果,用来统计计数
    /// </summary>
    public class cOverResult
    {
        /// <summary>
        /// 台车号
        /// </summary>
        public int testNo = 0;
        /// <summary>
        /// 是否合格
        /// </summary>
        public bool isPass = true;
    }
    /// <summary>
    /// 当天检测数据
    /// </summary>
    public class cTodayData
    {
        /// <summary>
        /// 检测开始时间
        /// </summary>
        public DateTime TodayTime;
        /// <summary>
        /// 检测数据,0:总数,1:合格数,2:不合格数
        /// </summary>
        public long[] TestCount = new long[3];
        public cTodayData()
        {
            TodayTime = DateTime.Now;
            for (int i = 0; i < TestCount.Length; i++)
            {
                TestCount[i] = 0;
            }
        }
        public static bool WriteToday(string path,string Nodes, string value)
        {
            return cXml.WriteNode(path, Nodes, value);
        }
        public static bool WriteToday(string nodes, string value)
        {
            return WriteToday(cMain.path + "\\Data\\DataToday.xml", nodes, value);
        }
        public static bool ReadToday(string path, ref DateTime todayTime, ref long[] testCount)
        {
            bool isOk = false;
            cXml mXml = new cXml();
            try
            {
                string[] Nodes = new string[4];
                string[] defaultValue = new string[4];
                string[] readValue = new string[4];
                Nodes[0] = "TodayTime";
                defaultValue[0] = DateTime.Now.ToString();
                for (int i = 0; i < testCount.Length; i++)
                {
                    Nodes[i + 1] = "TestCount" + i.ToString();
                    defaultValue[i + 1] = "0";
                }
                readValue = cXml.ReadNodes(path, Nodes, defaultValue);
                todayTime = Num.DateTimeParse(readValue[0]);
                for (int i = 0; i < testCount.Length; i++)
                {
                    testCount[i] = Num.LongParse(readValue[i + 1]);
                }
                isOk = true;
            }
            catch (Exception exc)
            {
                cMain.WriteErrorToLog(exc.Message);
                isOk = false;
            }
            return isOk;
        }
        public static bool ReadToday(ref DateTime todayTime,ref long[] testCount)
        {
            return ReadToday(cMain.path + "\\Data\\DataToday.xml",ref todayTime,ref testCount);
        }
    }

}
