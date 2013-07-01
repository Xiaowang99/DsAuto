using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using DsAuto.AW.Remote.SSH;

namespace DsAuto.AW.Remote
{
    interface IEquip
    {
        /// <summary>
        /// 支持TELNET与SSH操作的设备需要支持的操作
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="desStr"></param>
        /// <param name="delayTime"></param>
        void Send(string cmd, string desStr, int delayTime);
    }

    public class TelnetEquip : IEquip
    {
        private IPAddress ip;
        private int port;
        private string user;
        private string pwd;

        private int timeOut;
        private DateTime lastLoginTime = DateTime.MinValue;
        SSHApplication _channel = null;

        public string Ip
        {
            get { return ip.ToString() ; }
            set { ip = IPAddress.Parse(value); }
        }

        public int Port
        {
            get { return port; }
            set { port = value; }
        }

        public SSHApplication Channel
        {
            get { return _channel; }
            set { _channel = value; }
        }

        public string User
        {
            get { return user; }
            set { user = value; }
        }

        public string Pwd
        {
            get { return pwd; }
            set { pwd = value; }
        }

        public int TimeOut
        {
            get { return timeOut; }
            set { timeOut = value;  }
        }

        /// <summary>
        /// 默认登录成功
        /// </summary>
        /// <returns></returns>
        public virtual bool AutoLogin()
        {
            return true;
        }

        public virtual bool Send(string cmd, out string ret, int delayTime, int waitTime = 1000, string desStr = "",bool stringFormat = true)
        {
            ret = "";

            //判断SSH是否登录
            try
            {
                if ((DateTime.Now - lastLoginTime).TotalSeconds >= this.timeOut)
                {
                    if (!AutoLogin())
                    {
                        throw new Exception("autoLogin fail");
                    }
                }
            }
            catch (Exception e)
            {
                throw new UnauthorizedAccessException("fail Login" + e.Message);
            }

            //命令发送2次
            try
            {
                try
                {
                    ret = this.Channel.InputCmd(cmd, desStr, waitTime);
                }
                catch (Exception)
                {
                    ret = this.Channel.InputCmd(cmd, desStr, waitTime);
                }
                lastLoginTime = DateTime.Now;
                return true;
            }
            catch (UnauthorizedAccessException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                lastLoginTime = DateTime.MinValue;
                ret = e.Message;
                return false;
            }
        }

        public void Send(string cmd, string desStr, int delayTime)
        {
            Send(cmd, desStr, delayTime);
        }
    }

}
