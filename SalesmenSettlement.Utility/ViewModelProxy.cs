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
    public class ViewModelProxy : RealProxy
    {
        private INotifyPropertyChanged _target;

        public ViewModelProxy(INotifyPropertyChanged target) : base(target.GetType())
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
                MethodInfo method = _target.GetType().GetMethod("OnPropertyChanged", new Type[] { typeof(string) });

                if (method != null)
                {
                    method.Invoke(_target, new object[] { propertyName });
                }
            }
        }

        //static
        public static T Proxy<T>(T target) where T : INotifyPropertyChanged
        {
            return (T)new ViewModelProxy(target).GetTransparentProxy();
        }
    }
}
