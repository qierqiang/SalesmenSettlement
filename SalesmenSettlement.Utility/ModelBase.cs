using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SalesmenSettlement.Utility
{
    public class ModelBase : NotifyPropertyChanged
    {
        protected bool _modified = false;

        protected ModelBase() { }

        public bool GetIsModified() => _modified;

        public static T CreateInstance<T>() where T : ModelBase
        {
            return NotifyPropertyChangedProxy<T>.CreateInstance(Activator.CreateInstance<T>());
        }

        public static T CreateInstanceFromDataRow<T>(DataRow row) where T : ModelBase
        {
            T ins = CreateInstance<T>();
            InitPropertyFromDataRow(ins, row);
            ins._modified = false;
            return ins;
        }

        public static void InitPropertyFromDataRow<T>(T target, DataRow row)
        {

        }
    }
}
