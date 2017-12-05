// Created by Snokye

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace ZOC.IO
{
    /// <summary>
    /// 数据库基类
    /// 
    /// 更新日志：2011-02-27
    ///     添加了开始事务结束事务的功能
    /// </summary>
    public abstract class Database : MarshalByRefObject, IDisposable
    {
        #region ctor

        /// <summary>新实例</summary>
        protected Database()
        {
            this.KeepConnectionOpen = false;
        }

        /// <summary>新实例</summary>
        protected Database(string connection)
            : this()
        {
            if (string.IsNullOrEmpty(connection))
                throw new ArgumentNullException("connection");

            this.Connection = CreateConnection(this.DatabaseType, connection);
        }

        /// <summary>新实例</summary>
        protected Database(IDbConnection connection)
            : this()
        {
            if (connection == null)
                throw new ArgumentNullException("connection");

            this.Connection = connection;
        }

        /// <summary>释放组件所占用的资源</summary>
        public virtual void Dispose()
        {
            if (this.Connection != null)
            {
                //if (this.Connection.State != ConnectionState.Closed)
                //    this.Connection.Close();

                //this.Connection.Dispose();
                //this.Connection = null;
            }
        }

        #endregion

        #region Connection

        /// <summary>与数据库的连接Connection实例</summary>
        public abstract IDbConnection Connection { get; set; }

        /// <summary>保持连接处于打开状态</summary>
        public bool KeepConnectionOpen { get; set; }

        /// <summary>数据库类型</summary>
        public abstract DatabaseType DatabaseType { get; }

        /// <summary>打开连接</summary>
        protected virtual void OpenConnection()
        {
            if (this.Connection == null)
                throw new Exception("Connection属性未设置!");

            if (this.Connection.State == ConnectionState.Closed)
                this.Connection.Open();
        }

        /// <summary>关闭连接</summary>
        protected virtual void CloseConnection()
        {
            if (!this.KeepConnectionOpen &&
                this.Transaction == null &&
                this.Connection.State != ConnectionState.Closed)
                this.Connection.Close();
        }

        #endregion

        #region Static

        /// <summary>创建数据库连接实例</summary>
        public static IDbConnection CreateConnection(DatabaseType type, string cnn)
        {
            switch (type)
            {
                case DatabaseType.MSSQL: return new SqlConnection(cnn);
                case DatabaseType.Access: return new OleDbConnection(cnn);
                case DatabaseType.Excel: return new OleDbConnection(cnn);
                default: throw new NotSupportedException();
            }
        }

        /// <summary>创建Command实例</summary>
        public static IDbCommand CreateCommand(DatabaseType type)
        {
            switch (type)
            {
                case DatabaseType.MSSQL: return new SqlCommand { CommandTimeout = 60 };
                case DatabaseType.Access: return new OleDbCommand { CommandTimeout = 60 };
                case DatabaseType.Excel: return new OleDbCommand { CommandTimeout = 60 };
                default: throw new NotSupportedException();
            }
        }

        /// <summary>创建数据适配器实例</summary>
        public static IDbDataAdapter CreateAdapter(DatabaseType type)
        {
            switch (type)
            {
                case DatabaseType.MSSQL: return new SqlDataAdapter();
                case DatabaseType.Access: return new OleDbDataAdapter();
                case DatabaseType.Excel: return new OleDbDataAdapter();
                default: throw new NotSupportedException();
            }
        }

        /// <summary>创建参数实例</summary>
        public static IDataParameter CreateParameter(DatabaseType type)
        {
            switch (type)
            {
                case DatabaseType.MSSQL: return new SqlParameter();
                case DatabaseType.Access: return new OleDbParameter();
                case DatabaseType.Excel: return new OleDbParameter();
                default: throw new NotSupportedException();
            }
        }

        /// <summary>创建参数实例</summary>
        public static IDataParameter CreateParameter(DatabaseType type, string parmName, object value)
        {
            if (F.IsNullOrBlank(parmName))
                throw new ArgumentNullException("parmName");

            if (value == null)
                value = DBNull.Value;

            IDataParameter result = CreateParameter(type);
            result.ParameterName = parmName;
            result.Value = value;
            return result;
        }

        /// <summary>创建新的Database实例</summary>
        public static Database CreateDatabase(DatabaseType type, string cnn)
        {
            switch (type)
            {
                case DatabaseType.MSSQL: return new MSSQL(cnn);
                case DatabaseType.Access: return new Access(cnn);
                case DatabaseType.Excel: return new Excel(cnn, true);
                default: throw new NotSupportedException();
            }
        }

        #endregion

        #region Transaction

        /// <summary>事务</summary>
        public IDbTransaction Transaction { get; private set; }

        /// <summary>标记现在开始进行事务处理,在结束事务前所有的操作都会在事务内进行</summary>
        public void BeginTransaction()
        {
            if (this.Transaction != null)
                throw new Exception("当前数据库不支持并行事务");

            OpenConnection();
            IDbTransaction result = this.Connection.BeginTransaction();
            this.Transaction = result;
        }

        /// <summary>回滚事务</summary>
        public void RollbackTransaction()
        {
            if (this.Transaction == null)
                throw new Exception("还没有开始事务");

            this.Transaction.Rollback();
            this.Transaction = null;

            CloseConnection();
        }

        /// <summary>提交事务</summary>
        public void CommitTransaction()
        {
            if (this.Transaction == null)
                throw new Exception("还没有开始事务");

            this.Transaction.Commit();
            this.Transaction = null;

            CloseConnection();
        }

        #endregion

        #region ExecuteNonQuery

        /// <summary>执行 SQL 语句并返回受影响的行数</summary>
        public virtual int ExecuteNonQuery(string sql)
        {
            if (F.IsNullOrBlank(sql))
                throw new ArgumentNullException("sql");

            IDbCommand cmd = CreateCommand(this.DatabaseType);
            cmd.CommandText = sql;
            return ExecuteNonQuery(cmd);
        }
        /// <summary>执行 SQL 语句并返回受影响的行数</summary>
        public virtual int ExecuteNonQuery(IDbCommand cmd)
        {
            return ExecuteNonQuery(cmd, null);
        }
        /// <summary>执行 SQL 语句并返回受影响的行数</summary>
        public virtual int ExecuteNonQuery(IDbCommand cmd, params IDbDataParameter[] parms)
        {
            lock (this)
            {
                if (cmd == null)
                    throw new ArgumentNullException("cmd");

                if (parms != null && parms.Length != 0)
                    foreach (IDbDataParameter param in parms)
                        cmd.Parameters.Add(param);

                OpenConnection();

                try
                {
                    cmd.Connection = this.Connection;
                    cmd.Transaction = this.Transaction;
                    return cmd.ExecuteNonQuery();
                }
                finally
                {
                    CloseConnection();
                }
            }
        }

        #endregion

        #region ExecuteScalar

        /// <summary>执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行。</summary>
        public virtual object ExecuteScalar(string sql)
        {
            if (F.IsNullOrBlank(sql))
                throw new ArgumentNullException("sql");

            IDbCommand cmd = CreateCommand(this.DatabaseType);
            cmd.CommandText = sql;
            return ExecuteScalar(cmd);
        }
        /// <summary>执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行。</summary>
        public virtual object ExecuteScalar(IDbCommand cmd)
        {
            return ExecuteScalar(cmd, null);
        }
        /// <summary>执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行。</summary>
        public virtual object ExecuteScalar(IDbCommand cmd, params IDbDataParameter[] parms)
        {
            lock (this)
            {
                if (cmd == null)
                    throw new ArgumentNullException("cmd");

                if (parms != null && parms.Length != 0)
                    foreach (IDbDataParameter param in parms)
                        cmd.Parameters.Add(param);

                OpenConnection();

                try
                {
                    cmd.Connection = this.Connection;
                    cmd.Transaction = this.Transaction;
                    return cmd.ExecuteScalar();
                }
                finally
                {
                    CloseConnection();
                }
            }
        }

        #endregion

        #region ExecuteReader

        /// <summary>生成一个 OleDbDataReader</summary>
        public virtual IDataReader ExecuteReader(string sql)
        {
            if (F.IsNullOrBlank(sql))
                throw new ArgumentNullException("sql");

            IDbCommand cmd = CreateCommand(this.DatabaseType);
            cmd.CommandText = sql;
            return ExecuteReader(cmd);
        }
        /// <summary>生成一个 OleDbDataReader</summary>
        public virtual IDataReader ExecuteReader(IDbCommand cmd)
        {
            return ExecuteReader(cmd, null);
        }
        /// <summary>生成一个 OleDbDataReader</summary>
        public virtual IDataReader ExecuteReader(IDbCommand cmd, params IDbDataParameter[] parms)
        {
            lock (this)
            {
                if (cmd == null)
                    throw new ArgumentNullException("cmd");

                if (parms != null && parms.Length != 0)
                    foreach (IDbDataParameter param in parms)
                        cmd.Parameters.Add(param);

                OpenConnection();

                cmd.Connection = this.Connection;
                cmd.Transaction = this.Transaction;

                return cmd.ExecuteReader();
            }
        }

        #endregion

        #region GetDataSet

        /// <summary>执行查询，并返回查询所返回的结果集</summary>
        public virtual DataSet GetDataSet(string sql)
        {
            if (F.IsNullOrBlank(sql))
                throw new ArgumentNullException("sql");

            IDbCommand cmd = CreateCommand(this.DatabaseType);
            cmd.CommandText = sql;
            return GetDataSet(cmd);
        }
        /// <summary>执行查询，并返回查询所返回的结果集</summary>
        public virtual DataSet GetDataSet(IDbCommand cmd)
        {
            return GetDataSet(cmd, null);
        }
        /// <summary>执行查询，并返回查询所返回的结果集</summary>
        public virtual DataSet GetDataSet(IDbCommand cmd, params IDbDataParameter[] parms)
        {
            lock (this)
            {
                if (cmd == null)
                    throw new ArgumentNullException("cmd");

                if (parms != null && parms.Length != 0)
                    foreach (IDbDataParameter param in parms)
                        cmd.Parameters.Add(param);

                OpenConnection();

                try
                {
                    cmd.Connection = this.Connection;
                    cmd.Transaction = this.Transaction;
                    IDbDataAdapter adapter = CreateAdapter(this.DatabaseType);
                    adapter.SelectCommand = cmd;
                    DataSet result = new DataSet();
                    adapter.Fill(result);
                    return result;
                }
                finally
                {
                    CloseConnection();
                }
            }
        }

        #endregion

        #region GetDataTable

        /// <summary>执行查询，并返回查询所返回的表</summary>
        public virtual DataTable GetDataTable(string sql)
        {
            if (F.IsNullOrBlank(sql))
                throw new ArgumentNullException("sql");

            IDbCommand cmd = CreateCommand(this.DatabaseType);
            cmd.CommandText = sql;
            return GetDataTable(cmd);
        }
        /// <summary>执行查询，并返回查询所返回的表</summary>
        public virtual DataTable GetDataTable(IDbCommand cmd)
        {
            return GetDataTable(cmd, null);
        }
        /// <summary>执行查询，并返回查询所返回的表</summary>
        public virtual DataTable GetDataTable(IDbCommand cmd, params IDbDataParameter[] parms)
        {
            lock (this)
            {
                if (cmd == null)
                    throw new ArgumentNullException("cmd");

                if (parms != null && parms.Length != 0)
                    foreach (IDbDataParameter param in parms)
                        cmd.Parameters.Add(param);

                OpenConnection();

                try
                {
                    cmd.Connection = this.Connection;
                    cmd.Transaction = this.Transaction;
                    IDbDataAdapter adapter = CreateAdapter(this.DatabaseType);
                    adapter.SelectCommand = cmd;
                    DataSet result = new DataSet();
                    adapter.Fill(result);

                    return result.Tables[0];
                }
                finally
                {
                    CloseConnection();
                }
            }
        }

        #endregion

        #region ExecuteTransaction

        /// <summary>执行数据库事务操作</summary>
        public virtual int[] ExecuteTransaction(IEnumerable<string> sqls)
        {
            IDbCommand[] cmds = new IDbCommand[F.Count(sqls)];

            for (int i = 0; i < cmds.Length; i++)
            {
                IDbCommand cmd = CreateCommand(this.DatabaseType);
                cmd.CommandText = F.ElementAt(sqls, i);
                cmds[i] = cmd;
            }

            return ExecuteTransaction(cmds);
        }
        /// <summary>执行数据库事务操作</summary>
        public virtual int[] ExecuteTransaction(IEnumerable<IDbCommand> cmds)
        {
            lock (this)
            {
                if (cmds == null || !F.Any(cmds))
                    throw new ArgumentNullException("cmds");

                OpenConnection();

                IDbTransaction tran;// = this.Connection.BeginTransaction();

                if (this.Transaction != null)
                    tran = this.Transaction;
                else
                    tran = this.Connection.BeginTransaction();

                int[] result = new int[F.Count(cmds)];

                try
                {
                    for (int i = 0; i < result.Length; i++)
                    {
                        IDbCommand c = F.ElementAt(cmds, i);
                        c.Connection = this.Connection;
                        c.Transaction = tran;
                        result[i] = c.ExecuteNonQuery();
                    }

                    if (tran != this.Transaction)
                        tran.Commit();

                    return result;
                }
                catch
                {
                    try { tran.Rollback(); }
                    catch { }
                    throw;
                }
                finally
                {
                    CloseConnection();
                }
            }
        }

        #endregion

        #region GetColumnValues

        /// <summary>执行一条语句返回结果的第一列所有的值集合,忽略其实的列</summary>
        public virtual IEnumerable<T> GetColumnValues<T>(string sql)
        {
            if (F.IsNullOrBlank(sql))
                throw new ArgumentNullException("sql");

            IDbCommand cmd = CreateCommand(this.DatabaseType);
            cmd.CommandText = sql;
            return GetColumnValues<T>(cmd);
        }
        /// <summary>执行一条语句返回结果的第一列所有的值集合,忽略其实的列</summary>
        public virtual IEnumerable<T> GetColumnValues<T>(IDbCommand cmd)
        {
            return GetColumnValues<T>(cmd, null);
        }
        /// <summary>执行一条语句返回结果的第一列所有的值集合,忽略其实的列</summary>
        public virtual IEnumerable<T> GetColumnValues<T>(IDbCommand cmd, params IDbDataParameter[] parms)
        {
            DataTable dt = this.GetDataTable(cmd, parms);

            foreach (DataRow row in dt.Rows)
                yield return (T)row[0];
        }

        #endregion
    }
}
