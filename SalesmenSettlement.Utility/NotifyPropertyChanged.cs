using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SalesmenSettlement.Utility
{
    public class NotifyPropertyChanged : MarshalByRefObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null && !propertyName.IsNullOrWhiteSpace())
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void OnPropertyChanged<T>(Expression<Func<T>> expression)
        {
            if (expression.Body is MemberExpression memberExpress)
            {
                OnPropertyChanged(memberExpress.Member.Name);
            }
        }

        public static T CreateProxy<T>() where T : NotifyPropertyChanged, new()
        {
            T t = new T();
            return NotifyPropertyChangedProxy<T>.CreateInstance(t);
        }
    }
}
