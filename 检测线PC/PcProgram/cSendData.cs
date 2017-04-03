using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
namespace PcProgram
{
    class cSendData
    {
        int indexAtBar = 0;
        /// <summary>
        /// 在行程开关上的工位
        /// </summary>
        public int IndexAtBar
        {
            get { return indexAtBar; }
            set { indexAtBar = value; }
        }
        bool is_A_OK = false;//低启请求

        public bool Is_A_OK
        {
            get { return is_A_OK; }
            set { is_A_OK = value; }
        }
        bool[] is_B_OK =new bool[cMain.AllCount];// false;//发送条码

        public bool[] Is_B_OK
        {
            get { return is_B_OK; }
            set { is_B_OK = value; }
        }
        bool is_S_OK = false;//发送系统设置

        public bool Is_S_OK
        {
            get { return is_S_OK; }
            set { is_S_OK = value; }
        }
        bool is_T_OK = false;//停机

        public bool Is_T_OK
        {
            get { return is_T_OK; }
            set { is_T_OK = value; }
        }
        bool is_N_OK = false;//下一步

        public bool Is_N_OK
        {
            get { return is_N_OK; }
            set { is_N_OK = value; }
        }
        bool is_K_OK = false;//开始

        public bool Is_K_OK
        {
            get { return is_K_OK; }
            set { is_K_OK = value; }
        }
        bool is_U_OK = false;//更新

        public bool Is_U_OK
        {
            get { return is_U_OK; }
            set { is_U_OK = value; }
        }
        bool is_D_OK = false;//当前实时数据

        public bool Is_D_OK
        {
            get { return is_D_OK; }
            set { is_D_OK = value; }
        }

        bool is_J_OK = false;//下位机上传检测数据

        public bool Is_J_OK
        {
            get { return is_J_OK; }
            set { is_J_OK = value; }
        }
        bool is_R_OK = false;//下位机上传条码

        public bool Is_R_OK
        {
            get { return is_R_OK; }
            set { is_R_OK = value; }
        }
        bool is_O_OK = false;//下位机结束时,发送是否合格,用来统计数据

        public bool Is_O_OK
        {
            get { return is_O_OK; }
            set { is_O_OK = value; }
        }
        bool is_E_Ok = false;

        public bool Is_E_Ok
        {
            get { return is_E_Ok; }
            set { is_E_Ok = value; }
        }

        cNetResult mNetResult = new cNetResult();

        public cNetResult MNetResult
        {
            get { return mNetResult; }
            set { mNetResult = value; }
        }
        bool isSetSendBarFrm = false;

        public bool IsSetSendBarFrm
        {
            get { return isSetSendBarFrm; }
            set { isSetSendBarFrm = value; }
        }
        FrmAutoTest sendBarFrm;

