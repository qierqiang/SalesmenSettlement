using SalesmenSettlement.LocalService;
using SalesmenSettlement.Model;
using SalesmenSettlement.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SalesmenSettlement.Forms
{
    public partial class LoginForm : Form
    {
        private Dictionary<string, string> _userHistory = new Dictionary<string, string>();

        private ViewModel _vm;

        public LoginForm()
        {
            InitializeComponent();
            InitViewModel();
            DataBind();
        }

        private void InitViewModel()
        {
            _vm = new ViewModel();
            ViewModel.Impletement(ref _vm);
            _vm.PropertyChanged += ViewModelPropertyChanged;
        }

        public void DataBind()
        {
            cmbUserName.DataBindings.Add(new Binding("Text", _vm, "UserName", false, DataSourceUpdateMode.OnPropertyChanged));
            txtPwd.DataBindings.Add(new Binding("Text", _vm, "Password", false, DataSourceUpdateMode.OnPropertyChanged));
            cClearLogin.DataBindings.Add(new Binding("Checked", _vm, "ClearLogin", false, DataSourceUpdateMode.OnPropertyChanged));
            cRememberPwd.DataBindings.Add(new Binding("Checked", _vm, "RememberPwd", false, DataSourceUpdateMode.OnPropertyChanged));
        }

        private void bLogin_Click(object sender, EventArgs e)
        {
            string userName = _vm.UserName;
            string pwdMd5 = _vm.Password.GetMD5();
            var user = ModelFactory.GetInstance().GetModel<UserInfo>("userName=@p0 and PasswordMD5=@p1",
                new SqlParameter("@p0", userName), new SqlParameter("@p1", pwdMd5));

            if (user != null)
            {
                MessageBox.Show("登录成功！");
                SaveUserHistory();
            }
            else
            {
                lMsg.Visible = true;
                ResetTimer();
            }
        }

        private void LoadUserHistory()
        {
            //只加载最近10个
            var dirs = new DirectoryInfo(UserProfile.UserProfileDirecotry).GetDirectories().OrderByDescending(d => d.LastWriteTime).Take(10);

            foreach (var d in dirs)//每个目录一个用户（的所有设置）
            {
                //找密码
                string pwd = UserProfile.GetProfileContent(d.Name, "password");
                _userHistory[d.Name] = pwd;
                cmbUserName.Items.Add(d.Name);
            }
            if (_userHistory.Any())
            {
                _vm.UserName = _userHistory.Keys.First();
            }
        }

        private void SaveUserHistory()
        {
            if (cRememberPwd.Checked)
            {
                UserProfile.SaveContent(_vm.UserName, "password", _vm.Password);
            }
            else
            {
                UserProfile.Delete(_vm.UserName, "password");
            }
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

        private void LoginForm_Load(object sender, EventArgs e)
        {
            LoadUserHistory();
        }

        private void ViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "UserName")
            {
                if (_userHistory.ContainsKey(_vm.UserName))
                {
                    string pwd = _userHistory[_vm.UserName];
                    if (pwd != null)
                    {
                        _vm.Password = pwd;
                        _vm.RememberPwd = true;
                    }
                    else
                    {
                        _vm.Password = string.Empty;
                        _vm.RememberPwd = false;
                    }
                }
                else
                {
                    _vm.Password = string.Empty;
                    _vm.RememberPwd = false;
                }
            }
            else if (e.PropertyName == "ClearLogin")
            {
                if (_vm.ClearLogin)
                {
                    _vm.RememberPwd = false;
                }
            }
            else if (e.PropertyName == "RememberPwd")
            {
                if (_vm.RememberPwd)
                {
                    _vm.ClearLogin = false;
                }
            }
        }
    }

    class ViewModel : NotifyPropertyChanged
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool ClearLogin { get; set; }
        public bool RememberPwd { get; set; }
    }
}
