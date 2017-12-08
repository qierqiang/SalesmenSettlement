using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SalesmenSettlement.Forms
{
    public class EditControlTypeAttribute : Attribute
    {
        public Type Type { get; private set; }

        public EditControlTypeAttribute(Type type)
        {
            Type = type;
        }

        public static string GetEditControlName<T>(Expression<Func<T>> property)
        {
            if (property.Body is MemberExpression expression)
            {
                return GetEditControlName(typeof(T), expression.Member.Name);
            }

            throw new ArgumentException("参数表示的不是属性！", "property");
        }

        public static string GetEditControlName(Type vmType, string propertyName)
        {
            var query = from p in vmType.GetProperties()
                        from a in p.GetCustomAttributes(typeof(EditControlTypeAttribute), true)
                        where p.Name == propertyName
                        select ((EditControlTypeAttribute)a).Type.Name;

            var attr = query.FirstOrDefault();

            return attr == null ? null : attr.Substring(0, 1).ToLower() + attr.Substring(1) + "_" + propertyName;
        }
    }
}
