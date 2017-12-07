using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SalesmenSettlement.Model
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
    public class ValidateAttribute : Attribute
    {
        //public bool Nullable { get; private set; }

        public int Length { get; private set; }

        public int Precision { get; private set; }

        public ValidateAttribute(/*bool nullable = true, */int length = -1, int precision = 0)
        {
            //Nullable = nullable;
            Length = length;
            Precision = precision;
        }
    }
}
