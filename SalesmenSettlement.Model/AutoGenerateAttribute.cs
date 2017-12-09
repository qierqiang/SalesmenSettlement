using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SalesmenSettlement.Model
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class AutoGenerateAttribute : Attribute { }
}
