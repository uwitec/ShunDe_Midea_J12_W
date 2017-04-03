using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
namespace PcProgram
{
   static class cData
    {
       public static OleDbConnection ConnMain = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|\\Data\\Main.mdb;Persist Security Info=True");
       public static OleDbConnection ConnData = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|\\Data\\Data.mdb;Persist Security Info=True");
       public static ADODB.Connection _ConnData = new ADODB.Connection();

       public static bool GetIdFromBar(string bar,ref string Id,out string ErrorStr)
       {
           bool returnResult = false;
           int lenBar = bar.Length;
           int i;
           bool isFindBarSet = false;
           string sqlCommand = "select * from BarcodeSet where selected1=true";
           DataSet ds = readData(sqlCommand, ConnMain);
           ErrorStr = "";
           if (ds.Tables[0].Rows.Count > 0)
           {
               for (i = 0; i < ds.Tables[0].Rows.Count; i++)
               {
                   DataRow dr = ds.Tables[0].Rows[i];
                   if (Num.IntParse(dr["barcodeLen"]) == lenBar)
                   {
                       Id = bar.Substring(Num.IntParse(dr["startBit"]) - 1, Num.IntParse(dr["ModeIdLen"]));
                       isFindBarSet = true;
                       break;
                   }
               }
               if (isFindBarSet)
               {
                   returnResult = true;
               }
               else
               {
                   ErrorStr = "没有找到与当前扫描条码相符的条码设置,请检查条码设置";
               }
           }
           else
           {
               ErrorStr = "条码参数没有设置或者启用,请检查条码设置";
           }
           return returnResult;
       }
       public static bool GetICCard(out string[] ICCardNum)
       {
           bool returnResult = false;
           string[] tmpICCardNum = new string[cMain.AllCount];
           string sqlCommand = "select * from CardSet";
           try
           {
               DataSet ds = readData(sqlCommand, ConnMain);
               if (ds.Tables[0].Rows.Count > 0)
               {
                   ICCardNum = new string[ds.Tables[0].Rows.Count];
                   foreach (DataRow dr in ds.Tables[0].Rows)
                   {
                       int index = Num.IntParse(dr["HostId"]) - 1;
                       if (index >= 0 && index < cMain.AllCount)
                       {
                           tmpICCardNum[index] = dr["ICard"].ToString();
                       }
                   }
               }
               returnResult = true;
           }
           catch (Exception exc)
           {
               cMain.WriteErrorToLog("GetICCard is Error:" + exc.Message);
           }
           ICCardNum = tmpICCardNum;
           return returnResult;
       }
       //public static DataTable GetErrorReport(cNetResult mNetResult)
       //{
       //   cModeSet ModeSet=new cModeSet();
       //    DataTable dt = new DataTable();
       //    try
       //    {
       //        string ErrorStr = "";
       //        GetSetFromId(mNetResult.RunResult.mId, ref ModeSet, out ErrorStr);
       //        dt.Columns.Add("TestNo", typeof(int));
       //        dt.Columns.Add("Mode", typeof(string));
       //        dt.Columns.Add("BarCode", typeof(string));
       //        for (int i = 0; i < cMain.DataShow; i++)
       //        {
       //            dt.Columns.Add(string.Format("DataUp{0}", i + 1), typeof(double));
       //            dt.Columns.Add(string.Format("DataDown{0}", i + 1), typeof(double));
       //            dt.Columns.Add(string.Format("Data{0}", i + 1), typeof(double));
       //        }
       //        DataRow dr = dt.NewRow();
       //        dr["TestNo"] = mNetResult.RunResult.mTestNo;
       //        dr["Mode"] = mNetResult.RunResult.mMode;
       //        dr["BarCode"] = mNetResult.RunResult.mBar;
       //        for (int i = 0; i < cMain.DataShow; i++)
       //        {
       //            dr[string.Format("Data{0}", i + 1)] = mNetResult.StepResult.mData[i];
       //            dr[string.Format("DataUp{0}", i + 1)] = ModeSet.mHighData[mNetResult.RunResult.mStepId, i];
       //            dr[string.Format("DataDown{0}", i + 1)] = ModeSet.mLowData[mNetResult.RunResult.mStepId, i];
       //        }
       //        dt.Rows.Add(dr);
       //    }
       //    catch (Exception exc)
       //    {
       //        cMain.WriteErrorToLog("GetErrorReport is Error:" + exc.Message);
       //    }
       //    return dt;
       //}
       public static DataTable ReadWenDuData(string sql)
       {
           DataSet ds = readData(sql, ConnData);
           return ds.Tables[0];
       }
       public static bool SaveAnGui(string bar,cAiNuo9641B.AiNuo9641Data AiNuoData)
       {
           bool returnResult = false;
           try
           {
               string sqlCommand = "insert into WenShiDu values({0},'{1}'{2}{3})";
               string tempStrDate = string.Format("#{0}#", DateTime.Now);
               string tempStrValue = "";
               string tempStrPass = "";
               for (int i = 0; i < 4; i++)
               {
                   double tempData = 0;
                   bool tempPass = true;
                   for (int j = 0; j < AiNuoData.mAiNuo9641Data.Length; j++)
                   {
                       if ((int)AiNuoData.mAiNuo9641Data[j].mTestName == i)
                       {
                           tempData = AiNuoData.mAiNuo9641Data[j].mTestData;
                           tempPass = (AiNuoData.mAiNuo9641Data[j].mTestResult == cAiNuo9641B.TestStepResultEnum.OK) ? true : false;
                           break;
                       }
                   }
                   tempStrValue = tempStrValue + "," + string.Format("{0:F3}", tempData);
                   tempStrPass = tempStrPass + "," + string.Format("{0}", tempPass);
               }
               for (int i = 4; i < 7; i++)
               {
                   tempStrValue = tempStrValue + ",0";
                   tempStrPass = tempStrPass + ",True";
               }
               sqlCommand = string.Format(sqlCommand, tempStrDate,bar, tempStrValue, tempStrPass);
               //cMain.WriteErrorToLog(sqlCommand);
               if (upData(sqlCommand, cData.ConnData) > 0)
               {
                   returnResult = true;
               }
           }
           catch(Exception exc)
           {
               returnResult = false;
               cMain.WriteErrorToLog(exc.Message);
           }
           return returnResult;
       }
       //public static bool SaveWenDuData(DateTime dt, double[] WenShiDu)
       //{
       //    bool returnResult = false;
       //    try
       //    {
       //        string sqlCommand = "Insert into WenShiDu values({0}{1})";
       //        string tempStrDate = string.Format("#{0}#", dt);
       //        string tempStrValue = "";
       //        for (int i = 0; i < WenShiDu.Length; i++)
       //        {
       //            tempStrValue = tempStrValue + "," + string.Format("{0:F3}", WenShiDu[i]);
       //        }
       //        sqlCommand = string.Format(sqlCommand, tempStrDate, tempStrValue);
       //        if (upData(sqlCommand, cData.ConnData) > 0)
       //        {
       //            returnResult = true;
       //        }
       //    }
       //    catch (Exception exc)
       //    {
       //        returnResult = false;
       //        cMain.WriteErrorToLog(exc.Message);
       //    }
       //    return returnResult;
       //}
       public static bool SaveJianCeData(cNetResult mNetResult)
       {
           bool returnResult = false;
           int i;
           string sqlCommand = "Insert into AllData values({0}{1}{2})";
           string tempStrHead = "#{0}#,'{1}','{2}','{3}',{4},{5},'{6}',{7},'{8}'";
           string tempStrData = "";
           string tempStrBool = "";
           bool stepResult = true ;
           try
           {
               if (mNetResult.StepResult.mIsStepPass == 0)
               {
                   stepResult = false;
               }
               tempStrHead = string.Format(tempStrHead,
                   mNetResult.RunResult.mTestTime,
                   mNetResult.RunResult.mBar,
                   mNetResult.RunResult.mId,
                   mNetResult.RunResult.mMode,
                   mNetResult.RunResult.mTestNo,
                   mNetResult.RunResult.mJiQi,
                   stepResult.ToString(),
                   mNetResult.RunResult.mStepId,
                   mNetResult.RunResult.mStep);
               for (i = 0; i < 50; i++)
               {
                   if (i < cMain.DataShow)
                   {
                       tempStrData = tempStrData + "," + mNetResult.StepResult.mData[i].ToString();
                       if (mNetResult.StepResult.mIsDataPass[i] == 0)
                       {
                           tempStrBool = tempStrBool + ",false";
                       }
                       else
                       {
                           tempStrBool = tempStrBool + ",true";
                       }
                   }
                   else
                   {
                       tempStrData = tempStrData + ",0";
                       tempStrBool = tempStrBool + ",true";
                   }
               }
               sqlCommand = string.Format(sqlCommand, tempStrHead, tempStrData, tempStrBool);
               if (upData(sqlCommand, ConnData) > 0)
               {
                   returnResult = true;
               }
               else
               {
                   returnResult = false;
               }
           }
           catch (Exception exc)
           {
               cMain.WriteErrorToLog("cData SaveJianCeData is Error " + exc.ToString());
               returnResult= false;
           }
           return returnResult;
       }
       public static bool isLogin(string PassWord)
       {
           bool islogin = false;
           DataSet ds = new DataSet();
           ds = readData(string.Format("select * from users where password='{0}'",PassWord), ConnMain);
           if (ds.Tables[0].Rows.Count > 0)
           {
               islogin = true;    
           }
           return islogin;
       }

       public static DataSet readData (string sqlCommand,OleDbConnection conn)
       {
           DataSet ds = new DataSet();
           lock (conn)
           {
               OleDbCommand cmd = new OleDbCommand(sqlCommand, conn);
               OleDbDataAdapter oda = new OleDbDataAdapter(cmd);
               oda.Fill(ds);
               return ds;
           }
       }
       public static int upData(string sqlCommand,OleDbConnection conn)
       {
           int x = 0;
           try
           {
               if (conn.State == ConnectionState.Closed)
               {
                   conn.Open();
               }
               OleDbCommand cmd = new OleDbCommand(sqlCommand, conn);
               cmd.CommandType = CommandType.Text;
               x = cmd.ExecuteNonQuery();
           }
           catch (Exception exc)
           {
               cMain.WriteErrorToLog("cData upData is Error "+sqlCommand+"\r\n" + exc.ToString());
               x = 0;
           }
           return x;//返回受影响的行数
       }
   }

}
