using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace DsAuto.AW.Remote.Socket.SocketClient
{
    public class SocketClient : ISocketClient
    {
        /// <summary>
        /// ipServer的IP
        /// </summary>
        string _ip = null;

        /// <summary>
        /// ipServer的端口号
        /// </summary>
        int _port = 0;

        public SocketClient(string ip, int port)
        {
            _ip = ip;
            _port = port;
            
        }
    }

    class tcpclient
    {
        public bool IsConnect = false;
        public bool IsSend = false;
        public bool IsRev = false;
        // 负责接收的线程 
        
        private Socket client = null;
        //  private static ManualResetEvent receiveDone = new ManualResetEvent(false);
        private static ManualResetEvent sendDone = new ManualResetEvent(false);
        private static ManualResetEvent connectDone = new ManualResetEvent(false);

        //static bool isconn = false;

        public static String m_status = String.Empty;
        // 接收字节大小.
        public const int BufferSize = 256;
        // 缓冲字节数组.
        public static byte[] buffer = new byte[BufferSize];

        //获取本地IP地址
        public string Client()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            return ipAddress.ToString();
        }
        //建立连接
        public bool conn(string serverIP, int partIP)
        {
            try
            {
                IPEndPoint remoteEP = new IPEndPoint(System.Net.IPAddress.Parse(serverIP), partIP);//将网络端点表示为 IP 地址和端口号
                // 创立Socket连接
                Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//IP 版本 4 的地址//支持可靠、双向、基于连接的字节流，而不重复数据，也不保留边界//传输控制协议

                connectDone.Reset();//将事件状态设置为非终止状态，导致线程阻止
                client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallBack), client);//开始一个对远程主机连接的异步请求。//
                connectDone.WaitOne();//当在派生类中重写时，阻止当前线程，直到当前的 WaitHandle 收到信号


            }
            catch
            {
                IsConnect = false;
                MessageBox.Show("错误1:\r\n连接失败！！", "提示信息");
            }
            return IsConnect;
        }
        private void ConnectCallBack(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState; //获取异步操作的状态
                //response = "与主机" + tbIpAddr.Text + "端口" + tbPort.Text + "连接成功";
                //isconn = true;
                client.EndConnect(ar);//结束挂起的异步连接请求
                // Thread thread = new Thread(new ThreadStart(recvThread));
                try
                {
                    byte[] byteData = Encoding.Default.GetBytes("准备完毕，可以通话" + "\r\n");
                    client.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallBack), client);//将数据异步发送到连接的 Socket
                    sendDone.WaitOne();
                }
                catch (Exception ee) { MessageBox.Show(ee.Message); }
                IsConnect = true;
                Thread thread = new Thread(new ThreadStart(recvThread));
                thread.Start();   //开始接收数据线程
                connectDone.Set();  //将指定事件的状态设置为终止。

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                IsConnect = false;
                connectDone.Set();
            }
        }
        public void SendCallBack(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;
                sendDone.Set();
                IsSend = true;
            }
            catch //(Exception exp)
            {
                sendDone.Set();
                IsSend = false;
                IsConnect = false;
            }
        }
        public void recvThread()//线程
        {
            try
            {
                client.BeginReceive(buffer, 0, BufferSize, 0, new AsyncCallback(ReceiveCallback), client);
            }
            catch //(Exception ee)
            {
                IsSend = false;
                IsConnect = false;

            }
        }

        public void ReceiveCallback(IAsyncResult ar)
        {
            if (IsConnect == false)
            {
                return;
            }
            Socket client = (Socket)ar.AsyncState;
            try
            {
                int bytesRead = client.EndReceive(ar);//结束挂起的异步读取。返回接收到的字节数。
                if (bytesRead > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(Encoding.Default.GetString(buffer, 0, bytesRead));//储存数据
                    m_status = "接收到" + client.RemoteEndPoint + "服务器数据：" + sb.ToString();
                    client.BeginReceive(buffer, 0, BufferSize, 0, new AsyncCallback(ReceiveCallback), client);//开始从连接的 Socket 中异步接收数据
                }
                else
                {
                    IsConnect = false;
                }
            }
            catch //(Exception e)
            {
                IsConnect = false;
            }
        }

        public void interrupt()//中断连接
        {
            try
            {
                //isconn = true;
                client.Shutdown(SocketShutdown.Both);
                client.Close();
                //client = null;
            }
            catch
            {
                MessageBox.Show("连接尚未建立,关闭无效");
            }
        }
        public bool sengdata(byte[] byteData, int len)
        {
            try
            {
                IsSend = true;
                if (IsConnect)
                {
                    sendDone.Reset();
                    client.BeginSend(byteData, 0, len, 0, new AsyncCallback(SendCallBack), client);
                    sendDone.WaitOne();  //等待回调函数发送完毕
                }
            }
            catch //(Exception ee)
            { IsConnect = false; }
            return IsSend;
        }//发送数据
        public void closeDO()
        {
            try
            {
                if (IsConnect)
                {
                    IsConnect = false;
                    client.Shutdown(SocketShutdown.Both);
                    client.Close();
                    client = null;
                }
            }
            catch { }
        }//中断Socket连接
    }
}
