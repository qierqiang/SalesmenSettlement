using SalesmenSettlement.LocalService;
using SalesmenSettlement.Model;
using SalesmenSettlement.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows.Forms;

namespace SalesmenSettlement.Forms
{
    public partial class ClientForm : Form
    {
        private int childFormNumber = 0;

        public ClientForm()
        {
            if (new LoginForm().ShowDialog() != DialogResult.OK)
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            LastInstance = this;
            InitializeComponent();
            Text = AppConfig.Instance.AppName;
            LoadIcon();
            FormState.LoadFormState(this, "ClientForm", null);
        }

        private void LoadIcon()
        {
            try
            {
                Icon ico = new Icon(AppDomain.CurrentDomain.BaseDirectory + "ico.ico");
                Icon = ico;
            }
            catch
            {
                ShowIcon = false;
            }
        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            Form childForm = new Form();
            childForm.MdiParent = this;
            childForm.Text = "窗口 " + childFormNumber++;
            childForm.Show();
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "文本文件(*.txt)|*.txt|所有文件(*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "文本文件(*.txt)|*.txt|所有文件(*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {


            //LoadWindowState();
            Dashboard childForm = new Dashboard();
            childForm.MdiParent = this;
            childForm.Show();
            childForm.WindowState = FormWindowState.Maximized;
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.ShowDialog(this);
        }

        private void ClientForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            FormState.SaveFormState(this, "ClientForm", null);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog(this);
        }

        private void changePwdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangePasswordForm.ViewModel model = new ChangePasswordForm.ViewModel();
            model.ID = ClientInfo.UserID;
            model = ViewModelProxy.Proxy(model);
            OpenDialogForm(typeof(AutoEditForm), "修改密码", model);
        }

        public Form OpenMDIForm(Type formType)
        {
            if (Activator.CreateInstance(formType) is Form frm)
            {
                frm.ShowInTaskbar = true;
                frm.MdiParent = this;
                //frm.Icon == Icon.de
                frm.Show();
                return frm;
            }

            return null;
        }

        public DialogResult OpenDialogForm(Type formType, string title, ViewModelBase viewModel = null)
        {
            Form frm;

            if (viewModel == null)
            {
                frm = Activator.CreateInstance(formType, title) as Form;
            }
            else
            {
                frm = Activator.CreateInstance(formType, viewModel, title) as Form;
            }
            if (frm != null)
            {
                //加载布局
                FormState.LoadFormState(frm, title, viewModel);
                //添加保存布局事件
                frm.ShowIcon = false;
                frm.ShowInTaskbar = false;
                MinimumSize = new Size(820, 200);
                frm.Closed += (object sender, EventArgs e) => { FormState.SaveFormState(frm, title, viewModel); };
                return frm.ShowDialog(this);
            }

            return DialogResult.None;
        }

        public static ClientForm LastInstance { get; set; }
    }
}
