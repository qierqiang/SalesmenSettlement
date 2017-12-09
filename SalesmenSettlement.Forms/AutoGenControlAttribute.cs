using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SalesmenSettlement.Forms
{

    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class AutoGenControlAttribute : Attribute
    {
        public Type EditorType { get; private set; }

        public string DisplayName { get; private set; }

        public bool ReadOnly { get; private set; }

        public bool Enabled { get; private set; }

        public bool BeginNewRow { get; private set; }

        public string GroupName { get; set; }

        public AutoGenControlAttribute(Type editorType, string displayName = null, bool readOnly = false, bool enabled = true, bool beginNewRow = false, string groupName = null)
        {
            EditorType = editorType;
            DisplayName = displayName;
            ReadOnly = readOnly;
            Enabled = enabled;
            BeginNewRow = beginNewRow;
            GroupName = groupName;
        }
    }
}
