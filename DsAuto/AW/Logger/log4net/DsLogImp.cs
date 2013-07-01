using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net.Core;
using System.Reflection;

namespace DsAuto.AW.Logger.log4net
{
    /// <summary>
    /// 日志模板类,用于自定义日志等级与行为,重载LogImpl与行为
    /// </summary>
    public class DsLogImp : LogImpl
    {
        private readonly static Type thisDeclaringType = typeof(DsLogImp);

        private List<string> propList;

        public DsLogImp(ILogger logger)
            : base(logger)
        {
            propList = typeof(LogInfo).GetProperties().Select(x => x.Name).ToList();
        }

        private LoggingEvent CreateLogEvent(object obj, Exception t)
        {
            string msg = obj.ToString();
            LoggingEvent loggingEvent = new LoggingEvent(thisDeclaringType, Logger.Repository,
                Logger.Name, Level.Info, msg, t);

            Type tp = obj.GetType();
            foreach (string property in propList)
            {
                PropertyInfo p = typeof(LogInfo).GetProperty(property);
                loggingEvent.Properties[property] = p.GetGetMethod().Invoke(obj, null);
            }

            return loggingEvent;
        }

        /// <summary>
        /// 日志与颜色
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="color"></param>
        private void Log(object msg, string color = "")
        {
            LogInfo logInfo;
            if (msg is string)
            {
                logInfo = new LogInfo((string)msg, (Color)Enum.Parse(typeof(Color), color, true));
            }
            else if (msg is LogInfo)
            {
                logInfo = msg as LogInfo;
            }
            else
            {
                throw new NotImplementedException();
            }

            LoggingEvent loggingEvent = CreateLogEvent(logInfo, null);
            Logger.Log(loggingEvent);
        }

        public override void Info(object message)
        {
            Log(message);
        }

        public override void Debug(object message)
        {
            Log(message);
        }

        public override void Warn(object message)
        {
            Log(message);
        }

        public override void Error(object message)
        {
            Log(message);
        }

        public override void Fatal(object message)
        {
            Log(message);
        }
    }
}

