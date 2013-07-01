using System;
using System.Text;
using Routrek.SSHC;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net;

namespace DsAuto.AW.Remote.SSH
{
    public class SSHReader :ISSHConnectionEventReceiver, ISSHChannelEventReceiver
    {
        public SSHConnection _conn;
        public bool _ready;

        /// <summary>
        /// 这个方法只是为了切面,不用来直接构造
        /// </summary>
        public SSHReader()
        {
 
        }
        /// <summary>
        /// 用来存放回显信息的开关
        /// </summary>
        public string Msg
        {
            get;
            set;
        }
        
        /// <summary>
        /// SSH设备的Ip
        /// </summary>
        public string Ip
        {
            get;
            set;
        }

        /// <summary>
        /// SSH设备的端口号
        /// </summary>
        public int Port
        {
            get;
            set;
        }

        /// <summary>
        /// SSH超时登出的时间
        /// </summary>
        public int TimeOut
        {
            get;
            set;
        }

        public void OnData(byte[] data, int offset, int length)
        {
            Msg = (Encoding.ASCII.GetString(data, offset, length));
            System.Console.Write(Msg);
            
        }
        public void OnDebugMessage(bool always_display, byte[] data)
        {
            Debug.WriteLine("DEBUG: " + Encoding.ASCII.GetString(data));
        }
        public void OnIgnoreMessage(byte[] data)
        {
            Debug.WriteLine("Ignore: " + Encoding.ASCII.GetString(data));
        }
        public void OnAuthenticationPrompt(string[] msg)
        {
            Debug.WriteLine("Auth Prompt " + msg[0]);
        }

        public void OnError(Exception error, string msg)
        {
            Debug.WriteLine("ERROR: " + msg);
        }
        public void OnChannelClosed()
        {
            Debug.WriteLine("Channel closed");
            _conn.Disconnect("");
            //_conn.AsyncReceive(this);
        }
        public void OnChannelEOF()
        {
            _pf.Close();
            Debug.WriteLine("Channel EOF");
        }
        public void OnExtendedData(int type, byte[] data)
        {
            Debug.WriteLine("EXTENDED DATA");
        }
        public void OnConnectionClosed()
        {
            Debug.WriteLine("Connection closed");
        }
        public void OnUnknownMessage(byte type, byte[] data)
        {
            Debug.WriteLine("Unknown Message " + type);
        }
        public void OnChannelReady()
        {
            _ready = true;
        }
        public void OnChannelError(Exception error, string msg)
        {
            Debug.WriteLine("Channel ERROR: " + msg);
        }
        public void OnMiscPacket(byte type, byte[] data, int offset, int length)
        {
        }

        public PortForwardingCheckResult CheckPortForwardingRequest(string host, int port, string originator_host, int originator_port)
        {
            PortForwardingCheckResult r = new PortForwardingCheckResult();
            r.allowed = true;
            r.channel = this;
            return r;
        }
        public void EstablishPortforwarding(ISSHChannelEventReceiver rec, SSHChannel channel)
        {
            _pf = channel;
        }

        public SSHChannel _pf;
    }

    /// <summary>
    /// 把这个SSHapplication做成切面aw
    /// </summary>
    public class SSHApplication :Aw.AwBase
    {
        /// <summary>
        /// SSHReader配置类
        /// </summary>
        private SSHReader _sshReader = null;

        /// <summary>
        /// SSHConnection 参数设置类
        /// </summary>
        private SSHConnectionParameter f = null;

        /// <summary>
        /// SSHConnection 连接
        /// </summary>
        private SSHConnection _conn = null;

        /// <summary>
        /// SSH通道
        /// </summary>
        public SSHChannel ch = null;

        /// <summary>
        /// socket连接
        /// </summary>
        public Socket _socket = null;

        /// <summary>
        /// SSH的Ip选项
        /// </summary>
        public string Ip
        {
            set;
            get;
        }

