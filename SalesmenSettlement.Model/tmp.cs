using System;
using System.ComponentModel;

namespace SalesmenSettlement.Model
{
    public class tmp : INotifyPropertyChanged, ICommonModel
    {

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null && !propertyName.IsNullOrWhiteSpace())
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
