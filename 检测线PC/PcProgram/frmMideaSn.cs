using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
namespace PcProgram
{
    public partial class frmMideaSn : Form
    {
        public static cSnSet mSnSet = new cSnSet();
        public enum MachineList
        {

        }
        int baud = 600;
        /// <summary>
        /// 波特率
        /// </summary>
        public int Baud
        {
            get { return baud; }
            set { baud = value; }
        }
        int check = 2;
        /// <summary>
        /// 校验
        /// </summary>
        public int Check
        {
            get { return check; }
            set { check = value; }
        }
        int data = 8;
        /// <summary>
        /// 数据位
        /// </summary>
        public int Data
        {
            get { return data; }
            set { data = value; }
        }
        int stop = 2;
        /// <summary>
        /// 停止位
        /// </summary>
        public int Stop
        {
            get { return stop; }
            set { stop = value; }
        }
        string sn = "";

        public string Sn
        {
            get { return sn; }
            set { sn = value; }
        }
        string Id = "";
        int Index = 0;
        int modeJiQi = 0;
        public frmMideaSn(string SnCode, string id, int index, int ModeJiQi)
        {
            sn = SnCode;
            Id = id;
            Index = index;
            modeJiQi = ModeJiQi;
            InitializeComponent();
        }
        private void frmSn_Load(object sender, EventArgs e)
        {
            this.Left = (Screen.PrimaryScreen.Bounds.Width - this.Width) / 2;
            this.Top = (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2;

            //if (cMain.mSysSet.mSnAuto == 1)
            //{
            //chkIsAuto.Checked = false;
            //}
            //else
            //{
            //    chkIsAuto.Checked = false;
            //}
            cbbMachine.Items.Clear();
            string[] tmpStr = cMain.JiQiStr.Split(',');
            for (int i = 0; i < tmpStr.Length; i++)
            {
                cbbMachine.Items.Add(tmpStr[i]);
            }
            cbbMachine.SelectedIndex = modeJiQi;
            frmInit(modeJiQi);
            snInit(sn, modeJiQi);
            cbbMachine.Enabled = false;
            timer1.Enabled = chkIsAuto.Enabled;
        }
        private cSnCodeSet DataFrmToClass()
        {
            cSnCodeSet tmpSnCodeSet = new cSnCodeSet();
            try
            {
                tmpSnCodeSet.FengSu = -1;
                if (cMain.JiQiStr.Split(',')[cbbMachine.SelectedIndex] == ("变频SN机") || cMain.JiQiStr.Split(',')[cbbMachine.SelectedIndex].IndexOf("内销旧指令") >= 0)
                {
                    switch (cbbFengSu.Text)
                    {
                        case "室内风机关":
                            tmpSnCodeSet.FengSu = 0;
                            break;
                        case "高风":
                            tmpSnCodeSet.FengSu = 1;
                            break;
                        case "中风":
                            tmpSnCodeSet.FengSu = 2;
                            break;
                        case "低风":
                            tmpSnCodeSet.FengSu = 3;
                            break;
                    }
                }
                if (cMain.JiQiStr.Split(',')[cbbMachine.SelectedIndex].IndexOf("AA") >= 0 || cMain.JiQiStr.Split(',')[cbbMachine.SelectedIndex] == "假PQ")
                {
                    if (cbbFengSu.Text == "高风(双风机)")
                    {
                        tmpSnCodeSet.FengSu = 0;
                    }
                    if (cbbFengSu.Text == "低风(风机1)")
                    {
                        tmpSnCodeSet.FengSu = 1;
                    }
                    if (cbbFengSu.Text == "低风(风机2)")
                    {
                        tmpSnCodeSet.FengSu = 2;
                    }
                    if (cbbFengSu.Text == "风机关")
                    {
                        tmpSnCodeSet.FengSu = 3;
                    }
                }
                tmpSnCodeSet.IsDanLen = chkDanLen.Checked;
                tmpSnCodeSet.IsKuaiJian = chkKuaiJian.Checked;
                tmpSnCodeSet.MoShi = -1;
                if (cbbMoshi.Text == "停机")
                {
                    tmpSnCodeSet.MoShi = 0;
                }
                if (cbbMoshi.Text == "制冷")
                {
                    tmpSnCodeSet.MoShi = 1;
                }
                if (cbbMoshi.Text == "制热")
                {
                    tmpSnCodeSet.MoShi = 2;
                }
                if (cbbMoshi.Text == "送风")
                {
                    tmpSnCodeSet.MoShi = 3;
                }
                tmpSnCodeSet.NeiJiNeng = -1;
                if ( cMain.JiQiStr.Split(',')[cbbMachine.SelectedIndex].IndexOf("新变频") >= 0)
                {
                    if (cbbNengJi.Text == "0.8匹(22机)")
                    {
                        tmpSnCodeSet.NeiJiNeng = 0;
                    }
                    if (cbbNengJi.Text == "1匹(25,26,28机)")
                    {
                        tmpSnCodeSet.NeiJiNeng = 1;
                    }
                    if (cbbNengJi.Text == "1.2匹(32机)")
                    {
                        tmpSnCodeSet.NeiJiNeng = 2;
                    }
                    if (cbbNengJi.Text == "1.5匹(35机)")
                    {
                        tmpSnCodeSet.NeiJiNeng = 3;
                    }
                }
                tmpSnCodeSet.NengLi = Num.IntParse(txtNengLi.Text);
                tmpSnCodeSet.PinLv = Num.IntParse(txtPinLv.Text);
            }
            catch (Exception exc)
            {
                cMain.WriteErrorToLog("frmMideaSn:DataFrmToClass:" + exc.Message);
            }
            return tmpSnCodeSet;

        }
        private void snInit(string sn, int indexJiQi)
        {
            sn = sn.Trim();
            if ((sn.Length <= 0) || ((sn.Length % 2) == 1))
            {
                return;
            }
            byte[] buff = new byte[sn.Length / 2];
            for (int i = 0; i < buff.Length; i++)
            {
                buff[i] = Convert.ToByte(sn.Substring(i * 2, 2), 16);
            }
            if (cMain.JiQiStr.Split(',')[indexJiQi].IndexOf("定频") >= 0)
            {
                return;
            }
            if ( cMain.JiQiStr.Split(',')[indexJiQi].IndexOf("新变频") >= 0)
            {
                if (buff.Length < 20)
                {
                    return;
                }
                switch (buff[6])
                {
                    case 0:
                        cbbMoshi.Text = "停机";
                        break;
                    case 1:
                        cbbMoshi.Text = "制冷";
                        break;
                    case 2:
                        cbbMoshi.Text = "制热";
                        break;
                    case 3:
                        cbbMoshi.Text = "送风";
                        break;
                }
                txtPinLv.Text = buff[7].ToString();
                switch (buff[8])
                {
                    case 0:
                    case 1:
                        cbbFengSu.Text = "室内风机关";
                        break;
                    case 2:
                    case 3:
                        cbbFengSu.Text = "高风";
                        break;
                    case 4:
                    case 5:
                        cbbFengSu.Text = "中风";
                        break;
                    case 8:
                    case 9:
                        cbbFengSu.Text = "低风";
                        break;
                }
                switch (buff[15])
                {
                    case 0x08:
                        cbbNengJi.Text = "0.8匹(22机)";
                        break;
                    case 0x0A:
                        cbbNengJi.Text = "1匹(25,26,28机)";
                        break;
                    case 0x0C:
                        cbbNengJi.Text = "1.2匹(32机)";
                        break;
                    case 0x0F:
                        cbbNengJi.Text = "1.5匹(35机)";
                        break;
                }
                chkKuaiJian.Checked = (buff[16] == 0x01);
            }
            if (cMain.JiQiStr.Split(',')[indexJiQi] == ("变频SN机") || cMain.JiQiStr.Split(',')[indexJiQi].IndexOf("内销") >= 0)
            {
                if (buff.Length < 18)
                {
                    return;
                }
                switch (buff[5])
                {
                    case 0:
                        cbbMoshi.Text = "停机";
                        break;
                    case 1:
                        cbbMoshi.Text = "制冷";
                        break;
                    case 2:
                        cbbMoshi.Text = "制热";
                        break;
                    case 3:
                        cbbMoshi.Text = "送风";
                        break;
                }
                txtPinLv.Text = buff[6].ToString();

                switch (buff[7])
                {
                    case 0:
                    case 1:
                        cbbFengSu.Text = "室内风机关";
                        break;
                    case 2:
                    case 3:
                        cbbFengSu.Text = "高风";
                        break;
                    case 4:
                    case 5:
                        cbbFengSu.Text = "中风";
                        break;
                    case 8:
                    case 9:
                        cbbFengSu.Text = "低风";
                        break;
                }
                chkKuaiJian.Checked = (buff[15] == 0x01);
                return;
            }
            if (cMain.JiQiStr.Split(',')[indexJiQi].IndexOf("55") >= 0)
            {
                switch (buff[1])
                {
                    case 0x00:
                        cbbMoshi.Text = "停机";
                        break;
                    case 0x02:
                        cbbMoshi.Text = "制冷";
                        break;
                    case 0x0C:
                        cbbMoshi.Text = "制热";
                        break;
                    case 0xE0:
                        cbbMoshi.Text = "送风";
                        break;
                }
                if ((buff[3] | 0x40) == 0x40)
                {
                    cbbFengSu.Text = "高风";
                }
                else
                {
                    if ((buff[3] | 0x20) == 0x20)
                    {
                        cbbFengSu.Text = "低风";
                    }
                    else
                    {
                        cbbNengJi.Text = "室内风机关";
                    }
                }
                chkKuaiJian.Checked = (buff[4] == 0x10);
                return;
            }
            if (cMain.JiQiStr.Split(',')[indexJiQi].IndexOf("AA") >= 0 || cMain.JiQiStr.Split(',')[indexJiQi]=="假PQ")
            {
                if ((buff[1] | 0x06) == 0x06)
                {
                    cbbFengSu.Text = "高风(双风机)";
                }
                if ((buff[1] | 0x04) == 0x04)
                {
                    cbbFengSu.Text = "低风(风机2)";
                }
                if ((buff[1] | 0x02) == 0x02)
                {
                    cbbFengSu.Text = "低风(风机1)";
                }
                if ((buff[1] | 0x00) == 0x00)
                {
                    cbbFengSu.Text = "低风(风机1)";
                }
                switch (buff[2])
                {
                    case 0x00:
                        cbbMoshi.Text = "停机";
                        break;
                    case 0x02:
                        cbbMoshi.Text = "制冷";
                        break;
                    case 0x0C:
                        cbbMoshi.Text = "制热";
                        break;
                    case 0xE0:
                        cbbMoshi.Text = "送风";
                        break;
                }
                return;
            }
            if (cMain.JiQiStr.Split(',')[indexJiQi].IndexOf("PQ") >= 0)
            {
                switch (buff[2])
                {
                    case 0:
                        cbbMoshi.Text = "停机";
                        break;
                    case 1:
                        cbbMoshi.Text = "送风";
                        break;
                    case 2:
                        cbbMoshi.Text = "制冷";
                        break;
                    case 3:
                        cbbMoshi.Text = "制热";
                        break;
                }
                txtNengLi.Text = buff[3].ToString();
            }
        }
        private string DataFrmToSn()
        {
            string SnCode = "";
            //string MoShi = "00";
            //string PinLv = "00";
            //string DuanKou = "00";
            //string WenDu = "00";
            //string FengSu = "00";
            //string KuaiJian = "00";
            byte[] SnCodeByte = new byte[20];
            int machineSelectedIndex = 1;
            string[] tempJiQiStr = cMain.JiQiStr.Split(',');
            for (int i = 0; i < tempJiQiStr.Length; i++)
            {
                if (tempJiQiStr[i] == cbbMachine.Text)
                {
                    machineSelectedIndex = i;
                    break;
                }
            }
            if (cbbMachine.Text.IndexOf("定频机") >= 0)
            {
                SnCode = "";
                return SnCode;
            }
            if (cbbMachine.Text.IndexOf("内销") >= 0 || cbbMachine.Text == "变频SN机")
            {
                SnCodeByte[0] = 0xAA;
                SnCodeByte[1] = 0x01;
                SnCodeByte[2] = 0x00;
                SnCodeByte[3] = 0x00;
                SnCodeByte[4] = 0x05;
                SnCodeByte[5] = 0x00;
                switch (cbbMoshi.Text)
                {
                    case "停机":
                        SnCodeByte[5] = 0x00;
                        break;
                    case "制冷":
                        SnCodeByte[5] = 0x01;
                        break;
                    case "制热":
                        SnCodeByte[5] = 0x02;
                        break;
                    case "送风":
                        SnCodeByte[5] = 0x03;
                        break;
                }
                SnCodeByte[6] = (byte)(Num.IntParse(txtPinLv.Text) & 0xFF);
                SnCodeByte[7] = 0x02;
                switch (cbbFengSu.Text)
                {
                    case "室内风机关":
                        SnCodeByte[7] = 0x00;
                        break;
                    case "高风":
                        SnCodeByte[7] = 0x02;
                        break;
                    case "中风":
                        SnCodeByte[7] = 0x04;
                        break;
                    case "低风":
                        SnCodeByte[7] = 0x08;
                        break;
                }
                if ((cbbMoshi.Text == "制热" && (machineSelectedIndex == 1 || machineSelectedIndex == 7))//制热
                    || (cbbMoshi.Text == "制冷" && machineSelectedIndex == 3))//出口机制冷
                {
                    SnCodeByte[7] = (byte)((SnCodeByte[7] + 1) & 0xFF);
                }
                SnCodeByte[8] = 0x00;
                SnCodeByte[9] = 0x00;
                SnCodeByte[10] = 0x19;
                if (cbbMoshi.Text == "停机" || cbbMoshi.Text == "送风")
                {
                    SnCodeByte[10] = 0x19;
                }
                if (cbbMoshi.Text == "制冷")//制冷
                {
                    SnCodeByte[10] = 0x11;
                }
                if (cbbMoshi.Text == "制热")//制热
                {
                    SnCodeByte[10] = 0x1E;
                }
                SnCodeByte[11] = 0x01;
                SnCodeByte[12] = 0x7D;
                SnCodeByte[13] = 0x7D;
                SnCodeByte[14] = 0x0F;
                SnCodeByte[15] = chkKuaiJian.Checked ? (byte)0x01 : (byte)0x00;
                cMideaSnCode.Crc(SnCodeByte, cMideaSnCode.CrcList.OldSn600, ref SnCodeByte[16], ref SnCodeByte[17]);
                SnCodeByte[17] = 0x55;
                for (int i = 0; i < 18; i++)
                {
                    SnCode = SnCode + string.Format("{0:X2}", SnCodeByte[i]);
                }
                return SnCode;
            }
            if (cbbMachine.Text.IndexOf("55") >= 0)
            {
                SnCodeByte[0] = 0x55;
                //00停  0C制热  02制冷  E0送风
                SnCodeByte[1] = 0x00;
                switch (cbbMoshi.Text)
                {
                    case "停机":
                        SnCodeByte[1] = 0x00;
                        break;
                    case "制冷":
                        SnCodeByte[1] = 0x02;
                        break;
                    case "制热":
                        SnCodeByte[1] = 0x0C;
                        break;
                    case "送风":
                        SnCodeByte[1] = 0xE0;
                        break;
                }
                SnCodeByte[2] = 0xFF;
                SnCodeByte[3] = 0x40;
                switch (cbbFengSu.Text)
                {
                    case "室内风机关":
                        switch (cbbMoshi.Text)
                        {
                            case "停机":
                            case "送风":
                                SnCodeByte[3] = 0x00;
                                break;
                            case "制冷":
                                SnCodeByte[3] = 0x80;
                                break;
                            case "制热":
                                SnCodeByte[3] = 0x90;
                                break;
                        }
                        break;
                    case "高风":
                        switch (cbbMoshi.Text)
                        {
                            case "停机":
                            case "送风":
                                SnCodeByte[3] = 0x40;
                                break;
                            case "制冷":
                                SnCodeByte[3] = 0xC0;
                                break;
                            case "制热":
                                SnCodeByte[3] = 0xD0;
                                break;
                        }
                        break;
                    case "低风":
                        switch (cbbMoshi.Text)
                        {
                            case "停机":
                            case "送风":
                                SnCodeByte[3] = 0x20;
                                break;
                            case "制冷":
                                SnCodeByte[3] = 0xA0;
                                break;
                            case "制热":
                                SnCodeByte[3] = 0xB0;
                                break;
                        }
                        break;
                }
                SnCodeByte[4] = chkKuaiJian.Checked ? (byte)0x10 : (byte)0x00;
                cMideaSnCode.Crc(SnCodeByte, cMideaSnCode.CrcList.ShuMa1023, ref SnCodeByte[5], ref SnCodeByte[6]);
                for (int i = 0; i < 6; i++)
                {
                    SnCode = SnCode + string.Format("{0:X2}", SnCodeByte[i]);
                }
                return SnCode;
            }
            if (cbbMachine.Text.IndexOf("AA") >= 0 || cbbMachine.Text=="假PQ")
            {
                SnCodeByte[0] = 0xAA;
                SnCodeByte[1] = 0x00;
                SnCodeByte[1] = 0x0E;
                switch (cbbFengSu.Text)
                {
                    case "高风(双风机)":
                        switch (cbbMoshi.Text)
                        {
                            case "制冷":
                                SnCodeByte[1] = 0x0E;
                                break;
                            case "制热":
                                SnCodeByte[1] = 0x0F;
                                break;
                            case "送风":
                                SnCodeByte[1] = 0x06;
                                break;
                        }
                        break;
                    case "低风(风机2)":
                        switch (cbbMoshi.Text)
                        {
                            case "制冷":
                                SnCodeByte[1] = 0x0C;
                                break;
                            case "制热":
                                SnCodeByte[1] = 0x0D;
                                break;
                            case "送风":
                                SnCodeByte[1] = 0x04;
                                break;
                        }
                        break;
                    case "低风(风机1)":
                        switch (cbbMoshi.Text)
                        {
                            case "制冷":
                                SnCodeByte[1] = 0x0A;
                                break;
                            case "制热":
                                SnCodeByte[1] = 0x0B;
                                break;
                            case "送风":
                                SnCodeByte[1] = 0x02;
                                break;
                        }
                        break;
                    case "风机关":
                        switch (cbbMoshi.Text)
                        {
                            case "制冷":
                                SnCodeByte[1] = 0x08;
                                break;
                            case "制热":
                                SnCodeByte[1] = 0x09;
                                break;
                        }
                        break;
                }
                SnCodeByte[2] = 0x00;
                switch (cbbMoshi.Text)
                {
                    case "停机":
                        SnCodeByte[2] = 0x00;
                        break;
                    case "制冷":
                        SnCodeByte[2] = 0x01;
                        break;
                    case "制热":
                        SnCodeByte[2] = 0x02;
                        break;
                    case "送风":
                        SnCodeByte[2] = 0x03;
                        break;
                }
                SnCodeByte[3] = 0x00;
                SnCodeByte[4] = chkDanLen.Checked ? (byte)0x00 : (byte)0x01;
                SnCodeByte[5] = 0x00;
                cMideaSnCode.Crc(SnCodeByte, cMideaSnCode.CrcList.TianHua1200, ref SnCodeByte[6], ref SnCodeByte[7]);
                SnCodeByte[7] = 0x55;
                //SnCode = Encoding.ASCII.GetString(SnCodeByte, 0, 8);
                for (int i = 0; i < 8; i++)
                {
                    SnCode = SnCode + string.Format("{0:X2}", SnCodeByte[i]);
                }
                return SnCode;
            }
            if (cbbMachine.Text.IndexOf("PQ") >= 0)
            {
                SnCodeByte[0] = 0xAA;
                SnCodeByte[1] = 0x01;
                switch (cbbMoshi.Text)
                {
                    case "停机":
                        SnCodeByte[2] = 0x00;
                        SnCodeByte[3] = 0x00;
                        SnCodeByte[4] = 0x19;
                        SnCodeByte[5] = 0x00;
                        break;
                    case "制冷":
                        SnCodeByte[2] = 0x02;
                        SnCodeByte[3] = (byte)(Num.IntParse(txtNengLi.Text) & 0xFF);
                        SnCodeByte[4] = 0x11;
                        SnCodeByte[5] = 0x01;
                        break;
                    case "制热":
                        SnCodeByte[2] = 0x03;
                        SnCodeByte[3] = (byte)(Num.IntParse(txtNengLi.Text) & 0xFF);
                        SnCodeByte[4] = 0x1E;
                        SnCodeByte[5] = 0x01;
                        break;
                    case "送风":
                        SnCodeByte[2] = 0x01;
                        SnCodeByte[3] = 0x00;
                        SnCodeByte[4] = 0x19;
                        SnCodeByte[5] = 0x00;
                        break;
                }
                SnCodeByte[6] = 0xFA;
                SnCodeByte[7] = 0x00;
                cMideaSnCode.Crc(SnCodeByte, cMideaSnCode.CrcList.PQ, ref SnCodeByte[8], ref SnCodeByte[9]);
                SnCodeByte[9] = 0x55;
                for (int i = 0; i < 10; i++)
                {
                    SnCode = SnCode + string.Format("{0:X2}", SnCodeByte[i]);
                }
                return SnCode;
            }
            if (cbbMachine.Text.IndexOf("新变频") >= 0)
            {

                SnCodeByte[0] = 0xA0;
                SnCodeByte[1] = 0x01;
                SnCodeByte[2] = 0x00;
                SnCodeByte[3] = 0x20;
                SnCodeByte[4] = 0x0C;
                SnCodeByte[5] = 0x11;
                SnCodeByte[6] = 0x00;
                switch (cbbMoshi.Text)
                {
                    case "停机":
                        SnCodeByte[6] = 0x00;
                        break;
                    case "制冷":
                        SnCodeByte[6] = 0x01;
                        break;
                    case "制热":
                        SnCodeByte[6] = 0x02;
                        break;
                    case "送风":
                        SnCodeByte[6] = 0x03;
                        break;
                }
                SnCodeByte[7] = (byte)(Num.IntParse(txtPinLv.Text) & 0xFF);
                SnCodeByte[8] = 0x02;
                switch (cbbFengSu.Text)
                {
                    case "室内风机关":
                        SnCodeByte[8] = 0x00;
                        break;
                    case "高风":
                        SnCodeByte[8] = 0x02;
                        break;
                    case "中风":
                        SnCodeByte[8] = 0x04;
                        break;
                    case "低风":
                        SnCodeByte[8] = 0x08;
                        break;
                }
                if (cbbMoshi.Text == "制热")
                {
                    SnCodeByte[8] = (byte)((SnCodeByte[8] + 1) & 0xFF);
                }
                SnCodeByte[9] = 0x00;
                SnCodeByte[10] = 0x00;
                SnCodeByte[11] = 0x19;
                if (cbbMoshi.Text == "停机" || cbbMoshi.Text == "送风")
                {
                    SnCodeByte[11] = 0x19;
                }
                if (cbbMoshi.Text == "制冷")//制冷
                {
                    SnCodeByte[11] = 0x11;
                }
                if (cbbMoshi.Text == "制热")//制热
                {
                    SnCodeByte[11] = 0x1E;
                }
                SnCodeByte[12] = 0x01;
                SnCodeByte[13] = 0x75;
                SnCodeByte[14] = 0x75;
                SnCodeByte[15] = 0x08;
                switch (cbbNengJi.Text)
                {
                    case "0.8匹(22机)":
                        SnCodeByte[15] = 0x08;
                        break;
                    case "1匹(25,26,28机)":
                        SnCodeByte[15] = 0x0A;
                        break;
                    case "1.2匹(32机)":
                        SnCodeByte[15] = 0x0C;
                        break;
                    case "1.5匹(35机)":
                        SnCodeByte[15] = 0x0F;
                        break;
                }
                SnCodeByte[16] = chkKuaiJian.Checked ? (byte)0x01 : (byte)0x00;
                SnCodeByte[17] = 0x00;
                cMideaSnCode.Crc(SnCodeByte, cMideaSnCode.CrcList.NewSn600, ref SnCodeByte[18], ref SnCodeByte[19]);
                for (int i = 0; i < 20; i++)
                {
                    SnCode = SnCode + string.Format("{0:X2}", SnCodeByte[i]);
                }
                return SnCode;
            }
            return SnCode;
        }
        //private void DataFileToFrm(string id, int index)
        //{
        //    try
        //    {
        //        cSnSet tmpSnSet = new cSnSet();
        //        tmpSnSet = (cSnSet)cXml.readXml(string.Format("{0}{1}.xml", SnIdDirectory, id), typeof(cSnSet), tmpSnSet);
        //        mSnSet.SnCodeSet[index] = tmpSnSet.SnCodeSet[index];
        //        tmpSnSet = null;
        //        if (cbbMachine.Text.IndexOf("内销") >= 0 || cbbMachine.Text == "变频SN机")
        //        {
        //            cbbMoshi.SelectedIndex = mSnSet.SnCodeSet[index].MoShi;
        //            cbbFengSu.SelectedIndex = mSnSet.SnCodeSet[index].FengSu;
        //            cbbNengJi.SelectedIndex = mSnSet.SnCodeSet[index].NeiJiNeng;
        //            txtPinLv.Text = mSnSet.SnCodeSet[index].PinLv.ToString();
        //            chkKuaiJian.Checked = mSnSet.SnCodeSet[index].IsKuaiJian;
        //            chkDanLen.Checked = false;
        //            txtNengLi.Text = "";
        //            return;
        //        }
        //        if (cbbMachine.Text.IndexOf("55") >= 0)
        //        {
        //            cbbMoshi.SelectedIndex = mSnSet.SnCodeSet[index].MoShi;
        //            cbbFengSu.SelectedIndex = mSnSet.SnCodeSet[index].FengSu;
        //            chkKuaiJian.Checked = mSnSet.SnCodeSet[index].IsKuaiJian;
        //            txtPinLv.Text = "";
        //            chkDanLen.Checked = false;
        //            txtNengLi.Text = "";
        //            return;
        //        }
        //        if (cbbMachine.Text.IndexOf("AA") >= 0 || cbbMachine.Text=="假PQ")
        //        {
        //            cbbMoshi.SelectedIndex = mSnSet.SnCodeSet[index].MoShi;
        //            cbbFengSu.SelectedIndex = mSnSet.SnCodeSet[index].FengSu;
        //            chkDanLen.Checked = mSnSet.SnCodeSet[index].IsDanLen;
        //            chkKuaiJian.Checked = false;
        //            txtPinLv.Text = "";
        //            txtNengLi.Text = "";
        //            return;
        //        }
        //        if (cbbMachine.Text.IndexOf("PQ") >= 0)
        //        {
        //            cbbMoshi.SelectedIndex = mSnSet.SnCodeSet[index].MoShi;
        //            cbbFengSu.SelectedIndex = mSnSet.SnCodeSet[index].FengSu;
        //            chkDanLen.Checked = false;
        //            chkKuaiJian.Checked = false;
        //            txtPinLv.Text = "";
        //            txtNengLi.Text = mSnSet.SnCodeSet[index].NengLi.ToString();
        //            return;
        //        }
        //    }
        //    catch (Exception exc)
        //    {
        //        cMain.WriteErrorToLog("FrmMideaSn:DataFileToFrm:" + exc.ToString());
        //    }
        //}
        private void frmInit(int machineIndex)
        {
            //    cbbMoshi.Items.Clear();
            //    cbbMoshi.Items.Add("停机");
            //    cbbMoshi.Items.Add("制冷");
            //    cbbMoshi.Items.Add("制热");
            //    cbbMoshi.Items.Add("送风");

            //    switch (machineIndex)
            //    {
            //        case 0:
            //            break;
            //        case 1:
            //        case 3:
            //            cbbFengSu.Items.Clear();
            //            cbbFengSu.Items.Add("室内风机关");
            //            cbbFengSu.Items.Add("高风");
            //            cbbFengSu.Items.Add("中风");
            //            cbbFengSu.Items.Add("低风");
            //            break;
            //        case 2:
            //            cbbFengSu.Items.Clear();
            //            cbbFengSu.Items.Add("室内风机关");
            //            cbbFengSu.Items.Add("高风");
            //            cbbFengSu.Items.Add("中风");
            //            cbbFengSu.Items.Add("低风");
            //            cbbNengJi.Items.Clear();
            //            cbbNengJi.Items.Add("0.8匹(22机)");
            //            cbbNengJi.Items.Add("1匹(25,26,28机)");
            //            cbbNengJi.Items.Add("1.2匹(32机)");
            //            cbbNengJi.Items.Add("1.5匹(35机)");
            //            break;
            //        case 4:
            //        case 7:
            //            cbbFengSu.Items.Clear();
            //            cbbFengSu.Items.Add("室内风机关");
            //            cbbFengSu.Items.Add("高风");
            //            cbbFengSu.Items.Add("低风");
            //            break;
            //        case 5:
            //            cbbFengSu.Items.Clear();
            //            cbbFengSu.Items.Add("高风(双风机)");
            //            cbbFengSu.Items.Add("低风(风机1)");
            //            cbbFengSu.Items.Add("低风(风机2)");
            //            cbbFengSu.Items.Add("风机关");
            //            break;
            //        case 6:
            //            break;
            //    }
            cbbMoshi.Items.Clear();
            cbbMoshi.Items.Add("停机");
            cbbMoshi.Items.Add("制冷");
            cbbMoshi.Items.Add("制热");
            cbbMoshi.Items.Add("送风");

            if (cbbMachine.Text.IndexOf("内销") >= 0 || cbbMachine.Text == "变频SN机")
            {
                cbbFengSu.Items.Clear();
                cbbFengSu.Items.Add("室内风机关");
                cbbFengSu.Items.Add("高风");
                cbbFengSu.Items.Add("中风");
                cbbFengSu.Items.Add("低风");
            }
            if (cbbMachine.Text.IndexOf("55") >= 0)
            {
                cbbFengSu.Items.Clear();
                cbbFengSu.Items.Add("室内风机关");
                cbbFengSu.Items.Add("高风");
                cbbFengSu.Items.Add("低风");
            }
            if (cbbMachine.Text.IndexOf("AA") >= 0 || cbbMachine.Text == "假PQ")
            {
                cbbFengSu.Items.Clear();
                cbbFengSu.Items.Add("高风(双风机)");
                cbbFengSu.Items.Add("低风(风机1)");
                cbbFengSu.Items.Add("低风(风机2)");
                cbbFengSu.Items.Add("风机关");
            }
            if (cbbMachine.Text.IndexOf("新变频") >= 0)
            {
                cbbFengSu.Items.Clear();
                cbbFengSu.Items.Add("室内风机关");
                cbbFengSu.Items.Add("高风");
                cbbFengSu.Items.Add("中风");
                cbbFengSu.Items.Add("低风");
                cbbNengJi.Items.Clear();
                cbbNengJi.Items.Add("0.8匹(22机)");
                cbbNengJi.Items.Add("1匹(25,26,28机)");
                cbbNengJi.Items.Add("1.2匹(32机)");
                cbbNengJi.Items.Add("1.5匹(35机)");
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            mSnSet.id = Id;
            timer1.Enabled = false;
            sn = txtSn.Text.Trim();
            //mSnSet.SnCodeSet[Index] = DataFrmToClass();
            //mSnSet.MachineIndex = cbbMachine.SelectedIndex;
            this.DialogResult = DialogResult.Yes;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            mSnSet.id = Id;
            timer1.Enabled = false;
            this.DialogResult = DialogResult.No;
        }

        private void cbbMachine_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbMachine.Text.IndexOf("新变频") >= 0)
            {
                chkKuaiJian.Enabled = true;
                chkDanLen.Enabled = false;
                txtNengLi.BackColor = Color.LightGray;
                txtNengLi.Enabled = false;
                txtPinLv.BackColor = Color.White;
                txtPinLv.Enabled = true;
                cbbMoshi.BackColor = Color.White;
                cbbMoshi.Enabled = true;
                cbbNengJi.BackColor = Color.White;
                cbbNengJi.Enabled = true;
                cbbFengSu.BackColor = Color.White;
                cbbFengSu.Enabled = true;
            }
            if (cbbMachine.Text.IndexOf("内销") >= 0 || cbbMachine.Text == "变频SN机")
            {
                cbbNengJi.BackColor = Color.LightGray;
                cbbNengJi.Enabled = false;
                chkKuaiJian.Enabled = true;
                chkDanLen.Enabled = false;
                txtNengLi.BackColor = Color.LightGray;
                txtNengLi.Enabled = false;
                txtPinLv.BackColor = Color.White;
                txtPinLv.Enabled = true;
                cbbMoshi.BackColor = Color.White;
                cbbMoshi.Enabled = true;
                cbbFengSu.BackColor = Color.White;
                cbbFengSu.Enabled = true;
                frmInit(cbbMachine.SelectedIndex);
                return;
            }
            if (cbbMachine.Text.IndexOf("55") >= 0)
            {
                chkKuaiJian.Enabled = true;
                chkDanLen.Enabled = false;
                txtNengLi.BackColor = Color.LightGray;
                txtNengLi.Enabled = false;
                txtPinLv.BackColor = Color.LightGray;
                txtPinLv.Enabled = false;
                cbbMoshi.BackColor = Color.White;
                cbbMoshi.Enabled = true;
                cbbNengJi.BackColor = Color.LightGray;
                cbbNengJi.Enabled = false;
                cbbFengSu.BackColor = Color.White;
                cbbFengSu.Enabled = true;
                frmInit(cbbMachine.SelectedIndex);
                return;
            }
            if (cbbMachine.Text.IndexOf("AA") >= 0 || cbbMachine.Text == "假PQ")
            {
                chkKuaiJian.Enabled = false;
                chkDanLen.Enabled = true;
                txtNengLi.BackColor = Color.LightGray;
                txtNengLi.Enabled = false;
                txtPinLv.BackColor = Color.LightGray;
                txtPinLv.Enabled = false;
                cbbMoshi.BackColor = Color.White;
                cbbMoshi.Enabled = true;
                cbbNengJi.BackColor = Color.LightGray;
                cbbNengJi.Enabled = false;
                cbbFengSu.BackColor = Color.White;
                cbbFengSu.Enabled = true;
                return;
            }
            if (cbbMachine.Text.IndexOf("PQ") >= 0)
            {
                chkKuaiJian.Enabled = false;
                chkDanLen.Enabled = false;
                txtNengLi.BackColor = Color.White;
                txtNengLi.Enabled = true;
                txtPinLv.BackColor = Color.LightGray;
                txtPinLv.Enabled = false;
                cbbMoshi.BackColor = Color.White;
                cbbMoshi.Enabled = true;
                cbbNengJi.BackColor = Color.LightGray;
                cbbNengJi.Enabled = false;
                cbbFengSu.BackColor = Color.LightGray;
                cbbFengSu.Enabled = false;
                frmInit(cbbMachine.SelectedIndex);
                return;
            }
            if (cbbMachine.Text.IndexOf("定频机") >= 0)
            {
                chkKuaiJian.Enabled = false;
                chkDanLen.Enabled = false;
                txtNengLi.BackColor = Color.LightGray;
                txtNengLi.Enabled = false;
                txtPinLv.BackColor = Color.LightGray;
                txtPinLv.Enabled = false;
                cbbMoshi.BackColor = Color.LightGray;
                cbbMoshi.Enabled = false;
                cbbNengJi.BackColor = Color.LightGray;
                cbbNengJi.Enabled = false;
                cbbFengSu.BackColor = Color.LightGray;
                cbbFengSu.Enabled = false;
                txtSn.Text = "";
                frmInit(cbbMachine.SelectedIndex);
                return;
            }
        }
        private bool LoadId(string id)
        {
            bool isOk = false;
            return isOk;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            txtSn.Text = DataFrmToSn();

        }
        private void chkIsAuto_CheckStateChanged(object sender, EventArgs e)
        {
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void chkIsAuto_CheckedChanged(object sender, EventArgs e)
        {
            //chkIsAuto.Enabled = !chkIsAuto.Enabled;
            timer1.Enabled = chkIsAuto.Checked;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            sn = "";
            this.DialogResult = DialogResult.Yes;
        }
        //private void txtPinLv_GotFocus(object sender, EventArgs e)
        //{
        //    btnOk.Focus();
        //    TextBox txt = (TextBox)sender;
        //    frmNumKeyBoard fm = new frmNumKeyBoard(Num.SingleParse(txt.Text));
        //    if (fm.ShowDialog() == DialogResult.Yes)
        //    {
        //        txt.Text = fm.SValue.ToString();
        //    }
        //}

        //private void txtSn_GotFocus(object sender, EventArgs e)
        //{
        //    btnOk.Focus();
        //    if (cMain.mSysSet.mBeiYong4 == 0)
        //    {
        //        TextBox txt = (TextBox)sender;
        //        frmHexNumberBoard fm = new frmHexNumberBoard(txt.Text);
        //        if (fm.ShowDialog() == DialogResult.Yes)
        //        {
        //            txt.Text = fm.ReturnValue;
        //        }
        //    }
        //}
    }
    /// <summary>
    /// SN指令类
    /// </summary>
    public class cSnCodeSet
    {
        private bool isKuaiJian = true;

