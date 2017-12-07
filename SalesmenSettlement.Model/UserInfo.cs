using System;
using System.ComponentModel;

namespace SalesmenSettlement.Model
{
    public class UserInfo : INotifyPropertyChanged, ICommonEntity
    {
        private Int64 _iD;


        [AutoGenerate]
        public Int64 ID
        {
            get { return _iD; }
            set
            {
                if (_iD != value)
                {
                    _iD = value;
                    OnPropertyChanged("ID");
                }
            }
        }

        private Guid _userID;



        public Guid UserID
        {
            get { return _userID; }
            set
            {
                if (_userID != value)
                {
                    _userID = value;
                    OnPropertyChanged("UserID");
                }
            }
        }

        private String _userName;


        [Validate(length: 50)]
        public String UserName
        {
            get { return _userName; }
            set
            {
                if (_userName != value)
                {
                    _userName = value;
                    OnPropertyChanged("UserName");
                }
            }
        }

        private String _displayName;


        [Validate(length: 50)]
        public String DisplayName
        {
            get { return _displayName; }
            set
            {
                if (_displayName != value)
                {
                    _displayName = value;
                    OnPropertyChanged("DisplayName");
                }
            }
        }

        private String _passwordMD5;


        [Validate(length: 32)]
        public String PasswordMD5
        {
            get { return _passwordMD5; }
            set
            {
                if (_passwordMD5 != value)
                {
                    _passwordMD5 = value;
                    OnPropertyChanged("PasswordMD5");
                }
            }
        }

        private Boolean? _disabled;



        public Boolean? Disabled
        {
            get { return _disabled; }
            set
            {
                if (_disabled != value)
                {
                    _disabled = value;
                    OnPropertyChanged("Disabled");
                }
            }
        }


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
