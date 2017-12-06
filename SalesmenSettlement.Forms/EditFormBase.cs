using SalesmenSettlement.LocalService;
using SalesmenSettlement.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SalesmenSettlement.Forms
{
    public class EditFormBase : Form
    {
        public event EventHandler DataSourceChanged;

        private ModelBase _dataSource;

        public ModelBase DataSource
        {
            get => _dataSource;
            set
            {
                if (_dataSource != value)
                {
                    _dataSource = value;
                    OnDataSourceChanged();
                }
            }
        }

        public virtual void DataBind() { }

        public virtual bool ValidateForm() { return true; }

        public virtual bool Submit()
        {
            if (ValidateForm())
            {
                ModelFactory.Instance.Save(_dataSource);
            }
            return true;
        }

        protected virtual void OnDataSourceChanged()
        {
            DataBind();
            DataSourceChanged?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (!e.Cancel && DataSource != null && DataSource.GetIsModified() && !Msgbox.DontSaveConfirm())
            {
                e.Cancel = true;
            }
        }
    }
}
