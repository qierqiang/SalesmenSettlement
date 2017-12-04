using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Text;

namespace SalesmenSettlement.Utility
{
    public class NotifyPropertyChangedProxy<T> : RealProxy where T : NotifyPropertyChanged
    {
        private T _target;

        public NotifyPropertyChangedProxy(T target)
        {
            _target = target;
        }

        public override IMessage Invoke(IMessage msg)
        {
            //PreProceede(msg);
            IMethodCallMessage callMessage = (IMethodCallMessage)msg;
            object returnValue = callMessage.MethodBase.Invoke(this._target, callMessage.Args);
            PostProceede(msg);
            return new ReturnMessage(returnValue, new object[0], 0, null, callMessage);
        }

        private void PreProceede(IMessage msg)
        {
            throw new NotImplementedException();
        }

        private void PostProceede(IMessage msg)
        {
            _target.OnPropertyChanged()
        }
    }
}