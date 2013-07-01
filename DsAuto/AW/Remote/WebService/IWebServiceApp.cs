using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Net;
using System.IO;
using System.Web;
using System.Web.Services.Description;
using System.Xml.Serialization;
using System.CodeDom;
using System.CodeDom.Compiler;

namespace DsAuto.AW.Remote.WebService
{
    
    interface IWebServiceApp
    {
        //IWebServiceApp用来规范动态引用webService的实例化方式
        
    }

    public class WebServiceApp : IWebServiceApp
    {
        /// <summary>
        /// 动态引用的webService程序集
        /// </summary>
        private Assembly asm = null;

        private object wsProxy = null;

        /// <summary>
        /// 动态webService的命名空间
        /// </summary>
        private string NameSpace
        {
            get { return "DsWSNamepace"; }
        }

        public WebServiceApp(string url, string proxyName)
        {
            //1.在这个地方动态实例化web代理类和代理空间
            WebClient webClient = new WebClient();

            //2.打开webService下载通道
            Stream stream = webClient.OpenRead(url);

            //3.创建和格式化 WSDL 文档
            ServiceDescription description = ServiceDescription.Read(stream);

            //4.创建客户端代理代理类。
            ServiceDescriptionImporter importer = new ServiceDescriptionImporter();
            importer.ProtocolName = "Soap"; // 指定访问协议。
            importer.Style = ServiceDescriptionImportStyle.Client; // 生成客户端代理
            importer.CodeGenerationOptions = CodeGenerationOptions.GenerateProperties | CodeGenerationOptions.GenerateNewAsync;
            importer.AddServiceDescription(description, null, null); // 添加 WSDL 文档

            //5.使用 CodeDom 编译客户端代理类
            CodeNamespace nmspace = new CodeNamespace(NameSpace); // 为代理类添加命名空间，缺省为全局空间。
            CodeCompileUnit unit = new CodeCompileUnit();
            unit.Namespaces.Add(nmspace);
            ServiceDescriptionImportWarnings warning = importer.Import(nmspace, unit);
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CompilerParameters parameter = new CompilerParameters();
            parameter.GenerateExecutable = false;
            parameter.GenerateInMemory = true;
            parameter.ReferencedAssemblies.Add("System.dll");
            parameter.ReferencedAssemblies.Add("System.XML.dll");
            parameter.ReferencedAssemblies.Add("System.Web.Services.dll");
            parameter.ReferencedAssemblies.Add("System.Data.dll");
            CompilerResults result = provider.CompileAssemblyFromDom(parameter, unit);

            //6.使用 Reflection 调用 WebService。
            if (!result.Errors.HasErrors)
            {
                this.asm = result.CompiledAssembly;
                Type t = this.asm.GetType("DsWSNamepace." + proxyName ); // 如果在前面为代理类添加了命名空间，此处需要将命名空间添加到类型前面。
                this.wsProxy = Activator.CreateInstance(t);
                #region 调试代码
                //Type t = this.asm.GetType("WebService"); // 如果在前面为代理类添加了命名空间，此处需要将命名空间添加到类型前面。
                //object o = Activator.CreateInstance(t);
                //MethodInfo method = t.GetMethod("HelloWorld");
                //Console.WriteLine(method.Invoke(o, null));
                #endregion
            }
            else
            {
                throw new Exception("CompiledAssembly： 动态编译WebService出现了异常!");
            }
        }

        public object[] InvokeMethod( string methodName, object allArgs)
        {
            object[] inputArgs = new object[] { };

            //调用webService的实例方法
            MethodInfo mi = this.wsProxy.GetType().GetMethod(methodName);

            var param = mi.GetParameters();
            //out 要从返回值中返回
            int iPmCount = (from o in param where o.IsOut == false select o).Count();

            object[] Args = new object[param.Length];

            //如果只有一个输入参数
            if (iPmCount == 0)
            {
                inputArgs = new object[] { };
            }
            else if (iPmCount == 1)
            {
                if (!(allArgs is object[]))
                {
                    inputArgs = new object[] { allArgs };
                }
                else
                {
                    inputArgs = allArgs as object[];
                }
            }
            else
            {
                if (!(allArgs is object[]) || ((object[])allArgs).Length != iPmCount)
                {
                    throw new Exception("非out的参数有：" + iPmCount.ToString() + "个.");
                }

                inputArgs = allArgs as object[];
            }

            object[] Result = new object[param.Length - iPmCount + 1];

            //生成传递的参数
            for (int i = 0, j = 0; i < param.Length; i++)
            {
                var PM = param[i];
                if (PM.IsOut)
                {
                    Args[i] = Result[j + 1] = null;
                    j++;
                }
                else
                {
                    Type type = PM.ParameterType;
                    if (type.BaseType.Name.Contains("Enum"))
                    {
                        if (inputArgs[i - 1].ToString() == "")
                        {
                            Args[i] = null;
                        }
                        else
                        {
                            Args[i] = Enum.Parse(PM.ParameterType, inputArgs[i - j].ToString());
                        }
                    }
                    else
                    {
                        Args[i] = inputArgs[i - j];
                        Convert.ChangeType(Args[i], type);
                    }
                }
            }
            Result[0] = mi.Invoke(this.wsProxy, Args);
            //生成传递的参数
            for (int i = 0, j = 0; i < param.Length; i++)
            {
                var PM = param[i];
                if (PM.IsOut)
                {
                    Result[j + 1] = Args[i];
                    j++;
                }
            }

            return Result;
        }

        /// <summary>
        /// 实例化webService对象
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public object Obj(string typeName)
        {
            Type t = asm.GetType(NameSpace + "." + typeName);
             return Activator.CreateInstance(t);
        }

        /// <summary>
        /// 实例化webService对象数组
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public object[] ObjAry(string typeName, int length)
        {
            Type t = asm.GetType(NameSpace + "." + typeName);
            Array objAry = Array.CreateInstance(t, length);
            return (object[])objAry;
        }

        public static void setValue(object obj, string propertyName, object value)
        {
            Type t = obj.GetType();
            PropertyInfo propertyInfo = t.GetProperty(propertyName);
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(obj, value, null);
            }
            else
            {
                throw new Exception(string.Format("setValue Failed: 未在该类中找到名为{0}的属性进行赋值", propertyName));
            }
            
        }

        public static object getValue(object obj, string propertyName)
        {
            Type t = obj.GetType();
            PropertyInfo propertyInfo = t.GetProperty(propertyName);
            if (propertyInfo != null)
            {
                return propertyInfo.GetValue(obj,null);
            }
            else
            {
                throw new Exception(string.Format("setValue Failed: 未在该类中找到名为{0}的属性进行赋值", propertyName));
            }

        }
    }
}
