using SalesmenSettlement.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Proxies;
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

        public bool GetIsExist<T>(long id)
        {
            if (id == 0)
                return false;

            string sql = "SELECT 1 FROM [{0}] WHERE [ID]=@p0".FormatWith(typeof(T).Name);
            return !_db.ExecuteScalar(new SqlCommand(sql), new SqlParameter("@p0", id)).IsNullOrDbNull();
        }

        public bool GetIsExist(ModelBase model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (model.ID == 0)
                return false;

            string sql = "SELECT 1 FROM [{0}] WHERE [ID]=@p0".FormatWith(model.GetType().Name);
            return !_db.ExecuteScalar(new SqlCommand(sql), new SqlParameter("@p0", model.ID)).IsNullOrDbNull();
        }

        public T GetModelByID<T>(long id) where T : ModelBase
        {
            return GetModel<T>("[ID]=@p0", new SqlParameter("@p0", id));
        }

        public T GetModel<T>(string where, params SqlParameter[] parms) where T : ModelBase
        {
            string sql = string.Format("SELECT * FROM [{0}]", typeof(T).Name);

            if (!where.IsNullOrWhiteSpace())
            {
                sql += " WHERE " + where;
            }

            DataTable dt = _db.GetDataTable(new SqlCommand(sql), parms);

            if (dt.Rows.Count > 0)
            {
                T model = GetOriginModel<T>();
                InitPropertyFromDataRow(model, dt.Rows[0]);
                return ModelProxy.Proxy(model);

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
                T model = GetOriginModel<T>();
                InitPropertyFromDataRow(model, r);
                result.Add(model);
            }

            return result;
        }

        public T GetOriginModel<T>() where T : ModelBase => Activator.CreateInstance<T>();

        public T ImpletementPropertyChanged<T>(T model) where T : ModelBase => ModelProxy.Proxy(model);

        public void Save(ModelBase model)
        {
            _db.BeginTransaction();

            try
            {
                if (GetIsExist(model))
                {
                    Update(model);
                }
                else
                {
                    Insert(model);
                }
                model.SetIsModified(false);
            }
            finally
            {
                _db.CommitTransaction();
            }
        }

        //private
        private void Insert(ModelBase model)
        {
            Type type = model.GetType();
            PropertyInfo[] properties = type.GetProperties();
            Dictionary<string, SqlParameter> dictionary = new Dictionary<string, SqlParameter>();

            foreach (var p in properties)
            {
                if (!p.GetCustomAttributes(typeof(DontSaveAttribute), true).Any())
                {
                    dictionary[p.Name] = new SqlParameter("@" + p.Name, p.GetValue(model, null));
                }
            }

            StringBuilder sb = new StringBuilder("INSERT INTO [{0}] (\r\n".FormatWith(type.Name));
            StringBuilder sbValues = new StringBuilder();

            foreach (var item in dictionary)
            {
                sb.AppendLine("\t[{0}],".FormatWith(item.Key));
                sbValues.AppendLine("\t@" + item.Key + ",");
            }

            sb.AppendLine(") VALUES (");
            sb.Append(sbValues.ToString());
            sb.Append(")\r\n");
            sb.Append("SELECT SCOPE_IDENTITY()");
            model.ID = (long)_db.ExecuteScalar(new SqlCommand(sb.ToString()), dictionary.Values.ToArray());
        }

        private void Update(ModelBase model)
        {
            Type type = model.GetType();
            PropertyInfo[] properties = type.GetProperties();
            Dictionary<string, SqlParameter> dictionary = new Dictionary<string, SqlParameter>();

            foreach (var p in properties)
            {
                if (!p.GetCustomAttributes(typeof(DontSaveAttribute), true).Any())
                {
                    dictionary[p.Name] = new SqlParameter("@" + p.Name, p.GetValue(model, null));
                }
            }

            dictionary["ID"] = new SqlParameter("@ID", model.ID);
            StringBuilder sb = new StringBuilder("UPDATE [{0}] SET \r\n".FormatWith(type.Name));

            foreach (var item in dictionary)
            {
                sb.AppendLine("\t[{0}] = @{0},".FormatWith(item.Key));
            }

            sb.Remove(sb.Length - 3, 3).Append("\r\nWHERE [ID] = @ID");

            _db.ExecuteNonQuery(new SqlCommand(sb.ToString()), dictionary.Values.ToArray());
        }

        //static
        private static ModelFactory _instance;

        public static ModelFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ModelFactory(AppConfig.Instance.DatabaseConnectionString);
                }
                return _instance;
            }
        }

        public static void InitPropertyFromDataRow(ModelBase target, DataRow row)
        {
            for (int i = 0; i < row.Table.Columns.Count; i++)
            {
                string colName = row.Table.Columns[i].Caption;
                object value = row[colName];
                SetPropertyValue(target, colName, value);
            }
        }

        private static bool SetPropertyValue(object target, string propertyName, object value)
        {
            if (value.IsNullOrDbNull())
            {
                return true;
            }

            Type type = target is RealProxy rp ? rp.GetProxiedType() : target.GetType();
            var p = type.GetProperty(propertyName);

            if (p != null && p.CanWrite)
            {
                if (p.PropertyType == typeof(string))
                {
                    p.SetValue(target, value.ToString(), new object[] { });
                }
                else
                {
                    p.SetValue(target, value, new object[] { });
                }
                return true;
            }
            return false;
        }

        //Obsolete
        [Obsolete()]
        public static ModelFactory GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ModelFactory(AppConfig.Instance.DatabaseConnectionString);
            }
            return _instance;
        }
    }
}
