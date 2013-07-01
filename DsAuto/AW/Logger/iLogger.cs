using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;
using log4net.Plugin;
using log4net;
using log4net.Core;

namespace DsAuto.AW.Logger
{
    //日志调试等级
    public enum DsLoggerLevel
    {
        //NULL
        NULL,
        //调试信息
        DEBUG,
        //提示信息
        INFOR,
        //警告信息
        WRAN,
        //错误信息
        ERROR,
        //致命信息
        FATAL,
    }

    interface IDsLogger
    {
        void Debug(string msg);

        void Infor(string msg);

        void Warn(string msg);

        void Error(string msg);

        void Fatal(string msg);

    }

    public class DsLogger : IDsLogger
    {
        public DsLoggerLevel LoggerLevel
        {
            get;
            set;
        }

        public void Debug(string msg)
        {
            if (LevelAllow(DsLoggerLevel.DEBUG))
                Console.WriteLine("Debug:     " + msg);
        }

        public void Infor(string msg)
        {
            if(LevelAllow(DsLoggerLevel.INFOR))
                Console.WriteLine("Infor:     " + msg);
        }

        public void Warn(string msg)
        {
            if(LevelAllow(DsLoggerLevel.WRAN))
                Console.WriteLine("Warn:     " + msg);
        }

        public void Error(string msg)
        {
            if(LevelAllow(DsLoggerLevel.ERROR))
                Console.WriteLine("Error:     " + msg);
        }

        public void Fatal(string msg)
        {
            if(LevelAllow(DsLoggerLevel.FATAL))
                Console.WriteLine("Fatal:     " + msg);
        }

        private bool LevelAllow(DsLoggerLevel loggerLevel)
        {
            DsLoggerLevel curLevel = (LoggerLevel == null) ? DsLoggerLevel.NULL : LoggerLevel;
            return (int)loggerLevel >= (int)curLevel;
        }
    }
}
