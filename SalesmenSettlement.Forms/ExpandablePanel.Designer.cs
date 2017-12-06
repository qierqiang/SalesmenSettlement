namespace SalesmenSettlement.Forms
{
    partial class ExpandablePanel
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.mainPanel = new System.Windows.Forms.Panel();
            this.horizontalLine1 = new SalesmenSettlement.Forms.HorizontalLine();
            this.bExpand = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lCaption = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainPanel
            // 
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 28);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(641, 416);
            this.mainPanel.TabIndex = 0;
            // 
            // horizontalLine1
            // 
            this.horizontalLine1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.horizontalLine1.Location = new System.Drawing.Point(19, 13);
            this.horizontalLine1.Name = "horizontalLine1";
            this.horizontalLine1.Size = new System.Drawing.Size(583, 2);
            this.horizontalLine1.TabIndex = 1;
            // 
            // bExpand
            // 
            this.bExpand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bExpand.FlatAppearance.BorderSize = 0;
            this.bExpand.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bExpand.Font = new System.Drawing.Font("Webdings", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.bExpand.Location = new System.Drawing.Point(608, 1);
            this.bExpand.Name = "bExpand";
            this.bExpand.Size = new System.Drawing.Size(26, 23);
            this.bExpand.TabIndex = 2;
            this.bExpand.Text = "5";
            this.bExpand.UseVisualStyleBackColor = true;
            this.bExpand.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lCaption);
            this.panel2.Controls.Add(this.bExpand);
            this.panel2.Controls.Add(this.horizontalLine1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(641, 28);
            this.panel2.TabIndex = 3;
            // 
            // lCaption
            // 
            this.lCaption.AutoSize = true;
            this.lCaption.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lCaption.Location = new System.Drawing.Point(15, 4);
            this.lCaption.Name = "lCaption";
            this.lCaption.Size = new System.Drawing.Size(0, 20);
            this.lCaption.TabIndex = 0;
            // 
            // ExpandablePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.panel2);
            this.Name = "ExpandablePanel";
            this.Size = new System.Drawing.Size(641, 444);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel mainPanel;
        private HorizontalLine horizontalLine1;
        private System.Windows.Forms.Button bExpand;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lCaption;
    }
}
