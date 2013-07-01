using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DsAuto.WEB.UIMode;
using DsAuto.WEB.Mode;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;

namespace DsAuto.WEB.AW
{
    public class IERobot: IRobot
    {
        /// <summary>
        /// IE浏览器驱动
        /// </summary>
        InternetExplorerDriver _driver;
        /// <summary>
        /// 选中元素的高亮处理
        /// </summary>
        public string HLColor { get; set; }

        /// <summary>
        /// 回退
        /// </summary>
        /// <returns></returns>
        public bool Back()
        {
            var result = _driver.ExecuteScript("location.go(-1);return 1");
            if ((int)result == 1)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 向前
        /// </summary>
        /// <returns></returns>
        public bool Forward()
        {
            var result = _driver.ExecuteScript("location.go(1);return 1");
            if ((int)result == 1)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 窗口最大化
        /// </summary>
        /// <returns></returns>
        public bool Max()
        {
            return true;
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <returns></returns>
        public bool Refresh()
        {
            return true;
        }

        /// <summary>
        /// 重启浏览器
        /// </summary>
        /// <returns></returns>
        public bool Reboot()
        {
            return true;
        }

        /// <summary>
        /// 浏览器是否存在
        /// </summary>
        /// <returns></returns>
        public bool Exist()
        {
            return true;
        }

        /// <summary>
        /// 启动浏览器
        /// </summary>
        /// <returns></returns>
        public bool Boot()
        {
            InternetExplorerOptions y = new InternetExplorerOptions()
            {
                IntroduceInstabilityByIgnoringProtectedModeSettings = true,
                IgnoreZoomLevel = true,
            };
            _driver = new InternetExplorerDriver(y);
            return true;
        }

        /// <summary>
        /// 求IE有效地WEB区域
        /// </summary>
        /// <returns></returns>
        public Rectangle GetClientWndRec()
        {
            return null;
        }

        /// <summary>
        /// 取窗体标题
        /// </summary>
        /// <returns></returns>
        public string GetTitle()
        {
            return null;
        }

        /// <summary>
        /// 退出IE进程
        /// </summary>
        /// <returns></returns>
        public bool Exit()
        {
            return true;
        }

        /// <summary>
        /// 求页面源代码
        /// </summary>
        public string PageSource { get; set; }

        /// <summary>
        /// 植入浏览器操作，script需要return
        /// </summary>
        /// <param name="script"></param>
        /// <param name="CheckCommonLoaded"></param>
        /// <returns></returns>
        public string EvalJs(string script, bool CheckCommonLoaded = true)
        {
            return null;
        }

        /// <summary>
        /// 执行JavaScript脚本
        /// </summary>
        /// <param name="script"></param>
        public void RawEvalJs(string script)
        { }

        /// <summary>
        /// 设置浏览器Url
        /// </summary>
        public string Url
        {
            set
            {
                _driver.Url = "http://" + value;
            }
        }

        /// <summary>
        /// 等待浏览器加载完成
        /// </summary>
        /// <param name="maxWaitTime"></param>
        /// <returns></returns>
        public bool Wait4Load(int maxWaitTime)
        {
            return true;
        }

        /// <summary>
        /// 一般按键
        /// </summary>
        /// <param name="by"></param>
        /// <returns></returns>
        public IButton Button(string by)
        {
            return null;
        }

        /// <summary>
        /// 单选按钮
        /// </summary>
        /// <param name="by"></param>
        /// <returns></returns>
        public IRadioButton RadioButton(string by)
        {
            return null;
        }

        /// <summary>
        /// 文字编辑框
        /// </summary>
        /// <param name="by"></param>
        /// <returns></returns>
        public ITextBox TextBox(string by)
        {
            return null;
        }

        /// <summary>
        /// 复选框
        /// </summary>
        /// <param name="by"></param>
        /// <returns></returns>
        public IComboBox ComboBox(string by)
        {
            return null;
        }
    }
}
