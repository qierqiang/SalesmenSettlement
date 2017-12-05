using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ZOC.IO
{
    /// <summary>数据操作辅助工具</summary>
    public class DbCommandBuilder : IDisposable
    {
        Dictionary<string, object> _fields = new Dictionary<string, object>();
        string _tableName = string.Empty;

        /// <summary></summary>
        public void Dispose()
        {
            _fields = null;
            _tableName = string.Empty;
        }

        /// <summary>新实例</summary>
        public DbCommandBuilder(string tableName)
        {
            if (F.IsNullOrBlank(tableName))
                throw new ArgumentNullException("tableName");

            _tableName = tableName;
        }

        /// <summary>字段集合</summary>
        public Dictionary<string, object> Fields
        {
            get { return _fields; }
        }

        /// <summary>表名</summary>
        public string TableName
        {
            get { return _tableName; }
        }

        /// <summary>数据库类型</summary>
        public DatabaseType DatabaseType { get; set; }

        /// <summary>生成插入命令</summary>
        public IDbCommand GetInsertCommand()
        {
            if (Fields.Count == 0)
                throw new Exception("字段集合中没有任何字段！");

            StringBuilder fields = new StringBuilder();
            StringBuilder values = new StringBuilder();
            List<IDataParameter> parms = new List<IDataParameter>();
            int counter = 0; // 参数计数器

            foreach (KeyValuePair<string, object> item in Fields)
            {
                if (F.IsNullOrBlank(item.Key))
                    throw new Exception("存在空名称的字段");

                fields.Append(string.Format("[{0}],", item.Key));
                string parmName = "@p" + counter++;
                values.Append(parmName + ",");
                parms.Add(Database.CreateParameter(this.DatabaseType, parmName, item.Value));
            }

            fields.Remove(fields.Length - 1, 1);
            values.Remove(values.Length - 1, 1);
            string sql = string.Format("INSERT INTO {0} ({1}) VALUES ({2})", TableName, fields, values);
            IDbCommand result = Database.CreateCommand(this.DatabaseType);
            result.CommandText = sql;
            parms.ForEach(p => result.Parameters.Add(p));
            return result;
        }

        /// <summary>生成更新命令</summary>
        public IDbCommand GetUpdateCommand()
        {
            if (Fields.Count == 0)
                throw new Exception("字段集合中没有任何字段！");

            StringBuilder sql = new StringBuilder(string.Format("UPDATE [{0}] SET ", TableName));
            int counter = 0;
            List<IDataParameter> parms = new List<IDataParameter>();

            foreach (KeyValuePair<string, object> item in Fields)
            {
                string parmName = "@p" + counter++;
                sql.Append(string.Format("[{0}]={1},", item.Key, parmName));
                parms.Add(Database.CreateParameter(this.DatabaseType, parmName, item.Value));
            }

            sql = sql.Remove(sql.Length - 1, 1);
            IDbCommand result = Database.CreateCommand(this.DatabaseType);
            result.CommandText = sql.ToString();
            parms.ForEach(p => result.Parameters.Add(p));
            return result;
        }
    }
}
