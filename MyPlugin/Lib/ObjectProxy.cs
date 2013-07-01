using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using MyPlugin.Mode;

namespace MyPlugin.Lib
{
    /*
     * ObjectProxy:
     *     it is used as proxy between IPlugin class and MarshalByRefObject which can
     *     be transfer from one domain to other domain.
     */
    public class ObjectProxy : MarshalByRefObject
    {
        private Assembly assembly = null;
        private string fullClassName = null;
        private Dictionary<string, object> objList = new Dictionary<string, object>();
        private IPlugin iPlugin; 

        /// <summary>
        /// Load IPlugin's assembly
        /// </summary>
        /// <param name="path"></param>
        public void LoadAssembly(string path)
        {
            assembly = Assembly.LoadFile(path);
            foreach (var item in assembly.GetTypes())
            {
                if (item.GetInterface("IPlugin")!= null && item.IsClass)
                {
                    fullClassName = item.FullName;
                    iPlugin = Obj(item) as IPlugin;
                    break;
                }
            }
        }

        /// <summary>
        /// get current Plugin
        /// </summary>
        public IPlugin Plugin
        {
            get { return this.iPlugin; }
        }

        /// <summary>
        /// construct IPlugin object
        /// </summary>
        /// <param name="tp"></param>
        /// <returns></returns>
        public object Obj(Type tp)
        {
            object obj = null;
            if (assembly == null)
                return false;

            if (tp == null)
                return false;

            if (!objList.ContainsKey(tp.FullName))
            {
                obj = Activator.CreateInstance(tp);
                objList.Add(tp.FullName, obj);
            }

            return objList[tp.FullName];
        }

        /// <summary>
        /// get property from IPlugin instance
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public object P(string propertyName)
        {
            if (assembly == null)
                return false;

            if (propertyName == null)
                return false;

            Type tp = assembly.GetType(fullClassName);
            PropertyInfo proInfo = tp.GetProperty(propertyName);
            if (proInfo == null)
                return false;

            return proInfo.GetValue(Obj(tp), null);
        }

        /// <summary>
        /// set property of IPlugin instance
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool P(string propertyName, object value)
        {
            if (assembly == null)
                return false;

            Type tp = assembly.GetType(fullClassName);
            if (tp == null)
                return false;

            PropertyInfo property = tp.GetProperty(propertyName);
            property.SetValue(Obj(tp), value, null);
            return true;

        }

        /// <summary>
        /// invoke a method in IPlugin instance
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public bool invoke(string methodName, params object[] para)
        {
            if (assembly == null)
                return false;

            Type tp = assembly.GetType(fullClassName);
            //貌似这个地方的判断是不需要的.
            if (tp == null)
                return false;

            MethodInfo memInfo = tp.GetMethod(methodName);
            if (memInfo == null)
                return false;

            memInfo.Invoke(Obj(tp), para);
            return true;
        }
    }
}