        public FrmAutoTest SendBarFrm
        {
            set { sendBarFrm = value; }
        }
        public cUdpSock[] McgsUdp = new cUdpSock[cMain.AllCount];
        public static int[] lastGetDataTime = new int[cMain.AllCount];
        public cSendData()
        {
            for (int i = 0; i < cMain.AllCount; i++)
            {
                McgsUdp[i] = new cUdpSock(3001 + i, 3000, "192.168.1." + (i + 101).ToString());
                McgsUdp[i].mDataReciveString += new cUdpSock.mDataReciveStringEventHandle(FrmAutoTest_mDataReciveString);
                lastGetDataTime[i] = Environment.TickCount;
            }
        }
        private void FrmAutoTest_mDataReciveString(object o, RecieveStringArgs e)
        {
            string[] tempStr = e.RemostHost.Split('.');
            int RemoteNum = Num.IntParse(tempStr[3]) - 101;
            cMain.isUdpInitError[RemoteNum] = true;
            string readStr = e.ReadStr;
            if (readStr.IndexOf("A~OK") >= 0)
            {
                string[] tempstr = readStr.Split('~');
                int remoteNum = Num.IntParse(tempstr[3]);
                switch (tempstr[2])
                {
                    case "OVER":
                        McgsUdp[remoteNum - 1].fUdpSend("A~OK~OVER");
                        cMain.isAccessDiQi = cMain.isAccessDiQi.Replace("," + string.Format("{0:D2},", remoteNum), ",");
                        break;
                    case "HOLD":
                        //McgsUdp[remoteNum - 1].fUdpSend("A~OK~HOLD");
                        if (cMain.isAccessDiQi.IndexOf(string.Format(",{0:D2},", remoteNum)) < 0)
                        {
                            cMain.isAccessDiQi = cMain.isAccessDiQi + string.Format("{0:D2},", remoteNum);
                        }
                        break;
                    case "ACCESS":
                        return;
                }
                tempstr = null;
                tempstr = cMain.isAccessDiQi.Split(',');
                if (tempstr.Length >= 3)
                {
                    McgsUdp[Num.IntParse(tempstr[1]) - 1].fUdpSend("A~OK~ACCESS");
                }
            }
            if (readStr.IndexOf("E~OK") >= 0)//关机返回
            {
                is_E_Ok = true;
            }
            if (readStr.IndexOf("B~OK") >= 0)//条码
            {
                is_B_OK[RemoteNum] = true;
            }
            if (readStr.IndexOf("S~OK") >= 0)//系统设置
            {
                is_S_OK = true;
            }
            if (readStr.IndexOf("T~OK") >= 0)//停机
            {
                is_T_OK = true;
            }
            if (readStr.IndexOf("N~OK") >= 0)//下一步
            {
                is_N_OK = true;
            }
            if (readStr.IndexOf("K~OK") >= 0)//开始
            {
                is_K_OK = true;
            }
            if (readStr.IndexOf("U~OK") >= 0)//更新
            {
                is_U_OK = true;
            }
            if (readStr.IndexOf("D~OK") >= 0)//当前实时数据
            {
                is_D_OK = true;
                mNetResult = GetNetResult(readStr);
                cMain.mCurrentData[mNetResult.RunResult.mTestNo - 1] = mNetResult;
                lastGetDataTime[mNetResult.RunResult.mTestNo - 1] = Environment.TickCount;
            }
            if (readStr.IndexOf("J~OK") >= 0)
            {
                is_J_OK = true;
                cMain.mNetResult = GetNetResult(readStr);
                cData.SaveJianCeData(cMain.mNetResult);
                cMain.mTempNetResult[cMain.mNetResult.RunResult.mTestNo - 1].RunResult[cMain.mNetResult.RunResult.mStepId] = cMain.mNetResult.RunResult;
                cMain.mTempNetResult[cMain.mNetResult.RunResult.mTestNo - 1].StepResult[cMain.mNetResult.RunResult.mStepId] = cMain.mNetResult.StepResult;
                McgsUdp[cMain.mNetResult.RunResult.mTestNo - 1].fUdpSend("J~OK");
                //if ((cMain.isPrint) && (cMain.mNetResult.RunResult.mId != "") && (cMain.mNetResult.StepResult.mIsStepPass == 0))
                //{
                //    DataTable tmpDt = cData.GetErrorReport(cMain.mNetResult);
                //    frmPrint fp = new frmPrint(tmpDt);
                //    fp.Show();
                //    if (!fp.IsDisposed)
                //    {
                //        fp.Close();
                //        fp.Dispose();
                //    }
                //}
            }
            if (readStr.IndexOf("R~OK") >= 0)//此处已实现条码读取上来,并发送成功标志,但上位机这头暂时未处理.(防止和232串口读条码不兼容)
            {
                is_R_OK = true;
                string[] tempstr = readStr.Split('~');
                indexAtBar = Num.IntParse(tempstr[2]) - 1;
                McgsUdp[indexAtBar].fUdpSend("R~OK");
                if (isSetSendBarFrm)
                {
                    sendBarFrm.SendBarMethod(tempstr[3]);
                }
            }
            if (readStr.IndexOf("X~OK") >= 0)
            {
                string[] tempstr = readStr.Split('~');
                indexAtBar = Num.IntParse(tempstr[2]) - 1;
            }
            if (readStr.IndexOf("O~OK") >= 0)
            {
                string[] tempstr = readStr.Split('~');
                cMain.mTodayData.TestCount[0]++;
                if (Num.BoolParse(tempstr[3]))
                {
                    cMain.mTodayData.TestCount[1]++;
                }
                else
                {
                    cMain.mTodayData.TestCount[2]++;
                }
                for (int i = 0; i < cMain.mTodayData.TestCount.Length; i++)
                {
                    cTodayData.WriteToday("TestCount" + i.ToString(), cMain.mTodayData.TestCount[i].ToString());
                }
            }
        }

        private cNetResult GetNetResult(string McgsSendStr)
        {
            cNetResult netResult = new cNetResult();
            string[] tempStr = McgsSendStr.Split('~');//0:"D",1:"OK",2:TestTime,3:ThisNo,4:StepId.........
            netResult.RunResult.mTestTime = DateTime.Parse(tempStr[2]);//
            netResult.RunResult.mTestNo = Num.IntParse(tempStr[3]);//
            netResult.RunResult.mStepId = Num.IntParse(tempStr[4]);
            netResult.RunResult.mStep = tempStr[5];
            netResult.RunResult.mMode = tempStr[6];//
            netResult.RunResult.mJiQi = Num.IntParse(tempStr[7]);//
            netResult.RunResult.mIsPass = bool.Parse(tempStr[8]);//
            netResult.RunResult.mId = tempStr[9];//
            netResult.RunResult.mBar = tempStr[10];//
            for (int i = 0; i < cMain.DataShow; i++)
            {
                netResult.StepResult.mData[i] = Num.DoubleParse(tempStr[11 + i * 2]);//
                netResult.StepResult.mIsDataPass[i] = Num.IntParse(tempStr[12 + i * 2]);//
            }
            netResult.StepResult.mIsStepPass = Num.IntParse(tempStr[11 + 2 * cMain.DataShow]);
            return netResult;
        }

    }
}
