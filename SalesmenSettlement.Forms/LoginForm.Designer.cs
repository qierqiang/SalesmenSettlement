namespace SalesmenSettlement.Forms
{
    partial class LoginForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.cRememberPwd = new System.Windows.Forms.CheckBox();
            this.bLogin = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cClearLogin = new System.Windows.Forms.CheckBox();
            this.txtPwd = new System.Windows.Forms.TextBox();
            this.cmbUserName = new System.Windows.Forms.ComboBox();
            this.lMsg = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // cRememberPwd
            // 
            this.cRememberPwd.AutoSize = true;
            this.cRememberPwd.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.cRememberPwd.Location = new System.Drawing.Point(541, 164);
            this.cRememberPwd.Name = "cRememberPwd";
            this.cRememberPwd.Size = new System.Drawing.Size(108, 16);
            this.cRememberPwd.TabIndex = 2;
            this.cRememberPwd.Text = "记住密码(慎用)";
            this.cRememberPwd.UseVisualStyleBackColor = true;
            this.cRememberPwd.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbUserName_KeyPress);
            // 
            // bLogin
            // 
            this.bLogin.Font = new System.Drawing.Font("黑体", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bLogin.ForeColor = System.Drawing.SystemColors.Highlight;
            this.bLogin.Location = new System.Drawing.Point(500, 199);
            this.bLogin.Name = "bLogin";
            this.bLogin.Size = new System.Drawing.Size(149, 69);
            this.bLogin.TabIndex = 3;
            this.bLogin.Text = "登录";
            this.bLogin.UseVisualStyleBackColor = true;
            this.bLogin.Click += new System.EventHandler(this.bLogin_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.label1.Location = new System.Drawing.Point(304, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "用户名";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.label2.Location = new System.Drawing.Point(304, 119);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 16);
            this.label2.TabIndex = 6;
            this.label2.Text = "密码";
            // 
            // cClearLogin
            // 
            this.cClearLogin.AutoSize = true;
            this.cClearLogin.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.cClearLogin.Location = new System.Drawing.Point(307, 164);
            this.cClearLogin.Name = "cClearLogin";
            this.cClearLogin.Size = new System.Drawing.Size(156, 16);
            this.cClearLogin.TabIndex = 4;
            this.cClearLogin.Text = "登录后清除所有用户设置";
            this.cClearLogin.UseVisualStyleBackColor = true;
            this.cClearLogin.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbUserName_KeyPress);
            // 
            // txtPwd
            // 
            this.txtPwd.Font = new System.Drawing.Font("Verdana", 15.75F);
            this.txtPwd.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtPwd.HideSelection = false;
            this.txtPwd.Location = new System.Drawing.Point(366, 109);
            this.txtPwd.MaxLength = 100;
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.PasswordChar = '*';
            this.txtPwd.Size = new System.Drawing.Size(283, 33);
            this.txtPwd.TabIndex = 1;
            this.txtPwd.UseSystemPasswordChar = true;
            this.txtPwd.WordWrap = false;
            this.txtPwd.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbUserName_KeyPress);
            // 
            // cmbUserName
            // 
            this.cmbUserName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbUserName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbUserName.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbUserName.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cmbUserName.FormattingEnabled = true;
            this.cmbUserName.Location = new System.Drawing.Point(366, 67);
            this.cmbUserName.MaxLength = 50;
            this.cmbUserName.Name = "cmbUserName";
            this.cmbUserName.Size = new System.Drawing.Size(283, 33);
            this.cmbUserName.TabIndex = 0;
            this.cmbUserName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbUserName_KeyPress);
            // 
            // lMsg
            // 
            this.lMsg.AutoSize = true;
            this.lMsg.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lMsg.ForeColor = System.Drawing.Color.DeepPink;
            this.lMsg.Location = new System.Drawing.Point(304, 232);
            this.lMsg.Name = "lMsg";
            this.lMsg.Size = new System.Drawing.Size(152, 16);
            this.lMsg.TabIndex = 7;
            this.lMsg.Text = "用户名或密码错误！";
            this.lMsg.Visible = false;
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(261, 256);
            this.panel1.TabIndex = 8;
            // 
            // timer1
            // 
            this.timer1.Interval = 3000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(685, 280);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lMsg);
            this.Controls.Add(this.cmbUserName);
            this.Controls.Add(this.txtPwd);
            this.Controls.Add(this.cClearLogin);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bLogin);
            this.Controls.Add(this.cRememberPwd);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.LoginForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cRememberPwd;
        private System.Windows.Forms.Button bLogin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox cClearLogin;
        private System.Windows.Forms.TextBox txtPwd;
        private System.Windows.Forms.ComboBox cmbUserName;
        private System.Windows.Forms.Label lMsg;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}