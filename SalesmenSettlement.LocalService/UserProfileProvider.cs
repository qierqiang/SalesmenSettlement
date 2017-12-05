using SalesmenSettlement.Utility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SalesmenSettlement.LocalService
{
    public class UserProfileProvider
    {
        private static UserProfileProvider _instance;

        private Database _db;

        public UserProfileProvider(string cnnString)
        {
            _db = new Database(cnnString);
        }

        public string GetProfile(string category, string key, string defaultValue)
        {
            string sql = "SELECT [Value] FROM UserProfile WHERE [Category]=@P0 AND [Key]=@p1";
            SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@p0", category), new SqlParameter("@p1", key) };
            object o = _db.ExecuteScalar(new SqlCommand(sql), parameters);
            if (o.IsNullOrDbNull())
            {
                return defaultValue;
            }
            else
            {
                return o.ToString();
            }
        }

        public T GetProfile<T>(string category, string key, T defaultValue) where T : IConvertible
        {
            string tmp = GetProfile(category, key, null);
            return tmp == null ? defaultValue : (T)Convert.ChangeType(tmp, typeof(T));
        }

        public void SetProfile(string category, string key, string value)
        {
            string sql = "IF EXISTS (SELECT 1 FROM UserProfile WHERE [Category]=@p0 AND [Key]=@p1)" + "\r\n" +
                         "BEGIN" + "\r\n" +
                         "  UPDATE UserProfile SET Value=@p2 WHERE [Category]=@p0 AND [Key]=@p1" + "\r\n" +
                         "END" + "\r\n" +
                         "ELSE" + "\r\n" +
                         "BEGIN" + "\r\n" +
                         "  INSERT INTO UserProfile VALUES (@p0, @p1, @p2)" + "\r\n" +
                         "END";
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@p0", category),
                new SqlParameter("@p1", key),
                new SqlParameter("@p2", value)
            };
            _db.ExecuteNonQuery(new SqlCommand(sql), parameters);
        }

        //static

        public static UserProfileProvider GetInstance()
        {
            if (_instance == null)
            {
                _instance = new UserProfileProvider(AppConfig.GetInstance().DatabaseConnectionString);
            }
            return _instance;
        }
    }
}
