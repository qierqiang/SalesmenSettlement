using SalesmenSettlement.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SalesmenSettlement.Forms
{
    public partial class ChangePasswordForm : EditFormBase
    {
        public ChangePasswordForm(int i)
        {
            InitializeComponent();
        }

        public override void DataBind()
        {
            base.DataBind();
        }

        public override bool ValidateForm()
        {
            return base.ValidateForm();
        }

        public override bool Submit()
        {
            return base.Submit();
        }

        class ViewModel : MarshalByRefObject
        {
            public long ID { get; set; }

            [EditControlType(typeof(TextBox))]
            public string OldPassword { get; set; }

            [EditControlType(typeof(TextBox))]
            public string NewPassword { get; set; }

            [EditControlType(typeof(TextBox))]
            public string RepPassword { get; set; }
        }
    }
}
