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
        [DontSave]
        public long ID { get; set; }

        protected bool _modified = false;

        public bool GetIsModified() => _modified;

        public void SetIsModified(bool value) => _modified = value;

        public override void OnPropertyChanged(string propertyName)
        {
            _modified = true;
            base.OnPropertyChanged(propertyName);
        }
    }
}
