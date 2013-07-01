using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace DsAuto.Utility
{
    public class ModBase
    {
        public static DelegatedReflectionMemberAccessor access = new DelegatedReflectionMemberAccessor();

        public ModBase()
        {
            this.Empty();
        }

        private void Empty()
        {
            //Todo:
            PropertyInfo[] proInfos = this.GetType().GetProperties();
            foreach (PropertyInfo proTemp in proInfos)
            {
                if (proTemp.PropertyType.Name.Equals( "String"))
                {
                    access.SetValue(this, proTemp.Name, DsStr.NULL);
                }
            }
        }
    }
}
