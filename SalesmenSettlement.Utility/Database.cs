using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SalesmenSettlement.Utility
{
    public class Database
    {
        public Database(string cnnString)
        {
            ConnectionString = cnnString;
            Connection = new SqlConnection(ConnectionString);
        }

        public string ConnectionString { get; set; }


        #region Connection

        /// <summary>与数据库的连接Connection实例</summary>
        public IDbConnection Connection { get; set; }

        /// <summary>保持连接处于打开状态</summary>
        public bool KeepConnectionOpen { get; set; }

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
            if (sql.IsNullOrWhiteSpace())
                throw new ArgumentNullException("sql");

            IDbCommand cmd = new SqlCommand();
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
            if (sql.IsNullOrWhiteSpace())
                throw new ArgumentNullException("sql");

            IDbCommand cmd = new SqlCommand();
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
            if (sql.IsNullOrWhiteSpace())
                throw new ArgumentNullException("sql");

            IDbCommand cmd = new SqlCommand();
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
            if (sql.IsNullOrWhiteSpace())
                throw new ArgumentNullException("sql");

            IDbCommand cmd = new SqlCommand();
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
                    IDbDataAdapter adapter = new SqlDataAdapter();
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
            if (sql.IsNullOrWhiteSpace())
                throw new ArgumentNullException("sql");

            IDbCommand cmd = new SqlCommand();
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
                    IDbDataAdapter adapter = new SqlDataAdapter();
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
            IDbCommand[] cmds = new IDbCommand[sqls.Count()];

            for (int i = 0; i < cmds.Length; i++)
            {
                IDbCommand cmd = new SqlCommand();
                cmd.CommandText = sqls.ElementAt(i);
                cmds[i] = cmd;
            }

            return ExecuteTransaction(cmds);
        }
        /// <summary>执行数据库事务操作</summary>
        public virtual int[] ExecuteTransaction(IEnumerable<IDbCommand> cmds)
        {
            lock (this)
            {
                if (cmds == null || cmds.Any())
                    throw new ArgumentNullException("cmds");

                OpenConnection();

                IDbTransaction tran;// = this.Connection.BeginTransaction();

                if (this.Transaction != null)
                    tran = this.Transaction;
                else
                    tran = this.Connection.BeginTransaction();

                int[] result = new int[cmds.Count()];

                try
                {
                    for (int i = 0; i < result.Length; i++)
                    {
                        IDbCommand c = cmds.ElementAt(i);
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
            if (sql.IsNullOrWhiteSpace())
                throw new ArgumentNullException("sql");

            IDbCommand cmd = new SqlCommand();
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
