using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net.Appender;
using log4net.Core;
using log4net;
using System.IO;

namespace DsAuto.AW.Logger.log4net
{
    interface Logger
    {
    }

    public class MemoConsoleAppender : AppenderSkeleton
    {
        public Action<Color, string> ConsoleAction;

        protected override void Append(LoggingEvent loggingEvent)
        {
            StringWriter writer = new StringWriter();
            this.Layout.Format(writer, loggingEvent);

            var msg = writer.ToString();
            var index = msg.IndexOf('|');
            var color = Color.BLACK;
            if (index != -1)
            {
                var sColor = msg.Substring(0, index);
                if (!string.IsNullOrWhiteSpace(sColor))
                    //这儿颜色转换稍后再处理
                    ;
            }
            ConsoleAction(color, msg);
        }
    }
}
