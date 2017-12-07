using SalesmenSettlement.Model;
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
    public class ModelProvider
    {
        private Database _db;

        public ModelProvider(string cnnString)
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

        public bool GetIsExist(ICommonEntity model)
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

        public ICommonEntity GetModelByID(Type type, long id)
        {
            return GetModels(type, "[ID]=@p0", new SqlParameter("@p0", id)).FirstOrDefault() as ICommonEntity;
        }

        public List<object> GetModels(Type type, string where, params SqlParameter[] parms)
        {
            string sql = string.Format("SELECT * FROM [{0}]", type.Name);

            if (!where.IsNullOrWhiteSpace())
            {
                sql += " WHERE " + where;
            }

            SqlCommand cmd = new SqlCommand(sql);
            DataTable dt = _db.GetDataTable(cmd, parms);

            List<object> result = new List<object>();

            foreach (DataRow r in dt.Rows)
            {
                object model = Activator.CreateInstance(type);
                FillPropertiesWithDataRow(model, r);
                if (model is EntityBase m)
                {
                    m.SetUnModified();
                }
                result.Add(model);
            }

            return result;
        }

        /// <summary>
        /// 参数obj要求是ModelBase的派生类，且实现ICommonModel接口
        /// </summary>
        /// <param name="obj"></param>
        public void Save(object obj)
        {
            if (obj is EntityBase model && model is ICommonEntity commonModel)
            {
                _db.BeginTransaction();

                try
                {
                    if (commonModel.ID > 0)
                    {
                        Update(commonModel);
                        RefreshModelFromDatabase(commonModel);
                    }
                    else
                    {
                        long newID = Insert(commonModel);
                        RefreshModelFromDatabase(commonModel, newID);
                    }
                    _db.CommitTransaction();
                    model.SetUnModified();
                }
                catch
                {
                    _db.RollbackTransaction();
                    throw;
                }
            }
            else
            {
                throw new Exception("参数model不是有效的数据实体型！");
            }
        }

        public void RefreshModelFromDatabase(ICommonEntity model, long id = 0)
        {
            if (model.ID == 0)
            {
                if (id == 0)
                    throw new Exception("数据库中无此记录！");

                model.ID = id;
            }

            ICommonEntity tmp = GetModelByID(model.GetType(), model.ID);
            CopyTo(tmp, model);
        }

        public void CopyTo(object fromModel, object toModel)
        {
            throw new NotImplementedException();
        }

        //private
        /// <summary>
        /// 插入新记录并返回新记录的ID
        /// </summary>
        public long Insert(ICommonEntity model)
        {
            Type type = model.GetType();
            PropertyInfo[] properties = type.GetProperties();
            Dictionary<string, SqlParameter> dictionary = new Dictionary<string, SqlParameter>();

            foreach (var p in properties)
            {
                if (!p.GetCustomAttributes(typeof(AutoGenerateAttribute), true).Any())
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
            return (long)_db.ExecuteScalar(new SqlCommand(sb.ToString()), dictionary.Values.ToArray());
        }

        /// <summary>
        /// 用model更新数据库记录
        /// </summary>
        public void Update(ICommonEntity model)
        {
            Type type = model.GetType();
            PropertyInfo[] properties = type.GetProperties();
            Dictionary<string, SqlParameter> dictionary = new Dictionary<string, SqlParameter>();

            foreach (var p in properties)
            {
                if (!p.GetCustomAttributes(typeof(AutoGenerateAttribute), true).Any())
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
            if (_db.ExecuteNonQuery(new SqlCommand(sb.ToString()), dictionary.Values.ToArray()) == 0)
            {
                throw new Exception("更新失败，ExecuteNonQuery返回0。");
            }
        }

        /// <summary>
        /// 用model更新数据库记录,只会更新发生过修改的字段值！
        /// </summary>
        public void Update<T>(T model) where T : EntityBase, ICommonEntity
        {
            Type type = model.GetType();
            PropertyInfo[] properties = type.GetProperties();
            Dictionary<string, SqlParameter> dictionary = new Dictionary<string, SqlParameter>();

            foreach (var p in properties)
            {
                if (model.GetModifiedProperties().Contains(p.Name) && !p.GetCustomAttributes(typeof(AutoGenerateAttribute), true).Any())
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
            if (_db.ExecuteNonQuery(new SqlCommand(sb.ToString()), dictionary.Values.ToArray()) == 0)
            {
                throw new Exception("更新失败，ExecuteNonQuery返回0。");
            }
        }

        //static
        private static ModelProvider _instance;

        public static ModelProvider Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ModelProvider(AppConfig.Instance.DatabaseConnectionString);
                }
                return _instance;
            }
        }

        public static void FillPropertiesWithDataRow(object target, DataRow row)
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
        public static ModelProvider GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ModelProvider(AppConfig.Instance.DatabaseConnectionString);
            }
            return _instance;
        }
    }
}
