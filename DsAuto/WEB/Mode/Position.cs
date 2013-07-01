using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DsAuto.WEB.Mode
{
    /// <summary>
    /// WEB UI元素的矩形块大小
    /// </summary>
    public class Rectangle
    {
        /// <summary>
        /// x坐标(px)
        /// </summary>
        int x;
        /// <summary>
        /// y坐标(px)
        /// </summary>
        int y;
        /// <summary>
        /// 水平长度(px)
        /// </summary>
        int length;
        /// <summary>
        /// 垂直高度(px)
        /// </summary>
        int height;

        /// <summary>
        /// 取x坐标
        /// </summary>
        public int X
        {
            get { return x; }
            set { x = value; }
        }

        /// <summary>
        /// 取y坐标
        /// </summary>
        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        /// <summary>
        /// 取矩形方阵的长度
        /// </summary>
        public int Length
        {
            get { return length; }
            set { length = value; }
        }

        /// <summary>
        /// 取矩形方阵的高度
        /// </summary>
        public int Height
        {
            get { return height; }
            set { height = value; }
        }
    }
}
