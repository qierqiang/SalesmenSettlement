using SalesmenSettlement.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SalesmenSettlement.Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ChangePasswordForm.ViewModel view = new ChangePasswordForm.ViewModel();
            var query = typeof(ChangePasswordForm.ViewModel).GetProperties();
            autoEditor1.CreateControls(query);
            autoEditor2.CreateControls(query);
        }
    }
}
