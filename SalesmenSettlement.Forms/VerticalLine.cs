using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SalesmenSettlement.Forms
{
    public class VerticalLine : Label
    {

        public VerticalLine()
        {
            base.AutoSize = false;
            base.Text = string.Empty;
            base.BorderStyle = BorderStyle.Fixed3D;
            base.Size = new Size(2, 100);
            base.MaximumSize = new Size(2, 0);
            base.MinimumSize = new Size(2, 0);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public override bool AutoSize { get => false; set { } }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public override string Text { get => string.Empty; set { } }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public override BorderStyle BorderStyle { get => BorderStyle.Fixed3D; set { } }

        public new Size Size
        {
            get { return new Size(2, base.Height); }
            set
            {
                if (base.Size != value)
                {
                    base.Size = new Size(2, value.Height);
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public new int Width
        {
            get { return 2; }
            set { }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public override Size MaximumSize { get => new Size(2, 0); set { } }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public override Size MinimumSize { get => new Size(2, 0); set { } }
    }
}
