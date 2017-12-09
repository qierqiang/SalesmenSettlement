using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SalesmenSettlement.Forms
{
    public class UserProfileExtender : Component
    {
        public UserProfileExtender()
        {

        }

        public UserProfileExtender(IContainer container) : this()
        {
            container.Add(this);
        }

        public bool CanExtend(object extendee)
        {
            return extendee is Form || extendee is UserControl;
        }
    }
}
