using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DsAuto.AW.AOP;

namespace DsAuto.AW.Aw
{
    /// <summary>
    /// 所有的Aw操作类都从AwBase继承，这样子就可以使用Aop
    /// </summary>
    public class AwBase : ContextBoundObject
    {
        public AwBase() { }

        static object _AwBase;
        public static AwBase GetAwInstance(Type p)
        {
            if (_AwBase == null)
                _AwBase = Activator.CreateInstance(p);

            AopProxyBase _AopProxy = new AopProxyBase((MarshalByRefObject)_AwBase, p);
            return (AwBase)_AopProxy.GetTransparentProxy();
        }

        [AuthorInfo("创建","196360","2013-2-5")]
        public void method()
        { 
        }
    }
}
