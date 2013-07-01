using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.ObjectRenderer;

namespace DsAuto.AW.Logger.log4net
{
    /// <summary>
    /// 日志管理类,用于生成log操作类
    /// </summary>
    public class LogManager
    {
        public static string name;

        private static ILoggerWrapper WrapperCreationHandle(ILogger logger)
        {
            return new DsLogImp(logger);
        }

        public static ILog GetLogger(Assembly assembly, string name)
        {
            LogManager.name = name;

            var wrapperMap = new WrapperMap(new WrapperCreationHandler(WrapperCreationHandle));
            return (ILog)wrapperMap.GetWrapper(LoggerManager.GetLogger(assembly, name));
        }

        public static ILog GetLogger(string name)
        {
            return GetLogger(Assembly.GetCallingAssembly(), name);
        }
    }
}
