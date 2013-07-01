using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DsAuto.AW.Aw;
using DsAuto.AW.AOP;
using DsAuto.Utility;
using DsAuto.Remote.Telnet;
using System.Reflection;
using DsAuto.AW.Remote.WebService;
using System.Diagnostics;
using System.IO;
using DsAuto.AW.Logger;
using log4net.Appender;
using log4net.Config;
using DsAuto.AW.Logger.log4net;
using log4net;
using DsAuto.AW.processConsole;
using DsAuto.AW.timerTask;
using System.Data;
using DsAuto.Helper.SQLHelper;
using DsAuto.WEB.AW;

namespace TestPro
{
    /// <summary>
    /// 所有试验的小功能都罗列在testMethod里面
    /// </summary>
    [DeploymentItem("IEDriverServer.exe")]
    [TestClass]
    public class UnitTest1
    {
        FileStream fs1= null;
        StreamWriter sw = null;
        StringBuilder sb = new StringBuilder();
        static int times = 0;

        void OnDataReceived(object sender, DataReceivedEventArgs e)
        {
            var data = e;
            sb.Append(data.Data);
            sb.Append("times: " + times.ToString());
            sw.WriteLine(data.Data);
            sw.WriteLine("times: " + times.ToString());

        }

        void OnDataAllReceived(object sender, EventArgs e)
        {
            times++;
            sw.WriteLine("times: " + times.ToString());
            sw.Close();
            fs1.Close();
        }

        MemoConsoleAppender ConsoleAppender;

        log4net.Appender.FileAppender FileAppender;

        log4net.ILog log;

