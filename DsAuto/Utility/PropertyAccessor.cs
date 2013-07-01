using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DsAuto.Utility
{
    public interface IMemberAccessor
    {
        /// <summary>
        /// Get the member value of an object.
        /// </summary>
        /// <param name="instance">The object to get the member value from.</param>
        /// <param name="memberName">The member name, could be the name of a property of field. Must be public member.</param>
        /// <returns>The member value</returns>
        object GetValue(object instance, string memberName);

        /// <summary>
        /// Set the member value of an object.
        /// </summary>
        /// <param name="instance">The object to get the member value from.</param>
        /// <param name="memberName">The member name, could be the name of a property of field. Must be public member.</param>
        /// <param name="newValue">The new value of the property for the object instance.</param>
        void SetValue(object instance, string memberName, object newValue);
    }

    public interface INamedMemberAccessor  
    {  
        object GetValue(object instance);  
        void SetValue(object instance, object newValue);  
    }


    class PropertyAccessor<T, P> : INamedMemberAccessor
    {
        private Func<T, P> GetValueDelegate;
        private Action<T, P> SetValueDelegate;

        public PropertyAccessor(Type type, string propertyName)
        {
            var propertyInfo = type.GetProperty(propertyName);
            if (propertyInfo != null)
            {
                GetValueDelegate = (Func<T, P>)Delegate.CreateDelegate(typeof(Func<T, P>), propertyInfo.GetGetMethod());
                SetValueDelegate = (Action<T, P>)Delegate.CreateDelegate(typeof(Action<T, P>), propertyInfo.GetSetMethod());
            }
        }

        public object GetValue(object instance)
        {
            return GetValueDelegate((T)instance);
        }

        public void SetValue(object instance, object newValue)
        {
            SetValueDelegate((T)instance, (P)newValue);
        }
    }


    public class DelegatedReflectionMemberAccessor : IMemberAccessor
    {
        private static Dictionary<string, INamedMemberAccessor> accessorCache = new Dictionary<string, INamedMemberAccessor>();

        public object GetValue(object instance, string memberName)
        {
            return FindAccessor(instance, memberName).GetValue(instance);
        }

        public void SetValue(object instance, string memberName, object newValue)
        {
            FindAccessor(instance, memberName).SetValue(instance, newValue);
        }

        private INamedMemberAccessor FindAccessor(object instance, string memberName)
        {
            var type = instance.GetType();
            var key = type.FullName + memberName;
            INamedMemberAccessor accessor;
            accessorCache.TryGetValue(key, out accessor);
            if (accessor == null)
            {
                var propertyInfo = type.GetProperty(memberName);
                accessor = Activator.CreateInstance(typeof(PropertyAccessor<,>).MakeGenericType(type, propertyInfo.PropertyType), type, memberName) as INamedMemberAccessor;
                accessorCache.Add(key, accessor);
            }

            return accessor;
        }
    }
}
