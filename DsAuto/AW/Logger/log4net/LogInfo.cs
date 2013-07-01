using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DsAuto.AW.Logger.log4net
{
    public class LogInfo
    {
        private string msg;

        public string Msg
        {
            get {
                return msg;
            }
            set {
                msg = value;
            }
        }

        private Color color;

        public Color Color
        {
            get {
                return color;
            }
            set {
                color = value;
            }
        }

        public LogInfo(string _msg, Color _color)
        {
            msg = _msg;
            color = _color;
        }

        public LogInfo(string _msg)
        {
            msg = _msg;
            color = log4net.Color.BLACK;
        }

    }

    //打印信息的颜色
    public enum Color
    {
        //调试信息
        //提示信息
        BLACK,
        //警告信息
        GINK,
        //错误信息
        RED,
        //致命信息
        PURPLE,
    }
}
