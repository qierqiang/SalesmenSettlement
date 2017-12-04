using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SalesmenSettlement
{
    public static class CodeHelper
    {
        public static bool IsNullOrBlank(this string source)
        {
            return (source == null) || source.Trim().Length == 0;
        }
    }
}
