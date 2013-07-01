using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DsAuto.AW.Remote.Socket.SocketClient
{
    /// <summary>
    /// Socket客户端
    /// </summary>
    interface ISocketClient
    {
        bool Connect(string ServerIp, int port);

        void Send(string str);

        void Close();
    }
}
