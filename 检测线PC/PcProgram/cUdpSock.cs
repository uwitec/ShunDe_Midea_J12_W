using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using Microsoft.Win32;
using System.Windows.Forms;
namespace PcProgram
{
    public class cUdpSock
    {
        static string tempLastIp = "1";
        UdpClient mUdpClient;
        Thread mUdpThread;
        /// <summary>
        /// UDP端口数据 到达事件
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e">附带接收字符</param>
        public delegate void mDataReciveStringEventHandle(object o, RecieveStringArgs e);//委托
        public delegate void mDataReciveByteEventHandle(object o, RecieveByteArgs e);//委托
        public event mDataReciveStringEventHandle mDataReciveString;//事件
        public event mDataReciveByteEventHandle mDataReciveByte;//事件
        bool isOutClass = false;

        bool isOpen = false;//端口是否已打开
        /// <summary>
        /// 端口是否已打开
        /// </summary>
        public bool IsOpen
        {
            get { return isOpen; }
            set { isOpen = value; }
        }

        string _RemoteHostIp = "127.0.0.1";
        /// <summary>
        /// 远程主机发送过来的IP地址
        /// </summary>
        public string pRemoteHostIp
        {
            get { return _RemoteHostIp; }
        }
        int _RemoteHostPort = 20000;

