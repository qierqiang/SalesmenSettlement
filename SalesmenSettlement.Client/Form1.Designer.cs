namespace SalesmenSettlement.Client
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.autoEditor2 = new SalesmenSettlement.Forms.AutoEditGroup();
            this.autoEditor1 = new SalesmenSettlement.Forms.AutoEditGroup();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // autoEditor2
            // 
            this.autoEditor2.AutoSize = true;
            this.autoEditor2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.autoEditor2.DataSource = null;
            this.autoEditor2.Location = new System.Drawing.Point(27, 295);
            this.autoEditor2.MinimumSize = new System.Drawing.Size(806, 2);
            this.autoEditor2.Name = "autoEditor2";
            this.autoEditor2.Size = new System.Drawing.Size(806, 55);
            this.autoEditor2.TabIndex = 3;
            this.autoEditor2.Title = "组标题";
            // 
            // autoEditor1
            // 
            this.autoEditor1.AutoSize = true;
            this.autoEditor1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.autoEditor1.DataSource = null;
            this.autoEditor1.Location = new System.Drawing.Point(27, 41);
            this.autoEditor1.MinimumSize = new System.Drawing.Size(806, 2);
            this.autoEditor1.Name = "autoEditor1";
            this.autoEditor1.Size = new System.Drawing.Size(806, 55);
            this.autoEditor1.TabIndex = 2;
            this.autoEditor1.Title = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1206, 519);
            this.Controls.Add(this.autoEditor2);
            this.Controls.Add(this.autoEditor1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private Forms.AutoEditGroup autoEditor1;
        private Forms.AutoEditGroup autoEditor2;
    }
}