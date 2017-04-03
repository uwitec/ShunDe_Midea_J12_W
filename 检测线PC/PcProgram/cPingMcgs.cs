using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Text.RegularExpressions;
using System.Net.NetworkInformation;
namespace PcProgram
{
    public delegate void mcgsPing(IPAddress hostIP, string hostStatue);
    class cPingMcgs
    {
        int badCount;
        int firstIp, endIp;
        static string badNo;
        public void mcgsPing(IPAddress hostIP, string hostStatue)
        {
            string strIpLast;
            strIpLast = hostIP.ToString().Substring(hostIP.ToString().LastIndexOf(".") + 1);
            if (hostStatue == "连接失败")
            {
                badCount = badCount + 1;
                badNo = badNo + strIpLast.ToString() + "连接失败;";
                cMain.isUdpInitError[Num.IntParse(strIpLast) - 1] = false;//UDP初始化失败
            }
            else
            {
                cMain.isUdpInitError[Num.IntParse(strIpLast) - 1] = true;//UDP初始化成功
            }
        }
        public static string localHostIp()
        {
            string returnStr = "127.0.0.1";
            IPHostEntry ih = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in ih.AddressList)
            {
                if (ip.ToString() != "127.0.0.1")
                {
                    returnStr = ip.ToString();
                    if (returnStr == "200.200.200.101")
                    {
                        break;
                    }
                }
            }
            return returnStr;
        }
        public void PingMcgs(string startIp,string stopIp)
        {
            int index;
            string SameIp;
            firstIp = 0;
            endIp = 0;
            badCount = 0;
            if (!FoundMatch(startIp) || !FoundMatch(stopIp))
            {
                System.Windows.Forms.MessageBox.Show("输入的 IP地址不正确,请重新输入", "错误", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }
            firstIp = int.Parse(startIp.Substring(startIp.LastIndexOf(".") + 1));
            endIp = int.Parse(stopIp.Substring(stopIp.LastIndexOf(".") + 1));
            SameIp = startIp.Substring(0, startIp.LastIndexOf(".")+1);
            if (firstIp > endIp)
            {
                int midIp;
                midIp = firstIp; firstIp = endIp; endIp = midIp;
            }
            Thread[] thPing = new Thread[endIp - firstIp + 1];
            for (index = firstIp; index <= endIp; index++)
            {
                cPing cp = new cPing(IPAddress.Parse(SameIp + index.ToString()));
                cp.mp = new mcgsPing(mcgsPing);
                thPing[index - firstIp] = new Thread(new ThreadStart(cp.mPing));
                thPing[index - firstIp].IsBackground = true;
                thPing[index - firstIp].Start();
            }
            foreach (Thread t in thPing)
            {
                t.Join();
            }
            if (badCount > 0)
            {
                System.Windows.Forms.MessageBox.Show(badNo + "共" + badCount.ToString() + "台", "错误", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }
        private bool FoundMatch(string txt)
          {
              try
              {
                  return Regex.IsMatch(txt, @"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$");  
              }
              catch
              {
                 return false;
             }
         }

    }
    class cPing
    {
        private IPAddress _hostIP;
        public mcgsPing mp;
        public IPAddress HostIP
        {
            get { return _hostIP; }
            set { _hostIP = value; }
        }
        public cPing(IPAddress hostIP)
        {
            _hostIP = hostIP;
        }
        public void mPing()
        {
            string hostName;
            try
            {
                Ping p = new Ping();
                PingReply pr = p.Send(_hostIP);
                if (pr.Status == IPStatus.Success)
                {
                    hostName = "连接成功";
                }
                else
                {
                    hostName = "连接失败";
                }

            }
            catch
            {
                hostName = "连接失败";
            }

            mp(_hostIP, hostName);
        }
    }
}
