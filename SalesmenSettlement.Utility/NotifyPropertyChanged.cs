﻿using System;
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

        public virtual void OnPropertyChanged(string propertyName)
        {
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

        public static T CreateProxy<T>() where T : NotifyPropertyChanged, new()
        {
            T t = new T();
            return NotifyPropertyChangedProxy<T>.CreateInstance(t);
        }

        public static T Impletement<T>(ref T obj) where T: NotifyPropertyChanged
        {
            obj = NotifyPropertyChangedProxy<T>.CreateInstance(obj);
            return obj;
        }
    }
}
