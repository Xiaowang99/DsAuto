using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DsAuto.WEB.Mode;

namespace DsAuto.WEB.UIMode
{
    /// <summary>
    /// 每一个WEB元素所必须继承的接口
    /// </summary>
    public interface IWEBUIObj
    {
        /// <summary>
        /// 元素上的文字
        /// </summary>
        string Text { get; }

        /// <summary>
        /// 返回元素的矩阵信息
        /// </summary>
        Rectangle Rect { get; }

        /// <summary>
        /// 获取元素的式样
        /// </summary>
        string Style { get; }

        /// <summary>
        /// 元素是否存在
        /// </summary>
        /// <returns></returns>
        bool IsExist();

        /// <summary>
        /// 返回该元素的子元素
        /// </summary>
        IWEBUIObj[] Children { get; }

        /// <summary>
        /// 带深度的子元素数组
        /// </summary>
        /// <param name="depth"></param>
        /// <returns></returns>
        IWEBUIObj[] GetChildren(int depth);

        /// <summary>
        /// 带条件的筛选子元素组
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IWEBUIObj[] FilterChildren(string condition);

        /// <summary>
        /// 元素是否存在(否则抛异常)
        /// </summary>
        void AssertExist();

        /// <summary>
        /// 单击行为
        /// </summary>
        /// <returns></returns>
        bool Click();

        /// <summary>
        /// 双击行为
        /// </summary>
        /// <returns></returns>
        bool DbClick();

        /// <summary>
        /// 右击行为
        /// </summary>
        /// <returns></returns>
        bool RtClick();

        /// <summary>
        /// 鼠标在元素之上
        /// </summary>
        /// <returns></returns>
        bool MouseOn();

        /// <summary>
        /// 鼠标从元素上移开
        /// </summary>
        /// <returns></returns>
        bool MouseOff();

        /// <summary>
        /// 点击菜单元素(没有菜单跳出来则抛异常)
        /// </summary>
        /// <returns></returns>
        bool MemuClick();

        /// <summary>
        /// 元素焦点落在该元素上
        /// </summary>
        /// <returns></returns>
        bool Focus();

        /// <summary>
        /// 获得该名称的元素属性
        /// </summary>
        /// <param name="AttName"></param>
        /// <returns></returns>
        string GetAttribute(string AttName);
    }

    /// <summary>
    /// IButton 普通按钮的接口
    /// </summary>
    public interface IButton : IWEBUIObj
    {
        /// <summary>
        /// 设置获取按钮的状态
        /// </summary>
        int Select { get; set; }

        /// <summary>
        /// 按住按钮time(毫秒)
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        bool Push(int time);
    }

    /// <summary>
    /// 单选框按钮
    /// </summary>
    public interface IRadioButton : IWEBUIObj
    {
        /// <summary>
        /// 选中的元素
        /// </summary>
        int Select { get; set; }
    }

    /// <summary>
    /// 复选框按钮
    /// </summary>
    public interface ICheckButton : IWEBUIObj
    {
        /// <summary>
        /// 已选择的序号
        /// </summary>
        int Select { set; get; }
    }

    /// <summary>
    /// 菜单栏
    /// </summary>
    public interface IMenu
    {
        /// <summary>
        /// 菜单是否存在
        /// </summary>
        /// <returns></returns>
        bool IsExist();

        /// <summary>
        /// 退出菜单栏
        /// </summary>
        /// <returns></returns>
        bool Exit();

        /// <summary>
        /// 获得菜单子项个数
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        int GetItemCount(string path);

        /// <summary>
        /// 得到子项的文字
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        string GetItemText(string path = "");

        /// <summary>
        /// 选择菜单项
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        bool Click(string path = "");

        /// <summary>
        /// 是否被选择
        /// </summary>
        string Selected { get; set; }

        /// <summary>
        /// 显示菜单项
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        bool ShowSubItem(string path = "");
    }

    /// <summary>
    /// 复选框接口
    /// </summary>
    public interface IComboBox : IWEBUIObj
    {
        /// <summary>
        /// 
        /// </summary>
        int Selected { get; set; }

        /// <summary>
        /// 得到复选框的文本内容
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// 子项的个数s
        /// </summary>
        int count { get; }

        /// <summary>
        /// 得到子项的文本
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        string GetItemText(int index);

        /// <summary>
        /// 根据子项的名称找到索引位置
        /// </summary>
        /// <param name="itemText"></param>
        /// <returns></returns>
        int IndexOfItems(string itemText);
    }

    /// <summary>
        /// 链接元素接口
        /// </summary>
    public interface ILink : IWEBUIObj
        {

        }

