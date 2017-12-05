using SalesmenSettlement.LocalService;
using SalesmenSettlement.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SalesmenSettlement.Forms
{
    public partial class LoginForm : Form
    {
        private Dictionary<string, string> _userHistory = new Dictionary<string, string>();

        public LoginForm()
        {
            InitializeComponent();
        }

        private void bLogin_Click(object sender, EventArgs e)
        {
            string userName = cmbUserName.Text.Trim();
            string pwdMd5 = txtPwd.Text.GetMD5();
            var user = ModelFactory.GetInstance().GetModel<UserInfo>("userName=@p0 and PasswordMD5=@p1",
                new SqlParameter("@p0", userName), new SqlParameter("@p1", pwdMd5));

            if (user != null)
            {
                MessageBox.Show("登录成功！");
            }
            else
            {
                lMsg.Visible = true;
                ResetTimer();
            }
        }

        private void LoadUserHistory()
        {
            DirectoryInfo dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            FileInfo[] files = dir.GetFiles("u_*.dat").OrderByDescending(f => f.LastWriteTime).ToArray();

            if (files.Length > 10)
            {
                foreach (FileInfo f in files.OrderByDescending(f => f.LastWriteTime).Skip(10))
                {
                    f.Delete();
                }
                files = files.Take(10).ToArray();
            }

            foreach (FileInfo f in files)
            {
                //_userHistory.Add(f.Name.Substring(2, f.Name.Length - 6), File.ReadAllLines(f.FullName).FirstOrDefault() ? "");
            }
        }

        private void SaveUserHistory()
        {
            string fileName = AppDomain.CurrentDomain.BaseDirectory + "u_" + cmbUserName.Text.Trim() + ".dat";
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            string content = cRememberPwd.Checked ? txtPwd.Text.GetMD5() : string.Empty;
            File.AppendAllText(fileName, content);
        }

        private void ResetTimer()
        {
            timer1.Stop();
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lMsg.Visible = false;
            timer1.Stop();
        }
    }
}
