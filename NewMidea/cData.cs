using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
namespace NewMideaProgram
{
    static class cData
    {
        public static OleDbConnection ConnMain = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|\\Data\\Main.mdb;Persist Security Info=True");
        public static OleDbConnection ConnData = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|\\Data\\Data.mdb;Persist Security Info=True");
        public static ADODB.Connection _ConnData = new ADODB.Connection();
        
        public static bool SaveJianCeData(cNetResult mNetResult)
        {
            bool returnResult = false;
            int i;
            string sqlCommand = "Insert into AllData values({0}{1}{2})";
            string tempStrHead = "#{0:yyyy-MM-dd HH:mm:ss}#,'{1}','{2}','{3}',{4},{5},'{6}',{7},'{8}'";
            string tempStrData = "";
            string tempStrBool = "";
            bool stepResult = true;
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
                returnResult = false;
            }
            return returnResult;
        }
        public static DataSet readData(string sqlCommand, OleDbConnection conn)
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
        public static int upData(string sqlCommand, OleDbConnection conn)
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
                cMain.WriteErrorToLog("cData upData is Error " + sqlCommand + "\r\n" + exc.ToString());
                x = 0;
            }
            return x;//返回受影响的行数
        }
        public static void SaveMes(cMesResult netResult)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(netResult.RunResult.mBar);
            sb.Append(";");
            sb.Append(string.Format("{0:yyyy-MM-dd};{0:HH:mm:ss}", netResult.RunResult.mTestTime));
            sb.Append(";");
            sb.Append(netResult.RunResult.mIsPass);
            sb.Append(";");
            sb.Append("MD");
            sb.Append(";");
        }
    }

}
