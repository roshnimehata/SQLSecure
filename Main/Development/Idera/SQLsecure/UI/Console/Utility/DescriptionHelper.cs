using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace Idera.SQLsecure.UI.Console.Utility
{
    class DescriptionHelper
    {
        internal static string GetDescription(Type type, string name)
        {
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    object[] attributes = field.GetCustomAttributes(typeof (DescriptionAttribute), true);
                    if (attributes.Length > 0)
                        return ((DescriptionAttribute) attributes[0]).Description;
                }
            }
            return name;
        }

        internal static string GetPlural(string name)
        {
            string plural = name;

            if (plural.EndsWith("y"))
            {
                plural = plural.Substring(0, plural.Length - 1) + @"ies";
            }
            else
            {
                plural += @"s";
            }

            return plural;
        }

        #region Enum Helper

        internal static string GetEnumDescription(object o)
        {
            System.Type otype = o.GetType();
            if (otype.IsEnum)
            {
                FieldInfo field = otype.GetField(Enum.GetName(otype, o));
                if (field != null)
                {
                    object[] attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), true);
                    if (attributes.Length > 0)
                        return ((DescriptionAttribute)attributes[0]).Description;
                }
            }
            return o.ToString();
        }

        #endregion
    }
}
