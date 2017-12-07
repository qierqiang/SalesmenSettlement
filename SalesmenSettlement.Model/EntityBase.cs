using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SalesmenSettlement.Model
{
    public class EntityBase : INotifyPropertyChanged
    {
        private bool _isModified;
        private HashSet<string> _modifiedProperties = new HashSet<string>();

        public bool GetIsModified() => _isModified;

        public void SetUnModified()
        {
            _isModified = false;
            _modifiedProperties.Clear();
        }

        public HashSet<string> GetModifiedProperties() => _modifiedProperties;

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            _isModified = true;
            _modifiedProperties.Add(propertyName);

            if (PropertyChanged != null && !propertyName.IsNullOrWhiteSpace())
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
