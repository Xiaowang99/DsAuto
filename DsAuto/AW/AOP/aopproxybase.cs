using System;
using System.Runtime.Remoting ;
using System.Runtime.Remoting.Proxies ;
using System.Runtime.Remoting.Messaging ;
using System.Runtime.Remoting.Services ;
using System.Runtime.Remoting.Activation ;

namespace DsAuto.AW.AOP
{
    /// <summary>
    /// AopProxyBase所有自定义AOP代理类都从此类派生，覆写IAopOperator接口，实现具体的前/后处理。
    /// 2005.04.12
    /// </summary>
    public class AopProxyBase : RealProxy, IAopOperator
    {
        private readonly MarshalByRefObject target; //默认透明代理

        public AopProxyBase(MarshalByRefObject obj, Type type)
            : base(type)
        {
            this.target = obj;
        }

        #region Invoke
        public override IMessage Invoke(IMessage msg)
        {
            bool useAspect = false;
            IMethodCallMessage call = (IMethodCallMessage)msg;

            //查询目标方法是否使用了启用AOP的MethodAopSwitcherAttribute
            foreach (Attribute attr in call.MethodBase.GetCustomAttributes(false))
            {
                AuthorInfoAttribute methodAopAttr = attr as AuthorInfoAttribute;
                if (methodAopAttr != null)
                {
                    if (methodAopAttr.Action.Equals("创建") || methodAopAttr.Action.Equals("修改"))
                    {
                        useAspect = true;
                        break;
                    }
                    else
                    {
                        throw new Exception(string.Format("Invalid AuthorInfo: {0},{1}", methodAopAttr.Action, methodAopAttr.Author));
                    }
                }
            }

            if (useAspect)
            {
                this.PreProcess(msg);
            }

            //如果触发的是构造函数，此时target的构建还未开始
            IConstructionCallMessage ctor = call as IConstructionCallMessage;
            if (ctor != null)
            {
                //获取最底层的默认真实代理
                RealProxy default_proxy = RemotingServices.GetRealProxy(this.target);

                default_proxy.InitializeServerObject(ctor);
                MarshalByRefObject tp = (MarshalByRefObject)this.GetTransparentProxy(); //自定义的透明代理 this

                return EnterpriseServicesHelper.CreateConstructionReturnMessage(ctor, tp);
            }

            IMethodReturnMessage result_msg = RemotingServices.ExecuteMessage(this.target, call); //将消息转化为堆栈，并执行目标方法，方法完成后，再将堆栈转化为消息

            if (useAspect)
            {
                this.PostProcess(msg, result_msg);
            }

            return result_msg;

        }
        #endregion

        #region IAopOperator成员

        public virtual void PreProcess(IMessage requestMsg) { }
        public virtual void PostProcess(IMessage requestMsg, IMessage Respond) { }
        #endregion
    }
}