        /// <summary>
        /// 是否是快检指令
        /// </summary>
        public bool IsKuaiJian
        {
            get { return isKuaiJian; }
            set { isKuaiJian = value; }
        }
        private bool isDanLen = false;

        /// <summary>
        /// 是否是单冷机
        /// </summary>
        public bool IsDanLen
        {
            get { return isDanLen; }
            set { isDanLen = value; }
        }
        private int nengLi = 0;

        /// <summary>
        /// PQ机的能级系数
        /// </summary>
        public int NengLi
        {
            get { return nengLi; }
            set { nengLi = value; }
        }
        private double pinLv = 0;

        /// <summary>
        /// 变频机运转的频率
        /// </summary>
        public double PinLv
        {
            get { return pinLv; }
            set { pinLv = value; }
        }
        private int moShi = 0;

        /// <summary>
        /// 内机运转的频率
        /// </summary>
        public int MoShi
        {
            get { return moShi; }
            set { moShi = value; }
        }
        private int neiJiNeng = 0;

        /// <summary>
        /// 内机的能级系数
        /// </summary>
        public int NeiJiNeng
        {
            get { return neiJiNeng; }
            set { neiJiNeng = value; }
        }
        private int fengSu = 0;

        /// <summary>
        /// 外机风机风速
        /// </summary>
        public int FengSu
        {
            get { return fengSu; }
            set { fengSu = value; }
        }
    }
    public class cSnSet
    {
        /// <summary>
        /// 相关联的ID号
        /// </summary>
        public string id = "Def";
        /// <summary>
        /// 机器序号
        /// </summary>
        public int MachineIndex = 0;
        /// <summary>
        /// 10个步骤的指令设置
        /// </summary>
        public cSnCodeSet[] SnCodeSet = new cSnCodeSet[cModeSet.StepCount];
        public cSnSet()
        {
            for (int i = 0; i < SnCodeSet.Length; i++)
            {
                SnCodeSet[i] = new cSnCodeSet();
            }
        }
        //public static string DataClassToStr(cSnSet snSet)
        //{
        //    string ss = "";
        //    ss = ss + snSet.id + "~";
        //    ss = ss + snSet.MachineIndex.ToString() + "~";
        //    for (int i = 0; i < snSet.SnCodeSet.Length; i++)
        //    {
        //        ss = ss + snSet.SnCodeSet[i].FengSu.ToString() + "~";
        //        ss = ss + snSet.SnCodeSet[i].IsDanLen.ToString() + "~";
        //        ss = ss + snSet.SnCodeSet[i].IsKuaiJian.ToString() + "~";
        //        ss = ss + snSet.SnCodeSet[i].MoShi.ToString() + "~";
        //        ss = ss + snSet.SnCodeSet[i].NeiJiNeng.ToString() + "~";
        //        ss = ss + snSet.SnCodeSet[i].NengLi.ToString() + "~";
        //        ss = ss + snSet.SnCodeSet[i].PinLv.ToString() + "~";
        //    }
        //    return ss;
        //}
        //public static cSnSet DataXmlToClass(string id)
        //{
        //    cSnSet mSnSet = new cSnSet();
        //    mSnSet = (cSnSet)cXml.readXml(string.Format("{0}{1}.xml", frmMideaSn.SnIdDirectory, id), typeof(cSnSet), mSnSet);
        //    return mSnSet;
        //}
        //public static void DataClassToXml(cSnSet mSnSet)
        //{
        //    cXml.saveXml(string.Format("{0}{1}.xml", frmMideaSn.SnIdDirectory, mSnSet.id), typeof(cSnSet), mSnSet);
        //}
        //public static cSnSet DataStrToClass(string data)
        //{
        //    if (data.IndexOf("C~") == 0)
        //    {
        //        data = data.Substring(2);
        //    }
        //    cSnSet mSnSet = new cSnSet();
        //    try
        //    {
        //        string[] tempStr = data.Split('~');
        //        int index = 0;
        //        mSnSet.id = tempStr[index++];
        //        mSnSet.MachineIndex = Num.IntParse(tempStr[index++]);
        //        for (int i = 0; i < mSnSet.SnCodeSet.Length; i++)
        //        {
        //            mSnSet.SnCodeSet[i].FengSu = Num.IntParse(tempStr[index++]);
        //            mSnSet.SnCodeSet[i].IsDanLen = Num.BoolParse(tempStr[index++]);
        //            mSnSet.SnCodeSet[i].IsKuaiJian = Num.BoolParse(tempStr[index++]);
        //            mSnSet.SnCodeSet[i].MoShi = Num.IntParse(tempStr[index++]);
        //            mSnSet.SnCodeSet[i].NeiJiNeng = Num.IntParse(tempStr[index++]);
        //            mSnSet.SnCodeSet[i].NengLi = Num.IntParse(tempStr[index++]);
        //            mSnSet.SnCodeSet[i].PinLv = Num.IntParse(tempStr[index++]);
        //        }
        //    }
        //    catch
        //    { }
        //    return mSnSet;
        //}
    }
}