        public void LogInt(string fileName)
        {
            log = DsAuto.AW.Logger.log4net.LogManager.GetLogger("DsLog");

            if (FileAppender == null)
            {
                FileAppender = new FileAppender()
                {
                    Layout = new log4net.Layout.PatternLayout("PRE style=\"color:%property{Color}\">%d   %property{Msg}</PRE>"),
                    AppendToFile = true,
                };
             }
            FileAppender.File = Path.Combine(@"E:\", string.Format("TestProcedure.{0}.html", fileName));
            FileAppender.ActivateOptions();

             if (ConsoleAppender == null)
             {
                 ConsoleAppender = new MemoConsoleAppender()
                 {
                     Layout = new log4net.Layout.PatternLayout("%property{Color}|%d %property{Msg}%n")
                 };
                 ConsoleAppender.ActivateOptions();
                 log4net.Config.BasicConfigurator.Configure(ConsoleAppender);
             }
        }

        [TestMethod]
        public void TestMethod1()
        {
            #region[AOP]
            //AwBase _t = AwBase.GetAwInstance();
            //_t.method();
            #endregion

            #region[Reflection]
            //Assembly _a = Assembly.LoadFile(@"E:\Vs_Project\DsAuto\TestPro\bin\Debug\DsAuto.dll");

            //Type[] _types = _a.GetTypes();

            //foreach (Type temp in _types)
            //{
            //    string fullName = temp.FullName;

            //    if (fullName.Contains( "AuthorInfoAttribute") && temp.GetType().IsClass == true)
            //    {
            //        var o = (AuthorInfoAttribute)_a.CreateInstance(fullName);
            //        Console.Write(o.IsDefaultAttribute() );
            //    }
            //}
            #endregion

            #region [ModBase]
            //ModeDemo v = new ModeDemo();
            //Console.WriteLine(v.Name);
            #endregion

            #region [TelnetAction]
            //Telnet _t = new Telnet("127.0.0.1", 12345, 50);
            //_t.Send("hello");
            #endregion

            #region[WebService]
            //WebServiceApp obj = new WebServiceApp(@"http://localhost:56835/HelloWorld.asmx?wsdl", "HelloWorld");
            //string[] param = new string[2]{"hello","world"};
            //object site1 = obj.Obj("Participate");
            //WebServiceApp.setValue(site1, "Str", "abc");
            //object site2 = obj.Obj("Participate");
            //WebServiceApp.setValue(site2, "Str", "abcd");
            //object[] siteAry = obj.ObjAry("Participate", 2);
            //siteAry[0] = site1;
            //siteAry[1] = site2;
            //obj.InvokeMethod("ReHelloWorldSiteList", new object[] { siteAry });
            #endregion

            #region[WEB]
            //To do
            //本人对Web不是很熟悉，因此这部分稍后再完成
            //基于Selenium开源web自动化项目
            //IERobot ie = new IERobot();
            //ie.Boot();
            //ie.Url = "www.baidu.com";
            #endregion

            #region[异步取数据]
            //if (File.Exists("e:\\TestTxt.txt"))
            //{
            //    File.Delete("e:\\TestTxt.txt");
            //    fs1 = new FileStream("e:\\TestTxt.txt", FileMode.Create, FileAccess.Write);
            //    //创建写入文件                 
            //    sw = new StreamWriter(fs1);
            //    //开始写入值                
            //}
            //else
            //{
            //    fs1 = new FileStream("e:\\TestTxt.txt", FileMode.Create, FileAccess.Write);
            //    //创建写入文件                 
            //    sw = new StreamWriter(fs1);
            //}

            //Process p = new Process();
            //p.StartInfo.FileName = @"E:\MutexInCSharp.exe";
            //p.StartInfo.CreateNoWindow = false;
            //p.StartInfo.RedirectStandardOutput = true;
            //p.StartInfo.UseShellExecute = false;
            //p.EnableRaisingEvents = true;
            
            //p.OutputDataReceived += new DataReceivedEventHandler(OnDataReceived);
            //p.Exited += new EventHandler(OnDataAllReceived);
           
            //p.Start();
            //p.BeginOutputReadLine();
            //p.WaitForExit();


            #endregion

            #region[DsLogger]
            //DsLogger log = new DsLogger();
            //log.LoggerLevel = DsLoggerLevel.FATAL;
            //log.Debug("ddd");
            //log.Fatal("fatal");
            //log.Infor("dddss");
            //log.Warn("dsdsd");
            #endregion

            #region[log4net日志模块]
            //LogInt("gtr");
            //log.Debug(new LogInfo("ddd", Color.PURPLE));
            //log.Info(new LogInfo("ddd", Color.RED));
            #endregion

            #region[processConsole模块]
            //processControl pConsole = new processControl();
            //pConsole.DataHandler += (obj) =>
            //{
            //    Console.WriteLine("DataHandler");
            //};

            //pConsole.ExitedHandler += (obj) =>
            //{
            //    Console.WriteLine("ExitedHandler");
            //};

            //processControl preConsole = new processControl();
            //preConsole.DataHandler += (obj)=>
            //{
            //    ;
            //};

            //preConsole.ExitedHandler += (obj) =>
            //{
            //    pConsole.Excute(new object[]{@"E:\MutexInCSharp.exe"});
            //};

            //preConsole.Excute(new object[]{@"E:\MutexInCSharp.exe"});

            //pConsole.Excute(new object[]{@"E:\MutexInCSharp.exe"});
            #endregion

            #region[timer模块]
            //TimeSchedule t = new TimeSchedule(DateTime.Now.AddHours(2));
            //t.Task += (obj)=>
            //{
            //    pConsole.Excute(new object[]{@"E:\MutexInCSharp.exe"});
            //};

            //t.Start();
            #endregion

            #region[SQLHelper类]
            //int result = SQLHelper.ExecuteCommand("SELECT * FROM Lesson");
            #endregion

            #region[SQLiteHelper类]
            //SQLITEHelper sqliteInS = new SQLITEHelper();
            //sqliteInS.CreateDB("sqliteDb");
            //sqliteInS.ExcuteSql("create table tbl_testcase(name nvarchar(20), testCluster nvarchar(20))");
            #endregion          
        }
    }

    #region [ModBase]
    //public class ModeDemo : ModBase
    //{
    //    public string name;

    //    public string Name
    //    {
    //        get { return name; }
    //        set { name = value; }
    //    }

    //    public string NAme()
    //    {
    //        return "Name";
    //    }
    //}
    #endregion
}
