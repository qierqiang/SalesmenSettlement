using SalesmenSettlement.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SalesmenSettlement.LocalService
{
    public static class ClientInfo
    {
        static ClientInfo()
        {
            Directory = new Hashtable();
        }

        public static string UserLoginName { get; set; }

        public static string UserName { get; set; }

        public static Hashtable Directory { get; private set; }

        public static DateTime GetServerTime()
        {
            Database db = new Database(AppConfig.Instance.DatabaseConnectionString);
            string sql = "SELECT GETDATE()";
            return Convert.ToDateTime(db.ExecuteScalar(sql));
        }
    }
}
