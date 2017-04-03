//#define WinCe
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections;
using System.Data;
namespace NewMideaProgram
{
    public  class cMain
    {
        //总设置
        public static bool isComPuter = true;
        public static bool isDebug = false;//是否调试模式
        public const int DataAll = 15;
        public const int DataShow = 18;
        public const int DataProtect = 20;
        public const int DataKaiGuang = 10;
        //public const int DataPlcMPoint = 17;//PLC输出M点个数
        //public const int DataPlcDPoint = 0;//Plc输出D点个数
        public const int AllCount = 7;//小车总数
        public static string[] DataAllTitleStr = new string[2]{"Air Inlet1,Air Outlet1,Air Inlet2,Air Outlet2,Air Inlet3,Air Outlet3,Air Inlet4,Air Outlet4,Pressure1,Pressure2,Pressure3,Pressure4,Voltage A,Voltage B,Voltage C,Current A,Current B,Current C,Power A,Power B,Power C", 
            "进风温度(℃),出风温度(℃),进管温度(℃),出管温度(℃),进管压力(Mpa),出管压力(Mpa),A相电压(V),B相电压(V),C相电压(V),A相电流(A),B相电流(A),C相电流(A),A相功率(W),B相功率(W),C相功率(W)"};
        public static string[] DataShowTitleStr = new string[2] { "Voltage(V),Current(A),Power(W),Pressure(Mpa),Air Inlet(℃),Air Outlet(℃),Temperature Diff(℃),Hz,T1(℃),T2(℃),T3(℃)",
            "电压(V),电流(A),功率(W),进管压力(Mpa),出管压力(Mpa),进风温度(℃),出风温度(℃),风温差(℃),进管温度(℃),出管温度(℃),管温差(℃),W.频率(Hz),W.冷凝(℃),W.环温(℃),W.排气(℃),W.外风机,W.物料编码,W.版本号"};
        public static string staticIsShow = "0,0,0,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0";//1为要显示的曲线,0为不显示
        public static string[] BiaoZhunJiStr = new string[2] { "unit1(A),unit2(B)", "1#标准机,2#标准机" };//标准机3(C),标准机4(D)
        public static string[] BuZhouMingStr = new string[2] { "Low Vol,Heating,Cooling,Stop, ", "低启,制热,制冷,停机,待机,检加热带" };
        public const string DianYuanStr = "220V,110V";
        public const string DianXinHao = "220V,24V";
        public static string[] ZYSFMode = new string[2] {"全过程正压收氟,收氟过程正压收氟,不作用", "全过程正压收氟,收氟过程正压收氟,不作用" };
        public static string[] ZYSFDoing = new string[2] { "Alerting,Stop", "报警,停机" };
        public static string[] JiQiStr = new string[2] { "Normal,SN 600 Old,SN 600 Out,SN 600 New,Normal S(55),Normal S(AA),UL", "定频机,新变频,定频SN机(AA开头),定频SN机(55开头),PQ机,假PQ" };//变频M型,变频N型,数码机(1023),数码机(1200单冷),数码机(1200冷暖),天花机(1200冷暖),数码机(12F00×1023),东芝机,窗机";
        public static string[] BaoHuStr = new string[2]{"Baud,Parity,Datas,StopBit,,,,,High pressure protection(Mpa),Protection delay(s),"+
            "Low pressure protection(Mpa),Protection delay(s),High current protection(A),Protection Delay(s),"+
            "Low current protection(A),Protection Delay(s),,,,", "高压保护压力(Mpa),高压保护延时(s),低压保护压力(Mpa),低压保护延时(s),电流保护上限(A),电流保护上限延时(s),电流保护下限(A),电流保护下限延时(s),正收停机压力(Mpa),正收停机延时(s),正收报警压力(Mpa),正收报警延时(s),,,," +
            ",,,,"};
        public static string[] XiTongStr = new string[2]{"Last barcode,Last ID,Bound,Doing,Plc Com,485 Com,BP Com," +
            "UL Com,Address,Language,PassWord,AutoSn,Pressure ratio K1,Pressure ratio K2,Pressure ratio K3,"+
            "Pressure ratio K4,Pressure ratio B1,Pressure ratio B2,Pressure ratio B3,Pressure ratio B4",
            "上一次条码,上一次ID,PLC使用串口,条码使用串口,Rs485使用串口,变频板端口,PQ端口,通用密码,压力变比K1,压力变比K2,压力变比B1,压力变比B2,正压收氟(1使用/0不使用),正压收氟动作(1不停机/0停机)"};
        public static string[] KaiGuangStr = new string[2] { "High wind,Four-way Value,Fan,Compressor,Spare", "内机高风,四通阀,压缩机,备用" };

