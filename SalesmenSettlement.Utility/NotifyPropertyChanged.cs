using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace SalesmenSettlement.Utility
{
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged<T>(Expression<Func<T>> expression)
        {
            if (expression.Body is MemberExpression memberExpress && PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(memberExpress.Member.Name));
            }
        }

        public virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}