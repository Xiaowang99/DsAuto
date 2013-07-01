using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace DsAuto.AW.timerTask
{
    public class TimeSchedule
    {
        public delegate void Execute(object x);

        public event Execute Task;

        /// <summary>
        /// 准时执行任务的时间
        /// </summary>
        DateTime t = DateTime.MinValue;

        Timer task = new Timer(1000);

        public TimeSchedule(DateTime _t )
        {
            t = _t;
            //一秒钟执行一次
            task.Elapsed += new ElapsedEventHandler(ScheduleTask);
        }

        public void Start()
        {
            task.Start();
        }

        private void ScheduleTask(object sender, ElapsedEventArgs e)
        {
            task.Stop();
            var x = e;
            if (DateTime.Now > t)
            {
                if (Task != null)
                    Task(null);
            }
            else
            {
                task.Start();
            }
        }
    }
}
