using System;
using System.Collections.Generic;
using System.Text;

namespace Idera.SQLsecure.UI.Console.Import
{
    public class PropertyOrderAttribute:Attribute
    {
        public PropertyOrderAttribute(int order)
        {
            Order = order;
        }

        public int Order { get; set; }
    }
}