        /// <summary>
        /// 远程主机发送过来的port端口
        /// </summary>
        public int pRemoteHostPort
        {
            get { return _RemoteHostPort; }
        }
        string errorStr = "";//出错信息
        /// <summary>
        /// 出错信息
        /// </summary>
        public string pErrorStr
        {
            get { return errorStr; }
            set { errorStr = value; }
        }
        int localPort = 20000;//本地端口
        /// <summary>
        /// 本地端口
        /// </summary>
        public int pLocalPort
        {
            get { return localPort; }
            set { localPort = value; }
        }
        int remotPort = 20001;//远程端口
        /// <summary>
        /// 远程端口
        /// </summary>
        public int pRemotPort
        {
            get { return remotPort; }
            set { remotPort = value; }
        }
        string remotHost = "127.0.0.1";//远程主机地址
        /// <summary>
        /// 远程主机IP
        /// </summary>
        public string pRemotHost
        {
            get { return remotHost; }
            set { remotHost = value; }
        }
        /// <summary>
        /// 构造函数,初始化类
        /// </summary>
        /// <param name="mLocalPort">本地端口</param>
        /// <param name="mRemotPort">远程端口</param>
        /// <param name="mRemotHost">远程主机IP地址</param>
        public cUdpSock(int mLocalPort, int mRemotPort, string mRemotHost)
        {
            localPort = mLocalPort;
            remotPort = mRemotPort;
            remotHost = mRemotHost;
            cUdpInit();
        }
        /// <summary>
        /// 构造函数,初始化类
        /// </summary>
        /// <param name="mLocalPort">本地端口</param>
        public cUdpSock(int mLocalPort)
        {
            localPort = mLocalPort;
            cUdpInit();
        }
        private void cUdpInit()//初始化UDP类,打开多线程.
        {
            try
            {
                mUdpClient = new UdpClient(localPort);
            }
            catch
            {
                errorStr = errorStr + DateTime.Now.ToString() + "   UDP客户端创建失败,端口已被占用" + "\n";
                return;
            }
            try
            {
                mUdpThread = new Thread(new ThreadStart(udpListen));
                mUdpThread.IsBackground = true;
                mUdpThread.Start();
            }
            catch
            {
                errorStr = errorStr + DateTime.Now.ToString() + "   创建线程失败" + "\n";
                return;
            }
            IsOpen = true;
        }
        /// <summary>
        /// 循环监听端口,数据到达时,触发事件
        /// </summary>
        private void udpListen()//
        {
            try
            {
                IPEndPoint ie = new IPEndPoint(IPAddress.Parse(remotHost), remotPort);
                while (!isOutClass)
                {
                    byte[] b = mUdpClient.Receive(ref ie);

                    _RemoteHostIp = ie.Address.ToString();
                    _RemoteHostPort = ie.Port;
                    if (mDataReciveByte != null)
                    {
                        mDataReciveByte(null, new RecieveByteArgs((_RemoteHostPort % 100), ie.Address.ToString(), b));
                    }
                    string s = Encoding.GetEncoding("gb2312").GetString(b, 0, b.Length);
                    if (mDataReciveString != null)
                    {
                        mDataReciveString(null, new RecieveStringArgs((_RemoteHostPort % 100), ie.Address.ToString(), s));
                    }
                }
            }
            catch
            {
                udpListen();
            }
        }
        /// <summary>
        /// 将指定字符串发送到初始化的远程主机端口
        /// </summary>
        /// <param name="mSendStr">发送的字符串</param>
        /// <returns>发送过程是否出错</returns>
        public bool fUdpSend(string mSendStr)//将指定字符串发送到初始化的远程主机端口
        {
            try
            {
                byte[] b = Encoding.GetEncoding("gb2312").GetBytes(mSendStr);
                IPEndPoint ie = new IPEndPoint(IPAddress.Parse(remotHost), remotPort);
                mUdpClient.Send(b, b.Length, ie);
            }
            catch
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 将指定字符串发送到指定主机,指定端口
        /// </summary>
        /// <param name="mRemotHost">远程主机IP地址</param>
        /// <param name="mRemotPort">远程主机端口</param>
        /// <param name="mSendStr">发送字符串</param>
        /// <returns>返回发送是否出错</returns>
        public bool fUdpSend(string mRemotHost, int mRemotPort, string mSendStr)//将指定字符串发送到指定IP指定端口
        {
            try
            {
                byte[] b = Encoding.GetEncoding("gb2312").GetBytes(mSendStr);
                IPEndPoint ie = new IPEndPoint(IPAddress.Parse(mRemotHost), mRemotPort);
                mUdpClient.Send(b, b.Length, ie);
            }
            catch
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 关闭UDP端口,关闭UDP线程
        /// </summary>
        /// <returns>返回关闭是否成功</returns>
        public bool fUdpClose()//关闭UDP端口.关闭UDP线程
        {
            bool ReturnValue = false;
            try
            {
                isOutClass = true;
                mUdpThread.Abort();
                mUdpClient.Close();
                mUdpClient = null;
                mDataReciveByte = null;
                mDataReciveString = null;
                ReturnValue = true;
                IsOpen = false;
            }
            catch
            {
                ReturnValue = false;
            }
            return ReturnValue;
        }
        /// <summary>
        /// 获取本机IP地址最后一位
        /// </summary>
        /// <returns>string,本机IP地址最后一位</returns>
        public static string LastIp()
        {
            return tempLastIp;
        }
        private static string NetBoardName()
        {
            string localIDName = "DM9000A";
            object value = Registry.GetValue(string.Format(@"HKEY_LOCAL_MACHINE\Comm\{0}\Linkage", "Tcpip"), "Bind", "DM9000A");
            if (value != null)
            {
                if (value.GetType() == typeof(string))
                {
                    localIDName = (string)value;
                }
                if (value.GetType() == typeof(string[]))
                {
                    localIDName = ((string[])value)[0];
                }
            }
            return localIDName;

        }
        /// <summary>
        /// 获取本地IP地址信息
        /// </summary>
        /// <returns>IPAddress,获取本地IP地址信息</returns>
        public static IPAddress LoaclIp()
        {
            string localIpGet = "127.0.0.1";
            object value = Registry.GetValue(string.Format(@"HKEY_LOCAL_MACHINE\Comm\{0}\Parms\TcpIp", NetBoardName()), "IpAddress", "127.0.0.1");
            if (value != null)
            {
                if (value.GetType() == typeof(string))
                {
                    localIpGet = (string)value;
                    tempLastIp = localIpGet.Substring(localIpGet.LastIndexOf(".") + 1);
                }
                if (value.GetType() == typeof(string[]))
                {
                    localIpGet = ((string[])value)[0];
                    tempLastIp = localIpGet.Substring(localIpGet.LastIndexOf(".") + 1);
                }
            }
            IPAddress localip = IPAddress.Parse(localIpGet);
            return localip;
        }
    }
    public class RecieveStringArgs : EventArgs
    {
        public readonly string RemostHost;
        public readonly string ReadStr;
        public readonly int RemotNum;
        public RecieveStringArgs(int mRemotNum,string mRemostHost, string mReadStr)
        {
            RemotNum=mRemotNum;
            RemostHost = mRemostHost;
            ReadStr = mReadStr;
        }
    }
    public class RecieveByteArgs : EventArgs
    {
        public readonly string RemostHost;
        public readonly byte[] ReadByte;
        public readonly int RemotNum;
        public RecieveByteArgs(int mRemotNum, string mRemostHost, byte[] mReadByte)
        {
            RemotNum = mRemotNum;
            RemostHost = mRemostHost;
            ReadByte = mReadByte;
        }
    }
}
