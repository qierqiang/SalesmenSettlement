using System;
using SalesmenSettlement.Utility;

namespace SalesmenSettlement.Model
{
    public class UserInfo : ModelBase
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string PasswordMD5 { get; set; }
        public bool Disabled { get; set; }
    }
}
