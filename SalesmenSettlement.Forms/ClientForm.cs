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
            InitializeComponent();
            Text = AppConfig.Instance.AppName;
            LoadIcon();
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

        private void LoadWindowState()
        {
            var example = new { FormWindowState = FormWindowState.Normal, Location = Point.Empty, Size = Size.Empty };
            var profile = LocalUserProfile.GetAnonymousTypeProfile(ClientInfo.UserLoginName, "ClientWindow", example);
            if (profile != null)
            {
                WindowState = profile.FormWindowState;
                if (WindowState != FormWindowState.Maximized)
                {
                    Location = profile.Location;
                    Size = profile.Size;
                }
            }
        }
        private void SaveWindowState()
        {
            var profile = new { FormWindowState = WindowState, Location, Size };
            LocalUserProfile.Save(ClientInfo.UserLoginName, "ClientWindow", profile);
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
            if (new LoginForm().ShowDialog(this) != DialogResult.OK)
            {
                Close();
                return;
            }

            LoadWindowState();
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
            if (!ClientInfo.UserLoginName.IsNullOrWhiteSpace())
                SaveWindowState();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog(this);
        }
    }
}