    /// <summary>
    /// 文本编辑框
    /// </summary>
    public interface ITextBox : IWEBUIObj
        {
            /// <summary>
            /// 获得当前子项的文字
            /// </summary>
            string Text { get; set; }

            /// <summary>
            /// 输入框的末尾是否有错误提示符
            /// </summary>
            bool IsError { get; }

            /// <summary>
            /// 错误的文字内容
            /// </summary>
            string ErrorText { get; }
        }

    public interface IList : IWEBUIObj
    {
        /// <summary>
        /// 行数
        /// </summary>
        int RowCount { get; }

        /// <summary>
        /// 行数与RowCount一致
        /// </summary>
        int Count { get; }

        /// <summary>
        /// 列数
        /// </summary>
        int ColCount { get; }

        /// <summary>
        /// 列名
        /// </summary>
        string[] Header { get; }

        /// <summary>
        /// 设置\获取, 选择行，多行序号以逗号分割
        /// </summary>
        string Selected { get; set; }

        /// <summary>
        /// 取消选择行，其他行状态不变
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        bool UnSelect(int row);

        /// <summary>
        /// 选择所有行
        /// </summary>
        /// <returns></returns>
        bool SelectAll();

        /// <summary>
        /// 取消所有行
        /// </summary>
        /// <returns></returns>
        bool UnSelectAll();

        /// <summary>
        /// 选择多行
        /// </summary>
        /// <param name="rowList">行号，多行以逗号分割</param>
        /// <returns></returns>
        bool SelectRange(string rowList);

        /// <summary>
        /// 取消选择多行
        /// </summary>
        /// <param name="rowList"></param>
        /// <returns></returns>
        bool UnSelectRange(string rowList);

        /// <summary>
        /// 双击单元格
        /// </summary>
        /// <param name="cellPath"></param>
        /// <returns></returns>
        bool DbClick(params int[] cellPath);

        /// <summary>
        /// 单击
        /// </summary>
        /// <param name="cellPath"></param>
        /// <returns></returns>
        bool Click(params int[] cellPath);

        /// <summary>
        /// 右击
        /// </summary>
        /// <param name="cellPath"></param>
        /// <returns></returns>
        bool ReClick(params int[] cellPath);

        /// <summary>
        /// 总页数
        /// </summary>
        int PageCount { get; }

        /// <summary>
        /// 展开
        /// </summary>
        /// <param name="cellPath"></param>
        /// <returns></returns>
        bool Expand(params int[] cellPath);

        /// <summary>
        /// 获取目标元素
        /// </summary>
        /// <param name="cellPath">1,3代表1行3列</param>
        /// <returns></returns>
        IWEBUIObj GetCellItem(params int[] cellPath);
    }

    public interface ITree : IWEBUIObj
    {
        /// <summary>
        /// 得到菜单子项个数
        /// </summary>
        /// <param name="nodePath">1,2,3代表</param>
        /// <returns></returns>
        int GetItemCount(params int[] nodePath);

        /// <summary>
        /// 求指定路径节点的文字
        /// </summary>
        /// <param name="nodePath"></param>
        /// <returns></returns>
        string GetItemText(params int[] nodePath);

        /// <summary>
        /// 是否选取
        /// </summary>
        string Selected { get; set; }

        /// <summary>
        /// 单击展开
        /// </summary>
        /// <param name="nodePath"></param>
        /// <returns></returns>
        bool Click(params int[] nodePath);

        /// <summary>
        /// 展开与Click类似
        /// </summary>
        /// <param name="nodePath"></param>
        /// <returns></returns>
        bool Expand(params int[] nodePath);

        /// <summary>
        /// 全部展开
        /// </summary>
        /// <returns></returns>
        bool ExpandAll();

        /// <summary>
        /// 收缩
        /// </summary>
        /// <param name="nodePath"></param>
        /// <returns></returns>
        bool Collapse(params int[] nodePath);

        /// <summary>
        /// 全部收缩
        /// </summary>
        /// <returns></returns>
        bool CollapseAll();

        /// <summary>
        /// 双击
        /// </summary>
        /// <param name="nodePath"></param>
        /// <returns></returns>
        bool DbClick(params int[] nodePath);

        /// <summary>
        /// 右击
        /// </summary>
        /// <param name="nodePath"></param>
        /// <returns></returns>
        bool RtClick(params int[] nodePath);

        /// <summary>
        /// 取出一个单元格的元素
        /// </summary>
        /// <param name="cellPath"></param>
        /// <returns></returns>
        IWEBUIObj GetCellItem(params int[] cellPath);
    }

    public interface ITab : IWEBUIObj
    {
        /// <summary>
        /// 设置\获取 激活的选项卡序号
        /// </summary>
        int Selected { set; get; }

        int Count { get; }
    }


}
