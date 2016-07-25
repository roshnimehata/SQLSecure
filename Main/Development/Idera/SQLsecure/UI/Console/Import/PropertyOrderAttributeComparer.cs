using System.Collections.Generic;
using System.Reflection;

namespace Idera.SQLsecure.UI.Console.Import
{
    public class PropertyOrderAttributeComparer : IComparer<PropertyInfo>
    {
        public int Compare(PropertyInfo x, PropertyInfo y)
        {
            var xOrderAttribute = x.GetCustomAttributes(typeof(PropertyOrderAttribute), false);
            var yOrderAttribute = y.GetCustomAttributes(typeof(PropertyOrderAttribute), false);
            int xOrder = 0;
            int yOrder = 0;

            if (xOrderAttribute.Length > 0) xOrder = ((PropertyOrderAttribute)xOrderAttribute[0]).Order;
            if (yOrderAttribute.Length > 0) yOrder = ((PropertyOrderAttribute)yOrderAttribute[0]).Order;

            return xOrder.CompareTo(yOrder);
        }
    }
}