using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections;

namespace ZOC.IO
{
    /// <summary>Sql数据库操作</summary>
    public static class SqlOperations
    {
        /// <summary>通过语句读取一个值</summary>
        public static object GetValue(string sql, string cnnString)
        {
            return GetValue(sql, cnnString, null);
        }
        /// <summary>通过语句读取一个值</summary>
        public static object GetValue(string sql, string cnnString, params SqlParameter[] parms)
        {
            if (string.IsNullOrEmpty(sql))
                throw new ArgumentNullException("sql");

            if (string.IsNullOrEmpty(cnnString))
                throw new ArgumentNullException("cnnString");

            SqlConnection cnn = new SqlConnection(cnnString);
            SqlCommand cmd = new SqlCommand(sql, cnn);

            if (parms != null)
                cmd.Parameters.AddRange(parms);

            cnn.Open();

            try
            {
                return cmd.ExecuteScalar();
            }
            finally
            {
                cnn.Close();
                cnn.Dispose();
                cmd.Dispose();
            }
        }

        /// <summary>执行一条语句,返回影响行数</summary>
        public static int ExcuteQuery(string sql, string cnnString)
        {
            return ExcuteQuery(sql, cnnString, null);
        }
        /// <summary>执行一条语句,返回影响行数</summary>
        public static int ExcuteQuery(string sql, string cnnString, params SqlParameter[] parms)
        {
            if (string.IsNullOrEmpty(sql))
                throw new ArgumentNullException("sql");

            if (string.IsNullOrEmpty(cnnString))
                throw new ArgumentNullException("cnnString");

            SqlConnection cnn = new SqlConnection(cnnString);
            SqlCommand cmd = new SqlCommand(sql, cnn);

            if (parms != null)
                cmd.Parameters.AddRange(parms);

            cnn.Open();

            try
            {
                return cmd.ExecuteNonQuery();
            }
            finally
            {
                cnn.Close();
                cnn.Dispose();
                cmd.Dispose();
            }
        }

        /// <summary>通过语句返回一个数据集</summary>
        public static DataSet GetDataSet(string sql, string cnnString)
        {
            return GetDataSet(sql, cnnString, null);
        }
        /// <summary>通过语句返回一个数据集</summary>
        public static DataSet GetDataSet(string sql, string cnnString, params SqlParameter[] parms)
        {
            if (string.IsNullOrEmpty(sql))
                throw new ArgumentNullException("sql");

            if (string.IsNullOrEmpty(cnnString))
                throw new ArgumentNullException("cnnString");

            SqlConnection cnn = new SqlConnection(cnnString);
            SqlCommand cmd = new SqlCommand(sql, cnn);

            if (parms != null)
                cmd.Parameters.AddRange(parms);

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            cnn.Open();

            try
            {
                DataSet result = new DataSet();
                adapter.Fill(result);
                return result;
            }
            finally
            {
                cnn.Close();
                cnn.Dispose();
                adapter.Dispose();
            }
        }

        /// <summary>执行数据库事务</summary>
        public static ArrayList Transaction(IEnumerable<string> sqlCollction, string cnnString)
        {
            if (!sqlCollction.Any())
                return new ArrayList();

            if (string.IsNullOrEmpty(cnnString))
                throw new ArgumentNullException("cnnString");

            SqlConnection cnn = new SqlConnection(cnnString);
            cnn.Open();
            SqlTransaction tran = cnn.BeginTransaction();

            ArrayList result = new ArrayList();

            try
            {
                foreach (string sql in sqlCollction)
                {
                    SqlCommand cmd = new SqlCommand(sql, cnn, tran);
                    result.Add(cmd.ExecuteScalar());
                }

                tran.Commit();
                return result;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
            finally
            {
                cnn.Close();
                tran.Dispose();
                cnn.Dispose();
            }
        }
        /// <summary>执行数据库事务</summary>
        public static ArrayList Transaction(IEnumerable<SqlCommand> cmdCollection, string cnnString)
        {
            if (!cmdCollection.Any())
                return new ArrayList();

            if (string.IsNullOrEmpty(cnnString))
                throw new ArgumentNullException("cnnString");

            //foreach (SqlCommand cmd in cmdCollection)
            //    if (string.IsNullOrEmpty(cmd.CommandText))
            //        throw new Exception("部分SqlCommand对象的执行命令为空");

            SqlConnection cnn = new SqlConnection(cnnString);
            cnn.Open();
            SqlTransaction tran = cnn.BeginTransaction();

            try
            {
                ArrayList result = new ArrayList();

                foreach (SqlCommand cmd in cmdCollection)
                {
                    cmd.Connection = cnn;
                    cmd.Transaction = tran;
                    result.Add(cmd.ExecuteScalar());
                }

                tran.Commit();
                return result;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
            finally
            {
                cnn.Close();
                tran.Dispose();
                cnn.Dispose();
            }
        }

        /// <summary>通过语句返回一个DataReader</summary>
        public static SqlDataReader GetReader(string sql, string cnnString)
        {
            return GetReader(sql, cnnString, null);
        }
        /// <summary>通过语句返回一个DataReader</summary>
        public static SqlDataReader GetReader(string sql, string cnnString, params SqlParameter[] parms)
        {
            if (string.IsNullOrEmpty(sql))
                throw new ArgumentNullException("sql");

            if (string.IsNullOrEmpty(cnnString))
                throw new ArgumentNullException("cnnString");

            SqlConnection cnn = new SqlConnection(cnnString);
            SqlCommand cmd = new SqlCommand(sql, cnn);

            if (parms != null)
                cmd.Parameters.AddRange(parms);

            cnn.Open();

            SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return reader;
        }
    }
}
