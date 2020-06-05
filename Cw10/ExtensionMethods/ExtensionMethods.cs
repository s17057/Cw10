using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cw10.Models;
using System.Reflection;

namespace Cw10.ExtensionMethods
{
    public static class ExtensionMethods
    {
        public static void SetProperty(this StudentApbd stud, String propName, Object val)
        {
            if (val != null)
            {
                Type type = stud.GetType();
                PropertyInfo prop = type.GetProperty(propName);
                prop.SetValue(stud, val, null);
            }
        }
    }
}
