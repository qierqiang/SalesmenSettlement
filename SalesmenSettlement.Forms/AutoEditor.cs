using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SalesmenSettlement.Utility;

namespace SalesmenSettlement.Forms
{
    public partial class AutoEditor : UserControl
    {
        public event EventHandler DataSourceChanged;

        //private ViewModel _dataSource;

        //public AutoEditor()
        //{
        //    InitializeComponent();
        //}

        //public AutoEditor(ViewModel datasource) : this()
        //{
        //    //Task
        //}

        //public virtual ViewModel DataSource
        //{
        //    get => _dataSource;
        //    set
        //    {
        //        if (_dataSource != value)
        //        {
        //            _dataSource = value;
        //            OnDataSourceChanged();
        //        }
        //    }
        //}

        protected virtual void DataBind()
        {
            //if (!DataSource.IsProxy())
            //{
            //    _dataSource = _dataSource.GetProxy();
            //}
        }

        protected virtual void OnDataSourceChanged()
        {
            DataBind();
            DataSourceChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
