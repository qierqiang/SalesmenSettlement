using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Text;

namespace SalesmenSettlement.Utility
{
    public class NotifyPropertyChangedProxy<T> : RealProxy where T : MarshalByRefObject, INotifyPropertyChanged
    {
        private T _target;

        public NotifyPropertyChangedProxy(T target) : base(typeof(T))
        {
            _target = target;
        }

        public override IMessage Invoke(IMessage msg)
        {
            IMethodCallMessage callMessage = (IMethodCallMessage)msg;
            object returnValue = callMessage.MethodBase.Invoke(this._target, callMessage.Args);
            InvokeOnPropertyChanged(callMessage);
            return new ReturnMessage(returnValue, new object[0], 0, null, callMessage);
        }

        private void InvokeOnPropertyChanged(IMethodCallMessage msg)
        {
            if (msg.MethodName.StartsWith("set_") && msg.Args.Length == 1)
            {
                string propertyName = msg.MethodName.Substring(4);
                MethodInfo method = typeof(T).GetMethod("OnPropertyChanged", new Type[] { typeof(string) });

                if (method != null)
                {
                    method.Invoke(_target, new object[] { propertyName });
                }
            }
        }

        public static T CreateInstance()
        {
            T instance = Activator.CreateInstance<T>();
            return (T)new NotifyPropertyChangedProxy<T>(instance).GetTransparentProxy();
        }

        public static T CreateInstance(T refObject)
        {
            return (T)new NotifyPropertyChangedProxy<T>(refObject).GetTransparentProxy();
        }
    }
}