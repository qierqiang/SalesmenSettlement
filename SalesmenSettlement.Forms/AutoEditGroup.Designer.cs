namespace SalesmenSettlement.Forms
{
    partial class AutoEditGroup
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
            this.flowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lExpandStatus = new System.Windows.Forms.Label();
            this.lTitle = new System.Windows.Forms.Label();
            this.horizontalLine1 = new SalesmenSettlement.Forms.HorizontalLine();
            this.flowPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowPanel
            // 
            this.flowPanel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.flowPanel.AutoSize = true;
            this.flowPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowPanel.Controls.Add(this.panel1);
            this.flowPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowPanel.Location = new System.Drawing.Point(0, 0);
            this.flowPanel.Name = "flowPanel";
            this.flowPanel.Size = new System.Drawing.Size(806, 52);
            this.flowPanel.TabIndex = 0;
            this.flowPanel.WrapContents = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lExpandStatus);
            this.panel1.Controls.Add(this.lTitle);
            this.panel1.Controls.Add(this.horizontalLine1);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 46);
            this.panel1.TabIndex = 0;
            // 
            // lExpandStatus
            // 
            this.lExpandStatus.AutoSize = true;
            this.lExpandStatus.Font = new System.Drawing.Font("Webdings", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.lExpandStatus.Location = new System.Drawing.Point(23, 10);
            this.lExpandStatus.Name = "lExpandStatus";
            this.lExpandStatus.Size = new System.Drawing.Size(31, 24);
            this.lExpandStatus.TabIndex = 2;
            this.lExpandStatus.Text = "6";
            this.lExpandStatus.DoubleClick += new System.EventHandler(this.Title_DoubleClick);
            // 
            // lTitle
            // 
            this.lTitle.AutoSize = true;
            this.lTitle.Font = new System.Drawing.Font("黑体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lTitle.Location = new System.Drawing.Point(52, 13);
            this.lTitle.Name = "lTitle";
            this.lTitle.Size = new System.Drawing.Size(0, 21);
            this.lTitle.TabIndex = 0;
            this.lTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lTitle.DoubleClick += new System.EventHandler(this.Title_DoubleClick);
            // 
            // horizontalLine1
            // 
            this.horizontalLine1.Location = new System.Drawing.Point(69, 23);
            this.horizontalLine1.Name = "horizontalLine1";
            this.horizontalLine1.Size = new System.Drawing.Size(684, 2);
            this.horizontalLine1.TabIndex = 1;
            // 
            // AutoEditGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.flowPanel);
            this.MinimumSize = new System.Drawing.Size(806, 0);
            this.Name = "AutoEditGroup";
            this.Size = new System.Drawing.Size(806, 55);
            this.flowPanel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowPanel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lTitle;
        private HorizontalLine horizontalLine1;
        private System.Windows.Forms.Label lExpandStatus;
    }
}
