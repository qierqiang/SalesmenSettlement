using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

namespace SalesmenSettlement.Forms
{
    public class HorizontalLine : Label
    {
        public HorizontalLine()
        {
            base.AutoSize = false;
            base.Text = string.Empty;
            base.BorderStyle = BorderStyle.Fixed3D;
            base.Height = 2;
            base.MaximumSize = new Size(0, 2);
            base.MinimumSize = new Size(0, 2);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public override bool AutoSize { get => false; set { } }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public override string Text { get => string.Empty; set { } }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public override BorderStyle BorderStyle { get => BorderStyle.Fixed3D; set { } }

        public new Size Size
        {
            get { return new Size(base.Width, 2); }
            set
            {
                if (base.Size != value)
                {
                    base.Size = new Size(value.Width, 2);
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public new int Height
        {
            get { return 2; }
            set { }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public override Size MaximumSize { get => new Size(0, 2); set { } }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public override Size MinimumSize { get => new Size(0, 2); set { } }
    }
}
