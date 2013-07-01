using System;

namespace DsAuto.AW.AOP
{
　　/// <summary>
　　/// MethodAopSwitcherAttribute用于决定一个被AopProxyAttribute修饰的class的某个特定方法是否启用截获。
　　/// 创建原因：绝大多数时候我们只希望对某个类的一部分Method而不是所有Method使用截获。
　　/// 使用方法：如果一个方法没有使用MethodAopSwitcherAttribute特性或使用MethodAopSwitcherAttribute(false)修饰，
　　///　　　都不会对其进行截获。只对使用了MethodAopSwitcherAttribute(true)启用截获。
　  /// 2005.05.11
　　/// </summary>
　　[AttributeUsage(AttributeTargets.Method ,AllowMultiple = true )]
　　public class AuthorInfoAttribute : Attribute
　　{
        private string _Action;

        private string _Author;

        private string _Time;

        private string _des;

        public AuthorInfoAttribute(string Action, string Author, string Time)
　　    {
            this._Action = Action;
            this._Author = Author;
            this._Time = Time;
　　    }

        public AuthorInfoAttribute(string Action, string Author, string Time, string Des)
        {
            this._Action = Action;
            this._Author = Author;
            this._Time = Time;
            this._des = Des;
        }

        //public AuthorInfoAttribute()
        //{ }

        public string Action {
            get { return this._Action; }
            set { this._Action = value; }
        }

        public string Author {
            get { return this._Author; }
            set { this._Author = value; }
        }

        public string Time {
            get { return this._Time; }
            set { this._Time = value; }
        }

        public string Des
        {
            get { return this._des; }
            set { this._des = value; }
        }
　　}
}
