using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Proxies;
using System.Text;

namespace SalesmenSettlement.Utility
{
    public class ViewModelBase : NotifyPropertyChanged
    {
        protected bool _modified = false;

        public bool GetIsModified() => _modified;

        public void SetUnModified() => _modified = false;

        public override void OnPropertyChanged(string propertyName)
        {
            _modified = true;
            base.OnPropertyChanged(propertyName);
        }
    }
}
