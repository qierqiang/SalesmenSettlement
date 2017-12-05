using System;
using System.Data;
using System.Data.SqlClient;

namespace ZOC.IO
{
    /// <summary>微软SQL数据库操作类</summary>
    public class MSSQL : Database
    {
        SqlConnection _cnn;

        /// <summary>新实例</summary>
        public MSSQL() { }

        /// <summary>新实例</summary>
        public MSSQL(string connection) : base(connection) { }

        /// <summary>新实例</summary>
        public MSSQL(SqlConnection connection) : base(connection) { }

        /// <summary>与数据库的连接Connection实例</summary>
        public override IDbConnection Connection
        {
            get { return _cnn; }
            set
            {
                //if (!(value is SqlConnection))
                //    throw new Exception("所给的值不是有效的SqlConnection.");

                SqlConnection cnn = (SqlConnection)value;

                if (_cnn != cnn)
                {
                    if (_cnn != null)
                    {
                        if (_cnn.State != ConnectionState.Closed)
                            _cnn.Close();

                        _cnn.Dispose();
                    }

                    _cnn = cnn;
                }
            }
        }

        /// <summary>数据库的类型</summary>
        public override DatabaseType DatabaseType
        {
            get { return DatabaseType.MSSQL; }
        }

        /// <summary>获取数据库列表</summary>
        public virtual string[] GetDatabaseNames()
        {
            lock (this)
            {
                OpenConnection();

                try
                {
                    string sql = "SELECT [name] FROM [sysdatabases] WHERE [dbid] > 6 ORDER BY [name]";
                    DataTable dt = GetDataTable(sql);
                    string[] result = new string[dt.Rows.Count];

                    for (int i = 0; i < dt.Rows.Count; i++)
                        result[i] = (string)dt.Rows[i][0];

                    return result;
                }
                finally
                {
                    CloseConnection();
                }
            }
        }
    }
}