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
        public ChangePasswordForm(int userID)
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

        public ///////TODO:

        class ViewModel : ViewModelBase
        {
            public long ID { get; set; }

            [AutoGenControl(typeof(TextBox), "原密码", beginNewRow: true)]
            public string OldPassword { get; set; }

            [AutoGenControl(typeof(TextBox), "新密码", beginNewRow: true)]
            public string NewPassword { get; set; }

            [AutoGenControl(typeof(TextBox), "重复", beginNewRow: true)]
            public string RepPassword { get; set; }
        }
    }
}
