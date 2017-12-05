using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Proxies;
using System.Text;

namespace SalesmenSettlement.Utility
{
    public class ModelBase : NotifyPropertyChanged
    {
        protected bool _modified = false;

        protected ModelBase() { }

        public bool GetIsModified() => _modified;

        public override void OnPropertyChanged(string propertyName)
        {
            _modified = true;
            base.OnPropertyChanged(propertyName);
        }

        //static
        public static T CreateInstance<T>() where T : ModelBase
        {
            return NotifyPropertyChangedProxy<T>.CreateInstance(Activator.CreateInstance<T>());
        }

        public static T CreateInstanceFromDataRow<T>(DataRow row) where T : ModelBase
        {
            T ins = Activator.CreateInstance<T>();
            InitPropertyFromDataRow(ins, row);
            ins._modified = false;
            ins = NotifyPropertyChangedProxy<T>.CreateInstance(ins);
            return ins;
        }

        public static void InitPropertyFromDataRow<T>(T target, DataRow row)
        {
            for (int i = 0; i < row.Table.Columns.Count; i++)
            {
                string colName = row.Table.Columns[i].Caption;
                object value = row[colName];
                SetPropertyValue(target, colName, value);
            }
        }

        private static bool SetPropertyValue(object target, string propertyName, object value)
        {
            if (value.IsNullOrDbNull())
            {
                return true;
            }

            Type type = target is RealProxy rp ? rp.GetProxiedType() : target.GetType();
            var p = type.GetProperty(propertyName);

            if (p != null)
            {
                Console.WriteLine(value.GetType().Name);
                if (p.PropertyType == typeof(string))
                {
                    p.SetValue(target, value.ToString(), new object[] { });
                }
                else
                {
                    p.SetValue(target, value, new object[] { });
                }
                return true;
            }
            return false;
        }
    }
}
