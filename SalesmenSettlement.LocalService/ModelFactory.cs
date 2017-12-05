using SalesmenSettlement.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SalesmenSettlement.LocalService
{
    public class ModelFactory
    {
        private Database _db;

        public ModelFactory(string cnnString)
        {
            _db = new Database(cnnString);
        }

        private static ModelFactory _instance;

        public static ModelFactory GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ModelFactory(AppConfig.GetInstance().DatabaseConnectionString);
            }
            return _instance;
        }

        public T GetModel<T>(string where, params SqlParameter[] parms) where T : ModelBase
        {
            string sql = string.Format("SELECT * FROM [{0}]", typeof(T).Name);

            if (!where.IsNullOrWhiteSpace())
            {
                sql += " WHERE " + where;
            }

            SqlCommand cmd = new SqlCommand(sql);
            DataTable dt = _db.GetDataTable(cmd, parms);

            if (dt.Rows.Count > 0)
            {
                return ModelBase.CreateInstanceFromDataRow<T>(dt.Rows[0]);
            }
            return null;
        }

        public List<T> GetModelList<T>(string where, params SqlParameter[] parms) where T : ModelBase
        {
            string sql = string.Format("SELECT * FROM [{0}]", typeof(T).Name);

            if (!where.IsNullOrWhiteSpace())
            {
                sql += " WHERE " + where;
            }

            SqlCommand cmd = new SqlCommand(sql);
            DataTable dt = _db.GetDataTable(cmd, parms);

            List<T> result = new List<T>();

            foreach (DataRow r in dt.Rows)
            {
                result.Add(ModelBase.CreateInstanceFromDataRow<T>(r));
            }

            return result;
        }
    }
}
