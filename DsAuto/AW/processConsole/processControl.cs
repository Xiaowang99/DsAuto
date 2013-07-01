using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace DsAuto.AW.processConsole
{
    /// <summary>
    /// 进程控制器,可以绑定接受消息的事件+结束事件处理
    /// </summary>
    public class processControl
    {
        public delegate void  Action(object x);

        public event Action DataHandler;

        public event Action ExitedHandler;

        public void Excute(object args)
        {
            Process p = new Process();
            var parameters = args as object[];

            p.StartInfo.CreateNoWindow = false;
            p.StartInfo.FileName = (string)parameters[0];
            p.StartInfo.UseShellExecute = false;
            p.EnableRaisingEvents = true;
            p.StartInfo.RedirectStandardOutput = true;

            p.OutputDataReceived += new DataReceivedEventHandler(DataReceivedHandler);
            p.Exited += new EventHandler(EventHandler);

            p.Start();
            p.BeginOutputReadLine();
            p.WaitForExit();

            //p.OutputDataReceived += new DataReceivedEventHandler(OnDataReceived);
            //p.Exited += new EventHandler(OnDataAllReceived);

            //p.Start();
            //p.BeginOutputReadLine();
            //p.WaitForExit();
        }

        private void DataReceivedHandler(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Data))
            {
                if (DataHandler != null)
                    DataHandler(e.Data);
            }
        }

        private void EventHandler(object sender, EventArgs e)
        {
            if (ExitedHandler != null)
                ExitedHandler(null);
        }

        public void Start(string path, string testFilePath)
        {
            Thread _t = new Thread(Excute);
            _t.Start(new object[] { path, testFilePath });
        }
    }
}
