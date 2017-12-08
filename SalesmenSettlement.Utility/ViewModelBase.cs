using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Remoting.Proxies;
using System.Text;

namespace SalesmenSettlement.Utility
{
    public class ViewModelBase :MarshalByRefObject, INotifyPropertyChanged
    {
        protected bool _modified = false;

        public bool GetIsModified() => _modified;

        public void SetUnModified() => _modified = false;
        
        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            _modified = true;

            if (PropertyChanged != null && !propertyName.IsNullOrWhiteSpace())
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public virtual void OnPropertyChanged<T>(Expression<Func<T>> expression)
        {
            if (expression.Body is MemberExpression memberExpress)
            {
                OnPropertyChanged(memberExpress.Member.Name);
            }
        }
    }
}
