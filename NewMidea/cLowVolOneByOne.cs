using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
namespace NewMideaProgram
{
    public class cLowVolOneByOne
    {
        public class Admin
        {
            List<int> OneByOne = new List<int>();
            cUdpSock udp;
            public Admin(int localPort)
            {
                udp = new cUdpSock(localPort);
                udp.mDataReciveString += new cUdpSock.mDataReciveStringEventHandle(udp_mDataReciveString);
            }

            public Admin()
                : this(10100)
            { }
            private void udp_mDataReciveString(object o, RecieveStringArgs e)
            {
                string result = e.ReadStr;
                string[] buff = result.Split('~');
                if (buff == null || buff.Length < 3)
                {
                    return;
                }
                switch (buff[0])
                {
                    case "Access":
                        if (!OneByOne.Contains(buff[2].ToInt()))
                        {
                            OneByOne.Add(buff[2].ToInt());
                        }
                        if (OneByOne.Count > 0)
                        {
                            result = string.Format("{0}~{1}", result, OneByOne[0]);
                        }
                        else
                        {
                            result = string.Format("{0}~{1}", result, -1);
                        }
                        break;
                    case "Del":
                        if (OneByOne.Contains(buff[2].ToInt()))
                        {
                            OneByOne.Remove(buff[2].ToInt());
                        }
                        break;
                }
                udp.fUdpSend(e.RemostHost, e.RemotPort, result);
            }
        }
        public class Client
        {
            bool sendAccess = false;
            bool allowAccess = false;

            bool delAccess = false;

            int random = 0;
            public int TimeOut
            { get; set; }
            public int LocalPort
            { get; set; }
            cUdpSock udp;
            public Client(int localPort, string remotHost)
                : this(localPort, remotHost, 10100)
            {
            }
            public Client(int localPort, string remotHost, int remotPort)
            {
                this.TimeOut = 500;
                this.LocalPort = localPort;
                udp = new cUdpSock(localPort, remotPort, remotHost);
                udp.mDataReciveString += new cUdpSock.mDataReciveStringEventHandle(udp_mDataReciveString);
            }
            /// <summary>
            /// 发送请求
            /// </summary>
            /// <param name="msg"></param>
            /// <returns></returns>
            public bool GetAccess()
            {
                allowAccess = false;
                random = (int)(Num.Rand() * 10000);
                send(string.Format("Access~{0}~{1}", random, LocalPort), ref sendAccess);
                return allowAccess;
            }
            /// <summary>
            /// 删除请求
            /// </summary>
            /// <returns></returns>
            public bool Del()
            {
                random = (int)(Num.Rand() * 1000);
                send(string.Format("Del~{0}~{1}", random, LocalPort), ref delAccess);
                return delAccess;
            }
            private void send(string value, ref bool result)
            {
                result = false;
                int timeOut = 0;
                int sendTimes = 1;
                while (sendTimes > 0)
                {
                    timeOut = 0;
                    udp.fUdpSend(value);
                    while (timeOut * 100 < TimeOut)
                    {
                        timeOut++;
                        Thread.Sleep(100);
                        if (result)
                        { break; }
                    }
                    if (result)
                    { break; }
                    sendTimes--;
                }
            }
            private void udp_mDataReciveString(object o, RecieveStringArgs e)
            {
                string[] buff = e.ReadStr.Split('~');
                if (buff == null || buff.Length < 3)
                {
                    return;
                }
                switch (buff[0])
                {
                    case "Access":
                        if (buff.Length == 4)
                        {
                            if (buff[3].ToInt() == this.LocalPort)
                            {
                                allowAccess = true;
                            }
                            if (buff[1].ToInt() == random && buff[2].ToInt() == this.LocalPort)
                            {
                                sendAccess = true;
                            }
                        }
                        break;
                    case "Del":
                        if (buff[1].ToInt() == random && buff[2].ToInt() == this.LocalPort)
                        {
                            delAccess = true;
                        }
                        break;
                }
            }
        }
    }
}
