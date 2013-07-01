using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DsAuto.WEB.Mode;

namespace DsAuto.WEB.UIMode
{
    /// <summary>
    /// 浏览器对象
    /// </summary>
    public interface IRobot
    {
        /// <summary>
        /// 选中元素的高亮处理
        /// </summary>
        string HLColor { get; set; }

        /// <summary>
        /// 回退
        /// </summary>
        /// <returns></returns>
        bool Back();

        /// <summary>
        /// 向前
        /// </summary>
        /// <returns></returns>
        bool Forward();

        /// <summary>
        /// 窗口最大化
        /// </summary>
        /// <returns></returns>
        bool Max();

        /// <summary>
        /// 刷新
        /// </summary>
        /// <returns></returns>
        bool Refresh();

        /// <summary>
        /// 重启浏览器
        /// </summary>
        /// <returns></returns>
        bool Reboot();

        /// <summary>
        /// 浏览器是否存在
        /// </summary>
        /// <returns></returns>
        bool Exist();

        /// <summary>
        /// 启动浏览器
        /// </summary>
        /// <returns></returns>
        bool Boot();

        /// <summary>
        /// 求IE有效地WEB区域
        /// </summary>
        /// <returns></returns>
        Rectangle GetClientWndRec();

        /// <summary>
        /// 取窗体标题
        /// </summary>
        /// <returns></returns>
        string GetTitle();

        /// <summary>
        /// 退出IE进程
        /// </summary>
        /// <returns></returns>
        bool Exit();

        /// <summary>
        /// 求页面源代码
        /// </summary>
        string PageSource { get; }

        /// <summary>
        /// 植入浏览器操作，script需要return
        /// </summary>
        /// <param name="script"></param>
        /// <param name="CheckCommonLoaded"></param>
        /// <returns></returns>
        string EvalJs(string script, bool CheckCommonLoaded = true);

        /// <summary>
        /// 执行JavaScript脚本
        /// </summary>
        /// <param name="script"></param>
        void RawEvalJs(string script);

        /// <summary>
        /// 设置浏览器Url
        /// </summary>
        string Url { set; }

        /// <summary>
        /// 等待浏览器加载完成
        /// </summary>
        /// <param name="maxWaitTime"></param>
        /// <returns></returns>
        bool Wait4Load(int maxWaitTime);

        /// <summary>
        /// 一般按键
        /// </summary>
        /// <param name="by"></param>
        /// <returns></returns>
        IButton Button(string by);

        /// <summary>
        /// 单选按钮
        /// </summary>
        /// <param name="by"></param>
        /// <returns></returns>
        IRadioButton RadioButton(string by);

        /// <summary>
        /// 文字编辑框
        /// </summary>
        /// <param name="by"></param>
        /// <returns></returns>
        ITextBox TextBox(string by);

        /// <summary>
        /// 复选框
        /// </summary>
        /// <param name="by"></param>
        /// <returns></returns>
        IComboBox ComboBox(string by);
        
    }
}