        public static string[] ReadPlcMPointStr = new string[] { "启动", "停止","正收报警","报警停机","正压收氟","低压停机","收氟确认" };
        public static int[] ReadPlcMPointInt = new int[] { 100, 101, 112, 113, 114, 115,106 };
        public static string[] DataPlcMPointStr = new string[] { "1#内机", "2#内机", "低风", "四通阀", "压缩机", "上电", "低启", "黄灯", "绿灯", "红灯", "备用",  "正收开关", "正收动作","蜂鸣器", "总复位" };//控制输出点名称都从这里来
        public static int[] DataPlcMPointInt = new int[] { 10, 10, 11, 12, 13, 14, 15, 16, 17, 20, 22, 110, 111, 21, 200 };//控制输出点对应地址
        public static string[] DataPlcDPointStr = new string[] {"停机压力","停机延时","报警压力","报警延时","报警停机","压力报警上限","压力报警下限" };
        public static int[] DataPlcDPointInt = new int[] { 502, 504, 501, 503, 500, 601, 602 };
        public const string NetBoardName = "EMAC1";//此处是网卡的名称,用来在注册表取IP地址的
        public static string RemoteHostName = "192.168.1.100";//上位机地址
        public static int IndexLanguage = 1;
        public static string strLanguage = "zh-CN";
        public const int FlushDataTime = 1000;//刷新数据显示时间间隔 
        public static bool isNeedPassWord = true;//界面是否启用密码
        public static string AppPath = Application.StartupPath + "\\";
        public float xSize = 1, ySize = 1;//屏幕缩放倍数
        public string[] DataAllTitle = new string[40];
        public static string[] DataShowTitle = new string[20];
        //当前界面用
        public static cModeSet mModeSet = new cModeSet();//机型设置信息
        public static cSystemSet mSysSet = new cSystemSet();//系统设置信息
        public static cTestResult mTestResult = new cTestResult();//步骤检测数据,用来显示到界面
        public static cNetResult mNetResult = new cNetResult();//步骤检测数据,用来传送上位机
        public static cMesResult mMesResult = new cMesResult();
        public static cNetResult mTempNetResult = new cNetResult();//上传上位机实时数据
        public static cAllResult mAllResult = new cAllResult();//用于mes数据保存
        public static cKBValue mKBValue = new cKBValue();//KB值设置信息
        public static cNetModeSet mNetModeSet = new cNetModeSet();//检测条码等设置
        public static cBarSet mBarSet = new cBarSet();//本地条码识别
        public static LocalSaveValue LocalSaveValue = new LocalSaveValue();
        public static cXml mBarXml = new cXml(cMain.AppPath + "\\barset.xml");//条码存储XML
        public static int ThisNo = 0;
        static int indexFrmSetLabel = 0;
        static object o = new object();
        #region
        /// <summary>
        /// 当前检测状态
        /// </summary>
        public struct NowStatue
        {
            /// <summary>
            /// 检测开始时间,用于传回给上位机的检测开始时间
            /// </summary>
            public DateTime TestTime;
            /// <summary>
            /// 检测开始时间,用于给画曲线用的计数开始时间
            /// </summary>
            public int TestStartTime;
            /// <summary>
            /// 步骤开始时间
            /// </summary>
            public int StepStartTime;
            /// <summary>
            /// 当前步骤已运行时间
            /// </summary>
            public int StepCurTime;
            /// <summary>
            /// 暂停时间
            /// </summary>
            public int HoldTime;
            /// <summary>
            /// 当前正在检测的步骤
            /// </summary>
            public int CurrentId;//当前正在检测的步骤```
            /// <summary>
            /// 是否开始检测
            /// </summary>
            public bool isStarting;//是否开始检测
            /// <summary>
            /// 停机原因ID号
            /// </summary>
            public StopValue StopId;//停止的原因ID```
            /// <summary>
            /// 是否已完成启动输出
            /// </summary>
            public bool isStarted;//是否启动完成
            /// <summary>
            /// 是否已完成停止复位输出
            /// </summary>
            public bool isStoped;//量否停止完成`
            /// <summary>
            /// 第一个低启的步骤号
            /// </summary>
            public int DiQiId;//低启步骤号````
            /// <summary>
            /// 第一个制热的步骤号
            /// </summary>
            public int firstHotId;//第一个制热的步骤号```
            /// <summary>
            /// 第一个制冷的步骤号
            /// </summary>
            public int firstColdId;//第一个制冷的步骤号``
            /// <summary>
            /// 最后一个制热
            /// </summary>
            public int lastHotId;
            /// <summary>
            /// 最后一个制冷
            /// </summary>
            public int lastColdId;
            /// <summary>
            /// 自动显示步骤测试数据
            /// </summary>
            public StepShowData mStepShowData;
            /// <summary>
            /// 当前检测是否合格
            /// </summary>
            public bool isPass;//是否合格````
            /// <summary>
            /// 上一个测试步骤
            /// </summary>
            public string PrevStep;
            /// <summary>
            /// 手动暂停
            /// </summary>
            public bool HandlePause;
            /// <summary>
            /// 是否打印完毕
            /// </summary>
            public bool PrintOver;
            /// <summary>
            /// 是否已报警
            /// </summary>
            public bool OutBeep;
        }//当前检测状态
        /// <summary>
        /// 当前检测停机原因
        /// </summary>
        public enum StopValue
        {
            /// <summary>
            /// 正常停机
            /// </summary>
           IsOk=0, 
            /// <summary>
            /// 正压收氟保护停机
            /// </summary>
            ZYSFProtect,
            /// <summary>
            /// 高压保护停机
            /// </summary>
            HighPressProtect,
            /// <summary>
            /// 低压保护停机
            /// </summary>
            LowPressProtect,
            /// <summary>
            /// 电流过大保护停机
            /// </summary>
            HighCurProtect,
            /// <summary>
            /// 电流过小保护停机
            /// </summary>
            LowCurProtect,
            /// <summary>
            /// 切换电源失败
            /// </summary>
            ChangeVolFail
        }//停机原因
        /// <summary>
        /// 步骤数据显示窗口,自动显示哪一步数据
        /// </summary>
        public enum StepShowData
        {
            /// <summary>
            /// 自动,不显示
            /// </summary>
            ShowByAuto=0,
            /// <summary>
            /// 自动显示第一个制热数据
            /// </summary>
            ShowByFirstHot,
            /// <summary>
            /// 自动显示最后一个制热的数据
            /// </summary>
            ShowByLastHot,
            /// <summary>
            /// 自动显示第一个制冷数据
            /// </summary>
            ShowByFirstCold,
            /// <summary>
            /// 自动显示最后一个制冷数据
            /// </summary>
            ShowByLastCold,
            /// <summary>
            /// 自动显示第一个不合格步骤数据
            /// </summary>
            ShowByFirstNG,
            /// <summary>
            /// 自动显示上一个测试步骤数据
            /// </summary>
            ShowByPreStep
        }
        public enum PassWord
        {
            NoPassWord=0,
            PassWord1,
            PassWord22,
            PassWord333,
            PassWord110,
            PassWord911//超级密码来着
        }
        #endregion
        public cMain()//系统初始化
        {
            if (System.Environment.OSVersion.Platform.ToString().ToUpper() == "WIN32NT")
            {
                isComPuter = true;
                AppDomain.CurrentDomain.SetData("DataDirectory",AppPath);
            }
            else
            {
                isComPuter = false;
            }
            if (!isComPuter)
            {
                xSize = (Single)640 / (Single)Screen.PrimaryScreen.Bounds.Width;
                ySize = (Single)400 / (Single)Screen.PrimaryScreen.Bounds.Height;
            }
            if (!Directory.Exists(cMain.AppPath))
            {
                Directory.CreateDirectory(cMain.AppPath);
            }
            if (!File.Exists(cMain.AppPath+"\\Log.txt"))
            {
                File.AppendText(cMain.AppPath+"\\Log.txt");
            }
            if (!Directory.Exists(cMain.AppPath+"\\ID\\"))
            {
                Directory.CreateDirectory(cMain.AppPath+"\\ID\\");
            }
            if (!File.Exists(cMain.AppPath+"\\ID\\DEF.txt"))
            {
                frmSet.DataClassToFile(mModeSet);
            }
            else
            {
                if (!frmSet.DataFileToClass((cMain.AppPath+"\\ID\\DEF.txt"), out mModeSet,true))
                {
                    frmSet.DataClassToFile(mModeSet);
                }
            }
            if (!File.Exists(cMain.AppPath+"\\KbValue.txt"))
            {
                frmKB.DataClassToFile(mKBValue);
            }
            else 
            {
                if (!frmKB.DataFileToClass(cMain.AppPath+"\\KBValue.txt", out mKBValue))
                {
                    frmKB.DataClassToFile(mKBValue);
                }
            }
            if (!File.Exists(cMain.AppPath + "\\BarSet.xml"))
            {
                mBarXml = new cXml(cMain.AppPath + "\\BarSet.xml");
                frmBarSet.DataClsToTxt(mBarSet);
            }
            else
            {
                if (!frmBarSet.DataTxtToCls(cMain.AppPath + "\\BarSet.xml", out mBarSet))
                {
                    frmBarSet.DataClsToTxt(mBarSet);
                }
            }
            LocalSaveValue.Load();
            if (!Directory.Exists(LocalSaveValue.MesDirectory))
            {
                try
                {
                    Directory.CreateDirectory(LocalSaveValue.MesDirectory);
                }
                catch
                {
                    MessageBox.Show("对不起，MES文件路径错误，请重新设置", "错误的路径", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            DataAllTitle = DataAllTitleStr[cMain.IndexLanguage].Split(',');
            DataShowTitle = DataShowTitleStr[cMain.IndexLanguage].Split(',');
        }
        public static void CreateFile(string FileName)
        {
            FileStream fs = File.Create(FileName);
            fs.Close();
        }
        public static string ReadFile(string FileName)//读取文本文件
        {
            string returnStr="";
            try
            {
                StreamReader sr = new StreamReader(FileName);
                returnStr = sr.ReadToEnd();
                sr.Close();
                sr = null;
            }
            catch(Exception exc)
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
        public static void WriteFile(string FileName, string WriteStr,bool append)//写入文本文件
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
            lock (o)
            {
                FileInfo fi = new FileInfo(cMain.AppPath+"\\Log.txt");
                if (fi.Length > 4096 * 1024)//防止错误年月累加,把整个硬盘都用光..
                {
                    fi = null;
                    WriteFile(cMain.AppPath+"\\Log.txt", DateTime.Now.ToLocalTime() + "  " + ErrorStr + (char)13 + (char)10, false);
                }
                else
                {
                    fi = null;
                    WriteFile(cMain.AppPath+"\\Log.txt", DateTime.Now.ToLocalTime() + "  " + ErrorStr + (char)13 + (char)10, true);
                }
            }

        }
        /// <summary>
        /// 缩放窗口中的控件.
        /// </summary>
        /// <param name="sender">要进行窗体控件缩放的窗体</param>
        public static void initFrom(System.Windows.Forms.Control.ControlCollection cc)//缩放窗体
        {
            Panel nowPanel;
            Button nowButton;
            TextBox nowTextBox;
            Label nowLabel;
            ComboBox nowComboBox;
            CheckBox nowCheckBox;
            DataGrid nowDataGrid;
            DataGridView nowDataGridView;
            RadioButton nowRadioButton;
            DateTimePicker nowDateTimePicker;
            IEnumerator FormEnum = cc.GetEnumerator();
            while (FormEnum.MoveNext())
            {
                if (FormEnum.Current is RadioButton)
                {
                    nowRadioButton = (RadioButton)FormEnum.Current;
                    nowRadioButton.Left = (int)(nowRadioButton.Left / frmMain.mMain.xSize);
                    nowRadioButton.Top = (int)(nowRadioButton.Top / frmMain.mMain.ySize);
                    nowRadioButton.Width = (int)(nowRadioButton.Width / frmMain.mMain.xSize);
                    nowRadioButton.Height = (int)(nowRadioButton.Height / frmMain.mMain.ySize);
                }
                if (FormEnum.Current is DateTimePicker)
                {
                    nowDateTimePicker = (DateTimePicker)FormEnum.Current;
                    nowDateTimePicker.Left = (int)(nowDateTimePicker.Left / frmMain.mMain.xSize);
                    nowDateTimePicker.Top = (int)(nowDateTimePicker.Top / frmMain.mMain.ySize);
                    nowDateTimePicker.Width = (int)(nowDateTimePicker.Width / frmMain.mMain.xSize);
                    nowDateTimePicker.Height = (int)(nowDateTimePicker.Height / frmMain.mMain.ySize);
                }
                if (FormEnum.Current is Panel)
                {
                    nowPanel = (Panel)FormEnum.Current;
                    nowPanel.Left = (int)(nowPanel.Left / frmMain.mMain.xSize);
                    nowPanel.Top = (int)(nowPanel.Top / frmMain.mMain.ySize);
                    nowPanel.Width = (int)(nowPanel.Width / frmMain.mMain.xSize);
                    nowPanel.Height = (int)(nowPanel.Height / frmMain.mMain.ySize);
                }
                if (FormEnum.Current is Button)
                {
                    nowButton = (Button)FormEnum.Current;
                    nowButton.Left = (int)(nowButton.Left / frmMain.mMain.xSize);
                    nowButton.Top = (int)(nowButton.Top / frmMain.mMain.ySize);
                    nowButton.Width = (int)(nowButton.Width / frmMain.mMain.xSize);
                    nowButton.Height = (int)(nowButton.Height / frmMain.mMain.ySize);
                }
                if (FormEnum.Current is TextBox)//文本框
                {
                    nowTextBox = (TextBox)FormEnum.Current;
                    nowTextBox.Left = (int)(nowTextBox.Left / frmMain.mMain.xSize);
                    nowTextBox.Top = (int)(nowTextBox.Top / frmMain.mMain.ySize);
                    nowTextBox.Width = (int)(nowTextBox.Width / frmMain.mMain.xSize);
                    nowTextBox.Height = (int)(nowTextBox.Height / frmMain.mMain.ySize);
                }
                if (FormEnum.Current is Label)//标签
                {
                    nowLabel = (Label)FormEnum.Current;
                    nowLabel.Left = (int)(nowLabel.Left / frmMain.mMain.xSize);
                    nowLabel.Top = (int)(nowLabel.Top / frmMain.mMain.ySize);
                    nowLabel.Width = (int)(nowLabel.Width / frmMain.mMain.xSize);
                    nowLabel.Height = (int)(nowLabel.Height / frmMain.mMain.ySize);
                    int intTag =Num.IntParse(nowLabel.Tag);
                    switch (intTag)
                    {
                        case 1:
                            nowLabel.BackColor = System.Drawing.Color.PaleTurquoise;//浅蓝色
                            break;
                        case 2:
                            nowLabel.BackColor = System.Drawing.Color.White;//白色
                            break;
                        case 3:
                            nowLabel.BackColor = System.Drawing.Color.Khaki;//
                            nowLabel.Font = new System.Drawing.Font("宋体", 13, System.Drawing.FontStyle.Bold);
                            break;
                        case 4:
                            nowLabel.BackColor = System.Drawing.Color.Silver;//银色

                            break;
                        case 5:
                            break;
                        case 9://设置界面的标签
                            nowLabel.BackColor = System.Drawing.Color.White;
                            if (indexFrmSetLabel < DataShow * 2)
                            {
                                if ((indexFrmSetLabel % 2) == 0)
                                {
                                    nowLabel.Text = DataShowTitle[(indexFrmSetLabel / 2)] + "Min,最小值".Split(',')[cMain.IndexLanguage];
                                }
                                else
                                {
                                    nowLabel.Text = DataShowTitle[((indexFrmSetLabel - 1) / 2)] + "Max,最大值".Split(',')[cMain.IndexLanguage];
                                }
                                indexFrmSetLabel++;
                            }
                            else
                            {
                                nowLabel.Visible = false;
                            }
                            break;
                        default:
                            nowLabel.BackColor = System.Drawing.Color.White;
                            break;
                    }
                }
                if (FormEnum.Current is ComboBox)//下拉框
                {
                    nowComboBox = (ComboBox)FormEnum.Current;
                    nowComboBox.Left = (int)(nowComboBox.Left / frmMain.mMain.xSize);
                    nowComboBox.Top = (int)(nowComboBox.Top / frmMain.mMain.ySize);
                    nowComboBox.Width = (int)(nowComboBox.Width / frmMain.mMain.xSize);
                    nowComboBox.Height = (int)(nowComboBox.Height / frmMain.mMain.ySize);
                    string[] tmpStr;
                    int intTag = Num.IntParse(nowComboBox.Tag);
                    switch (intTag)
                    {
                        case 1:
                            tmpStr = DianYuanStr.Split(',');
                            for (int j = 0; j < tmpStr.Length; j++)
                            {
                                nowComboBox.Items.Add(tmpStr[j]);
                            }
                            break;
                        case 2:
                            tmpStr = BiaoZhunJiStr[cMain.IndexLanguage].Split(',');
                            for (int j = 0; j < tmpStr.Length; j++)
                            {
                                nowComboBox.Items.Add(tmpStr[j]);
                            }
                            break;
                        case 3:
                            tmpStr = JiQiStr[cMain.IndexLanguage].Split(',');
                            for (int j = 0; j < tmpStr.Length; j++)
                            {
                                nowComboBox.Items.Add(tmpStr[j]);
                            }
                            break;
                        case 4:
                            tmpStr = BuZhouMingStr[cMain.IndexLanguage].Split(',');
                            for (int j = 0; j < tmpStr.Length; j++)
                            {
                                nowComboBox.Items.Add(tmpStr[j]);
                            }
                            break;
                        case 5:
                            tmpStr = ZYSFMode[cMain.IndexLanguage].Split(',');
                            for (int j = 0; j < tmpStr.Length; j++)
                            {
                                nowComboBox.Items.Add(tmpStr[j]);
                            }
                            break;
                        case 6:
                            tmpStr = ZYSFDoing[cMain.IndexLanguage].Split(',');
                            for (int j = 0; j < tmpStr.Length; j++)
                            {
                                nowComboBox.Items.Add(tmpStr[j]);
                            }
                            break;
                        case 7:
                            tmpStr = DianXinHao.Split(',');
                            for (int j = 0; j < tmpStr.Length; j++)
                            {
                                nowComboBox.Items.Add(tmpStr[j]);
                            }
                            break;
                        default:
                            break;
                    }
                }
                if (FormEnum.Current is CheckBox)
                {
                    nowCheckBox = (CheckBox)FormEnum.Current;
                    nowCheckBox.Left = (int)(nowCheckBox.Left / frmMain.mMain.xSize);
                    nowCheckBox.Top = (int)(nowCheckBox.Top / frmMain.mMain.ySize);
                    nowCheckBox.Width = (int)(nowCheckBox.Width / frmMain.mMain.xSize);
                    nowCheckBox.Height = (int)(nowCheckBox.Height / frmMain.mMain.ySize);
                }
                if (FormEnum.Current is DataGrid)
                {
                    nowDataGrid = (DataGrid)FormEnum.Current;
                    nowDataGrid.Font = new System.Drawing.Font("宋体", 14, System.Drawing.FontStyle.Regular);
                    string[] tempStr;
                    int intTag = Num.IntParse(nowDataGrid.Tag);
                    switch (intTag)
                    {
                        // 1,保护数据表格,2,上下限数据表格,3,系统设置表格,4,KB值表格,5,单机数据表格,6,条码设置表格
                        case 1:
                            DataTable dt = new DataTable("newTable");
                            tempStr = BaoHuStr[cMain.IndexLanguage].Split(',');
                            dt.Columns.Add("name1", typeof(string));
                            dt.Columns.Add("data1", typeof(string));
                            dt.Columns.Add("name2", typeof(string));
                            dt.Columns.Add("data2", typeof(string));
                            for (int j = 0; j < 10; j++)
                            {
                                DataRow row = dt.NewRow();
                                row["name1"] = tempStr[j];
                                //row["data1"] = "";
                                row["name2"] = tempStr[j + 10];
                                //row["data2"] = "2";
                                dt.Rows.Add(row);
                            }
                            nowDataGrid.DataSource = dt;
                            DataGridTableStyle ts = new DataGridTableStyle();
                            ts.MappingName = dt.TableName;  //映射style对应数据源的表名，很重要，否则无数据显示 
                            int numColumns = dt.Columns.Count;
                            DataGridTextBoxColumn aColumnTextColumn;
                            for (int j = 0; j < numColumns; j++)
                            {
                                aColumnTextColumn = new DataGridTextBoxColumn();
                                aColumnTextColumn.MappingName = dt.Columns[j].ColumnName; //映射数据源的列名，很重要，否则无数据显示
                                if (j % 2 == 0)
                                {
                                    aColumnTextColumn.Width = nowDataGrid.Width / 2 - 100;
                                }
                                else
                                {
                                    aColumnTextColumn.Width = 80;
                                }
                                ts.GridColumnStyles.Add(aColumnTextColumn);
                            }
                            nowDataGrid.TableStyles.Add(ts);
                            break;
                        case 4:
                            DataTable dt4 = new DataTable("newTable");
                            tempStr = DataAllTitleStr[cMain.IndexLanguage].Split(',');
                            dt4.Columns.Add("name0", typeof(string));
                            dt4.Columns.Add("name1", typeof(string));
                            dt4.Columns.Add("name2", typeof(string));
                            dt4.Columns.Add("name3", typeof(string));
                            dt4.Columns.Add("name4", typeof(string));
                            dt4.Columns.Add("name5", typeof(string));
                            dt4.Columns.Add("name6", typeof(string));
                            dt4.Columns.Add("name7", typeof(string));
                            dt4.Columns.Add("name8", typeof(string));
                            dt4.Columns.Add("name9", typeof(string));
                            int rowCount = 0;
                            rowCount = (int)Math.Ceiling((double)(DataAll / 2.000));
                            for (int j = 0; j < rowCount; j++)
                            {
                                DataRow row = dt4.NewRow();
                                row["name0"] = tempStr[j];
                                //row["data1"] = "0";
                                if (j + rowCount < DataAll)
                                {
                                    row["name5"] = tempStr[j + rowCount];
                                }
                                //row["data2"] = "0";
                                dt4.Rows.Add(row);
                            }
                            nowDataGrid.DataSource = dt4;
                            DataGridTableStyle ts4 = new DataGridTableStyle();
                            ts4.MappingName = dt4.TableName;  //映射style对应数据源的表名，很重要，否则无数据显示 
                            int numColumns4 = dt4.Columns.Count;
                            DataGridTextBoxColumn aColumnTextColumn4;
                            for (int j = 0; j < numColumns4; j++)
                            {
                                aColumnTextColumn4 = new DataGridTextBoxColumn();
                                aColumnTextColumn4.MappingName = dt4.Columns[j].ColumnName; //映射数据源的列名，很重要，否则无数据显示
                                switch(j)
                                {
                                    case 0:
                                    case 2:
                                    case 3:
                                    case 7:
                                    case 8:
                                        aColumnTextColumn4.Width = nowDataGrid.Width / 10 - 20;
                                        break;
                                    default:
                                        aColumnTextColumn4.Width = nowDataGrid.Width / 10 + 20; ;
                                        break;
                                }
                                ts4.GridColumnStyles.Add(aColumnTextColumn4);
                            }
                            nowDataGrid.TableStyles.Add(ts4);
                            break;
                        case 3:
                            DataTable dt3 = new DataTable("newTable");
                            tempStr = XiTongStr[cMain.IndexLanguage].Split(',');
                            dt3.Columns.Add("name1", typeof(string));
                            dt3.Columns.Add("data1", typeof(string));
                            dt3.Columns.Add("name2", typeof(string));
                            dt3.Columns.Add("data2", typeof(string));
                            for (int j = 0; j < 10; j++)
                            {
                                DataRow row = dt3.NewRow();
                                row["name1"] = tempStr[j];
                                //row["data1"] = "0";
                                row["name2"] = tempStr[j + 10];
                                //row["data2"] = "0";
                                dt3.Rows.Add(row);
                            }
                            nowDataGrid.DataSource = dt3;
                            DataGridTableStyle ts3 = new DataGridTableStyle();
                            ts3.MappingName = dt3.TableName;  //映射style对应数据源的表名，很重要，否则无数据显示 
                            int numColumns3 = dt3.Columns.Count;
                            DataGridTextBoxColumn aColumnTextColumn3;
                            for (int j = 0; j < numColumns3; j++)
                            {
                                aColumnTextColumn3 = new DataGridTextBoxColumn();
                                aColumnTextColumn3.MappingName = dt3.Columns[j].ColumnName; //映射数据源的列名，很重要，否则无数据显示
                                //if (j % 2 == 0)
                                //{
                                    aColumnTextColumn3.Width = nowDataGrid.Width / 4;
                                //}
                                //else
                                //{
                                //    aColumnTextColumn3.Width = 80;
                                //}
                                ts3.GridColumnStyles.Add(aColumnTextColumn3);
                            }
                            nowDataGrid.TableStyles.Add(ts3);
                            break;
                        case 6:
                            DataTable dt6 = new DataTable("newTable");
                            dt6.Columns.Add("是否启用", typeof(string));
                            dt6.Columns.Add("条码长度", typeof(string));
                            dt6.Columns.Add("条码起始位", typeof(string));
                            dt6.Columns.Add("条码识别码长度", typeof(string));
                            for (int i = 0; i < 10; i++)
                            {
                                DataRow dr = dt6.NewRow();
                                dt6.Rows.Add(dr);
                            } 
                            nowDataGrid.DataSource = dt6;
                            DataGridTableStyle ts6 = new DataGridTableStyle();
                            ts6.MappingName = dt6.TableName;  //映射style对应数据源的表名，很重要，否则无数据显示 
                            int numColumns6 = dt6.Columns.Count;
                            DataGridTextBoxColumn aColumnTextColumn6;
                            for (int j = 0; j < numColumns6; j++)
                            {
                                aColumnTextColumn6 = new DataGridTextBoxColumn();
                                aColumnTextColumn6.HeaderText = dt6.Columns[j].ColumnName;
                                aColumnTextColumn6.MappingName = dt6.Columns[j].ColumnName; //映射数据源的列名，很重要，否则无数据显示
                                if (j == 0)
                                {
                                    aColumnTextColumn6.Width = 120;
                                }
                                else
                                {
                                    aColumnTextColumn6.Width = (nowDataGrid.Width - 120) / 3;
                                }
                                ts6.GridColumnStyles.Add(aColumnTextColumn6);
                            }
                            nowDataGrid.TableStyles.Add(ts6);
                            break;
                        case 5:
                            DataTable dt5 = new DataTable("newTable");
                            dt5.Columns.Add("步骤名", typeof(string));
                            dt5.Columns.Add("是否合格", typeof(string));

                            tempStr = DataShowTitleStr[cMain.IndexLanguage].Split(',');
                            for (int i = 0; i < tempStr.Length; i++)
                            {
                                dt5.Columns.Add(tempStr[i], typeof(string));
                            }
                            for (int i = 0; i < cModeSet.StepCount; i++)
                            {
                                DataRow dr = dt5.NewRow();
                                dt5.Rows.Add(dr);
                            } 
                            nowDataGrid.DataSource = dt5;
                            DataGridTableStyle ts5 = new DataGridTableStyle();
                            ts5.MappingName = dt5.TableName;  //映射style对应数据源的表名，很重要，否则无数据显示 
                            int numColumns5 = dt5.Columns.Count;
                            DataGridTextBoxColumn aColumnTextColumn5;
                            for (int j = 0; j < numColumns5; j++)
                            {
                                aColumnTextColumn5 = new DataGridTextBoxColumn();
                                aColumnTextColumn5.HeaderText = dt5.Columns[j].ColumnName;
                                aColumnTextColumn5.MappingName = dt5.Columns[j].ColumnName; //映射数据源的列名，很重要，否则无数据显示
                                aColumnTextColumn5.Width = 120;
                                ts5.GridColumnStyles.Add(aColumnTextColumn5);
                            }
                            nowDataGrid.TableStyles.Add(ts5);
                            break;
                    }
                }
                if (FormEnum.Current is DataGridView)
                {
                    string[] tempStr;
                    nowDataGridView = (DataGridView)FormEnum.Current;
                    string intTag = nowDataGridView.Tag.ToString();
                    DataGridViewTextBoxColumn dataGridViewTextBoxColumn;
                    DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn;
                    DataGridViewComboBoxCell dataGridViewComboBoxCell;
                    switch (intTag)
                    {
                        case "SystemSet":
                            dataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
                            dataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            dataGridViewTextBoxColumn.DefaultCellStyle.BackColor = System.Drawing.Color.LightPink;
                            dataGridViewTextBoxColumn.HeaderText = "Name,参数名称".Split(',')[cMain.IndexLanguage];
                            dataGridViewTextBoxColumn.ValueType = typeof(string);
                            nowDataGridView.Columns.Add(dataGridViewTextBoxColumn);
                            nowDataGridView.Columns[0].ReadOnly = true;

                            dataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
                            dataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            //dataGridViewTextBoxColumn.DefaultCellStyle.BackColor = System.Drawing.Color.LightPink;
                            dataGridViewTextBoxColumn.HeaderText = "Data,参数值".Split(',')[cMain.IndexLanguage];
                            dataGridViewTextBoxColumn.ValueType = typeof(string);
                            nowDataGridView.Columns.Add(dataGridViewTextBoxColumn);

                            for (int i = 0; i < cMain.XiTongStr[cMain.IndexLanguage].Split(',').Length; i++)
                            {
                                nowDataGridView.Rows.Add();
                                DataGridViewRow dr = nowDataGridView.Rows[i];
                                dr.Cells[0].Value = cMain.XiTongStr[cMain.IndexLanguage].Split(',')[i];
                            }
                            nowDataGridView.Rows[0].Cells[1].ReadOnly = true;//ID号只读
                            nowDataGridView.Rows[1].Cells[1].ReadOnly = true;//上一次条码只读

                            for (int i = 0; i < 5; i++)
                            {
                                dataGridViewComboBoxCell = new DataGridViewComboBoxCell();//使用串口
                                dataGridViewComboBoxCell.Items.Clear();
                                foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
                                {
                                    dataGridViewComboBoxCell.Items.Add(s);
                                }
                                if (dataGridViewComboBoxCell.Items.Count < 4)
                                {
                                    dataGridViewComboBoxCell.Items.Clear();
                                    for (int j = 0; j < 20; j++)
                                    {
                                        dataGridViewComboBoxCell.Items.Add(string.Format("COM{0}", j + 1));
                                    }
                                }
                                nowDataGridView.Rows[i + 2].Cells[1] = dataGridViewComboBoxCell;
                            }

                            break;
                        case "KB":
                            for (int i = 0; i < 2; i++)//KB值用分左右两组数据
                            {
                                dataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
                                dataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                dataGridViewTextBoxColumn.DefaultCellStyle.BackColor = System.Drawing.Color.LightPink;
                                dataGridViewTextBoxColumn.HeaderText = "Name,参数名称".Split(',')[cMain.IndexLanguage];
                                dataGridViewTextBoxColumn.ValueType = typeof(string);
                                nowDataGridView.Columns.Add(dataGridViewTextBoxColumn);

                                dataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
                                dataGridViewCheckBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                dataGridViewCheckBoxColumn.HeaderText = "Use?,是否计量".Split(',')[cMain.IndexLanguage];
                                dataGridViewCheckBoxColumn.ValueType = typeof(bool);
                                nowDataGridView.Columns.Add(dataGridViewCheckBoxColumn);

                                dataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
                                dataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                dataGridViewTextBoxColumn.DefaultCellStyle.Format = "0.000";
                                dataGridViewTextBoxColumn.HeaderText = "Read,读取值".Split(',')[cMain.IndexLanguage];
                                dataGridViewTextBoxColumn.ValueType = typeof(double);
                                nowDataGridView.Columns.Add(dataGridViewTextBoxColumn);

                                dataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
                                dataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                dataGridViewTextBoxColumn.DefaultCellStyle.Format = "0.000";
                                dataGridViewTextBoxColumn.HeaderText = "K Value,计量K值".Split(',')[cMain.IndexLanguage];
                                dataGridViewTextBoxColumn.ValueType = typeof(double);
                                nowDataGridView.Columns.Add(dataGridViewTextBoxColumn);

                                dataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
                                dataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                dataGridViewTextBoxColumn.DefaultCellStyle.Format = "0.000";
                                dataGridViewTextBoxColumn.HeaderText = "B Value,计量B值".Split(',')[cMain.IndexLanguage];
                                dataGridViewTextBoxColumn.ValueType = typeof(double);
                                nowDataGridView.Columns.Add(dataGridViewTextBoxColumn);


                                dataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
                                dataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                dataGridViewTextBoxColumn.DefaultCellStyle.Format = "0.000";
                                dataGridViewTextBoxColumn.HeaderText = "Reality,实际值".Split(',')[cMain.IndexLanguage];
                                dataGridViewTextBoxColumn.ValueType = typeof(double);
                                nowDataGridView.Columns.Add(dataGridViewTextBoxColumn);
                            }

                            int tempLen = (int)Math.Ceiling(DataAll / 2.00);
                            tempStr = DataAllTitleStr[cMain.IndexLanguage].Split(',');
                            for (int i = 0; i < tempLen; i++)
                            {
                                nowDataGridView.Rows.Add();
                                DataGridViewRow dr = nowDataGridView.Rows[i];
                                dr.Cells[0].Value = tempStr[i];
                                if ((i + tempLen) < tempStr.Length)
                                {
                                    dr.Cells[6].Value = tempStr[i + tempLen];
                                }
                            }
                            break;
                        case "ProtectSet":
                            dataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
                            dataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            dataGridViewTextBoxColumn.HeaderText = "Name,参数项一".Split(',')[cMain.IndexLanguage];
                            dataGridViewTextBoxColumn.ValueType = typeof(string);
                            dataGridViewTextBoxColumn.ReadOnly = true;
                            nowDataGridView.Columns.Add(dataGridViewTextBoxColumn);

                            dataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
                            dataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            dataGridViewTextBoxColumn.HeaderText = "Data,数据项一".Split(',')[cMain.IndexLanguage];
                            dataGridViewTextBoxColumn.ValueType = typeof(string);
                            nowDataGridView.Columns.Add(dataGridViewTextBoxColumn);

                            dataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
                            dataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            dataGridViewTextBoxColumn.HeaderText = "Name,参数项二".Split(',')[cMain.IndexLanguage];
                            dataGridViewTextBoxColumn.ReadOnly = true;
                            dataGridViewTextBoxColumn.ValueType = typeof(string);
                            nowDataGridView.Columns.Add(dataGridViewTextBoxColumn);

                            dataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
                            dataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            dataGridViewTextBoxColumn.HeaderText = "Data,数据项二".Split(',')[cMain.IndexLanguage];
                            dataGridViewTextBoxColumn.ValueType = typeof(string);
                            nowDataGridView.Columns.Add(dataGridViewTextBoxColumn);

                            tempStr = BaoHuStr[cMain.IndexLanguage].Split(',');
                            for (int i = 0; i < 10; i++)
                            {
                                nowDataGridView.Rows.Add();
                                DataGridViewRow dr = nowDataGridView.Rows[i];
                                dr.Cells[0].Value = tempStr[i];
                                dr.Cells[2].Value = tempStr[i + 10];
                            }
                            break;
                        case "BarCodeSet":
                            dataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
                            dataGridViewCheckBoxColumn.HeaderText = "是否启用";
                            dataGridViewCheckBoxColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            dataGridViewCheckBoxColumn.ValueType = typeof(bool);
                            nowDataGridView.Columns.Add(dataGridViewCheckBoxColumn);

                            dataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
                            dataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            dataGridViewTextBoxColumn.HeaderText = "Barcode Length,条码长度".Split(',')[cMain.IndexLanguage];
                            dataGridViewTextBoxColumn.ValueType = typeof(int);
                            nowDataGridView.Columns.Add(dataGridViewTextBoxColumn);

                            dataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
                            dataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            dataGridViewTextBoxColumn.HeaderText = "ID Start,ID起始位".Split(',')[cMain.IndexLanguage];
                            dataGridViewTextBoxColumn.ValueType = typeof(int);
                            nowDataGridView.Columns.Add(dataGridViewTextBoxColumn);

                            dataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
                            dataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            dataGridViewTextBoxColumn.HeaderText = "ID Length,ID长度".Split(',')[cMain.IndexLanguage];
                            dataGridViewTextBoxColumn.ValueType = typeof(int);
                            nowDataGridView.Columns.Add(dataGridViewTextBoxColumn);

                            for (int i = 0; i < 10; i++)
                            {
                                nowDataGridView.Rows.Add();
                                DataGridViewRow dr = nowDataGridView.Rows[i];
                                dr.HeaderCell.Value = string.Format("{0}", i + 1);
                            }
                            break;
                        case "DataShow":
                            tempStr = cMain.DataShowTitleStr[cMain.IndexLanguage].Split(',');
                            for (int i = 0; i < DataShow; i++)
                            {
                                dataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
                                dataGridViewCheckBoxColumn.HeaderText = tempStr[i];
                                dataGridViewCheckBoxColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                dataGridViewCheckBoxColumn.ValueType = typeof(bool);
                                nowDataGridView.Columns.Add(dataGridViewCheckBoxColumn);
                            }
                            nowDataGridView.Rows.Add(1);
                            break;
                        case "UpAndDownSet":
                            DataGridViewComboBoxColumn dataGridViewComboBoxColumn = new DataGridViewComboBoxColumn();
                            tempStr = cMain.BuZhouMingStr[cMain.IndexLanguage].Split(',');
                            for (int i = 0; i < tempStr.Length; i++)
                            {
                                dataGridViewComboBoxColumn.Items.Add(tempStr[i]);
                            }
                            dataGridViewComboBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            dataGridViewComboBoxColumn.HeaderText = "Name,步骤名".Split(',')[cMain.IndexLanguage];
                            dataGridViewComboBoxColumn.ValueType = typeof(string);
                            nowDataGridView.Columns.Add(dataGridViewComboBoxColumn);

                            dataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
                            dataGridViewTextBoxColumn.HeaderText = "Time,时间".Split(',')[cMain.IndexLanguage];
                            dataGridViewTextBoxColumn.ValueType = typeof(int);
                            nowDataGridView.Columns.Add(dataGridViewTextBoxColumn);

                            dataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
                            dataGridViewTextBoxColumn.HeaderText = "Sn Code,发送指令/频率".Split(',')[cMain.IndexLanguage];
                            dataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                            dataGridViewTextBoxColumn.ValueType = typeof(string);
                            nowDataGridView.Columns.Add(dataGridViewTextBoxColumn);

                            tempStr = cMain.KaiGuangStr[cMain.IndexLanguage].Split(',');//开关量
                            for (int i = 0; i < DataKaiGuang; i++)
                            {
                                dataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
                                dataGridViewCheckBoxColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                dataGridViewCheckBoxColumn.ValueType = typeof(bool);
                                nowDataGridView.Columns.Add(dataGridViewCheckBoxColumn);
                                if (i < tempStr.Length)
                                {
                                    dataGridViewCheckBoxColumn.HeaderText = tempStr[i];
                                    if (tempStr[i].IndexOf("SN") > 0)
                                    {
                                        dataGridViewCheckBoxColumn.DefaultCellStyle.BackColor = System.Drawing.Color.Pink;
                                    }
                                }
                                else
                                {
                                    nowDataGridView.Columns[3 + i].Visible = false;
                                }
                            }
                            tempStr = cMain.DataShowTitleStr[cMain.IndexLanguage].Split(',');
                            for (int i = 0; i < DataShow * 2; i++)
                            {
                                dataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
                                if ((i % 2) == 0)
                                {
                                    dataGridViewTextBoxColumn.HeaderText = string.Format("{0}_Min", tempStr[i / 2]);
                                }
                                else
                                {
                                    dataGridViewTextBoxColumn.HeaderText = string.Format("{0}_Max", tempStr[(i - 1) / 2]);
                                }
                                dataGridViewTextBoxColumn.ValueType = typeof(double);
                                nowDataGridView.Columns.Add(dataGridViewTextBoxColumn);
                            }
                            nowDataGridView.Rows.Add(cModeSet.StepCount);
                            for (int i = 0; i < cModeSet.StepCount; i++)
                            {
                                nowDataGridView.Rows[i].HeaderCell.Value = string.Format("{0}", i + 1);
                                if ((i % 2) == 1)
                                {
                                    nowDataGridView.Rows[i].DefaultCellStyle.BackColor = System.Drawing.SystemColors.ControlLight;
                                }
                            }
                            break;
                    }
                }
            }
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
    /// <summary>
    /// 读取D点解析
    /// </summary>
    public enum MPoint
    {
        /// <summary>
        /// 正收停机
        /// </summary>
        ZYSFAdd = 4,//isOK
        /// <summary>
        /// 下一步制冷
        /// </summary>
        NextColdAdd = 8,
        /// <summary>
        /// 下一步制热
        /// </summary>
        NextHotAdd = 2,
        /// <summary>
        /// 下一步低启
        /// </summary>
        NextDiQiAdd = 16,
        /// <summary>
        /// 行程开关
        /// </summary>
        XingChengAdd =1,//isOk
        /// <summary>
        /// 手动停止
        /// </summary>
        StopAdd = 64,//isOK
        /// <summary>
        /// 手动开始
        /// </summary>
        StartAdd = 32//isOk
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
    }
    /// <summary>
    /// 机型设置
    /// </summary>
    [Serializable]
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
        public string[] mStepId = new string[cModeSet.StepCount]; //步骤ID 0~4
        /// <summary>
        /// 设定步骤检测时间,10元素1维数组
        /// </summary>
        public int[] mSetTime = new int[cModeSet.StepCount];//检测时间
        /// <summary>
        /// 开关量,10*cMain.DataKaiGuang元素2维数组
        /// </summary>
        public bool[,] mKaiGuan = new bool[cModeSet.StepCount, cMain.DataKaiGuang];
        /// <summary>
        /// 发送指令,10元素1维数组
        /// </summary>
        public string[] mSendStr = new string[cModeSet.StepCount];
        /// <summary>
        /// 数据上限,10*20元素2维数组
        /// </summary>
        public Single[,] mHighData = new Single[cModeSet.StepCount, cMain.DataShow];
        /// <summary>
        /// 数据下限,10*20元素2维数组
        /// </summary>
        public Single[,] mLowData = new Single[cModeSet.StepCount, cMain.DataShow];
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
            for (i = 0; i < cModeSet.StepCount; i++)
            {
                mStepId[i] =(cMain.IndexLanguage==0)?"Heating": "制热";//检测步骤
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
        public string mPLCCOM = "COM2";
        /// <summary>
        /// 条码使用端口
        /// </summary>
        public string mBarCom = "COM6";
        /// <summary>
        /// 报警模式
        /// </summary>
        public string m485COM = "COM3";
        /// <summary>
        /// PQ端口
        /// </summary>
        public string mPQCom = "COM4";
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
        public double[] PressK = new double[2];
        /// <summary>
        /// 压力变比B,4个元素的1维数组
        /// </summary>
        public double[] PressB = new double[2];
        public int ZYSFArea = 0;
        public int ZYSFDoing = 1;
        public cSystemSet()
        {
            int i;
            for (i = 0; i < PressK.Length; i++)
            {
                PressK[i] = 0.01;
                PressB[i] = 0;
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
        /// <summary>
        /// 取对应序号的计量后值
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public double ShowValue(int index)
        {
            if (index >= 0 && index < cMain.DataAll)
            {
                return frmMain.dataRead[index] * valueK[index] + valueB[index];
            }
            return 0;
        }
    }
    public class cAllResult
    {
        cModeSet modeSet;

        public cModeSet ModeSet
        {
            get { return modeSet; }
            set { modeSet = value; }
        }
        cRunResult runResult;

        public cRunResult RunResult
        {
            get { return runResult; }
            set { runResult = value; }
        }
        cStepResult[] stepResult = new cStepResult[cModeSet.StepCount];

        public cStepResult[] StepResult
        {
            get { return stepResult; }
            set { stepResult = value; }
        }
        public cAllResult()
        {
            Init();
        }
        public void Init()
        {
            ModeSet = new cModeSet();
            RunResult = new cRunResult();
            StepResult = new cStepResult[cModeSet.StepCount];
            for (int i = 0; i < StepResult.Length; i++)
            {
                StepResult[i] = new cStepResult();
            }
        }
        public void SetStepResult(int index,cStepResult stepResult)//堆栈差别
        {
            StepResult[index].mIsStepPass = stepResult.mIsStepPass;
            for (int i = 0; i < cMain.DataShow; i++)
            {
                StepResult[index].mData[i] = stepResult.mData[i];
                StepResult[index].mIsDataPass[i] = stepResult.mIsDataPass[i];
            }
        }
        public void Save()
        {
            DateTime now = DateTime.Now;
            int stepIndex = 0;
            int testCount = 0;
            string testValue = "";
            string fileName = string.Format("{0}\\{1:yyyyMMddHHmmss}{2}.txt", cMain.LocalSaveValue.MesDirectory,now, RunResult.mBar);
            string fileValue = string.Format("{0};", RunResult.mBar); //条码
            fileValue = string.Format("{0}{1:yyyy-MM-dd};{1:HH:mm:ss};", fileValue, now);//时间
            fileValue = string.Format("{0}{1};", fileValue, RunResult.mIsPass);//结果
            fileValue = string.Format("{0}{1};", fileValue, "MD");//用户
            fileValue = string.Format("{0}{1};", fileValue, cUdpSock.LastIp());
            fileValue = string.Format("{0}{1};", fileValue, "性能测试");
            fileValue = string.Format("{0}{1};", fileValue, cUdpSock.LoaclIp().ToString());
            for (int i = 0; i < StepResult.Length && i < ModeSet.mStepId.Length; i++)
            {
                if (ModeSet.mSetTime[i] > 0)
                {
                    fileValue = string.Format("{0}\r\n", fileValue);
                    stepIndex++;
                    fileValue = string.Format("{0}{1};", fileValue, stepIndex);
                    fileValue = string.Format("{0}{1};", fileValue, ModeSet.mStepId[i]);
                    fileValue = string.Format("{0}{1};", fileValue, (StepResult[i].mIsStepPass != 0));
                    testCount = 0;
                    testValue = "";
                    for (int j = 0; j < ModeSet.mShow.Length; j++)
                    {
                        if (ModeSet.mShow[j])
                        {
                            testCount++;
                            testValue = string.Format("{0}\r\n\t{1};{2:F2};{3};{4:F2};{5:F2}",
                                testValue,cMain.DataShowTitle[j], StepResult[i].mData[j],
                                (StepResult[i].mIsDataPass[j] != 0), ModeSet.mLowData[i,j], ModeSet.mHighData[i,j]);
                        }
                    }
                    fileValue = string.Format("{0}{1}{2}", fileValue, testCount, testValue);
                }
            }
            cMain.WriteFile(fileName, fileValue, false);
        }
    }
    [Serializable]
    public class cNetResult//传回上位机数据
    {
        public cRunResult RunResult = new cRunResult();
        public cStepResult StepResult = new cStepResult();
    }
    [Serializable]
    public class cMesResult
    {
        public cRunResult RunResult = new cRunResult();
        public List<cStepResult> StepResult = new List<cStepResult>();
        public cMesResult()
        {
            Init();
        }
        public void Init()
        {
            RunResult = new cRunResult();
            StepResult.Clear();
        }
    }
    [Serializable]
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
    [Serializable]
    public class cRunResult//正在运行机器信息结果
    {
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
        /// 检测日期,时间
        /// </summary>
        public DateTime mTestTime = DateTime.Now;
        /// <summary>
        /// 检测总结果是否合格
        /// </summary>
        public bool mIsPass = true;
        /// <summary>
        /// 步骤ID ,当前检测第几步
        /// </summary>
        public int mStepId = -1; //步骤ID 1~10
        /// <summary>
        /// 步骤号,当前检测步骤检测项目
        /// </summary>
        public string mStep = ""; //步骤号 
        /// <summary>
        /// 曲线存放路径
        /// </summary>
        public string mQuXianImage = "";
        /// <summary>
        /// 曲线图值　
        /// </summary>
        public string QuXianValue = "";
    }
    /// <summary>
    /// 单步检测数据
    /// </summary>
    [Serializable]
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
            for(i=0;i<cMain.DataShow;i++)
            {
                mData[i]=0;
                mIsDataPass[i]=-1;
            }
        }
    }
    /// <summary>
    /// 条码设置
    /// </summary>
    public class cBarSet
    {
        /// <summary>
        /// 扫描条码后是否自动启动
        /// </summary>
        public bool mIsAutoStart = true;
        /// <summary>
        /// 是否使用本机条码识别方法,或者使用远程电脑识别方法
        /// </summary>
        public bool mIsWinCeBar = true;
        /// <summary>
        /// 此条码设置是否使用
        /// </summary>
        public bool[] mIsUse = new bool[10];
        /// <summary>
        /// 条码长度
        /// </summary>
        public int[] mIntBarLength = new int[10];
        /// <summary>
        /// 条码识别码开始位
        /// </summary>
        public int[] mIntBarStart = new int[10];
        /// <summary>
        /// 条码识别码长度
        /// </summary>
        public int[] mIntBarCount = new int[10];
        public cBarSet()
        {
            for (int i = 0; i < 10; i++)
            {
                mIsUse[i] = false;
                mIntBarCount[i] = 0;
                mIntBarStart[i] = 0;
                mIntBarLength[i] = 0;
            }
        }
    }
    public class LocalSaveValue
    {
        static string filePath = string.Format("{0}\\LocalSaveValue.txt", Application.StartupPath);
        int formIndex = 0;
        /// <summary>
        /// 界面方案
        /// </summary>
        public int FormIndex
        {
            get { return formIndex; }
            set { formIndex = value; }
        }
        string mesDirectory = "D:\\datatxt\\";
        /// <summary>
        /// MES文件夹
        /// </summary>
        public string MesDirectory
        {
            get { return mesDirectory; }
            set { mesDirectory = value; }
        }
        public LocalSaveValue()
        {
            FormIndex = 0;
            MesDirectory = "D:\\datatxt\\";
        }
        public void Load()
        {
            LocalSaveValue tmp = (LocalSaveValue)cXml.readXml(filePath, typeof(LocalSaveValue), new LocalSaveValue());
            this.FormIndex = tmp.FormIndex;
            this.MesDirectory = tmp.MesDirectory;
            Save();
        }
        public void Save()
        {
            cXml.saveXml(filePath, typeof(LocalSaveValue), this);
        }

    }
}
