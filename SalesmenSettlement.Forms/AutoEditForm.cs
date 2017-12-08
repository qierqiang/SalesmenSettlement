using SalesmenSettlement.Model;
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
    public partial class AutoEditForm : EditFormBase
    {
        public AutoEditForm()
        {
            InitializeComponent();
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            Submit();
        }

        private void bClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        //public virtual void AutoGenerateControls()
    }
}
