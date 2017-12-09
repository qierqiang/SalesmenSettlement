using SalesmenSettlement.LocalService;
using SalesmenSettlement.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SalesmenSettlement.Model;

namespace SalesmenSettlement.Forms
{
    public class EditFormBase : Form
    {
        public event EventHandler DataSourceChanged;

        private ViewModelBase _dataSource;

        public ViewModelBase DataSource
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

        public EditFormBase() { }

        public EditFormBase(ViewModelBase model)
        {
            DataSource = model;
        }

        public virtual void DataBind()
        {
            //if ()
        }

        public virtual bool ValidateForm() { return true; }

        public virtual bool Submit()
        {
            if (ValidateForm())
            {
                EntityProvider.Instance.Save(DataSource);
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

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // EditFormBase
            // 
            this.ClientSize = new System.Drawing.Size(954, 447);
            this.Name = "EditFormBase";
            this.ResumeLayout(false);

        }
    }
}