        /// <summary>
        /// 端口号
        /// </summary>
        public int Port
        {
            set;
            get;
        }

        /// <summary>
        /// SSH超时登出
        /// </summary>
        public int TimeOut
        {
            set;
            get;
        }

        /// <summary>
        /// 此处的私有构造方法只是为了AOP
        /// 不要用这个方法进行构造
        /// </summary>
        public SSHApplication()
        {
        }

        /// <summary>
        /// 暂时没用貌似
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="timeOut"></param>
        public void InitSSHApp(string ip, int port, int timeOut)
        {
            Ip = ip;
            Port = port;
            TimeOut = timeOut;
            
        }

        public static SSHApplication SSHAppGetIns(string ip, int port, int timeOut, string userName, string userPwd)
        {
            SSHApplication _tempSSHApp = null;
            _tempSSHApp = (SSHApplication)SSHApplication.GetAwInstance(typeof(SSHApplication));
            //实例化这个f
            if (_tempSSHApp.f == null)
            {
                _tempSSHApp.f = new SSHConnectionParameter();
                _tempSSHApp.f.UserName = userName;
#if false //SSH1
			//SSH1
			f.Password = "";
			f.Protocol = SSHProtocol.SSH2;
			f.AuthenticationType = AuthenticationType.Password;
			f.PreferableCipherAlgorithms = new CipherAlgorithm[] { CipherAlgorithm.Blowfish, CipherAlgorithm.TripleDES };
#else //SSH2
			    _tempSSHApp.f.Password = userPwd;
			    _tempSSHApp.f.Protocol = SSHProtocol.SSH2;
			    _tempSSHApp.f.AuthenticationType = AuthenticationType.Password;
			    _tempSSHApp.f.WindowSize = 0x1000;
#endif
            }

            if(_tempSSHApp._sshReader == null)
            {
                _tempSSHApp._sshReader = new SSHReader();
            }

		    _tempSSHApp._socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			//s.Blocking = false;
			_tempSSHApp._socket.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
            _tempSSHApp._conn = SSHConnection.Connect(_tempSSHApp.f, _tempSSHApp._sshReader, _tempSSHApp._socket);
            _tempSSHApp._sshReader._conn = _tempSSHApp._conn;
            return _tempSSHApp;
        }

        /// <summary>
        /// 打开SSH通道
        /// </summary>
        public void OpenSSH()
        {
            //打开之前先进行一次关闭？
            CloseSSH();

            ch = _conn.OpenShell(_sshReader);
			_sshReader._pf = ch;
        }

        /// <summary>
        /// 关闭SSH通道
        /// </summary>
        public void CloseSSH()
        {
             if(!_conn.IsClosed)
            {
                _conn.Close();
            }
        }

        /// <summary>
        /// SSH输入接口
        /// </summary>
        /// <param name="command"></param>
        /// <param name="endRecStr"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public string InputCmd(string command, string endRecStr, int timeOut)
        {
            _sshReader.Msg = null;
            byte[] data = Encoding.ASCII.GetBytes(command.TrimEnd('\r','\n'));
            _sshReader._pf.Transmit(data);
            var isWait = WaitString(endRecStr, timeOut);
            return _sshReader.Msg;
            
        }

        /// <summary>
        /// 返回以endRecStr结尾的回显信息,不然就继续等待
        /// </summary>
        /// <param name="command"></param>
        /// <param name="endRecStr"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        private string WaitString(string endRecStr, int timeOut)
        {
            int deltaTime = 1000;
            while(_sshReader.Msg != null)
            {
                string tempStr = _sshReader.Msg;

                if(_sshReader.Msg.Contains(endRecStr))
                    break;

                if(timeOut >= deltaTime)
                {
                    System.Threading.Thread.Sleep(deltaTime);
                }
                else
                    break;
            }
            return _sshReader.Msg;
        }
			
    }
}
